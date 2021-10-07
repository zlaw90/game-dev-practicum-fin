//using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TransportShip : Ship
{
    [Header("Transport Debug")]
    public Resource resource;
    public ShipPropertyValue capacity;

    public GameObject destination { get; private set; }
    private Planet destinationPlanet;
    protected bool returning = false;

    private int cap;

    public virtual void SetResource()
    {
        resource = new Resource(0, Resource.ResourceKind.metal);
    }
    public override void Start()
    {
        base.Start();
        SetResource(); SetDestination(); SetCap(); 
    }
    private float planetDistance(Planet planet)
    {
        return Vector3.Distance(planet.gameObject.transform.position, owner.playerBase.gameObject.transform.position);
    }
    public IPlayer PlayerForControl(Planet.controlEnum control)
    {
        if (control == Planet.controlEnum.player1)
            return ControlledPlayer.Instance;
        else if (control == Planet.controlEnum.player2)
            return AIPlayer.Instance;
        else
            return null;
    }
    public override void Update()
    {
        base.Update();
        bool planetNoLongerValid = false;
        if (destinationPlanet != null && PlayerForControl(destinationPlanet.control) != owner)
            planetNoLongerValid = true;
        if(planetNoLongerValid)
        {
            SetDestination(false);
            return;
        }
        if (Vector3.Distance(navAgent.destination, gameObject.transform.position) < 8)
        {
            //Destination Reached
            if (!returning)
            {
                Planet planet = destination.GetComponent<Planet>();
                var acquiredResources = planet.removeResources(new Resource(cap, resource.kind));
                resource.amount += acquiredResources.amount;
                SetDestination(true);
            }
            else
            {
                owner.AddResources(resource);
                resource.amount = 0;
                SetDestination(false);

            }
        }
    }


    private void SetCap() { cap = capacity.Value * 100; }


    public void SetDestination(bool goHome = false)
    {
        returning = goHome;
        if (!goHome)
        {
            var Playerplanets = owner.OwnedPlanets();
            if(Playerplanets.Count == 0)
            {
                SetDestination(true);
                return;
            }
           // var neutralPlanets = owner.NeutralPlanets();
            var viablePlanets = Playerplanets.Where(x => x.resources.Any(y => y.kind == resource.kind)).ToList();//.Concat(neutralPlanets.Where(x => x.resources.Any(y => y.kind == resource.kind))).ToList();
         //   var idealPlanets = viablePlanets.Where(x => x.resources.Any(y => y.kind == resource.kind && y.amount >= cap));
            viablePlanets.Sort(delegate (Planet x, Planet y)
            {
                var attractCompare = planetAttractiveness(y).CompareTo(planetAttractiveness(x));
                if (attractCompare == 0)
                    return planetDistance(x).CompareTo(planetDistance(y));
                return attractCompare;
            });

            var location = viablePlanets.FirstOrDefault();
            if(location != null)
            {
                var pos = location.gameObject.transform.position;
                navAgent.SetDestination(new Vector3(pos.x, gameObject.transform.position.y, pos.z));
                destination = location.gameObject;
                destinationPlanet = location;
            }

        }
        else
        {
            var pos = owner.playerBase.transform.position;
            navAgent.SetDestination(new Vector3(pos.x, gameObject.transform.position.y, pos.z));
            destination = owner.gameObject;
            destinationPlanet = null;
        }
    }


    private float planetAttractiveness(Planet planet)
    {
        var transports = owner.Fleet.Ships.Where(x => x is TransportShip);
        var travellingShips = transports.Where(x => (x as TransportShip).destination == planet.gameObject);
        var distance = planetDistance(planet);
        //Debug.Log("Travelling Ships: " + travellingShips.Count());
        //Debug.Log("Distance: " + distance);
        // Debug.Log("Resources: " + planet.PlanetResourcesAsDictionary[resource.kind].amount);
        // Debug.Log("Resource Divided: " + planet.PlanetResourcesAsDictionary[resource.kind].amount / ((float)cap * travellingShips.Count() + 1));
        // Debug.Log("Final:" + planet.PlanetResourcesAsDictionary[resource.kind].amount / ((float)cap * travellingShips.Count() + 1) / (distance / 100));
        float attractiveness = planet.PlanetResourcesAsDictionary[resource.kind].amount / ((float)cap * travellingShips.Count() + 1);
        if(travellingShips.Count() < 3)
            attractiveness /= (distance / 100);
        return attractiveness;
        //return planet.PlanetResourcesAsDictionary[resource.kind].amount / ((float)cap * travellingShips.Count() + 1) / (distance / 100);
    }
}
