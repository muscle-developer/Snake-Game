using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySnake : MonoBehaviour
{
    public NavMeshAgent agent; // 적 스네이크의 NavMeshAgent
    private Transform player; // 플레이어 객체 참조
    private EnemySnakeManager enemySnakeManager; // EnemySnakeManager 인스턴스 참조

    public List<Vector3> positionsHistory = new List<Vector3>(); // 스네이크의 위치 히스토리 기록 (몸체가 머리를 따라가도록 사용)
    public List<GameObject> bodyParts = new List<GameObject>(); // 스네이크 몸체의 게임 오브젝트 목록

    // 적 스네이크의 레벨
    public int enemyLevel;

    // 적이 플레이어를 추적하는 상태와 타이머
    private float chaseStartTime; // 추적 시작 시간 기록
    [SerializeField]
    private bool isChasing; // 현재 추적 중인지 여부
    [SerializeField]
    private bool isCooldown; // 추적 후 쿨다운 중인지 여부
    private float cooldownTime = 5f; // 쿨다운 시간 (추적 종료 후 대기 시간)

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent 컴포넌트 가져오기
        player = GameManager.Instance.player.transform; // GameManager를 통해 플레이어 참조
        enemySnakeManager = EnemySnakeManager.Instance; // 싱글톤 EnemySnakeManager 인스턴스 가져오기
        
        // 첫 번째 위치를 히스토리에 추가
        positionsHistory.Add(transform.position);
        
        // 초기 몸체 추가
        InitializeBodyParts();

        // 적 스네이크의 레벨을 초기 몸체 개수로 설정
        enemyLevel = bodyParts.Count;

        // 초기 상태 설정
        isChasing = false; // 시작 시 추적하지 않음
        isCooldown = false; // 쿨다운 상태 아님
    }

    // 초기 몸체를 추가하는 함수
    public void InitializeBodyParts()
    {
        // 초기 몸체를 3개 생성
        for (int i = 0; i < 3; i++)
        {
            AddBodyPart();
        }
    }

    void Update()
    {
        // 플레이어의 레벨을 가져옴 (머리를 제외한 몸체 수로 계산하므로 -1)
        int playerLevel = SnakeManager.Instance.BodyParts.Count - 1;

        if (GameManager.Instance.player == null)
            return;
            
        // 적 스네이크가 플레이어를 추적하거나 랜덤하게 이동
        // 추적 조건: 쿨다운 상태가 아니며, 적 스네이크의 레벨이 플레이어보다 높고, 플레이어가 탐지 범위 내에 있을 때
        if (!isCooldown && enemyLevel > playerLevel && Vector3.Distance(transform.position, player.position) < enemySnakeManager.detectionRange)
        {   
            // 추적을 시작하지 않은 경우, 추적 시작
            if (!isChasing)
            {
                isChasing = true;
                chaseStartTime = Time.time; // 추적 시작 시간 기록
            }

            // 추적 시간이 5초 미만일 경우 플레이어 추적
            if (Time.time - chaseStartTime < 5f)
            {
                agent.SetDestination(player.position); // 플레이어 위치로 이동 경로 설정
            }
            else
            {
                isChasing = false; // 5초 경과 시 추적 중지
                isCooldown = true; // 쿨다운 상태로 전환
                chaseStartTime = Time.time; // 쿨다운 시작 시간 기록
            }
        }
        else
        {
            // 추적하지 않을 때 랜덤 방향으로 이동
            if (!agent.hasPath)
            {
                Vector3 randomDirection = Random.insideUnitSphere * 10f; // 반경 10 유닛 내에서 무작위 방향 생성
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, 10f, 1); // NavMesh 상 무작위 위치 찾기
                agent.SetDestination(hit.position); // 무작위 위치로 이동 경로 설정
            }

            // 쿨다운 시간이 지나면 추적 가능 상태로 복귀
            if (isCooldown && Time.time - chaseStartTime >= cooldownTime)
            {
                isCooldown = false;
            }
        }

        // 위치 히스토리 갱신
        positionsHistory.Insert(0, transform.position);

        // 히스토리 크기 제한 (불필요하게 커지지 않도록 1000개로 제한)
        if (positionsHistory.Count > 1000)
        {
            positionsHistory.RemoveAt(positionsHistory.Count - 1);
        }

        // 몸체가 머리를 따라 이동하도록 처리
        FollowBodyParts();
    }

    // 새로운 몸체 추가 함수
    public void AddBodyPart()
    {
        Vector3 newPosition;

        // 기존 몸체가 있는 경우 마지막 몸체의 위치를 참조하여 뒤에 추가
        if (bodyParts.Count > 0)
            newPosition = positionsHistory[Mathf.Clamp(bodyParts.Count * (int)enemySnakeManager.gap, 0, positionsHistory.Count - 1)];
        else
            // 첫 번째 몸체는 머리 뒤에 추가
            newPosition = transform.position - transform.forward * enemySnakeManager.gap;

        // 풀에서 새 몸체 가져오기 및 위치 지정
        GameObject newBodyPart = PoolManager.Instance.GetFromPool(enemySnakeManager.enemyBodyPrefab, newPosition, Quaternion.identity, transform);

        // 새 몸체를 리스트에 추가
        bodyParts.Add(newBodyPart);

        // 생성된 몸체의 위치를 히스토리에 추가
        positionsHistory.Add(newBodyPart.transform.position);
    }

    // 몸체가 머리를 따라가도록 하는 함수
    public void FollowBodyParts()
    {
        for (int i = 0; i < bodyParts.Count; i++)
        {
            // 각 몸체가 따라갈 목표 위치 설정
            Vector3 targetPosition = positionsHistory[Mathf.Clamp(i * (int)enemySnakeManager.gap, 0, positionsHistory.Count - 1)];
            Vector3 moveDirection = targetPosition - bodyParts[i].transform.position;

            // 몸체 이동 (머리를 따라 목표 위치로 이동)
            bodyParts[i].transform.position += moveDirection * enemySnakeManager.bodySpeed * Time.deltaTime;

            // 몸체 회전 (이동 방향으로 회전)
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                bodyParts[i].transform.rotation = Quaternion.Slerp(bodyParts[i].transform.rotation, targetRotation, enemySnakeManager.bodySpeed * Time.deltaTime);
            }
        }
    }

    // 적 스네이크가 죽을 때 몸체를 무작위 위치에 생성하는 함수
    public void SpawnBodyPartsOnDeath()
    {
        // 몸체가 생성될 반경
        float spawnRadius = 2.0f;

        // 몸체 개수만큼 반복하여 생성
        for (int i = 0; i < bodyParts.Count; i++)
        {
            // 무작위 위치 설정
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = 0; // 평면에서만 위치 설정

            // 무작위 위치 계산
            Vector3 spawnPosition = transform.position + randomOffset;

            // 풀에서 새 몸체를 가져와 무작위 위치에 생성
            GameObject newBodyPart = PoolManager.Instance.GetFromPool(enemySnakeManager.enemyBodyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
