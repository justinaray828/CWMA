using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundcylinder : MonoBehaviour
{

    public float speed;
    private float startingSpeed;

    void OnEnable()
    {
        startingSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, -Time.deltaTime * speed, 0, Space.Self);
    }

    public float GetStartingSpeed()
    {
        return startingSpeed;
    }
}
