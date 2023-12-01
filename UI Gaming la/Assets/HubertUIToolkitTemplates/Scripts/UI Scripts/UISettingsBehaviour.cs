using UnityEngine;
using UnityEngine.UIElements;

public class UISettingsBehaviour : MonoBehaviour
{
    [SerializeField] private ScriptableSettings settings;
    [SerializeField] private UIDocument document;
    private VisualElement rootVisualElement;
    
    private VisualElement settingsElement;
    
    private Slider globalVolumeSlider;
    private Slider musicVolumeSlider;
    private Slider sfxVolumeSlider;
    private Slider voiceVolumeSlider;
    private Toggle autoDialogueToggle;
    private FloatField dialogueSpeedField;
    private TextField playerNameField;
    
    private void Start()
    {
        settings.LoadSettings();
        rootVisualElement = document.rootVisualElement;
        
        settingsElement = rootVisualElement.Q<VisualElement>("settings");

        globalVolumeSlider = rootVisualElement.Q<Slider>("global-volume-slider");
        globalVolumeSlider.Query<Label>().First().text = "Global Volume";
        globalVolumeSlider.RegisterCallback<ChangeEvent<float>>(OnGlobalVolumeChanged);
        
        musicVolumeSlider = rootVisualElement.Q<Slider>("music-volume-slider");
        musicVolumeSlider.Query<Label>().First().text = "Music Volume";
        musicVolumeSlider.RegisterCallback<ChangeEvent<float>>(OnMusicVolumeChanged);
        
        sfxVolumeSlider = rootVisualElement.Q<Slider>("sfx-volume-slider");
        sfxVolumeSlider.Query<Label>().First().text = "SFX Volume";
        sfxVolumeSlider.RegisterCallback<ChangeEvent<float>>(OnSFXVolumeChanged);
        
        voiceVolumeSlider = rootVisualElement.Q<Slider>("voice-volume-slider");
        voiceVolumeSlider.Query<Label>().First().text = "Voice Volume";
        voiceVolumeSlider.RegisterCallback<ChangeEvent<float>>(OnVoiceVolumeChanged);
        
        autoDialogueToggle = rootVisualElement.Q<Toggle>("dialogue-auto-toggle");
        autoDialogueToggle.Query<Label>().First().text = "Auto Dialogue";
        autoDialogueToggle.RegisterCallback<ChangeEvent<bool>>(OnAutoDialogueChanged);
        
        dialogueSpeedField = rootVisualElement.Q<FloatField>("dialogue-speed-field");
        dialogueSpeedField.Query<Label>().First().text = "Delay Between Lines";
        dialogueSpeedField.RegisterCallback<ChangeEvent<float>>(OnDialogueSpeedChanged);
        
        playerNameField = rootVisualElement.Q<TextField>("player-name-field");
        playerNameField.label = "Player Name";
        playerNameField.RegisterCallback<ChangeEvent<string>>(OnPlayerNameChanged);
        
        var button = rootVisualElement.Q<Button>("close-button");
        button.RegisterCallback<ClickEvent>(HideOptions);
        
        button = rootVisualElement.Q<Button>("reset-button");
        button.text = "Reset";
        button.RegisterCallback<ClickEvent>(ResetSettings);
        
        button = rootVisualElement.Q<Button>("save-button");
        button.text = "Save";
        button.RegisterCallback<ClickEvent>(SaveSettings);
        
        settings.OnSettingsChanged += SyncSliders;
        
        SyncSliders(settings);
    }

    private void SyncSliders(ScriptableSettings _)
    {
        globalVolumeSlider.value = settings.GlobalVolume;
        musicVolumeSlider.value = settings.MusicVolume;
        sfxVolumeSlider.value = settings.SFXVolume;
        voiceVolumeSlider.value = settings.VoiceVolume;
        autoDialogueToggle.value = settings.AutoDialogue;
        dialogueSpeedField.value = settings.DialogueSpeed;
        playerNameField.value = settings.PlayerName;
    }
    
    private void HideOptions(EventBase ctx)
    {
        settingsElement.style.display = DisplayStyle.None;
        settings.LoadSettings();
    }
    
    private void ResetSettings(ClickEvent _)
    {
        settings.Reset();
    }

    private void SaveSettings(ClickEvent _)
    {
        settings.SaveSettings();
    }
    
    private void OnGlobalVolumeChanged(ChangeEvent<float> ctx) => settings.ChangeGlobalVolume(ctx.newValue);
    private void OnMusicVolumeChanged(ChangeEvent<float> ctx) => settings.ChangeMusicVolume(ctx.newValue);
    private void OnSFXVolumeChanged(ChangeEvent<float> ctx) => settings.ChangeSFXVolume(ctx.newValue);
    private void OnVoiceVolumeChanged(ChangeEvent<float> ctx) => settings.ChangeVoiceVolume(ctx.newValue);
    private void OnAutoDialogueChanged(ChangeEvent<bool> ctx) => settings.ChangeAutoDialogue(ctx.newValue);
    private void OnDialogueSpeedChanged(ChangeEvent<float> ctx) => settings.ChangeDialogueSpeed(ctx.newValue);
    private void OnPlayerNameChanged(ChangeEvent<string> ctx) => settings.ChangePlayerName(ctx.newValue);
}
