using System;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings", order = 1)]
public class ScriptableSettings : ScriptableObject
{
    private const float DefaultGlobalVolume = 100f;
    private const float DefaultMusicVolume = 100f;
    private const float DefaultSfxVolume = 100f;
    private const float DefaultVoiceVolume = 100f;
    private const float DefaultDialogueSpeed = 0.02f;
    private const bool DefaultAutoDialogue = false;
    private const string DefaultPlayerName = "Player";
    
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField,Range(0f,100f)] private float globalVolume = DefaultGlobalVolume;
    public event Action<float> OnGlobalVolumeChanged; 
    public float GlobalVolume
    {
        get => globalVolume;
        private set
        {
            globalVolume = value;
            audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-80f, 0f, globalVolume/100f));
            OnGlobalVolumeChanged?.Invoke(globalVolume);
            OnSettingsChanged?.Invoke(this);
        }
    }
    
    [SerializeField,Range(0f,100f)] private float musicVolume = DefaultMusicVolume;
    public event Action<float> OnMusicVolumeChanged; 
    public float MusicVolume
    {
        get => musicVolume;
        private set
        {
            musicVolume = value;
            audioMixer.SetFloat("MusicVolume",Mathf.Lerp(-80f, 0f, musicVolume/100f));
            OnMusicVolumeChanged?.Invoke(musicVolume);
            OnSettingsChanged?.Invoke(this);
        }
    }
    
    [SerializeField,Range(0f,100f)] private float sfxVolume = DefaultSfxVolume;
    public event Action<float> OnSFXVolumeChanged;
    public float SFXVolume
    {
        get => sfxVolume;
        private set
        {
            sfxVolume = value;
            audioMixer.SetFloat("SFXVolume", Mathf.Lerp(-80f, 0f, sfxVolume/100f));
            OnSFXVolumeChanged?.Invoke(sfxVolume);
            OnSettingsChanged?.Invoke(this);
        }
    }
    
    [SerializeField,Range(0f,100f)] private float voiceVolume = DefaultVoiceVolume;
    public event Action<float> OnVoiceVolumeChanged;
    public float VoiceVolume
    {
        get => voiceVolume;
        private set
        {
            voiceVolume = value;
            audioMixer.SetFloat("VoiceVolume", Mathf.Lerp(-80f, 0f, voiceVolume/100f));
            OnVoiceVolumeChanged?.Invoke(voiceVolume);
            OnSettingsChanged?.Invoke(this);
        }
    }
    
    [Header("Dialogue Settings")]
    [SerializeField,Min(0)] private Vector2 dialogueSpeedRange = new Vector2(0f,2f);
    [SerializeField,Range(0f,1f)] private float dialogueSpeed = DefaultDialogueSpeed;
    public event Action<float> OnDialogueSpeedChanged; 
    public float DialogueSpeed
    {
        get => dialogueSpeed;
        private set
        {
            dialogueSpeed = value;
            OnDialogueSpeedChanged?.Invoke(dialogueSpeed);
            OnSettingsChanged?.Invoke(this);
        }
    }
    
    [SerializeField] private bool autoDialogue = DefaultAutoDialogue;
    [field: SerializeField] public float AutoDialogueDelay { get; private set; } = 1f;
    public event Action<bool> OnAutoDialogueChanged;
    public bool AutoDialogue
    {
        get => autoDialogue;
        private set
        {
            autoDialogue = value;
            OnAutoDialogueChanged?.Invoke(autoDialogue);
            OnSettingsChanged?.Invoke(this);
        }
    }

    [SerializeField] private string playerName = DefaultPlayerName;
    public event Action<string> OnPlayerNameChanged; 
    public string PlayerName
    {
        get => playerName;
        private set
        {
            playerName = value;
            OnPlayerNameChanged?.Invoke(playerName);
            OnSettingsChanged?.Invoke(this);
        }
    }

    public event Action<ScriptableSettings> OnSettingsChanged;

    public void Reset()
    {
        GlobalVolume = DefaultGlobalVolume;
        MusicVolume = DefaultMusicVolume;
        SFXVolume = DefaultSfxVolume;
        VoiceVolume = DefaultVoiceVolume;
        DialogueSpeed = DefaultDialogueSpeed;
        AutoDialogue = DefaultAutoDialogue;
        PlayerName = DefaultPlayerName;
    }

    public void LoadSettings()
    {
        GlobalVolume = PlayerPrefs.GetFloat("GlobalVolume", DefaultGlobalVolume);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", DefaultMusicVolume);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", DefaultSfxVolume);
        VoiceVolume = PlayerPrefs.GetFloat("VoiceVolume", DefaultVoiceVolume);
        DialogueSpeed = PlayerPrefs.GetFloat("DialogueSpeed", DefaultDialogueSpeed);
        AutoDialogue = PlayerPrefs.GetInt("AutoDialogue", DefaultAutoDialogue ? 1 : 0) == 1;
        PlayerName = PlayerPrefs.GetString("PlayerName", DefaultPlayerName);
    }
    
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("GlobalVolume", GlobalVolume);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
        PlayerPrefs.SetFloat("VoiceVolume", VoiceVolume);
        PlayerPrefs.SetFloat("DialogueSpeed", DialogueSpeed);
        PlayerPrefs.SetInt("AutoDialogue", AutoDialogue ? 1 : 0);
        PlayerPrefs.SetString("PlayerName", PlayerName);
        
        OnSettingsChanged?.Invoke(this);
    }
    
    public void ChangeGlobalVolume(float value)
    {
        GlobalVolume = value;
    }
    
    public void ChangeMusicVolume(float value)
    {
        MusicVolume = value;
    }
    
    public void ChangeSFXVolume(float value)
    {
        SFXVolume = value;
    }
    
    public void ChangeVoiceVolume(float value)
    {
        VoiceVolume = value;
    }
    
    public void ChangeDialogueSpeed(float value)
    {
        DialogueSpeed = value;
    }
    
    public void ChangeAutoDialogue(bool value)
    {
        AutoDialogue = value;
    }
    
    public void ChangePlayerName(string value)
    {
        PlayerName = value;
    }
}
