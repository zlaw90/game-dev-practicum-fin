using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealthbar : MonoBehaviour
{
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.down); // so it is constantly horizontal
        transform.position = new Vector3( // so it is constantly 5 world units above the ship on the z axis
            transform.parent.position.x, 
            transform.parent.position.y, 
            transform.parent.position.z + 5
        );
    }
}
