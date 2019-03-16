using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection2 : MonoBehaviour
{
    private GameObject[] characterContainer;
    private int characterIndex;

    void Start()
    {
        characterIndex = PlayerPrefs.GetInt("CharacterSelect2");
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

    public void ToggleLeft2()
    {
        characterContainer[characterIndex].SetActive(false);

        characterIndex--;
        if (characterIndex < 0)
            characterIndex = characterContainer.Length - 1;

        characterContainer[characterIndex].SetActive(true);

        PlayerPrefs.SetInt("CharacterSelect2", characterIndex);
    }

    public void ToggleRight2()
    {
        characterContainer[characterIndex].SetActive(false);

        characterIndex++;
        if (characterIndex == characterContainer.Length)
            characterIndex = 0;

        characterContainer[characterIndex].SetActive(true);

        PlayerPrefs.SetInt("CharacterSelect2", characterIndex);
    }
}
