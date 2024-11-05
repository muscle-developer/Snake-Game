using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] 
    private Image fadeImage;
    [SerializeField]
    private TMP_InputField input_Nickname;
    [SerializeField]
    private Button btn_Start;

    private Coroutine co = null;
    public bool deleteData;
    private float fadeDuration = 1f;

    private void Awake()
    {
        if (deleteData)
            PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeOut());
        GameManager.Instance.isLive = true;
    }

    public void OnInputFieldValueChanged()
    {
        btn_Start.interactable = input_Nickname.text.Length > 0;
    }

    public void OnClickBtnStart()
    {
        if (co == null)
            co = StartCoroutine(SceneTrans("Main Game"));
    }

    private IEnumerator FadeOut()
    {
        Color color = fadeImage.color;
        float startAlpha = color.a;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, 0f, normalizedTime);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }

    private IEnumerator FadeIn()
    {
        Color color = fadeImage.color;
        float startAlpha = color.a;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, 1f, normalizedTime);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }

    private IEnumerator SceneTrans(string sceneName)
    {
        input_Nickname.interactable = false;
        PlayerPrefs.SetString("Nickname", input_Nickname.text);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        yield return StartCoroutine(FadeIn());
        async.allowSceneActivation = true;
        
        co = null;
    }
}
