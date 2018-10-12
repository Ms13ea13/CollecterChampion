using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerStage1 : MonoBehaviour
{
    [SerializeField]
    private PlayerBase[] players;

    private TimeManager timeManager;

    [SerializeField]
    private EndPanel m_EndPanel;
    
    public Image _targetObj;

    [SerializeField]
    private GameObject gameEndPanel;
    
    private static GameManagerStage1 _instance;
    public static GameManagerStage1 GetInstance()
    {
        return _instance;
    }

    [SerializeField]
    private string _setTargetPicture;
    public static string _targetPicture;

    void Start ()
    {
        _instance = this;
        _targetPicture = "Item1";
        timeManager = TimeManager.GetInstance();
        timeManager.SetTimeText(TimeManager.GetInstance().GetTimer().ToString());
    }

    void Update()
    {
        _setTargetPicture = _targetPicture;
        timeManager.SetTimer(Time.deltaTime);
        int temp = (int)timeManager.GetTimer();
        timeManager.SetTimeText(temp.ToString());
    }
    
    public static void UpdateTargetPicture(string _wantTag)
    {
        _targetPicture = _wantTag;
    }

    public void UpdatePicture(Sprite pic)
    {
        _targetObj.sprite = pic;
    }
    
    public void SetGameEnd(bool checkEnd)
    {
        Time.timeScale = 0;
        gameEndPanel.SetActive(checkEnd);
        m_EndPanel.SettingEndPanel(players);
    }
}
