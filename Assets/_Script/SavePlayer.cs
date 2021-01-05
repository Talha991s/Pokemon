using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayer : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {
        if(PlayerPrefs.GetInt("Saved") == 1 && PlayerPrefs.GetInt("TimeToLoad")==1)
        {
            float pX = player.transform.position.x;
            float py = player.transform.position.y;

            pX = PlayerPrefs.GetFloat("p_x");
            py = PlayerPrefs.GetFloat("p_y");
            player.transform.position = new Vector2(pX, py);
            PlayerPrefs.SetInt("TimeToLoad",0);
            PlayerPrefs.Save();
        }

    }
    public void PlayerPosSave()
    {
        PlayerPrefs.SetFloat("p_x", player.transform.position.x);
        PlayerPrefs.SetFloat("p_y", player.transform.position.y);
        PlayerPrefs.SetInt("Saved", 1);
        PlayerPrefs.Save();
    }

    public void PlayerPauseLoad()
    {
        PlayerPrefs.SetInt("TimeToLoad", 1);
        PlayerPrefs.Save();
    }
}
