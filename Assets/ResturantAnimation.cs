using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Animator))]
public class ResturantAnimation : MonoBehaviour
{
    private Animator restaurantAnimator;

    // Start is called before the first frame update
    void Start()
    {
        restaurantAnimator = GetComponent<Animator>();
        restaurantAnimator.SetBool("ReaLeaving", true); ;
    }

    [YarnCommand("GinaLeaves")]
    public void GinaLeaves()
    {
        SetGinaLeavesBool(true);
    }

    [YarnCommand("GinaReturns")]
    public void GinaReturns()
    {
        SetGinaLeavesBool(false);
    }

    [YarnCommand("ReaLeaves")]
    public void ReaLeaves()
    {
        SetReaLeavesBool(true);
    }

    [YarnCommand("ReaReturns")]
    public void ReaReturns()
    {
        SetReaLeavesBool(false);
    }

    private void SetGinaLeavesBool(bool boolean)
    {
        EventManager.TriggerEvent("GinaWalking");
        restaurantAnimator.SetBool("GinaLeaving", boolean);
    }

    private void SetReaLeavesBool(bool boolean)
    {
        if (boolean) { EventManager.TriggerEvent("ReaWalking"); }
        else { EventManager.TriggerEvent("ReaWalkingAway"); }
        restaurantAnimator.SetBool("ReaLeaving", boolean);
    }
}
