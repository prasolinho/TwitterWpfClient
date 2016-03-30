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

namespace TwitterWpfClient.Pages
{
    /// <summary>
    /// Interaction logic for NewsFeed.xaml
    /// </summary>
    public partial class NewsFeed : Page
    {
        public NewsFeed()
        {
            InitializeComponent();
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
            //foreach (TextBlock tb in Framework.Helpers.WpfTemplate.FindVisualChildren<TextBlock>(lstTweets))
            //{
            //    if (tb.Name == "txtTweetText")
            //    {
            //        long id = Convert.ToInt64(tb.Tag);
            //        Tweet tweet = tweets.Where(t => t.Id == id).First();


            //        if (!tweet.ContentModified && tweet.HasMentions)
            //        {

            //            Debug.WriteLine("TODO: " + tweet.Text);

            //            tb.Inlines.Clear();

            //            int count = tweet.Mentions.Length;
            //            int mentionIdx = 0;
            //            bool allMentions = false;

            //            StringBuilder sb = new StringBuilder(100);
            //            for (int i = 0; i < tweet.Text.Length; i++)
            //            {
            //                if (!allMentions)
            //                {
            //                    if (i == tweet.Mentions[mentionIdx].StartIndex)
            //                    {
            //                        if (sb.Length > 0)
            //                        {
            //                            tb.Inlines.Add(sb.ToString());
            //                            sb.Clear();
            //                        }
            //                    }
            //                    if (i == tweet.Mentions[mentionIdx].EndIndex)
            //                    {
            //                        Run hyperLinkText = new Run(tweet.Mentions[mentionIdx].ScreenName);
            //                        Hyperlink hyperlink = new Hyperlink(hyperLinkText);
            //                        hyperlink.NavigateUri = new Uri("https://twitter.com/" + tweet.Mentions[mentionIdx].ScreenName);
            //                        hyperlink.RequestNavigate += Hyperlink_RequestNavigate;

            //                        tb.Inlines.Add(hyperlink);

            //                        sb.Clear();

            //                        mentionIdx++;
            //                        if (mentionIdx == tweet.Mentions.Length)
            //                            allMentions = true;
            //                    }
            //                }
            //                Char c = tweet.Text[i];
            //                sb.Append(c);
            //            }

            //            if (sb.Length > 0)
            //            {
            //                tb.Inlines.Add(sb.ToString());
            //            }
            //            tweet.ContentModified = true;
            //        }
            //    }
            //}
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            UpdateVisualItems();
        }
    }
}
