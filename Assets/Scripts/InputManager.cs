using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    
    public DialoguePanelController dialoguePanelController;

    bool keyPress = false;

    string inputString = Inputs.STRINGS.Input.ToString();

    void Update()
    {
        InputKeyPress();
    }

    void InputKeyPress()
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
