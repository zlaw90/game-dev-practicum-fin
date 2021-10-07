using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public IPlayer Owner;
    public List<Collider> nearbyShips = new List<Collider>();
    private static bool gameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        if (gameOver) return;
        nearbyShips.RemoveAll(x => x == null);
        foreach(var ship in nearbyShips)
        {
            var shipComponent = ship.GetComponent<Ship>();
            if(Vector3.Distance(ship.gameObject.transform.position, transform.position) <= 8)
            {
                if (shipComponent.owner != Owner)
                    if (Owner is ControlledPlayer)
                    {
                        gameOver = true;
                        ControlledPlayer.Instance.GameEnd(false);
                    }
                    else
                    {
                        gameOver = true;
                        ControlledPlayer.Instance.GameEnd(true);
                    }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        var ship = other.gameObject.GetComponent<Ship>();
        if(ship is AttackShip attacker)
        {
            if(!nearbyShips.Contains(other))
                nearbyShips.Add(other);

        }
    }
    private void OnTriggerExit(Collider other)
    {
     //   if (nearbyShips.Contains(other))
     //       nearbyShips.Remove(other);
    }

}
