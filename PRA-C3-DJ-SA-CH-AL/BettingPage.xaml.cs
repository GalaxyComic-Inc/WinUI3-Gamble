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

private void SaveBet_Click(object sender, RoutedEventArgs e)
{
    // Validate the bet amount
    if (decimal.TryParse(BetAmountInput.Text, out decimal betAmount))
    {
        // Convert betAmount to int
        int betAmountInt = (int)betAmount;

        if (betAmountInt <= 0)
        {
            ApiData.Text = "Bet amount must be greater than 0.";
            return;
        }

        // Check if the user has enough credits
        if (betAmountInt > CurrentUser.Credits)
        {
            ApiData.Text = "You do not have enough credits to place this bet.";
            return;
        }

        // Get the player guess and the winning guess (you can update as needed)
        string playerGuess = PlayerGuessInput.Text;
        var winningGuess = WinningGuessComboBox.SelectedItem as ComboBoxItem;
        string winningTeam = winningGuess?.Content.ToString();

        if (string.IsNullOrEmpty(playerGuess) || winningGuess == null)
        {
            ApiData.Text = "Please fill in all the fields.";
            return;
        }

        // Save the bet in the database
        using (var dbContext = new UserDbContext())
        {
            // Create a new bet object
            var newBet = new Bets
            {
                MatchId = 1, // Set the actual match ID here
                UserId = CurrentUser.Id, // Assuming UserId is available
                Amount = betAmountInt,
                PlayerGuess = int.Parse(playerGuess), // Assuming player guess is an integer
                WinningGuess = winningTeam, // Convert string to integer (1 = Team1, 2 = Team2, 3 = Draw)
                BetTime = DateTime.Now
            };

            // Add the new bet to the database
            dbContext.Bets.Add(newBet);
            dbContext.SaveChanges(); // Commit the changes to the database

            // After saving the bet, update the user's credits
            CurrentUser.Credits -= betAmountInt;

            // Update the UI with CurrentUser data
            ResultText.Text = $"Name: {CurrentUser.UserName} \nCredits: {CurrentUser.Credits}";

                    // Find the user in the database and update their credits
                    var user = dbContext.Users.FirstOrDefault(u => u.Id == CurrentUser.Id);
            if (user != null)
            {
                // Update the user's credits in the database
                user.Credits = CurrentUser.Credits;
                dbContext.SaveChanges(); // Save the updated user info back to the database
            }
            else
            {
                ApiData.Text = "User not found in the database.";
                return;
            }
        }

        // Display the result and the updated credits
        ApiData.Text = $"Bet placed: ${betAmountInt} on {winningTeam}. \nRemaining Credits: {CurrentUser.Credits}";
    }
    else
    {
        ApiData.Text = "Invalid bet amount. Please enter a valid number.";
    }
}


    }
}
