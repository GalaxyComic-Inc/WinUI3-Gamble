using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PRA_C3_DJ_SA_CH_AL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class OverviewPage : Page
    {
        public OverviewPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Retrieve the passed 'user' object from the navigation parameter
            if (e.Parameter is User user)
            {
                // Use the user object (for example, display user data in the UI)
                ResultText.Text = $"User ID: {user.Id}, Name: {user.UserName} Credits: {user.Credits}";

                // You can also use other user properties or perform logic based on user data
            }
            else
            {
                // Handle the case where the parameter is not the expected 'user' object
                ResultText.Text = "User data is missing or invalid.";
            }
        }
    }
}
