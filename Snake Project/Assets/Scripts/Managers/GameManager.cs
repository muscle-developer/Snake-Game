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
    
    public float gameTime = 60;  // 게임 시간 (초)
    private float initialGameTime;  // 초기화된 게임 시간 저장용

    public IGameState currentState; // 상태 체크

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

    // 현재 상태를 새로운 상태로 변경하는 메서드
    public void SetState(IGameState newState)
    {
        // 현재 상태가 존재하면, 상태를 종료
        if (currentState != null)
        {
            Debug.Log($"Exiting {currentState.GetType().Name}"); // 현재 상태 이름을 로그로 출력
            currentState.ExitState(this); // 현재 상태의 ExitState 메서드 호출 (종료 작업 수행)
        }

        // 새로운 상태로 전환
        currentState = newState; // currentState를 새로운 상태로 갱신

        // 새로운 상태의 EnterState 메서드를 호출하여 초기화 작업 수행
        Debug.Log($"Entering {currentState.GetType().Name}"); // 새 상태 이름을 로그로 출력
        currentState.EnterState(this); // 새로운 상태의 초기화 작업 수행
    }

    // 게임 오버 처리
    public void GameOver()
    {
        isLive = false;

        if(mainCanvs != null)
            mainCanvs.GameoverUI();
    }
}
