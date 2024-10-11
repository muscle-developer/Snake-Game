using UnityEngine;
using TMPro;

public class SnakeCanvas : MonoBehaviour
{   
    [SerializeField]
    private TMP_Text levelText; // 텍스트 컴포넌트
    private int level = 0; // 초기 레벨 설정
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // 메인 카메라를 찾음
        UpdateLevelText(); // 초기 텍스트 설정
    }

    void Update()
    {
        LookAtCamera(); // 텍스트가 항상 카메라를 바라보도록 함
    }
    
    // 적 스네이크의 레벨을 받아오기 위해(적은 n개의 몸통을 가지고 시작하기때문)
    public void SetLevel(int initialLevel)
    {
        level = initialLevel;
        UpdateLevelText();
    }

    public void LevelUp()
    {
        level++; // 레벨 증가
        UpdateLevelText();
    }

    private void UpdateLevelText()
    {
        levelText.text = "Lv. " + level; // 텍스트 업데이트
    }

    private void LookAtCamera()
    {
        // 텍스트가 항상 카메라를 바라보도록 회전
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                         mainCamera.transform.rotation * Vector3.up);
    }
}
