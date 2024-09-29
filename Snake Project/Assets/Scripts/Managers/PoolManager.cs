using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;


    void Awake()
    {
        Instance = this;
    }
}
