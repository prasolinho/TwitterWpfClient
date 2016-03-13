using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TweetSharp;

namespace TwitterWpfClient.Pages
{
    /// <summary>
    /// Interaction logic for NewsFeed.xaml
    /// </summary>
    public partial class NewsFeed : Page
    {
        //public IEnumerable<Tweet> Tweets;

        public NewsFeed()
        {
            InitializeComponent();
            //DataContext = Tweets;
            var result = Service.Twitter.Instance.GetTweets();

            List<Tweet> tweets = new List<Tweet>(result.Count());
            foreach (var item in result)
            {
                tweets.Add(new Tweet
                {
                    AuthorName = item.User.Name,
                    AuthorScreenName = "@" + item.User.ScreenName,
                    CreatedDate = item.CreatedDate,
                    ProfileImageUrl = item.User.ProfileImageUrl,
                    Text = item.Text
                });
            }
            //Tweets = tweets;
            lstTweets.ItemsSource = tweets;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //var result = await Service.Twitter.Instance.GetTweetsAsync();

            //List<Tweet> tweets = new List<Tweet>(result.Count());
            //foreach (var item in result)
            //{
            //    tweets.Add(new Tweet
            //    {
            //        AuthorName = item.User.Name,
            //        AuthorScreenName = item.User.ScreenName,
            //        CreatedDate = item.CreatedDate,
            //        ProfileImageUrl = item.User.ProfileImageUrl,
            //        Text = item.Text
            //    });
            //}

            //Tweets = tweets;
        }
    }

    public class Tweet
    {
        public string AuthorName { get; set; }
        public string AuthorScreenName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Text { get; set; }
    }
}
