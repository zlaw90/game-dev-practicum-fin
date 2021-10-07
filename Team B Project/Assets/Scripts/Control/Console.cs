using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.IO;

public class Console : MonoBehaviour
{
    public enum devType { dev_JonStarfighter, };
    public static List<Ship> dev_fleet = new List<Ship>();
    public static bool cheats = false;
    InputField input;
    // Start is called before the first frame update
    private void Awake()
    {
        input = GetComponentInChildren<InputField>();

        foreach(devType type in Enum.GetValues(typeof(devType)))
        {
            var shipObject = Resources.Load(Path.Combine("Ships", "Prefabs", type.ToString())) as GameObject; // Assets/Resources/Ships/Prefabs/[Ship Name]
            if (shipObject != null && dev_fleet != null)
                dev_fleet.Add(shipObject.GetComponent<Ship>());
            else
                continue;
        }
    }
    public void Submit()
    {
        string command = input.text;
        input.text = "";
        input.caretPosition = 0;
      // Debug.Log("Command: " + command);
        CheckCommand(command);
        gameObject.SetActive(false);
    }
    void CheckCommand(string command)
    {
        if (command.ToLower() == "sv_cheats 1") //Enable Cheats
        {
            Debug.Log("Cheats Activated");
            cheats = true;
        }
        else if(command.ToLower() == "sv_cheats 0") //Disable Cheats
        {
            Debug.Log("Cheats Deactivated");
            cheats = false;
        }
        else if(command.ToLower() == "thats so metal") //Gain Metal resources
        {
            if (cheats) {
                Debug.Log("+10000 metal");
                ControlledPlayer.Instance.AddResources(new Resource(10000, Resource.ResourceKind.metal));
            }
            else {
                Debug.Log("Enable Cheats First");
            }
        }
        else if(command.ToLower() == "miniature sun") //Gain Fuel resources
        {
            if (cheats) {
                Debug.Log("+10000 fuel");
                ControlledPlayer.Instance.AddResources(new Resource(10000, Resource.ResourceKind.fuel));
            }
            else {
                Debug.Log("Enable Cheats First");
            }
        }
        else if(command.ToLower() == "unfixed bugs") //spawn the dev fleet
        {
            if (cheats) {
                Debug.Log("unfixed bugs, dev fleet");
                foreach(Ship ship in dev_fleet) {
                    for (int i = 0; i < 10; i++) {
                        GameObject shipObj = GameObject.Instantiate(ship.gameObject, ControlledPlayer.Instance.playerBase.transform.position, ControlledPlayer.Instance.playerBase.transform.rotation, ControlledPlayer.Instance.transform);
                        shipObj.GetComponent<Ship>().SetOwner(ControlledPlayer.Instance);
                        shipObj.layer = 8; // 8 is the player layer
                    }
                }
            }
            else {
                Debug.Log("Enable Cheats First");
            }
        }
        else if (command.ToLower() == "adorable bugs") // Spawn basic fighters
        {
            if (cheats) {
                Debug.Log("Adorable bugs, 100 basic fighters");
                var CostFuel = (int)StarShipUtilities.Instance.ShipDictionary[Ship.shipType.BasicStarfighter].price.fuel;
                var Cost = (int)StarShipUtilities.Instance.ShipDictionary[Ship.shipType.BasicStarfighter].price.metal;
                ControlledPlayer.Instance.AddResources(new Resource(CostFuel * 100, Resource.ResourceKind.fuel));
                ControlledPlayer.Instance.AddResources(new Resource(Cost * 100, Resource.ResourceKind.metal));
                for (int i = 0; i < 100; i++)
                    ControlledPlayer.Instance.SpawnUnit(Ship.shipType.BasicStarfighter);
            }
            else {
                Debug.Log("Enable Cheats First");
            }
        }
        else if (command.ToLower() == "for sparta") // spawn spartan fighters
        {
            if (cheats) {                
                Debug.Log("for sparta, 300 spartan fighters");
                var CostFuel = (int)StarShipUtilities.Instance.ShipDictionary[Ship.shipType.SpartanStarfighter].price.fuel;
                var Cost = (int)StarShipUtilities.Instance.ShipDictionary[Ship.shipType.SpartanStarfighter].price.metal;
                ControlledPlayer.Instance.AddResources(new Resource(CostFuel * 300, Resource.ResourceKind.fuel));
                ControlledPlayer.Instance.AddResources(new Resource(Cost * 300, Resource.ResourceKind.metal));
                for (int i = 0; i < 300; i++)
                    ControlledPlayer.Instance.SpawnUnit(Ship.shipType.SpartanStarfighter);
            }
            else {
                Debug.Log("Enable Cheats First");
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
