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
        private User CurrentUser { get; set; } // Holds the currently logged-in user
        private readonly UserDbContext _dbContext; // Database context for user data
        private readonly DataApi _dataApi; // API client for fetching data

        public OverviewPage()
        {
            this.InitializeComponent();
            _dbContext = new UserDbContext();
            _dataApi = new DataApi();
        }

        // ----------------------------------------------------------------------------
        // API SECTION
        // ----------------------------------------------------------------------------

        /// <summary>
        /// Handles the button click to load matches from the API.
        /// </summary>
        private async void LoadMatches_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Fetch matches from the API
                List<Matches> matches = await _dataApi.GetMatchesAsync();

                // Bind matches to the ListView
                MatchesList.ItemsSource = matches;
            }
            catch (Exception ex)
            {
                // Show an error dialog if API call fails
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

        // ----------------------------------------------------------------------------
        // DATABASE SECTION
        // ----------------------------------------------------------------------------

        /// <summary>
        /// Handles page navigation and loads user data upon navigation to this page.
        /// </summary>
        protected override async void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                // Retrieve the current user from navigation parameters
                if (e.Parameter is User user)
                {
                    CurrentUser = user;

                    // Display user details
                    ResultText.Text = $"Name: {CurrentUser.UserName}\nCredits: {CurrentUser.Credits}";

                    // Load the user's bets
                    await LoadBets(CurrentUser.Id);
                }
                else
                {
                    ResultText.Text = "User data is missing or invalid.";
                }
            }
            catch (Exception ex)
            {
                // Handle potential navigation or data errors
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

        /// <summary>
        /// Loads the user's bets from the database and binds them to the UI.
        /// </summary>
        private async Task LoadBets(int userId)
        {
            try
            {
                // Query the database for the user's bets
                var bets = await _dbContext.Bets
                    .Where(b => b.UserId == userId)
                    .ToListAsync();

                // Bind bets to the ListView
                BetsListView.ItemsSource = bets.Select(b => new
                {
                    BetAmount = b.Amount,
                    BetTime = b.BetTime.ToString("g") // Format the date/time of the bet
                }).ToList();
            }
            catch (Exception ex)
            {
                // Show an error dialog if database query fails
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

        // ----------------------------------------------------------------------------
        // NAVIGATION SECTION
        // ----------------------------------------------------------------------------

        /// <summary>
        /// Navigates back to the OverviewPage with the current user data.
        /// </summary>
        private void OverviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser != null)
            {
                Frame.Navigate(typeof(OverviewPage), CurrentUser);
            }
        }

        /// <summary>
        /// Navigates to the BettingPage with the current user data.
        /// </summary>
        private void BettingButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser != null)
            {
                Frame.Navigate(typeof(BettingPage), CurrentUser);
            }
        }
    }
}
