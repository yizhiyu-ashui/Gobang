using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetPlayer : NetworkBehaviour {

    [SyncVar]
    public ChessType chessColor = ChessType.Black;
    private Button retractBtn;
    //private bool isDoubleMode = false;

    void Start()
    {
        if (isLocalPlayer)
        {
            CmdSetPlayer();
            if (chessColor == ChessType.Watch)
                return;
            retractBtn = GameObject.Find("RetractBtn").GetComponent<Button>();
            retractBtn.onClick.AddListener(RetractBtn);  
        }

        Debug.Log(Network.player.ipAddress);
    }

    void FixedUpdate()
    {
        if (isLocalPlayer && chessColor != ChessType.Watch && !NetChessBoard.Instance.gameStart)
            NetChessBoard.Instance.GameEnd();

        if (isLocalPlayer &&NetChessBoard.Instance.playerNum >1 && chessColor == NetChessBoard.Instance.turn && NetChessBoard.Instance.timer > 0.3f)  //等待对手下棋时间，0.3s后方可到对手下
            PlayerChess();

        if (isLocalPlayer && chessColor != ChessType.Watch)
            ChangeRetractBtn();

    }

    public void PlayerChess()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CmdChess(pos);
        }
    }

    [Command]
    public void CmdChess(Vector2 pos)
    {
        NetChessBoard.Instance.PlayChess(new int[2] { (int)(pos.x + 7.5f), (int)(pos.y + 7.5f) });
        NetChessBoard.Instance.timer = 0;
    }

    void ChangeRetractBtn()
    {
        if (chessColor == ChessType.Watch)
            return;
        retractBtn.interactable = NetChessBoard.Instance.turn == chessColor;
    }

    [Command]
    public void CmdSetPlayer()
    {
        NetChessBoard.Instance.playerNum++;
        if (NetChessBoard.Instance.playerNum == 1)
        {
            chessColor = ChessType.Black;
        }
        else if (NetChessBoard.Instance.playerNum == 2)
        {
            chessColor = ChessType.White;
        }
        else
        {
            chessColor = ChessType.Watch;
        }
    }

    public void RetractBtn()
    {
        CmdRetractBtn();
    }

    [Command]
    public void CmdRetractBtn()
    {
        NetChessBoard.Instance.RetractChess();
    }
}
