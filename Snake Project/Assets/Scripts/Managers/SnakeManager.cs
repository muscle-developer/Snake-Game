using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    // 스네이크 관리를 위한 매니저 스크립트
    // 역할: 게임의 전역적인 상태와 시스템을 관리. 주로 스네이크의 몸체를 추가하고, 몸체의 리스트를 관리하는 역할을 합니다.
    public static SnakeManager Instance;
    // Settings
    [Header("스네이크 관리")]
    public float snkaeSpeed = 5.0f; // 스네이크의 이동 속도
    public float rotationSpeed = 300.0f; // 스네이크가 회전하는 속도
    public float bodySpeed = 5.0f; // 스네이크의 몸체가 따라오는 속도
    public int gap = 10; // 몸체 간의 거리 (PositionsHistory 내에서의 인덱스 차이)

    // References
    public GameObject BodyPrefab;

    // Lists
    public List<GameObject> BodyParts = new List<GameObject>(); // 스네이크 몸체 오브젝트들을 저장하는 리스트
    public List<Vector3> PositionsHistory = new List<Vector3>(); // 스네이크의 위치 히스토리, 몸체들이 이 히스토리를 따라감

    [Header("아이템 관련")]
    private bool isMagnetActive = false;

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

        // 게임이 시작될 때 스네이크의 머리 위치를 PositionsHistory에 추가
        if (BodyParts.Count > 0)
        {
            PositionsHistory.Add(BodyParts[0].transform.position); // 첫 번째 BodyPart는 뱀의 머리
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AddBodyPart();
        }
    }

    public void AddBodyPart() 
    {
        // PositionsHistory가 비어 있는 경우 현재 위치를 히스토리에 추가
        if (PositionsHistory.Count == 0)
            PositionsHistory.Add(transform.position);

        // 새로운 몸체가 추가될 위치를 결정:첫 번째 몸체일 경우 현재 스네이크의 위치, 아니면 히스토리에서 적당한 위치를 찾아서 할당
        Vector3 newPosition = BodyParts.Count == 0 ? transform.position : PositionsHistory[Mathf.Clamp(BodyParts.Count * gap, 0, PositionsHistory.Count - 1)];

        // 몸체 프리팹을 해당 위치에 인스턴스화하고, 첫 번째 몸체의 자식으로 설정
        GameObject body = Instantiate(BodyPrefab, newPosition, Quaternion.identity, BodyParts[0].transform);

        // 생성된 몸체를 BodyParts 리스트에 추가
        BodyParts.Add(body);
    }

    // 스피드 부스트 적용 함수
    public void ApplySpeedBoost(float boostAmount, float duration)
    {
        snkaeSpeed += boostAmount;
        StartCoroutine(ResetSpeedAfterDuration(boostAmount, duration));
    }

    IEnumerator ResetSpeedAfterDuration(float boostAmount, float duration)
    {
        yield return new WaitForSeconds(duration);
        snkaeSpeed -= boostAmount;
    }

    // 자석 효과 활성화 함수
    public void ActivateMagnet(float duration)
    {
        isMagnetActive = true;
        StartCoroutine(DeactivateMagnetAfterDuration(duration));
    }

    IEnumerator DeactivateMagnetAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isMagnetActive = false;
    }
}
