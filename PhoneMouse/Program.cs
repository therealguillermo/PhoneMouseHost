using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhoneMouse;
using Eto.Forms;
using Eto.Drawing;
using System.IO;

namespace PhoneMouseTrayApp
{
    class Program
    {
        private static string? localIP;
        private static Application? app;
        private static TrayIndicator? tray;
        private static BroadcastService? broadcastService;

        static void Main(string[] args)
        {
            localIP = NetworkHelper.GetLocalIPAddress();
            
            // Initialize Eto.Forms application
            app = new Application(Eto.Platform.Detect);
            
            // Configure application to not show in Dock
            if (Eto.Platform.Detect.IsMac)
            {
                app.Style = "application";
            }
            
            // Create tray indicator
            tray = new TrayIndicator
            {
                Title = "PhoneMouse",
                Image = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "icons", "trayicon.png"))
            };

            // Create menu
            var menu = new ContextMenu();
            menu.Items.Add(new ButtonMenuItem { Text = $"IP: {localIP}", Enabled = false });
            menu.Items.Add(new SeparatorMenuItem());
            menu.Items.Add(new ButtonMenuItem { Text = "Quit", Command = new Command((s, e) => {
                broadcastService?.Dispose();
                tray.Visible = false;
                app.Quit();
            })});

            tray.Menu = menu;
            tray.Visible = true;

            // Start the broadcast service
            broadcastService = new BroadcastService();
            broadcastService.Start();

            // Start the web server
            var serverTask = Task.Run(() => RunWebServer(args));
            
            // Run the application
            app.Run();
        }

        private static void RunWebServer(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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
            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapHub<ControlHub>("/controlHub");

            app.Urls.Add("http://0.0.0.0:5123");
            app.Urls.Add("http://localhost:5123");
            app.Urls.Add($"http://{localIP}:5123");

            app.Run();
        }
    }
}