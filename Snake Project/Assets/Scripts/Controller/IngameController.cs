using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 개별 객체 제어: 특정 객체나 캐릭터의 행동과 동작을 제어합니다.
// 입력 처리: 플레이어의 입력을 받아 해당 객체가 어떻게 반응할지를 결정합니다.
// 객체 중심: 주로 개별 게임 객체에 붙어서 동작하며, 해당 객체의 움직임, 애니메이션, 상호작용 등을 관리합니다.
public class IngameController : MonoBehaviour
{
    public static IngameController Instance;
    [Header("Sanke")]
    public GameObject snakeHeadPrefab; // SnakeHead 프리팹
    public GameObject enemySnakePrefab; // Enemysnake 프리팹
    public int enemySnakeCount = 3; // 적 뱀의 갯수

    public Vector2 spawnAreaSize; // 생성 영역 크기
    public List<GameObject> rangeObject = new List<GameObject>(); // 적을 생성할 여러 개의 Floor 오브젝트들

    [Header("UI")] 
    public VariableJoystick joystick;
    public UIJoystick uiJoystick;
    public UIViewFloatingHUD uiViewFloatingHUD;

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

        // 플레이어 HUD 생성 및 연결
        CreateHUD(snakeHead.transform, 0, true);
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
                enemySnake.name = $"EnemySnake_{i}"; // 스네이크 이름 설정
                // 적에 필요한 초기화 코드 추가 가능
                EnemySnakeManager.Instance.InitializeEnemySnake(enemySnake); // EnemySnakeManager에 적 초기화

                // 적의 HUD 캔버스를 생성하고 스네이크와 연결
                CreateHUD(enemySnake.transform, 3, false);
            }
        }
    }

    // HUD를 생성하는 메서드
    private void CreateHUD(Transform snakeTransform, int initialLevel, bool isPlayer = false)
    {
        var snakeCanvas = uiViewFloatingHUD;
        if (snakeCanvas != null)
        {
            snakeCanvas.Initialize(snakeTransform, initialLevel, isPlayer); // Transform과 초기 레벨 전달
        }
    }

    // HUD를 삭제하는 메서드
    public void RemoveHUD(GameObject snakeTransform)
    {
        // 적 스네이크 삭제 전에 HUD도 삭제
        uiViewFloatingHUD.RemoveSnakeHUD(snakeTransform.transform);
    }

    // 적을 랜덤한 위치를 반환하는 함수
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

    // 촬영을 위한 임시 코드
    // private Vector3 ReturnRandomPosition()
    // {
    //     if (SnakeManager.Instance.BodyParts.Count == 0)
    //         return Vector3.zero;

    //     // 플레이어 스네이크의 위치 가져오기
    //     Vector3 playerPosition = SnakeManager.Instance.BodyParts[0].transform.position;

    //     // 생성 반경 설정
    //     float spawnRadius = 10f; // 적 생성 반경
    //     float minRadius = 5f; // 플레이어와 너무 가까운 위치 방지

    //     // 랜덤 각도와 거리로 위치 계산
    //     float randomAngle = Random.Range(0, 360) * Mathf.Deg2Rad;
    //     float randomDistance = Random.Range(minRadius, spawnRadius);

    //     float offsetX = Mathf.Cos(randomAngle) * randomDistance;
    //     float offsetZ = Mathf.Sin(randomAngle) * randomDistance;

    //     Vector3 randomPosition = new Vector3(offsetX, 0.5f, offsetZ);

    //     return playerPosition + randomPosition;
    // }

}
