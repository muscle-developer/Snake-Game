using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 개별 객체 제어: 특정 객체나 캐릭터의 행동과 동작을 제어합니다.
// 입력 처리: 플레이어의 입력을 받아 해당 객체가 어떻게 반응할지를 결정합니다.
// 객체 중심: 주로 개별 게임 객체에 붙어서 동작하며, 해당 객체의 움직임, 애니메이션, 상호작용 등을 관리합니다.
public class IngameController : MonoBehaviour
{
    public static IngameController Instance;
    public VariableJoystick joystick;
    public UIJoystick uiJoystick;
    public GameObject snakeHeadPrefab; // SnakeHead 프리팹
    public GameObject enemySnakePrefab; // Enemysnake 프리팹

    void Awake()
    {
        IngameController.Instance = this;
    }

    void Start()
    {
        InstantiateSnakeHead();
        InstantiateEnemySnakes(); // 적 스네이크 생성 메서드 호출
    }

    private void InstantiateSnakeHead()
    {
        GameObject snakeHead = Instantiate(snakeHeadPrefab, transform.position, transform.rotation, transform);
        SnakeHead snakeHeadObj = snakeHead.GetComponent<SnakeHead>();  

        if(snakeHeadObj != null)
        {
            snakeHeadObj.joystick = joystick;
            uiJoystick.Initialize(snakeHeadObj);
        }

        SnakeManager.Instance.BodyParts.Add(snakeHead); // SnakeHead를 BodyParts에 추가
        SnakeManager.Instance.PositionsHistory.Add(snakeHead.transform.position); // SnakeHead의 위치를 PositionsHistory에 추가

        // GameManager의 player 변수 설정
        // GameManager.Instance.SetPlayer(snakeHead.transform);
    }

    private void InstantiateEnemySnakes()
    {
        // 예시로 3개의 EnemySnake를 생성
        for (int i = 0; i < 3; i++)
        {
            Instantiate(enemySnakePrefab, new Vector3(i * 2, 0, 0), Quaternion.identity);
        }
    }
}
