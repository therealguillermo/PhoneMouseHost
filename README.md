# PhoneMouseHost

**PhoneMouse** is an ASP.NET Core SignalR application that lets you control your Windows PC using an iOS client app. Connect over your local network to perform mouse movements, keyboard inputs, and more in real-time. This project provides a seamless way to interact with your computer remotely.

## Features
- **Mouse Movement and Actions**: Control the mouse pointer, click, drag, *and scroll from your mobile device. note: in development*
- **Keyboard Input**: Send text and commands via the custom keyboard on the iOS app.
- **Local Network Access**: Connection is restricted to devices within the same local network.

## Prerequisites
- A Windows PC with **.NET 6.0** runtime installed.
- An iOS device with the **[PhoneMouseClient](https://github.com/therealguillermo/PhoneMouseClient-iOS)** app installed.
- Both devices connected to the same local network.

## Installation and Execution

### Option 1:
1. Clone the repository:
   ```bash
   git clone git@github.com:therealguillermo/PhoneMouseHost.git
   cd PhoneMouseHost
2. Build and run with .NET 6.0+
   ```bash
   dotnet build
   dotnet run
### Option 2:
1. Go to the Releases section of this repository.
2. Download the latest version of the pre-compiled binaries (PhoneMouseHost.zip).
3. Extract the files to a folder on your PC.
4. Run the application by double-clicking PhoneMouseHost.exe or move application to Startup folder.
   
