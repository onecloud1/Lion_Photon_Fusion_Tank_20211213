using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// 連線輸入資料
/// 保存連線玩家輸入資訊
/// </summary>
public struct NetworkInputData : INetworkInput
{
    //坦克移動方向
    public Vector3 direction;
    //是否點擊左鍵
    public bool inputFire;
}
