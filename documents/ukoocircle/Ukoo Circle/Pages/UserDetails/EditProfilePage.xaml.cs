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

namespace Ukoo_Circle
{
    public partial class EditProfilePage : PhoneApplicationPage
    {
        PersonalInfoClass personalInfo = new PersonalInfoClass();
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private string xmlStringToGet = "<message>" +
                                           "<msg>get_person</msg>" +
                                           "<person " +
                                           "search_item = 'value'/>" +
                                           "</message>";
        private string number;
        private int sent = 0;
        public EditProfilePage()
        {
            InitializeComponent();

            FetchDatabase fetch = new FetchDatabase();            
            number = fetch.getNumber();
            //progressbar.Visibility = System.Windows.Visibility.Collapsed;

            xmlStringToGet = "<message>" +
                        "<msg>get_person</msg>" +
                        "<person " +
                        "search_item = '" + number + "'/>" +
                        "</message>";

            SentPostReport();

            string profileUrl = "http://192.168.43.221:8081/ukoo_circle/core/engine.php";
            //myProfile.Navigate(new Uri(profileUrl, UriKind.Absolute));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("annie");/*
            //get form data
            xmlStringToGet = "<message>" +
                             "<msg>edit_person</msg>" +
                             "<person " +
                             "person_first_name ="+ txFirstName.Text +
                             "person_surname ="+ txSurname.Text +
                             "person_middle_name ="+ txMiddleName.Text +
                             "person_date_of_birth ="+ txDateOfBirth.Text +
                             "person_eye_color ="+ txEyeColor.Text +
                             "person_blood_type ="+ txBloodType.Text +
                             "person_height ="+ txHeight.Text +
                             "person_weight ="+ txWeight.Text +
                             "person_email_address ="+ txEmailAddress.Text +
                             "person_hometown ="+ txHometown.Text +
                             "person_current_residence ="+ txCurrentResidence.Text +
                             "person_nationality ="+ txNationality.Text +
                             "person_occupation ="+ txOccupation.Text +
                             "person_gender ="+ txGender.Text +
                             "person_marital_status ="+ txMaritalStatus.Text + "/>" +
                             "</message>";
            //send new xml
            MessageBox.Show(xmlStringToGet);

            SentPostReport();*/
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
                            if (reader.NodeType == XmlNodeType.Element)
                                if (reader.Name == "person")
                                {


                                    progressbar.Visibility = System.Windows.Visibility.Collapsed;
                                    //List<string> PersonInfo = new List<string>();

                                    var PersonInfo = new Dictionary<string, string>();


                                    String person_first_name = reader["person_first_name"];
                                    personalInfo.setPersonFirstName(reader["person_first_name"]);
                                    PersonInfo["person_first_name"] = person_first_name;
                                    new PersonalInfoClass().getPersonFirstName();
                                    Debug.WriteLine("Person's First Name: " + person_first_name);
                                    txFirstName.Text = person_first_name;

                                    personalInfo.setPersonSurname(reader["person_surname"]);
                                    String person_surname = reader["person_surname"];
                                    PersonInfo["person_surname"] = person_surname;
                                    Debug.WriteLine("Person's Surname: " + person_surname);
                                    txSurname.Text = person_surname;

                                    personalInfo.setPersonMiddleName(reader["person_middle_name"]);
                                    String person_middle_name = reader["person_middle_name"];
                                    PersonInfo["person_middle_name"] = person_middle_name;
                                    Debug.WriteLine("Person's Middle Name: " + person_middle_name);
                                    txMiddleName.Text = person_middle_name;

                                    personalInfo.setPersonDateOfBirth(reader["person_date_of_birth"]);
                                    String person_date_of_birth = reader["person_date_of_birth"];
                                    PersonInfo["person_date_of_birth"] = person_date_of_birth;
                                    Debug.WriteLine("Person's Date of Birth: " + person_date_of_birth);
                                    txDateOfBirth.Text = person_date_of_birth;

                                    personalInfo.setPersonEyeColor(reader["person_eye_color"]);
                                    String person_eye_color = reader["person_eye_color"];
                                    PersonInfo["person_date_of_birth"] = person_eye_color;
                                    Debug.WriteLine("Person's Eye Colour: " + person_eye_color);
                                    txEyeColor.Text = person_eye_color;

                                    personalInfo.setPersonBloodType(reader["person_blood_type"]);
                                    String person_blood_type = reader["person_blood_type"];
                                    PersonInfo["person_blood_type"] = person_blood_type;
                                    Debug.WriteLine("Person's Blood Type: " + person_blood_type);
                                    txBloodType.Text = person_blood_type;

                                    personalInfo.setPersonHeight(reader["person_height"]);
                                    String person_height = reader["person_height"];
                                    PersonInfo["person_height"] = person_height;
                                    Debug.WriteLine("Person's Height: " + person_height);
                                    txHeight.Text = person_height;

                                    personalInfo.setPersonWeight(reader["person_weight"]);
                                    String person_weight = reader["person_weight"];
                                    PersonInfo["person_weight"] = person_weight;
                                    Debug.WriteLine("Person's Weight: " + person_weight);
                                    txWeight.Text = person_weight;

                                    personalInfo.setPersonPhoneNumber(reader["person_phone_number"]);
                                    String person_phone_number = reader["person_phone_number"];
                                    PersonInfo["person_phone_number"] = person_phone_number;
                                    Debug.WriteLine("Person's Phone Number: " + person_phone_number);
                                    txPhoneNumber.Text = person_phone_number;

                                    personalInfo.setPersonEmailAddress(reader["person_email_address"]);
                                    String person_email_address = reader["person_email_address"];
                                    PersonInfo["person_email_address"] = person_email_address;
                                    Debug.WriteLine("Person's Email Address: " + person_email_address);
                                    txEmailAddress.Text = person_email_address;

                                    personalInfo.setPersonHomeTown(reader["person_hometown"]);
                                    String person_hometown = reader["person_hometown"];
                                    PersonInfo["person_hometown"] = person_hometown;
                                    Debug.WriteLine("Person's Hometown: " + person_hometown);
                                    txHometown.Text = person_hometown; //person_hometown

                                    personalInfo.setPersonCurrentResidence(reader["person_current_residence"]);
                                    String person_current_residence = reader["person_current_residence"];
                                    PersonInfo["person_current_residence"] = person_current_residence;
                                    Debug.WriteLine("Person's Current Residence: " + person_current_residence);
                                    txCurrentResidence.Text = person_current_residence;

                                    personalInfo.setPersonNationality(reader["person_nationality"]);
                                    String person_nationality = reader["person_nationality"];
                                    PersonInfo["person_nationality"] = person_nationality;
                                    Debug.WriteLine("Person's Nationality: " + person_nationality);
                                    txNationality.Text = person_nationality;

                                    personalInfo.setPersonOccupation(reader["person_occupation"]);
                                    String person_occupation = reader["person_occupation"];
                                    PersonInfo["person_occupation"] = person_occupation;
                                    Debug.WriteLine("Person's Occupation: " + person_occupation);
                                    txOccupation.Text = person_occupation;

                                    personalInfo.setPersonGender(reader["person_gender"]);
                                    String person_gender = reader["person_gender"];
                                    PersonInfo["person_gender"] = person_gender;
                                    Debug.WriteLine("Person's Gender: " + person_gender);
                                    txGender.Text = person_gender;

                                    personalInfo.setPersonMaritalStatus(reader["person_marital_status"]);
                                    String person_marital_status = reader["person_marital_status"];
                                    PersonInfo["person_marital_status"] = person_marital_status;
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
            string postData = "data=" + xmlStringToGet;

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

        private void btnSave_Click_1(object sender, RoutedEventArgs e)
        {
            progressbar.Visibility = System.Windows.Visibility.Visible;
            if ((string.IsNullOrWhiteSpace(txDateOfBirth.Text)) && (string.IsNullOrWhiteSpace(txEyeColor.Text)) && (string.IsNullOrWhiteSpace(txFirstName.Text)) && (string.IsNullOrWhiteSpace(txMiddleName.Text)) && (string.IsNullOrWhiteSpace(txSurname.Text)) && (string.IsNullOrWhiteSpace(txBloodType.Text)) && (string.IsNullOrWhiteSpace(txHeight.Text)) && (string.IsNullOrWhiteSpace(txGender.Text)) && (string.IsNullOrWhiteSpace(txMaritalStatus.Text)) && (string.IsNullOrWhiteSpace(txWeight.Text)) && (string.IsNullOrWhiteSpace(txEmailAddress.Text)) && (string.IsNullOrWhiteSpace(txGender.Text)) && (string.IsNullOrWhiteSpace(txNationality.Text)) && (string.IsNullOrWhiteSpace(txCurrentResidence.Text)) && (string.IsNullOrWhiteSpace(txHometown.Text)) && (string.IsNullOrWhiteSpace(txOccupation.Text)))
            {
                progressbar.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show("Fill in the appropriate credentials");
            }
            else
            {
                xmlStringToGet = "<message>" +
                             "<msg>edit_person</msg>" +
                             "<person " +
                             "person_first_name = '" + txFirstName.Text + "' " +
                             "person_surname = '" + txSurname.Text + "' " +
                             "person_middle_name = '" + txMiddleName.Text + "' " +
                             "person_date_of_birth = '" + txDateOfBirth.Text + "' " +
                             "person_eye_color = '" + txEyeColor.Text + "' " +
                             "person_blood_type = '" + txBloodType.Text + "' " +
                             "person_height = '" + txHeight.Text + "' " +
                             "person_weight = '" + txWeight.Text + "' " +
                             "person_email_address = '" + txEmailAddress.Text + "' " +
                             "person_hometown = '" + txHometown.Text + "' " +
                             "person_current_residence = '" + txCurrentResidence.Text + "' " +
                             "person_nationality = '" + txNationality.Text + "' " +
                             "person_occupation = '" + txOccupation.Text + "' " +
                             "person_gender = '" + txGender.Text + "' " +
                             "person_marital_status = '" + txMaritalStatus.Text + "'/>" +
                             "</message>";
                //send new xml
                MessageBox.Show(xmlStringToGet);
                sent = 1;

                SentPostReport();
            }
        }
    }
}