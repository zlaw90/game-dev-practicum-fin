using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    Vector3 lastPosition, currPosition;


    private void Start()
    {
        lastPosition = Camera.main.transform.position;
    }

    void Update()
    {
        currPosition = Camera.main.transform.position;
        Vector3 dif = currPosition - lastPosition;
        transform.Translate(dif/3);
        lastPosition = currPosition;
    }
}
