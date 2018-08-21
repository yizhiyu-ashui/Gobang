using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public ChessType chessColor = ChessType.Black;
    private Button retractBtn;
    //private bool isDoubleMode = false;

    protected virtual void Start()
    {
        retractBtn = GameObject.Find("RetractBtn").GetComponent<Button>();
        //if (PlayerPrefs.GetInt("Mode") == 2)
        //    isDoubleMode = true;
    }

    protected virtual void FixedUpdate()
    {
        if (chessColor == ChessBoard.Instance.turn && ChessBoard.Instance.timer > 1.0f)  //等待对手下棋时间，1s后方可到对手下
            PlayerChess();

        ChangeRetractBtn();
    }

    public virtual void PlayerChess()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //print((int)(pos.x + 7.5f) + "++" + (int)(pos.y + 7.5f));
            //点击位置超出棋盘不作反应
            if ((int) (pos.x + 7.5f) < 0 || (int) (pos.x + 7.5f) > 14 || (int) (pos.y + 7.5f) < 0 ||
                (int) (pos.y + 7.5f) > 14)
                return;

            ChessBoard.Instance.PlayChess(new int[2] { (int)(pos.x + 7.5f), (int)(pos.y + 7.5f) });
            ChessBoard.Instance.timer = 0;
        }
    }

    protected virtual void ChangeRetractBtn()
    {
        if (chessColor == ChessType.Watch)
            return;
        retractBtn.interactable = ChessBoard.Instance.turn == chessColor;
    }
}
