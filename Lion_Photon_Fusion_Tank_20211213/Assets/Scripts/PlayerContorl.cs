using UnityEngine;
using Fusion;

/// <summary>
/// 坦克玩家控制器
/// 前後左右移動
/// 旋轉塔與發射子彈
/// </summary>
public class PlayerContorl : NetworkBehaviour
{
    #region 欄位
    [Header("移動速度"), Range(0, 100)]
    public float speed = 7.5f;
    [Header("發射子彈間隔"), Range(0, 1.5f)]
    public float intervalFire = 0.35f;
    [Header("子彈物件")]
    public GameObject bullet;


    private NetworkCharacterController ncc;
    #endregion

    #region 事件

    private void Awake()
    {
        ncc = GetComponent<NetworkCharacterController>();
    }
    #endregion

    #region 方法

    // Fusion 固定更新事件 約等於 unity fixed updata
    public override void FixedUpdateNetwork()
    {
        Move();
    }

    private void Move()
    {
        //如果 有 輸入資料
        if(GetInput(out NetworkInputData dataInput))
        {
            // 連線角色控制器.移動(速度 * 方向 * 連線一幀時間)
            ncc.Move(speed * dataInput.direction * Runner.DeltaTime);
        }
    }
    #endregion
}
