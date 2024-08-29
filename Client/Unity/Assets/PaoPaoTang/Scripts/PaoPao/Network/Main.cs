using Fantasy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Main : MonoBehaviour
{
    public UIButton uIButton;
    public UIButton stopButton;

    private void Start()
    {
        //uIButton = GetComponent<UIButton>();
        UIEventListener.Get(uIButton.gameObject).onClick = EnterLobby;
        UIEventListener.Get(stopButton.gameObject).onClick = StopLobby;

    }

    private void StopLobby(GameObject go)
    {
        uIButton.enabled = true;
        UIEventListener.Get(uIButton.gameObject).onClick = EnterLobby;
        UIEventListener.Get(stopButton.gameObject).onClick = null;
        NetWorkManager.Instance.Session.Call(new C2G_LobbyRequest()
        {
            StatusCode = 2
        });
        uIButton.GetComponentInChildren<UILabel>().text = "开始排队";
    }

    void EnterLobby(GameObject go)
    {
        uIButton.enabled = false;
        UIEventListener.Get(uIButton.gameObject).onClick = null;
        UIEventListener.Get(stopButton.gameObject).onClick = StopLobby;
        NetWorkManager.Instance.Session.Call(new C2G_LobbyRequest()
        {
            StatusCode = 1
        });
        uIButton.GetComponentInChildren<UILabel>().text = "排队中";
    }
}
