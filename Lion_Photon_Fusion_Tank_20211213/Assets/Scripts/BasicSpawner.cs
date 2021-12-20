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
    [Header("���a�ͦ���m")]
    public Transform[] traSpawnPoints;

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
    /// <summary>
    /// ���a�s�u��J�欰
    /// </summary>
    /// <param name="runner">�s�u���澹</param>
    /// <param name="input">��J��T</param>
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData inputData = new NetworkInputData();    //�s�W�s�u��J��� ���c

        #region �ۭq��J����P���ʸ�T
        if (Input.GetKey(KeyCode.W)) inputData.direction += Vector3.forward;    
        if (Input.GetKey(KeyCode.S)) inputData.direction += Vector3.back;
        if (Input.GetKey(KeyCode.A)) inputData.direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) inputData.direction += Vector3.right;
        #endregion

        inputData.inputFire = Input.GetKey(KeyCode.Mouse0); //���� �o�g

        input.Set(inputData);   //��J��T.�]�w(�s�u��J���)
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }
    /// <summary>
    /// ���a����ƶ��X�G���a�ѦҸ�T.���a�s�u����
    /// </summary>
    private Dictionary<PlayerRef, NetworkObject> players = new Dictionary<PlayerRef, NetworkObject>();

    /// <summary>
    /// ���a�[�J�ж���
    /// </summary>
    /// <param name="runner">�s�u���澹</param>
    /// <param name="player">���a��T</param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        //�H���ͦ��I = �H���d��.�ͦ���m�ƶq
        int randomSpawnPoint = UnityEngine.Random.Range(0, traSpawnPoints.Length);
        // �s�u���澹.�ͦ�(����.�y��.����.���a��T)
        NetworkObject playerNetworkObject = runner.Spawn(goPlayer, traSpawnPoints[randomSpawnPoint].position, Quaternion.identity, player);
        // �N���a�ѦҸ�ƻP�p�u���� �[��r�嶰�X��
        players.Add(player, playerNetworkObject);
    }

    // ���a�h�X��
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
       if(players.TryGetValue(player,out NetworkObject playerNetworkObject))
        {
            runner.Despawn(playerNetworkObject); //�s�u���澹.�����ͦ�(�Ӫ��a�s�u���󲾰�)
            players.Remove(player);              // ���a���X.����(�Ӫ��a)
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
