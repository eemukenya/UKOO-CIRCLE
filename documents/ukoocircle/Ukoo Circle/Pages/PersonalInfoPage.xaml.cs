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
using System.IO;
using System.Diagnostics;
using System.Xml;

namespace Ukoo_Circle
{
    public partial class PersonalInfoPage : PhoneApplicationPage
    {
        PersonalInfoClass personalInfo = new PersonalInfoClass();
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private string xmlString = "<message>" +
                                   "<msg>get_person</msg>" +
                                   "<person " +
                                   "search_item = 'value'/>" +
                                   "</message>";
        private string number;
        public PersonalInfoPage()
        {
            InitializeComponent();
            
            //get user's phone-number
            FetchDatabase fetch = new FetchDatabase();
            number = fetch.getNumber();

            //MessageBox.Show(number);

            Console.Write("Number: " + number);

            xmlString = "<message>" +
                        "<msg>get_person</msg>" +
                        "<person " +
                        "search_item = '" + number + "'/>" +
                        "</message>";

            SentPostReport();

            string profileUrl = "http://192.168.43.221:8081/ukoo_circle/core/engine.php";
            //myProfile.Navigate(new Uri(profileUrl, UriKind.Absolute));

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
            else
            {
                Debug.WriteLine("Response is:" + results);
                //MessageBox.Show("Login Attempt was " + results);

                using (XmlReader reader = XmlReader.Create(new StringReader(results)))
                {

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                            if (reader.Name == "person")
                            {

                                //List<string> PersonInfo = new List<string>();

                                var PersonInfo = new Dictionary<string, string>();

                                progressbar.Visibility = System.Windows.Visibility.Collapsed;


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
                                txFather.Text = person_hometown;

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

                                personalInfo.setPersonSpouse(reader["person_spouse"]);
                                String person_spouse = reader["person_spouse"];
                                PersonInfo["person_spouse"] = person_spouse;
                                Debug.WriteLine("Person's Spouse: " + person_spouse);
                                //

                                personalInfo.setPersonFather(reader["person_fathers_full_name"]);
                                String person_fathers_full_name = reader["person_fathers_full_name"];
                                PersonInfo["person_fathers_full_name"] = person_fathers_full_name;
                                Debug.WriteLine("Person's Father: " + person_fathers_full_name);
                                txFather.Text = person_fathers_full_name;

                                personalInfo.setPersonMother(reader["person_mothers_full_name"]);
                                String person_mothers_full_name = reader["person_mothers_full_name"];
                                PersonInfo["person_mothers_full_name"] = person_mothers_full_name;
                                Debug.WriteLine("Person's Mother: " + person_mothers_full_name);

                                personalInfo.setPersonChildren(reader["person_children"]);
                                String person_children = reader["person_children"];
                                PersonInfo["person_children"] = person_children;
                                Debug.WriteLine("Person's Children: " + person_children);

                                personalInfo.setPersonBio(reader["person_bio"]);
                                String person_bio = reader["person_bio"];
                                PersonInfo["person_bio"] = person_bio;
                                Debug.WriteLine("Person's Bio: " + person_bio);

                                personalInfo.setPersonProfile(reader["person_profile"]);
                                String person_profile = reader["person_profile"];
                                PersonInfo["person_profile"] = person_profile;
                                Debug.WriteLine("Person's Profile: " + person_profile);

                                break;
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
                Debug.WriteLine(ex);
            }

            allDone.Set();
        }
    }
}