using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class FadeScript : MonoBehaviour
{

    public GameObject FadePanel;

    [YarnCommand("Fade")]
    public void Fade()
    {
        FadePanel.SetActive(true);
    }
}
