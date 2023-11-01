using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScreenshotController : MonoBehaviour
{
    [SerializeField] private Button _screenshotButton;
    [SerializeField] private Image _image;
    [SerializeField] private ScreenshotSettings _screenshotSettings;

    private ScreenshotService _screenshotService;

    private void Start()
    {
        _screenshotButton.onClick.AddListener(StartSettingImageSequence);
        _screenshotService = new ScreenshotService(_screenshotSettings);
        _screenshotService.ScreenshotMade += SetImage;
    }

    private void OnDestroy()
    {
        _screenshotButton.onClick.RemoveListener(StartSettingImageSequence);
        _screenshotService.ScreenshotMade -= SetImage;
    }

    private void StartSettingImageSequence()
    {
        _screenshotService.StartSettingImageSequence();
    }
    
    private void SetImage(Sprite[] sprites)
    {
        if (sprites == null) return;

        _image.sprite = sprites[Random.Range(0, sprites.Length)];
        _image.gameObject.SetActive(true);
    }
}