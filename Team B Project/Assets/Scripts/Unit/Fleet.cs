using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(IPlayer))]
public class Fleet : MonoBehaviour
{
    private IPlayer owner;
    private IPlayer enemy;
    public List<Ship> EnemyShips
    {
        get
        {
            var ships = enemy.GetComponentsInChildren<Ship>();
            if (ships != null)
                return ships.ToList();
            else
                return new List<Ship>();
        }
    }
    public List<Ship> Ships
    {
        get
        {
            var ships = gameObject.GetComponentsInChildren<Ship>();
            if (ships != null)
                return ships.ToList();
            else
                return new List<Ship>();
        }
    }


    public void Start()
    {
        owner = GetComponent<IPlayer>();
        if (owner is ControlledPlayer)
            enemy = AIPlayer.Instance;
        else
            enemy = ControlledPlayer.Instance;
    }
}
