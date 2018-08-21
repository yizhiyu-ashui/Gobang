using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetChessBoard : NetworkBehaviour {

    static NetChessBoard _instance;

    [SyncVar]
    public ChessType turn = ChessType.Black;
    public int[,] grid;
    public GameObject[] prefabs;
    public float timer = 0;
    [SyncVar]
    public bool gameStart = false;
    private Transform parent;
    public Stack<Transform> chessStack = new Stack<Transform>();
    public Text winner;

    [SyncVar]  //同步变量
    public int playerNum = 0;//玩家人数，第一个进来的代表黑棋，第二个进来的代表白棋，后面进来代表观战

    public static NetChessBoard Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        grid = new int[15, 15];

        parent = GameObject.Find("Parent").transform;
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
    }
    /// <summary>
    /// 下棋
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public void  PlayChess(int[] pos)
    {
        if (!gameStart)
            return;
        pos[0] = Mathf.Clamp(pos[0], 0, 14);
        pos[1] = Mathf.Clamp(pos[1], 0, 14);

        if (grid[pos[0], pos[1]] != 0)
            return;
        if (turn == ChessType.Black)
        {
            GameObject go = Instantiate(prefabs[0], new Vector3(pos[0] - 7, pos[1] - 7), Quaternion.identity);
            NetworkServer.Spawn(go); //在每个客户端中都生成同样的物体(对象)
            chessStack.Push(go.transform);
            go.transform.SetParent(parent);
            grid[pos[0], pos[1]] = 1;
            //判断胜负
            if (CheckWinner(pos))
            {
                //GameEnd();
                gameStart = false;
                return;
            }
            turn = ChessType.White;
        }
        else if (turn == ChessType.White)
        {
            GameObject go = Instantiate(prefabs[1], new Vector3(pos[0] - 7, pos[1] - 7), Quaternion.identity);
            NetworkServer.Spawn(go); 
            chessStack.Push(go.transform);
            go.transform.SetParent(parent);
            grid[pos[0], pos[1]] = 2;
            //判断胜负
            if (CheckWinner(pos))
            {
                //GameEnd();
                gameStart = false;
                return;
            }
            turn = ChessType.Black;
        }
    }
    /// <summary>
    /// 结束
    /// </summary>
     public void GameEnd()
    {
       
        winner.transform.parent.parent.gameObject.SetActive(true);
        switch (turn)
        {
            case ChessType.Watch:
                break;
            case ChessType.Black:
                winner.text = "黑棋胜！";
                break;
            case ChessType.White:
                winner.text = "白棋胜！";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Debug.Log(turn + "赢了！");
    }

    /// <summary>
    /// 判断输赢
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CheckWinner(int[] pos)
    {
        if (CheckOneLine(pos, new int[2] { 1, 0 })) //左右
            return true;
        if (CheckOneLine(pos, new int[2] { 0, 1 })) //上下
            return true;
        if (CheckOneLine(pos, new int[2] { 1, 1 }))  //正斜
            return true;
        if (CheckOneLine(pos, new int[2] { 1, -1 })) //反斜
            return true;
        return false;
    }

    /// <summary>
    /// 检查一条线
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public bool CheckOneLine(int[] pos, int[] offset)
    {
        int lineNum = 1;
        //右边
        for (int i = offset[0], j = offset[1];
            (pos[0] + i >= 0 && pos[0] + i < 15) && (pos[1] + j >= 0 && pos[1] + j < 15); i += offset[0], j += offset[1])
        {
            if (grid[pos[0] + i, pos[1] + j] == (int)turn)
            {
                lineNum++;
            }
            else
            {
                break;
            }
        }
        //左边
        for (int i = -offset[0], j = -offset[1];
            (pos[0] + i >= 0 && pos[0] + i < 15) && (pos[1] + j >= 0 && pos[1] + j < 15); i -= offset[0], j -= offset[1])
        {
            if (grid[pos[0] + i, pos[1] + j] == (int)turn)
            {
                lineNum++;
            }
            else
            {
                break;
            }
        }

        if (lineNum > 4)
            return true;
        return false;
    }

    /// <summary>
    /// 悔棋
    /// </summary>
    public void RetractChess()
    {
        if (chessStack.Count > 1)
        {
            Transform tran = chessStack.Pop();
            grid[(int)(tran.position.x + 7), (int)(tran.position.y + 7)] = 0;
            Destroy(tran.gameObject);
            tran = chessStack.Pop();
            grid[(int)(tran.position.x + 7), (int)(tran.position.y + 7)] = 0;
            Destroy(tran.gameObject);
        }
    }

    public void OnButton_New()
    {
        gameStart = true;
        while (chessStack.Count > 0)
        {
            Transform tran = chessStack.Pop();
            grid[(int)(tran.position.x + 7), (int)(tran.position.y + 7)] = 0;
            Destroy(tran.gameObject);
        }
    }

    public void OnQuitBtn()
    {
        NetworkManager.singleton.matchMaker.DropConnection(NetworkManager.singleton.matchInfo.networkId,
            NetworkManager.singleton.matchInfo.nodeId,
            0, NetworkManager.singleton.OnDropConnection);

        NetworkManager.singleton.StopHost();
    }
}
