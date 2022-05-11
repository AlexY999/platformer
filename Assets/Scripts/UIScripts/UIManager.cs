using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UIElements.Toggle;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<RectTransform> Pages;
    [SerializeField] private SceneChanger sceneChanger;
    [SerializeField] private int CurrentPage = 0;
    [SerializeField] private float PageTransitionDuration = 1f;
    [SerializeField] private AnimationCurve TransitionCurve;

    private Vector3 _position;
    private Coroutine _transitionCoroutine;

    private static UIManager instance;

    public static UIManager Instance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Initialization();
    }
    
    private void Initialization()
    {
        OffAllPages();
        Pages[0].gameObject.SetActive(true);
    }

    public void OnButtonClisk()
    {
        AudioManager.Instance().OnButtonClickPlayAudioClip();
    }

    public void PreviousPage()
    {
        if (CurrentPage > 0)
        {
            CurrentPage--;
            Transition(CurrentPage + 1, CurrentPage, false);
        }
    }

    public void NextPage()
    {
        if (CurrentPage < Pages.Count - 1)
        {
            CurrentPage++;
            Transition(CurrentPage - 1, CurrentPage, true);
        }
    }

    public void GoToGameScene()
    {
        sceneChanger.gameObject.SetActive(true);
        sceneChanger.StartSceneChange(Pages[0].gameObject);
    }

    private void LoadGameScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");
        asyncLoad.completed += OnMenuSceneLoadCompleted;
    }

    private void OnMenuSceneLoadCompleted(AsyncOperation obj)
    {
        SceneChanger sceneChanger = GameObject.Find("LoadingWindow").GetComponent<SceneChanger>();
        sceneChanger.StartSceneChange();
    }

    private void Transition(int previous, int next, bool goingRight)
    {
        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
        }

        _transitionCoroutine = StartCoroutine(TransitionCoroutine(previous, next, goingRight));
    }

    private IEnumerator TransitionCoroutine(int previous, int next, bool goingRight)
    {
        float screenWidth = gameObject.GetComponent<RectTransform>().rect.width;

        _position.y = Pages[previous].localPosition.y;
        _position.z = Pages[previous].localPosition.z;

        Pages[next].gameObject.SetActive(true);
        UnBlockAllButtons(false);
        
        float timeSpent = 0f;
        while (timeSpent < PageTransitionDuration)
        {
            if (goingRight)
            {
                _position.x = Mathf.Lerp(0f, -screenWidth, TransitionCurve.Evaluate(timeSpent/PageTransitionDuration));
                Pages[previous].localPosition = _position;
                _position.x = Mathf.Lerp(screenWidth, 0f, TransitionCurve.Evaluate(timeSpent/PageTransitionDuration));
                Pages[next].localPosition = _position;
            }
            else
            {
                _position.x = Mathf.Lerp(0f, screenWidth, TransitionCurve.Evaluate(timeSpent/PageTransitionDuration));
                Pages[previous].localPosition = _position;
                _position.x = Mathf.Lerp(-screenWidth, 0f, TransitionCurve.Evaluate(timeSpent/PageTransitionDuration));
                Pages[next].localPosition = _position;
            }

            timeSpent += Time.deltaTime;
            yield return null;
        }

        UnBlockAllButtons(true);
        Pages[previous].gameObject.SetActive(false);
    }

    private void UnBlockAllButtons(bool pointer)
    {
        List<Button> buttonsList = new List<Button>();

        foreach (var page in Pages)
        {
            if (page.gameObject.activeSelf)
            {
                Button[] buttons = page.GetComponentsInChildren<Button>();
                buttonsList.AddRange(buttons);
            }
        }

        foreach (var button in buttonsList)
        {
            button.enabled = pointer;
        }
    }

    private void OffAllPages()
    {
        foreach (var page in Pages)
        {
            page.gameObject.SetActive(false);
        }
    }
}