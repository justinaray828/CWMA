using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Yarn.Unity;

public class CarScript : MonoBehaviour
{
    private VideoPlayer vp;

    // Start is called before the first frame update
    void Start()
    {
        vp = GetComponentInChildren<VideoPlayer>();
    }

    [YarnCommand("CarStop")]
    public void CarStop()
    {
        vp.Pause();
    }

    [YarnCommand("CarGo")]
    public void CarGo()
    {
        vp.Play();
    }
}
