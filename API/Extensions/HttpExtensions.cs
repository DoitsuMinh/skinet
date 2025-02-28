using static API.Enums.TimeUnits;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AppendCookie(this HttpResponse response, string key, string value, TimeUnit timeUnit, int expiryValue)
        {
            var option = new CookieOptions
            {
                
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            };
            switch (timeUnit)
            {
                case TimeUnit.Seconds:
                    option.Expires = DateTime.UtcNow.AddSeconds(expiryValue);
                    break;
                case TimeUnit.Minutes:
                    option.Expires = DateTime.UtcNow.AddMinutes(expiryValue);
                    break;
                case TimeUnit.Hours:
                    option.Expires = DateTime.UtcNow.AddHours(expiryValue);
                    break;
                case TimeUnit.Days:
                    option.Expires = DateTime.UtcNow.AddDays(expiryValue);
                    break;
            }
            response.Cookies.Append(key, value, option);
        }
    }
}
