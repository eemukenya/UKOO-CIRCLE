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

namespace Ukoo_Circle
{
    public partial class MyProfilePage : PhoneApplicationPage
    {
        public MyProfilePage()
        {
            InitializeComponent();

            string profileUrl = "http://192.168.43.221:8081/ukoo_circle/core/engine.php";
            //myProfile.Navigate(new Uri(profileUrl, UriKind.Absolute));

            PersonalInfoClass personalInfo = new PersonalInfoClass();

            txtUserFullName.Text = personalInfo.getPersonFirstName() + personalInfo.getPersonMiddleName()+ personalInfo.getPersonMiddleName();

            Debug.WriteLine("Here is my name "+ personalInfo.getPersonFirstName() +" "+ personalInfo.getPersonMiddleName()+" " + personalInfo.getPersonMiddleName());




        }

        //Application bar links
        private void btnHome_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
        }

        private void btnProfile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/MyProfilePage.xaml", UriKind.Relative));
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

        //Personal Info link
        private void btnPersonInfo_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/PersonalInfoPage.xaml", UriKind.Relative));
        }

        private void btnFamily_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/MyFamilyPage.xaml", UriKind.Relative));
        }
    }
}