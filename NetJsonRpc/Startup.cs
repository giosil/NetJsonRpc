using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NetJsonRpc.Auth;

namespace NetJsonRpc
{
    public class Startup
    {
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
                    .AddSessionStateTempDataProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\log\\NetJsonRpc.log");

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

        private static void HandleLogin(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var username = context.Request.Query["username"];
                var password = context.Request.Query["password"];

                User user = LoginModule.Login(username, password);

                // See Auth.SessionExtensions
                context.Session.SetUser(user);

                string response = user != null ? user.Username + " logged" : "Authentication failed";
                
                await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(response));
            });
        }
    }
}
