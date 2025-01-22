
using System;
using System.Drawing;
using System.Windows.Forms;
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

        // Import necessary Windows API functions to manipulate the console window
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();



        [STAThread] // Marking the Main method for STAThread (required for NotifyIcon)
        static void Main(string[] args)
        {

            bool showConsole = true;
            // Hide the console window if running as a Windows application
            if (Environment.UserInteractive && !showConsole) // Checks if running interactively (console mode)
            {
                // Set the console window to invisible
                var handle = GetConsoleWindow();
                ShowWindow(handle, SW_HIDE);
                FreeConsole();
            }
            // Run the web server (ASP.NET Core)
            Task.Run(() => RunWebServer(args));

            // Run the tray application (Windows Forms)
            RunTrayApplication();
        }

        private static void RunTrayApplication()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            NotifyIcon trayIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon("icons/PhoneMouseHost.ico"), // Ensure you have the icon
                Text = "PhoneMouse Host",
                Visible = true
            };

            // Create the tray menu
            ContextMenuStrip trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Open", null, (sender, e) => OpenApp());
            trayMenu.Items.Add("Exit", null, (sender, e) => ExitApp(trayIcon));

            // Attach the menu to the tray icon
            trayIcon.ContextMenuStrip = trayMenu;

            // Run the tray application loop
            Application.Run();
        }

        private static void OpenApp()
        {
            MessageBox.Show($"PhoneMouse Server Running\n\nHosted on local network at:\n{NetworkHelper.GetLocalIPAddress()}\n", "Info");

        }

        private static void ExitApp(NotifyIcon trayIcon)
        {
            trayIcon.Visible = false;
            trayIcon.Dispose();
            Application.Exit();
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