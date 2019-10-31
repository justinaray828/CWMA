using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

/// <summary>
/// Handles all dialogue functions
/// </summary>
public class DialoguePanelController : MonoBehaviour
{
    public RectTransform dialoguePanelRectTransform;
    public float highestPanelLocation;
    public Text dialogueText;
    public Text nameText;
    public Animator animator;
    public GameObject nameTextPanel;
    public bool dialogueEnabled;

    private float smoothTime = 0.3f;
    private float yVelocity = 0.3f;
    private float lowestPanelLocation = -115f;
    private float panelLocation;

    private string currentCharName;
    private float currentCharPitch;
    private CharacterData currentChacter;

    public void SetCurrentCharacter(CharacterData character)
    {
        currentChacter = character;
        nameText.text = character.name;
    }
    public CharacterData GetCurrentCharacter()
    {
        return currentChacter;
    }

    /// <summary>
    /// Set to true to pop up panel and false to bring it down.
    /// </summary>
    public bool PopUp
    {
        get => animator.GetBool("IsOpen");
        set => animator.SetBool("IsOpen", value);
    }

    public void AdvanceDialogue(string speakerName, string speakerText)
    {
        nameText.text = speakerName + ":";
        dialogueText.text = speakerText;
    }

    public void EnableDialogue(bool boolean)
    {
        dialogueText.gameObject.SetActive(boolean);
        nameText.gameObject.SetActive(boolean);
        nameTextPanel.SetActive(boolean);
        dialogueEnabled = boolean;
    }

    private void ToggleDialogueText(bool textState)
    {
        nameText.enabled = textState;
        nameTextPanel.SetActive(textState);
        dialogueText.enabled = textState;
    }

}