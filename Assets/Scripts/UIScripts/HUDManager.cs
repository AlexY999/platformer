using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private SceneChanger sceneChanger;

    public void ChangeScore(int score)
    {
        scoreText.text = score.ToString();
    }
    
    public void OnMenuButtonClick()
    {
        sceneChanger.gameObject.SetActive(true);
        sceneChanger.StartSceneChange(gameObject);
        Invoke(nameof(LoadMenuScene), 1.5f);
        AudioManager.Instance().OnButtonClickPlayAudioClip();
    }

    private void LoadMenuScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MenuScene");
        asyncLoad.completed += OnMenuSceneLoadCompleted;
    }

    private void OnMenuSceneLoadCompleted(AsyncOperation obj)
    {
        SceneChanger changer = GameObject.Find("LoadingWindow").GetComponent<SceneChanger>();
        changer.StartSceneChange();
    }
}
