using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
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
        private List<Matches> _cachedMatches;

        // Store the name of the guessed team or "Draw"
        public string PlayerGuessName { get; private set; }

        public BettingPage()
        {
            this.InitializeComponent();
            _dataApi = new DataApi(); // Assume DataApi handles API calls
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is User user)
            {
                CurrentUser = user;
                ResultText.Text = $"Name: {CurrentUser.UserName} | Credits: {CurrentUser.Credits}";
                await LoadMatchesAsync();
            }
            else
            {
                ResultText.Text = "User data is missing or invalid.";
            }
        }

        private async Task LoadMatchesAsync()
        {
            try
            {
                _cachedMatches = await _dataApi.GetMatchesAsync();

                if (_cachedMatches.Any())
                {
                    WinningGuessComboBox.ItemsSource = _cachedMatches.Select(m => new ComboBoxItem
                    {
                        Content = $"{m.Team1Name} vs {m.Team2Name}",
                        Tag = m.Id
                    }).ToList();
                    ApiData.Text = "Matches loaded successfully.";
                }
                else
                {
                    ApiData.Text = "No matches available.";
                }
            }
            catch (Exception ex)
            {
                ApiData.Text = $"Error loading matches: {ex.Message}";
            }
        }

        private void WinningGuessComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                PlayerGuessRadioPanel.Children.Clear();

                if (WinningGuessComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    int matchId = (int)selectedItem.Tag;
                    var selectedMatch = _cachedMatches.FirstOrDefault(m => m.Id == matchId);

                    if (selectedMatch == null)
                    {
                        ApiData.Text = "Match details not found.";
                        return;
                    }

                    // Add RadioButton for Team1
                    PlayerGuessRadioPanel.Children.Add(new RadioButton
                    {
                        Content = selectedMatch.Team1Name,
                        Tag = selectedMatch.Team1Id,
                        Margin = new Thickness(5)
                    });

                    // Add RadioButton for Team2
                    PlayerGuessRadioPanel.Children.Add(new RadioButton
                    {
                        Content = selectedMatch.Team2Name,
                        Tag = selectedMatch.Team2Id,
                        Margin = new Thickness(5)
                    });

                    // Add RadioButton for Draw
                    PlayerGuessRadioPanel.Children.Add(new RadioButton
                    {
                        Content = "Draw",
                        Tag = 0,
                        Margin = new Thickness(5)
                    });
                }
            }
            catch (Exception ex)
            {
                ApiData.Text = $"Error updating PlayerGuessRadioPanel: {ex.Message}";
            }
        }

        private async void SaveBet_Click(object sender, RoutedEventArgs e)
        {
            // Validate bet amount
            if (!decimal.TryParse(BetAmountInput.Text, out decimal betAmount) || betAmount <= 0)
            {
                ApiData.Text = "Invalid bet amount.";
                return;
            }

            if (betAmount > CurrentUser.Credits)
            {
                ApiData.Text = "Insufficient credits.";
                return;
            }

            // Get selected match
            if (!(WinningGuessComboBox.SelectedItem is ComboBoxItem selectedMatch))
            {
                ApiData.Text = "Please select a match.";
                return;
            }

            int matchId = (int)selectedMatch.Tag;

            // Get selected team or draw
            var selectedRadio = PlayerGuessRadioPanel.Children
                .OfType<RadioButton>()
                .FirstOrDefault(rb => rb.IsChecked == true);

            if (selectedRadio == null)
            {
                ApiData.Text = "Please select your guess.";
                return;
            }

            int playerGuess = (int)selectedRadio.Tag;

            // Save the selected team's name or "Draw" in PlayerGuessName
            PlayerGuessName = selectedRadio.Content.ToString();

            // Deduct credits and save the bet
            CurrentUser.Credits -= (int)betAmount;
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    // Save bet and update user credits
                    dbContext.Bets.Add(new Bets
                    {
                        MatchId = matchId,
                        UserId = CurrentUser.Id,
                        Amount = (int)betAmount,
                        PlayerGuess = playerGuess,
                        PlayerGuessName = PlayerGuessName, // Save the guessed team's name
                        BetTime = DateTime.Now
                    });

                    dbContext.Users.First(u => u.Id == CurrentUser.Id).Credits = CurrentUser.Credits;
                    await dbContext.SaveChangesAsync();
                }

                ResultText.Text = $"Name: {CurrentUser.UserName} | Credits: {CurrentUser.Credits}";
                ApiData.Text = $"Bet placed successfully! You guessed: {PlayerGuessName}. Remaining credits: {CurrentUser.Credits}.";
            }
            catch (Exception ex)
            {
                ApiData.Text = $"Error saving bet: {ex.Message}";
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