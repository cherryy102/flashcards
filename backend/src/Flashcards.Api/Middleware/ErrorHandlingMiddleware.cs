using Flashcards.Domain.Exceptions;

namespace Flashcards.Api.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
		private readonly ILogger<ErrorHandlingMiddleware> logger;

		public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
		{
			this.logger = logger;
		}
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
			try
			{
				await next.Invoke(context);
			}
			catch(NotFoundException ex)
			{
				context.Response.StatusCode = 400;
                logger.LogWarning(ex.Message);
                await context.Response.WriteAsJsonAsync(new { 
					Status = 400,
					Message = ex.Message 
				});
			}
            catch (CustomException ex)
            {
                context.Response.StatusCode = 400;
                logger.LogWarning(ex.Message);
                await context.Response.WriteAsJsonAsync(new
                {
                    Status = 400,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
			{	
				context.Response.StatusCode = 500;
				logger.LogError(ex.Message);
				await context.Response.WriteAsJsonAsync(new
                {
                    Status = 500,
                    Message = "Something went wrong"
                });

			}
        }
    }
}
