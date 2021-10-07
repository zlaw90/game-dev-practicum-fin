using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPurchaseButtonTest : MonoBehaviour
{
    public Ship.shipType ship;
    ControlledPlayer player = ControlledPlayer.Instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PurchaseShip()
    {
        //if (player.Resources >= ship.cost) {
        ControlledPlayer.Instance.SpawnUnit(ship);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}