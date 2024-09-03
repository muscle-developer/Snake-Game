using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // 여러 개의 Floor 오브젝트들
    public List<GameObject> rangeObject = new List<GameObject>();
    public GameObject capsul;
    // public enum ItemName {APPLE, SPEED_ITEM, BUFF_ITEM}

    private void Awake()
    {
        // "Floor" 태그가 붙은 모든 오브젝트를 찾아 리스트에 추가
        GameObject[] spawnArea = GameObject.FindGameObjectsWithTag("Floor");
        rangeObject.AddRange(spawnArea);

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

        StartCoroutine(RandomRespawn_Coroutine());
    }

    IEnumerator RandomRespawn_Coroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            // 생성 위치 부분에 위에서 만든 함수 Return_RandomPosition() 함수 대입
            GameObject instantCapsul = Instantiate(capsul, Return_RandomPosition(), Quaternion.identity);
        }
    }
    
    Vector3 Return_RandomPosition()
    {
        if (rangeObject.Count == 0)
        {
            Debug.LogError("rangeObject 리스트가 비어있습니다. Floor 오브젝트들이 추가되지 않았습니다.");
            return Vector3.zero; // 기본값 반환
        }

        // rangeObject 리스트에서 무작위로 하나의 GameObject 선택
        GameObject randomRangeObject = rangeObject[Random.Range(0, rangeObject.Count)];

        // 선택된 GameObject가 null인지 확인
        if (randomRangeObject == null)
        {
            Debug.LogError("rangeObject 리스트에 null 값이 있습니다.");
            return Vector3.zero; // 기본값 반환
        }

        // 선택된 GameObject의 Collider 정보 가져오기
        BoxCollider rangeCollider = randomRangeObject.GetComponent<BoxCollider>();
        if (rangeCollider == null)
        {
            Debug.LogError($"선택된 오브젝트 '{randomRangeObject.name}'에 BoxCollider가 없습니다.");
            return Vector3.zero; // 기본값 반환
        }

        // 선택된 Collider의 중심 위치와 크기를 기준으로 랜덤한 위치 계산
        Vector3 originPosition = rangeCollider.transform.position;
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        // Collider의 크기 내에서 무작위 위치 계산
        float randomX = Random.Range(-range_X / 2, range_X / 2);
        float randomZ = Random.Range(-range_Z / 2, range_Z / 2);
        Vector3 randomPosition = new Vector3(randomX, 0f, randomZ);

        // Collider의 중심 위치에 무작위 위치를 더하여 최종 위치 계산
        Vector3 respawnPosition = originPosition + randomPosition;
        return respawnPosition;
    }
}
