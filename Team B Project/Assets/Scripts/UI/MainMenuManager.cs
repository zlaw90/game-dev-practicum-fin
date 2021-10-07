using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

	public AudioSource buttonPress;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        buttonPress.Play();
        SceneManager.LoadScene(1);
    }
    public void Credits()
    {
        buttonPress.Play();
        SceneManager.LoadScene(2);
    }

    public void Quit()
    {
    	buttonPress.Play();
    	Application.Quit();
    }
}
