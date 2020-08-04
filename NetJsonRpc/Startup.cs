using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NetJsonRpc.Auth;

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

            app.Map("/login", HandleLogin);

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseMvc();
        }

        private void HandleLogin(IApplicationBuilder app)
        {
            // Intermediate processing example...
            app.Use(async (context, next) =>
            {
                var username = context.Request.Query["username"];

                this._logger.LogInformation("### /login username=" + username + "...");

                await next.Invoke();
            });

            app.Run(async context =>
            {
                var username = context.Request.Query["username"];
                var password = context.Request.Query["password"];

                this._logger.LogInformation("### LoginModule.Login(" + username + ",*)...");

                User user = LoginModule.Login(username, password);

                // See Auth.SessionExtensions
                context.Session.SetUser(user);

                string response = user != null ? user.Username + " logged" : "Authentication failed";

                await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(response));
            });
        }
    }
}
