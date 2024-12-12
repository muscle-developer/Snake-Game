public interface IGameState
{
    void EnterState(GameManager gameManager);  // 상태에 진입할 때 실행
    void UpdateState(GameManager gameManager); // 매 프레임 실행
    void ExitState(GameManager gameManager);   // 상태에서 나갈 때 실행
}

