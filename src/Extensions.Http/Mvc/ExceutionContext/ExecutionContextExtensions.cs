using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Http.Mvc
{
    public static class ExecutionContextExtensions
    {
        public static void AddExecutionContext(this IServiceCollection services)
        {
            services.AddScoped<IExecutionContext, CurrentContext>();
        }
    }
}
