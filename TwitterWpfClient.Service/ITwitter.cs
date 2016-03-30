using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TweetSharp;

namespace TwitterWpfClient.Service
{
    public interface ITwitter
    {
        Uri GetAuthorizationUri();
        Task<Uri> GetAuthorizationUriAsync();
        OAuthAccessToken GetAccessToken(string verifier);
        Task<OAuthAccessToken> GetAccessTokenAsync(string verifier);
        bool AuthetnticateWithToken(string token, string tokenSecret);
        IEnumerable<TwitterStatus> GetTweets();
        Task<IEnumerable<TwitterStatus>> GetTweetsAsync();
    }
}