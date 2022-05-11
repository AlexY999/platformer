using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        Invoke("LoadGameScene", 1.5f);
        AudioManager.Instance().OnButtonClickPlayAudioClip();
    }

    private void LoadGameScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MenuScene");
        asyncLoad.completed += OnMenuSceneLoadCompleted;
    }

    private void OnMenuSceneLoadCompleted(AsyncOperation obj)
    {
        SceneChanger sceneChanger = GameObject.Find("LoadingWindow").GetComponent<SceneChanger>();
        sceneChanger.StartSceneChange();
    }
}
