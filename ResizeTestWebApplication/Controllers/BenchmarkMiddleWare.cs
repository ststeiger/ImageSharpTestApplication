
namespace ResizeTestWebApplication
{


    public class BenchmarkMiddleWare
    {

        private readonly Microsoft.AspNetCore.Http.RequestDelegate _next;
        public BenchmarkMiddleWare(Microsoft.AspNetCore.Http.RequestDelegate next)
        {
            _next = next;
        }


        public async System.Threading.Tasks.Task Invoke(Microsoft.AspNetCore.Http.HttpContext context)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            context.Response.OnStarting(delegate (object state)
            {
                sw.Stop();

                string elapsed = sw.Elapsed.ToString("h':'mm':'ss'.'fffffff"
                    , System.Globalization.CultureInfo.InvariantCulture
                );

                // context.Response.Headers.Add("X-Elapsed-Time", sw.ElapsedTicks.ToString());
                // context.Response.Headers.Add("X-Elapsed-Time", elapsed);
                context.Response.Headers["X-Elapsed-Time"] = elapsed;

                return System.Threading.Tasks.Task.FromResult(0);
            }, null);

            await _next.Invoke(context);
        } // End Function Invoke 


    } // End Class BenchmarkMiddleWare 


} // End Namespace ResizeTestWebApplication 
