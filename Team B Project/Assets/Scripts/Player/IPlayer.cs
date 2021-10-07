using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//This is a player class
[RequireComponent(typeof(Fleet))]
public abstract class IPlayer : MonoBehaviour
{
    public Dictionary<Resource.ResourceKind, Resource> Resources;
    private List<object> _ownedPlanets = new List<object>();
    public GameObject playerBase;
    
    [System.NonSerialized]
    public Fleet Fleet;
    protected static Planet[] _planets;
    public abstract List<Planet> OwnedPlanets();
    public List<Planet> NeutralPlanets() { return _planets.Where(x => x.control == Planet.controlEnum.neutral).ToList();}

    protected virtual void Start()
    {
        SpawnUnit(Ship.shipType.Freighter);
            _planets = UnityEngine.Resources.FindObjectsOfTypeAll<Planet>();
    }
    public virtual void Awake()
    {
        Resources = new Dictionary<Resource.ResourceKind, Resource>();
        Resources[Resource.ResourceKind.metal] = new Resource(450, Resource.ResourceKind.metal);
        Resources[Resource.ResourceKind.fuel] = new Resource(100, Resource.ResourceKind.fuel);
        Fleet = GetComponent<Fleet>();
    }
    public virtual void Update() {}


    public void AddResources(Resource resourceToAdd)
    {
        Resources[resourceToAdd.kind].amount += resourceToAdd.amount;
    }


    public Vector3 GetHomeLocation() {return playerBase.transform.position; }


    //Player Actions
    public void SpawnUnit(Ship.shipType unitType/*, GameObject waypoint*/)
    {
        GameObject shipPrefab = StarShipUtilities.Instance.ShipDictionary[unitType].gameObject;
        //Instantiate Ship Prefab, subtract resources
        if(Resources[Resource.ResourceKind.metal].amount >=shipPrefab.GetComponent<Ship>().price.metal && Resources[Resource.ResourceKind.fuel].amount >= shipPrefab.GetComponent<Ship>().price.fuel) //this needs to be changed to reflect
        {
            Vector2 CircleAdjust = UnityEngine.Random.insideUnitCircle.normalized * 10;
            Vector3 spawnPos = new Vector3(playerBase.transform.position.x + CircleAdjust.x, playerBase.transform.position.y, playerBase.transform.position.z + CircleAdjust.y);
            Resources[Resource.ResourceKind.metal].amount -= (int)shipPrefab.GetComponent<Ship>().price.metal;
            Resources[Resource.ResourceKind.fuel].amount -= (int)shipPrefab.GetComponent<Ship>().price.fuel;
            GameObject ship = GameObject.Instantiate(shipPrefab, spawnPos, playerBase.transform.rotation, this.transform);
            ship.GetComponent<Ship>().SetOwner(this);
            if (this is ControlledPlayer) {
                ship.layer = 8; // 8 is the player layer
            }
            else {
                ship.layer = 9; // 9 is the AI layer
                DisplayEnemySprite(ship);
            }
        }
        else Debug.Log("Not enough resources");
    }

    public void DisplayEnemySprite(GameObject ship)
    {
        var controlSprite = UnityEngine.Resources.Load<Sprite>("enemyDenotion");//load the correect sprite for this
        GameObject child = new GameObject();//create a child to add the sprite to
        SpriteRenderer renderer = child.AddComponent<SpriteRenderer>();
        renderer.sprite = controlSprite;
        child.transform.parent = ship.transform;
        child.transform.position = ship.transform.position + new Vector3(0f, 5f, 0f);
        child.transform.localScale = new Vector3(8f, 8f, 0f);
        //child.transform.rotation = new Quaternion(.9f, 0f, 0f, 1.0f);
    }

}
