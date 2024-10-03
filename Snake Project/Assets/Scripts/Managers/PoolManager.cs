using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    public Transform objectPoolParent; // 풀링된 오브젝트들의 부모를 관리할 빈 Transform

    void Awake()
    {
        Instance = this;
    }

    // 특정 프리팹에 대한 풀 생성
    public void CreatePool(GameObject prefab, int poolSize)
    {
        string prefabName = prefab.name;
        if (!poolDictionary.ContainsKey(prefabName))
        {
            poolDictionary[prefabName] = new Queue<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject itemObj = Instantiate(prefab);
                itemObj.SetActive(false);
                itemObj.transform.SetParent(objectPoolParent, false); // 부모 설정
                poolDictionary[prefabName].Enqueue(itemObj);
            }
        }
    }

    // 풀에서 오브젝트 가져오기
    public GameObject GetFromPool(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        string prefabName = prefab.name;
        GameObject itemObj;

        if (poolDictionary.ContainsKey(prefabName) && poolDictionary[prefabName].Count > 0)
        {
            itemObj = poolDictionary[prefabName].Dequeue();
        }
        else
        {
            // 풀에 더 이상 오브젝트가 없으면 새로 생성
            itemObj = Instantiate(prefab, position, rotation);
        }

        itemObj.SetActive(true);
        itemObj.transform.position = position;
        itemObj.transform.rotation = rotation;
        if (parent != null)
        {
            itemObj.transform.SetParent(parent, false);
        }
        return itemObj;
    }

    // 오브젝트를 풀로 반환하기
    public void ReturnToPool(GameObject itemObj)
    {
        itemObj.SetActive(false);
        string prefabName = itemObj.name.Replace("(Clone)", "").Trim();

        if (poolDictionary.ContainsKey(prefabName))
        {
            // 'Spawn Area'라는 오브젝트를 찾아 반환된 아이템을 그 하위에 두도록 설정
            GameObject spawnArea = GameObject.Find("Spawn Area");

            if (spawnArea != null)
            {
                Transform spawnTransform = spawnArea.transform.Find($"Spawn {prefabName.ToLower()}");

                if (spawnTransform != null)
                {
                    // 반환할 때 부모를 해당 스폰 트랜스폼으로 설정
                    itemObj.transform.SetParent(spawnTransform, false);
                }
            }

            // 풀로 다시 반환
            poolDictionary[prefabName].Enqueue(itemObj);
        }
        else
        {
            Destroy(itemObj);
        }
    }
}
