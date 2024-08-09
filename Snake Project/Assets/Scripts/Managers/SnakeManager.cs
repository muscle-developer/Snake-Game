using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    // 스네이크 관리를 위한 매니저 스크립트
    // 역할: 게임의 전역적인 상태와 시스템을 관리. 주로 스네이크의 몸체를 추가하고, 몸체의 리스트를 관리하는 역할을 합니다.
    // 전역에서 사용할 수 있도록 싱글톤화
    public static SnakeManager Instance;
    // Settings
    public float snkaeSpeed = 5.0f;
    public float rotationSpeed = 180.0f;
    public float bodySpeed = 5.0f;
    public int gap = 10;

    // References
    public GameObject BodyPrefab;

    // Lists
    public List<GameObject> BodyParts = new List<GameObject>();
    public List<Vector3> PositionsHistory = new List<Vector3>();

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

        // 게임 시작 시 뱀의 머리 위치를 PositionsHistory에 추가
        if (BodyParts.Count > 0)
        {
            PositionsHistory.Add(BodyParts[0].transform.position);
        }
    }

    public void AddBodyPart() 
    {
        // PositionsHistory가 비어 있는 경우 현재 위치를 추가
        if (PositionsHistory.Count == 0)
            PositionsHistory.Add(transform.position);

        Vector3 newPosition = BodyParts.Count == 0 ? transform.position : PositionsHistory[Mathf.Clamp(BodyParts.Count * gap, 0, PositionsHistory.Count - 1)];

        GameObject body = Instantiate(BodyPrefab, newPosition, Quaternion.identity, BodyParts[0].transform);
        BodyParts.Add(body);
    }
}
