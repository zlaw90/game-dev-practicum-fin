using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTransportShip : TransportShip
{
    // Start is called before the first frame update
    public override void SetResource()
    {
        resource = new Resource(0, Resource.ResourceKind.fuel);
    }

}
