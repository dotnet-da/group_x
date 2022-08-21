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
using System.Windows.Shapes;
using System.Diagnostics;

namespace frontend
{
    /// <summary>
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        List<Student> studentsCollection;
        public StudentWindow()
        {
            InitializeComponent();
        }

        static async Task<string> AddStudent(string fn, string ln)
        {
            var userName = "admin";
            var passwd = "1234";
            var authData = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
            var response = string.Empty;

           //Student objStudent = new Student(fn, ln);
           Student objStudent = new Student();
            objStudent.Fname = fn;
            objStudent.Lname = ln;

            var json = JsonConvert.SerializeObject(objStudent);
            var postData = new StringContent(json, Encoding.UTF8, "application/json");

            var url = Environment.baseUrl + "student";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authData));
            HttpResponseMessage result = await client.PostAsync(url, postData);
            response = await result.Content.ReadAsStringAsync();
            return response;
        }
        private void btnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            string fname=textFn.Text;
            string lname=textLn.Text;
            var data = Task.Run(() => AddStudent(fname, lname));
            data.Wait();

            if (data.Result.Length > 0)
            {
                string response = data.Result;
                Debug.WriteLine(response);
                if (string.Compare(response, "0") == 0)
                {
                    textInfo.Text = "Something went wrong";
                }
                else
                {
                    textInfo.Text = "User added successfully";
                }
            }
            else
            {
                MessageBox.Show("Something went wrong");
            }
        }

        private void btnShowStudents_Click(object sender, RoutedEventArgs e)
        {
            InitializeStudentCollection();
        }

        private void InitializeStudentCollection()
        {
            var data = Task.Run(() => GetStudentsFromApi());
            data.Wait();
            if (data.Result.Length > 3) //Result is not []
            {
                dynamic students = JsonConvert.DeserializeObject(data.Result);
                
                studentsCollection = new List<Student>();
                foreach (var student in students)
                {
                    studentsCollection.Add(new Student {Id=student.id_student, Fname = student.fname, Lname = student.lname});
                }
                dataGridStudents.ItemsSource = studentsCollection;
            }
            else
            {
                MessageBox.Show("There is no books");
            }
        }

        static async Task<string> GetStudentsFromApi(string id = null)
        {
            var userName = "admin";
            var passwd = "1234";
            var authData = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
            var response = string.Empty;
            var url = Environment.baseUrl + "student";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authData));
            HttpResponseMessage result = await client.GetAsync(url);
            response = await result.Content.ReadAsStringAsync();
            return response;
        }
    }
}
