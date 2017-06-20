using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MiddleWareFiltersExample.Logger.RequestLoggerMiddleWare;

namespace MiddleWareFiltersExample.Logger
{
    /// <summary>
    /// Provides extension methods to the <see cref="IApplicationBuilder"/> class which make it easy
    /// to configure the <see cref="RequestLoggerMiddleware"/>.
    /// </summary>
    public static class RequestLoggerExtensions
    {           
        /// <summary>
        /// Adds the <see cref="RequestLoggerMiddleware"/> to the application.
        /// </summary>
        /// <param name="builder">
        /// The application to which to add the <see cref="RequestLoggerMiddleware"/>.
        /// </param>
        /// <returns>
        /// The updated <see cref="IApplicationBuilder"/>.
        /// </returns>
        public static IApplicationBuilder UseRequestLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggerMiddleware>();
        }
    }
}
