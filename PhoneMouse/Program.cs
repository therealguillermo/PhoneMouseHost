
#if WINDOWS
using System.Windows.Forms;
#endif

using System;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhoneMouse;
using System.Runtime.InteropServices;

namespace PhoneMouseTrayApp
{
    class Program
    {
        static void Main(string[] args)
        {

            bool showConsole = true;

            // Run the web server (ASP.NET Core)
            Console.WriteLine($"starting server on {NetworkHelper.GetLocalIPAddress()}");
            
            var serverTask = Task.Run(() => RunWebServer(args));
            serverTask.Wait();

            Console.WriteLine($"end");

        }

        private static void RunWebServer(string[] args)
        {
            var localIP = NetworkHelper.GetLocalIPAddress();

            // Create builder and setup App
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            //builder.Services.AddRazorPages();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsName", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            builder.Services.AddSingleton<MouseController>();
            builder.Services.AddSignalR();
            builder.Services.AddControllers();


            var app = builder.Build();

            app.UseCors("CorsName");
            // Configure request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Map SignalR hub
            //app.MapRazorPages();
            app.MapHub<ControlHub>("/controlHub");

            // Restrict to local network
            app.Urls.Add("http://0.0.0.0:5123"); // Listen on all local interfaces
            app.Urls.Add("http://localhost:5123");
            app.Urls.Add($"http://{localIP}:5123"); // Replace <localIP> with the PC's local network IP.


            Console.WriteLine("Prgm started!");
            app.Run();
        }
    }
}