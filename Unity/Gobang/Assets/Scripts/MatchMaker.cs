using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class MatchMaker : MonoBehaviour {

    NetworkManager manager;
    public string roomName;

    private List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    GameObject btn;
    [SerializeField]
    Transform parent;

    private void Start()
    {
        manager = NetworkManager.singleton;
        if (manager.matchMaker == null)
            manager.StartMatchMaker();
    }

    public void setRoomName(string name)
    {
        roomName = name;
    }
    //创建房间
    public void OnCreateRoomBtn()
    {
        if (string.IsNullOrEmpty(roomName))
            return;
        manager.matchMaker.CreateMatch(roomName, 3, true, "", "", "", 0, 0, manager.OnMatchCreate);
    }
    //刷新
    public void OnReFreshBtn()
    {
        manager.matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchList);
    }
    //刷新回调函数
    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        // ...
        if (!success)
        {
            Debug.Log("error"); 
            return;
        }

        ClearList();
        foreach (var match in matches)
        {
            GameObject go = Instantiate(btn, parent);
            roomList.Add(go);
            go.GetComponent<JoinButton>().SetValue(match);
        }
    }

    private void ClearList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }

    public void OnReturnBtn()
    {
        SceneManager.LoadScene(0);
    }
}
