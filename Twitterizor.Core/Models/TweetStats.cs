using System;
using System.Collections.Generic;

namespace Twitterizor.Core.Models
{
    public class TweetStats
    {
        public UInt64 TotalTweets { get; set; }
        public double TweetsPerHour { get; set; }
        public double TweetsPerMinute { get; set; }
        public double TweetsPerSecond { get; set; }
        public double TweetsWithEmojisPercent { get; set; }
        public double TweetsWithUrlsPercent { get; set; }
        public double TweetsWithPhotoUrlsPercent { get; set; }
        public IEnumerable<CountableElement> TopEmojis { get; set; }
        public IEnumerable<CountableElement> TopDomains { get; set; }
        public IEnumerable<CountableElement> TopHashtags { get; set; }
    }
}
