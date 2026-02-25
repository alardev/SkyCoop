# Sky Co-op

This is a fork of the Reborn branch of the **Sky Co-Op** mod in order to support the latest versions of [The Long Dark](https://store.steampowered.com/app/305620), and the latest release of the [Melon Loader](https://github.com/LavaGang/MelonLoader).

**Sky Co-op** is a multiplayer mod for [The Long Dark](https://store.steampowered.com/app/305620), that is powered by [Melon Loader](https://github.com/LavaGang/MelonLoader) developed by [Lava Gang](https://github.com/LavaGang).

This is a personal fork developed by myself. I am not affiliated with the original author [Filigrani](https://github.com/Filigrani).

## Currently Supported Features
WIP: I am still reviewing the codebase. I will update this section soon.

## Planned Features
- CI/CD for fast and easily reproducible releases for all platforms.
- Proper Documentation for developers and players.

Additional planned features are under consideration. Feel free to file for issues or contribute any if you desire.

## FAQ
### Why did you fork it?
I seek to improve the overall quality of the mod, seeing as it is currently lacking in many areas such as localizations and does not support the latest game version (2.51 as of time of writing). I am also quite impatient, opinionated and generally like to avoid working with others, which is why I decided to take matters in my own hands.

### Why did you remove the Texas Holdem card game and the talking fish feature?
Initially I received a healthy chuckle from discovering those features in the original codebase, but ultimately I quickly decided to remove them in order to focus on the core aspects of the mod, which I have deemed as a top priority. I might reintegrate them later as togglable features.

### Where could I request a feature?
Use the issues section to file for a bug or a feature request.

### Will the multiplayer mod support other mods?
Depends on the nature of the mod. If the mod is purely cosmetic, then I expect it won't cause any issues. Item mods, however, such as food additions etc., should be treated with more involvement, as all players should have it installed in order to avoid desync issues and other bugs. If the mod alters any core game behaviours then more work needs to be done. This is an issue I will look into as it's something I'm personally interested as well.

### Will you support Linux/MacOS as well as cross-platform support?
Yes. I myself use Linux (Bazzite + Arch) and am not aware of any technical limitations as for why this should be a problem. Current development is done on linux targeting windows builds on Proton/Wine. I do plan to support native builds for Linux and maybe even MacOS as well once I get a clear understanding of the inner workings of all of the components. 

## Setting up the project
### Prerequisites for Linux/Proton:
1. Install the windows version of the game in steam using the Proton Compatibility layer.
2. Run the game as is atleast once to ensure the proton prefix creation.
3. Install protontricks
4. Install dotnet6desktop using protontricks in the games prefix.
    ```bash
    protontricks 305620 dotnetdesktop6
    ```
5. install the pre-release/nightly melonloader to the game root directory. Use the MelonLoader.Installer.Linux for convenience, which will automatically detect the correct path and type of game installation and setup the melonloader for you.
### Building the __Sky Co-Op__ mod libraries (SkyCoopServer and SkyCoopClient)
1. Install the latest dotnet 6 SDK in your linux development environment.

    Arch W/ Paru: `paru -S dotnet-sdk-6.0`

3. Install NuGet package manager.
4. Install `WebRtcVadSharp` version 1.3.2.0 and copy the DLL from your NuGet lib folder to the `SkyCoop/dependencies` directory.
5. Download the latest ModSettings.dll and place it inside the `SkyCoop/dependencies` directory.
6. in SkyCoop folder run `dotnet build`
7. The built DLLs should be inside your `SkyCoop/Output/Debug/` directory.
8. Copy the built DLLs and JSON files from the `SkyCoop/Output/Debug/` directory to your Mods folder.
9. Install `Mono.Nat`, `LiteNetLib` and `OpenVoiceSharp` via NuGet package manager and copy them to your Mods folder.
The default location for these libraries is in the `~/.nuget/packages/` directory. The correct DLLs are located inside the `lib` directory. 
11. Place the latest ModSettings.dll and ModComponent.dll mods into your Mods folder.

### Building the AssetBundles and the .modcomponent

WIP: Coming Soon

# Disclaimer
The mod developers of **Sky Co-Op** are not affiliated in any way with [The Long Dark](https://store.steampowered.com/app/305620). [The Long Dark](https://store.steampowered.com/app/305620) is a product of [Hinterland Studio Inc](https://hinterlandgames.com/).
