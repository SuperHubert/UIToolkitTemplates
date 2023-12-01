public class InputManager
{
    private GameControls controls;
    public static GameControls Controls => Instance.controls;

    private static InputManager instance;
    
    private static InputManager Instance => instance ??= new InputManager();

    private InputManager()
    {
        controls = new GameControls();
    }
    
    public static void Enable()
    {
        Controls.Enable();
    }
    
    public static void Disable()
    {
        Controls.Disable();
    }
    
    public static void SwitchToUIControls()
    {
        Controls.Game.Disable();
        Controls.UI.Enable();
    }
        
    public static void SwitchToGameControls()
    {
        Controls.UI.Disable();
        Controls.Game.Enable();
    }
}
