﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;
    public GameObject deletePanel;
    public CharacterSelector[] characterDelete;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
        Time.timeScale = 1f;
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void DeleteSave()
    {
        deletePanel.SetActive(true);
    }

    public void ConfirmDelete()
    {
        deletePanel.SetActive(false);

        foreach(CharacterSelector theChar in characterDelete)
        {
            PlayerPrefs.SetInt(theChar.playerToSpawn.name, 0);
        }
    }

    public void CancelDelete()
    {
        deletePanel.SetActive(false);
    }
}
