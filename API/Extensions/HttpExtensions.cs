//namespace API.Extensions
//{
//    public static class HttpExtensions
//    {
//        public static void AppendCookie(this HttpResponse response, string key, string value, int expirySeconds = 0)
//        {
//            var option = new CookieOptions
//            {
//                HttpOnly = true,
//                Secure = true,
//                SameSite = SameSiteMode.Strict,
//                Expires = DateTime.UtcNow.AddMinutes(3)
//            };
//            //if (expirySeconds.HasValue)
//            //{
//            //    option.Expires = DateTime.UtcNow.AddSeconds(expirySeconds.Value);
//            //}
//            response.Cookies.Append(key, value, option);
//        }
//    }
//}
