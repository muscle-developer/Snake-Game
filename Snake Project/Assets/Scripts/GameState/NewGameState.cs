using UnityEngine;

// 새로운 게임 상태 클래스
public class NewGameState : IGameState
{
    // 상태 진입 시 호출
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Entering New Game State");
        gameManager.isNewGame = true;   // 새로운 게임 상태 활성화
        gameManager.isCurrentGame = false; // 현재 게임 상태 비활성화
        gameManager.isNextGame = false; // 다음 게임 상태 비활성화
        gameManager.isLive = true; // 게임 진행 상태로 설정

        gameManager.gameTime = 60; // 새로운 게임의 기본 시간 설정
    }

    // 상태 업데이트 로직 (매 프레임 호출)
    public void UpdateState(GameManager gameManager)
    {
        // 게임 시간이 0 이하가 되면 GameOver 상태로 전환
        if (gameManager.gameTime <= 0)
        {
            gameManager.SetState(new GameOverState());  // 상태 전환
        }
    }

    // 상태 종료 시 호출
    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exiting New Game State"); // 게임 종료 디버깅
    }
}

// 다음 게임 상태 클래스
public class NextGameState : IGameState
{
    // 상태 진입 시 호출
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Entering Next Game State");
        gameManager.isNextGame = true; // 다음 게임 상태 활성화
        gameManager.isNewGame = false; // 새로운 게임 상태 비활성화
        gameManager.isCurrentGame = false; // 현재 게임 상태 비활성화
        gameManager.isLive = true; // 게임 진행 상태로 설정
    }

    // 상태 업데이트 로직 (매 프레임 호출)
    public void UpdateState(GameManager gameManager)
    {
        // 게임 시간이 0 이하가 되면 GameOver 상태로 전환
        if (gameManager.gameTime <= 0)
        {
            gameManager.SetState(new GameOverState()); // 상태 전환
        }
    }

    // 상태 종료 시 호출
    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exiting Next Game State");
    }
}

// 현재 게임 상태 클래스
public class CurrentGameState : IGameState
{
    // 상태 진입 시 호출
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Entering Current Game State");
        gameManager.isCurrentGame = true; // 현재 게임 상태 활성화
        gameManager.isNewGame = false; // 새로운 게임 상태 비활성화
        gameManager.isNextGame = false; // 다음 게임 상태 비활성화
        gameManager.isLive = true; // 게임 진행 상태로 설정
    }

    // 상태 업데이트 로직 (매 프레임 호출)
    public void UpdateState(GameManager gameManager)
    {
        // 게임 시간이 0 이하가 되면 GameOver 상태로 전환
        if (gameManager.gameTime <= 0)
        {
            gameManager.SetState(new GameOverState()); // 상태 전환
        }
    }

    // 상태 종료 시 호출
    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exiting Current Game State");
    }
}

// 게임 오버 상태 클래스
public class GameOverState : IGameState
{
    // 상태 진입 시 호출
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Entering Game Over State");
        gameManager.isLive = false; // 게임 진행 상태 비활성화
        gameManager.isCurrentGame = false; // 현재 게임 상태 비활성화
        gameManager.isNewGame = false; // 새로운 게임 상태 비활성화
        gameManager.isNextGame = false; // 다음 게임 상태 비활성화

        // 게임 오버 UI를 활성화
        gameManager.mainCanvs.GameoverUI();
    }

    // 상태 업데이트 로직 (게임 오버 상태에선 특별한 업데이트 없음)
    public void UpdateState(GameManager gameManager)
    {
        // Game Over 상태는 별도의 업데이트 로직 없음
    }

    // 상태 종료 시 호출
    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exiting Game Over State");
    }
}
