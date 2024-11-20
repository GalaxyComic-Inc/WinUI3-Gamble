using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using PRA_C3_DJ_SA_CH_AL.Models;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
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
    public sealed partial class LoginPage : Page
    {
        private readonly UserDbContext _dbContext;
        public LoginPage()
        {
            this.InitializeComponent();

            _dbContext = new UserDbContext();
            _dbContext.Database.EnsureCreated();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var userName = UserNameTextBox.Text;
            var password = PasswordBox.Password;

            // Retrieve user from DB
            var user = await _dbContext.Users
                .Where(u => u.UserName == userName)
                .FirstOrDefaultAsync();

            var userId = user?.Id;

            if (user == null)
            {
                ResultText.Text = "Invalid password or User not found.";
                return;
            }

            // Verify the entered password with the stored hash
            if (user.VerifyPassword(password))
            {
                ResultText.Text = "Login successful!";
                Frame.Navigate(typeof(OverviewPage), user);  // Navigate to OverviewPage
            }
            else
            {
                ResultText.Text = "Invalid password or User not found.";
            }
        }
        private void HomePageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
