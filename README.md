# Unity Preview Server

[![CI](https://github.com/frozenbonito/UnityPreviewServer/actions/workflows/ci.yaml/badge.svg)](https://github.com/frozenbonito/UnityPreviewServer/actions/workflows/ci.yaml)
[![License](https://img.shields.io/github/license/frozenbonito/UnityPreviewServer)](https://github.com/frozenbonito/UnityPreviewServer/blob/main/LICENSE)
[![Release](https://img.shields.io/github/v/release/frozenbonito/UnityPreviewServer)](https://github.com/frozenbonito/UnityPreviewServer/releases/latest)
[![OpenUPM](https://img.shields.io/npm/v/jp.frozenbonito.preview-server?label=openupm&registry_uri=https://package.openupm.com&color=orange)](https://openupm.com/packages/jp.frozenbonito.preview-server/)

Unity Preview Server is a Unity editor extension for previewing WebGL application locally.

This extension allows you to run your WebGL application on a fixed port without building it every time.

## Supported Unity Editor

- 2021.3 or later

## Installation

### Install via git URL

1. Open Package Manager window in your Unity Editor (`Main Menu > Window > Package Manager`).
2. Click `+` and select `Add package from git URL...`.
3. Add `https://github.com/frozenbonito/UnityPreviewServer.git?path=Packages/jp.frozenbonito.preview-server`.

You can specify a version with a URL suffix like `v1.0.0`.
For example, `https://github.com/frozenbonito/UnityPreviewServer.git?path=Packages/jp.frozenbonito.preview-server#v1.0.0`.

### Install via OpenUPM

See the [OpenUPM package documentation](https://openupm.com/packages/jp.frozenbonito.preview-server/).

## Usage

After installing this extension, `Preview Server` item will be added to the main menu.

### Start server

`Main Menu > Preview Server > Start`

### Stop server

`Main Menu > Preview Server > Stop`

The server will automatically stop when the editor is closed or the assembly is reloaded.

### Preview Server Window

`Main Menu > Preview Server > Open Window...`

You can edit the user settings, start and stop the server, and view the logs in the window.

## Configurations

### Preferences

`Main Menu > Edit > Preferences... > Preview Server`

#### Default Port

The port number to use for the preview server.
This will be used as the default value for all projects.

Default: 5000

### Project Settings

`Main Menu > Edit > Project Settings... > Preview Server`

#### Default Build Location

The build location for WebGL; the contents in it will be served by the preview server.
This will be used as the default location for the project.
Only folders with in a project can be set.

Default: `Build`

### User Settings

`Main Menu > Preview Server > Open Window...`

User settings are user-specific and project-specific settings.

#### Use Default Location

If this item is checked, the server will serve the contents in the [default build location](#default-build-location).
Otherwise, the [build location in the user settings](#build-location) will be used.

Default: `true`

#### Build Location

The build location for WebGL; the contents in it will be served by the preview server.
If [`Use Default Location`](#use-default-location) is checked, this is ignored.

Default: The current location for the WebGL build, or `${PROJECT_PATH}/Build`

#### Use Default Port

If this item is checked, the server will use the [default port](#default-port).
Otherwise, the [port in the user settings](#port) will be used.

Default: `true`

#### Port

The port number to use for the preview server.
If [`Use Default Port`](#use-default-port) is checked, this is ignored.

Default: 5000

## Related project

[unisrv](https://github.com/frozenbonito/unisrv) - A preview server for Unity WebGL application.
