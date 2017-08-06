using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Ukoo_Circle
{
    public partial class UserDetails : PhoneApplicationPage
    {
      

        public UserDetails()
        {
            InitializeComponent();

            /*string profileUrl = "http://192.168.43.221:8081/ukoo_circle/core/engine.php";
            myProfile.Navigate(new Uri(profileUrl, UriKind.Absolute));*/
        }

        //Application menu bar
        private void btnHome_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
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
            //NavigationService.Navigate(new Uri("/UserDetailsPage.xaml", UriKind.Relative));
        }

        private void btnLocation_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/LocationPage.xaml", UriKind.Relative));
        }


        //Familial link buttons
        private void childrenBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/UserDetails/ChildrenListPage.xaml", UriKind.Relative));
        }

        private void spouseBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/UserDetails/SpouseListPage.xaml", UriKind.Relative));
        }

        private void parentsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/UserDetails/ParentListPage.xaml", UriKind.Relative));
        }

        private void siblingsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/UserDetails/SiblingsListPage.xaml", UriKind.Relative));
        }

        private void cousinsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/UserDetails/CousinsListPage.xaml", UriKind.Relative));
        }

        private void profileBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/UserDetails/EditProfilePage.xaml", UriKind.Relative));
        }
    }
}