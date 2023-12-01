using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIMenuBehaviour : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    private VisualElement rootVisualElement;

    [SerializeField] private string gameSceneName = "TestScene";

    private VisualElement menuElement;
    private VisualElement settingsElement;
    private VisualElement creditsElement;
    
    private void Start()
    {
        rootVisualElement = document.rootVisualElement;
        
        menuElement = rootVisualElement.Q<VisualElement>("menu");
        settingsElement = rootVisualElement.Q<VisualElement>("settings");
        creditsElement = rootVisualElement.Q<VisualElement>("credits");
        
        var button = rootVisualElement.Q<Button>("play-button");
        button.text = "Play";
        button.RegisterCallback<ClickEvent>(Play);
        
        button = rootVisualElement.Q<Button>("settings-button");
        button.text = "Settings";
        button.RegisterCallback<ClickEvent>(ShowOptions);
        
        button = rootVisualElement.Q<Button>("credits-button");
        button.text = "Credits";
        button.RegisterCallback<ClickEvent>(Credits);
        
        button = rootVisualElement.Q<Button>("quit-button");
        button.text = "Quit";
        button.RegisterCallback<ClickEvent>(Quit);
        
        //settings
        button = settingsElement.Q<Button>("close-button");
        button.RegisterCallback<ClickEvent>(ShowMenu);
        
        button = creditsElement.Q<Button>("close-button");
        button.RegisterCallback<ClickEvent>(ShowMenu);
    }

    private void Play(EventBase ctx)
    {
        SceneManager.LoadScene(gameSceneName);
    }
    
    private void ShowOptions(EventBase ctx)
    {
        settingsElement.style.display = DisplayStyle.Flex;
        menuElement.style.display = DisplayStyle.None;
    }

    private void ShowMenu(EventBase ctx)
    {
        settingsElement.style.display = DisplayStyle.None;
        creditsElement.style.display = DisplayStyle.None;
        menuElement.style.display = DisplayStyle.Flex;
    }

    private void Credits(EventBase ctx)
    {
        creditsElement.style.display = DisplayStyle.Flex;
        menuElement.style.display = DisplayStyle.None;
    }
    
    private void Quit(EventBase ctx)
    {
        Application.Quit();
    }
}
