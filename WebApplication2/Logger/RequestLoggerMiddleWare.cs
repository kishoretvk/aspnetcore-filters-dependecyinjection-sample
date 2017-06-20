using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiddleWareFiltersExample.Logger
{
    public class RequestLoggerMiddleWare
    {

        /// <summary>
        /// Logs all Web API requests.
        /// </summary>
        public class RequestLoggerMiddleware
        {
            /// <summary>
            /// The next middleware in the call chain.
            /// </summary>
            private readonly RequestDelegate next;

            /// <summary>
            /// Initializes a new instance of the <see cref="RequestLoggerMiddleware"/> class.
            /// </summary>
            /// <param name="next">
            /// The next middleware in the call chain.
            /// </param>
            public RequestLoggerMiddleware(RequestDelegate next)
            {
                this.next = next;
            }

            /// <summary>
            /// Invokes the <see cref="RequestLoggerMiddleware"/> on the current <see cref="HttpContext"/>
            /// </summary>
            /// <param name="context">
            /// The <see cref="HttpContext"/> in which to execute the middleware.
            /// </param>
            /// <returns>
            /// A <see cref="Task"/> which represents the asynchronous operation.
            /// </returns>
            public async Task Invoke(HttpContext context)
            {
                var request = context.Request;
                var path = request.Path;
                ResponseLoggerStream responseLoggerStream = null;
                RequestLoggerStream requestLoggerStream = null;

                // Only trace JSON requests
                bool shouldTrace =
                    context.Request.ContentType != null
                    && context.Request.ContentType.StartsWith("application/json");

                if (shouldTrace)
                {
                    responseLoggerStream = new ResponseLoggerStream(context.Response.Body, ownsParent: false);
                    context.Response.Body = responseLoggerStream;

                    requestLoggerStream = new RequestLoggerStream(context.Request.Body, ownsParent: false);
                    context.Request.Body = requestLoggerStream;
                }

                try
                {
                    // Invoke the other requests first.
                    await this.next.Invoke(context);

                    // The response type is available now, also filter on response type - only JSON
                    shouldTrace = shouldTrace
                        && context.Response.ContentType != null
                        && context.Response.ContentType.StartsWith("application/json");
                    

                    if (shouldTrace)
                    {
                        MemoryStream requestTracerStream = requestLoggerStream.TracerStream;

                        var requestData = requestTracerStream.ToArray();
                        string requestContent = Encoding.UTF8.GetString(requestData);

                        // The requestContent string will contain the original request data

                        MemoryStream responseTracerStream = responseLoggerStream.TracerStream;

                        var responseData = responseTracerStream.ToArray();
                        string responseContent = Encoding.UTF8.GetString(responseData);
                    }
                }
                finally
                {
                    // Remove any evidence of us having done tracing :). The stream to which the response is sent, and from which
                    // the requests are read, is the underlying TCP stream, and can be re-used for multiple requests. So we want
                    // to remove ourselves from the stream, to prevent the next request from still having our tracing attached
                    // to it.
                    if (responseLoggerStream != null)
                    {
                        context.Response.Body = responseLoggerStream.Inner;
                        responseLoggerStream.Dispose();
                    }

                    if (requestLoggerStream != null)
                    {
                        context.Request.Body = requestLoggerStream.Inner;
                        requestLoggerStream.Dispose();
                    }
                }
            }
        }

    }


}