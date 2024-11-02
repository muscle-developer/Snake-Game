using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform player;

    [Header("게임 관련")]
    public bool isLive;
    
    // 게임 시간 (초)
    private float gameTime = 30;
    // 남은 게임 시간 텍스트
    [SerializeField]
    private TMP_Text timeText;


    public void Awake()
    {
        GameManager.Instance = this;
    }

    void Start()
    {   
        Init();
    }

    private void Init()
    {
        // rankScore = PlayerPrefs.GetInt("rankScore");
        // text_RankScore.text = rankScore.ToString();
    }
    

    // 게임 오버 처리
    public void GameOver()
    {
        Debug.Log("Game Over! Player's snake has been destroyed.");
        // 여기에서 게임 오버 UI 표시나 리스타트 기능 등을 추가할 수 있습니다.
        // 예: UIManager.Instance.ShowGameOverScreen();
    }
}
