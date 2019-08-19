using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Animator))]
public class ReaAnimatorController : MonoBehaviour
{
    Animator reaAnimator;

    private void Start()
    {
        reaAnimator = GetComponent<Animator>();
    }

    [YarnCommand("reaLeaves")]
    public void reaLeaves()
    {
        SetreaLeavesBool(false);
    }

    [YarnCommand("reaReturns")]
    public void reaReturns()
    {
        SetreaLeavesBool(true);
    }

    private void SetreaLeavesBool(bool boolean)
    {
        reaAnimator.SetBool("reaReturns", boolean);
    }
}