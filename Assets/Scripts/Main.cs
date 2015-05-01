using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    public void NewGame()
    {
        Debug.Log("click");
        Application.LoadLevelAsync("Game");
    }
}
