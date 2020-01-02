using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Yarn.Unity;

public class CarScript : MonoBehaviour
{
    //private VideoPlayer vp;
    private backgroundcylinder bc;
    public float slowVidTime;
    private Coroutine currentCR;

    // Start is called before the first frame update
    void Start()
    {
        //vp = GetComponentInChildren<VideoPlayer>();
        bc = FindObjectOfType<backgroundcylinder>();
        Debug.Log("bc: " + bc);
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
        if (currentCR != null) { StopCoroutine(currentCR); }
        currentCR = StartCoroutine(SlowVid());
    }

    [YarnCommand("CarGo")]
    public void CarGo()
    {
        if (currentCR != null) { StopCoroutine(currentCR); }
        StartCoroutine(SpeedUpVid());
    }

    private IEnumerator SlowVid()
    {
        float startingSpeed = bc.speed;
        Debug.Log("starting speed: " + startingSpeed);
        while (bc.speed > 0f)
        {
            bc.speed -= startingSpeed * (Time.deltaTime/slowVidTime);
            //Debug.Log(vp.playbackSpeed);
            yield return null;
        }
        bc.speed = 0f;
        currentCR = null;
    }

    private IEnumerator SpeedUpVid()
    {
        float endSpeed = bc.GetStartingSpeed();
        while (bc.speed < endSpeed)
        {
            bc.speed += endSpeed * (Time.deltaTime / slowVidTime);
            //Debug.Log(vp.playbackSpeed);
            yield return null;
        }
        bc.speed = endSpeed;
        currentCR = null;
    }

    private void PauseVideo()
    {
        //vp.Pause();
    }
    private void PlayVideo()
    {
        //vp.Play();
    }
}
