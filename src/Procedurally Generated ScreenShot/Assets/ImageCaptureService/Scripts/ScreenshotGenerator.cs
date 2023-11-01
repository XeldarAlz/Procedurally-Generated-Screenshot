using UnityEngine;

/// <summary>
/// Component for generating screenshots based on provided settings.
/// </summary>
public class ScreenshotGenerator : MonoBehaviour
{
    [SerializeField] private Transform _screenShotPosition;
    [SerializeField] private Camera _screenshotCamera;

    private RenderTexture _renderTexture;

    /// <summary>
    /// Generates a screenshot of a specified game object with the given settings.
    /// </summary>
    /// <param name="objectToCapture">The game object to capture in the screenshot.</param>
    /// <param name="screenshotSettings">The settings to use for capturing the screenshot.</param>
    /// <returns>A Texture2D containing the generated screenshot.</returns>
    public Texture2D MakeScreenshot(GameObject objectToCapture, ScreenshotSettings screenshotSettings)
    {
        GameObject screenshotModel = Instantiate(objectToCapture, _screenShotPosition);

        AdjustScreenshotCameraSettings(screenshotSettings);

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = _screenshotCamera.targetTexture;

        _screenshotCamera.Render();

        Texture2D texture = new Texture2D(_screenshotCamera.targetTexture.width, _screenshotCamera.targetTexture.height);
        texture.ReadPixels(new Rect(0, 0, _screenshotCamera.targetTexture.width, _screenshotCamera.targetTexture.height), 0, 0);
        texture.Apply();

        RenderTexture.active = currentRT;

        DestroyImmediate(screenshotModel);

        return texture;
    }

    private void AdjustScreenshotCameraSettings(ScreenshotSettings screenshotSettings)
    {
        _screenshotCamera.orthographic = screenshotSettings.IsOrthographic;

        if (_renderTexture != null) return;
        _renderTexture = new RenderTexture(screenshotSettings.Width, screenshotSettings.Height, 0);
        _screenshotCamera.targetTexture = _renderTexture;
        _renderTexture = null;
    }
}