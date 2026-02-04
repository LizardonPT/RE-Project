using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public InputActionProperty actionValue;
    public InputActionProperty actionButton;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float value = actionValue.action.ReadValue<float>();
        Debug.Log("Value: " + value);

        bool buttonPressed = actionButton.action.IsPressed();
        Debug.Log("Button Pressed: " + buttonPressed);
    }
}
