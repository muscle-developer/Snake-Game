using UnityEngine;

public class UICanvasControllerInput : MonoBehaviour
{
    [Header("Output")]
    public SnakeHead inputs = null;

    public void VirtualMoveInput(Vector2 virtualMoveDirection, bool virtualIsLookMoveState)
    {
        if(inputs == null)
            return;
        inputs.MoveInput(virtualMoveDirection);
        inputs.IsMoveInputExist(virtualIsLookMoveState);
    }
}
