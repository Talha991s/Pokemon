using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transtion;
    public float transitionTime = 2.0f;


    public void LoadNextLevel()
    {
        // add a coroutine to delay the transition
        StartCoroutine(Loadlevel(SceneManager.GetActiveScene().buildIndex +1));
    }

    IEnumerator Loadlevel(int levelindex)
    {
        //play animation
        transtion.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelindex);
    }
}
