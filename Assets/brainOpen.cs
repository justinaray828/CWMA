using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brainOpen : MonoBehaviour
{
    public void TriggerOpen()
    {
        EventManager.TriggerEvent("brainOpen");
    }
    public void TriggerClose()
    {
        EventManager.TriggerEvent("brainClose");
    }
}
