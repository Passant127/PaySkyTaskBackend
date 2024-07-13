
using Clinic.Application.Contracts;
using Serilog.Context;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace PaySkyTask.API.Middlware;

public class UserScopeMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<UserScopeMiddleware> _logger;

    public UserScopeMiddleware(RequestDelegate next, ILogger<UserScopeMiddleware> logger)
	{
		_next = next;
		_logger = logger;
    }

	public async Task InvokeAsync(HttpContext context, IUserContextService userContext)
	{
		if (context.User.Identity is { IsAuthenticated: true })
		{
			var user = context.User;
            userContext.Email = user.FindFirstValue(ClaimTypes.Email);
            userContext.UserId =user.FindFirstValue(ClaimTypes.NameIdentifier);

            var maskedUsername = Regex.Replace(user.FindFirstValue(ClaimTypes.Name) ?? "", @"^(..).*", m => m.Groups[1].Value + new string('*', user.FindFirstValue(ClaimTypes.Name).Length - 2));
			var maskUserId = Regex.Replace(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "", @"^(..).*", m => m.Groups[1].Value + new string('*', user.FindFirstValue(ClaimTypes.NameIdentifier).Length - 2));
            using (LogContext.PushProperty("UserName", maskedUsername))
            using (LogContext.PushProperty("UserId", maskUserId))
            {
                await _next(context);
            }

        }
		else
		{
			await _next(context);
		}
	}
}
