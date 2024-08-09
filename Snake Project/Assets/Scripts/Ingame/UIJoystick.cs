using UnityEngine;
using UnityEngine.UI;

public class UIJoystick : MonoBehaviour
{
    [SerializeField]
    private UICanvasControllerInput uICanvasControllerInput; // UI와 게임 로직 간의 연결
    [SerializeField]
    private VirtualJoystick moveJoyStick; // 조이스틱의 UI 요소를 담당하는 VirtualJoystick 인스턴스
    private bool isInteractable = true; // 조이스틱이 상호작용 가능한지 여부를 나타내는 변수

    void Awake()
    {
        // UIJoystick이 활성화될 때 조이스틱의 상호작용 가능 여부를 설정
        moveJoyStick.enabled = isInteractable;
    }

    public void Initialize(SnakeHead snakeHead)
    {
        if (snakeHead != null)
        {
            uICanvasControllerInput.inputs = snakeHead; // UICanvasControllerInput의 inputs 속성을 주어진 SnakeHead로 설정
            SetInteractableAlpha(this.transform, isInteractable); // 조이스틱 UI의 투명도를 설정
        }
        else
        {
            // SnakeHead 인스턴스가 설정되지 않았을 때 에러 메시지를 출력
            Debug.LogError("SnakeHead 인스턴스를 설정하는 데 실패했습니다.");
        }
    }

    public void SetInteractableAlpha(Transform transform, bool isInteractable)
    {
        // 주어진 Transform 아래의 모든 Image 컴포넌트의 투명도를 설정하는 메서드
        foreach (Image item in transform.GetComponentsInChildren<Image>(true))
        {
            if (item.sprite == null)
                continue; // Sprite가 없는 경우에는 변경하지 않음

            // 이미지의 색상을 새롭게 설정
            var newColor = item.color;
            newColor.a = isInteractable ? 1f : 0.2f; // 상호작용 가능 여부에 따라 투명도를 변경
            item.color = newColor;
        }
    }
}
