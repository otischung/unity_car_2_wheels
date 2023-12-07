# Unity Car 2 Wheels

This project uses Unity version 2022.3.15f1.

The Scripts/ directory in Assets/ is a submodule whose source is from [alianlbj23](https://github.com/alianlbj23/Unity_script).



## Steps

### URDF Importer

Reference: https://github.com/Unity-Technologies/URDF-Importer

1. Open the Package Manager from the Unity Menu. Click `Window -> Package Manager`. A new package manager window will appear.

2. Click on the `+` sign on the top left corner of the package manager window and click on `Add Package from Git URL`.
3. Enter the git URL for the URDF Importer with the latest version tag (currently v0.5.2) `https://github.com/Unity-Technologies/URDF-Importer.git?path=/com.unity.robotics.urdf-importer#v0.5.2` in the text box and press `Enter`.
4. Click `Import URDF`.



### NuGet for Unity

Reference: https://github.com/GlitchEnzo/NuGetForUnity

1. Open Package Manager window (Window | Package Manager)
2. Click `+` button on the upper-left of a window, and select "Add package from git URL..."
3. Enter the following URL and click `Add` button

```
https://github.com/GlitchEnzo/NuGetForUnity.git?path=/src/NuGetForUnity
```



> ***NOTE:\*** To install a concrete version you can specify the version by prepending #v{version} e.g. `#v2.0.0`. For more see [Unity UPM Documentation](https://docs.unity3d.com/Manual/upm-git.html).

#### 

### WebSocket Sharp - Net Standard

1. Open the NuGet from the Unity Menu. Click `Nuget -> Manage NuGet Packages`. A new NuGet for Unity window will appear.
2. Search the package `websocketsharp-netstanderd` and then install it.

