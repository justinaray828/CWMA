using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Yarn.Unity;

public class CarScript : MonoBehaviour
{
    private VideoPlayer vp;
    public float slowVidTime;
    public AnimationCurve ac;

    // Start is called before the first frame update
    void Start()
    {
        vp = GetComponentInChildren<VideoPlayer>();
    }

    [YarnCommand("CarStop")]
    public void CarStop()
    {
        StartCoroutine(SlowVid());
    }

    [YarnCommand("CarGo")]
    public void CarGo()
    {
        StartCoroutine(SpeedUpVid());
    }

    private IEnumerator SlowVid()
    {
        while (vp.playbackSpeed > 0.50)
        {
            vp.playbackSpeed -= 0.2f * (slowVidTime);
            Debug.Log(vp.playbackSpeed);
            yield return new WaitForSeconds(1f); ;
        }
        vp.Pause();
    }
    private IEnumerator SpeedUpVid()
    {
        vp.Play();
        while (vp.playbackSpeed < 1f)
        {
            vp.playbackSpeed += 0.2f * (slowVidTime);
            Debug.Log(vp.playbackSpeed);
            yield return new WaitForSeconds(1f); ;
        }
    }
}
