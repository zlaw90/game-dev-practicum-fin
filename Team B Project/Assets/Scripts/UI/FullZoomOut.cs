using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullZoomOut : MonoBehaviour
{
    public CameraControl camControl;
    public Vector3 outPos = new Vector3(0, 1, 0);
    public Vector3 inPos;

    public float inFOV;
    public float outFOV = 120;

    private Vector3 moveVel = Vector3.one;
    private float zoomVel = 0;
    private float camSpeed = 0.4f;

    public void zoomOutFunc()
    {
        if (camControl.isZoomedOut) return;
        this.StopAllCoroutines();
        inPos = transform.position;
        StartCoroutine(zoomOut());
        StartCoroutine(moveCam(outPos));
    }

    public IEnumerator zoomOut()
    {
        camControl.isZoomedOut = true;

        // saving these for zooming back in
        inFOV = GetComponent<Camera>().orthographicSize;

        while (GetComponent<Camera>().orthographicSize < outFOV)
        {
            //Debug.Log(GetComponent<Camera>().orthographicSize + " / " + outFOV);
            GetComponent<Camera>().orthographicSize = Mathf.SmoothDamp(GetComponent<Camera>().orthographicSize, outFOV, ref zoomVel, camSpeed);
            if (GetComponent<Camera>().orthographicSize > 119) // this fixes a bug lol
                GetComponent<Camera>().orthographicSize = 120;
            yield return null;
        }
    }

    public void zoomInFunc()
    {
        this.StopAllCoroutines();
        StartCoroutine(zoomIn(inPos));
       // StartCoroutine(moveCam(inPos));
    }

    public IEnumerator zoomIn(Vector3 dest)
    {
        while (GetComponent<Camera>().orthographicSize > inFOV || Vector3.Distance(transform.position, dest) > 1)
        {
            GetComponent<Camera>().orthographicSize = Mathf.SmoothDamp(GetComponent<Camera>().orthographicSize, inFOV, ref zoomVel, camSpeed);
            if (GetComponent<Camera>().orthographicSize < inFOV + 1) // this fixes a bug lol
                GetComponent<Camera>().orthographicSize = inFOV;
            transform.position = Vector3.SmoothDamp(transform.position, dest, ref moveVel, camSpeed);
            yield return null;
        }
        camControl.isZoomedOut = false;
    }

    public IEnumerator moveCam(Vector3 dest)
    {
        
        while (Vector3.Distance(transform.position, dest) > 0.01)
        {
            transform.position = Vector3.SmoothDamp(transform.position, dest, ref moveVel, camSpeed);
            yield return null;
        }
    }
}
