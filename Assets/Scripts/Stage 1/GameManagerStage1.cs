using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerStage1 : MonoBehaviour
{
    public Image _targetObj;

    private static GameManagerStage1 _instance;
    public static GameManagerStage1 GetInstance()
    {
        return _instance;
    }

    [SerializeField]
    private string _setTargetPicture;
    public static string _targetPicture;

    [SerializeField]
    private int _showScore;
    public static int _score;

    void Start ()
    {
        _instance = this;
        _targetPicture = "Item1";
    }

    void Update()
    {
        _setTargetPicture = _targetPicture;
        _showScore = _score;
    }

    public static void UpdateTargetPicture(string _wantTag)
    {
        _targetPicture = _wantTag;
        Debug.Log("now : " + _targetPicture);
    }

    public void UpdatePicture(Sprite pic)
    {
        _targetObj.sprite = pic;
    }
}
