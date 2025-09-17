namespace DevOrbitAPI.Middleware
{
    public class JwtCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Cookies["Jwt"]; // must match your cookie name

            if (!string.IsNullOrEmpty(token)) //so this checks if the token is empty then if the header has Authorization it simply removes
                                                // it and add this Authorixation Bearer and then token.
            {
                // ✅ Remove existing header if already there
                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Request.Headers.Remove("Authorization");
                }

                context.Request.Headers.Add("Authorization", "Bearer " + token);
            }

            await _next(context);
        }
    }
}
