using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform player;

    public void Awake()
    {
        GameManager.Instance = this;
    }

    void Start()
    {   
        StartCoroutine(FindPlayerCoroutine());
    }
    
    private IEnumerator FindPlayerCoroutine()
    {
        while (player == null)
        {
            player = FindObjectOfType<SnakeHead>()?.transform; // ?.는 널 조건부 연산자로, FindObjectOfType<SnakeHead>()가 null인 경우 안전하게 target을 null로 설정합니다.
            yield return null;
        }
    }
}
