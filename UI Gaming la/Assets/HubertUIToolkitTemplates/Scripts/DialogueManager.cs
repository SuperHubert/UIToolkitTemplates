using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private ScriptableSettings settings;
    [SerializeField]
    private UIDialogueBehaviour dialogueBehaviour;

    private Story currentStory;
    private bool HasNoCurrentStory => currentStory is null;

    private Coroutine displayingLineRoutine;
    private Coroutine autoPlayRoutine;
    private string currentLine;
    private string currentLineComplete;

    private void Start()
    {
        settings.LoadSettings();
    }

    private void OnDisable()
    {
        RemoveCallbacks();
    }

    private void AddCallbacks()
    {
        InputManager.Controls.Game.MouseClick.performed += NextLine;
        settings.OnSettingsChanged += OnSettingsUpdated;

        dialogueBehaviour.OnChoiceSelected += SelectChoice;
    }

    private void RemoveCallbacks()
    {
        PauseManager.OnGamePaused -= OnGamePaused;

        InputManager.Controls.Game.MouseClick.performed -= NextLine;
        settings.OnSettingsChanged -= OnSettingsUpdated;

        dialogueBehaviour.OnChoiceSelected -= SelectChoice;
    }

    public void EnterDialogue( TextAsset inkJSON )
    {
        if( !HasNoCurrentStory ) return;

        AddCallbacks();

        CreateStory(inkJSON.text);

        dialogueBehaviour.SetText( "" );
        dialogueBehaviour.SetBackground( "default" );
        dialogueBehaviour.ShowSpeaker( false );
        dialogueBehaviour.Show();

        NextLine();
    }

    private void CreateStory(string text)
    {
        currentStory = new Story( text );
        
        currentStory.BindExternalFunction("debug",( string str ) => Debug.Log( str ) );
        currentStory.BindExternalFunction("getVariables", SyncVariables );
    }

    private void SyncVariables()
    {
        currentStory.variablesState[ "player_name" ] = settings.PlayerName;
    }

    public void ExitDialogue()
    {
        if( HasNoCurrentStory ) return;

        StopAllCoroutines();
        autoPlayRoutine = null;
        displayingLineRoutine = null;

        RemoveCallbacks();
        dialogueBehaviour.Show( false );

        currentStory = null;
    }

    private void NextLine( InputAction.CallbackContext _ )
    {
        NextLine();
    }

    private void NextLine()
    {
        if( HasNoCurrentStory ) return;

        autoPlayRoutine = null;

        if( displayingLineRoutine != null )
        {
            FastForwardCurrentLine();
            return;
        }

        var choices = currentStory.currentChoices.Count;

        if( choices > 0 )
        {
            DisplayChoices();
            return;
        }

        if( currentStory.canContinue )
        {
            dialogueBehaviour.HideChoices();

            var text = currentStory.Continue();
            if( text.Length > 0 )
                if( text[ 0 ] == '\n' )
                    text = currentStory.Continue();

            DisplayLine( text );

            HandleTags( currentStory.currentTags );

            return;
        }

        ExitDialogue();
    }

    private void FastForwardCurrentLine()
    {
        if( displayingLineRoutine != null )
        {
            StopCoroutine( displayingLineRoutine );
            displayingLineRoutine = null;
        }

        PauseManager.OnGamePaused -= OnGamePaused;

        dialogueBehaviour.SetText( currentLineComplete );

        DisplayChoices();

        TryAutoPlay();
    }

    private void DisplayLine( string text )
    {
        currentLineComplete = text;
        currentLineComplete = currentLineComplete.Replace( "%PLAYERNAME%", settings.PlayerName );

        if( settings.DialogueSpeed <= 0f )
        {
            FastForwardCurrentLine();
            return;
        }

        PauseManager.OnGamePaused += OnGamePaused;

        displayingLineRoutine = StartCoroutine( DisplayLineRoutine() );

        return;

        IEnumerator DisplayLineRoutine()
        {
            for( int i = 0; i < currentLineComplete.Length; i++ )
            {
                currentLine = currentLineComplete[ ..i ];
                dialogueBehaviour.SetText( currentLine );
                yield return new WaitForSeconds( settings.DialogueSpeed );
            }

            FastForwardCurrentLine();
        }
    }

    private void TryAutoPlay()
    {
        if( settings.AutoDialogue ) autoPlayRoutine = StartCoroutine( AutoplayRoutine() );

        return;

        IEnumerator AutoplayRoutine()
        {
            yield return new WaitForSeconds( settings.AutoDialogueDelay );

            NextLine();
        }
    }

    private void OnGamePaused( bool isPaused )
    {
        if( isPaused )
        {
            InputManager.Controls.Game.MouseClick.performed -= NextLine;
        }
        else
        {
            InputManager.Controls.Game.MouseClick.performed += NextLine;
        }
    }

    private void OnSettingsUpdated( ScriptableSettings _ )
    {
        if( settings.AutoDialogue )
        {
            if( autoPlayRoutine == null && displayingLineRoutine == null )
            {
                TryAutoPlay();
            }
        }
        else
        {
            if( autoPlayRoutine != null )
            {
                StopCoroutine( autoPlayRoutine );
                autoPlayRoutine = null;
            }
        }
    }

    private void HandleTags( List<string> currentTags )
    {
        foreach( var currentTag in currentTags )
        {
            var split = currentTag.Split( ':' );
            var key = split[ 0 ];
            var value = split.Length > 1 ? split[ 1 ] : null;

            HandleTag( key, value );
        }
    }

    private void HandleTag( string key, string value )
    {
        Debug.Log( $"Handling tag '{key}':'{value}'" );

        switch( key )
        {
            case "speaker":
                var has_speaker = value != null;
                dialogueBehaviour.ShowSpeaker( has_speaker );

                if( has_speaker )
                {
                    var speaker = value;
                    dialogueBehaviour.SetSpeaker( speaker );
                }

                break;
            case "background":
                dialogueBehaviour.SetBackground( value );
                break;
            default:
                Debug.LogWarning( $"Couldn't find key '{key}'" );
                break;
        }
    }

    private void DisplayChoices()
    {
        if( HasNoCurrentStory ) return;

        if( currentStory.currentChoices.Count > 0 )
        {
            DisplayChoices( currentStory.currentChoices );
        }
    }

    private void DisplayChoices( List<Choice> choices )
    {
        dialogueBehaviour.ShowChoices( choices.Select( choice => choice.text ).ToList() );
    }

    public void SelectChoice( int choiceIndex )
    {
        if( HasNoCurrentStory ) return;

        currentStory.ChooseChoiceIndex( choiceIndex );

        NextLine();
    }
}
