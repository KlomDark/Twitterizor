namespace Twitterizor.Core
{
    public class TwitterSecrets
    {
        public string ApiKey { get; set; }
        public string ApiSecretKey { get; set; }
        public string BearerToken { get; set; } // Only this is actually used.
    }
}
