using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using TweetSharp;

namespace TwitterWpfClient.Pages
{
    /// <summary>
    /// Interaction logic for NewsFeed.xaml
    /// </summary>
    public partial class NewsFeed : Page
    {
        //public IEnumerable<Tweet> Tweets;
        List<Tweet> tweets;

        public NewsFeed()
        {
            InitializeComponent();
            //DataContext = Tweets;
            var result = Service.Twitter.Instance.GetTweets();

            tweets = new List<Tweet>(result.Count());
            foreach (var item in result)
            {
                if (item.Entities.Media.Count > 1)
                {

                }
                if (item.Entities.Urls.Count > 1)
                {

                }

                Tweet tw = new Tweet();
                tw.Id = item.Id;
                tw.Text = item.Text;
                tw.CreatedDate = item.CreatedDate.AddHours(1);

                if (item.Entities.Media.Count > 0)
                {
                    TwitterMedia media = item.Entities.Media[0];
                    if (media.MediaType == TwitterMediaType.Photo)
                        tw.ImageUrl = media.MediaUrl;
                }
                if (item.Entities.Urls.Count > 0)
                {
                    TwitterUrl urlData = item.Entities.Urls[0];
                    tw.DisplayUrl = urlData.DisplayUrl;
                    tw.Url = urlData.Value;

                    tw.Text = item.Text.Remove(urlData.StartIndex, urlData.EndIndex - urlData.StartIndex);
                }

                if (item.Entities.Mentions.Count > 0)
                {
                    tw.HasMentions = true;

                    tw.Mentions = item.Entities.Mentions
                        .OrderBy(m => m.StartIndex)
                        .Select(m => new Mention
                        {
                            StartIndex = m.StartIndex,
                            EndIndex = m.EndIndex,
                            ScreenName = m.ScreenName
                        })
                        .ToArray();

                   

                    //tw.Text = tw.TweetText.Text;
                }

                tw.AuthorName = item.User.Name;
                tw.AuthorScreenName = "@" + item.User.ScreenName;
                tw.ProfileImageUrl = item.User.ProfileImageUrl;

                tweets.Add(tw);
            }
            //Tweets = tweets;
            lstTweets.ItemsSource = tweets;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
            e.Handled = true; // Aby nie otwierać w programie
        }

        /// <summary>
        /// http://stackoverflow.com/questions/1851620/handling-scroll-event-on-listview-in-c-sharp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstTweets_Loaded(object sender, RoutedEventArgs e)
        {
            if (VisualTreeHelper.GetChildrenCount(lstTweets) != 0)
            {
                Decorator border = VisualTreeHelper.GetChild(lstTweets, 0) as Decorator;
                ScrollViewer sv = border.Child as ScrollViewer;
                sv.ScrollChanged += ScrollViewer_ScrollChanged;
            }

            UpdateVisualItems();
        }

        private void UpdateVisualItems()
        {
            foreach (TextBlock tb in Framework.Helpers.WpfTemplate.FindVisualChildren<TextBlock>(this))
            {
                if (tb.Name == "txtTweetText")
                {
                    long id = Convert.ToInt64(tb.Tag);
                    Tweet tweet = tweets.Where(t => t.Id == id).First();
                    if (tweet.HasMentions)
                    {
                        tb.Inlines.Clear();

                        int count = tweet.Mentions.Length;
                        int mentionIdx = 0;
                        bool allMentions = false;

                        StringBuilder sb = new StringBuilder(100);
                        for (int i = 0; i < tweet.Text.Length; i++)
                        {
                            if (!allMentions)
                            {
                                if (i == tweet.Mentions[mentionIdx].StartIndex)
                                {
                                    if (sb.Length > 0)
                                    {
                                        tb.Inlines.Add(sb.ToString());
                                        sb.Clear();
                                    }
                                }
                                if (i == tweet.Mentions[mentionIdx].EndIndex)
                                {
                                    Run hyperLinkText = new Run(tweet.Mentions[mentionIdx].ScreenName);
                                    Hyperlink hyperlink = new Hyperlink(hyperLinkText);
                                    hyperlink.NavigateUri = new Uri("https://twitter.com/" + tweet.Mentions[mentionIdx].ScreenName);
                                    hyperlink.RequestNavigate += Hyperlink_RequestNavigate;

                                    tb.Inlines.Add(hyperlink);

                                    sb.Clear();

                                    mentionIdx++;
                                    if (mentionIdx == tweet.Mentions.Length)
                                        allMentions = true;
                                }
                            }
                            Char c = tweet.Text[i];
                            sb.Append(c);
                        }

                        if (sb.Length > 0)
                        {
                            tb.Inlines.Add(sb.ToString());
                        }
                    }
                }
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            UpdateVisualItems();
        }
    }

    public class Tweet
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }

        public string DisplayUrl { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }

        public string AuthorName { get; set; }
        public string AuthorScreenName { get; set; }
        public string ProfileImageUrl { get; set; }

        //public TextBlock TweetText { get; set; }

        public bool HasMentions { get; set; }
        public Mention[] Mentions { get; set; }
    }

    public class Mention
    {
        public int Order { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public string ScreenName { get; set; }
    }
}
