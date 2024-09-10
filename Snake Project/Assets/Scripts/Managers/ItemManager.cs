using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<GameObject> rangeObject = new List<GameObject>(); // 여러 개의 Floor 오브젝트들
    public GameObject applePrefab; // 생성할 사과 프리팹
    public int initialAppleCount = 300; // 초기 생성할 사과 개수
    public int minAppleCount = 100; // 사과가 이 수 이하로 줄어들면 새로 생성
    public int appleToRespawn = 50; // 부족할 때마다 새로 생성할 사과 개수

    [SerializeField]
    private List<GameObject> spawnedApples = new List<GameObject>(); // 생성된 사과들을 관리할 리스트

    private void Awake()
    {
        if (rangeObject.Count == 0)
            return;

        // "Floor" 태그가 붙은 모든 오브젝트를 찾아 리스트에 추가
        GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
        rangeObject.AddRange(floors);

        // 리스트에서 null 값을 제거
        rangeObject.RemoveAll(item => item == null);
    }

    private void Start()
    {
        // 리스트가 비어있지 않은지 확인
        if (rangeObject.Count == 0)
        {
            Debug.LogError("rangeObject 리스트가 비어있습니다. Floor 오브젝트들이 추가되지 않았습니다.");
            return;
        }

        // 초기 사과 생성
        SpawnApples(initialAppleCount);

        // 사과 수를 지속적으로 확인하고 생성하는 코루틴 시작
        StartCoroutine(CheckAndRespawnApples());
    }

    // 사과를 특정 개수만큼 생성하는 함수
    private void SpawnApples(int count)
    {
        GameObject spawnArea = GameObject.Find("Spawn Area");
        Transform spawnAppleTransform = spawnArea.transform.Find("Spawn Apple");

        if (spawnArea == null && spawnAppleTransform == null)
            return;

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = ReturnRandomPosition(spawnAppleTransform);
            if (spawnPosition != Vector3.zero) // 유효한 위치일 경우에만 사과 생성
            {
                // Instantiate 오브젝트를 부모(spawnAppleTransform)를 설정하여 생성
                GameObject apple = Instantiate(applePrefab, spawnPosition, Quaternion.identity, spawnAppleTransform);
                spawnedApples.Add(apple);
            }
        }
#region BoxCollider을 사용한 랜덤생성
        // for (int i = 0; i < count; i++)
        // {
        //     // 무작위로 rangeObject에서 오브젝트 선택
        //     GameObject randomRangeObject = rangeObject[Random.Range(0, rangeObject.Count)];
            
        //     // Spawn Apple의 Transform을 가져옵니다.
        //     Transform spawnTransform = randomRangeObject.transform.Find("Spawn Apple");

        //     if (spawnTransform != null)
        //     {
        //         // 랜덤 위치를 반환받아 사과 생성
        //         Vector3 spawnPosition = ReturnRandomPosition(spawnTransform);
        //         if (spawnPosition != Vector3.zero) // 유효한 위치일 경우에만 사과 생성
        //         {
        //             GameObject apple = Instantiate(applePrefab, spawnPosition, Quaternion.identity);
        //             spawnedApples.Add(apple);
        //         }
        //     }
        //     else
        //     {
        //         Debug.LogError($"'{randomRangeObject.name}' 오브젝트에 'Spawn Apple' 자식이 없습니다.");
        //     }
        // }
#endregion
    }

    // 사과의 수가 줄어들면 새로 생성하는 코루틴
    IEnumerator CheckAndRespawnApples()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            // 현재 사과의 개수를 확인
            spawnedApples.RemoveAll(apple => apple == null); // 리스트에서 제거된 사과(null)를 삭제
            if (spawnedApples.Count < minAppleCount)
            {
                int applesToSpawn = appleToRespawn;
                Debug.Log($"사과 개수가 부족합니다. {applesToSpawn}개 추가 생성합니다.");
                SpawnApples(applesToSpawn);
            }
        }
    }

    private Vector3 ReturnRandomPosition(Transform spawnTransform)
    {
        if (rangeObject.Count == 0)
            return Vector3.zero; // 기본값 반환

        // 무작위로 rangeObject에서 오브젝트 선택
        GameObject randomRangeObject = rangeObject[Random.Range(0, rangeObject.Count)];

#region Pos, Scale을 사용한 랜덤 생성
        // 오브젝트의 Transform을 기준으로 범위 계산 (localScale을 사용)
        Vector3 originPosition = randomRangeObject.transform.position;
        Vector3 scale = randomRangeObject.transform.localScale;

        // X, Z 축 범위 내에서 랜덤 좌표 생성
        float randomX = Random.Range(-scale.x / 2, scale.x / 2);
        float randomZ = Random.Range(-scale.z / 2, scale.z / 2);
        
        // Y축은 0으로 고정하고 X, Z 축에만 변화를 줌
        Vector3 randomPosition = new Vector3(randomX, 0f, randomZ);

        // 최종 생성 위치 계산
        Vector3 respawnPosition = originPosition + randomPosition;

        return respawnPosition;
#endregion

#region BoxCollider를 사용한 랜덤생성
        // Spawn Apple의 BoxCollider 가져오기
        // BoxCollider rangeCollider = spawnTransform.GetComponent<BoxCollider>();

        // if (rangeCollider == null)
        // {
        //     Debug.LogError($"'{spawnTransform.name}' 오브젝트에 BoxCollider가 없습니다.");
        //     return spawnTransform.position; // Collider가 없어도 위치만 반환
        // }

        // Vector3 originPosition = spawnTransform.position;
        // float range_X = rangeCollider.size.x * spawnTransform.localScale.x;
        // float range_Z = rangeCollider.size.z * spawnTransform.localScale.z;

        // // 랜덤 좌표 계산
        // float randomX = Random.Range(-range_X / 2, range_X / 2);
        // float randomZ = Random.Range(-range_Z / 2, range_Z / 2);
        // Vector3 randomPosition = new Vector3(randomX, 0f, randomZ);

        // Vector3 respawnPosition = originPosition + randomPosition;

        // return respawnPosition;
#endregion  
    }
}
