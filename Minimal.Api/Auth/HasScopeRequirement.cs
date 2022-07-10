using Microsoft.AspNetCore.Authorization;

namespace Minimal.Api.Auth
{
    public class HasScopeRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public string Scope { get; }
        public HasScopeRequirement(string issuer, string scope)
        {
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }
    }
}
