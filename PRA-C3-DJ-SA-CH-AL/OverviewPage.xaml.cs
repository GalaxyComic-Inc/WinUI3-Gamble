using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.EntityFrameworkCore;
using PRA_C3_DJ_SA_CH_AL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRA_C3_DJ_SA_CH_AL
{
    public sealed partial class OverviewPage : Page
    {
        private User CurrentUser { get; set; }
        private UserDbContext _dbContext;

        public OverviewPage()
        {
            this.InitializeComponent();
            _dbContext = new UserDbContext();
        }

        protected override async void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Retrieve the current user from the navigation parameter
            if (e.Parameter is User user)
            {
                CurrentUser = user;
                ResultText.Text = $"Name: {CurrentUser.UserName} \nCredits: {CurrentUser.Credits}";

                // Load the user's bets
                await LoadBets(CurrentUser.Id);
            }
            else
            {
                ResultText.Text = "User data is missing or invalid.";
            }
        }

        private async Task LoadBets(int userId)
        {
            // Retrieve the bets for the user from the database
            var bets = await _dbContext.Bets
                .Where(b => b.UserId == userId)
                .ToListAsync();

            // Bind the bets to the ListView
            BetsListView.ItemsSource = bets.Select(b => new
            {
                BetAmount = b.Amount,
                BetTime = b.BetTime.ToString("g") // Format the bet time
            }).ToList();
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
