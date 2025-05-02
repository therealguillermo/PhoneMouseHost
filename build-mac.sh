#!/bin/bash

# Set variables
APP_NAME="PhoneMouse"
APP_BUNDLE="$APP_NAME.app"
DOTNET_ROOT="/usr/local/share/dotnet"
FRAMEWORK="net9.0"
DMG_NAME="$APP_NAME.dmg"
DMG_TEMP="temp_dmg"
DMG_FINAL="$APP_NAME-final.dmg"

# Clean up previous builds
rm -rf "PhoneMouse/bin/Release"
rm -rf "$APP_BUNDLE"
rm -rf "$DMG_NAME"
rm -rf "$DMG_TEMP"
rm -rf "$DMG_FINAL"

# Build the application
echo "Building $APP_NAME..."
dotnet publish PhoneMouse/PhoneMouse.csproj \
    -c Release \
    -r osx-x64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -p:IncludeNativeLibrariesForSelfExtract=true

# Create app bundle structure
echo "Creating app bundle..."
mkdir -p "$APP_BUNDLE/Contents/MacOS"
mkdir -p "$APP_BUNDLE/Contents/Resources"

# Copy binary
cp "PhoneMouse/bin/Release/$FRAMEWORK/osx-x64/publish/PhoneMouse" "$APP_BUNDLE/Contents/MacOS/"

# Copy icons
cp "PhoneMouse/icons/appicon.icns" "$APP_BUNDLE/Contents/Resources/"
mkdir -p "$APP_BUNDLE/Contents/MacOS/icons"
cp "PhoneMouse/icons/trayicon.png" "$APP_BUNDLE/Contents/MacOS/icons/"

# Create Info.plist
cat > "$APP_BUNDLE/Contents/Info.plist" << EOF
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundlePackageType</key>
    <string>APPL</string>
    <key>CFBundleIdentifier</key>
    <string>com.phonemouse.app</string>
    <key>CFBundleName</key>
    <string>PhoneMouse</string>
    <key>CFBundleExecutable</key>
    <string>PhoneMouse</string>
    <key>CFBundleIconFile</key>
    <string>appicon</string>
    <key>CFBundleShortVersionString</key>
    <string>1.0.0</string>
    <key>CFBundleVersion</key>
    <string>1</string>
    <key>LSMinimumSystemVersion</key>
    <string>10.15</string>
    <key>NSHighResolutionCapable</key>
    <true/>
    <key>LSUIElement</key>
    <true/>
    <key>LSBackgroundOnly</key>
    <false/>
    <key>NSAppleScriptEnabled</key>
    <true/>
    <key>NSUIElement</key>
    <true/>
    <key>NSRequiresAquaSystemAppearance</key>
    <false/>
    <key>NSAccessibilityEnabled</key>
    <true/>
    <key>NSAccessibilityAssistiveDeviceEnabled</key>
    <true/>
    <key>NSInputMonitoringUsageDescription</key>
    <string>PhoneMouse needs input monitoring to control your mouse and keyboard.</string>
    <key>NSInputMonitoringUsage</key>
    <true/>
    <key>NSAppleEventsUsageDescription</key>
    <string>PhoneMouse needs to control your mouse and keyboard.</string>
    <key>NSAppleEventsUsage</key>
    <true/>
    <key>NSAccessibilityUsageDescription</key>
    <string>PhoneMouse needs accessibility permissions to control your mouse and keyboard.</string>
    <key>NSAccessibilityUsage</key>
    <true/>
    <key>NSAccessibilityRequestPermission</key>
    <true/>
    <key>NSAccessibilityRequestPermissionDescription</key>
    <string>PhoneMouse needs accessibility permissions to control your mouse and keyboard. Please grant these permissions in System Settings.</string>
    <key>NSAccessibilityRequestPermissionTimeout</key>
    <integer>30</integer>
</dict>
</plist>
EOF

# Make the binary executable
chmod +x "$APP_BUNDLE/Contents/MacOS/PhoneMouse"

# Create DMG
echo "Creating DMG..."

# Create temporary DMG directory
mkdir -p "$DMG_TEMP"
cp -R "$APP_BUNDLE" "$DMG_TEMP/"

# Create Applications symlink
ln -s /Applications "$DMG_TEMP/Applications"

# Create the DMG
hdiutil create -volname "$APP_NAME" -srcfolder "$DMG_TEMP" -ov -format UDRW "$DMG_NAME"

# Mount the DMG
MOUNT_DIR="/Volumes/$APP_NAME"
hdiutil attach -readwrite -noverify -noautoopen "$DMG_NAME"

# Set the DMG window style
echo '
   tell application "Finder"
     tell disk "'$APP_NAME'"
           open
           set current view of container window to icon view
           set toolbar visible of container window to false
           set statusbar visible of container window to false
           set the bounds of container window to {400, 100, 900, 400}
           set theViewOptions to the icon view options of container window
           set arrangement of theViewOptions to not arranged
           set icon size of theViewOptions to 72
           set position of item "'$APP_NAME.app'" of container window to {100, 150}
           set position of item "Applications" of container window to {400, 150}
           close
           open
           update without registering applications
           delay 5
           close
     end tell
   end tell
' | osascript

# Unmount the DMG
hdiutil detach "$MOUNT_DIR"

# Convert the DMG to read-only
hdiutil convert "$DMG_NAME" -format UDZO -o "$DMG_FINAL"

# Clean up
rm -f "$DMG_NAME"
rm -rf "$DMG_TEMP"
mv "$DMG_FINAL" "$DMG_NAME"

echo "Build complete! You can find the application at $APP_BUNDLE and the installer at $DMG_NAME" 