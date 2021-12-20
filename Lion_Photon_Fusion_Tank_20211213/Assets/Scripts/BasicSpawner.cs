using UnityEngine;
using UnityEngine.UI;
//引用 Fusion 命名空間 連線用
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// 連線基底生成器
/// </summary>
public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    #region 欄位
    [Header("創建與加入房間欄位")]
    public InputField inputFiedCreateRoom;
    public InputField inputFiedJoinRoom;
    [Header("玩家控制物件")]
    public NetworkPrefabRef goPlayer;
    [Header("畫布連線")]
    public GameObject goCancas;
    [Header("玩家生成位置")]
    public Transform[] traSpawnPoints;

    //玩家輸入的房間名稱
    private string roomNameInput;
    private NetworkRunner runner;
    #endregion

    /// <summary>
    /// 按鈕點擊呼叫：創建房間
    /// </summary>
    #region 方法
    public void BtnCreatRoom()
    {
        roomNameInput = inputFiedCreateRoom.text;
        print("創建房間：" + roomNameInput);
        StartGame(GameMode.Host);
    }

    public void BtnJoinRoom()
    {
        roomNameInput = inputFiedJoinRoom.text;
        print("加入房間：" + roomNameInput);
        StartGame(GameMode.Client);
    }

    /// <summary>
    /// 開始連線遊戲
    /// </summary>
    /// <param name="mode">連線模式：主機、客戶</param>
    private async void StartGame(GameMode mode)
    {
        print("<color=yellow>開始連線</color>");

        runner = gameObject.AddComponent<NetworkRunner>(); // 連線執行器 = 添加元件<連線執行器>
        runner.ProvideInput = true;                        // 連線執行器.是否提供輸入 = 是

        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomNameInput,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneObjectProvider = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        print("<color=yellow>連線完成</color>");
        goCancas.SetActive(false);
    }
    #endregion

    #region Fusion 回乎函式區域
    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
       
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
      
    }
    /// <summary>
    /// 玩家連線輸入行為
    /// </summary>
    /// <param name="runner">連線執行器</param>
    /// <param name="input">輸入資訊</param>
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData inputData = new NetworkInputData();    //新增連線輸入資料 結構

        #region 自訂輸入按鍵與移動資訊
        if (Input.GetKey(KeyCode.W)) inputData.direction += Vector3.forward;    
        if (Input.GetKey(KeyCode.S)) inputData.direction += Vector3.back;
        if (Input.GetKey(KeyCode.A)) inputData.direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) inputData.direction += Vector3.right;
        #endregion

        inputData.inputFire = Input.GetKey(KeyCode.Mouse0); //左鍵 發射

        input.Set(inputData);   //輸入資訊.設定(連線輸入資料)
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }
    /// <summary>
    /// 玩家的資料集合：玩家參考資訊.玩家連線物件
    /// </summary>
    private Dictionary<PlayerRef, NetworkObject> players = new Dictionary<PlayerRef, NetworkObject>();

    /// <summary>
    /// 當玩家加入房間後
    /// </summary>
    /// <param name="runner">連線執行器</param>
    /// <param name="player">玩家資訊</param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        //隨機生成點 = 隨機範圍.生成位置數量
        int randomSpawnPoint = UnityEngine.Random.Range(0, traSpawnPoints.Length);
        // 連線執行器.生成(物件.座標.角度.玩家資訊)
        NetworkObject playerNetworkObject = runner.Spawn(goPlayer, traSpawnPoints[randomSpawnPoint].position, Quaternion.identity, player);
        // 將玩家參考資料與聯線物件 加到字典集合內
        players.Add(player, playerNetworkObject);
    }

    // 當玩家退出後
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
       if(players.TryGetValue(player,out NetworkObject playerNetworkObject))
        {
            runner.Despawn(playerNetworkObject); //連線執行器.取消生成(該玩家連線物件移除)
            players.Remove(player);              // 玩家集合.移除(該玩家)
        }
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
       
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
      
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
      
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
       
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
       
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

    #endregion
}
