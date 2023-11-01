using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor window for managing screenshot capture settings.
/// </summary>
public class ScreenshotWindow : EditorWindow
{
    private static ScreenshotSettings screenshotSettings;

    private static ScreenshotWindow screenshotWindow;
    private static ScreenshotService screenshotService;
    private static string pathToLoad;
    private static string pathToSave;

    private const int WINDOW_MIN_SIZE_X = 400;
    private const int WINDOW_MIN_SIZE_Y = 500;
    private const int BUTTON_SIZE_X = 200;
    private const int BUTTON_SIZE_Y = 30;
    private const int WINDOW_HALF_SPACE = 200;
    private const int WINDOW_QUARTA_SPACE = 100;

    [MenuItem(itemName: "Screenshot/ScreenshotWindow")]
    public static void OpenWindow()
    {
        screenshotWindow = GetWindow<ScreenshotWindow>(typeof(ScreenshotWindow));
        screenshotWindow.minSize = new Vector2(WINDOW_MIN_SIZE_X, WINDOW_MIN_SIZE_Y);
        screenshotWindow.Show();
        screenshotSettings = Resources.FindObjectsOfTypeAll<ScreenshotSettings>().FirstOrDefault();
    }

    private static void InitScreenshotData()
    {
        screenshotWindow.Close();
        screenshotService = new ScreenshotService(screenshotSettings);
        screenshotService.StartSettingImageSequence();
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        DrawWindowHeader();

        if (screenshotSettings == null)
        {
            DrawScreenshotSettingsCreationButton();
            GUILayout.EndVertical();
            return;
        }

        DrawScreenshotSettingsEditor();
        DrawScreenshotSettingsCreationButton();
        ShowCloseWindowButton();
        GUILayout.EndVertical();
    }

    private void DrawWindowHeader()
    {
        GUILayout.Label("Screenshot capture settings", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Screenshot settings SO");
        screenshotSettings = EditorGUILayout.ObjectField(screenshotSettings, typeof(ScreenshotSettings), false, GUILayout.Width(WINDOW_HALF_SPACE)) as ScreenshotSettings;
        GUILayout.EndHorizontal();
    }

    private void DrawScreenshotSettingsEditor()
    {
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Screenshot scene name");
        screenshotSettings.SceneName = EditorGUILayout.TextField(screenshotSettings.SceneName, GUILayout.Width(WINDOW_HALF_SPACE));
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        GUILayout.Label("Screenshot sizes", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Screenshot height");
        screenshotSettings.Height = EditorGUILayout.IntField(screenshotSettings.Height, GUILayout.Width(WINDOW_HALF_SPACE));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Screenshot width");
        screenshotSettings.Width = EditorGUILayout.IntField(screenshotSettings.Width, GUILayout.Width(WINDOW_HALF_SPACE));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Is orthographic");
        screenshotSettings.IsOrthographic = EditorGUILayout.Toggle(screenshotSettings.IsOrthographic, GUILayout.Width(WINDOW_HALF_SPACE));
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Path to get prefabs to capture");
        screenshotSettings.PathToGetPrefabs = EditorGUILayout.TextField(screenshotSettings.PathToGetPrefabs, GUILayout.Width(WINDOW_HALF_SPACE));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Should screenshot be saved");
        screenshotSettings.IsShouldBeSaved = EditorGUILayout.Toggle(screenshotSettings.IsShouldBeSaved, GUILayout.Width(WINDOW_HALF_SPACE));
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (screenshotSettings.IsShouldBeSaved)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Save format");
            screenshotSettings.FormatToSave = (SaveFormat)EditorGUILayout.EnumPopup(screenshotSettings.FormatToSave, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Path to save screenshots");
            screenshotSettings.PathToSave = EditorGUILayout.TextField(screenshotSettings.PathToSave, GUILayout.Width(200));
            GUILayout.EndHorizontal();
        }
    }

    private void DrawScreenshotSettingsCreationButton()
    {
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.Label(GUIContent.none, GUILayout.Width(WINDOW_QUARTA_SPACE));

        if (GUILayout.Button("Create screenshot settings", GUILayout.Width(BUTTON_SIZE_X), GUILayout.Height(BUTTON_SIZE_Y)))
        {
            CreateScreenshotSettings();
        }

        GUILayout.Label(GUIContent.none, GUILayout.Width(WINDOW_QUARTA_SPACE));
        GUILayout.EndHorizontal();
    }

    private void ShowCloseWindowButton()
    {
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.Label(GUIContent.none, GUILayout.Width(WINDOW_QUARTA_SPACE));

        if (EditorApplication.isPlaying == false)
        {
            DrawLaunchPlayModeButton();
        }
        else
        {
            DrawMakeScreenshotButton();
        }

        GUILayout.Label(GUIContent.none, GUILayout.Width(WINDOW_QUARTA_SPACE));
        GUILayout.EndHorizontal();
    }

    private void DrawLaunchPlayModeButton()
    {
        if (!GUILayout.Button("Launch Play mode", GUILayout.Width(BUTTON_SIZE_X), GUILayout.Height(BUTTON_SIZE_Y))) return;
        EditorApplication.EnterPlaymode();
        screenshotWindow.Close();
    }

    private void DrawMakeScreenshotButton()
    {
        if (GUILayout.Button("Make screenshots", GUILayout.Width(BUTTON_SIZE_X), GUILayout.Height(BUTTON_SIZE_Y)))
        {
            InitScreenshotData();
        }
    }

    private void CreateScreenshotSettings()
    {
        screenshotSettings = CreateInstance<ScreenshotSettings>();

        if (AssetDatabase.GetSubFolders("Assets/ImageCaptureService").Contains("Assets/ImageCaptureService/ScreenshotSettings") == false)
        {
            AssetDatabase.CreateFolder("Assets/ImageCaptureService", "ScreenshotSettings");
        }

        AssetDatabase.CreateAsset(screenshotSettings, $"Assets/ImageCaptureService/ScreenshotSettings/ScreenshotSettings{screenshotSettings.GetHashCode()}.asset");
        AssetDatabase.SaveAssets();
    }
}