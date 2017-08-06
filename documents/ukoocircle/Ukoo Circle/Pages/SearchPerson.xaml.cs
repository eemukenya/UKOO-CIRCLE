using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Threading;

namespace Ukoo_Circle.Pages
{
    class Search
    {
        public string ref_id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
    }
    public partial class SearchPerson : PhoneApplicationPage
    {
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private string xmlString = "<message>" +
                                   "<msg>get_person</msg>" +
                                   "<person " +
                                   "search_item = 'value'/>" +
                                   "</message>";
        
        public SearchPerson()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string search_value = "";
            //string index = "";

            if (NavigationContext.QueryString.TryGetValue("data", out search_value))
            {
                //MessageBox.Show("step 2 + index is " + index);
                //use string
                xmlString = "<message>" +
                        "<msg>get_person_android</msg>" +
                        "<person " +
                        "search_item = '" + search_value + "'/>" +
                        "</message>";

                //MessageBox.Show(xmlString);
                SentPostReport();
                
            }

        }

        private void ParseResults(string results)
        {
            if (string.IsNullOrEmpty(results))
            {
                progressbar.Visibility = System.Windows.Visibility.Collapsed;
                Debug.WriteLine("No response");
                MessageBox.Show("Fetching results was unsuccessful...Try again");
                return;
            }
            else if (results == "no_match")
            {
                progressbar.Visibility = System.Windows.Visibility.Collapsed;
                Debug.WriteLine("No match");
                MessageBox.Show("Search Request got no match...Try again");
                return;
            }
            else
            {
                Debug.WriteLine("Response is:" + results);
                //MessageBox.Show("Login Attempt was " + results);

                using (XmlReader reader = XmlReader.Create(new StringReader(results)))
                {
                    List<Search> allrels = new List<Search>();
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                            if (reader.Name == "person")
                            {
                                //List<string> PersonInfo = new List<string>();

                                var PersonInfo = new Dictionary<string, string>();

                                progressbar.Visibility = System.Windows.Visibility.Collapsed;

                                String relative_referee_id = reader["relative_referee_id"];
                                Debug.WriteLine("Relative's Referee Id: " + relative_referee_id);

                                String person_first_name = reader["person_first_name"];
                                Debug.WriteLine("Person's First Name: " + person_first_name);
                                //txFirstName.Text = person_first_name;

                                String person_middle_name = reader["person_middle_name"];
                                Debug.WriteLine("Person's Middle Name: " + person_middle_name);
                                //txFirstName.Text = person_first_name;

                                String person_surname = reader["person_surname"];
                                Debug.WriteLine("Person's Surname: " + person_surname);
                                //txSurname.Text = person_surname;

                                String person_phone_number = reader["person_phone_number"];
                                Debug.WriteLine("Person's Father's Name: " + person_phone_number);
                                //txFirstName.Text = person_first_name;

                                Search n = new Search();
                                n.ref_id = relative_referee_id;
                                n.name = person_first_name + " " + person_middle_name + " " + person_surname;
                                n.number = person_phone_number;
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
            NavigationService.Navigate(new Uri("/Pages/UserDetails/AddChildPage.xaml", UriKind.Relative));
        }

        private void allusers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Search person = allusers.SelectedItem as Search;

            //MessageBox.Show("Selected " + child.name);
            /*MessageBoxResult edit = MessageBox.Show("Edit " + child.name + "?", "Edit child", MessageBoxButton.OKCancel);

            if (edit == MessageBoxResult.OK)
            {
                //MessageBox.Show("Ready");
                //get relative_referee_id from record...navigate to editing page
                NavigationService.Navigate(new Uri("/Pages/UserDetails/EditChildPage.xaml?data=" + child.ref_id + "&index=" + allusers.SelectedIndex, UriKind.Relative));
            }*/
            NavigationService.Navigate(new Uri("/Pages/PaymentPage.xaml?data=" + person.ref_id + "&name=" + person.name + "&number=" + person.number, UriKind.Relative));
        }
    }
}