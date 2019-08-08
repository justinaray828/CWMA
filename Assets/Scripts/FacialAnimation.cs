using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

//Enum values need to be the same as the Facial Expression Animator Parameters
public enum Expression { Happy, Sad, Uncomfortable, Relief, Shocked };

/// <summary>
/// Simple script to set the face of a character
/// </summary>
public class FacialAnimation : MonoBehaviour
{
    public string currentExpression;
    public GameObject facialExpressionGameObject;
    public DialoguePanelController dialoguePanelController;
    [Header("Name to be displayed for talking character")]
    public string characterName;

    private Animator animator;
    private string talking = "Talking";

    // Start is called before the first frame update
    void Start()
    {
        animator = facialExpressionGameObject.GetComponent<Animator>();
        animator.SetBool(currentExpression, true);
    }

    /// <summary>
    /// In yarn use the command like below. All expressions are in the enum at the top of this code.
    /// 'ChangeExpression CharacterName Expression'
    /// </summary>
    /// <param name="expression"></param>
    [YarnCommand("ChangeExpression")]
    public void ChangeExpression(string expression)
    {
        animator.SetBool(expression, false);
        currentExpression = expression;
        animator.SetBool(expression, true);

        if (expression == talking)
            SetSpeakerOnPanel();
    }

    public void StopTalking()
    {
        if(currentExpression == talking)
        {
            animator.SetBool(talking, false);
        }
    }

    private void SetSpeakerOnPanel()
    {
        dialoguePanelController.nameText.text = characterName;
    }
}
