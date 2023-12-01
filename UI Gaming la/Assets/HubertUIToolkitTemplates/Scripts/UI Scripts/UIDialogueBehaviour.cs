using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIDialogueBehaviour : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    private VisualElement rootVisualElement;
    
    private VisualElement dialogueElement;
    private VisualElement speakerElement;
    private Label speakerText;
    private Label dialogueText;
    private List<Button> choiceButtons = new List<Button>();
    public event Action<int> OnChoiceSelected; 
    
    private void Start()
    {
        rootVisualElement = document.rootVisualElement;
        
        dialogueElement = rootVisualElement.Q<VisualElement>("dialogue-group");
        speakerElement = rootVisualElement.Q<VisualElement>("speaker-group");
        speakerText = speakerElement.Q<Label>("speaker-text");
        dialogueText = rootVisualElement.Q<Label>("dialogue-text");

        for (int i=0;i < 8;i++)
        {
            var index = i;
            var button = rootVisualElement.Q<Button>($"choice-button-{i}");
            button.RegisterCallback<ClickEvent>(LocalChoiceMethod);
            choiceButtons.Add(button);
            
            continue;
            
            void LocalChoiceMethod(ClickEvent ctx)
            {
                OnChoiceSelected?.Invoke(index);
            }
        }
        
        Show(false);
    }

    public void Show(bool value = true)
    {
        dialogueElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        if(!value) HideChoices();
    }
    
    public void HideChoices()
    {
        foreach (var button in choiceButtons)
        {
            button.style.display = DisplayStyle.None;
        }
    }

    public void ShowChoices(List<string> questions)
    {
        if(questions.Count <= 0) return;
        
        for (var i = 0; i < choiceButtons.Count; i++)
        {
            var button = choiceButtons[i];
            if (i < questions.Count)
            {
                button.style.display = DisplayStyle.Flex;
                button.text = questions[i];
            }
            else
            {
                button.style.display = DisplayStyle.None;
            }
        }
    }
    
    
    public void ShowSpeaker(bool value = true)
    {
        speakerText.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
    }
    
    public void SetSpeaker(string speaker)
    {
        speakerText.text = speaker;
    }
    
    public void SetText(string dialogue)
    {
        dialogueText.text = dialogue;
    }

    
}
