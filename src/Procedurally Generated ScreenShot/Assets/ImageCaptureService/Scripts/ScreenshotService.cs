using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Service for capturing and saving screenshots based on provided settings.
/// </summary>
public class ScreenshotService
{
    public event Action<Sprite[]> ScreenshotMade;

    private readonly StringBuilder _stringBuilder = new StringBuilder();
    private ScreenshotGenerator _screenshotGenerator;
    private Sprite[] _screenshots = new Sprite[] { };
    private ScreenshotSettings _screenshotSettings;

    private const string INCORRECT_FOLDER_ERROR = "Folder does not contain prefabs to screenshot. Check the path of the folder to create screenshots";
    private const string PREFAB_SEARCH_PARAMETER = "*.prefab";
    private const char DOT_CHAR = '.';
    private const float PIVOT_CENTER_COORD = 0.5f;

    public ScreenshotService(ScreenshotSettings screenshotSettings)
    {
        _screenshotSettings = screenshotSettings;
    }

    public void StartSettingImageSequence()
    {
        RenewScreenshotArray();
    }

    private async Task LoadScreenshotScene()
    {
        if (SceneManager.GetSceneByName(_screenshotSettings.SceneName).isLoaded == false)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_screenshotSettings.SceneName, LoadSceneMode.Additive);

            while (asyncLoad.isDone == false)
            {
                await Task.Yield();
            }
        }

        if (_screenshotGenerator != null) return;
        _screenshotGenerator = SceneManager.GetSceneByName(_screenshotSettings.SceneName).GetComponent<ScreenshotGenerator>();
    }

    private async void RenewScreenshotArray()
    {
        _screenshots = null;
        await LoadScreenshotScene();

        string[] files = TryGetFilesToMakeScreenshots();
        if (files.Length > 0)
        {
            _screenshots = new Sprite[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(files[i]);
                _screenshots[i] = CreateScreenshotSprite(prefab);
            }
        }

        if (_screenshots == null)
        {
            Debug.LogError(INCORRECT_FOLDER_ERROR);
        }
        else
        {
            ScreenshotMade?.Invoke(_screenshots);
        }

        Debug.Log($"Files count in folder: {files.Length}");
        SceneManager.UnloadSceneAsync(_screenshotSettings.SceneName);
    }

    private void SaveScreenshot(Texture2D texture, string fileName)
    {
        if (_screenshotSettings.IsShouldBeSaved == false) return;

        BuildSavePath(_screenshotSettings, fileName);

        byte[] bytes = new byte[] { };

        switch (_screenshotSettings.FormatToSave)
        {
            case SaveFormat.JPG:
                bytes = texture.EncodeToJPG();
                break;
            case SaveFormat.PNG:
                bytes = texture.EncodeToPNG();
                break;
            case SaveFormat.TGA:
                bytes = texture.EncodeToTGA();
                break; ;
        }

        File.WriteAllBytes(_stringBuilder.ToString(), bytes);

        AssetDatabase.Refresh();
        Debug.Log($"Saved texture with width = {texture.width} and height = {texture.height}");
    }

    private void BuildSavePath(ScreenshotSettings screenshotSettings, string pngName)
    {
        _stringBuilder.Clear();
        _stringBuilder.Append(screenshotSettings.FullPathToSave);
        _stringBuilder.Append(pngName);
        _stringBuilder.Append(DOT_CHAR);
        _stringBuilder.Append(screenshotSettings.FormatToSave.ToString());
    }

    private string[] TryGetFilesToMakeScreenshots()
    {
        _stringBuilder.Clear();
        _stringBuilder.Append(_screenshotSettings.PathToGetPrefabs);
        _stringBuilder.Append(Path.AltDirectorySeparatorChar);
        return Directory.GetFiles(_stringBuilder.ToString(), PREFAB_SEARCH_PARAMETER, SearchOption.TopDirectoryOnly);
    }

    private Sprite CreateScreenshotSprite(GameObject objectToCapture)
    {
        Texture2D texture = _screenshotGenerator.MakeScreenshot(objectToCapture, _screenshotSettings);
        SaveScreenshot(texture, objectToCapture.name);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(PIVOT_CENTER_COORD, PIVOT_CENTER_COORD));
        return sprite;
    }
}