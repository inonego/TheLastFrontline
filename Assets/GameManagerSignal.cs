using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSignal : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void GoToTitleMenu()
    {
        GameManager.Instance.GoToTitleMenu();
    }
}
