// UIViewMain.cs
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIViewMain : MonoBehaviour
{
    // 남은 게임 시간 텍스트
    [SerializeField]
    private TMP_Text timeText;

    // 스코어 점수
    [SerializeField]
    private List<TMP_Text> scoreTextList = new List<TMP_Text>();
    // 적 스코어를 저장할 리스트
    private List<int> enemyScores = new List<int>();

    // 게임 종료 UI
    [SerializeField]
    private GameObject gameoverPopup; 

    void Start()
    {
        GameManager.Instance.mainCanvs = this;

        if(GameManager.Instance.isLive)
            gameoverPopup.gameObject.SetActive(false);

        InitScoreUI();
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
        scoreTextList[0].text = "Player Score : 0";
        for(int i = 1; i < scoreTextList.Count; i++)
        {
            scoreTextList[i].text = "Enemy Score : 0";
        }
    }

    // 플레이어 점수 업데이트 메서드   
    public void UpdatePlayerScore()
    {
        int playerScore = SnakeManager.Instance.BodyParts.Count - 1;
        scoreTextList[0].text = "Player Score :" + playerScore.ToString();
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

    public void GameoverUI()
    {
        if(!GameManager.Instance.isLive)
        {
            gameoverPopup.gameObject.SetActive(true);
        }
    }
}
