using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResultController : MonoBehaviour
{
    public static GameResultController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameResultController>();
            return instance;
        }
    }
    private static GameResultController instance;

    [field: SerializeField] private Page winPage;
    [field: SerializeField] private Page losePage;

    public void FinishGame(bool win)
    {
        if (win)
            UIManager.Instance.PushPage(winPage);
        else
            UIManager.Instance.PushPage(losePage);
    }

    public void PlayAgain()
    {
        LevelManager.Instance.ResetState();

        GameManager.Instance.ReloadScene();
    }

    public void Quit()
    {
        GameManager.Instance.LoadScene("Home");
    }
}
