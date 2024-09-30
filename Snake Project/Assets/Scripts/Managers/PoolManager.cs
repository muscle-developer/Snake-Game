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

    public void CreatePool()
    {

    }

    public GameObject GetFromPool()
    {
        return gameObject;
    }

    // 오브젝트를 풀로 반환하기
    public void ReturnToPool()
    {

    }
}
