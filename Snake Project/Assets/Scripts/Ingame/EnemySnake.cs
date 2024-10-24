using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySnake : MonoBehaviour
{
    public NavMeshAgent agent; // 적 스네이크의 NavMeshAgent
    private Transform player; // 플레이어 참조
    private EnemySnakeManager enemySnakeManager;

    // 각 적 스네이크별 위치 히스토리와 몸체
    public List<Vector3> positionsHistory = new List<Vector3>(); 
    public List<GameObject> bodyParts = new List<GameObject>();

    // 적 스네이크 레벨
    public int level;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameManager.Instance.player.transform; // 플레이어 객체 참조
        enemySnakeManager = EnemySnakeManager.Instance;
        
        // 첫 위치 히스토리 추가
        positionsHistory.Add(transform.position);

        // 적 스네이크 초기화 시 레벨 설정
        level = bodyParts.Count;
        
        // 적 스네이크 초기화에서 몸체 추가를 호출
        InitializeBodyParts();
    }

    // 몸체 추가 초기화 함수
    public void InitializeBodyParts()
    {
        for (int i = 0; i < 3; i++) // 초기 몸체 3개 생성
        {
            AddBodyPart();
        }
    }

    void Update()
    {
        // 플레이어를 탐지하여 추적하거나 랜덤하게 움직임
        if (Vector3.Distance(transform.position, player.position) < enemySnakeManager.detectionRange)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            // 랜덤 이동
            if (!agent.hasPath)
            {
                Vector3 randomDirection = Random.insideUnitSphere * 10f;
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, 10f, 1);
                agent.SetDestination(hit.position);
            }
        }

        // 위치 히스토리 갱신
        positionsHistory.Insert(0, transform.position);

        // 히스토리 크기 제한 (필요에 따라 조절 가능)
        if (positionsHistory.Count > 1000)
        {
            positionsHistory.RemoveAt(positionsHistory.Count - 1);
        }

        // 몸체들이 머리를 따라가도록 관리
        FollowBodyParts();
    }

    // 몸체 추가 함수
    public void AddBodyPart()
    {
        Vector3 newPosition;

        // 기존 몸통이 있는 경우 마지막 몸통의 위치 참조
        if (bodyParts.Count > 0)
            // 마지막 몸통의 위치에서 enemySnakeManager.gap만큼 뒤쪽에 새 몸통 생성
            newPosition = positionsHistory[Mathf.Clamp(bodyParts.Count * (int)enemySnakeManager.gap, 0, positionsHistory.Count - 1)];
        else
            // 첫 몸통은 머리의 뒤에 생성
            newPosition = transform.position - transform.forward * enemySnakeManager.gap;

        // 새 몸통 생성
        GameObject newBodyPart = PoolManager.Instance.GetFromPool(enemySnakeManager.enemyBodyPrefab, newPosition, Quaternion.identity, transform);

        // 새 몸통을 리스트에 추가
        bodyParts.Add(newBodyPart);

        // 생성된 몸통 위치를 positionsHistory에 추가
        positionsHistory.Add(newBodyPart.transform.position);
    }

    // 몸체들이 머리를 따라가도록 하는 함수
    public void FollowBodyParts()
    {
        for (int i = 0; i < bodyParts.Count; i++)
        {
            // 각 몸통이 따라갈 목표 위치 설정
            Vector3 targetPosition = positionsHistory[Mathf.Clamp(i * (int)enemySnakeManager.gap, 0, positionsHistory.Count - 1)];
            Vector3 moveDirection = targetPosition - bodyParts[i].transform.position;

            // 몸체 이동
            bodyParts[i].transform.position += moveDirection * enemySnakeManager.bodySpeed * Time.deltaTime;

            // 몸체 회전
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                bodyParts[i].transform.rotation = Quaternion.Slerp(bodyParts[i].transform.rotation, targetRotation, enemySnakeManager.bodySpeed * Time.deltaTime);
            }
        }
    }

    // 적 스네이크가 죽을 때 몸통을 생성하는 함수
    public void SpawnBodyPartsOnDeath()
    {
        // 몸통을 생성할 반경
        float spawnRadius = 2.0f;

        // 적 스네이크의 몸통 개수만큼 반복
        for (int i = 0; i < bodyParts.Count; i++)
        {
            // 무작위 위치 생성
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = 0; // 평면상에서만 위치를 변경하도록 Y축은 0으로 고정

            // 무작위로 생성될 위치 계산
            Vector3 spawnPosition = transform.position + randomOffset;

            // 새 몸통을 풀에서 가져와 무작위 위치에 생성
            GameObject newBodyPart = PoolManager.Instance.GetFromPool(enemySnakeManager.enemyBodyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Snake Head"))
    //     {
    //         // 플레이어의 레벨을 가져옴
    //         int playerLevel = SnakeManager.Instance.BodyParts.Count;
    //         int enemyLevel = level;

    //         // 플레이어 레벨과 적 스네이크 레벨 비교
    //         if (playerLevel > enemyLevel)
    //         {
    //             // Transform playerTransform = other.transform.root; // 적의 머리나 몸체의 부모 트랜스폼을 가져옴
    //             IngameController.Instance.RemoveHUD(other.gameObject);

    //             // 플레이어의 레벨이 더 높으면 적 스네이크 제거
    //             enemySnakeManager.DestroyEnemySnake(this);

    //             // 죽은 자리에 몸통 생성
    //             SpawnBodyPartsOnDeath();
    //         }
    //     }
    // }
}
