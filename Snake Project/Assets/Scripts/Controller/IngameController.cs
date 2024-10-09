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

    public Vector2 spawnAreaSize; // 생성 영역 크기
    public List<GameObject> rangeObject = new List<GameObject>(); // 적을 생성할 여러 개의 Floor 오브젝트들

    void Awake()
    {
        IngameController.Instance = this;

        // "Floor" 태그를 가진 모든 오브젝트를 찾아 rangeObject 리스트에 추가
        GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
        rangeObject.AddRange(floors);

        // 리스트에서 null 값을 제거하여 유효한 오브젝트들만 남김
        rangeObject.RemoveAll(item => item == null);
    }

    void Start()
    {
        InstantiateSnakeHead();
        InstantiateEnemySnakes(3); // 적 스네이크 생성 메서드 호출
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

    // 여러 적을 랜덤한 위치에 생성
    private void InstantiateEnemySnakes(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // 랜덤한 위치 생성
            Vector3 randomPosition = ReturnRandomPosition();

            if (randomPosition != Vector3.zero)
            {
                GameObject enemySnake = Instantiate(enemySnakePrefab, randomPosition, Quaternion.identity, transform);
                // 적에 필요한 초기화 코드 추가 가능
            }
        }
    }

    // 랜덤한 위치를 반환하는 함수
    private Vector3 ReturnRandomPosition()
    {
        if (rangeObject.Count == 0)
            return Vector3.zero;

        // 무작위로 rangeObject에서 오브젝트 선택
        GameObject randomRangeObject = rangeObject[Random.Range(0, rangeObject.Count)];
        Vector3 originPosition = randomRangeObject.transform.position;
        Vector3 scale = randomRangeObject.transform.localScale;

        float randomX = Random.Range(-scale.x / 2, scale.x / 2);
        float randomZ = Random.Range(-scale.z / 2, scale.z / 2);
        Vector3 randomPosition = new Vector3(randomX, 0.5f, randomZ);

        return originPosition + randomPosition;
    }
}
