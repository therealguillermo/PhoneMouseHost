#!/bin/bash

# Kill any existing instances
pkill -f PhoneMouse

# Wait a moment for the process to fully terminate
sleep 1

# Build the latest version
echo "Building latest version..."
./build-mac.sh

# Get the full path to the executable
APP_PATH="$(pwd)/PhoneMouse.app/Contents/MacOS/PhoneMouse"
echo "Launching executable from: $APP_PATH"

# Launch the executable with proper permissions and sandbox disabled
"$APP_PATH" --no-sandbox --enable-accessibility 2>&1 | tee /tmp/PhoneMouse.log &

# Wait a moment for the app to start
sleep 2

# Check if the app is running
if pgrep -f PhoneMouse > /dev/null; then
    echo "PhoneMouse is running"
    echo "Output is being logged to /tmp/PhoneMouse.log"
    echo "Press Ctrl+C to stop"
    tail -f /tmp/PhoneMouse.log
else
    echo "Failed to start PhoneMouse"
    echo "Check /tmp/PhoneMouse.log for details"
fi 