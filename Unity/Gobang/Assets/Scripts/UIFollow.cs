using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFollow : MonoBehaviour {

	void Update ()
	{
	    if (ChessBoard.Instance.chessStack.Count > 0)
	        transform.position = ChessBoard.Instance.chessStack.Peek().position;
	}
    //重玩
    public void OnRelayBtn()
    {
        SceneManager.LoadScene(1);
    }

    //返回
    public void ReturnBtn()
    {
        SceneManager.LoadScene(0);
    }
}
