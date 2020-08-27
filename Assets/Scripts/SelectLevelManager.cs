using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevelManager : MonoBehaviour
{
    public void OpenMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OpenTopRankScene()
    {
        SceneManager.LoadScene("TopRank");
    }
    public void OpenSelectLevelScene()
    {
        SceneManager.LoadScene("SelectLevel");
    }

    public void OpenSelectBallScene()
    {
        SceneManager.LoadScene("SelectBall");
    }

    public void SelectAncientLevel()
    {
        SceneManager.LoadScene("AncientMap");
    }

    public void SelectStoneLevel()
    {
        SceneManager.LoadScene("StoneMap");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
