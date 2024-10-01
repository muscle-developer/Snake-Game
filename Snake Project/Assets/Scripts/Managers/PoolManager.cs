using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

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
                poolDictionary[prefabName].Enqueue(itemObj);
            }
        }
    }

    // 풀에서 오브젝트 가져오기
    public GameObject GetFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        string prefabName = prefab.name;
        if (poolDictionary.ContainsKey(prefabName) && poolDictionary[prefabName].Count > 0)
        {
            GameObject itemObj = poolDictionary[prefabName].Dequeue();
            itemObj.SetActive(true);
            itemObj.transform.position = position;
            itemObj.transform.rotation = rotation;
            return itemObj;
        }
        else
        {
            // 풀에 더 이상 오브젝트가 없으면 새로 생성
            GameObject itemObj = Instantiate(prefab, position, rotation);
            return itemObj;
        }
    }

    // 오브젝트를 풀로 반환하기
    public void ReturnToPool(GameObject itemObj)
    {
        itemObj.SetActive(false);
        string prefabName = itemObj.name.Replace("(Clone)", "").Trim();
        if (poolDictionary.ContainsKey(prefabName))
        {
            poolDictionary[prefabName].Enqueue(itemObj);
        }
        else
        {
            Destroy(itemObj);
        }
    }
}
