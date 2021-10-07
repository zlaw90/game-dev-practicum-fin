using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUITracking : MonoBehaviour
{
    ControlledPlayer player;
    [SerializeField] Text FuelAmount;
    [SerializeField] Text MetalAmount;

    // Start is called before the first frame update
    void Start()
    {
        player = ControlledPlayer.Instance;
        MetalAmount.text = player.Resources[Resource.ResourceKind.metal].amount.ToString(); 
        FuelAmount.text = player.Resources[Resource.ResourceKind.fuel].amount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        FuelAmount.text = player.Resources[Resource.ResourceKind.fuel].amount.ToString();
        MetalAmount.text = player.Resources[Resource.ResourceKind.metal].amount.ToString(); 
    }
    public void AddMetal()
    {
        player.AddResources(new Resource(100, Resource.ResourceKind.metal));
    }  
    public void AddFuel()
    {
        player.AddResources(new Resource(100, Resource.ResourceKind.fuel));
    }
}
