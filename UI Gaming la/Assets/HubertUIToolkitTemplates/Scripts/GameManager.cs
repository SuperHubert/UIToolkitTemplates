using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField]
    private DialogueManager dialogueManager;
    [SerializeField]
    private TextAsset debugTextAsset;
    
    private IEnumerator Start() 
    {
        InputManager.Enable();
        InputManager.SwitchToGameControls();

        yield return null;
        
        if (debugTextAsset != null)
        {
            dialogueManager.EnterDialogue(debugTextAsset);
        }
    }
}
