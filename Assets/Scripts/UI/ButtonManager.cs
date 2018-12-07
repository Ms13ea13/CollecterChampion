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

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void Replay()
    {
        if (Time.timeScale == 0) 
            Time.timeScale = 1;
        StartCoroutine(LoadingLevel_Replay());
       
    }

    public void GoToStory()
    {
        StopAllCoroutines();
        if (Time.timeScale == 0) 
            Time.timeScale = 1;
        StartCoroutine(LoadingLevel_GoToStory());
        
    }

    public void GoToState()
    {
        StopAllCoroutines();
        if (Time.timeScale == 0) 
            Time.timeScale = 1;
        StartCoroutine(LoadingLevel_GoToState());
       
    }

    IEnumerator LoadingLevel_GoToStory()
    {
        yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("04Story");
    }

    IEnumerator LoadingLevel_GoToState()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("State");
    }

    IEnumerator LoadingLevel_Replay()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("NewTUTORIAL");
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
