using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetWorkUI : NetworkBehaviour {

    public void StartHost()
    {
        NetworkManager.singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.singleton.networkAddress = GameObject.Find("IP_InputField").GetComponent<InputField>().text; 
        NetworkManager.singleton.StartClient();
    }

    public void StopHost()
    {
        NetworkManager.singleton.StopHost();
    }

    public void OffOnlineSet()
    {
        GameObject.Find("Host").GetComponent<Button>().onClick.AddListener(StartHost);
        GameObject.Find("Client").GetComponent<Button>().onClick.AddListener(StartClient);
    }

    public void OnLineSet()
    {
        GameObject.Find("ReturnBtn").GetComponent<Button>().onClick.AddListener(StopHost); 
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceeneLoaded;
    }

    private void OnSceeneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //switch (arg0.buildIndex)
        //{
        //    case 0:
        //        OffOnlineSet();
        //        break;
        //    case 1:
        //        OnLineSet();
        //        break;
        //    default:
        //        break;
        //}

    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceeneLoaded;
    }
}
