using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Yarn.Unity;

public class CarScript : MonoBehaviour
{
    private VideoPlayer vp;
    public float slowVidTime;

    // Start is called before the first frame update
    void Start()
    {
        vp = GetComponentInChildren<VideoPlayer>();
    }

    void OnEnable()
    {
        EventManager.StartListening("Car_go", CarGo);
        EventManager.StartListening("Car_stop", CarStop);
        EventManager.StartListening("Pause", PauseVideo);
        EventManager.StartListening("Unpause", PlayVideo);
    }

    void OnDisable()
    {
        EventManager.StopListening("Car_go", CarGo);
        EventManager.StopListening("Car_stop", CarStop);
        EventManager.StopListening("Pause", PauseVideo);
        EventManager.StopListening("Unpause", PlayVideo);

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

    private void PauseVideo()
    {
        vp.Pause();
    }
    private void PlayVideo()
    {
        vp.Play();
    }
}
