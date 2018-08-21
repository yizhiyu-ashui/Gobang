using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour {


    public void Hideing()
    {
        gameObject.SetActive(false);
        ChessBoard.Instance.gameStart = true;
    }
}
