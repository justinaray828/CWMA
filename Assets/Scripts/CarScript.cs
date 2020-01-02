using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Yarn.Unity;

public class CarScript : MonoBehaviour
{
    private backgroundcylinder bc;
    private Coroutine currentCR;
    private Animator anim;

    public float speedChangeTime;

    // Start is called before the first frame update
    void Start()
    {
        bc = FindObjectOfType<backgroundcylinder>();
        anim = GameObject.Find("Main Camera").GetComponent<Animator>();
    }

    void OnEnable()
    {
        EventManager.StartListening("Car_go", CarGo);
        EventManager.StartListening("Car_stop", CarStop);
    }

    void OnDisable()
    {
        EventManager.StopListening("Car_go", CarGo);
        EventManager.StopListening("Car_stop", CarStop);
    }

    [YarnCommand("CarStop")]
    public void CarStop()
    {
        if (currentCR != null) { StopCoroutine(currentCR); }
        currentCR = StartCoroutine(SlowVid());
        anim.SetBool("carStop", true);
    }

    [YarnCommand("CarGo")]
    public void CarGo()
    {
        if (currentCR != null) { StopCoroutine(currentCR); }
        StartCoroutine(SpeedUpVid());
        anim.SetBool("carStop", false);
    }

    private IEnumerator SlowVid()
    {
        float startingSpeed = bc.speed;
        while (bc.speed > 0f)
        {
            bc.speed -= startingSpeed * (Time.deltaTime/speedChangeTime);
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
            bc.speed += endSpeed * (Time.deltaTime / speedChangeTime);
            yield return null;
        }
        bc.speed = endSpeed;
        currentCR = null;
    }

}
