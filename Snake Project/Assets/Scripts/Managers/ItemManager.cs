using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public enum ItemType {APPLE, SPEED, MAGNET};
    [System.Serializable]
    public class ItemData
    {
        public ItemType itemType;
        public GameObject prefab; // 아이템의 프리팹
        public int poolSize; // 각 아이템에 대한 풀 크기
    }

    [Header("아이템 데이터")]
    public List<ItemData> items = new List<ItemData>(); // 여러 아이템을 리스트로 관리

    [Header("먹이 아이템")]
    public int minAppleCount = 300; // 사과가 이 수 이하로 줄어들면 새로 생성
    public int appleToRespawn = 50; // 부족할 때마다 새로 생성할 사과 개수

    [Header("생성된 아이템 관리")]
    [SerializeField]
    private List<GameObject> spawnedItems = new List<GameObject>(); // 생성된 아이템들을 통합 관리할 리스트
    public List<GameObject> rangeObject = new List<GameObject>(); // 아이템을 생성할 여러 개의 Floor 오브젝트들

    private void Awake()
    {
        Instance = this;

        // "Floor" 태그를 가진 모든 오브젝트를 찾아 rangeObject 리스트에 추가
        GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
        rangeObject.AddRange(floors);

        // 리스트에서 null 값을 제거하여 유효한 오브젝트들만 남김
        rangeObject.RemoveAll(item => item == null);

        // 풀 생성
        foreach (var itemData in items)
        {
            PoolManager.Instance.CreatePool(itemData.prefab, itemData.poolSize);
        }
    }

    private void Start()
    {
        // 리스트가 비어있지 않은지 확인
        if (rangeObject.Count == 0)
        {
            Debug.LogError("rangeObject 리스트가 비어있습니다. Floor 오브젝트들이 추가되지 않았습니다.");
            return;
        }

        // 초기 아이템 생성 (사과와 버프 아이템)
        SpawnItemsByType(ItemType.APPLE, minAppleCount);
        SpawnItemsByType(ItemType.SPEED, 15);
        SpawnItemsByType(ItemType.MAGNET, 15);

        // 사과 수를 지속적으로 확인하고 생성하는 코루틴 시작
        StartCoroutine(CheckAndRespawnApples());
    }

    // 아이템 타입에 따라 아이템 스폰
    private void SpawnItemsByType(ItemType type, int count)
    {
        // "Spawn Area"를 찾아 스폰 위치를 관리
        GameObject spawnArea = GameObject.Find("Spawn Area");
        // "Spawn {type}"라는 이름의 자식 오브젝트를 찾음 (예: Spawn apple, Spawn speed 등)
        Transform spawnTransform = spawnArea?.transform.Find($"Spawn {type.ToString().ToLower()}");

        if (spawnArea == null || spawnTransform == null)
            return;

        // 아이템 리스트에서 주어진 타입의 아이템 데이터를 찾음
        ItemData itemData = items.Find(item => item.itemType == type);

        if (itemData == null)
            return;

        for (int i = 0; i < count; i++)
        {
            // 해당 타입의 프리팹에 맞는 랜덤 위치를 반환
            Vector3 spawnPosition = ReturnRandomPosition(itemData.prefab);

            // 유효한 스폰 위치(Vector3.zero이 아닌 위치)를 받은 경우에만 아이템을 생성
            if (spawnPosition != Vector3.zero)
            {
                // 풀에서 아이템을 가져와서 사용
                GameObject item = PoolManager.Instance.GetFromPool(itemData.prefab, spawnPosition, Quaternion.identity, spawnTransform);

                // 스폰 트랜스폼의 자식으로 설정 (Spawn Apple 등의 하위 오브젝트로 생성)
                item.transform.SetParent(spawnTransform, false); // 스폰 시에만 부모 설정

                // 생성된 아이템을 리스트에 추가하여 관리
                spawnedItems.Add(item);
            }
        }
    }


#region 사과를 특정 개수만큼 생성하는 함수
    private void SpawnApples(int count)
    {
        // GameObject spawnArea = GameObject.Find("Spawn Area");
        // Transform spawnAppleTransform = spawnArea.transform.Find("Spawn Apple");

        // if (spawnArea == null && spawnAppleTransform == null)
        //     return;

        // for (int i = 0; i < count; i++)
        // {
        //     // 랜덤한 위치를 얻음
        //     Vector3 spawnPosition = ReturnRandomPosition(spawnAppleTransform);
        //     // 유효한 위치일 경우에만 사과 생성
        //     if (spawnPosition != Vector3.zero) 
        //     {
        //         // Instantiate 오브젝트를 부모(spawnAppleTransform)를 설정하여 생성
        //         GameObject apple = Instantiate(applePrefab, spawnPosition, Quaternion.identity, spawnAppleTransform);
        //         spawnedApples.Add(apple);
        //     }
        // }
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
#endregion

    // 사과의 수를 확인하고 부족하면 다시 스폰
    IEnumerator CheckAndRespawnApples()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            spawnedItems.RemoveAll(item => item == null); // null 값 제거

            int appleCount = spawnedItems.FindAll(item => item.name.Contains("Apple")).Count;

            if (appleCount < minAppleCount)
            {
                Debug.Log($"사과가 부족합니다. {appleToRespawn}개 추가 생성.");
                SpawnItemsByType(ItemType.APPLE, appleToRespawn);
            }
        }
    }

    // 랜덤한 위치 반환
    private Vector3 ReturnRandomPosition(GameObject prefab)
    {
        if (rangeObject.Count == 0)
            return Vector3.zero;

        GameObject randomRangeObject = rangeObject[Random.Range(0, rangeObject.Count)];
        Vector3 originPosition = randomRangeObject.transform.position;
        Vector3 scale = randomRangeObject.transform.localScale;

        float randomX = Random.Range(-scale.x / 2, scale.x / 2);
        float randomZ = Random.Range(-scale.z / 2, scale.z / 2);
        Vector3 randomPosition = new Vector3(randomX, 0.5f, randomZ);

        return originPosition + randomPosition;
    }

#region Scale, Collider 사용한 랜덤생성
//     private Vector3 ReturnRandomPosition(GameObject prefab, Transform spawnTransform)
//     {
//         if (rangeObject.Count == 0)
//             return Vector3.zero; // 기본값 반환

//         // 무작위로 rangeObject에서 오브젝트 선택
//         GameObject randomRangeObject = rangeObject[Random.Range(0, rangeObject.Count)];

// #region Pos, Scale을 사용한 랜덤 생성
//         // 오브젝트의 Transform을 기준으로 범위 계산 (localScale을 사용)
//         Vector3 originPosition = randomRangeObject.transform.position;
//         Vector3 scale = randomRangeObject.transform.localScale;

//         // X, Z 축 범위 내에서 랜덤 좌표 생성
//         float randomX = Random.Range(-scale.x / 2, scale.x / 2);
//         float randomZ = Random.Range(-scale.z / 2, scale.z / 2);
        
//         // Y축은 0으로 고정하고 X, Z 축에만 변화를 줌
//         Vector3 randomPosition = new Vector3(randomX, 0.5f, randomZ);

//         // 최종 생성 위치 계산
//         Vector3 respawnPosition = originPosition + randomPosition;

//         return respawnPosition;
// #endregion

// #region BoxCollider를 사용한 랜덤생성
//         // Spawn Apple의 BoxCollider 가져오기
//         // BoxCollider rangeCollider = spawnTransform.GetComponent<BoxCollider>();

//         // if (rangeCollider == null)
//         // {
//         //     Debug.LogError($"'{spawnTransform.name}' 오브젝트에 BoxCollider가 없습니다.");
//         //     return spawnTransform.position; // Collider가 없어도 위치만 반환
//         // }

//         // Vector3 originPosition = spawnTransform.position;
//         // float range_X = rangeCollider.size.x * spawnTransform.localScale.x;
//         // float range_Z = rangeCollider.size.z * spawnTransform.localScale.z;

//         // // 랜덤 좌표 계산
//         // float randomX = Random.Range(-range_X / 2, range_X / 2);
//         // float randomZ = Random.Range(-range_Z / 2, range_Z / 2);
//         // Vector3 randomPosition = new Vector3(randomX, 0f, randomZ);

//         // Vector3 respawnPosition = originPosition + randomPosition;

//         // return respawnPosition;
// #endregion  
//     }
#endregion
}
