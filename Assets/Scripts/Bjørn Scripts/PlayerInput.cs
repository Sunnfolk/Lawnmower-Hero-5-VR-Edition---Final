using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public int radioOn = 1;
    public int rotateDir;

    private void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            if (radioOn == 1) radioOn = 0;
            else radioOn = 1;
        }
        
        if (Keyboard.current.qKey.isPressed) rotateDir = 1;
        if (Keyboard.current.eKey.isPressed) rotateDir = -1;
        if (!Keyboard.current.eKey.isPressed && !Keyboard.current.qKey.isPressed) rotateDir = 0;
    }
}
