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

namespace Ukoo_Circle.Pages.UserDetails
{
    public partial class EditChildPage : PhoneApplicationPage
    {
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private string xmlString = "<message>" +
                                   "<msg>get_person</msg>" +
                                   "<person " +
                                   "search_item = 'value'/>" +
                                   "</message>";
        private int sent = 0;
        private int childIndex = 0;
        public EditChildPage()
        {
            InitializeComponent();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string ref_id = "";
            string index = "";

            if (NavigationContext.QueryString.TryGetValue("data", out ref_id))
            {
                if (NavigationContext.QueryString.TryGetValue("index", out index))
                {
                    childIndex = int.Parse(index);
                    //MessageBox.Show("step 2 + index is " + index);
                    //use string
                    xmlString = "<message>" +
                            "<msg>get_specific_relative</msg>" +
                            "<relative " +
                            "relative_referee_id = '" + ref_id + "'/>" +
                            "</message>";

                    //MessageBox.Show(xmlString);
                    SentPostReport();
                }
                
            }
                
        }

        private void ParseResults(string results)
        {
            if (string.IsNullOrEmpty(results))
            {
                progressbar.Visibility = System.Windows.Visibility.Collapsed;
                Debug.WriteLine("No response");
                MessageBox.Show("Error updating profile details. Please try again");
                return;
            }
            else
            {
                progressbar.Visibility = System.Windows.Visibility.Collapsed;
                Debug.WriteLine("Response is:" + results);
                //MessageBox.Show("Login Attempt was " + results);
                if (sent == 1) //update done...close
                {
                    MessageBox.Show("Profile details updated successfully");
                    NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
                }
                else
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(results)))
                    {

                        while (reader.Read())
                        {
                            //move reader by the number of indexes thru nodes
                            for (int i = 0; i < childIndex; i++)
                            {
                                reader.Read();
                            }
                            if (reader.NodeType == XmlNodeType.Element)
                                if (reader.Name == "relative")
                                {
                                    /*if (true)
                                    {
                                        reader.GetAttribute(4);
                                        MessageBox.Show(reader.GetAttribute(4).ToString());
                                    }*/ //not useful...jumps to specific attribute
                                    progressbar.Visibility = System.Windows.Visibility.Collapsed;

                                    String person_first_name = reader["relative_firstname"];
                                    Debug.WriteLine("Person's First Name: " + person_first_name);
                                    txFirstName.Text = person_first_name;

                                    String person_surname = reader["relative_surname"];
                                    Debug.WriteLine("Person's Surname: " + person_surname);
                                    txSurname.Text = person_surname;

                                    String person_middle_name = reader["relative_middlename"];
                                    Debug.WriteLine("Person's Middle Name: " + person_middle_name);
                                    txMiddleName.Text = person_middle_name;

                                    String person_relative_referee_id = reader["relative_referee_id"];
                                    Debug.WriteLine("Person's Date of Birth: " + person_relative_referee_id);
                                    txRefID.Text = person_relative_referee_id;

                                    String person_fathers_name = reader["relative_fathers_name"];
                                    Debug.WriteLine("Person's Eye Colour: " + person_fathers_name);
                                    txFathersName.Text = person_fathers_name;

                                    String person_mothers_name = reader["relative_mothers_name"];
                                    Debug.WriteLine("Person's Blood Type: " + person_mothers_name);
                                    txMothersName.Text = person_mothers_name;

                                    String person_spouse_name = reader["relative_spouse_name"];
                                    Debug.WriteLine("Person's Height: " + person_spouse_name);
                                    txSpouseName.Text = person_spouse_name;

                                    String person_is_living = reader["relative_isliving"];
                                    Debug.WriteLine("Person's Weight: " + person_is_living);
                                    txLiving.Text = person_is_living;

                                    String person_phone_number = reader["relative_phone_number"];
                                    Debug.WriteLine("Person's Phone Number: " + person_phone_number);
                                    txPhoneNo.Text = person_phone_number;
                                    
                                    String person_occupation = reader["relative_occupation"];
                                    Debug.WriteLine("Person's Occupation: " + person_occupation);
                                    txOccupation.Text = person_occupation;
                                    
                                    String person_gender = reader["relative_gender"];
                                    Debug.WriteLine("Person's Gender: " + person_gender);
                                    txGender.Text = person_gender;
                                    
                                    String person_marital_status = reader["relative_marital_status"];
                                    Debug.WriteLine("Person's Marital Status: " + person_marital_status);
                                    txMaritalStatus.Text = person_marital_status;

                                    break;
                                }
                        }
                    }
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

        private void btnAddChild_Click(object sender, RoutedEventArgs e)
        {
            progressbar.Visibility = System.Windows.Visibility.Visible;
            //MessageBox.Show("goone");
            if ((string.IsNullOrWhiteSpace(txPhoneNo.Text)) && (string.IsNullOrWhiteSpace(txRefID.Text)) && (string.IsNullOrWhiteSpace(txFirstName.Text)) && (string.IsNullOrWhiteSpace(txMiddleName.Text)) && (string.IsNullOrWhiteSpace(txSurname.Text)) && (string.IsNullOrWhiteSpace(txFathersName.Text)) && (string.IsNullOrWhiteSpace(txMothersName.Text)) && (string.IsNullOrWhiteSpace(txGender.Text)) && (string.IsNullOrWhiteSpace(txMaritalStatus.Text)) && (string.IsNullOrWhiteSpace(txSpouseName.Text)) && (string.IsNullOrWhiteSpace(txLiving.Text)) && (string.IsNullOrWhiteSpace(txOccupation.Text)))
            {
                progressbar.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show("Fill in the appropriate credentials");
            }
            else
            {
                string relative_relation_type = "4";
                string phone_number = txPhoneNo.Text;
                string ref_id = txRefID.Text;
                string first_name = txFirstName.Text;
                string middle_name = txMiddleName.Text;
                string sur_name = txSurname.Text;
                string father_name = txFathersName.Text;
                string mother_name = txMothersName.Text;
                string gender = txGender.Text;
                string marital_status = txMaritalStatus.Text;
                string spouse_name = txSpouseName.Text;
                string living = txLiving.Text;
                string occupation = txOccupation.Text;

                xmlString = "<message>" +
                             "<msg>edit_relative</msg>" +
                             "<relative " +
                             "relative_relation_type = '" + relative_relation_type + "' " +
                             "relative_phone_number = '" + phone_number + "' " +
                             "relative_referee_id = '" + ref_id + "' " +
                             "relative_firstname = '" + first_name + "' " +
                             "relative_middlename = '" + middle_name + "' " +
                             "relative_surname = '" + sur_name + "' " +
                             "relative_fathers_name = '" + father_name + "' " +
                             "relative_mothers_name = '" + mother_name + "' " +
                             "relative_gender = '" + gender + "' " +
                             "relative_marital_status = '" + marital_status + "' " +
                             "relative_spouse_name = '" + spouse_name + "' " +
                             "relative_isliving = '" + living + "' " +
                             "relative_occupation = '" + occupation + "'/>" +
                             "</message>";

                //MessageBox.Show(xmlString);
                SentPostReport();
            }
        }
    }
}