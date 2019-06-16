using UnityEngine;

//Enum values need to be the same as the Facial Expression Animator Parameters
public enum Expressions { Happy, Sad, Uncomfortable, Relief, Shocked };

/// <summary>
/// Simple script to set the face of a character
/// </summary>
public class FacialExpressions : MonoBehaviour
{
    [SerializeField] private Expression currentExpression;
    [SerializeField] private GameObject facialExpressionGameObject;
    private Animator animator;

    // Start is called before the first frame update
    private void Start()
    {
        animator = facialExpressionGameObject.GetComponent<Animator>();
        animator.SetBool(currentExpression.ToString(), true);
    }

    public void ChangeExpression(Expression expression)
    {
        animator.SetBool(currentExpression.ToString(), false);
        currentExpression = expression;
        animator.SetBool(currentExpression.ToString(), true);
    }
}