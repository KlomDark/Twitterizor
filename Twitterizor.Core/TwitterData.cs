using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Twitterizor.Core.Models;

namespace Twitterizor.Core
{
    public sealed class TwitterData
    {
        #region Constants
        private const string EmojiRegex = "(?:0\x20E3|1\x20E3|2\x20E3|3\x20E3|4\x20E3|5\x20E3|6\x20E3|7\x20E3|8\x20E3|9\x20E3|#\x20E3|\\*\x20E3|\xD83C(?:\xDDE6\xD83C(?:\xDDE8|\xDDE9|\xDDEA|\xDDEB|\xDDEC|\xDDEE|\xDDF1|\xDDF2|\xDDF4|\xDDF6|\xDDF7|\xDDF8|\xDDF9|\xDDFA|\xDDFC|\xDDFD|\xDDFF)|\xDDE7\xD83C(?:\xDDE6|\xDDE7|\xDDE9|\xDDEA|\xDDEB|\xDDEC|\xDDED|\xDDEE|\xDDEF|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF6|\xDDF7|\xDDF8|\xDDF9|\xDDFB|\xDDFC|\xDDFE|\xDDFF)|\xDDE8\xD83C(?:\xDDE6|\xDDE8|\xDDE9|\xDDEB|\xDDEC|\xDDED|\xDDEE|\xDDF0|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF5|\xDDF7|\xDDFA|\xDDFB|\xDDFC|\xDDFD|\xDDFE|\xDDFF)|\xDDE9\xD83C(?:\xDDEA|\xDDEC|\xDDEF|\xDDF0|\xDDF2|\xDDF4|\xDDFF)|\xDDEA\xD83C(?:\xDDE6|\xDDE8|\xDDEA|\xDDEC|\xDDED|\xDDF7|\xDDF8|\xDDF9|\xDDFA)|\xDDEB\xD83C(?:\xDDEE|\xDDEF|\xDDF0|\xDDF2|\xDDF4|\xDDF7)|\xDDEC\xD83C(?:\xDDE6|\xDDE7|\xDDE9|\xDDEA|\xDDEB|\xDDEC|\xDDED|\xDDEE|\xDDF1|\xDDF2|\xDDF3|\xDDF5|\xDDF6|\xDDF7|\xDDF8|\xDDF9|\xDDFA|\xDDFC|\xDDFE)|\xDDED\xD83C(?:\xDDF0|\xDDF2|\xDDF3|\xDDF7|\xDDF9|\xDDFA)|\xDDEE\xD83C(?:\xDDE8|\xDDE9|\xDDEA|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF6|\xDDF7|\xDDF8|\xDDF9)|\xDDEF\xD83C(?:\xDDEA|\xDDF2|\xDDF4|\xDDF5)|\xDDF0\xD83C(?:\xDDEA|\xDDEC|\xDDED|\xDDEE|\xDDF2|\xDDF3|\xDDF5|\xDDF7|\xDDFC|\xDDFE|\xDDFF)|\xDDF1\xD83C(?:\xDDE6|\xDDE7|\xDDE8|\xDDEE|\xDDF0|\xDDF7|\xDDF8|\xDDF9|\xDDFA|\xDDFB|\xDDFE)|\xDDF2\xD83C(?:\xDDE6|\xDDE8|\xDDE9|\xDDEA|\xDDEB|\xDDEC|\xDDED|\xDDF0|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF5|\xDDF6|\xDDF7|\xDDF8|\xDDF9|\xDDFA|\xDDFB|\xDDFC|\xDDFD|\xDDFE|\xDDFF)|\xDDF3\xD83C(?:\xDDE6|\xDDE8|\xDDEA|\xDDEB|\xDDEC|\xDDEE|\xDDF1|\xDDF4|\xDDF5|\xDDF7|\xDDFA|\xDDFF)|\xDDF4\xD83C\xDDF2|\xDDF5\xD83C(?:\xDDE6|\xDDEA|\xDDEB|\xDDEC|\xDDED|\xDDF0|\xDDF1|\xDDF2|\xDDF3|\xDDF7|\xDDF8|\xDDF9|\xDDFC|\xDDFE)|\xDDF6\xD83C\xDDE6|\xDDF7\xD83C(?:\xDDEA|\xDDF4|\xDDF8|\xDDFA|\xDDFC)|\xDDF8\xD83C(?:\xDDE6|\xDDE7|\xDDE8|\xDDE9|\xDDEA|\xDDEC|\xDDED|\xDDEE|\xDDEF|\xDDF0|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF7|\xDDF8|\xDDF9|\xDDFB|\xDDFD|\xDDFE|\xDDFF)|\xDDF9\xD83C(?:\xDDE6|\xDDE8|\xDDE9|\xDDEB|\xDDEC|\xDDED|\xDDEF|\xDDF0|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF7|\xDDF9|\xDDFB|\xDDFC|\xDDFF)|\xDDFA\xD83C(?:\xDDE6|\xDDEC|\xDDF2|\xDDF8|\xDDFE|\xDDFF)|\xDDFB\xD83C(?:\xDDE6|\xDDE8|\xDDEA|\xDDEC|\xDDEE|\xDDF3|\xDDFA)|\xDDFC\xD83C(?:\xDDEB|\xDDF8)|\xDDFD\xD83C\xDDF0|\xDDFE\xD83C(?:\xDDEA|\xDDF9)|\xDDFF\xD83C(?:\xDDE6|\xDDF2|\xDDFC)))|[\xA9\xAE\x203C\x2049\x2122\x2139\x2194-\x2199\x21A9\x21AA\x231A\x231B\x2328\x23CF\x23E9-\x23F3\x23F8-\x23FA\x24C2\x25AA\x25AB\x25B6\x25C0\x25FB-\x25FE\x2600-\x2604\x260E\x2611\x2614\x2615\x2618\x261D\x2620\x2622\x2623\x2626\x262A\x262E\x262F\x2638-\x263A\x2648-\x2653\x2660\x2663\x2665\x2666\x2668\x267B\x267F\x2692-\x2694\x2696\x2697\x2699\x269B\x269C\x26A0\x26A1\x26AA\x26AB\x26B0\x26B1\x26BD\x26BE\x26C4\x26C5\x26C8\x26CE\x26CF\x26D1\x26D3\x26D4\x26E9\x26EA\x26F0-\x26F5\x26F7-\x26FA\x26FD\x2702\x2705\x2708-\x270D\x270F\x2712\x2714\x2716\x271D\x2721\x2728\x2733\x2734\x2744\x2747\x274C\x274E\x2753-\x2755\x2757\x2763\x2764\x2795-\x2797\x27A1\x27B0\x27BF\x2934\x2935\x2B05-\x2B07\x2B1B\x2B1C\x2B50\x2B55\x3030\x303D\x3297\x3299]|\xD83C[\xDC04\xDCCF\xDD70\xDD71\xDD7E\xDD7F\xDD8E\xDD91-\xDD9A\xDE01\xDE02\xDE1A\xDE2F\xDE32-\xDE3A\xDE50\xDE51\xDF00-\xDF21\xDF24-\xDF93\xDF96\xDF97\xDF99-\xDF9B\xDF9E-\xDFF0\xDFF3-\xDFF5\xDFF7-\xDFFF]|\xD83D[\xDC00-\xDCFD\xDCFF-\xDD3D\xDD49-\xDD4E\xDD50-\xDD67\xDD6F\xDD70\xDD73-\xDD79\xDD87\xDD8A-\xDD8D\xDD90\xDD95\xDD96\xDDA5\xDDA8\xDDB1\xDDB2\xDDBC\xDDC2-\xDDC4\xDDD1-\xDDD3\xDDDC-\xDDDE\xDDE1\xDDE3\xDDEF\xDDF3\xDDFA-\xDE4F\xDE80-\xDEC5\xDECB-\xDED0\xDEE0-\xDEE5\xDEE9\xDEEB\xDEEC\xDEF0\xDEF3]|\xD83E[\xDD10-\xDD18\xDD80-\xDD84\xDDC0]";
        private const string UrlRegex = @"http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?";
        private const string HashtagRegex = @"(?<=#)\w+";
        #endregion Constants

        #region Class Infrastructure
        /// <summary>
        /// Thread-Safe Lazy Singleton (See https://csharpindepth.com/articles/singleton#lazy for more info)
        /// </summary>
        private static readonly Lazy<TwitterData> lazy = new Lazy<TwitterData>(() => new TwitterData(), true); // The "true" parameter makes this object thread-safe.

        /// <summary>
        /// The instance object for external code to access.
        /// </summary>
        public static TwitterData Instance { get { return lazy.Value; } }
        #endregion Class Infrastructure

        #region Public Methods
        public void AnalyzeTweet(Tweet tweet)
        {
            Instance.TotalTweets++;
            Instance.TweetTracker.Add(DateTime.Now);
            Instance.ExtractEmojis(tweet.data.text);
            Instance.ExtractHashtags(tweet.data.text);
            Instance.ExtractUrls(tweet.data.text);
        }

        public void Reset()
        {
            TotalTweets = 0;
            TweetsWithEmojis = 0;
            TweetsWithHashTags = 0;
            TweetsWithUrls = 0;
            TweetsWithPhotoUrls = 0;
            StartTime = DateTime.Now;
            TweetTracker = new BlockingCollection<DateTime>();
            TopEmojis = new List<CountableElement>();
            TopHashtags = new List<CountableElement>();
            TopUrlDomains = new List<CountableElement>();
        }
        #endregion Public Methods

        #region Properties
        /// <summary>
        /// Total Number of Tweets Received
        /// </summary>
        public UInt64 TotalTweets { get; private set; }

        /// <summary>
        /// Count of Tweets that contain Emojis
        /// </summary>
        public UInt64 TweetsWithEmojis { get; private set; }

        /// <summary>
        /// Count of Tweets that contain Hashtags
        /// </summary>
        public UInt64 TweetsWithHashTags { get; private set; }

        /// <summary>
        /// What time did this code start running?
        /// </summary>
        public DateTime StartTime { get; private set; } = DateTime.Now;

        /// <summary>
        /// Average Tweets Per Hour
        /// </summary>
        public double AverageTweetsPerHour
        {
            get
            {
                var elapsedTime = DateTime.Now - StartTime;
                var totalHours = elapsedTime.TotalSeconds / 60.0 / 60.0;
                var average = TweetTracker.Count / totalHours;

                return average;
            }
        }

        /// <summary>
        /// Average Tweets Per Minute
        /// </summary>
        public double AverageTweetsPerMinute
        {
            get
            {
                var elapsedTime = DateTime.Now - StartTime;
                var totalMinutes = elapsedTime.TotalSeconds / 60.0;
                var average = TweetTracker.Count / totalMinutes;

                return average;
            }
        }

        /// <summary>
        /// Average Tweets per Second
        /// </summary>
        public double AverageTweetsPerSecond
        {
            get
            {
                var totalSeconds = (DateTime.Now - StartTime).TotalSeconds;
                var average = TweetTracker.Count / totalSeconds;

                return average;
            }
        }

        /// <summary>
        /// Tracks the timestamp of each received tweet. Used to determine "Average Tweets Per x" values.
        /// </summary>
        public BlockingCollection<DateTime> TweetTracker { get; private set; } = new BlockingCollection<DateTime>();

        // NOTE: So why not KeyValuePair<int, string> instead of CountableElement? Because I needed a nullable object. Cannot have a null KVP so had to come up with another approach.
        /// <summary>
        /// Top Emojis
        /// </summary>
        public List<CountableElement> TopEmojis { get; private set; } = new List<CountableElement>();

        /// <summary>
        /// Top Hashtags
        /// </summary>
        public List<CountableElement> TopHashtags { get; private set; } = new List<CountableElement>();

        /// <summary>
        /// The top Domains encountered in Tweets with Urls.
        /// </summary>
        public List<CountableElement> TopUrlDomains { get; private set; } = new List<CountableElement>();

        /// <summary>
        /// Number of Tweets that contain a URL.
        /// </summary>
        public int TweetsWithUrls { get; private set; }

        /// <summary>
        /// Number of Tweets that contain a Photo URL.
        /// </summary>
        public int TweetsWithPhotoUrls { get; private set; }
        #endregion Properties

        #region Private Methods
        private void ExtractEmojis(string text)
        {
            var foundFirst = false; // Update TweetsWithEmojis only on first Emoji found in a tweet

            foreach (Match match in Regex.Matches(text, EmojiRegex))
            {
                if (foundFirst == false)
                {
                    TweetsWithEmojis++;
                    foundFirst = true;
                }

                var matchingElement = TopEmojis.FirstOrDefault(x => x.Element == match.Value);
                AddOrUpdateEmojiMatch(match.Value, matchingElement);
            }
        }

        private void AddOrUpdateEmojiMatch(string curEmoji, CountableElement matchingElement)
        {
            if (matchingElement == null)
            {
                var newElement = new CountableElement
                {
                    Count = 1,
                    Element = curEmoji
                };

                TopEmojis.Add(newElement);
            }
            else
            {
                matchingElement.Count++;
            }
        }

        private void ExtractUrls(string text)
        {
            var foundFirstUrl = false; // Update TweetsWithUrls only on first Url found in a tweet
            var foundFirstPhotoUrl = false; // Update TweetsWithUrls only on first Photo Url found in a tweet

            foreach (Match match in Regex.Matches(text, UrlRegex))
            {
                if (foundFirstUrl == false)
                {
                    TweetsWithUrls++;
                    foundFirstUrl = true;
                }

                if (foundFirstPhotoUrl == false &&
                    (
                        match.Value.Contains(".gif", StringComparison.CurrentCultureIgnoreCase) ||
                        match.Value.Contains(".jpg", StringComparison.CurrentCultureIgnoreCase) ||
                        match.Value.Contains(".png", StringComparison.CurrentCultureIgnoreCase) ||
                        match.Value.Contains("t.co/", StringComparison.CurrentCultureIgnoreCase) ||
                        match.Value.Contains("pic.twitter", StringComparison.CurrentCultureIgnoreCase) ||
                        match.Value.Contains("instagram.com", StringComparison.CurrentCultureIgnoreCase))
                    )
                {
                    TweetsWithPhotoUrls++;
                    foundFirstPhotoUrl = true;
                }

                if (Uri.TryCreate(match.Value, uriKind: UriKind.Absolute, out Uri uri))
                {
                    var domain = uri.DnsSafeHost;

                    var matchingElement = TopUrlDomains.FirstOrDefault(x => x.Element == domain);
                    AddOrUpdateUrlDomainMatch(domain, matchingElement);
                }
            }
        }

        private void AddOrUpdateUrlDomainMatch(string domain, CountableElement matchingElement)
        {
            // Ignore partial versions of t.co
            if (domain == "t" || domain == "t." || domain == "t.c")
                return;

            if (matchingElement == null)
            {
                var newElement = new CountableElement
                {
                    Count = 1,
                    Element = domain
                };

                TopUrlDomains.Add(newElement);
            }
            else
            {
                matchingElement.Count++;
            }
        }

        private void ExtractHashtags(string text)
        {
            foreach (Match match in Regex.Matches(text, HashtagRegex))
            {
                var matchingElement = TopHashtags.FirstOrDefault(x => x.Element == match.Value);
                AddOrUpdateHashtagMatch(match.Value, matchingElement);
            }
        }

        private void AddOrUpdateHashtagMatch(string hashtag, CountableElement matchingElement)
        {
            if (matchingElement == null)
            {
                var newElement = new CountableElement
                {
                    Count = 1,
                    Element = hashtag
                };

                TopHashtags.Add(newElement);
            }
            else
            {
                matchingElement.Count++;
            }
        }
        #endregion Private Methods
    }
}
