using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform player;

    public void Awake()
    {
        GameManager.Instance = this;
    }

    // 게임 시작 시 SnakeHead를 생성하고 있는데,
    // GameManager에서 player 변수가 올바르게 설정되지 않는 문제는 SnakeHead가 Awake()에서 GameManager.Instance.player에 할당되기 전에 GameManager의 Awake()가 먼저 실행되기 때문입니다. 
    // 이로 인해 player 변수가 null 상태일 수 있습니다.
    // SnakeHead를 생성한 후에 GameManager의 player 변수를 설정하도록 변경, 아래와 같이 수정
    // public void SetPlayer(Transform playerTransform)
    // {
    //     player = playerTransform;
    // }

    void Start()
    {   
        // StartCoroutine(FindPlayerCoroutine());
    }
    
    // private IEnumerator FindPlayerCoroutine()
    // {
    //     while (player == null)
    //     {
    //         player = FindObjectOfType<SnakeHead>()?.transform; // ?.는 널 조건부 연산자로, FindObjectOfType<SnakeHead>()가 null인 경우 안전하게 target을 null로 설정합니다.
    //         yield return null;
    //     }
    // }

    // 게임 오버 처리
    public void GameOver()
    {
        Debug.Log("Game Over! Player's snake has been destroyed.");
        // 여기에서 게임 오버 UI 표시나 리스타트 기능 등을 추가할 수 있습니다.
        // 예: UIManager.Instance.ShowGameOverScreen();
    }
}
