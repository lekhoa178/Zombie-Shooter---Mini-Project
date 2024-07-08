using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeController : MonoBehaviour
{
    public void Play()
    {
        GameManager.Instance.LoadScene("Main");
    }

    public void Quit()
    {
        GameManager.Instance.CloseGame();
    }
}
