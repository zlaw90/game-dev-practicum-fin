using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class AttackShip : Ship
{
    public ShipPropertyValue attackRange;
    public ShipPropertyValue attackStrength;
    public ShipPropertyValue attackSpeed;
    float setDestinationRange = 8f;//distance from a planet this ship must be to gain control of it and move to next target
    string[] planetList = { "Player2MainPlanet", "Player2Planet1", "Player2Planet3", "Player2Planet2", "NeutralTop2", "NeutralTopMain", "NeutralTop3", "NeutralTop1", "NeutralCenter1", "NeutralPlanet3", "NeutralPlanet2", "NeutralCenterMain", "NeutralPlanet6", "NeutralPlanet4", "NeutralPlanet5", "NeutralBottom1", "NeutralBottomMain", "NeutralBottom3", "NeutralBottom2", "Player1Planet2", "Player1Planet3", "Player1Planet1", "Player1MainPlanet" };
    //the string array above lists the order planets are to be visited, for player ships -> start at Length-1 and move towords 0, for AI ships -> start at 0 and move towords Length-1
    int planetTargetIndex;//used to track which planet this ship is targeting

    [Header("Attack Debug")]
    public bool isFiring = false;
    public List<GameObject> targetList = new List<GameObject>();

    // 3 is too small for attackRange, use attackRange(3) to calculate points of balance and use (attackRange * sale) to set actual attack range 
    private float attackScale = 10;
    private int trueAttack = 0;
    private float attackTimer;
    private int nextTarget;


    private void SetAttackRange() { GetComponent<SphereCollider>().radius = attackRange.Value * attackScale; }
    private void SetAttackStrength() { trueAttack = attackStrength.Value * 2; }
    private void SetAttackSpeed() { }


    public override void Start()
    {
        base.Start();

        SetAttackRange();
        SetAttackStrength();
        attackTimer = attackSpeed.Value;
        UpdateDestinationPlanetIndex();

    }

    void UpdateDestinationPlanetIndex()
    {
        if (this.isPlayer)
        {
            var lastNotOwnedPlanet = planetList.FirstOrDefault(x => GameObject.Find(x).GetComponent<Planet>().control == Planet.controlEnum.player1);
            if (lastNotOwnedPlanet == null)
                planetTargetIndex = planetList.Length - 1;
            else
                planetTargetIndex = Mathf.Clamp(planetList.ToList().IndexOf(lastNotOwnedPlanet) - 1, 0, planetList.Length - 1);
        }
        else if (!this.isPlayer)
        {
            var lastNotOwnedPlanet = planetList.LastOrDefault(x => GameObject.Find(x).GetComponent<Planet>().control == Planet.controlEnum.player2);
            if (lastNotOwnedPlanet == null)
                planetTargetIndex = planetList.Length - 1;
            else
                planetTargetIndex = Mathf.Clamp(planetList.ToList().IndexOf(lastNotOwnedPlanet) + 1, 0, planetList.Length - 1);
        }
    }
    public void SetDestinationToEnemyBase()
    {
        UpdateDestinationPlanetIndex();
        if (this.isPlayer)
        {
            if (Mathf.Abs(this.transform.position.x - GameObject.Find(planetList[planetTargetIndex]).transform.position.x) < setDestinationRange && Mathf.Abs(this.transform.position.z - GameObject.Find(planetList[planetTargetIndex]).transform.position.z) < setDestinationRange)//checks to see if this ship is close enough to its target planet
            {
                GameObject.Find(planetList[planetTargetIndex]).GetComponent<Planet>().SwitchControl(Planet.controlEnum.player1);//gains control of the previous planet
                if (planetTargetIndex > 0)
                {
                    navAgent.SetDestination(GameObject.Find(planetList[planetTargetIndex - 1]).transform.position);//sets destination to next planet
                    --planetTargetIndex;
                }
            }
            else
                navAgent.SetDestination(GameObject.Find(planetList[planetTargetIndex]).transform.position);//sets destination to current planet
        }
        if (!this.isPlayer)
        {
            if (Mathf.Abs(this.transform.position.x - GameObject.Find(planetList[planetTargetIndex]).transform.position.x) < setDestinationRange && Mathf.Abs(this.transform.position.z - GameObject.Find(planetList[planetTargetIndex]).transform.position.z) < setDestinationRange)//checks to see if this ship is close enough to its target planet
            {
                GameObject.Find(planetList[planetTargetIndex]).GetComponent<Planet>().SwitchControl(Planet.controlEnum.player2);//gains control of the previous planet
                if (planetTargetIndex < planetList.Length-1)
                {
                    navAgent.SetDestination(GameObject.Find(planetList[planetTargetIndex + 1]).transform.position);//sets destination to next planet
                    ++planetTargetIndex;
                }
            }
            else
                navAgent.SetDestination(GameObject.Find(planetList[planetTargetIndex]).transform.position);//sets destination to current planet
        }
    }
    public void SetDestinationToTargetShip()
    {
        if (target == null)
            target = targetList.FirstOrDefault();
        if (target == null) return;
        navAgent.SetDestination(target.transform.position);
    }
    public override void Update()
    {
        base.Update();
        targetList.RemoveAll(x => x == null);
        if (!target)
        {
            if (targetList.Count == 0)
                SetDestinationToEnemyBase();
            else
                SetDestinationToTargetShip();
            return;
        }
        attack();
    }


    //Detect enemy
    public void OnTriggerEnter(Collider collider)
    {
        Ship ship;
        if (collider.gameObject.TryGetComponent(out ship) == true)
            if (owner.Fleet.EnemyShips.Contains(ship))
            {
                targetList.Add(collider.gameObject);
                if(target == null)
                {
                    target = ship.gameObject;
                    attackTimer = 0;
                    SetDestinationToTargetShip();
                }
            }
    }
    public void OnTriggerExit(Collider other)
    {
        if (targetList.Contains(other.gameObject))
            targetList.Remove(other.gameObject);
    }
    private float GetTrueAttackSpeed()
    {
        return (6 - attackSpeed.Value) / 2f;
    }
    void attack()
    {
        isFiring = true;
        attackTimer += Time.deltaTime;
        if (attackTimer >= GetTrueAttackSpeed())
        {
            shoot();
            GetComponent<AudioSource>().Play(); // shooting sound effect
            target.GetComponent<Ship>().takeDamage(trueAttack);
            attackTimer = 0;
            isFiring = false;
        }
    }


    void shoot()
    {
        string color;
        if (owner is ControlledPlayer) color = "blue";
        else color = "red";

        GameObject laser = (GameObject)Instantiate(
            Resources.Load($"Lasers/{color}Laser"), 
            transform.position, 
            Quaternion.Euler(90, transform.rotation.eulerAngles.y, 0)
        );

        laser.GetComponent<LaserController>().range = GetComponent<SphereCollider>().radius;
        laser.GetComponent<LaserController>().owner = owner;
    }
}
