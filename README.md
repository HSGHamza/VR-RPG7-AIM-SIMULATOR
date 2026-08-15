# RPGVR Gun Training

RPGVR is a Unity-based virtual-reality gun-training project. It combines interactive VR weapons and environments with selectable scenes and a wave-based combat mode.

## Requirements

- Unity **2022.3.62f1** (the version recorded by this project)
- A VR-ready device and runtime compatible with the configured Oculus/OpenXR providers
- Android tooling in Unity if building for a standalone headset

The project includes these main VR dependencies:

- XR Interaction Toolkit 2.6.4
- XR Plugin Management 4.5.0
- OpenXR 1.14.3 and Oculus XR 4.5.2
- XR Hands 1.4.0
- HurricaneVR

## Open the project

1. Open Unity Hub and choose **Add**.
2. Select this repository's root directory.
3. Open it with Unity 2022.3.62f1 and allow Unity to restore/import packages.
4. In the Project window, open a scene from `Assets/Scenes`.

The enabled scenes in **File > Build Settings** are:

1. `Assets/Scenes/Menu 3D.unity`
2. `Assets/Scenes/MapBuilding.unity`
3. `Assets/Scenes/battlefield.unity`
4. `Assets/Scenes/TanksField.unity`
5. `Assets/Scenes/Menu 3D 1.unity`

The first scene in this list is the normal application entry point.

## Run in the editor

Open `Assets/Scenes/Menu 3D.unity` and press Play. For VR input, ensure a supported headset is connected and its runtime is active before entering Play mode. The project also contains XR samples and third-party demo scenes; those are useful references but are not all part of the enabled build.

## Build for Android

1. Open **File > Build Settings** and switch the platform to **Android**.
2. In **Project Settings > XR Plug-in Management**, verify the intended Oculus/OpenXR provider for the target headset.
3. Confirm scenes in the build list and configure Player settings as needed.
4. Choose **Build** or **Build And Run**.

The current Player configuration identifies the app as `com.HSG.RPGVR`, requires Android API level 32 or later, and targets ARM64.

## Project layout

| Location | Purpose |
| --- | --- |
| `Assets/Scenes` | Project menus, maps, and combat scenes. |
| `Assets/MissionManager` | Wave progression and combat mission scripts. |
| `Assets/Menu` | In-game menu handling. |
| `Assets/HurricaneVR` | HurricaneVR framework and demos. |
| `Assets/VR FPS Kit` | VR FPS player, weapon, and enemy components. |
| `Assets/HQ Shooting Range` | Shooting-range environment assets. |
| `Assets/Firebase` | Firebase SDK files and integrations. |
| `Packages` | Unity package manifest and lock file. |
| `ProjectSettings` | Unity project, XR, input, and build settings. |

## Gameplay systems

`WaveController` in `Assets/MissionManager` starts combat waves, spawns three enemies per wave number, restores weapon ammo at each wave start, and advances automatically once all spawned enemies are defeated. `GameMenuManager` in `Assets/Menu` toggles the assigned menu using its configured Input System action.

## Version-control notes

Commit `Assets`, `Packages`, and `ProjectSettings`, including their `.meta` files. Unity-generated folders such as `Library`, `Temp`, `Logs`, and `obj` are excluded via `.gitignore` and should not be committed.

Firebase configuration is present under `Assets`. Review credentials and platform configuration before publishing the project or sharing a build.
