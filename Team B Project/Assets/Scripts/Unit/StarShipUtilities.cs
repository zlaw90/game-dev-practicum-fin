using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class StarShipUtilities : MonoBehaviour
{
    public static StarShipUtilities Instance;
    public Dictionary<Ship.shipType, Ship> ShipDictionary = new Dictionary<Ship.shipType, Ship>();


    void Awake()
    {
        Instance = this;
        BuildShipDictionary();
    }


    private void BuildShipDictionary()
    {
        foreach(Ship.shipType type in Enum.GetValues(typeof(Ship.shipType)))
        {
            var shipObject = Resources.Load(Path.Combine("Ships", "Prefabs", type.ToString())) as GameObject; // Assets/Resources/Ships/Prefabs/[Ship Name]
            if (shipObject != null)
                ShipDictionary[type] = shipObject.GetComponent<Ship>();
            else
                continue;
        //    Debug.Log($"Enum Type: {type}");
            var prefabObj = ShipDictionary[type];
            foreach(Transform child in prefabObj.transform)
            {
           //     Debug.Log($"Child Name: {child.gameObject.name}");
            }
        }
    }
}
