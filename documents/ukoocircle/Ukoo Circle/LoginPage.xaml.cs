using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.Text;
using System.Threading;
using Windows.Data.Xml.Dom;
using System.Xml;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections;
using System.Linq.Expressions;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Ukoo_Circle
{
    public partial class LoginPage : PhoneApplicationPage
    {

        private static ManualResetEvent allDone = new ManualResetEvent(false);

        private string xmlString = "<message><msg>login</msg><user username='value' password='value'/></message>";
        private string username;
        private string password;
       

        /*
        // Data context for the local database
        private ThisRoot thisDB;

        // Define an observable collection property that controls can bind to.
        private ObservableCollection<Roots> _roots;
        public ObservableCollection<Roots> Root
        {
            get
            {
                return _roots;
            }
            set
            {
                _roots = value;
            }
        }
        */

        //constructor
        public LoginPage()
        {
            InitializeComponent();

            
            progressbar.Visibility = System.Windows.Visibility.Collapsed;

            using (UkooDataContext context = new UkooDataContext(UkooDataContext.DBConnectionString))
            {
                if (!context.DatabaseExists())
                    context.CreateDatabase();
            }

            // Connect to the database and instantiate data context.
            //thisDB = new ThisRoot(ThisRoot.DBConnectionString);

            // Data context and observable collection are children of the main page.
            this.DataContext = this;

            
        }

        /*
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Define the query to gather all of the items.
            var items = from Roots todo in thisDB.Root
                        select todo;

            // Execute the query and place the results into a collection.
            Root = new ObservableCollection<Roots>(items);

            base.OnNavigatedTo(e);
        }

        private void savePerson()
        {

        }
        */

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            progressbar.Visibility = System.Windows.Visibility.Visible;

            if ((string.IsNullOrWhiteSpace(txtPassword.Password)) && (string.IsNullOrWhiteSpace(txtUsername.Text)))
            {
                progressbar.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show("Fill in the appropriate credentials");
            }
            else
            {
                username = txtUsername.Text;
                password = txtPassword.Password;

                xmlString = "<message><msg>login</msg><user username='" + username + "' password='" + password + "'/></message>";
                Debug.WriteLine(xmlString);

                SentPostReport();
            }      
        }

        private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/CreateAccountPage.xaml", UriKind.Relative));
        } 

        private void ParseResults(string results)
        {
            if (string.IsNullOrEmpty(results))
            {
                progressbar.Visibility = System.Windows.Visibility.Collapsed;
                Debug.WriteLine("No response");
                MessageBox.Show("Login Attempt was unsuccessful");
                return;
            }
            else if (results == "error")
            {
                //save username + password in local storage
                /*string name = txtUsername.Text;
                string pass = txtPassword.Text;
                DatabaseAdd add = new DatabaseAdd();
                add.AddUser(name, pass);*/

                /*
                DatabaseAdd add = new DatabaseAdd();
                add.AddUser(txtUsername.Text, txtPassword.Password); //erroneous...can still be used as log
                */
                progressbar.Visibility = System.Windows.Visibility.Collapsed;
                Debug.WriteLine("Response is:" + results);
                MessageBox.Show("There was an error...Check your credentials and try again");
                //NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
            }
            else
            {
                //save username + password in local storage
                DatabaseAdd add = new DatabaseAdd();
                add.AddUser(txtUsername.Text, txtPassword.Password);

                progressbar.Visibility = System.Windows.Visibility.Collapsed;
                Debug.WriteLine("Response is:" + results);
                MessageBox.Show("Login Attempt was a " + results);
                NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
            }
        }


        public void SentPostReport()
        {
            string url = "http://cybertexdigitalcommunication.co.ke/ukoo_circle/core/engine.php";
            Uri uri = new Uri(url);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.ContentType = "application/x-www-form-urlencoded";

            // Set the Method property to 'POST' to post data to the URI.
            request.Method = "POST";
            request.UseDefaultCredentials = true;
            request.Accept = "multipart/form-data";

            //request.Accept = "multipart/form-data;";
            request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";
            request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);
            //allDone.WaitOne(1000);
        }


        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            Stream memStream = request.EndGetRequestStream(asynchronousResult);

            Debug.WriteLine("Sending request");
            //string xmlString = "<message><msg>login</msg><user username='1' password='1'/></message>";
            string postData = "data=" + xmlString;

            byte[] contentbytes = System.Text.Encoding.UTF8.GetBytes(postData);

            // send the content of the file.
            memStream.Write(contentbytes, 0, contentbytes.Length);

            
            memStream.Flush();
            memStream.Close();

            
            // Start the asynchronous operation to get the response
            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);

                string responseString = streamRead.ReadToEnd(); // this is a response server.
                Debug.WriteLine(responseString);

                Dispatcher.BeginInvoke(() => ParseResults(responseString));

                // Close the stream object
                streamResponse.Close();
                streamRead.Close();

                // Release the HttpWebResponse
                response.Close();
            }
            catch (Exception ex)
            {
                // error.
                Debug.WriteLine(ex);
            }

            allDone.Set();
        }

        /*
        //TABLE
        public class Roots
        {
            //ID
            private int _rootsId;

            [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
            public int RootsId
            {
                get
                {
                    return _rootsId;
                }
                set
                {
                    if (_rootsId != value)
                    {
                        _rootsId = value;
                    }
                }
            }

            //FIRST NAME
            private string _firstName;

            //COLUMN
            public string FirstName
            {
                get
                {
                    return _firstName;
                }
                set
                {
                    _firstName = value;
                }
            }

            //SURNAME
            private string _surName;

            //COLUMN
            public string SurName
            {
                get
                {
                    return _surName;
                }
                set
                {
                    _surName = value;
                }
            }
        }

        public class ThisRoot : DataContext
        {
            // Specify the connection string as a static, used in main page and app.xaml --can also work when not static
            public static string DBConnectionString = "Data Source=isostore:/ToDo.sdf";

            // Pass the connection string to the base class.
            public ThisRoot(string connectionString)
                : base(connectionString)
            { }

            // Specify a single table for the items.
            public Table<Roots> Root;
        }
        */

    }
}