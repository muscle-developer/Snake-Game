using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    // 전역에서 사용할 수 있도록 싱글톤화
    public static SnakeManager Instance;
    [SerializeField]
    private GameObject headPrefab; // 머리 프리팹
    [SerializeField]
    private GameObject bodyPrefab; // 몸통 프리팹
    [SerializeField]
    private int initialBodyParts = 3; // 초기 몸통 개수
    public float bodySpacing = 1.0f; // 몸통 간격

    private List<GameObject> bodyParts = new List<GameObject>(); // 몸통 목록
    public List<GameObject> BodyParts => bodyParts;
    private List<Vector3> headPositions = new List<Vector3>(); // 머리 위치 기록
    public List<Vector3> HeadPositions => headPositions;

    private const int maxHeadPositions = 100;
    private float recordInterval = 0.1f; // 위치 기록 간격
    private float recordTimer;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        // 예를들어 게임매니저 OR 아웃게임 컨트롤러에서 게임 실행시
        // 뱀의 머리를 생성

        // 초기 머리 위치를 기록합니다.
        RecordHeadPosition(transform.position);

        for(int i = 0; i < initialBodyParts; i++)
        {
            AddBodyPart();
        }

        // 머리와 몸통의 충돌을 무시합니다.
        Collider headCollider = GetComponent<Collider>();
        foreach (var body in bodyParts)
        {
            Collider bodyCollider = body.GetComponent<Collider>();
            if (bodyCollider != null && headCollider != null)
            {
                Physics.IgnoreCollision(headCollider, bodyCollider);
            }
        }
    }

    void FixedUpdate()
    {
        recordTimer += Time.fixedDeltaTime;
        if (recordTimer >= recordInterval)
        {
            RecordHeadPosition(transform.position);
            recordTimer = 0f;
        }
    }

    public void RecordHeadPosition(Vector3 position)
    {
        headPositions.Insert(0, position);
        if (headPositions.Count > maxHeadPositions)
        {
            headPositions.RemoveAt(headPositions.Count - 1);
        }
        Debug.Log($"Recorded head position: {position}");
    }

    public void AddBodyPart()
    {
        GameObject body = Instantiate(bodyPrefab);

        Vector3 spawnPosition;
        if (bodyParts.Count > 0)
        {
            // 마지막 몸통 파트의 위치를 가져와서 새 몸통 파트를 해당 위치에 배치합니다.
            Vector3 lastBodyPartPosition = bodyParts[bodyParts.Count - 1].transform.position;
            spawnPosition = lastBodyPartPosition - (transform.forward * bodySpacing);
        }
        else
        {
            // 첫 번째 몸통 파트는 초기 위치에 배치합니다.
            spawnPosition = transform.position - (transform.forward * bodySpacing);
        }

        body.transform.position = spawnPosition;
        bodyParts.Add(body);
    }
}
