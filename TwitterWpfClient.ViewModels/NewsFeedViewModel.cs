using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using TweetSharp;
using TwitterWpfClient.Models;

namespace TwitterWpfClient.ViewModels
{
    public class NewsFeedViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Tweet> tweets;

        public ObservableCollection<Tweet> Tweets
        {
            get
            {
                return tweets;
            }
            set
            {
                tweets = value;
                RaisePropertyChanged("Tweets");
            }

        }

        public NewsFeedViewModel()
        {
            LoadNewsFeedTweets();
        }

        public void LoadNewsFeedTweets()
        {
            var serviceResult = Service.Twitter.Instance.GetTweets();
            ObservableCollection<Tweet> tweets = new ObservableCollection<Tweet>();
            foreach (var tweet in serviceResult)
            {
                tweets.Add(ConvertToTweet(tweet));
            }

            Tweets = tweets;
        }

        private Tweet ConvertToTweet(TwitterStatus tweet)
        {
            Tweet tw = new Tweet();
            tw.Id = tweet.Id;
            tw.Text = tweet.Text;
            tw.CreatedDate = tweet.CreatedDate.AddHours(1);

            if (tweet.Entities.Media.Count > 0)
            {
                TwitterMedia media = tweet.Entities.Media[0];
                if (media.MediaType == TwitterMediaType.Photo)
                    tw.ImageUrl = media.MediaUrl;
            }
            if (tweet.Entities.Urls.Count > 0)
            {
                TwitterUrl urlData = tweet.Entities.Urls[0];
                tw.DisplayUrl = urlData.DisplayUrl;
                tw.Url = urlData.Value;

                tw.Text = tweet.Text.Remove(urlData.StartIndex, urlData.EndIndex - urlData.StartIndex);
            }

            if (tweet.Entities.Mentions.Count > 0)
            {
                tw.HasMentions = true;

                tw.Mentions = tweet.Entities.Mentions
                    .OrderBy(m => m.StartIndex)
                    .Select(m => new Mention
                    {
                        StartIndex = m.StartIndex,
                        EndIndex = m.EndIndex,
                        ScreenName = m.ScreenName
                    })
                    .ToArray();
            }

            tw.AuthorName = tweet.User.Name;
            tw.AuthorScreenName = "@" + tweet.User.ScreenName;
            tw.ProfileImageUrl = tweet.User.ProfileImageUrl;

            return tw;
        }
    }
}
