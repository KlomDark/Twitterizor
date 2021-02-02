using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Twitterizor.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetDataController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                double tweetsWithEmojisPercent = 0.0;
                double tweetsWithUrlsPercent = 0.0;
                double tweetsWithPhotoUrlsPercent = 0.0;

                if (Core.TwitterData.Instance.TotalTweets > 0)
                {
                    tweetsWithEmojisPercent = Core.TwitterData.Instance.TweetsWithEmojis / (double)Core.TwitterData.Instance.TotalTweets;
                    tweetsWithUrlsPercent = Core.TwitterData.Instance.TweetsWithUrls / (double)Core.TwitterData.Instance.TotalTweets;
                    tweetsWithPhotoUrlsPercent = Core.TwitterData.Instance.TweetsWithPhotoUrls / (double)Core.TwitterData.Instance.TotalTweets;
                }

                var topEmojis = Core.TwitterData.Instance.TopEmojis.OrderByDescending(x => x.Count).Take(10);
                var topDomains = Core.TwitterData.Instance.TopUrlDomains.OrderByDescending(x => x.Count).Take(10);
                var topHashTags = Core.TwitterData.Instance.TopHashtags.OrderByDescending(x => x.Count).Take(10);

                var tweetStats = new Core.Models.TweetStats
                {
                    TotalTweets = Core.TwitterData.Instance.TotalTweets,
                    TweetsPerHour = Core.TwitterData.Instance.AverageTweetsPerHour,
                    TweetsPerMinute = Core.TwitterData.Instance.AverageTweetsPerMinute,
                    TweetsPerSecond = Core.TwitterData.Instance.AverageTweetsPerSecond,
                    TopEmojis = topEmojis,
                    TweetsWithEmojisPercent = tweetsWithEmojisPercent,
                    TweetsWithUrlsPercent = tweetsWithUrlsPercent,
                    TweetsWithPhotoUrlsPercent = tweetsWithPhotoUrlsPercent,
                    TopDomains = topDomains,
                    TopHashtags = topHashTags
                };

                return Ok(tweetStats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post() // Planned to have several command parameters here, but ran out of time.
        {
            Core.TwitterData.Instance.Reset();

            return Ok(0);
        }
    }
}
