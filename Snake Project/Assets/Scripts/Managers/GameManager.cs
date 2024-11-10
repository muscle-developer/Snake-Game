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
    private float initialGameTime;  // 초기화된 게임 시간 저장용


    public void Awake()
    {
        GameManager.Instance = this;
        Init();
        isLive = true;
    }

    private void Init()
    {
        initialGameTime = gameTime; // 게임 시작 시간을 초기화 시간으로 설정
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
    }

    // 새로운 게임 시작을 위한 리셋 메서드
    public void ResetGame()
    {
        isLive = true;
        gameTime = initialGameTime; // 게임 시간을 초기화
        mainCanvs.ResetGame(); // UIViewMain의 UI 초기화 메서드 호출
    }
}
