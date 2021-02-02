namespace Twitterizor.Core
{
    public interface ISecretRevealer
    {
        TwitterSecrets GetTwitterSecrets();
    }
}
