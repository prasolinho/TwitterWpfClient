using System;

namespace TwitterWpfClient.Models
{
    /// <summary>
    /// Informacje o pojedynczym tweecie
    /// </summary>
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

        public bool HasMentions { get; set; }
        public Mention[] Mentions { get; set; }

        public bool ContentModified { get; set; }
    }
}
