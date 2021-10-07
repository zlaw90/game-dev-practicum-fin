using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class UnitSpawnTest : MonoBehaviour
{
    GameObject[] waypoints;
    //0 = player2 : 1 = player1
    [SerializeField] GameObject[] prefab;   //made this an array to try opposing ships
    
    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("SpawnPlayer1");
        waypoints = waypoints.Concat(GameObject.FindGameObjectsWithTag("SpawnPlayer2")).ToArray();
        Debug.Log("Hello" + waypoints.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnShip()
    {
        var waypoint = waypoints[Random.Range(0,waypoints.Length)];
        if (waypoint.CompareTag("SpawnPlayer2")) {
            GameObject.Instantiate(prefab[0], waypoint.transform.position, waypoint.transform.rotation);
        }
        else {
            GameObject.Instantiate(prefab[1], waypoint.transform.position, waypoint.transform.rotation);
        }
    }
}
