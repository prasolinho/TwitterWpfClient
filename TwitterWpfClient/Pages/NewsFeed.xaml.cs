using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
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
                    int count = item.Entities.Mentions.Count;
                    int mentionIdx = 0;
                    bool allMentions = false;

                    tw.TweetText = new TextBlock();

                    StringBuilder sb = new StringBuilder(100);
                    for (int i = 0; i < item.Text.Length; i++)
                    {
                        if (!allMentions)
                        {
                            if (i == item.Entities.Mentions[mentionIdx].StartIndex)
                            {
                                if (sb.Length > 0)
                                {
                                    tw.TweetText.Inlines.Add(sb.ToString());
                                    sb.Clear();
                                }
                            }
                            if (i == item.Entities.Mentions[mentionIdx].EndIndex)
                            {
                                Run hyperLinkText = new Run(item.Entities.Mentions[mentionIdx].ScreenName);
                                Hyperlink hyperlink = new Hyperlink(hyperLinkText);
                                hyperlink.NavigateUri = new Uri("https://twitter.com/" + item.Entities.Mentions[mentionIdx].ScreenName);
                                hyperlink.RequestNavigate += Hyperlink_RequestNavigate;

                                tw.TweetText.Inlines.Add(hyperlink);

                                sb.Clear();


                                mentionIdx++;
                                if (mentionIdx == item.Entities.Mentions.Count)
                                    allMentions = true;
                            }
                        }
                        Char c = item.Text[i];
                        sb.Append(c);
                    }

                    if (sb.Length > 0)
                    {
                        tw.TweetText.Inlines.Add(sb.ToString());
                    }

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
            int idx = 1;
            foreach (TextBlock tb in Framework.Helpers.WpfTemplate.FindVisualChildren<TextBlock>(this))
            {
                if (tb.Name == "txtTweetText")
                {
                    long id = Convert.ToInt64(tb.Tag);

                    //tb = tweets.Where(t => t.Id == id).Select(t => t.TweetText).First();

                    TextBlock @new = tweets.Where(t => t.Id == id).Select(t => t.TweetText).First();

                    if (@new != null && @new.Inlines.Count > 0)
                    {
                        tb.Inlines.Clear();
                        foreach (var inline in @new.Inlines)
                        {
                            try
                            {
                                string text = XamlWriter.Save(inline);
                                Stream s = new MemoryStream(Encoding.Default.GetBytes(text));
                                Inline temp = XamlReader.Load(s) as Inline;

                                tb.Inlines.Add(temp);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.ToString());
                            }
                        }
                    }
                    //else
                    //    tb.Text = @new.Text;

                    //tb.Inlines.AddRange(tweets.Where(t => t.Id == id).Select(t => t.TweetText.Inlines).First());

                    //Run run2 = new Run(" Please");
                    //Hyperlink hyperlink = new Hyperlink(run2)
                    //{
                    //    NavigateUri = new Uri("http://stackoverflow.com")
                    //};
                    //hyperlink.RequestNavigate += Hyperlink_RequestNavigate; //to be implemented

                    //tb.Inlines.Clear();
                    //tb.Inlines.Add("Test 1");
                    //tb.Inlines.Add(hyperlink);
                    //tb.Inlines.Add("koniec");
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

        public TextBlock TweetText { get; set; }

    }
}
