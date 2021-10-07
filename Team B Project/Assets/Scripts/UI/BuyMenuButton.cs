using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class BuyMenuButton : MonoBehaviour
{
    public Ship.shipType ship;

    public string shipName;
    public string shipDesc;
    [NonSerialized] public Sprite shipImage;
    [NonSerialized] public Color shipImageColor;
    [NonSerialized] public int attackPts;
    [NonSerialized] public int defensePts;
    [NonSerialized] public int moveSpeedPts;
    [NonSerialized] public int AtkSpeedPts;
    [NonSerialized] public int rangePts;
    [NonSerialized] public int fuelCost;
    [NonSerialized] public int metalCost;

    public void Start()
    {
        var ship = StarShipUtilities.Instance.ShipDictionary[this.ship];
        var spriteRenderer = ship.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        shipImage = spriteRenderer.sprite;
        shipImageColor = spriteRenderer.color;
        attackPts = ship is AttackShip ? (ship as AttackShip).attackStrength.Value : 0;
        defensePts = ship.armorStrength.Value;
        moveSpeedPts = ship.maxSpeed.Value;
        AtkSpeedPts = ship is AttackShip ? (ship as AttackShip).attackSpeed.Value : 0;
        rangePts = ship is AttackShip ? (ship as AttackShip).attackRange.Value : 0;
        fuelCost = (int)ship.price.fuel;
        metalCost = (int)ship.price.metal;
    }
}
