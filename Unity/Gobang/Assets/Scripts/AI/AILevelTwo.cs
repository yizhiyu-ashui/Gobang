using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * AI下棋二
 * 1、添加完善的棋谱
 * 2、循环枚举棋谱函数，得出每一种包含的可能分数
 * 3、评分函数，得到最大分位置
 * 4、下棋最大赢分概率
 */

public class AILevelTwo : AILevelOne {

    protected override void Start()
    {
        toScore.Add("aa___", 100);                      //眠二
        toScore.Add("a_a__", 100);
        toScore.Add("___aa", 100);
        toScore.Add("__a_a", 100);
        toScore.Add("a__a_", 100);
        toScore.Add("_a__a", 100);
        toScore.Add("a___a", 100);


        toScore.Add("__aa__", 500);                     //活二 "_aa___"
        toScore.Add("_a_a_", 500);
        toScore.Add("_a__a_", 500);

        toScore.Add("_aa__", 500);
        toScore.Add("__aa_", 500);


        toScore.Add("a_a_a", 1000);                     // bool lfirst = true, lstop,rstop = false  int AllNum = 1
        toScore.Add("aa__a", 1000);
        toScore.Add("_aa_a", 1000);
        toScore.Add("a_aa_", 1000);
        toScore.Add("_a_aa", 1000);
        toScore.Add("aa_a_", 1000);
        toScore.Add("aaa__", 1000);                     //眠三

        toScore.Add("_aa_a_", 9000);                    //跳活三
        toScore.Add("_a_aa_", 9000);

        toScore.Add("_aaa_", 10000);                    //活三       


        toScore.Add("a_aaa", 15000);                    //冲四
        toScore.Add("aaa_a", 15000);                    //冲四
        toScore.Add("_aaaa", 15000);                    //冲四
        toScore.Add("aaaa_", 15000);                    //冲四
        toScore.Add("aa_aa", 15000);                    //冲四        


        toScore.Add("_aaaa_", 1000000);                 //活四

        toScore.Add("aaaaa", float.MaxValue);           //连五
        if (chessColor != ChessType.Watch)
            Debug.Log(chessColor + "AILevelTwo");
    }

    public override void CheckOneLine(int[] pos, int[] offset, int chess)
    {
        bool lfirst = true, lstop = false, rstop = false;
        int AllNum = 1;
        string str = "a";
        int ri = offset[0], rj = offset[1];
        int li = -offset[0], lj = -offset[1];
        while (AllNum < 7&&(!lstop || !rstop))
        {
            if (lfirst)
            {
                //左边
                if ((pos[0] + li >= 0 && pos[0] + li < 15) &&
                    (pos[1] + lj >= 0 && pos[1] + lj < 15) && !lstop)
                {
                    if (ChessBoard.Instance.grid[pos[0] + li, pos[1] + lj] == chess)
                    {
                        AllNum++;
                        str = "a" + str;
                    }
                    else if (ChessBoard.Instance.grid[pos[0] + li, pos[1] + lj] == 0)
                    {
                        AllNum++;
                        str = "_" + str;
                        if (!rstop)
                            lfirst = false;
                    }
                    else
                    {
                        lstop = true;
                        if (!rstop)
                            lfirst = false;
                    }
                    li -= offset[0];
                    lj -= offset[1];
                }
                else
                {
                    lstop = true;
                    if (!rstop)
                        lfirst = false;
                }
            }
            else
            {
                //右边
                if ((pos[0] + ri >= 0 && pos[0] + ri < 15) &&
                    (pos[1] + rj >= 0 && pos[1] + rj < 15) && !lfirst)
                {
                    if (ChessBoard.Instance.grid[pos[0] + ri, pos[1] + rj] == chess)
                    {
                        AllNum++;
                        str += "a";
                    }
                    else if (ChessBoard.Instance.grid[pos[0] + ri, pos[1] + rj] == 0)
                    {
                        AllNum++;
                        str += "_";
                        if (!rstop)
                            lfirst = true;
                    }
                    else
                    {
                        rstop = true;
                        if (!lstop)
                            lfirst = true;
                    }
                    ri += offset[0];
                    rj += offset[1];
                }
                else
                {
                    rstop = true;
                    if (!lstop)
                        lfirst = true;
                }
            }
        }
        string cmpStr = "";
        foreach (var keyInfo in toScore)
        {
            if (str.Contains(keyInfo.Key))
            {
                if (cmpStr != "")
                {
                    if (toScore[keyInfo.Key] > toScore[cmpStr])
                        cmpStr = keyInfo.Key;
                }
                else
                {
                    cmpStr = keyInfo.Key;
                }
            }
        }

        if (cmpStr != "")
        {
            score[pos[0], pos[1]] += toScore[cmpStr];
        }
    }

    public override void SetScore(int[] pos)
    {
        base.SetScore(pos);
        CheckOneLine(pos, new int[2] { 1, 0 }, 2);
        CheckOneLine(pos, new int[2] { 0, 1 }, 2);
        CheckOneLine(pos, new int[2] { 1, 1 }, 2);
        CheckOneLine(pos, new int[2] { 1, -1 }, 2);
    }
}
