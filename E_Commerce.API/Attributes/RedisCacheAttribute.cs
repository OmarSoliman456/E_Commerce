using E_Commerce.Application.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Mime;
using System.Text;

namespace E_Commerce.API.Attributes
{
    public class RedisCacheAttribute(int time) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var service = context.HttpContext.RequestServices.GetRequiredService<ICachService>();
            var key = CreateCacheKey(context.HttpContext.Request);
            var value = await service.GetAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
                context.Result = new ContentResult()
                {
                    Content = value,
                    StatusCode = 200,
                    ContentType = "application/json"
                };
                return;
            }
            else
            {
                var executed = await next.Invoke();
                if(executed.Result is OkObjectResult result)
                {
                    await service.SetAsync(key, result, TimeSpan.FromSeconds(time));
                }
            }
        }
        private static string CreateCacheKey(HttpRequest context)
        {
            var key = new StringBuilder();
            key.Append(context.Path);
            foreach (var item in context.Query)
                key.Append($"{item.Key} | {item.Value}");
            return key.ToString();
        }
    }
}
