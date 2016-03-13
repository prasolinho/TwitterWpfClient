using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                if (item.Entities.Media.Count > 1)
                {

                }
                if (item.Entities.Urls.Count > 1)
                {

                }

                Tweet tw = new Tweet();
                tw.Text = item.Text;
                tw.CreatedDate = item.CreatedDate;

                if (item.Entities.Media.Count > 0)
                {
                    TwitterMedia media = item.Entities.Media[0];
                    if (media.MediaType == TwitterMediaType.Photo)
                        tw.ImageUrl = media.MediaUrl;
                }
                if (item.Entities.Urls.Count > 0)
                {
                    TwitterUrl urlData = item.Entities.Urls[0];
                    tw.Url = urlData.DisplayUrl;

                    tw.Text = item.Text.Remove(urlData.StartIndex, urlData.EndIndex - urlData.StartIndex);
                }

                tw.AuthorName = item.User.Name;
                tw.AuthorScreenName = "@" + item.User.ScreenName;
                tw.ProfileImageUrl = item.User.ProfileImageUrl;

                tweets.Add(tw);
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

        private void imgTweetImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("imgTweetImage_MouseLeftButtonUp");
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }
    }

    public class Tweet
    {
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }

        public string AuthorName { get; set; }
        public string AuthorScreenName { get; set; }
        public string ProfileImageUrl { get; set; }
        
    }
}
