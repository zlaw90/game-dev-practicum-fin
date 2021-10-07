using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameEndDisplayController : MonoBehaviour
{
    [SerializeField] GameObject GameEndPanel;
    [SerializeField] Text GameEndText;
    // Start is called before the first frame update
    void Start()
    {
        ControlledPlayer.Instance.GameDidEnd += Instance_GameDidEnd;
    }

    private void Instance_GameDidEnd(bool win)
    {
        GameEndText.text = win ? "You Win!" : "You Lose!";
        GameEndPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
