using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public List<Player> player = new List<Player>();

    private void Awake()
    {
        int player1 = PlayerPrefs.GetInt("Player1"); //黑棋
        int player2 = PlayerPrefs.GetInt("Player2"); //白棋
        PlayerPrefs.SetInt("Mode", 1);
        for (int i = 0; i < player.Count; i++)
        {
            if (i == player1)
            {
                player[i].chessColor = ChessType.Black;
            }
            else if (i == player2)
            {
                player[i].chessColor = ChessType.White;
            }
            else
            {
                player[i].chessColor = ChessType.Watch;
            }
        }
    }

    public void SetPlayer1(int i)
    {
        PlayerPrefs.SetInt("Player1", i);
    }

    public void SetPlayer2(int i)
    {
        PlayerPrefs.SetInt("Player2", i);
    }

    //开始游戏
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    //开始联网游戏（局域网）
    public void PlarNetGame()
    {
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// 更换先手
    /// </summary>
    public void ChangeChessColor()
    {
        for (int i = 0; i < player.Count; i++)
        {
            if (player[i].chessColor == ChessType.Black)
            {
                SetPlayer2(i);
            }
            else if (player[i].chessColor == ChessType.White)
            {
                SetPlayer1(i);
            }
            else
            {
                player[i].chessColor = ChessType.Watch;
            }
        }

        PlayGame();
    }

    public void DoubleMode()
    {
        PlayerPrefs.SetInt("Mode", 2); //2代表双人模式 ，1代表人机模式，0代表联网模式
    }

    //返回
    public void OnReturnBtn()
    {
        SceneManager.LoadScene(0);
    }

    //退出
    public void OnButton_Quit()
    {
        Application.Quit();
    }

}
