using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject optionPanel;
    public GameObject mainMenuPanel;

    public GameObject exitPanel;
    public GameObject yesButton;
    public GameObject noButton;
    public GameObject text;

   
    public void OptionButton()
    {
        optionPanel.SetActive(true);
    }

    public void closeOptionPanel()
    {
        optionPanel.SetActive(false);
    }


    //---------------------------------------------------------------------------------------------------------------------

    public void GoToHowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    /*public void BackToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void BackToSelectStage()
    {
        SceneManager.LoadScene(3);
    }

    public void GameEndReplayStage1()
    {
        SceneManager.LoadScene(4);
    }

    public void GameEndReplayStage2()
    {
        SceneManager.LoadScene(5);
    }*/

    public void ReplayStage1()
    {
        LeanTween.cancelAll();
        if (Time.timeScale == 0) 
            Time.timeScale = 1;
        StartCoroutine(LoadingLevel_ReplayStage1());
    }

    public void ReplayStage2()
    {
        LeanTween.cancelAll();
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        StartCoroutine(LoadingLevel_ReplayStage2());
    }

    public void GoToStory()
    {
        LeanTween.cancelAll();
        StopAllCoroutines();
        if (Time.timeScale == 0) 
            Time.timeScale = 1;
        StartCoroutine(LoadingLevel_GoToStory());
    }

    public void SelectStage()
    {
        LeanTween.cancelAll();
        StopAllCoroutines();
        if (Time.timeScale == 0) 
            Time.timeScale = 1;
        StartCoroutine(LoadingLevel_SelectStage());
    }

    IEnumerator LoadingLevel_GoToStory()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("04Story");
    }

    IEnumerator LoadingLevel_SelectStage()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("SelectStage");
    }

    IEnumerator LoadingLevel_ReplayStage1()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Stage1");
    }

    IEnumerator LoadingLevel_ReplayStage2()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Stage2");
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
        exitPanel.SetActive(true);
        yesButton.SetActive(true);
        noButton.SetActive(true);
        text.SetActive(true);
    }

    public void NoButton()
    {
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
