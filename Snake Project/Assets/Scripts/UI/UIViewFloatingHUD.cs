using UnityEngine;

public class UIViewFloatingHUD : MonoBehaviour
{
    [SerializeField]
    private GameObject levelTextPrefab; // 레벨 텍스트 프리팹
    public Transform levelParent; // HUD가 위치할 부모

    private FloatingHUDLevel playerHUD;
    private FloatingHUDLevel enemyHUD;

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
                playerHUD = floatingHUD; // 플레이어 HUD 저장
            }
            else
            {
                enemyHUD = floatingHUD; // 적 HUD 저장
            }
        }
    }

    // 플레이어 레벨업 메서드
    public void PlayerLevelUp()
    {
        if (playerHUD != null)
        {
            playerHUD.LevelUp(); // 플레이어 레벨업 호출
        }
    }

    // 적 레벨업 메서드
    public void EnemyLevelUp()
    {
        if (enemyHUD != null)
        {
            enemyHUD.LevelUp(); // 적 레벨업 호출
        }
    }
}
