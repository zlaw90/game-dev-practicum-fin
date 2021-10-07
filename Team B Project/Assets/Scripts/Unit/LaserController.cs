using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float range;
    public IPlayer owner;

    private bool struck = false;
    private Vector3 origin;


    void Start()
    {
        origin = transform.position;
    }


    void Update()
    {
        if (struck)
            return;

        Vector3 displacement = transform.position - origin;
        if (displacement.magnitude > range)
        {
            StartCoroutine(LaserStrike());
            struck = true;
        }

        transform.Translate(transform.forward * -60 * Time.deltaTime);
    }


    void OnTriggerEnter(Collider other)
    {
        if ((other.GetComponent<Ship>()?.owner is ControlledPlayer && owner is ControlledPlayer) ||
            (other.GetComponent<Ship>()?.owner is AIPlayer && owner is AIPlayer))
            return;

        StartCoroutine(LaserStrike());
        struck = true;
    }


    IEnumerator LaserStrike()
    {
        GetComponent<Animator>().SetBool("IsOutOfRange", true);
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);
    }
}
