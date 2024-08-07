using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 개별 객체 제어: 특정 객체나 캐릭터의 행동과 동작을 제어합니다.
// 입력 처리: 플레이어의 입력을 받아 해당 객체가 어떻게 반응할지를 결정합니다.
// 객체 중심: 주로 개별 게임 객체에 붙어서 동작하며, 해당 객체의 움직임, 애니메이션, 상호작용 등을 관리합니다.
public class IngameController : MonoBehaviour
{
    public GameObject snakeHeadPrefab; // SnakeHead 프리팹

    void Start()
    {
        InstantiateSnakeHead();
    }

    private void InstantiateSnakeHead()
    {
        GameObject snakeHead = Instantiate(snakeHeadPrefab, transform.position, transform.rotation, transform);
        TestManager.Instance.snakeBody.Add(snakeHead);
    }
}
