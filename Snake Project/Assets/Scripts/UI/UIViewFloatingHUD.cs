using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class UIViewFloatingHUD : MonoBehaviour
{
    [SerializeField]
    private UIViewMain uiViewMain;

    [Header("레벨")]
    [SerializeField]
    private GameObject levelTextPrefab; // 레벨 텍스트 프리팹
    public Transform levelParent; // 레벨 HUD가 위치할 부모

    [Header("닉네임")]
    [SerializeField]
    private GameObject nickNameTextPrefab; // 닉네임 텍스트 프리팹
    [SerializeField]
    private Transform nickNameParent;   // 닉네임 HUD가 위치할 부모

    private List<FloatingHUDLevel> playerHUDs = new List<FloatingHUDLevel>(); // 플레이어 HUD 리스트
    private List<FloatingHUDLevel> enemyHUDs = new List<FloatingHUDLevel>(); // 적 HUD 리스트
    private Dictionary<Transform, FloatingHUDNickName> nickNameHUDs = new Dictionary<Transform, FloatingHUDNickName>(); // 닉네임 HUD 관리

    [SerializeField]
    private bool HUDFollowing;

    public void Initialize(Transform snakeTransform, int initialLevel, bool isPlayer = false)
    {
        // HUD 인스턴스 생성
        GameObject newHUD = Instantiate(levelTextPrefab, levelParent);
        newHUD.name = snakeTransform.name; // 스네이크 이름 설정

        // FloatingHUDLevel 컴포넌트 가져오기
        FloatingHUDLevel floatingHUDLevel = newHUD.GetComponent<FloatingHUDLevel>();

        if (floatingHUDLevel != null)
        {
            HUDFollowing = true;
            // HUD 초기화
            floatingHUDLevel.Init(snakeTransform, isPlayer, initialLevel, HUDFollowing);

            if (isPlayer)
            {
                playerHUDs.Add(floatingHUDLevel); // 플레이어 HUD 리스트에 추가
            }
            else
            {
                enemyHUDs.Add(floatingHUDLevel); // 적 HUD 리스트에 추가
            }
        }

        // 닉네임 설정 및 닉네임 HUD 인스턴스 생성
        string nickname = isPlayer ? PlayerPrefs.GetString("Nickname", "Player") : snakeTransform.name;
        GameObject nickNameHUD = Instantiate(nickNameTextPrefab, nickNameParent);
        FloatingHUDNickName floatingHUDNickName = nickNameHUD.GetComponent<FloatingHUDNickName>();

        if (floatingHUDNickName != null)
        {
            HUDFollowing = true;
            floatingHUDNickName.Init(snakeTransform, isPlayer, HUDFollowing, nickname); // 닉네임 설정
            nickNameHUDs[snakeTransform] = floatingHUDNickName; // 닉네임 HUD 딕셔너리에 추가
        }
    }

    public void RemoveSnakeHUD(Transform enemyTransform)
    {
        if (enemyTransform == null)
        {
            Debug.LogWarning("enemyTransform is already destroyed.");
            return;
        }

        FloatingHUDLevel hudToRemove = enemyHUDs.Find(hud => hud.name == enemyTransform.name);
        if (hudToRemove != null)
        {
            hudToRemove.DestroyHUD();
            enemyHUDs.Remove(hudToRemove);
        }

        // 닉네임 HUD 제거
        if (nickNameHUDs.TryGetValue(enemyTransform, out FloatingHUDNickName nickNameHUD))
        {
            nickNameHUD.DestroyHUD();
            nickNameHUDs.Remove(enemyTransform);
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
                uiViewMain.UpdatePlayerScore();
            }
        }
    }

    // 특정 적의 레벨을 올리는 메서드 (인덱스 사용)
    public void EnemyLevelUp(int index)
    {
        if (index >= 0 && index < enemyHUDs.Count)
        {
            enemyHUDs[index].LevelUp(); // 해당 인덱스의 적 레벨업 호출
            uiViewMain.UpdateEnemyScores();
        }
    }
}
