using UnityEngine;
using System.Collections.Generic;

public class UIViewFloatingHUD : MonoBehaviour
{
    [SerializeField]
    private GameObject levelTextPrefab; // 레벨 텍스트 프리팹
    public Transform levelParent; // HUD가 위치할 부모

    private List<FloatingHUDLevel> playerHUDs = new List<FloatingHUDLevel>(); // 플레이어 HUD 리스트
    private List<FloatingHUDLevel> enemyHUDs = new List<FloatingHUDLevel>(); // 적 HUD 리스트

    public void Initialize(Transform snakeTransform, int initialLevel, bool isPlayer = false)
    {
        // HUD 인스턴스 생성
        GameObject newHUD = Instantiate(levelTextPrefab, levelParent);

        // FloatingHUDLevel 컴포넌트 가져오기
        FloatingHUDLevel floatingHUD = newHUD.GetComponent<FloatingHUDLevel>();

        if (floatingHUD != null)
        {
            // HUD 초기화
            floatingHUD.Init(snakeTransform, true, isPlayer, initialLevel);

            if (isPlayer)
            {
                playerHUDs.Add(floatingHUD); // 플레이어 HUD 리스트에 추가
            }
            else
            {
                enemyHUDs.Add(floatingHUD); // 적 HUD 리스트에 추가
            }
        }
    }

    // 플레이어 HUD를 레벨업하는 메서드
    public void PlayerLevelUp()
    {
        foreach (var playerHUD in playerHUDs)
        {
            if (playerHUD != null)
            {
                playerHUD.LevelUp(); // 플레이어 레벨업 호출
            }
        }
    }

    // 특정 적의 레벨을 올리는 메서드 (인덱스 사용)
    public void EnemyLevelUp(int index)
    {
        if (index >= 0 && index < enemyHUDs.Count)
        {
            enemyHUDs[index].LevelUp(); // 해당 인덱스의 적 레벨업 호출
        }
    }
}
