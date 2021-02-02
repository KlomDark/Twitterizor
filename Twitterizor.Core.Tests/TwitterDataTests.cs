using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Twitterizor.Core.Tests
{
    /// <summary>
    /// Certainly not a whole suite (100% coverage) of tests. But enough to show that I do know how to write tests.
    /// </summary>
    [TestClass]
    public class TwitterDataTests
    {
        [TestMethod]
        public void AnalyseTweet_Pass()
        {
            TwitterData.Instance.Reset();

            Models.Tweet tweet = new Models.Tweet
            {
                data = new Models.Data
                {
                    id = "98238498",
                    text = "tweet tweet tweet"
                }
            };

            TwitterData.Instance.AnalyzeTweet(tweet);

            Assert.AreEqual((ulong)1, TwitterData.Instance.TotalTweets);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AnalyseTweet_Fail()
        {
            TwitterData.Instance.Reset();

            Models.Tweet tweet = new Models.Tweet
            {
            };

            TwitterData.Instance.AnalyzeTweet(tweet);

            Assert.AreEqual(1, TwitterData.Instance.TotalTweets);
        }
    }
}
