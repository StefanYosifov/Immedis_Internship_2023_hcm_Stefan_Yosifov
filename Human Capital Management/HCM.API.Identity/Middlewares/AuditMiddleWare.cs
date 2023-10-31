namespace HCM.API.Middlewares
{
    using System.Security.Claims;

    using Common.Manager;
    using Data.Models;
    using HCM.Data;
    using Microsoft.AspNetCore.Http;
    using System.Text;
    using Task = System.Threading.Tasks.Task;

    public class AuditMiddleWare
    {
        private const string ControllerKey = "controller";
        private const string IdKey = "id";
        private readonly RequestDelegate next;

        public AuditMiddleWare(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
        {

            var request = context.Request;

            if (request.Path.ToString().Contains("/api"))
            {
                request.RouteValues.TryGetValue(ControllerKey, out var ControllerValue);

                var controllerName = ControllerValue!.ToString() ?? string.Empty;

                StringBuilder sb = new StringBuilder();

                if (request.Method == "GET")
                {
                    return;
                }

                switch (request.Method)
                {
                    case "POST":
                    case "PUT":
                        request.Body.Position = 0;
                        var reader = new StreamReader(request.Body, Encoding.UTF8);
                        var requestBody = await reader.ReadToEndAsync().ConfigureAwait(false);
                        request.Body.Position = 0;
                        sb.Append(requestBody);
                        break;
                    case "DELETE":
                        request.RouteValues.TryGetValue(IdKey, out var idValueObj);
                        sb.Append((string?)idValueObj ?? string.Empty);
                        break;
                }

                var auditLog = new AuditLog()
                {
                    
                    EntityName = controllerName,
                    Action = request.Method,
                    Timestamp = DateTime.UtcNow,
                    Changes = sb.ToString(),
                };

                dbContext.AuditLogs.Add(auditLog);
                await dbContext.SaveChangesAsync();
            }

        }
    }
}
