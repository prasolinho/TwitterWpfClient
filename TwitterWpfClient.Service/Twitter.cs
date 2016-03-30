using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;

namespace TwitterWpfClient.Service
{
    public class Twitter : ITwitter
    {
        private static readonly ITwitter instance = new Twitter();
        private static readonly object lockTwitter = new object();
        public static ITwitter Instance { get { return instance; } }

        private TwitterService service = new TwitterService(Framework.AppSettings.Instance.ConsumerKey, Framework.AppSettings.Instance.ConsumerSecret);

        //public TwitterService Service
        //{
        //    get
        //    {
        //        lock(lockTwitter)
        //        {
        //            if (service == null)
        //                service = new TwitterService(Framework.AppSettings.Instance.ConsumerKey, Framework.AppSettings.Instance.ConsumerSecret);
        //            return service;
        //        }
        //    }
        //}

        private OAuthRequestToken requestToken;

        private Twitter()
        {
        }

        public Uri GetAuthorizationUri()
        {
            requestToken = service.GetRequestToken();

            // Step 2 - Redirect to the OAuth Authorization URL
            Uri uri = service.GetAuthorizationUri(requestToken);

            return uri;
        }

        public async Task<Uri> GetAuthorizationUriAsync()
        {
            // Pass your credentials to the service
            //string consumerKey = Framework.AppSettings.Instance.ConsumerKey;
            //string consumerSecret = Framework.AppSettings.Instance.ConsumerSecret;
            //TwitterService service = new TwitterService(consumerKey, consumerSecret);

            // Step 1 - Retrieve an OAuth Request Token
            var result = await service.GetRequestTokenAsync();
            requestToken = result.Value;

            // Step 2 - Redirect to the OAuth Authorization URL
            Uri uri = service.GetAuthorizationUri(requestToken);

            return uri;
        }

        public OAuthAccessToken GetAccessToken(string verifier)
        {
            OAuthAccessToken access = service.GetAccessToken(requestToken, verifier);
            return access;
        }

        public async Task<OAuthAccessToken> GetAccessTokenAsync(string verifier)
        {
            // Step 3 - Exchange the Request Token for an Access Token
            TwitterAsyncResult<OAuthAccessToken> access = await service.GetAccessTokenAsync(requestToken, verifier);

            return access.Value;

            // Step 4 - User authenticates using the Access Token
            //service.AuthenticateWith(access.Token, access.TokenSecret);
        }

        public bool AuthetnticateWithToken(string token, string tokenSecret)
        {
            try
            {
                service.AuthenticateWith(token, tokenSecret);
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }


        public IEnumerable<TwitterStatus> GetTweets()
        {
            return service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions { Count = 30 });
        }

        public async Task<IEnumerable<TwitterStatus>> GetTweetsAsync()
        {
            TwitterAsyncResult<IEnumerable<TwitterStatus>> tweets = await service
                .ListTweetsOnHomeTimelineAsync(new ListTweetsOnHomeTimelineOptions { Count = 10 });

            if (tweets.Response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(string.Format("{0} [Code: {1}]", tweets.Response.StatusDescription, tweets.Response.StatusCode));

            return tweets.Value;
        }

    }
}
