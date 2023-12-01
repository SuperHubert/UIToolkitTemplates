using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static event Action<bool> OnGamePaused;  

    private static bool isPaused;

    public static bool IsPaused
    {
        get => isPaused;
        private set
        {
            isPaused = value;
            HandlePause();
            
        }
    }

    public static void RequestUnpause()
    {
        IsPaused = false;
    }
    
    private void OnEnable()
    {
        IsPaused = false;
    }

    private void OnDisable()
    {
        UnbindCallbacks();
    }

    private static void BindCallbacks()
    {
        InputManager.Controls.Game.Pause.performed += OnPausePerformed;
    }
    
    private static void UnbindCallbacks()
    {
        InputManager.Controls.Game.Pause.performed -= OnPausePerformed;
    }
    
    private static void HandlePause()
    {
        Time.timeScale = isPaused ? 0 : 1;

        if (isPaused)
        {
            UnbindCallbacks();
        }
        else
        {
            BindCallbacks();
        }
        
        OnGamePaused?.Invoke(isPaused);
    }

    private static void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        IsPaused = !IsPaused;
    }
}
