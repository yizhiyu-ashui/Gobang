using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * AI下棋一
 * 1、添加简单棋谱
 * 2、循环枚举棋谱，判断每条线路的分数
 * 3、评分函数，得出最大分数位置
 * 4、下棋最大分数位置
 */

public class AILevelOne : Player {

    protected Dictionary<string, float> toScore = new Dictionary<string, float>();
    public float[,] score = new float[15, 15];

    protected override void Start()
    {
        toScore.Add("_aa_", 100);
        toScore.Add("aa_", 50);
        toScore.Add("_aa", 50);

        toScore.Add("_aaa_", 1000);
        toScore.Add("aaa_", 500);
        toScore.Add("_aaa", 500);

        toScore.Add("_aaaa_", 10000);
        toScore.Add("aaaa_", 5000);
        toScore.Add("_aaaa", 5000);

        toScore.Add("aaaaa", float.MaxValue);
        toScore.Add("aaaaa_", float.MaxValue);
        toScore.Add("_aaaaa", float.MaxValue);
        toScore.Add("_aaaaa_", float.MaxValue);

        if (chessColor != ChessType.Watch)
            Debug.Log(chessColor + "AILevelOne");
    }

    public override void PlayerChess()
    {
        if (ChessBoard.Instance.chessStack.Count == 0)
        {
            if (ChessBoard.Instance.PlayChess(new int[2] { 7, 7 }))
                ChessBoard.Instance.timer = 0;
            return;
        }

        float maxScore = 0;
        int[] maxpos = new int[2] { 7, 7 };
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                if (ChessBoard.Instance.grid[i,j] == 0)
                {
                    SetScore(new int[2] { i, j });
                    if (score[i,j]>= maxScore)
                    {
                        maxpos[0] = i;
                        maxpos[1] = j;
                        maxScore = score[i, j];
                    }
                }
            }
        }

        if (ChessBoard.Instance.PlayChess(maxpos))
            ChessBoard.Instance.timer = 0;
    }


    public virtual void CheckOneLine(int[] pos, int[] offset,int chess)
    {
        string str = "a";
        //右边
        for (int i = offset[0], j = offset[1];
            (pos[0] + i >= 0 && pos[0] + i < 15) && 
            (pos[1] + j >= 0 && pos[1] + j < 15); 
            i += offset[0], j += offset[1])
        {
            if (ChessBoard.Instance.grid[pos[0] + i, pos[1] + j] == chess)
            {
                str += "a";
            }
            else if (ChessBoard.Instance.grid[pos[0] + i, pos[1] + j] == 0)
            {
                str += "_";
                break;
            }
            else
            {
                break;
            }
        }
        //左边
        for (int i = -offset[0], j = -offset[1];
            (pos[0] + i >= 0 && pos[0] + i < 15) &&
            (pos[1] + j >= 0 && pos[1] + j < 15); 
            i -= offset[0], j -= offset[1])
        {
            if (ChessBoard.Instance.grid[pos[0] + i, pos[1] + j] == chess)
            {
                str = "a"+str;
            }
            else if (ChessBoard.Instance.grid[pos[0] + i, pos[1] + j] == 0)
            {
                str = "_"+str;
                break;
            }
            else
            {
                break;
            }
        }

        if (toScore.ContainsKey(str))
        {
            score[pos[0], pos[1]] += toScore[str]; 
        }

    }

    public virtual void SetScore(int[] pos)
    {
        score[pos[0], pos[1]] = 0;

        CheckOneLine(pos, new int[2] { 1, 0 },1);
        CheckOneLine(pos, new int[2] { 0, 1 },1);
        CheckOneLine(pos, new int[2] { 1, 1 },1);
        CheckOneLine(pos, new int[2] { 1, -1 },1);

        //CheckOneLine(pos, new int[2] { 1, 0 }, 2);
        //CheckOneLine(pos, new int[2] { 0, 1 }, 2);
        //CheckOneLine(pos, new int[2] { 1, 1 }, 2);
        //CheckOneLine(pos, new int[2] { 1, -1 }, 2);
    }

    protected override void ChangeRetractBtn()
    {
    }
}
