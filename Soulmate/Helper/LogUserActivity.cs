using Microsoft.AspNetCore.Mvc.Filters;
using Soulmate.Extensions;
using SoulmateDAL.Interfaces;

namespace Soulmate.Helper
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            if (context.HttpContext.User.Identity?.IsAuthenticated != true) return;
            var userId = resultContext.HttpContext.User.GetUserId();
            var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUsersDAL>();
            var user = await repo.GetUserById(userId);
            if (user == null) return;
            user.LastActive = DateTime.UtcNow;
            await repo.SaveAllAsync();
        }
    }
}
