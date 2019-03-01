using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    private GameObject[] characterContainer;
    private int characterIndex;

    void Start()
    {
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        characterIndex = PlayerPrefs.GetInt("CharacterSelect");
        characterContainer = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            characterContainer[i] = transform.GetChild(i).gameObject;

        foreach (GameObject go in characterContainer)
            go.SetActive(false);

        if (characterContainer[characterIndex])
        {
            characterContainer[characterIndex].SetActive(true);
        }
    }

    public void ToggleLeft()
    {
        characterContainer[characterIndex].SetActive(false);

        characterIndex--;
        if (characterIndex < 0)
            characterIndex = characterContainer.Length - 1;

        characterContainer[characterIndex].SetActive(true);
    }

    public void ToggleRight()
    {
        characterContainer[characterIndex].SetActive(false);

        characterIndex++;
        if (characterIndex == characterContainer.Length)
            characterIndex = 0;

        characterContainer[characterIndex].SetActive(true);
    }

    public void SelectButton()
    {
        PlayerPrefs.SetInt("CharacterSelect", characterIndex);
        SceneManager.LoadScene("Stage1");
    }
}
