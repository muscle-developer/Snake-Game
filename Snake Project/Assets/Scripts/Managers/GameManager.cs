using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform player;
    public UIViewMain mainCanvs;

    [Header("게임 상태 관련")]
    public bool isLive;
    public bool isNewGame = false; // 새로 시작한 경우
    public bool isNextGame = false; // 성공 후 다음으로 넘어가는 게임
    public bool isCurrentGame = false; // 목표점수를 달성 못해서 재도전 게임
    
    // 게임 시간 (초)
    public float gameTime = 60;
    private float initialGameTime;  // 초기화된 게임 시간 저장용

    public IGameState currentState;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않음
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
        }
        Init();
    }

    private void Init()
    {
        // 게임 시간 초기화
        initialGameTime = 60; // 기본 값 설정 (필요 시 인스펙터에서 조정 가능)
        gameTime = initialGameTime;

        isLive = true;
    }

    private void Update()
    {
        if (!isLive)
            return;

        currentState?.UpdateState(this); // 현재 상태 업데이트

        gameTime -= Time.deltaTime;
        if (gameTime <= 0 && isLive)
        {
            GameOver();
        }
    }

    public void SetState(IGameState newState)
    {
        if (currentState != null)
        {
            Debug.Log($"Exiting {currentState.GetType().Name}");
            currentState.ExitState(this); // 기존 상태 종료
        }
        currentState = newState;
        Debug.Log($"Entering {currentState.GetType().Name}");
        currentState.EnterState(this); // 새로운 상태 진입
    }

    // 게임 오버 처리
    public void GameOver()
    {
        isLive = false;

        if(mainCanvs != null)
            mainCanvs.GameoverUI();
    }
}
