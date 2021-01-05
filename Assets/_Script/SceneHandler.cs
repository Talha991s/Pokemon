using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    SavePlayer saveddata;
    public LevelLoader loader;
    public GameObject pausepanel;
    public Animator animator;

    private void Start()
    {
        saveddata = FindObjectOfType<SavePlayer>();
    }


    public void ClickPlayButton()
    {
        PlayerPrefs.DeleteKey("p_x");
        PlayerPrefs.DeleteKey("p_y");
        PlayerPrefs.DeleteKey("TimeToLoad");
        PlayerPrefs.DeleteKey("Saved");
        loader.LoadNextLevel();
        SceneManager.LoadScene(1);
        
    }
    public void onClickLoadbutton()
    {
        SceneManager.LoadScene(1);
    }
    public void OnclickPaused()
    {
        animator.SetBool("isMoving", false);
        pausepanel.gameObject.SetActive(true);
    }
    public void onClickResume()
    {
        animator.SetBool("isMoving", true);
        pausepanel.gameObject.SetActive(false);
    }
    public void onClickSaveExit()
    {
        saveddata.PlayerPosSave();
        pausepanel.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void ClickQuitButton()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

}
