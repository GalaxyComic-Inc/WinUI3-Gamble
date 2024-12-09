using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.EntityFrameworkCore;
using PRA_C3_DJ_SA_CH_AL.Models;
using PRA_C3_DJ_SA_CH_AL.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Text.RegularExpressions;

namespace PRA_C3_DJ_SA_CH_AL
{
    public sealed partial class OverviewPage : Page
    {
        private User CurrentUser { get; set; }
        private readonly UserDbContext _dbContext;
        private readonly DataApi _dataApi;

        public OverviewPage()
        {
            this.InitializeComponent();
            _dbContext = new UserDbContext();
            _dataApi = new DataApi();
        }

        // Fetch matches and results from the API
        private async Task LoadMatchesAsync()
        {
            try
            {
                List<Matches> matches = await _dataApi.GetMatchesAsync();
                List<Results> results = await _dataApi.GetResultsAsync();

                MatchesList.ItemsSource = matches;
            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error Loading Matches",
                    Content = $"An error occurred: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await errorDialog.ShowAsync();
            }
        }

        // Loads the user's bets from the database
        private async Task LoadBets(int userId)
        {
            try
            {
                var bets = await _dbContext.Bets
                    .Where(b => b.UserId == userId)
                    .ToListAsync();

                List<Results> results = await _dataApi.GetResultsAsync();

                // Bind bets to the ListView
                BetsListView.ItemsSource = bets.Select(b => new
                {
                    BetAmount = b.Amount,
                    BetTime = b.BetTime.ToString("g"),
                    PlayerGuessName = b.PlayerGuessName,
                    CorrectBet = CheckBet(b, results) ? "Correct" : "Incorrect" // Check if the bet is correct
                }).ToList();
            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error Loading Bets",
                    Content = $"An error occurred: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await errorDialog.ShowAsync();
            }
        }



        // This method checks if the bet is correct
        private bool CheckBet(Bets bet, List<Results> matchResults)
        {
            // Iterate over matchResults with foreach loop
            foreach (var matchResult in matchResults)
            {
                // Check if the match result matches the bet's match ID
                if (matchResult.Id == bet.MatchId)
                {
                    // Now that we found the match result, check if the bet is correct
                    if (bet.PlayerGuess == matchResult.WinnerId || (bet.PlayerGuess == 0 && matchResult.WinnerId == 0))
                    {
                        if (bet.Payed == false)
                        {
                            UpdateUserCredits(true, bet);
                            
                        }
                        MarkBetAsPaid(bet);
                        return true; // The bet is correct
                    }
                    else
                    {
                        if (bet.Payed == false)
                        {
                            UpdateUserCredits(false, bet);
                        }
                        
                        return false; // The bet is incorrect
                    }
                }
            }
            MarkBetAsPaid(bet);
            return false; 
        }

        protected override async void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                if (e.Parameter is User user)
                {
                    CurrentUser = user;
                    ResultText.Text = $"Name: {CurrentUser.UserName} | Credits: {CurrentUser.Credits}";
                    await LoadBets(CurrentUser.Id);
                    await LoadMatchesAsync();
                }
                else
                {
                    ResultText.Text = "User data is missing or invalid.";
                }
            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error Loading User Data",
                    Content = $"An error occurred: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await errorDialog.ShowAsync();
            }
        }

        private async void UpdateUserCredits(bool isCorrect, Bets bet)
        {
            try
            {                
                bet.Payed = true;
                // Update the bet in the database
                _dbContext.Bets.Update(bet);
                await _dbContext.SaveChangesAsync();

                // Find the current user and update their credits
                if (isCorrect)
                {
                    // Double the bet amount if the bet is correct
                    CurrentUser.Credits += bet.Amount * 2;
                    ResultText.Text = $"Name: {CurrentUser.UserName} | Credits: {CurrentUser.Credits}";
                    
                    // Update the user in the database
                    _dbContext.Users.Update(CurrentUser);
                    await _dbContext.SaveChangesAsync();
                }


                // Refresh the user data in the UI
                ResultText.Text = $"Name: {CurrentUser.UserName} | Credits: {CurrentUser.Credits}";
            }
            catch (Exception ex)
            {
                
            }
        }

        private async void MarkBetAsPaid(Bets bet)
        {
            try
            {
                bet.Payed = true; // Set Paid to true

                // Update the bet in the database
                _dbContext.Bets.Update(bet);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
            }
        }

        // Navigation Methods
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