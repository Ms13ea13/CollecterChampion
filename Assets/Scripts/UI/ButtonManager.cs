using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    /*  public AudioClip MainMenu;//
      private AudioSource BGAudioSource;//*/

    

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
        StartCoroutine(LoadingLevel_backToMainMenu());
      /*  BGAudioSource = GetComponent<AudioSource>();//
        BGAudioSource.PlayOneShot(MainMenu);//*/

    }

    public void Replay()
    {
        StartCoroutine(LoadingLevel_Replay());
       
    }

    public void GoToStory()
    {
        StartCoroutine(LoadingLevel_GoToStory());
        
    }

    public void GoToState()//
    {
        StartCoroutine(LoadingLevel_GoToState());
       
    }

    IEnumerator LoadingLevel_GoToStory()
    {
        yield return new WaitForSeconds(1f);
       
            SceneManager.LoadScene("04Story");
        
       
        
    }//

    IEnumerator LoadingLevel_GoToState()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("State");
    }//

    IEnumerator LoadingLevel_Replay()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("NewTUTORIAL");
    }//

    IEnumerator LoadingLevel_backToMainMenu()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("03MainMenu");
    }//

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
