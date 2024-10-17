using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class FloatingHUDLevel : MonoBehaviour
{
    [SerializeField]
    private Transform followTarget; // 따라갈 타겟
    [SerializeField]
    private TMP_Text levelText; // 레벨 텍스트
    [SerializeField]
    private List<Color> levelColorList; // 레벨별 색상 리스트
    [SerializeField]
    private bool isFollowing = true; // 추적 여부
    [SerializeField]
    private float YOffset = 5f; // 오프셋 값

    private Camera mainCamera;

    private int currentLevel;

    void LateUpdate()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (followTarget != null && isFollowing)
        {
            // 월드 위치 계산
            Vector3 targetPosition = followTarget.position + new Vector3(0f, YOffset, 0f);
            transform.position = targetPosition;
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up); // 카메라를 바라보도록 설정 (UI가 카메라를 바라보게 함)
        }

        // if (followTarget != null && isFollowing)
        // {
        //     Vector3 worldPosition = followTarget.position + new Vector3(0f, YOffset, 0f);
        //     Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(mainCamera, worldPosition);

        //     // UI 위치 업데이트
        //     this.transform.position = new Vector3(screenPos.x, screenPos.y, 0);

        //     // 화면 경계를 넘어가지 않도록 조정
        //     this.transform.position = new Vector3(
        //         Mathf.Clamp(this.transform.position.x, 0, Screen.width),
        //         Mathf.Clamp(this.transform.position.y, 0, Screen.height),
        //         0
        //     );
        // }
    }

    // HUD 초기화
    public void Init(Transform target, bool isFollowing, bool isPlayer, int initialLevel)
    {
        this.followTarget = target;
        this.isFollowing = isFollowing;
        levelText.color = isPlayer ? levelColorList[0] : levelColorList[1]; // 플레이어와 적의 색상 구분
        SetLevel(initialLevel); // 초기 레벨 설정
    }

    // 레벨 설정
    public void SetLevel(int level)
    {
        currentLevel = level;
        levelText.text = $"Lv. {level}";
    }

    // 레벨 업
    public void LevelUp()
    {
        currentLevel++;
        levelText.text = $"Lv. {currentLevel}";
    }
}
