using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySnake : MonoBehaviour
{
    public GameObject bodyPrefab; // 스네이크 몸체 프리팹
    public List<GameObject> bodyParts = new List<GameObject>(); // 적 스네이크의 몸체
    public List<Vector3> positionsHistory = new List<Vector3>(); // 위치 히스토리
    public float gap = 10f; // 몸체 간의 거리
    public float bodySpeed = 3f; // 몸체가 따라오는 속도
    public float detectionRange = 15f; // 플레이어 탐지 범위
    public NavMeshAgent agent; // 적 스네이크의 NavMeshAgent
    [SerializeField]
    private Transform player; // 플레이어 참조

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameManager.Instance.player.transform; // 플레이어 객체 참조

        // 적 스네이크의 머리 위치 초기화
        positionsHistory.Add(transform.position);

        // 초기 몸체 생성
        for (int i = 0; i < 3; i++)
        {
            AddBodyPart();
        }
    }

    void Update()
    {
        // 플레이어를 탐지하여 추적하거나 랜덤하게 움직임
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            // 랜덤 이동 설정
            if (!agent.hasPath)
            {
                Vector3 randomDirection = Random.insideUnitSphere * 10f;
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, 10f, 1);
                agent.SetDestination(hit.position);
            }
        }

        // 적 스네이크의 위치를 히스토리에 기록
        positionsHistory.Insert(0, transform.position);

        // 위치 히스토리가 일정 크기를 넘으면 가장 오래된 기록을 제거
        if (positionsHistory.Count > 1000)
        {
            positionsHistory.RemoveAt(positionsHistory.Count - 1);
        }
    }

    public void AddBodyPart()
    {
        // 풀에서 몸체를 가져옴
        Vector3 newPosition = positionsHistory[Mathf.Clamp(bodyParts.Count * (int)gap, 0, positionsHistory.Count - 1)];
        GameObject body = PoolManager.Instance.GetFromPool(bodyPrefab, newPosition, Quaternion.identity, bodyParts.Count > 0 ? bodyParts[0].transform : null);
        bodyParts.Add(body);
    }

    void FixedUpdate()
    {
        // 적 스네이크의 몸체들이 머리 위치를 따라오도록 설정
        int index = 0;
        foreach (var body in bodyParts)
        {
            Vector3 point = positionsHistory[Mathf.Clamp(index * (int)gap, 0, positionsHistory.Count - 1)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * bodySpeed * Time.deltaTime;

            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                body.transform.rotation = Quaternion.Slerp(body.transform.rotation, targetRotation, bodySpeed * Time.deltaTime);
            }
            index++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Apple"))
        {
            // 사과를 먹고 몸체를 추가
            AddBodyPart();
            PoolManager.Instance.ReturnToPool(other.gameObject); // 사과를 풀로 반환
        }

        if (other.CompareTag("Snake Head"))
        {
            // 플레이어와 충돌 시 처리 로직
            if (bodyParts.Count > SnakeManager.Instance.BodyParts.Count)
            {
                // 적 스네이크가 플레이어보다 클 경우 플레이어 공격
                SnakeManager.Instance.DestroySnake();
            }
            else
            {
                // 플레이어가 더 클 경우 적 스네이크 제거
                Destroy(gameObject);
            }
        }
    }
}
