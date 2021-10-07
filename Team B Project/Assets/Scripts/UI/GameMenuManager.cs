using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameMenuManager : MonoBehaviour
{
    public Button[] buttons;
    public GameObject buyMenu;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnRightClick()
    {
        var mousePos = Mouse.current.position;
        GetComponent<RectTransform>().position = mousePos.ReadValue();

        buyMenu.SetActive(false);
        animator.SetTrigger("Appear");
    }
}
