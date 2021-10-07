using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FleetSizeTracking : MonoBehaviour
{
    ControlledPlayer player;
    [SerializeField] Text ShipCount;

    // Start is called before the first frame update
    void Start()
    {
        player = ControlledPlayer.Instance;
        ShipCount.text = "Fleet Count: " + player.Fleet.Ships.Count.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        ShipCount.text = "Fleet Count: " + player.Fleet.Ships.Count.ToString();
    }
}
