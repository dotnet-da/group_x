using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        static async Task<string> LoginHttp(string u, string p)
        {
  
            var response = string.Empty;

            User objectUser = new User(u, p);

            var json = JsonConvert.SerializeObject(objectUser);
            var postData = new StringContent(json, Encoding.UTF8, "application/json");

            var url = Environment.baseUrl + "login";
            var client = new HttpClient();
            var authData = Encoding.ASCII.GetBytes(Environment.authString);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authData));

            HttpResponseMessage result = await client.PostAsync(url, postData);
            response = await result.Content.ReadAsStringAsync();
            return response;
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string u = textUsername.Text;
            string p = textPassword.Text;
            var data = Task.Run(() => LoginHttp(u, p));
            data.Wait();

            if (data.Result.Length > 0)
            {
                string response = data.Result;
                Debug.WriteLine(response);
                if (string.Compare(response, "true") == 0)
                {
                    labelResult.Content = "Login OK";
                    //BookWindow objectBook=new BookWindow();
                    //objectBook.Show();
                    StudentWindow objectStudentWindow=new StudentWindow();
                    objectStudentWindow.Show();
                    this.Close();
                }
                else
                {
                    labelResult.Content = "Wrong username/password";
                }
            }
            else
            {
                MessageBox.Show("Something went wrong");
            }
        }
    }
}
