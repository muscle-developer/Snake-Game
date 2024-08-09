using UnityEngine;

public class UICanvasControllerInput : MonoBehaviour
{
    [Header("Output")]
    public SnakeHead inputs = null; // SnakeHead 컴포넌트의 참조 변수

    void Start()
    {
        if (inputs == null)
            inputs = FindObjectOfType<SnakeHead>();
    }

    // 가상 조이스틱의 입력을 처리하는 메서드입니다.
    // virtualMoveDirection은 조이스틱의 이동 방향을 나타내며, 
    // virtualIsLookMoveState는 조이스틱의 상태를 나타냅니다.
    public void VirtualMoveInput(Vector2 virtualMoveDirection, bool virtualIsLookMoveState)
    {
        if(inputs == null)
            return;

        // Vector2 형태의 조이스틱 입력 방향을 Vector3로 변환합니다.
        Vector3 moveDirection = new Vector3(virtualMoveDirection.x, 0, virtualMoveDirection.y);

        // SnakeHead의 MoveInput 메서드를 호출하여 새로운 이동 방향을 전달합니다.
        inputs.MoveInput(moveDirection);

        // SnakeHead의 IsMoveInputExist 메서드를 호출하여 입력 상태를 전달합니다.
        inputs.IsMoveInputExist(virtualIsLookMoveState);
    }
}
