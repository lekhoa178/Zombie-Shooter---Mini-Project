using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Page LoadingPage;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnLoading()
    {
        UIManager.Instance.PopAllPages();
        UIManager.Instance.PushPage(LoadingPage);
    }

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        StartCoroutine(LoadSceneAsync(GetCurrentSceneName()));
    }

    public void LoadScene(string name)
    {
        StartCoroutine(LoadSceneAsync(name));
    }

    private IEnumerator LoadSceneAsync(string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        operation.allowSceneActivation = false;

        OnLoading();

        float lerpValue = 0;

        do
        {
            lerpValue = Mathf.Lerp(lerpValue, operation.progress, 0.3f);
            yield return null;
        }
        while (!Mathf.Approximately(lerpValue, 0.9f));

        yield return new WaitForSeconds(1f);

        operation.allowSceneActivation = true;
    }

}
