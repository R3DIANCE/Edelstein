# Edelstein [![Build](https://github.com/Kaioru/Edelstein/actions/workflows/build.yaml/badge.svg)](https://github.com/Kaioru/Edelstein/actions/workflows/build.yaml)
A v.176.1 Mushroom game server emulator written in C#.

## 🚀 Getting started

### ✨ Usage

#### Prerequisites
* A running [PostgreSQL](https://www.postgresql.org) server!
* That's mostly it..

#### Download a release
1. Check the [releases](https://github.com/Kaioru/Edelstein/releases) tab and download the correct bundle based on your OS!

#### Download required assets 
2. Download the data from [Server.NX](https://github.com/Kaioru/Server.NX/releases)
3. Download the scripts from [Server.Scripts](https://github.com/Kaioru/Server.Scripts/releases)
4. Unzip both into the `data` and `scripts` folder respectively

#### Update configuration and migrations
5. Edit the `appsettings.json` file to the appropriate settings
6. Run the scripts in with the `migrate` prefix in sequence

#### Running the server
7. Run the `Edelstein.Daemon.Server` executable

### 🏗️ Builds
A nightly build is published at 00:00 UTC when there are changes to the 'dev' branch.

* Executables are available under [releases](https://github.com/Kaioru/Edelstein/releases/tag/nightly) tab with the `nightly` tag
* Protocol and Common libraries are pushed to [packages](https://github.com/Kaioru?tab=packages&repo_name=Edelstein)

#### Setting up your project for Github Packages
1. Create a Personal Access Token with the 'read:packages' scope
2. Create a `nuget.config` file on your project root with the following contents:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <packageSources>
        <clear />
        <add key="github" value="https://nuget.pkg.github.com/Kaioru/index.json" />
    </packageSources>
    <packageSourceCredentials>
        <github>
            <add key="Username" value="GITHUB_USERNAME" />
            <add key="ClearTextPassword" value="GITHUB_PERSONAL_ACCESS_TOKEN" />
        </github>
    </packageSourceCredentials>
</configuration>
```
3. Remember to set your Github Username and Personal Access Token!

Check the [here](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry) for more on how to setup the NuGet registry.

## 📦 Extra Stuff
* [Server.NX](https://github.com/kaioru/server.nx) - the source for the Server.nx file.
* [Server.Scripts](https://github.com/kaioru/server.scripts) - various scripts for use with Edelstein.

## ⭐️ Acknowledgements
* [Darter](https://github.com/RajanGrewal) - for the amazing support and reference point for mushroom game-related stuff!
* [Fraysa](https://github.com/gilmatok) - the inspiration of creating a mushroom game server in C# and help on Discord
* [pokiuwu](https://github.com/pokiuwu) - great resource of auth hooks, mushroom game-related information
* [swordie](https://bitbucket.org/swordiemen/swordie/src/master/) - really cool open source v176.1 mushroom game server in Java, alot of referencing done from there