using Microsoft.AspNetCore.Http;

namespace Bookstore.Web.Helpers
{
    public static class OwinRequestExtensions
    {
        public static string GetReturnUrl(this HttpContext context)
        {
            var request = context.Request;
            return $"{request.Scheme}://{request.Host}/signin-oidc";
        }
    }
}