using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class FloatingHUDLevel : MonoBehaviour
{
    [SerializeField]
    private Transform followTarget; // 따라갈 타겟
    [SerializeField]
    private TMP_Text levelText; // 레벨을 표시할 텍스트 컴포넌트
    [SerializeField]
    private List<Color> levelColorList; // 레벨별 색상 리스트 (플레이어와 적 구분)
    [SerializeField]
    private bool isFollowing = true; // HUD가 타겟을 따라가는지 여부
    [SerializeField]
    private float YOffset = 5f; // HUD의 Y축 오프셋, 캐릭터 위로 띄워지는 거리 조정

    private Camera mainCamera; // 메인 카메라 참조
    private int currentLevel; // 현재 레벨 저장

    void LateUpdate()
    {
        if (mainCamera == null)
            mainCamera = Camera.main; // 메인 카메라 참조가 없다면 찾기

        if (followTarget != null && isFollowing) // 타겟이 존재하고 추적이 활성화된 경우
        {
            // 타겟 위치 + 오프셋 값 적용
            Vector3 targetPosition = followTarget.position + new Vector3(0f, YOffset, 0f);
            transform.position = targetPosition;

            // HUD가 카메라를 바라보도록 설정
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }
    }

    // HUD 초기화
    public void Init(Transform target, bool isPlayer, int initialLevel, bool isFollowing)
    {
        this.followTarget = target; // 따라갈 타겟 설정
        this.isFollowing = isFollowing; // 추적 여부 설정
        levelText.color = isPlayer ? levelColorList[0] : levelColorList[1]; // 플레이어와 적의 색상 구분
        SetLevel(initialLevel); // 초기 레벨 설정
    }

    // 현재 레벨 설정
    public void SetLevel(int level)
    {
        currentLevel = level;
        levelText.text = $"Lv. {level}"; // 텍스트에 "Lv."와 함께 레벨 표시
    }

    // 레벨업 처리
    public void LevelUp()
    {
        currentLevel++; // 레벨 증가
        levelText.text = $"Lv. {currentLevel}"; // HUD 텍스트 업데이트
    }

    // 스네이크가 죽었을 때 레벨 텍스트 없애기
    public void DestroyHUD()
    {
        if (gameObject != null)
        {
            Destroy(gameObject); // 게임 오브젝트 삭제
        }
    }
}
