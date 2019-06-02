using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class DialogueSystem : Yarn.Unity.DialogueUIBehaviour
{
    public GameObject dialogueContainer;
    public GameObject continuePrompt;
    [Tooltip("How quickly to show the text, in seconds per character")]
    public float textSpeed = 0.025f;
    public List<Button> optionButtons;
    public RectTransform gameControlsContainer;
    [Tooltip("This will be attached to UICanvas")]
    public DialoguePanelController dialoguePanelController;

    private Yarn.OptionChooser SetSelectedOption;

    private void Awake()
    {

    }

    public override IEnumerator RunLine(Yarn.Line line)
    {
        // Show the text
        dialoguePanelController.PopUp = true;
        dialoguePanelController.dialogueText.gameObject.SetActive(true);

        if (textSpeed > 0.0f)
        {
            // Display the line one character at a time
            var stringBuilder = new StringBuilder();

            foreach (char c in line.text)
            {
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
        while (Input.anyKeyDown == false)
        {
            yield return null;
        }

        // Hide the text and prompt
        dialoguePanelController.dialogueText.gameObject.SetActive(false);

        if (continuePrompt != null)
            continuePrompt.SetActive(false);

        dialoguePanelController.PopUp = false;
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

    /// Run an internal command.
    public override IEnumerator RunCommand(Yarn.Command command)
    {
        // "Perform" the command
        Debug.Log("Command: " + command.text);

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
}
