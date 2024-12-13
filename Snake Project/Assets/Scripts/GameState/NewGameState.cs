using UnityEngine;

public class NewGameState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Entering New Game State");
        gameManager.isNewGame = true;   // 새로운 게임 상태로 설정
        gameManager.isCurrentGame = false;
        gameManager.isNextGame = false;
        gameManager.isLive = true;

        gameManager.gameTime = 60;
    }

    public void UpdateState(GameManager gameManager)
    {
        // NewGame 상태에서 필요한 업데이트 로직
        if (gameManager.gameTime <= 0)
        {
            gameManager.SetState(new GameOverState());  // 게임 오버 상태로 전환
        }
    }

    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exiting New Game State");
    }
}

public class NextGameState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Entering Next Game State");
        gameManager.isNextGame = true;
        gameManager.isNewGame = false;
        gameManager.isCurrentGame = false;
        gameManager.isLive = true;
    }

    public void UpdateState(GameManager gameManager)
    {
        if (gameManager.gameTime <= 0)
        {
            gameManager.SetState(new GameOverState());
        }
    }

    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exiting Next Game State");
    }
}

public class CurrentGameState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Entering Current Game State");
        gameManager.isCurrentGame = true;
        gameManager.isNewGame = false;
        gameManager.isNextGame = false;
        gameManager.isLive = true;
    }

    public void UpdateState(GameManager gameManager)
    {
        if (gameManager.gameTime <= 0)
        {
            gameManager.SetState(new GameOverState());
        }
    }

    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exiting Current Game State");
    }
}

public class GameOverState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Entering Game Over State");
        gameManager.isLive = false;
        gameManager.isCurrentGame = false;
        gameManager.isNewGame = false;
        gameManager.isNextGame = false;
        gameManager.mainCanvs.GameoverUI();
    }

    public void UpdateState(GameManager gameManager)
    {
        // Game Over 상태에서는 추가 업데이트 로직 없음
    }

    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exiting Game Over State");
    }
}
