using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
