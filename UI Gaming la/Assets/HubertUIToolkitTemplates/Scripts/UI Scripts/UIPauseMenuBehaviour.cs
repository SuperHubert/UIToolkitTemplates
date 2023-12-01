using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIPauseMenuBehaviour : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    private VisualElement rootVisualElement;
    
    [SerializeField] private string menuSceneName = "MenuScene";
    
    private VisualElement menuElement;
    private VisualElement settingsElement;
    
    private void OnEnable()
    {
        rootVisualElement = document.rootVisualElement;
        
        menuElement = rootVisualElement.Q<VisualElement>("menu");
        settingsElement = rootVisualElement.Q<VisualElement>("settings");
        
        var button = rootVisualElement.Q<Button>("resume-button");
        button.text = "Resume";
        button.RegisterCallback<ClickEvent>(Resume);
        
        button = rootVisualElement.Q<Button>("settings-button");
        button.text = "Settings";
        button.RegisterCallback<ClickEvent>(ShowOptions);
        
        button = rootVisualElement.Q<Button>("menu-button");
        button.text = "Return";
        button.RegisterCallback<ClickEvent>(Return);
        
        //settings
        button = settingsElement.Q<Button>("close-button");
        button.RegisterCallback<ClickEvent>(ShowMenu);
        
        PauseManager.OnGamePaused += HandlePause;
        
        Resume(null);
    }

    private void OnDisable()
    {
        PauseManager.OnGamePaused -= HandlePause;
    }

    private void HandlePause(bool value)
    {
        if (value) ShowMenu(null);
        else HideAll();
    }
    
    private void Resume(EventBase ctx)
    {
        PauseManager.RequestUnpause();
    }

    private void HideAll()
    {
        menuElement.style.display = DisplayStyle.None;
        settingsElement.style.display = DisplayStyle.None;
    }
    
    private void ShowOptions(EventBase ctx)
    {
        settingsElement.style.display = DisplayStyle.Flex;
        menuElement.style.display = DisplayStyle.None;
    }

    private void ShowMenu(EventBase ctx)
    {
        settingsElement.style.display = DisplayStyle.None;
        menuElement.style.display = DisplayStyle.Flex;
    }
    
    private void Return(EventBase ctx)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
