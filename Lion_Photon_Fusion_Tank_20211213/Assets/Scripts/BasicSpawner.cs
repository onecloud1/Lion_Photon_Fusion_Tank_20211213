using UnityEngine;
using UnityEngine.UI;
//�ޥ� Fusion �R�W�Ŷ� �s�u��
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// �s�u�򩳥ͦ���
/// </summary>
public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    #region ���
    [Header("�ЫػP�[�J�ж����")]
    public InputField inputFiedCreateRoom;
    public InputField inputFiedJoinRoom;
    [Header("���a�����")]
    public NetworkPrefabRef goPlayer;
    [Header("�e���s�u")]
    public GameObject goCancas;

    //���a��J���ж��W��
    private string roomNameInput;
    private NetworkRunner runner;
    #endregion

    /// <summary>
    /// ���s�I���I�s�G�Ыةж�
    /// </summary>
    #region ��k
    public void BtnCreatRoom()
    {
        roomNameInput = inputFiedCreateRoom.text;
        print("�Ыةж��G" + roomNameInput);
        StartGame(GameMode.Host);
    }

    public void BtnJoinRoom()
    {
        roomNameInput = inputFiedJoinRoom.text;
        print("�[�J�ж��G" + roomNameInput);
        StartGame(GameMode.Client);
    }

    /// <summary>
    /// �}�l�s�u�C��
    /// </summary>
    /// <param name="mode">�s�u�Ҧ��G�D���B�Ȥ�</param>
    private async void StartGame(GameMode mode)
    {
        print("<color=yellow>�}�l�s�u</color>");

        runner = gameObject.AddComponent<NetworkRunner>(); // �s�u���澹 = �K�[����<�s�u���澹>
        runner.ProvideInput = true;                        // �s�u���澹.�O�_���ѿ�J = �O

        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomNameInput,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneObjectProvider = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        print("<color=yellow>�s�u����</color>");
        goCancas.SetActive(false);
    }
    #endregion

    #region Fusion �^�G�禡�ϰ�
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

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
      
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }
    /// <summary>
    /// ���a�[�J�ж���
    /// </summary>
    /// <param name="runner">�s�u���澹</param>
    /// <param name="player">���a��T</param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // �s�u���澹.�ͦ�(����.�y��.����.���a��T)
        runner.Spawn(goPlayer, new Vector3(-5, 1, -10), Quaternion.identity, player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
       
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
