using System.Security.Claims;
namespace Plugins
{
    public class ApiKeyMiddleware
    {
        private const string ApiKeyHeaderName = "X-API-Key";
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context
        //, IEmpresaRepository empresaRepo
        )
        {
            var path = context.Request.Path;

            // Apenas ativa o middleware em rotas específicas (ex: /api/empresa)
            if (path.StartsWithSegments("/empresa"))
            {
                if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var providedKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("API Key ausente");
                    return;
                }

                var empresa = new { Id = 1 }; // await empresaRepo.GetByApiKeyAsync(providedKey);
                if (empresa == null)
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("API Key inválida");
                    return;
                }

                // Opcional: adicionar claims para futura autorização
                var claims = new[] { new Claim("EmpresaId", empresa.Id.ToString()) };
                var identity = new ClaimsIdentity(claims, "ApiKey");
                context.User = new ClaimsPrincipal(identity);
            }

            await _next(context);
        }
    }
}

