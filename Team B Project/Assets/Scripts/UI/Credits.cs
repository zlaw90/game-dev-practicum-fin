using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

    public GameObject creditsRun;
    public int waitDuration = 5;


	void Start () {
        StartCoroutine(RollCreds());
	}

    IEnumerator RollCreds ()
    {
        yield return new WaitForSeconds(0.5f);
        creditsRun.SetActive(true);
        yield return new WaitForSeconds(waitDuration);
        SceneManager.LoadScene(0);
    }
}