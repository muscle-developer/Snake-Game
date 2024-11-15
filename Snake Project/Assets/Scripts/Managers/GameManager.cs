using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform player;
    public UIViewMain mainCanvs;

    [Header("게임 관련")]
    public bool isLive;
    public bool isNewGame = false; // 새로 시작한 경우
    public bool isNextGame = false; // 성공 후 다음으로 넘어가는 게임
    public bool isCurrentGame = false; // 목표점수를 달성 못해서 재도전 게임
    
    // 게임 시간 (초)
    public float gameTime = 60;
    private float initialGameTime;  // 초기화된 게임 시간 저장용


    public void Awake()
    {
        GameManager.Instance = this;
        Init();
        isLive = true;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Game")
        {
            // isNextGame이 true인 경우 상태를 변경하지 않음
            if (isNextGame)
            {
                return;
            }
            else if (isCurrentGame)
            {
                isNewGame = false;
                isNextGame = false;
            }
            else
            {
                isNewGame = true;
                isCurrentGame = false;
                isNextGame = false;
            }
        }
    }

    private void Init()
    {
        initialGameTime = gameTime; // 게임 시작 시간을 초기화 시간으로 설정
        isNewGame = false;
        isNextGame = false;
        isCurrentGame = false;
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
        isNewGame = false; // 게임 리셋 시 새로운 게임 상태를 false로 설정
        mainCanvs.ResetGame(); // UIViewMain의 UI 초기화 메서드 호출
    }
}
