using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class MenuSettingsUI : MonoBehaviour
{
    [SerializeField] private RectTransform settingsButton;
    [SerializeField] private CanvasGroup[] buttons;

    [Header("Audio")]
    [SerializeField] private AudioButton musicButton;
    [SerializeField] private AudioButton soundsButton;

   

    private bool isSettingsOpened;

    public bool IsSoundsEnabled;
    public bool IsMusicEnabled;
    private void Awake()
    {
        IsSoundsEnabled = true;
        IsMusicEnabled = true;
        UpdateAudioButtonIcon(soundsButton, IsSoundsEnabled);
        UpdateAudioButtonIcon(musicButton, IsMusicEnabled);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].alpha = 0;
            buttons[i].blocksRaycasts = false;
        }

       
    }

    public void OnSoundsButtonPressed()
    {
        IsSoundsEnabled = !IsSoundsEnabled;
        UpdateAudioButtonIcon(soundsButton, IsSoundsEnabled);
       
    }

    public void OnMusicButtonPressed()
    {
        IsMusicEnabled = !IsMusicEnabled;
        UpdateAudioButtonIcon(musicButton, IsMusicEnabled);
        if (IsMusicEnabled)
            AudioManager.instance.PlayBgm1(true);
        else
        
            AudioManager.instance.PlayBgm1(false);
    }

  

    private void UpdateAudioButtonIcon(AudioButton audioButton, bool enabled)
        => audioButton.ButtonIcon.sprite = enabled ? audioButton.EnabledSprite : audioButton.DisabledSprite;

    private const float ButtonsAnimationDuration = 0.15f;
    private const float ButtonsAnimationDelay = 0.025f;
    public void ChangeSettingsPanelVisibility()
    {
        float targetAlpha = isSettingsOpened ? 0 : 1;
        float targetPosition = isSettingsOpened ? 0 : settingsButton.rect.height;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].transform.DOLocalMoveY(targetPosition * (i + 1), ButtonsAnimationDuration).SetEase(Ease.OutCubic).SetDelay(ButtonsAnimationDelay * i);
            buttons[i].DOFade(targetAlpha, ButtonsAnimationDuration).SetDelay(ButtonsAnimationDelay * i);
            buttons[i].blocksRaycasts = !isSettingsOpened;
        }

        isSettingsOpened = !isSettingsOpened;
    }

    [System.Serializable]
    private struct AudioButton
    {
        public Image ButtonIcon;
        [Space]
        public Sprite EnabledSprite;
        public Sprite DisabledSprite;
    }
}
