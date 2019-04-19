using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerEndScene : MonoBehaviour
{
    [SerializeField]
    private LevelLoader loadEndStory;
    public int endScene = 0;
	// Use this for initialization
	void Start ()
    {
        endScene = PlayerPrefs.GetInt("StageIndex");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (endScene >= 5)
        {
            if (Time.timeScale == 0)
                Time.timeScale = 1;

            //SceneManager.LoadScene("StoryEnd");
            loadEndStory.Loadlevel(20);
            PlayerPrefs.SetInt("StageIndex", 0);
        }
        else
        {
            Debug.LogError(endScene);
        }
	}
}
