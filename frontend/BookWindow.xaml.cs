using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace frontend
{
    /// <summary>
    /// Interaction logic for Book.xaml
    /// </summary>
    public partial class BookWindow : Window
    {
        public BookWindow()
        {
            InitializeComponent();
        }

        private void btnGetOneBook_Click(object sender, RoutedEventArgs e)
        {
            string id = textBook_id.Text;
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Give the id of the book");
            }
            else
            {
                var data = Task.Run(() => GetBookFromApi(id));
                data.Wait();
                if (data.Result.Length > 0)
                {
                    JObject j = JObject.Parse(data.Result);
                    Debug.WriteLine(j["name"].ToString());
                    labelName.Content = j["name"].ToString();
                    labelAuthor.Content = j["author"].ToString();
                    labelISBN.Content = j["isbn"].ToString();
                    Debug.WriteLine(data.Result);
                }
                else
                {
                    MessageBox.Show("Book id does not exists");
                }
            }
        }
        static async Task<string> GetBookFromApi(string id = null)
        {
            var authData = Encoding.ASCII.GetBytes(Environment.authString);
            var response = string.Empty;
            var url = Environment.baseUrl+"book/" + id;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authData));
            HttpResponseMessage result = await client.GetAsync(url);
            response = await result.Content.ReadAsStringAsync();
            return response;

        }
        static async Task<string> GetBooksFromApi(string id = null)
        {
            var authData = Encoding.ASCII.GetBytes(Environment.authString);
            var response = string.Empty;
            var url = Environment.baseUrl + "book";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authData));
            HttpResponseMessage result = await client.GetAsync(url);
            response = await result.Content.ReadAsStringAsync();
            return response;

        }

        private void btnGetAllBooks_Click(object sender, RoutedEventArgs e)
        {
            var data = Task.Run(() => GetBooksFromApi());
            data.Wait();
            Console.WriteLine(data.Result);
            if (data.Result.Length > 3) //Result is not []
            {
                dynamic books = JsonConvert.DeserializeObject(data.Result);

                gridBooks.ItemsSource = books;//writes the data to DataGrid

                string book_data = "";
                foreach (var book in books)
                {
                    book_data += book.name + " | " + book.author + " | " + book.isbn + "\n";
                }
                textBooks.Text = book_data;



            }
            else
            {
                MessageBox.Show("There is no books");
            }
        }
    }
}
