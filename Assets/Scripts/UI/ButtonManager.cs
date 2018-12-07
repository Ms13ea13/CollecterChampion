﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject optionPanel;
    public GameObject mainMenuPanel;
    //public GameObject muteButton;

    public GameObject exitPanel;
    public GameObject yesButton;
    public GameObject noButton;
    public GameObject text;

    public void OptionButton()
    {
        optionPanel.SetActive(true);
        //muteButton.SetActive(false);
    }

    public void closeOptionPanel()
    {
        optionPanel.SetActive(false);
    }


    //---------------------------------------------------------------------------------------------------------------------

    public void backToMainMenu()
    {
        SceneManager.LoadScene("03MainMenu");
    }

    public void Replay()
    {
        SceneManager.LoadScene("NewTUTORIAL");
    }

    public void GoToStory()
    {
        SceneManager.LoadScene("04Story");
    }

    public void MainMenuButton()
    {
        mainMenuPanel.SetActive(true);
        yesButton.SetActive(true);
        noButton.SetActive(true);
        text.SetActive(true);
    }

    public void ExitButton()
    {
        //muteButton.SetActive(false);

        exitPanel.SetActive(true);
        yesButton.SetActive(true);
        noButton.SetActive(true);
        text.SetActive(true);
    }

    public void NoButton()
    {
        //muteButton.SetActive(false);
        mainMenuPanel.SetActive(false);

        exitPanel.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);
        text.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
