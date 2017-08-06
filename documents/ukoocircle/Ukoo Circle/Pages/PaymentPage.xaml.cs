using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Ukoo_Circle.Pages
{
    public partial class PaymentPage : PhoneApplicationPage
    {
        public PaymentPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string number = "";
            //string index = "";

            if (NavigationContext.QueryString.TryGetValue("number", out number))
            {
                //MessageBox.Show("send to " + number + "?");
            }

        }

        private void btnProceed_Click(object sender, RoutedEventArgs e)
        {
            //progressbar.Visibility = System.Windows.Visibility.Visible;
            if ((string.IsNullOrWhiteSpace(txAmount.Text)) && (string.IsNullOrWhiteSpace(txPhoneNumber.Text) && (string.IsNullOrWhiteSpace(txRefNo.Text))))
            {
                //progressbar.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show("Fill in the fields");
            }
            else
            {
                //progressbar.Visibility = System.Windows.Visibility.Collapsed;
                NavigationService.Navigate(new Uri("/Pages/BioPage.xaml", UriKind.Relative));

            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult edit = MessageBox.Show("Exit search?", "Close results", MessageBoxButton.OKCancel);

            if (edit == MessageBoxResult.OK)
            {
                NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
            }

            base.OnBackKeyPress(e);
        }
    }
}