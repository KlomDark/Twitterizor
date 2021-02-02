using System;
using Microsoft.Extensions.Options;

namespace Twitterizor.Core
{
    public class SecretRevealer : ISecretRevealer
    {
        private readonly TwitterSecrets _secrets;

        public SecretRevealer(IOptions<TwitterSecrets> secrets)
        {
            _secrets = secrets.Value ?? throw new ArgumentNullException(nameof(secrets));
        }

        public TwitterSecrets GetTwitterSecrets()
        {
            return _secrets;
        }
    }
}
