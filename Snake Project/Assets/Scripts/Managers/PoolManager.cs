using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    // 풀링할 오브젝트들을 저장할 딕셔너리, 프리팹 이름을 키로 사용하고 해당 오브젝트들의 큐를 값으로 가짐
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    // 풀링된 오브젝트들의 부모가 될 빈 Transform (Hierarchy 관리)
    public Transform objectPoolParent;

    void Awake()
    {
        // PoolManager 인스턴스 초기화 (싱글톤 패턴)
        Instance = this;
    }

    // 특정 프리팹에 대한 오브젝트 풀 생성
    public void CreatePool(GameObject prefab, int poolSize)
    {
        // 프리팹의 이름을 키로 사용
        string prefabName = prefab.name;

        // 풀에 해당 프리팹이 없을 때만 풀 생성
        if (!poolDictionary.ContainsKey(prefabName))
        {
            // 새로운 큐 생성
            poolDictionary[prefabName] = new Queue<GameObject>();

            // 지정된 수만큼 오브젝트를 생성하여 풀에 추가
            for (int i = 0; i < poolSize; i++)
            {
                GameObject itemObj = Instantiate(prefab); // 프리팹 인스턴스화
                itemObj.SetActive(false); // 오브젝트 비활성화
                itemObj.transform.SetParent(objectPoolParent, false); // 부모 설정
                poolDictionary[prefabName].Enqueue(itemObj); // 큐에 오브젝트 추가
            }
        }
    }

    // 풀에서 오브젝트 가져오기 (풀 매니저 -> 오브젝트)
    public GameObject GetFromPool(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        // 프리팹 이름을 통해 딕셔너리에서 풀을 찾음
        string prefabName = prefab.name;
        GameObject itemObj;

        // 풀에 해당 프리팹의 오브젝트가 남아 있으면 가져옴
        if (poolDictionary.ContainsKey(prefabName) && poolDictionary[prefabName].Count > 0)
        {
            itemObj = poolDictionary[prefabName].Dequeue(); // 큐에서 오브젝트 제거
        }
        else
        {
            // 풀에 남은 오브젝트가 없으면 새로 생성
            itemObj = Instantiate(prefab, position, rotation);
        }

        // 오브젝트 설정
        itemObj.SetActive(true); // 오브젝트 활성화
        itemObj.transform.position = position; // 위치 설정
        itemObj.transform.rotation = rotation; // 회전 설정

        // 부모가 지정되었으면 부모 설정
        if (parent != null)
        {
            itemObj.transform.SetParent(parent, false);
        }
        return itemObj; // 오브젝트 반환
    }

    // 오브젝트를 풀로 반환하기 (오브젝트 -> 풀 매니저)
    public void ReturnToPool(GameObject itemObj)
    {
        // 오브젝트 비활성화
        itemObj.SetActive(false);

        // 프리팹 이름에서 "(Clone)"을 제거하여 원본 이름을 추출
        string prefabName = itemObj.name.Replace("(Clone)", "").Trim();

        // 해당 프리팹 이름에 해당하는 풀이 있으면
        if (poolDictionary.ContainsKey(prefabName))
        {
            // 'Spawn Area'라는 오브젝트를 찾아 반환할 오브젝트의 부모로 설정
            GameObject spawnArea = GameObject.Find("Spawn Area");

            if (spawnArea != null)
            {
                // 'Spawn {PrefabName}'라는 이름의 트랜스폼을 찾음
                Transform spawnTransform = spawnArea.transform.Find($"Spawn {prefabName.ToLower()}");

                // 해당 트랜스폼이 있으면 그 하위로 반환할 오브젝트를 설정
                if (spawnTransform != null)
                {
                    itemObj.transform.SetParent(spawnTransform, false);
                }
            }

            // 오브젝트를 큐에 다시 추가하여 풀에 반환
            poolDictionary[prefabName].Enqueue(itemObj);
        }
        else
        {
            // 풀에 해당 프리팹 이름이 없으면 오브젝트를 파괴
            Destroy(itemObj);
        }
    }
}
