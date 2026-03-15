using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CareRepositories.Interfaces;

namespace HealthcareApp.Filters
{
    public class SessionAuthAttribute : ActionFilterAttribute
    {
        private readonly string _requiredRole;

        public SessionAuthAttribute(string requiredRole = "")
        {
            _requiredRole = requiredRole;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sessionRepository = context.HttpContext.RequestServices.GetRequiredService<ISessionRepository>();
            var sessionId = context.HttpContext.Request.Cookies["SessionID"];

            if (string.IsNullOrEmpty(sessionId) || !sessionRepository.IsValid(sessionId))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            var session = sessionRepository.GetByIdWithUser(sessionId);
            if (session == null)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            // Check role if specified
            if (!string.IsNullOrEmpty(_requiredRole) && session.Role != _requiredRole)
            {
                context.Result = new ForbidResult();
                return;
            }

            // Store user info in ViewBag for easy access in Views
            if (context.Controller is Controller controller)
            {
                controller.ViewBag.CurrentUser = session.User;
                controller.ViewBag.UserRole = session.Role;
            }

            base.OnActionExecuting(context);
        }
    }
}
