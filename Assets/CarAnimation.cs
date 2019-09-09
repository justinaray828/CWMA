using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Animator))]
public class CarAnimation : MonoBehaviour
{
    private Animator carAnimator;

    // Start is called before the first frame update
    void Start()
    {
        carAnimator = GetComponent<Animator>();
        SetGinaEntersBool(false);
        SetPlayerEntersBool(false);
    }

    [YarnCommand("GinaEnters")]
    public void GinaEnters()
    {
        SetGinaEntersBool(true);
    }

    [YarnCommand("PlayerEnters")]
    public void PlayerLeaves()
    {
        SetPlayerEntersBool(true);
    }

    private void SetGinaEntersBool(bool boolean)
    {
        carAnimator.SetBool("GinaEnters", boolean);
    }

    private void SetPlayerEntersBool(bool boolean)
    {
        carAnimator.SetBool("PlayerEnters", boolean);
    }
}
