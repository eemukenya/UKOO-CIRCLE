using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Ukoo_Circle.Resources;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml;
using Windows.Web.Http;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Ukoo_Circle
{
    public partial class MainPage : PhoneApplicationPage
    {
        PersonalInfoClass personalInfo = new PersonalInfoClass();
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private string number;
        private string searchValue = null;
        private string xmlString = "<message>"+
                                   "<msg>get_person</msg>"+
                                   "<person "+
                                   "search_item = 'value'/>"+
                                   "</message>";

        

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            progressbar.Visibility = System.Windows.Visibility.Collapsed;

            FetchDatabase fetch = new FetchDatabase();
            number = fetch.getNumber();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void btnHome_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void btnProfile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/MyProfilePage.xaml", UriKind.Relative));
        }

        private void btnSettings_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/SettingsPage.xaml", UriKind.Relative));
        }

        private void btnUserDetails_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/UserDetailsPage.xaml", UriKind.Relative));
        }

        private void btnLocation_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/LocationPage.xaml", UriKind.Relative));
        }

        private void paymentBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/PaymentPage.xaml", UriKind.Relative));
        }

        private void btnSearch_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            progressbar.Visibility = System.Windows.Visibility.Visible;
            //NavigationService.Navigate(new Uri("/Pages/LocationPage.xaml", UriKind.Relative));

            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                progressbar.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show("Enter the person's name");
            }
            else
            {
                NavigationService.Navigate(new Uri("/Pages/SearchPerson.xaml?data=" + txtSearch.Text, UriKind.Relative));
                /*searchValue = txtSearch.Text;
                xmlString = "<message>" +
                        "<msg>get_person_android</msg>" +
                        "<person " +
                        "search_item = '"+searchValue+"'/>" +
                        "</message>";

                Debug.WriteLine(xmlString);

                SentPostReport();*/
            }


            
        }

        

        private void ParseResults(string results)
        {
            if (string.IsNullOrEmpty(results))
            {
                progressbar.Visibility = System.Windows.Visibility.Collapsed;
                Debug.WriteLine("No response");
                MessageBox.Show("Search Request was unsuccessful");
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
                progressbar.Visibility = System.Windows.Visibility.Collapsed;
                Debug.WriteLine("Response is:" + results);
                //MessageBox.Show("Search Request was " + results);

                using (XmlReader reader = XmlReader.Create(new StringReader(results)))
                {

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                            if (reader.Name == "person")
                            {

                                
                                


                                break;
                            }
                    }


                    /*reader.ReadToFollowing("person");
                    reader.MoveToFirstAttribute();
                    reader.MoveToNextAttribute();
                   
                    string genre = reader.Value;

                    reader.ReadToFollowing("title");*/
                }

                if(number == txtSearch.Text)
                {
                    NavigationService.Navigate(new Uri("/Pages/PaymentPage.xaml", UriKind.Relative));
                }
                else
                {
                    //NavigationService.Navigate(new Uri("/Pages/MyProfilePage.xaml", UriKind.Relative));
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

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult edit = MessageBox.Show("Exit application?", "Close application", MessageBoxButton.OKCancel);

            if (edit == MessageBoxResult.OK)
            {
                Application.Current.Terminate();
            }

            base.OnBackKeyPress(e);
        }


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}