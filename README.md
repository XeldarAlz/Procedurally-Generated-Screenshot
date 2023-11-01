# Procedurally-Generated-Screenshot

Scope: Create universal GD/Technical artist-Tool which give opportunity to make and use different screenshot presets,
screenshot scenes and create .png/.jpg or .tga icons from existing 3d models (prefabs).
Also, this tool could be used by developers to create screenshots in runtime and set sprites in necessary placeholders without saving
of images in project folders.


According to this scope, there is such entities:
1. ScreenshotWindow script is an custom EditorWindow.
- Gives an opportunity to create custom new presets or choose active screenshot presets 

2. ScreenshotSettings script is a Scriptable object of screenshot settings presets. Handle:
- Name of scene with installed Camera and position of model to be captured
- Screenshot size
- Some camera settings
- Path to folder with prefabs to make screenshot
- Path to folder to save captured images
- Save image format

3. ScreenshotService script. Take responsibility for:
- Creating ScreenshotGenerator with specific settings preset
- Loading and unloading of ScreenshotScene 
- Building path and transferring prefabs to make screenshot
- Transferring array of captured sprites (screenshots) 
- Choosing of save format and building path to save folder
- Saving screenshots

4. ScreenshotGenerator script. Take responsibility for:
- Creating and positioning of Prefabs before screenshot made
- Making screenshot
- Destroying of Prefabs after screenshot made

Usage requirements:
* Use ScreenshotWindow to make screenshots from the Editor runtime mode (any scene should be launched)
* Path to Load prefabs should start from Assets folder! Example: Assets/3DTOYS/PREFABS
* Path to Save prefabs could start without explicit pointing on Assets folder! Example: ImageCaptureService/Resources
* You could create new ScreenShotSettings via Service or with Unity editor. Created settings from the service get name with file hash ending
* Created new ScreenshotSettings are placing into the Assets/ScreenCaptureService folder. If there is no such a folder,
folder will be created automatically
* Using ScreenshotWindow out of Editor runtime playmode allow trigger Launching Editor play mode. 
In this case, ScreenshotWindow should be opened again.
* Possible to capture screenshots and set it in necessary place without saving on file. Look at the folder "Example" and ScreenshotController class