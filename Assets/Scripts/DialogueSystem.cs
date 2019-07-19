using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : Yarn.Unity.DialogueUIBehaviour
{
    public GameObject dialogueContainer;
    public GameObject continuePrompt;
    public float textSpeed;

    [Tooltip("How quickly to show the text, in seconds per character")]
    public float textSpeedDefault = 0.025f;

    public float textSpeedSlow = 0.05f;
    public float textSpeedFast = 0f;
    public List<Button> optionButtons;
    public RectTransform gameControlsContainer;

    [Tooltip("This will be attached to UICanvas")]
    public DialoguePanelController dialoguePanelController;

    public CameraTransition cameraTransition;

    public SceneHandler sceneHandler;

    private Yarn.OptionChooser SetSelectedOption;

    private bool inBrainRoom = false;
    private bool inputPressed = false;

    private void Awake()
    {
        textSpeed = textSpeedDefault;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            inputPressed = true;
        }
    }

    public override IEnumerator RunLine(Yarn.Line line)
    {
        while (inBrainRoom && !cameraTransition.isCameraZoomedIn())
        {
            dialoguePanelController.dialogueText.gameObject.SetActive(false);
            dialoguePanelController.nameText.gameObject.SetActive(false);
            yield return null;//Wait until the camera is all the way zoomed in to continue
        }

        // Show the text
        dialoguePanelController.PopUp = true;
        dialoguePanelController.dialogueText.gameObject.SetActive(true);
        dialoguePanelController.nameText.gameObject.SetActive(true);

        if (textSpeed > 0.0f)
        {
            // Display the line one character at a time
            var stringBuilder = new StringBuilder();

            foreach (char c in line.text)
            {
                //InputPressed is set to true via the Update method (which is run before coroutines each frame)
                if (inputPressed)
                {
                    dialoguePanelController.dialogueText.text = line.text;
                    inputPressed = false;
                    break;
                }
                stringBuilder.Append(c);
                dialoguePanelController.dialogueText.text = stringBuilder.ToString();
                yield return new WaitForSeconds(textSpeed);
            }
        }
        else
        {
            // Display the line immediately if textSpeed == 0
            dialoguePanelController.dialogueText.text = line.text;
        }

        // Show the 'press any key' prompt when done, if we have one
        if (continuePrompt != null)
            continuePrompt.SetActive(true);

        // Wait for any user input
        while (inputPressed == false)
        {
            yield return null;
        }

        inputPressed = false;
        if (continuePrompt != null)
            continuePrompt.SetActive(false);
    }

    private void SpeedUpText(bool firstInput)
    {
    }

    public override IEnumerator RunOptions(Yarn.Options optionsCollection,
                                        Yarn.OptionChooser optionChooser)
    {
        // Do a little bit of safety checking
        if (optionsCollection.options.Count > optionButtons.Count)
        {
            Debug.LogWarning("There are more options to present than there are" +
                             "buttons to present them in. This will cause problems.");
        }

        //empty dialogue box text
        dialoguePanelController.dialogueText.text = "";

        // Display each option in a button, and make it visible
        int i = 0;
        foreach (var optionString in optionsCollection.options)
        {
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].GetComponentInChildren<Text>().text = optionString;
            i++;
        }

        // Record that we're using it
        SetSelectedOption = optionChooser;

        // Wait until the chooser has been used and then removed (see SetOption below)
        while (SetSelectedOption != null)
        {
            yield return null;
        }

        // Hide all the buttons
        foreach (var button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    /// Called by buttons to make a selection.
    public void SetOption(int selectedOption)
    {
        // Call the delegate to tell the dialogue system that we've
        // selected an option.
        SetSelectedOption(selectedOption);

        // Now remove the delegate so that the loop in RunOptions will exit
        SetSelectedOption = null;
    }

    /// Run an internal command.
    public override IEnumerator RunCommand(Yarn.Command command)
    {
        // "Perform" the command
        Debug.Log("Command: " + command.text);
        string InnerDateScene = "setscene innerDate";

        switch (command.text)
        {
            case "setscene innerDate":
                cameraTransition.ZoomIn();
                inBrainRoom = true;
                break;
            case "setScene nextScene":
                sceneHandler.LoadNextScene();
                break;
            default:
                cameraTransition.ZoomOut();
                inBrainRoom = false;
                break;
        }

        //if (command.text.Equals(InnerDateScene))
        //{
        //    cameraTransition.ZoomIn();
        //    inBrainRoom = true;
        //}
        //else
        //{
        //    cameraTransition.ZoomOut();
        //    inBrainRoom = false;
        //}

        //if(command.text.Equals("setScene nextScene"))
        //{
        //    sceneHandler.LoadNextScene();
        //}

        yield break;
    }

    /// Called when the dialogue system has started running.
    public override IEnumerator DialogueStarted()
    {
        Debug.Log("Dialogue starting!");

        // Enable the dialogue controls.
        if (dialogueContainer != null)
            dialogueContainer.SetActive(true);

        // Hide the game controls.
        if (gameControlsContainer != null)
        {
            gameControlsContainer.gameObject.SetActive(false);
        }

        yield break;
    }

    /// Called when the dialogue system has finished running.
    public override IEnumerator DialogueComplete()
    {
        Debug.Log("Complete!");

        // Hide the dialogue interface.
        if (dialogueContainer != null)
            dialogueContainer.SetActive(false);

        // Show the game controls.
        if (gameControlsContainer != null)
        {
            gameControlsContainer.gameObject.SetActive(true);
        }

        yield break;
    }

    public void SetTextSpeedDefault()
    {
        textSpeed = textSpeedDefault;
    }

    public void SetTextSpeedSlow()
    {
        textSpeed = textSpeedSlow;
    }
}