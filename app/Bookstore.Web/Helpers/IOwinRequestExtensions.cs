using Microsoft.AspNetCore.Http;

namespace Bookstore.Web.Helpers
{
    public static class HttpContextExtensions
    {
        public static string GetReturnUrl(this IHttpContextAccessor httpContextAccessor)
        {
            var request = httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}/signin-oidc";
        }
    }
}