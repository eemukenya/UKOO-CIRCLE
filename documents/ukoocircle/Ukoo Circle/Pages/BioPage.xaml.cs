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

namespace Ukoo_Circle.Pages
{
    class Family
    {
        public string ref_id { get; set; }
        public string name { get; set; }
        public string family { get; set; }
    }
    public partial class BioPage : PhoneApplicationPage
    {
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private string xmlString = "<message>" +
                                   "<msg>get_person</msg>" +
                                   "<person " +
                                   "search_item = 'value'/>" +
                                   "</message>";
        private string number;
        private int sent = 0;
        private string readerName;
        public BioPage()
        {
            InitializeComponent();

            readerName = "person";
            number = "1";

            xmlString = "<message>" +
                        "<msg>get_person</msg>" +
                        "<person " +
                        "search_item = '" + number + "'/>" +
                        "</message>";

            SentPostReport();
        }

        private void ParseResults(string results)
        {
            if (string.IsNullOrEmpty(results))
            {
                progressbar.Visibility = System.Windows.Visibility.Collapsed;
                Debug.WriteLine("No response");
                MessageBox.Show("Fetching children was unsuccessful...Try again");
                return;
            }
            else
            {
                Debug.WriteLine("Response is:" + results);
                //MessageBox.Show("Login Attempt was " + results);

                using (XmlReader reader = XmlReader.Create(new StringReader(results)))
                {
                    List<Family> allrels = new List<Family>();
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                            if (reader.Name == readerName)
                            {

                                progressbar.Visibility = System.Windows.Visibility.Collapsed;

                                if (sent == 0)
                                {
                                    String person_date_of_birth = reader["person_date_of_birth"];
                                    //Debug.WriteLine("Relative's Referee Id: " + person_date_of_birth);
                                    txDateOfBirth.Text = person_date_of_birth;

                                    String person_blood_type = reader["person_blood_type"];
                                    txBloodType.Text = person_blood_type;

                                    String person_eye_color = reader["person_eye_color"];
                                    txEyeColor.Text = person_eye_color;

                                    String person_height = reader["person_height"];
                                    txHeight.Text = person_height;

                                    String person_weight = reader["person_weight"];
                                    txWeight.Text = person_weight;

                                    String person_gender = reader["person_gender"];
                                    txGender.Text = person_gender;

                                    String person_marital_status = reader["person_marital_status"];
                                    txMaritalStatus.Text = person_marital_status;

                                    String person_phone_number = reader["person_phone_number"];
                                    txPhoneNumber.Text = person_phone_number;

                                    String person_email_address = reader["person_email_address"];
                                    txEmailAddress.Text = person_email_address;

                                    String person_hometown = reader["person_hometown"];
                                    txHometown.Text = person_hometown;

                                    String person_current_residence = reader["person_current_residence"];
                                    txCurrentResidence.Text = person_current_residence;

                                    String person_occupation = reader["person_occupation"];
                                    txOccupation.Text = person_occupation;

                                    String person_nationality = reader["person_nationality"];
                                    txNationality.Text = person_nationality;
                                }
                                

                                if (sent == 1) //round 2
                                {
                                    String relative_firstname = reader["relative_firstname"];

                                    String relative_middlename = reader["relative_middlename"];

                                    String relative_surname = reader["relative_surname"];

                                    String relative_fathers_name = reader["relative_fathers_name"];

                                    Family n = new Family();
                                    n.name = relative_firstname + " " + relative_middlename + " " + relative_surname;
                                    n.family = relative_fathers_name + "'s family";
                                    allrels.Add(n);
                                }
                            }

                    }
                    if (sent == 1)
                    {
                        allusers.ItemsSource = allrels;
                        sent = 2;
                        return;
                    }
                    
                }
                
            }
            readerName = "relative";
            xmlString = "<message>" +
                        "<msg>get_specific_relative</msg>" +
                        "<relative " +
                        "relative_referee_id = '" + number + "'/>" +
                        "</message>";

            sent = 1;
            SentPostReport();
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

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));

            base.OnBackKeyPress(e);
        }
    }
}