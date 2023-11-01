using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(order = 51, menuName = "Screenshots/Create new Settings", fileName = "Screenshot Settings")]
public class ScreenshotSettings : ScriptableObject
{
    [Header("Image settings")]
    [Tooltip("Width of the screenshot")] public int Width = 64;
    [Tooltip("Height of the screenshot")] public int Height = 64;

    [Space, Header("Scene and Camera settings")]
    [Tooltip("Name of the scene to capture")] public string SceneName;
    [Tooltip("Flag indicating if the camera is orthographic")] public bool IsOrthographic;

    [Space, Header("Save settings")]
    [Tooltip("Flag indicating if the screenshot should be saved")] public bool IsShouldBeSaved;
    [Tooltip("Folder with 3d models to get prefabs to screenshot")] public string PathToGetPrefabs;
    [Tooltip("Folder should be nested to Project Asset folder")] public string PathToSave;
    public SaveFormat FormatToSave;

    // Calculate the full path to save the screenshot
    public string FullPathToSave => String.Concat(Application.dataPath, Path.AltDirectorySeparatorChar, PathToSave, Path.AltDirectorySeparatorChar);
}
public enum SaveFormat
{
    PNG, // Portable Network Graphics format
    JPG, // JPEG format
    TGA  // Truevision TGA format
}