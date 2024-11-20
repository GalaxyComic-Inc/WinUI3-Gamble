using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PRA_C3_DJ_SA_CH_AL.Models;
using PRA_C3_DJ_SA_CH_AL;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
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
using Microsoft.EntityFrameworkCore;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PRA_C3_DJ_SA_CH_AL
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        private readonly UserDbContext _dbContext;
        public RegisterPage()
        {
            this.InitializeComponent();

            _dbContext = new UserDbContext();
            _dbContext.Database.EnsureCreated();

        }
        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var userName = UserNameTextBox.Text;
            var password = PasswordBox.Password;

            // Check if username already exists
            var existingUser = await _dbContext.Users
                .Where(u => u.UserName == userName)
                .FirstOrDefaultAsync();

            if (existingUser != null)
            {
                ResultText.Text = "Username already taken.";
                return;
            }

            // Create a new user and hash the password before saving
            var newUser = new User
            {
                UserName = userName
            };
            newUser.SetPassword(password);  // Hash the password
            newUser.Credits = 50; // default credits amount

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();  // Save to MySQL database

            ResultText.Text = "User registered successfully!";
            Frame.Navigate(typeof(OverviewPage));  // Navigate to OverviewPage
        }

        private void HomePageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
