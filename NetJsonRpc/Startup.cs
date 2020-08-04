using System;
using System.IO;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using iText.Html2pdf;
using iText.Kernel.Pdf;

namespace NetJsonRpc
{
    public class Startup
    {
        private ILogger<Startup> _logger;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDistributedMemoryCache();

            services
                .AddSession();

            services
                .AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    .AddSessionStateTempDataProvider()
                    .AddRazorPagesOptions(options =>
                    {
                        options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Initialize logger
            loggerFactory.AddFile(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\log\\NetJsonRpc.log");

            this._logger = loggerFactory.CreateLogger<Startup>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();

            var defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");

            app.UseDefaultFiles(defaultFilesOptions);

            app.UseStaticFiles();

            app.Map("/report", HandleReport);

            app.UseMvc();
        }

        private void HandleReport(IApplicationBuilder app)
        {
            // Intermediate processing example...
            app.Use(async (context, next) =>
            {
                var rows = context.Request.Query["rows"];
                var cols = context.Request.Query["cols"];

                this._logger.LogInformation("### /report rows=" + rows + ",cols=" + cols + "...");

                await next.Invoke();
            });

            app.Run(async context =>
            {
                var rows = Convert.ToInt32(context.Request.Query["rows"]);
                var cols = Convert.ToInt32(context.Request.Query["cols"]);

                if (rows < 1) rows = 10;
                if (cols < 1) cols = 10;

                var html = "<html><body>";
                html += "<h2 style=\"text-align:center;\">Report example</h2>";
                html += "<br>";
                html += "<hr>";
                html += "<br>";
                html += "<table border=\"1\" style=\"border:1px solid #888888;border-collapse:collapse;width:100%;\">";
                for (int r = 0; r < rows; r++)
                {
                    if(r % 2 == 0)
                    {
                        html += "<tr style=\"background-color:#eeeeee;\">";
                    }
                    else
                    {
                        html += "<tr>";
                    }
                    for (int c = 0; c < cols; c++)
                    {
                        html += "<td>" + r + "," + c + "</td>";
                    }
                    html += "</tr>";
                }
                html += "</table></body></html>";

                var memoryStream = new MemoryStream();
                using (var pdfWriter = new PdfWriter(memoryStream))
                {
                    pdfWriter.SetCloseStream(false);
                    using (var document = HtmlConverter.ConvertToDocument(html, pdfWriter))
                    {
                    }
                }
                memoryStream.Position = 0;
                byte[] buffer = memoryStream.GetBuffer();

                // context.Response.Headers.Add("Content-Disposition", "attachment; filename=report.pdf");
                context.Response.ContentType = "application/pdf";
                context.Response.ContentLength = buffer.Length;

                await context.Response.Body.WriteAsync(buffer);
            });
        }
    }
}
