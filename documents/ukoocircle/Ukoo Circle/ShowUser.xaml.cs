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
    public partial class ShowUser : PhoneApplicationPage
    {
        public ShowUser()
        {
            InitializeComponent();
            FetchDatabase fetch = new FetchDatabase();
            allusers.ItemsSource = fetch.getUsers();
        }
    }
}