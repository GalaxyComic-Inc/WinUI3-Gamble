using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using PRA_C3_DJ_SA_CH_AL.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PRA_C3_DJ_SA_CH_AL
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BettingPage : Page
    {
        private User CurrentUser { get; set; }
        public BettingPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Retrieve the passed 'user' object
            if (e.Parameter is User user)
            {
                CurrentUser = user;

                // Update the UI with CurrentUser data
                ResultText.Text = $"Name: {CurrentUser.UserName} \nCredits: {CurrentUser.Credits}";
            }
            else
            {
                ResultText.Text = "User data is missing or invalid.";
            }
        }

        private void OverviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser != null)
            {
                Frame.Navigate(typeof(OverviewPage), CurrentUser);
            }
        }

        private void BettingButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser != null)
            {
                Frame.Navigate(typeof(BettingPage), CurrentUser);
            }
        }
    }
}
