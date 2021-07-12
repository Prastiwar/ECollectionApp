using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI
{
    public static class HttpExtensions
    {
        public static async Task AddTokenAsync(this HttpRequestMessage request, HttpContext context, string scheme)
        {
            string token = await context.GetTokenAsync(scheme);
            request.Headers.Add(scheme, token);
        }

        public static Task AddJwtTokenAsync(this HttpRequestMessage request, HttpContext context) 
            => AddTokenAsync(request, context, JwtBearerDefaults.AuthenticationScheme);
    }
}
