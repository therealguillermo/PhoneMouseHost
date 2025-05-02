# PhoneMouseHost

**PhoneMouse** is an ASP.NET Core SignalR application that lets you control your Mac using an iOS client app. Connect over your local network to perform mouse movements, keyboard inputs, and more in real-time. This project provides a seamless way to interact with your computer remotely.

## Features
- **Mouse Movement and Actions**: Control the mouse pointer, click, drag, and scroll from your mobile device.
- **Keyboard Input**: Send text and commands via the custom keyboard on the iOS app.
- **Local Network Access**: Connection is restricted to devices within the same local network.

## Prerequisites
- A Mac with **.NET 9.0** SDK installed
- Xcode Command Line Tools
- An iOS device with the **[PhoneMouseClient](https://github.com/therealguillermo/PhoneMouseClient-iOS)** app installed
- Both devices connected to the same local network

## Installation and Execution

### Building from Source
1. Clone the repository:
   ```bash
   git clone <your-repo-url>
   cd PhoneMouseHost
   ```

2. Make the build script executable:
   ```bash
   chmod +x build-mac.sh
   ```

3. Build the application:
   ```bash
   ./build-mac.sh
   ```

4. Install the application:
   - Open the generated `PhoneMouse.dmg`
   - Drag `PhoneMouse.app` to your Applications folder

5. Run the application:
   - Open `PhoneMouse.app` from your Applications folder
   - Grant accessibility permissions when prompted
   - The app will start a local server and display the connection details

### Development
For development purposes, you can use the `launch-mac.sh` script which builds and runs the application with additional debugging flags:
```bash
./launch-mac.sh
```

## Notes
- The application requires accessibility permissions to control your mouse and keyboard
- When running from the Applications folder, you'll be prompted to grant these permissions
- The application runs in the background and can be accessed from the menu bar

## Troubleshooting
- If the application doesn't work after installation, check System Settings > Privacy & Security > Accessibility to ensure PhoneMouse has the necessary permissions
- Make sure both your Mac and iOS device are on the same local network
- Check the application logs in `/tmp/PhoneMouse.log` for any errors
