using UnityEngine;
using TMPro;

public class UIViewFloatingHUD : MonoBehaviour
{
    [SerializeField]
    private TMP_Text levelText; // 텍스트 컴포넌트
    private int level = 0; // 초기 레벨 설정
    private int playerLevel = 0; // 플레이어 레벨
    private Camera mainCamera;
    private Transform target; // 스네이크의 Transform
	public Transform levelParent;

    // 스네이크의 Transform을 받아오도록 설정
    public void Initialize(Transform snakeTransform, int initialLevel, bool isPlayer = false)
    {
        target = snakeTransform; // 스네이크의 위치를 추적
        if (isPlayer)
        {
            playerLevel = initialLevel; // 플레이어 레벨 설정
            UpdateLevelText(playerLevel, true); // 플레이어 레벨 업데이트
        }
        else
        {
            level = initialLevel; // 적 스네이크 초기 레벨 설정
            UpdateLevelText(level, false); // 적 레벨 업데이트
        }
        mainCamera = Camera.main; // 메인 카메라를 찾음
    }

    void Update()
    {
        if (target != null)
        {
            FollowSnake(); // 스네이크 머리 위에 텍스트 위치
            LookAtCamera(); // 텍스트가 항상 카메라를 바라보도록 설정
        }
    }

    // 레벨을 설정하는 메서드 (플레이어 레벨)
    public void SetPlayerLevel(int newLevel)
    {
        playerLevel = newLevel;
        UpdateLevelText(playerLevel, true);
    }

    // 레벨업 메서드 (플레이어 레벨)
    public void PlayerLevelUp()
    {
        playerLevel++;
        UpdateLevelText(playerLevel, true);
    }

    // 레벨을 설정하는 메서드 (적 레벨)
    public void SetEnemyLevel(int newLevel)
    {
        level = newLevel;
        UpdateLevelText(level, false);
    }

    // 레벨업 메서드 (적 레벨)
    public void EnemyLevelUp()
    {
        level++;
        UpdateLevelText(level, false);
    }

    // 레벨 텍스트를 업데이트하는 메서드
    private void UpdateLevelText(int currentLevel, bool isPlayer)
    {
        levelText.text = (isPlayer ? "Player Lv. " : "Enemy Lv. ") + currentLevel;
    }

    // 스네이크를 따라가도록 위치 설정
    private void FollowSnake()
    {
        Vector3 offset = new Vector3(0, 1.5f, 0); // 머리 위에 표시될 오프셋
        transform.position = target.position + offset; // 스네이크 위치 + 오프셋
    }

    // 텍스트가 항상 카메라를 바라보도록 설정
    private void LookAtCamera()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                         mainCamera.transform.rotation * Vector3.up);
    }
}
