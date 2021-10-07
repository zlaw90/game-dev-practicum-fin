using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraControl : MonoBehaviour
{
    public float speed = 15f;
    
    [Header("Zoom Options")]
    [Space]
    [SerializeField] float minCamSize = 10f;
    [SerializeField] float maxCamSize = 80f;
    public float zoomSpeed = 1.6f;
    //This is used to control the camera panning.
    
    [Header("Screen  Size")]
    [Space]
    [SerializeField] int xLimit;
    [SerializeField] int zLimit;
    
    private float directionZ = 0;
    private float directionX = 0;
    
    private float edgeDirx = 0;
    private float edgeDirz = 0;

    private Vector3 newPos;

    public bool isZoomedOut = false;
    
    private void Update()
    {
        if (isZoomedOut)
            return;

        var pos = Camera.main.transform.position;
        newPos = pos;
     //   Debug.Log("pos" + pos);
        if(directionX == 0 && directionZ == 0 )
        {
            if (edgeDirx != 0 || edgeDirz != 0)
            {
                Vector3 addVec = new Vector3(edgeDirx, 0, edgeDirz) * speed * Time.deltaTime;
          //      Debug.Log("addVec" + addVec);
                newPos = Camera.main.transform.position + addVec;
            }
        }
        else if(directionX != 0 || directionZ != 0)
        {
            Vector3 addVec = new Vector3(directionX, 0, directionZ) * speed * Time.deltaTime;
          //  Debug.Log("addVec" + addVec);
            newPos = Camera.main.transform.position + addVec;
        }
      //  Debug.Log("newPos" + newPos);

        if (newPos.x < xLimit && newPos.x > -xLimit)
        {
            Camera.main.transform.position = new Vector3(newPos.x, pos.y, pos.z);
        }
        
        pos = Camera.main.transform.position;
        
        
        if (newPos.z < zLimit && newPos.z > -zLimit)
        {   
            Camera.main.transform.position = new Vector3(pos.x, pos.y, newPos.z);
        }
       
    }

    public void OnZoom(InputValue value)
    {
        if (isZoomedOut)
            return;
        var zoomDir = value.Get<Vector2>();
        if(zoomDir.y == 1f)
        {
            Camera.main.orthographicSize -= zoomSpeed;
            if (Camera.main.orthographicSize < minCamSize)
                Camera.main.orthographicSize = minCamSize;
        }
        if(zoomDir.y == -1f)
        {
            Camera.main.orthographicSize += zoomSpeed;
            if (Camera.main.orthographicSize > maxCamSize)
                Camera.main.orthographicSize = maxCamSize;
        }
    }
    public void OnPanHorizontal(InputValue value)
    {
        directionX = value.Get<float>();
    }
    public void OnPanVertical(InputValue value)
    {
        directionZ = value.Get<float>();
    }
    public void OnCameraSpeed(InputValue value)
    {
        if (value.Get<float>() > 0f) speed *=  2;
        
        else if(value.Get<float>() == 0.0f) speed /= 2;
    }
    public void OnMousePosition(InputValue value)
    {
        Vector2 mPOS = value.Get<Vector2>();
        Vector2 viewPortPOS = Camera.main.ScreenToViewportPoint(mPOS);
       
        if(viewPortPOS.x < 0.025 && viewPortPOS.y > 0)edgeDirx = -1; 
        else if(viewPortPOS.x > 0.975 && viewPortPOS.x < 1) edgeDirx = 1;
        else edgeDirx = 0;

        if(viewPortPOS.y < 0.025 && viewPortPOS.y > 0) edgeDirz = -1;
        else if(viewPortPOS.y > 0.975 && viewPortPOS.y < 1) edgeDirz = 1;
        else edgeDirz = 0;
      
        
    }


}