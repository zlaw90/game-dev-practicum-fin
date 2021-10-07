using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AIPlayer : IPlayer
{
    public float timeBetweenActions = 3.0f;
    public float currentTime = 0.0f;
    public static AIPlayer Instance;
    public override void Awake()
    {
        base.Awake();
        Instance = this;
    }
    public override List<Planet> OwnedPlanets()
    {
        return _planets.Where(x => x.control == Planet.controlEnum.player2).ToList();
    }

    public override void Update()
    {
        base.Update();

        currentTime += Time.deltaTime;
        if (currentTime >= timeBetweenActions)
        {
            currentTime = 0f;
            //timeBetweenActions++;
            makeDecision();
            if (Console.cheats) {
                doCheats();
            }
        }
    }

    void makeDecision()
    {
        int myAttackShips = Fleet.Ships.Where(x => x is AttackShip).Count();
        int myTransportShips = Fleet.Ships.Where(x => x is TransportShip).Count();
        int myTransportFuelShips = Fleet.Ships.Where(x => x is FuelTransportShip).Count();
        int myMetalTransportShips = myTransportShips - myTransportFuelShips;
        int enemyAttackShips = Fleet.EnemyShips.Where(x => x is AttackShip).Count();
        int enemyTransportShips = Fleet.EnemyShips.Where(x => x is TransportShip).Count();
        uint attackShipPrice = StarShipUtilities.Instance.ShipDictionary[Ship.shipType.BasicStarfighter].price.metal;
        uint attackShipFuelPrice = StarShipUtilities.Instance.ShipDictionary[Ship.shipType.BasicStarfighter].price.fuel;
        uint transportShipPrice = StarShipUtilities.Instance.ShipDictionary[Ship.shipType.Freighter].price.metal;
        uint transportShipFuelPrice = StarShipUtilities.Instance.ShipDictionary[Ship.shipType.Freighter].price.fuel;
        bool canBuyAttack = Resources[Resource.ResourceKind.metal].amount >= attackShipPrice && Resources[Resource.ResourceKind.fuel].amount >= attackShipFuelPrice;
        bool canBuyTransport = Resources[Resource.ResourceKind.metal].amount >= transportShipPrice && Resources[Resource.ResourceKind.fuel].amount >= transportShipFuelPrice; ;

        if (myTransportShips > 0 && myAttackShips <= enemyAttackShips && canBuyAttack)
        {
            SpawnUnit(Ship.shipType.BasicStarfighter);
        }
        else if(enemyAttackShips <= myAttackShips && myTransportShips <= enemyTransportShips && canBuyTransport)
        {
            SpawnUnit(Ship.shipType.Freighter);
        }
        else
        {
            int rand = UnityEngine.Random.Range(0, 5);
            if (rand == 0 && canBuyAttack)
                SpawnUnit(Ship.shipType.BasicStarfighter);
            else if(canBuyTransport)
            {
             //   Debug.Log($"Transports: {myTransportShips} Fuel: {myTransportFuelShips} Metal: {myMetalTransportShips}");
                if (myTransportFuelShips == 0 && myTransportShips > 0)
                    SpawnUnit(Ship.shipType.FuelFreighter);
                else if ( ((float)myTransportFuelShips / myTransportShips) < 0.3)
                    SpawnUnit(Ship.shipType.FuelFreighter);
                else
                    SpawnUnit(Ship.shipType.Freighter);
            }
        }
        // Debug Ship Spawning
        //  SpawnUnit(Ship.shipType.Freighter);
        //  SpawnUnit(Ship.shipType.BasicStarfighter);
        //  AddResources(new Resource(1000, Resource.ResourceKind.metal));
        //  AddResources(new Resource(1000, Resource.ResourceKind.fuel));
    }

    void doCheats() {
        int rand = UnityEngine.Random.Range(0, 10);
        if (rand == 0) {
            rand = UnityEngine.Random.Range(0, 5);
            int Cost;
            int CostFuel;
            switch (rand) {
                case 0:
                    Debug.Log("+10000 metal : AI");
                    AIPlayer.Instance.AddResources(new Resource(10000, Resource.ResourceKind.metal));
                    break;
                case 1:
                    Debug.Log("+10000 fuel : AI");
                    AIPlayer.Instance.AddResources(new Resource(10000, Resource.ResourceKind.fuel));
                    break;
                case 2:
                    Debug.Log("unfixed bugs, dev fleet : AI");
                    foreach(Ship ship in Console.dev_fleet) {
                        for (int i = 0; i < 10; i++) {
                            GameObject shipObj = GameObject.Instantiate(ship.gameObject, AIPlayer.Instance.playerBase.transform.position, AIPlayer.Instance.playerBase.transform.rotation, AIPlayer.Instance.transform);
                            shipObj.GetComponent<Ship>().SetOwner(AIPlayer.Instance);
                            shipObj.layer = 9; // 8 is the player layer
                        }
                    }
                    break;
                case 3:
                    Debug.Log("Adorable bugs, 100 basic fighters : AI");
                    CostFuel = (int)StarShipUtilities.Instance.ShipDictionary[Ship.shipType.BasicStarfighter].price.fuel;
                    Cost = (int)StarShipUtilities.Instance.ShipDictionary[Ship.shipType.BasicStarfighter].price.metal;
                    AIPlayer.Instance.AddResources(new Resource(Cost * 100, Resource.ResourceKind.metal));
                    AIPlayer.Instance.AddResources(new Resource(CostFuel * 100, Resource.ResourceKind.fuel));
                    for (int i = 0; i < 100; i++)
                        AIPlayer.Instance.SpawnUnit(Ship.shipType.BasicStarfighter);
                    break;
                case 4:
                    Debug.Log("for sparta, 300 spartan fighters");
                    CostFuel = (int)StarShipUtilities.Instance.ShipDictionary[Ship.shipType.SpartanStarfighter].price.fuel;
                    Cost = (int)StarShipUtilities.Instance.ShipDictionary[Ship.shipType.SpartanStarfighter].price.metal;
                    AIPlayer.Instance.AddResources(new Resource(CostFuel * 300, Resource.ResourceKind.fuel));
                    AIPlayer.Instance.AddResources(new Resource(Cost * 300, Resource.ResourceKind.metal));
                    for (int i = 0; i < 300; i++)
                        AIPlayer.Instance.SpawnUnit(Ship.shipType.SpartanStarfighter);
                    break;
            }
        }
    }

}

