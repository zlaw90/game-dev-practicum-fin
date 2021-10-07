using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class Planet : MonoBehaviour
{
    /* 
     * HOW PLANETS WORK:
     * Each planet holds a state of which player has control of it,
     * stored in an enum
     * 
     * A planet's resources aren't finite! If a planet's current (cur) value
     * of a given resource is LESS than its maximum (max) value, after a few
     * seconds, the planet will gain 1 more of that resource
     * 
     * Multiple ships can pull the same resource from the same planet at the
     * same time
     */

    //Planet Types
    /*
    	Neutral = even gas/metal
    	Gas = +gas -metal
    	Mountain = +metal -gas
    */

    // Planet type
    public enum planetTypeEnum { neutral, gas, mountain, random };
    public planetTypeEnum planetType;

    // Building type
    public enum buildingTypeEnum { neutral, mine, factory };
    public buildingTypeEnum buildingType;

    // Player control
    public enum controlEnum { neutral, player1, player2 };
    public controlEnum control;

    // Resources
    public PlanetResource[] resources = new PlanetResource[2];
    private Dictionary<Resource.ResourceKind, Resource> _planetResourcesAsDictionary = new Dictionary<Resource.ResourceKind, Resource>();
    public IReadOnlyDictionary<Resource.ResourceKind, Resource> PlanetResourcesAsDictionary
    {
        get => _planetResourcesAsDictionary;
    }
    bool replenishing = false;

    [Serializable]
    public class PlanetResource : Resource
    {
        public int maxAmt; // Max amount of the resource the planet can have
        public PlanetResource(ResourceKind kind, int startingAmount, int maxAmount) : base(startingAmount, kind)
        {
            this.maxAmt = maxAmount;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DisplayController();
    }
    private void Awake()
    {
        foreach (Resource planetResource in resources)
            if (!_planetResourcesAsDictionary.ContainsKey(planetResource.kind))
                _planetResourcesAsDictionary.Add(planetResource.kind, planetResource);
    }
    // Update is called once per frame
    void Update()
    {
        if (!replenishing)
            StartCoroutine(ReplenishResources());
    }

    public void SwitchControl(controlEnum c)
    {
        if (control != c) {
            control = c;
            DisplayController();
        }
    }

    public Resource removeResources(Resource resourceToWithdraw)//removes resources from planet equally
    {
        Resource removed = new Resource(0, resourceToWithdraw.kind);
        int amountToWithdraw = resourceToWithdraw.amount;
        Resource resourceToExtract = resources.FirstOrDefault(x => x.kind == resourceToWithdraw.kind);
        if (resourceToExtract != null)
        {
            while (resourceToExtract.amount > 0 && amountToWithdraw > 0)
            {
                removed.amount += 1;
                amountToWithdraw -= 1;
                resourceToExtract.amount -= 1;
            }
        }
        return removed;
    }

    IEnumerator ReplenishResources()
    {
        replenishing = true;
        foreach (PlanetResource r in resources)
            if (r.amount < r.maxAmt)
                r.amount++;

        yield return new WaitForSeconds(15);
        replenishing = false;
    }

    private void DisplayController() {
        if (this.control != Planet.controlEnum.neutral)
        {
            if (this.control == Planet.controlEnum.player1)
            {
                if (this.transform.childCount !=0)//if the planet already had a sprite to denote control -> delete it to display the new one
                {
                    for (int i = 0; i < this.transform.childCount; ++i)
                    {
                        GameObject toDestroy = this.transform.GetChild(i).gameObject;
                        Destroy(toDestroy);
                    }
                }
                var controlSprite = Resources.Load<Sprite>("playerDenotion");//load the correct sprite for this
                GameObject child = new GameObject();//create a child to add the sprite to
                SpriteRenderer renderer = child.AddComponent<SpriteRenderer>();
                renderer.sprite = controlSprite;
                child.transform.parent = this.transform;
                child.transform.position = this.transform.position + new Vector3(0f, 0f, 8f);
                child.transform.localScale = new Vector3(.5f, .5f, 0f);
                child.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            }
            else if (this.control == Planet.controlEnum.player2)
            {
                if (this.transform.childCount !=0)//if the planet already had a sprite to denote control -> delete it to display the new one
                {
                    for (int i = 0; i < this.transform.childCount; ++i)
                    {
                        GameObject toDestroy = this.transform.GetChild(i).gameObject;
                        Destroy(toDestroy);
                    }
                    //GameObject toDestroy = this.transform.GetChild(0).gameObject;
                    //Destroy(toDestroy);
                }
                var controlSprite = Resources.Load<Sprite>("enemyDenotion");//load the correect sprite for this
                GameObject child = new GameObject();//create a child to add the sprite to
                SpriteRenderer renderer = child.AddComponent<SpriteRenderer>();
                renderer.sprite = controlSprite;
                child.transform.parent = this.transform;
                child.transform.position = this.transform.position + new Vector3(0f, 0f, 8f);
                child.transform.localScale = new Vector3(.5f, .5f, 0f);
                child.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            }
        }
    }
}