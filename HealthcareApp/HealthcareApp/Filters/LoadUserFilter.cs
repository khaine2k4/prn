using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CareRepositories.Interfaces;

namespace HealthcareApp.Filters
{
    public class LoadUserFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var sessionRepository = context.HttpContext.RequestServices.GetService<ISessionRepository>();
            if (sessionRepository != null)
            {
                var sessionId = context.HttpContext.Request.Cookies["SessionID"];
                if (!string.IsNullOrEmpty(sessionId) && sessionRepository.IsValid(sessionId))
                {
                    var session = sessionRepository.GetByIdWithUser(sessionId);
                    if (session != null && context.Controller is Controller controller)
                    {
                        controller.ViewBag.CurrentUser = session.User;
                        controller.ViewBag.UserRole = session.Role;
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
