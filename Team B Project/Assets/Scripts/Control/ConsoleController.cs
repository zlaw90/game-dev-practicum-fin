using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
public class ConsoleController : MonoBehaviour
{
    public GameObject console;
    internal static Key[] konami = new Key[] { Key.UpArrow, Key.UpArrow, Key.DownArrow, Key.DownArrow, Key.LeftArrow, Key.RightArrow, Key.LeftArrow, Key.RightArrow, Key.B, Key.A };
    private int konamiIndex = 0;
   // bool keyboard = false;
    // Start is called before the first frame update
    void Start()
    {

    }
    void CheckInput()
    {
       // Debug.Log(key);
        if (Keyboard.current[konami[konamiIndex]].wasPressedThisFrame)
            konamiIndex++;
        else
            konamiIndex = 0;
        if (konamiIndex >= konami.Length)
        {
            konamiIndex = 0;
            Konami();
        }
        if (Keyboard.current.backquoteKey.wasPressedThisFrame)
        {
            ActivateConsole();
        }

    }
    public void ActivateConsole()
    {
        console.gameObject.SetActive(true);
        console.GetComponentInChildren<UnityEngine.UI.InputField>().ActivateInputField();
    }
    public void Konami()
    {
        if (Console.cheats == false) return;
        Debug.Log("Konami Achieved");
        var attackCost = (int)StarShipUtilities.Instance.ShipDictionary[Ship.shipType.BasicStarfighter].price.metal;
        var attackFCost = (int)StarShipUtilities.Instance.ShipDictionary[Ship.shipType.BasicStarfighter].price.fuel;
        var transportCost = (int)StarShipUtilities.Instance.ShipDictionary[Ship.shipType.Freighter].price.metal;
        var transportFCost = (int)StarShipUtilities.Instance.ShipDictionary[Ship.shipType.Freighter].price.fuel;
        ControlledPlayer.Instance.AddResources(new Resource(attackCost * 50, Resource.ResourceKind.metal));
        ControlledPlayer.Instance.AddResources(new Resource(attackFCost * 50, Resource.ResourceKind.fuel));
        ControlledPlayer.Instance.AddResources(new Resource(transportCost * 50, Resource.ResourceKind.metal));
        ControlledPlayer.Instance.AddResources(new Resource(transportFCost * 50, Resource.ResourceKind.fuel));

        for (int i = 0; i < 50; i++)
            ControlledPlayer.Instance.SpawnUnit(Ship.shipType.Freighter);
        for (int i = 0; i < 50; i++)
            ControlledPlayer.Instance.SpawnUnit(Ship.shipType.BasicStarfighter);

    }
    // Update is called once per frame
    void Update()
    {
     //   foreach(Key key in Enum.GetValues(typeof(Key)))
     //   {
     //       if (key == Key.None || key == Key.IMESelected || key == Key.PrintScreen) continue;
     //       if (Keyboard.current[key].wasPressedThisFrame)
     //           CheckInput();
     //   }
        if (Keyboard.current.anyKey.wasPressedThisFrame)
            if(!Keyboard.current.printScreenKey.wasPressedThisFrame)
                CheckInput();

    }
}
