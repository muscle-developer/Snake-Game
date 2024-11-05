// UIViewMain.cs
using UnityEngine;
using TMPro;

public class UIViewMain : MonoBehaviour
{
    // 남은 게임 시간 텍스트
    [SerializeField]
    private TMP_Text timeText;

    void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        float gameTime = GameManager.Instance.gameTime;
        int min = Mathf.FloorToInt(gameTime / 60);
        int sec = Mathf.FloorToInt(gameTime % 60);
        timeText.text = string.Format("{0:D2}:{1:D2}", min, sec);
    }
}
