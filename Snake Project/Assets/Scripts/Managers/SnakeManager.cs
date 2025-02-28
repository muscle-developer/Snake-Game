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
    public float gap = 10f; // 몸체 간의 거리 (PositionsHistory 내에서의 인덱스 차이)

    // References
    public GameObject bodyPrefab;

    // Lists
    public List<GameObject> BodyParts = new List<GameObject>(); // 스네이크 몸체 오브젝트들을 저장하는 리스트
    public List<Vector3> PositionsHistory = new List<Vector3>(); // 스네이크의 위치 히스토리, 몸체들이 이 히스토리를 따라감

    [Header("아이템 관련")]
    public bool isMagnetActive = false;
    public float magnetRange = 10.0f; // 자석의 영향 범위
    public float magnetPullSpeed = 15.0f; // 아이템이 스네이크에게 끌려오는 속도
    
    // 추가: 스피드 부스트 코루틴을 추적할 변수
    private Coroutine speedBoostCoroutine;

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

    public void AddBodyPart() 
    {
        // PositionsHistory가 비어 있는 경우 현재 위치를 히스토리에 추가
        if (PositionsHistory.Count == 0)
            PositionsHistory.Add(transform.position);

        // 새로운 몸체가 추가될 위치를 결정:첫 번째 몸체일 경우 현재 스네이크의 위치, 아니면 히스토리에서 적당한 위치를 찾아서 할당
        // Vector3 newPosition = BodyParts.Count == 0 ? transform.position : PositionsHistory[Mathf.Clamp(BodyParts.Count * gap, 0, PositionsHistory.Count - 1)];

        // 히스토리에서 적절한 위치를 선택하도록 인덱스 계산
        int index = Mathf.Clamp(BodyParts.Count * (int)gap, 0, PositionsHistory.Count - 1);
        Vector3 newPosition = PositionsHistory[index];

        // 몸체 프리팹을 해당 위치에 인스턴스화하고, 첫 번째 몸체의 자식으로 설정 - 오브젝트 풀링
        GameObject body = PoolManager.Instance.GetFromPool(bodyPrefab, newPosition, Quaternion.identity, BodyParts.Count > 0 ? BodyParts[0].transform : null);

        // 생성된 몸체를 BodyParts 리스트에 추가
        BodyParts.Add(body);
    }

    public void DestroySnake()
    {
        // 1. 머리 제거 (이 경우에는 PoolManager로 반환)
        if (BodyParts.Count > 0)
        {
            GameObject head = BodyParts[0]; // BodyParts의 첫 번째 요소가 머리
            PoolManager.Instance.ReturnToPool(head);
            BodyParts.RemoveAt(0); // 리스트에서 머리 제거
        }

        // 2. 몸체 제거 (남은 몸체들도 PoolManager로 반환)
        foreach (GameObject body in BodyParts)
        {
            PoolManager.Instance.ReturnToPool(body);
        }

        // 3. 몸체 목록 초기화
        BodyParts.Clear();

        // 4. PositionsHistory 초기화 (위치 기록 초기화)
        PositionsHistory.Clear();

        // 5. 게임 오버 처리
        GameManager.Instance.GameOver();
    }


    // 스피드 부스트 적용 함수
    public void ApplySpeedBoost(float boostAmount, float duration)
    {
        // 이미 실행 중인 스피드 부스트가 있으면 중지
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
        }

        // 스피드가 5보다 작을 때만 부스트 추가
        if (snkaeSpeed == 5f && bodySpeed == 5f)
        {
            snkaeSpeed += boostAmount;
            bodySpeed += boostAmount;
            StartCoroutine(SmoothGapChange(5f, 0.5f)); // 스피드 부스트가 활성화될 때 gap을 부드럽게 줄임
        }

        // 지속시간을 갱신하여 새로운 코루틴 실행
        speedBoostCoroutine = StartCoroutine(ResetSpeedAfterDuration(boostAmount, duration));
    }

    // 일정 시간이 지나면 스피드를 원래대로 복구하는 함수
    IEnumerator ResetSpeedAfterDuration(float boostAmount, float duration)
    {
        // 부스트 지속시간을 기다림
        yield return new WaitForSeconds(duration);
        
        // 부스트 전의 속도로 복구
        snkaeSpeed -= boostAmount;
        bodySpeed -= boostAmount;
        
        // 속도가 복구될 때 간격을 부드럽게 원래대로 되돌림
        StartCoroutine(SmoothGapChange(10f, 1f)); 
    }

    // 간격을 부드럽게 조정하는 함수
    IEnumerator SmoothGapChange(float targetGap, float smoothTime)
    {
        float currentGap = gap; // 현재 간격
        float elapsedTime = 0f; // 경과 시간

        // smoothTime 동안 간격을 천천히 목표 값으로 변경
        while (elapsedTime < smoothTime)
        {
            gap = Mathf.Lerp(currentGap, targetGap, elapsedTime / smoothTime); // 부드럽게 간격 변경
            elapsedTime += Time.deltaTime; // 경과 시간 증가
            yield return null;
        }

        // 최종적으로 간격을 목표 값으로 설정
        gap = targetGap;
    }

    // 자석 효과를 활성화하는 함수
    public void ActivateMagnet(float duration)
    {
        // 이미 실행 중인 자석 비활성화 코루틴이 있으면 중지
        StopCoroutine(DeactivateMagnetAfterDuration(duration)); 
        isMagnetActive = true; // 자석 활성화 상태로 변경
        StartCoroutine(DeactivateMagnetAfterDuration(duration)); // 지속 시간 후 자석 비활성화 코루틴 실행
    }

    // 일정 시간이 지나면 자석 효과를 비활성화하는 함수
    IEnumerator DeactivateMagnetAfterDuration(float duration)
    {
        // 자석 지속시간을 기다림
        yield return new WaitForSeconds(duration);
        isMagnetActive = false; // 자석 효과 비활성화
    }

    // 자석 범위 내의 아이템들을 스네이크 쪽으로 끌어당기는 함수
    public void AttractItems()
    {
        // 플레이어의 위치를 중심으로 일정 범위 내의 모든 아이템 탐지
        Collider[] hitColliders = Physics.OverlapSphere(GameManager.Instance.player.transform.position, magnetRange);

        // 탐지된 각 아이템에 대해 처리
        foreach (var hitCollider in hitColliders)
        {
            // 사과 아이템인 경우
            if (hitCollider.CompareTag("Apple"))
            {
                // 아이템을 스네이크 쪽으로 이동시킴
                Vector3 direction = (GameManager.Instance.player.transform.position - hitCollider.transform.position).normalized;
                hitCollider.transform.position += direction * magnetPullSpeed * Time.deltaTime; // 자석 속도로 아이템 이동
            }
        }
    }
}
