using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Ukoo_Circle.user_details_views
{
    class Parents
    {
        public string name { get; set; }
        public string family { get; set; }
    }
    public partial class ParentListPage : PhoneApplicationPage
    {
        PersonalInfoClass personalInfo = new PersonalInfoClass();
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private string xmlString = "<message>" +
                                   "<msg>get_person</msg>" +
                                   "<person " +
                                   "search_item = 'value'/>" +
                                   "</message>";
        private string number;
        public ParentListPage()
        {
            InitializeComponent();

            FetchDatabase fetch = new FetchDatabase();
            //number = fetch.getNumber();
            number = "3"; //???

            xmlString = "<message>" +
                        "<msg>get_specific_relative</msg>" +
                        "<relative " +
                        "relative_referee_id = '" + number + "'/>" +
                        "</message>";

            SentPostReport();
        }

        private void ParseResults(string results)
        {
            if (string.IsNullOrEmpty(results))
            {
                progressbar.Visibility = System.Windows.Visibility.Collapsed;
                Debug.WriteLine("No response");
                MessageBox.Show("Fetching parents was unsuccessful...Try again");
                return;
            }
            else
            {
                Debug.WriteLine("Response is:" + results);
                //MessageBox.Show("Login Attempt was " + results);

                using (XmlReader reader = XmlReader.Create(new StringReader(results)))
                {
                    List<Parents> allrels = new List<Parents>();
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                            if (reader.Name == "relative")
                            {
                                //List<string> PersonInfo = new List<string>();
                                progressbar.Visibility = System.Windows.Visibility.Collapsed;

                                var PersonInfo = new Dictionary<string, string>();


                                String relative_firstname = reader["relative_firstname"];
                                personalInfo.setPersonFirstName(reader["relative_firstname"]);
                                PersonInfo["person_first_name"] = relative_firstname;
                                new PersonalInfoClass().getPersonFirstName();
                                Debug.WriteLine("Person's First Name: " + relative_firstname);
                                //txFirstName.Text = person_first_name;

                                String relative_middlename = reader["relative_middlename"];
                                personalInfo.setPersonMiddleName(reader["relative_middlename"]);
                                PersonInfo["relative_middlename"] = relative_middlename;
                                new PersonalInfoClass().getPersonMiddleName();
                                Debug.WriteLine("Person's Middle Name: " + relative_middlename);
                                //txFirstName.Text = person_first_name;

                                String relative_surname = reader["relative_surname"];
                                personalInfo.setPersonSurname(reader["relative_surname"]);
                                PersonInfo["relative_surname"] = relative_surname;
                                Debug.WriteLine("Person's Surname: " + relative_surname);
                                //txSurname.Text = person_surname;

                                String relative_fathers_name = reader["relative_fathers_name"];
                                personalInfo.setPersonFather(reader["relative_fathers_name"]);
                                PersonInfo["relative_fathers_name"] = relative_fathers_name;
                                new PersonalInfoClass().getPersonFather();
                                Debug.WriteLine("Person's Father's Name: " + relative_fathers_name);
                                //txFirstName.Text = person_first_name;

                                Parents n = new Parents();
                                n.name = relative_firstname + " " + relative_middlename + " " + relative_surname;
                                n.family = relative_fathers_name + "'s family";
                                allrels.Add(n);

                                //MessageBox.Show(tally.ToString());
                                //reader.MoveToNextAttribute();
                                //break;
                            }

                    }
                    allusers.ItemsSource = allrels;
                }
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
                System.Diagnostics.Debug.WriteLine(ex);
            }

            allDone.Set();
        }

        private void btnSettings_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/UserDetails/EditParentPage.xaml", UriKind.Relative));
        }
    }
}