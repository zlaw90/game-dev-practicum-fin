using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;
using System.IO;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Ship : MonoBehaviour
{
    public enum shipType { BasicStarfighter, SpartanStarfighter, Freighter, Bomber, FuelFreighter };

    // unsigned to prevent negative prices
    [Serializable]
    public struct ShipPrice
    {
        public uint fuel;
        public uint metal;
    }

    // to restrict the property values between _minVal and _maxVal
    [Serializable]
    public class ShipPropertyValue
    {
        private int _maxVal = 5, _minVal = 1;
        [SerializeField] private int _value;

        public int Value
        {
            get => _value;
            set => _value = correctValue(value);
        }

        private int correctValue(int x)
        {
            if (x < _minVal) return _minVal;
            if (x > _maxVal) return _maxVal;
            else return x;
        }
    }

    [Header("General Debug")]
    public GameObject target;       // Initally hold enemy's base(Attack)/resource point(Transport).
    public Slider healthSlider;     // Health UI
    private int health;  // Current health 
    public GameObject ExplosionPrefab;
    [Header("Ship Properties")]
    public shipType kind;
    public ShipPrice price; 
    public ShipPropertyValue maxSpeed;
    public ShipPropertyValue armorStrength;     // Max health 

    private Vector3 previousVelocity;
    private Vector3 previousAcceleration;
    private Vector3 currentVelocity;
    private Vector3 currentAcceleration;

    public IPlayer owner { get;  private set; } = null;
    protected NavMeshAgent navAgent;
    protected bool isPlayer;


    public virtual void Update() { moveAnimHandler(); }
    public virtual void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        currentVelocity = Vector3.zero;
        currentAcceleration = Vector3.zero;
    }
    public virtual void Start()
    {
        isPlayer = owner is ControlledPlayer;
        SetMaxSpeed(); SetMaxHealth(); SetHealthBarColor();
    }

    private int GetMaxHealth()
    {
        return (int)Math.Pow(armorStrength.Value, 2);
    }
    public void SetOwner(IPlayer owner) { this.owner = owner; }
    public void DestroyShip()
    {
        if(ExplosionPrefab != null)
        {
            Instantiate(
                ExplosionPrefab,
                transform.position,
                Quaternion.Euler(90, transform.rotation.y, 0)
            );
        }
        Destroy(gameObject);
    }
    public bool takeDamage(int attack)
    {
        int currentHealth = health - attack;
        if (currentHealth <= 0)
        {
            health = 0;
            DestroyShip();
            return false;
        }
        health = currentHealth;
        healthSlider.value = (float)health / GetMaxHealth();
        return true;
    }


    private void SetMaxSpeed() { navAgent.speed = maxSpeed.Value * 2; }
    private void SetMaxHealth() { health = GetMaxHealth(); }
    private void SetHealthBarColor()
    {
        Image[] images = GetComponentsInChildren<Image>();
        foreach (var i in images)
        {
            if (i.name == "Fill")
            {
                if (owner is ControlledPlayer) i.color = Color.green;
                else i.color = Color.red;
                return;
            }
        }
    }
    private void moveAnimHandler()
    {
        // update values and get acceleration
        currentVelocity = navAgent.velocity;
        currentAcceleration = (currentVelocity - previousVelocity) / Time.deltaTime;

        // booster animation if positive acceleration, still animation otherwise
        if (currentAcceleration.magnitude > previousAcceleration.magnitude) GetComponentInChildren<Animator>()?.SetBool("hasPosAcceleration", true);
        else GetComponentInChildren<Animator>()?.SetBool("hasPosAcceleration", false);

        // for the next call
        previousVelocity = currentVelocity;
        previousAcceleration = currentAcceleration;
    }
}
