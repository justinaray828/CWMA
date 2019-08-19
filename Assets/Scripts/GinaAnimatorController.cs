using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Animator))]
public class GinaAnimatorController : MonoBehaviour
{
    Animator ginaAnimator;

    private void Start()
    {
        ginaAnimator = GetComponent<Animator>();
    }

    [YarnCommand("ginaLeaves")]
    public void GinaLeaves()
    {
        SetGinaLeavesBool(true);
    }

    [YarnCommand("ginaReturns")]
    public void GinaReturns()
    {
        SetGinaLeavesBool(false);
    }

    private void SetGinaLeavesBool(bool boolean)
    {
        ginaAnimator.SetBool("ginaLeaves", boolean);
    }
}