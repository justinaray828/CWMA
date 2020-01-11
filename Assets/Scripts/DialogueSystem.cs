using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : Yarn.Unity.DialogueUIBehaviour
{
    public GameObject dialogueContainer;
    public GameObject continuePrompt;

    [Tooltip("This will be attached to Characters GameObject in scene")]
    private TalkingStop talkingStop;

    [Tooltip("How quickly to show the text, in seconds per character")]
    public float textSpeedDefault;
    public float textSpeedSlow;
    public float textSpeedFast;
    public float textPauseTimeDefault;
    public List<Button> optionButtons;
    public RectTransform gameControlsContainer;

    [HideInInspector]
    public float textSpeed;
    [HideInInspector]
    public float textPauseTime;

    [Tooltip("This will be attached to UICanvas")]
    public DialoguePanelController dialoguePanelController;

    public CameraTransition cameraTransition;

    public SceneHandler sceneHandler;

    public AudioManager audioM;
    public AudioManager audioMPrefab;

    private Yarn.OptionChooser SetSelectedOption;

    private bool inBrainRoom = false;
    private bool inBrainRoomCut = false;
    private bool inputPressed = false;
    private bool textPaused;


    private void Awake()
    {
        ResetFields();
        talkingStop = FindObjectOfType<TalkingStop>();
        audioM = FindObjectOfType<AudioManager>();
        if (audioM == null) { 
            audioM = Instantiate(audioMPrefab);
            audioM.name = "AudioManager";
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown && !PauseMenu.GameIsPaused)
        {
            //if escape is pressed, we don't want this to advance dialogue
            if (!Input.GetKeyDown(KeyCode.Escape)) { inputPressed = true; }
        }
    }

    private void ResetFields()
    {
        textSpeed = textSpeedDefault;
        textPaused = false;
        textPauseTime = textPauseTimeDefault;
    }

    public override IEnumerator RunLine(Yarn.Line line)
    {
        while (cameraTransition.startZoom)
        {
            dialoguePanelController.EnableDialogue(false);
            yield return null;//Wait until the camera is all the way zoomed in to continue
        }

        List<KeyValuePair<int, string>> commands = new List<KeyValuePair<int, string>>();
        string lineString = line.text;

        lineString = ParseLine(lineString, ref commands);

        /*foreach (KeyValuePair<int, string> command in commands)
        {
            Debug.Log(command.Key + " -> " + command.Value);
        }*/

        // Show the text
        dialoguePanelController.PopUp = true;
        dialoguePanelController.EnableDialogue(true);

        if (textSpeed > 0.0f)
        {
            // Display the line one character at a time
            var stringBuilder = new StringBuilder();
            int counter = 0;

            audioM.PlayBlip(dialoguePanelController.GetCurrentCharacter().talkingPitch);

            foreach (char c in lineString)
            {
                //InputPressed is set to true via the Update method (which is run before coroutines each frame)
                if (inputPressed)
                {
                    dialoguePanelController.dialogueText.text = lineString;
                    inputPressed = false;
                    counter = 0;
                    break;
                }
                // for each index of the string, check if a markup is set for that index
                // (This could definitely be written to be more efficient. I just jotted this down for nwo)
                foreach (KeyValuePair<int, string> command in commands)
                {
                    if(command.Key == counter) { ExecuteMarkUp(command.Value); }
                }

                //if pause markup has been executed, textPaused will be set to true
                if (textPaused)
                {
                    //loop for amount of time equal to the pause time set
                    for (float timer = textPauseTime; timer >= 0; timer -= Time.deltaTime)
                    {
                        //check for input as we go
                        if (inputPressed)
                        {
                            break;
                        }
                        yield return null;
                    }

                    textPaused = false;
                    if (inputPressed)
                    {
                        dialoguePanelController.dialogueText.text = lineString;
                        inputPressed = false;
                        counter = 0;
                        break;
                    }
                }

                counter++;
                stringBuilder.Append(c);
                dialoguePanelController.dialogueText.text = stringBuilder.ToString();
                yield return new WaitForSeconds(textSpeed);
            }
            audioM.StopBlip();
        }
        else
        {
            // Display the line immediately if textSpeed == 0
            dialoguePanelController.dialogueText.text = lineString;
        }

        ///Stops all talking in scene after text is done
        talkingStop.StopAllTalking();

        // Show the 'press any key' prompt when done, if we have one
        if (continuePrompt != null)
            continuePrompt.SetActive(true);

        // Wait for any user input
        while (inputPressed == false)
        {
            yield return null;
        }

        //reset Markup variables
        ResetFields();

        inputPressed = false;
        if (continuePrompt != null)
            continuePrompt.SetActive(false);
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
        dialoguePanelController.nameText.text = "";
        dialoguePanelController.dialogueText.text = "";
        dialoguePanelController.nameTextPanel.SetActive(false);

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

        EventManager.TriggerEvent("ButtonClicked");

        // Hide all the buttons
        foreach (var button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }
        inputPressed = false;
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
        //Debug.Log("Command: " + command.text);
        //Debug.Log("listenToJordy Variable: " + Choices.listenedToJordy);
        if(command.text.StartsWith("setscene") && inBrainRoom)
        {
            if (command.text == "setscene Dryve") { 
                EventManager.TriggerEvent("ExitBrainRoom");
                cameraTransition.ZoomOut();
            }
            else { EventManager.TriggerEvent("ExitBrainRoomQuick"); }
            inBrainRoom = false;
        }
        switch (command.text)
        {
            case "setscene innerDateCut":
                EventManager.TriggerEvent("EnterBrainRoomQuick");
                cameraTransition.ToggleBrainRoomCut();
                inBrainRoom = true;
                break;

            case "setscene innerDate":
                EventManager.TriggerEvent("EnterBrainRoom");
                cameraTransition.ZoomIn();
                inBrainRoom = true;
                break;

            case "setScene nextScene":
                sceneHandler.LoadNextScene();
                break;
            case "setscene 06_Ending_SecondDate":
                sceneHandler.LoadScene("06_Ending_SecondDate");
                break;
            case "setscene 07_Ending_Jordy":
                if (Choices.listenedToJordy) {
                    sceneHandler.LoadScene("07_Ending_Jordy");
                }else{
                    sceneHandler.LoadScene("08_Ending_Alone");
                }
                break;
                
            case "setscene 08_Ending_Alone":
                sceneHandler.LoadScene("08_Ending_Alone");
                break;
                
            case "listenToJordy":
                Choices.listenedToJordy = true;
                break;

            case "setscene 09_Credits":
                StartCoroutine(RollCredits());
                break;

            default:
                inBrainRoom = false;
                break;
        }
        yield break;
    }

    public IEnumerator RollCredits()
    {
        EventManager.TriggerEvent("FinishGame");
        FadeScript fade = FindObjectOfType<FadeScript>();
        if(fade != null)
        {
            fade.Fade();
        }
        yield return new WaitForSeconds(1.6f);
        dialoguePanelController.PopUp = false;
        yield return new WaitForSeconds(1.5f);
        sceneHandler.LoadScene("09_Credits");
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

        yield return new WaitForSeconds(5f);

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

    /// <summary>
    /// Parses out in-line markup commands.
    /// </summary>
    /// <returns>The line string with markup removed, as well as a ref to the list of commands</returns>
    /// <param name="line">Line.</param>
    /// <param name="commands">ref to Commands. Key value pairs of index, command string</param>
    private string ParseLine(string line, ref List<KeyValuePair<int, string>> commands)
    {
        bool lineMarkUp = false;
        string command = "";
        int count = 0;
        List<KeyValuePair<int, int>> removals = new List<KeyValuePair<int, int>>();

        int l = line.Length;

        //iterate through the line character by character
        for (int i = 0; i < l; i++)
        {
            //if a bracket is encountered, turn on the flag for the beginning of a markup
            if (line[i] == '[')
            {
                lineMarkUp = true;
                continue;
            }
            //if we are currenty in a markup
            if (lineMarkUp)
            {
                //if we are at the end of the markup, then add the build markup command 
                //to a key value pair, marking the index location within the string
                if (line[i] == ']')
                {
                    commands.Add(new KeyValuePair<int, string>(count, command));
                    removals.Add(new KeyValuePair<int, int>(count, command.Length + 2));
                    command = "";
                    lineMarkUp = false;
                    continue;
                }
                //otherwise, add the current character to the command string being built
                command = command + line[i];
                continue;
            }
            //we only increase the count when we are not inside a markup. 
            //aka we are only counting indexes of the final string text, with markups removed
            count++;
        }

        //once the markups have been stored, remove them from the line.
        foreach (KeyValuePair<int, int> r in removals)
        {
            line = line.Remove(r.Key, r.Value);
        }

        return line;
    }

    /// <summary>
    /// Executes in-line markup
    /// 3 possible markups:
    /// -set any field name on Dialogue System to a float. Ex: [textSpeed=0.5]
    /// -execute single word command. Ex: [slow]
    /// -reset commands Ex: [/slow]
    /// </summary>
    /// <param name="s">Command string</param>
    private void ExecuteMarkUp(string s)
    {
        var words = s.Split('=');
        //if syntax includes setting a variable equal to a value, then attempt to do so
        if (words.Length == 2)
        {
            //searches variables in this object to see if one matches the name given
            System.Reflection.FieldInfo fieldName = this.GetType().GetField(words[0]);
            if (fieldName != null)
            {
                if(float.TryParse(words[1],out float value)) { fieldName.SetValue(this, value); }
                else { Debug.LogWarning("field- " + words[0] + " not set to valid float"); }
            }
            else
            {
                Debug.LogWarning("Field name not found: " + words[0]);
            }
        }
        else
        {
            bool stopCommand = false;

            if (s.Contains("/"))
            {
                words = s.Split('/');
                if (words.Length != 2) { Debug.LogWarning("Invalid markup : " + s + " - too many slashes"); }
                stopCommand = true;
                s = words[1];
            }

            /* we could do this without a switch statement by either using a hashtable 
             * or by using other functions that would be called via reflection. 
             * but this is easiest for now.
             */
            switch (s)
            {
                case "slow":
                    if (stopCommand) { textSpeed = textSpeedDefault; }
                    else { textSpeed = textSpeedSlow; }
                    break;
                case "pause":
                    textPaused = true;
                    break;
                default:
                    Debug.LogWarning("markup not recognized: " + s);
                    break;
            }

        }

    }
}