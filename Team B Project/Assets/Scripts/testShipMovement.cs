using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testShipMovement : MonoBehaviour
{
    GameObject[] targetBag;//bag of potiential targets
    GameObject currentTarget;
    GameObject nullTarget;//empty gameObject to assert target hasn't been destroyed

    string myTargetTag;

    float pursuitRange = 500f;
    float firingRange = 1f;


    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("testShipEnemy"))//assert what this' tag is to find enemy ships
            myTargetTag = "BasePlayer1";
        else
            myTargetTag = "BasePlayer2";

        targetBag = GameObject.FindGameObjectsWithTag(myTargetTag);//gather objects of requisite tag

        UpdateTarget();
        
    }

    public Vector3 targeted() {//method to find targets position
        return gameObject.transform.position;
    }

    public void UpdateTarget()
    {
        int index = 0;//loop index

        while (currentTarget == nullTarget && index != targetBag.Length)//find a new target if need be/ make sure array bounds are honored
        {

            if (targetBag[index] != gameObject && Mathf.Abs(targetBag[index].transform.position.x - transform.position.x) < pursuitRange && Mathf.Abs(targetBag[index].transform.position.z - transform.position.z) < pursuitRange)//if possible target is relatively close assign as new target
                currentTarget = targetBag[index];
            else
                ++index;//else keep searching

        }
    }
    // Update is called once per frame
    void Update()
    {
        targetBag = GameObject.FindGameObjectsWithTag(myTargetTag);//gather objects of requisite tag
        UpdateTarget();
        if (currentTarget == null) return;
        if (Mathf.Abs(currentTarget.transform.position.x - transform.position.x) > firingRange && Mathf.Abs(currentTarget.transform.position.z - transform.position.z) > firingRange)//if target is out of firing range move twords it
        {
            GetComponent<NavMeshAgent>().SetDestination(new Vector3(currentTarget.transform.position.x, currentTarget.transform.position.y, currentTarget.transform.position.z));//move tranform
            transform.rotation = Quaternion.LookRotation(gameObject.GetComponent<NavMeshAgent>().velocity.normalized);//face target
        }
        else
            GetComponent<NavMeshAgent>().SetDestination(new Vector3(transform.position.x, .25f, transform.position.z));//if close enough to fire at hold position

    }
}
