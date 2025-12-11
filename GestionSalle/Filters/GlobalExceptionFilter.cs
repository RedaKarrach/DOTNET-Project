using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace GestionSalle.Filters
{
 public class GlobalExceptionFilter : IExceptionFilter
 {
 private readonly ILogger<GlobalExceptionFilter> _logger;
 public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) { _logger = logger; }
 public void OnException(ExceptionContext context)
 {
 _logger.LogError(context.Exception, "Unhandled exception");
 context.Result = new RedirectToActionResult("Error", "Home", null);
 context.ExceptionHandled = true;
 }
 }
}
