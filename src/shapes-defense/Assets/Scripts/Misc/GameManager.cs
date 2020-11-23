using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public void LoadLevel1()
    {
        SceneManager.LoadSceneAsync("Level1");
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadSceneAsync("StartMenu");
    }
}
