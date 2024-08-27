using Fantasy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class NetWorkManager : SingletonMono<NetWorkManager>
{


    public string Ip= "127.0.0.1";
    public ushort Port= 20000;
    protected Scene _scene;
    protected Session _session;

    public float frame = 1 / 30f;
    public float nextUpdateFrame = 0;

    public Session Session { get => _session;private set => _session = value; }

    public event Action<ConnectResult> ConnectEvent;
    ConnectStatus connectStatus = ConnectStatus.NoStart;

    private async FTask OnConnectAddressableClick()
    {
        // 1、初始化Fantasy
        _scene = await Fantasy.Entry.Initialize(GetType().Assembly);
        // 2、连接到Gate服务器
        connectStatus = ConnectStatus.Starting;
        _session = _scene.Connect(
            $"{Ip}:{Port}",
            NetworkProtocolType.KCP,
            OnConnectComplete,
            OnConnectFail,
            OnConnectDisconnect,
            false, 5000);
        // 3、发送C2G_CreateAddressableRequest协议给Gate，进行创建Addressable.
        var response = (G2C_CreateAddressableResponse)await _session.Call(new C2G_CreateAddressableRequest());
        if (response.ErrorCode != 0)
        {
            Log.Debug("创建Addressable失败！");

            return;
        }
        Log.Debug("创建Addressable成功！");
        InitNetwork().Coroutine();
    }
    private void OnConnectComplete()
    {
        Debug.Log("连接成功");
        connectStatus = ConnectStatus.Started;
        _session.AddComponent<SessionHeartbeatComponent>().Start(2000);
        ConnectEvent?.Invoke(ConnectResult.Succ);
    }

    private void OnConnectFail()
    {
        Debug.LogError("连接失败");
        connectStatus = ConnectStatus.NoStart;
        ConnectEvent?.Invoke(ConnectResult.Fail);
    }

    private void OnConnectDisconnect()
    {
        Debug.LogError("连接断开");
        connectStatus = ConnectStatus.NoStart;
        ConnectEvent?.Invoke(ConnectResult.Break);
    }

    async FTask InitNetwork()
    {
       var response= (M2C_ResponseInit) await _session.Call(new C2M_RequestInit());
        for(int i = 0; i < response.initData.Count; i++)
        {
            var data= response.initData[i];
            CreateGameObjByServer(data);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        InitNetworkObj();
    }
    void Start()
    {
        OnConnectAddressableClick().Coroutine();
    }
    private void FixedUpdate()
    {
        if(connectStatus==ConnectStatus.Started&&Time.time> nextUpdateFrame)
        {
            nextUpdateFrame=Time.time+frame;
            UpdateNetworkObj();
        }
    }
}
