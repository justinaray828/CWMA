using UnityEngine;

public class InputManager : MonoBehaviour
{
    public DialoguePanelController dialoguePanelController;

    private bool keyPress = false;

    private string inputString = Inputs.STRINGS.Input.ToString();

    private void Update()
    {
        InputKeyPress();
    }

    private void InputKeyPress()
    {
        if (Input.GetButtonDown(inputString) && keyPress == false)
        {
            keyPress = true;
        }

        if (Input.GetButtonUp(inputString))
        {
            keyPress = false;
        }
    }
}