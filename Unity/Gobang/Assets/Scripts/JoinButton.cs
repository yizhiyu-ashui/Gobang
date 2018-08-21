using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class JoinButton : MonoBehaviour {

    NetworkManager manager;
    public Text nameText;
    public MatchInfoSnapshot info;

    private void Start()
    {
        manager = NetworkManager.singleton;
        if (manager.matchMaker == null)
            manager.StartMatchMaker();
    }

    public void SetValue(MatchInfoSnapshot _info)
    {
        info = _info;
        nameText.text = info.name;
    }

    public void OnJoinBtn()
    {
        Debug.Log("加入");
        manager.matchMaker.JoinMatch(info.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
    }
}
