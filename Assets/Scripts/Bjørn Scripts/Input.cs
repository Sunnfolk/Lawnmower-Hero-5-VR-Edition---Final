using UnityEngine;

public class Input : MonoBehaviour
{
    public float crankAxis;
    
    #region - InputActions
    //Get Input Controls
    private PlayerInputActions _Input;
    
    private void Awake()
    {
        _Input = new PlayerInputActions();
    }
    private void OnEnable()
    {
        _Input.Enable();
    }
    private void OnDisable()
    {
        _Input.Disable();
    }
    #endregion

    private void Update()
    {
        _Input.PlayerTest.Gascrank.performed += x => crankAxis = x.ReadValue<float>();
    }
}
