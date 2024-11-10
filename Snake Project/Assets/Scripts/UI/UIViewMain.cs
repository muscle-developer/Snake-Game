// UIViewMain.cs
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIViewMain : MonoBehaviour
{
    // 남은 게임 시간 텍스트
    [SerializeField]
    private TMP_Text timeText;
    
    // 이전 최고 점수
    private int previousScore = 0; 
    // 현재 점수
    private int currentScore = 0;
    // 목표 점수
    private int targetScore = 10;
    // 목표 점수
    [SerializeField]
    private TMP_Text targetScoreText;
    // 스코어 점수
    [SerializeField]
    private List<TMP_Text> scoreTextList = new List<TMP_Text>();
    // 적 스코어를 저장할 리스트
    private List<int> enemyScores = new List<int>();

    // 게임 종료 UI
    [SerializeField]
    private GameObject gameoverPopup; 

    // 게임 성공 UI
    [SerializeField]
    private GameObject successPopup;

    void Start()
    {
        GameManager.Instance.mainCanvs = this;

        if(GameManager.Instance.isLive)
        {
            gameoverPopup.gameObject.SetActive(false);
            successPopup.gameObject.SetActive(false);
        }

        InitScoreUI();

        // 이전 최고 점수 로드
        previousScore = PlayerPrefs.GetInt("HighScore", 0);
        scoreTextList[0].text = "Player Score: " + previousScore;

        // 초기 목표 점수 UI 설정
        targetScoreText.text = "Target Score: " + targetScore;
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        float gameTime = GameManager.Instance.gameTime;
        int min = Mathf.FloorToInt(gameTime / 60);
        int sec = Mathf.FloorToInt(gameTime % 60);
        timeText.text = string.Format("{0:D2}:{1:D2}", min, sec);
    }

    private void InitScoreUI()
    {
        scoreTextList[0].text = "Player Score :" + previousScore;
        for(int i = 1; i < scoreTextList.Count; i++)
        {
            scoreTextList[i].text = "Enemy Score : 0";
        }
    }

    // 플레이어 점수 업데이트 메서드   
    public void UpdatePlayerScore()
    {
        currentScore = SnakeManager.Instance.BodyParts.Count - 1;
        scoreTextList[0].text = "Player Score: " + currentScore.ToString();

        // 현재 점수가 목표 점수에 도달하면 성공 처리
        if (currentScore >= targetScore)
        {
            SuccessUI(); // 성공 UI 표시
        }

        // 현재 점수가 이전 최고 점수보다 높으면 최고 점수 갱신
        if (currentScore > previousScore)
        {
            previousScore = currentScore;
            PlayerPrefs.SetInt("HighScore", previousScore); // PlayerPrefs에 저장
            PlayerPrefs.Save(); // 저장을 즉시 반영
        }
    }

    // 적 점수 업데이트 메서드
    public void UpdateEnemyScores()
    {
        enemyScores.Clear(); // 이전 점수 초기화
        foreach (var enemy in EnemySnakeManager.Instance.enemySnakes) // EnemySnakeManager에 있는 모든 적 스네이크를 조회
        {
            int enemyScore = enemy.bodyParts.Count; // 적 스코어는 몸체 개수로 계산
            enemyScores.Add(enemyScore);
        }

        for (int i = 0; i < enemyScores.Count; i++)
        {
            scoreTextList[i + 1].text = "Enemy Score : " + enemyScores[i].ToString(); // 적 점수를 UI에 표시
        }
    }

    private void SuccessUI()
    {
        // 성공 화면 활성화
        successPopup.SetActive(true);

        // 목표 점수 증가 (다음 게임을 위해 설정)
        targetScore += 10;
        targetScoreText.text = "Target Score: " + targetScore;

        // 다음 게임을 위해 현재 점수 초기화
        currentScore = 0;
    }

    public void GameoverUI()
    {
        if(!GameManager.Instance.isLive)
        {
            gameoverPopup.gameObject.SetActive(true);
        }
    }

    // 새로운 게임 시작 메서드 (성공 화면을 닫고 게임 초기화)
    public void ResetGame()
    {
        successPopup.SetActive(false); // 성공 화면 닫기
        GameManager.Instance.ResetGame(); // 게임 초기화 메서드 호출 (필요 시 GameManager에 구현)
        InitScoreUI(); // UI 초기화
    }
}
