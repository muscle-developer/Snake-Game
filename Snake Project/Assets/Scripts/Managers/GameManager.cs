using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform player;
    public UIViewMain mainCanvs;

    [Header("게임 관련")]
    public bool isLive;
    
    // 게임 시간 (초)
    public float gameTime = 60;


    public void Awake()
    {
        GameManager.Instance = this;
        Init();
        isLive = true;
    }

    private void Init()
    {
        // rankScore = PlayerPrefs.GetInt("rankScore");
        // text_RankScore.text = rankScore.ToString();
    }
    
    private void Update()
    {
        if (!isLive)
            return;

        gameTime -= Time.deltaTime;
        if (gameTime <= 0)
        {
            GameOver();
        }
    }

    // 게임 오버 처리
    public void GameOver()
    {
        isLive = false;

        mainCanvs.GameoverUI();

        Debug.Log("Game Over! Player's snake has been destroyed.");
        // 여기에서 게임 오버 UI 표시나 리스타트 기능 등을 추가할 수 있습니다.
        // 예: UIManager.Instance.ShowGameOverScreen();
    }
}
