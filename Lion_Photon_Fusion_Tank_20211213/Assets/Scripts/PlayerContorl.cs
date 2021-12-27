using UnityEngine;
using Fusion;
using UnityEngine.UI;

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
    public Bullet bullet;
    [Header("子彈生成位置")]
    public Transform PointFire;
    [Header("砲塔")]
    public Transform traTowe;

    // 聊天輸入區域
    private InputField inputMessage;
    private Text textAllMessage;

    private NetworkCharacterController ncc;
    #endregion

    #region 屬性
    /// <summary>
    /// 開槍間隔計時器
    /// </summary>
    public TickTimer interval { get; set; }
    #endregion

    #region 事件

    private void Awake()
    {
        ncc = GetComponent<NetworkCharacterController>();
        textAllMessage = GameObject.Find("聊天訊息").GetComponent<Text>();
        inputMessage = GameObject.Find("聊天輸入區域").GetComponent<InputField>();
        inputMessage.onEndEdit.AddListener((string message) => { });
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("子彈")) Destroy(gameObject);
    }

    #endregion

    #region 方法
    /// <summary>
    /// 輸入訊息與同步訊息
    /// </summary>
    /// <param name="message">輸入資料</param>
    public void InputMessage(string message)
    {
        if(Object.HasInputAuthority)
        {
            RPC_SendMessage(message);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void RPC_SendMessage(string message, RpcInfo info = default)
    {
        textAllMessage.text += message + "\n";
    }

    // Fusion 固定更新事件 約等於 unity fixed updata
    public override void FixedUpdateNetwork()
    {
        Move();
        Fire();
    }

    private void Move()
    {
        //如果 有 輸入資料
        if(GetInput(out NetworkInputData dataInput))
        {
            // 連線角色控制器.移動(速度 * 方向 * 連線一幀時間)
            ncc.Move(speed * dataInput.direction * Runner.DeltaTime);

            Vector3 positionMouse = dataInput.positionMouse;
            positionMouse.y = traTowe.transform.position.y;

            traTowe.forward = positionMouse - transform.position;
        }
    }
    /// <summary>
    /// 開槍
    /// </summary>
    private void Fire()
    {
        if(GetInput(out NetworkInputData dataInput)) // 如果 玩家有輸入資料
        {
            if (interval.ExpiredOrNotRunning(Runner))   //如果 開槍間隔器 過期或沒有在執行
            {
                if (dataInput.inputFire)                // 如果 輸入資料是開槍左鍵
                {
                    interval = TickTimer.CreateFromSeconds(Runner, intervalFire);  //建立計時器

                    Runner.Spawn(                       // 連線.生誠(連線物件.座標.角度.輸入權限.匿名函式(執行器.生成物件))
                        bullet,
                        PointFire.position,
                        PointFire.rotation,
                        Object.InputAuthority,
                        (runner, objectSpawn) =>
                        {
                            objectSpawn.GetComponent<Bullet>().Init();
                        });
                }
            }
        }       
    }
    #endregion
}
