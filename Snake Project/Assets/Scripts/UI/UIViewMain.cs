using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

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

    [Header("게임 종료")]
    // 게임 종료 UI
    [SerializeField]
    private GameObject gameoverPopup;
    [SerializeField]
    private TMP_Text finalScoreText;
    [SerializeField]
    private TMP_Text nickNameText;

    [Header("게임 성공")]
    // 게임 성공 UI
    [SerializeField]
    private GameObject successPopup;

    [SerializeField] 
    private Image fadeImage;
    private float fadeDuration = 1f;

    // 비동기 페이드 전환을 위한 코루틴
    private Coroutine coroutine;

    void Start()
    {
        GameManager.Instance.mainCanvs = this;

        if (GameManager.Instance.isNewGame || GameManager.Instance.isNextGame || GameManager.Instance.isCurrentGame)
        {
            GameManager.Instance.isLive = true;
            GameManager.Instance.gameTime = 60; // 기본 게임 시간 설정
            InitScoreUI();
        }

        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeOut());

        if(GameManager.Instance.isLive)
        {
            gameoverPopup.gameObject.SetActive(false);
            successPopup.gameObject.SetActive(false);
        }

        if(GameManager.Instance.isNextGame)
        {
            // 이전 최고 점수 로드
            previousScore = PlayerPrefs.GetInt("HighScore", 0);
            scoreTextList[0].text = "내 점수: " + previousScore;

            // 목표 점수 증가 (다음 게임을 위해 설정)
            targetScore += 10;
            // 초기 목표 점수 UI 설정
            targetScoreText.text = "목표 점수: " + targetScore;
        }
        else if(GameManager.Instance.isCurrentGame)
        {
            previousScore = PlayerPrefs.GetInt("HighScore", 0);
            scoreTextList[0].text = "내 점수: " + previousScore;

            targetScoreText.text = "목표 점수: " + targetScore;
        }
        else if(GameManager.Instance.isNewGame)
        {
            InitScoreUI();
        }
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
        scoreTextList[0].text = "내 점수 :" + currentScore;
        for(int i = 1; i < scoreTextList.Count; i++)
        {
            scoreTextList[i].text = "상대 점수 : 3";
        }
        targetScoreText.text = "목표 점수 :" + targetScore.ToString();
    }

    // 플레이어 점수 업데이트 메서드   
    public void UpdatePlayerScore()
    {
        currentScore = SnakeManager.Instance.BodyParts.Count - 1;
        scoreTextList[0].text = "내 점수: " + currentScore.ToString();

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
            scoreTextList[i + 1].text = "상대 점수 : " + enemyScores[i].ToString(); // 적 점수를 UI에 표시
        }
    }

    private void SuccessUI()
    {
        // 성공 화면 활성화
        successPopup.SetActive(true);

        targetScoreText.text = "목표 점수: " + targetScore;

        // 다음 게임을 위해 현재 점수 초기화
        currentScore = 0;
    }

    public void GameoverUI()
    {
        if(!GameManager.Instance.isLive)
        {
            gameoverPopup.gameObject.SetActive(true);
            finalScoreText.text = currentScore.ToString();
            string nickname = PlayerPrefs.GetString("Nickname", "Player");
            nickNameText.text = nickname;
        }
    }

    // 새로운 게임 시작 메서드 (성공 화면을 닫고 게임 초기화)
    public void ResetGame()
    {
        successPopup.SetActive(false); // 성공 화면 닫기

        // 다음 목표 점수를 표시합니다.
        targetScoreText.text = "목표 점수: " + targetScore;
    }

    // 버튼 클릭 관련
    // 나가기 버튼 눌렀을 때
    public void OnClickGameExit(string sceneName)
    {
        if(coroutine == null)
        {
            coroutine = StartCoroutine(SceneTrans(sceneName));
            GameManager.Instance.SetState(new NewGameState());
        }
    }

    // 다시하기 버튼
    public void OnClickRetry(string sceneName)
    {

        if(coroutine == null)
        {
            coroutine = StartCoroutine(SceneTrans(sceneName));
            GameManager.Instance.SetState(new CurrentGameState());
        }
    }

    // 성공 시 다음 단계로 가는 버튼
    public void OnClickNextLevel(string sceneName)
    {

        if (coroutine == null)
        {
            coroutine = StartCoroutine(SceneTrans(sceneName));
            GameManager.Instance.SetState(new NextGameState());
        }
    }

    private IEnumerator FadeOut()
    {
        Color color = fadeImage.color;
        float startAlpha = color.a;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, 0f, normalizedTime);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }

    private IEnumerator FadeIn()
    {
        Color color = fadeImage.color;
        float startAlpha = color.a;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, 1f, normalizedTime);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }

    // 비동기 씬 전환으로 화면 페이드 인아웃 연출 주기.
    private IEnumerator SceneTrans(string sceneName)
    {
        // 먼저 페이드 인
        fadeImage.gameObject.SetActive(true);
        yield return StartCoroutine(FadeIn());

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        // 페이드 효과가 끝난 후 씬을 로드하고 활성화
        yield return new WaitForSeconds(1f);
        async.allowSceneActivation = true;
        
        // 씬 전환 후 페이드 아웃
        yield return StartCoroutine(FadeOut());
        fadeImage.gameObject.SetActive(false); // 페이드 효과 종료 후 비활성화

        coroutine = null;
    }
}
