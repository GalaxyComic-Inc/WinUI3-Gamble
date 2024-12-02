using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PRA_C3_DJ_SA_CH_AL.Data;
using PRA_C3_DJ_SA_CH_AL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRA_C3_DJ_SA_CH_AL
{
    public sealed partial class BettingPage : Page
    {
        private User CurrentUser { get; set; }
        private readonly DataApi _dataApi;

        public BettingPage()
        {
            this.InitializeComponent();
            _dataApi = new DataApi(); // Assume DataApi handles API calls
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Retrieve the passed 'user' object
            if (e.Parameter is User user)
            {
                CurrentUser = user;

                // Update the UI with CurrentUser data
                ResultText.Text = $"Name: {CurrentUser.UserName} \nCredits: {CurrentUser.Credits}";

                // Fetch available matches from the API
                await LoadMatchesAsync();
            }
            else
            {
                ResultText.Text = "User data is missing or invalid.";
            }
        }

        /// <summary>
        /// Fetches matches from the API and binds them to the UI.
        /// </summary>
        private async Task LoadMatchesAsync()
        {
            try
            {
                List<Matches> matches = await _dataApi.GetMatchesAsync();

                WinningGuessComboBox.ItemsSource = matches.Select(m => new ComboBoxItem
                {
                    Content = $"{m.Team1Name} vs {m.Team2Name}",
                    Tag = m.Id // Storing the match ID for later use
                }).ToList();

                ApiData.Text = "Matches loaded successfully.";
            }
            catch (Exception ex)
            {
                ApiData.Text = $"Error loading matches: {ex.Message}";
            }
        }

        private void SaveBet_Click(object sender, RoutedEventArgs e)
        {
            // Validate the bet amount
            if (decimal.TryParse(BetAmountInput.Text, out decimal betAmount))
            {
                int betAmountInt = (int)betAmount;

                if (betAmountInt <= 0)
                {
                    ApiData.Text = "Bet amount must be greater than 0.";
                    return;
                }

                if (betAmountInt > CurrentUser.Credits)
                {
                    ApiData.Text = "You do not have enough credits to place this bet.";
                    return;
                }

                // Extract selected Player Guess
                var selectedGuessRadioButton = PlayerGuessRadioPanel.Children.OfType<RadioButton>()
                                                .FirstOrDefault(rb => rb.IsChecked == true);
                if (selectedGuessRadioButton == null)
                {
                    ApiData.Text = "Please select a player guess.";
                    return;
                }

                int playerGuess = int.Parse(selectedGuessRadioButton.Tag.ToString());

                // Extract Winning Guess from ComboBox
                var winningGuess = WinningGuessComboBox.SelectedItem as ComboBoxItem;
                if (winningGuess == null)
                {
                    ApiData.Text = "Please select a match.";
                    return;
                }

                string winningTeam = winningGuess.Content.ToString();

                // Save the bet in the database
                using (var dbContext = new UserDbContext())
                {
                    var newBet = new Bets
                    {
                        MatchId = 1, // Set actual Match ID here
                        UserId = CurrentUser.Id,
                        Amount = betAmountInt,
                        PlayerGuess = playerGuess,
                        WinningGuess = winningTeam,
                        BetTime = DateTime.Now
                    };

                    dbContext.Bets.Add(newBet);
                    dbContext.SaveChanges();

                    // Update user credits
                    CurrentUser.Credits -= betAmountInt;
                    var user = dbContext.Users.FirstOrDefault(u => u.Id == CurrentUser.Id);
                    if (user != null)
                    {
                        user.Credits = CurrentUser.Credits;
                        dbContext.SaveChanges();
                    }
                }

                // Update UI
                ApiData.Text = $"Bet placed: ${betAmountInt} on {winningTeam}.\nRemaining Credits: {CurrentUser.Credits}";
            }
            else
            {
                ApiData.Text = "Invalid bet amount. Please enter a valid number.";
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
