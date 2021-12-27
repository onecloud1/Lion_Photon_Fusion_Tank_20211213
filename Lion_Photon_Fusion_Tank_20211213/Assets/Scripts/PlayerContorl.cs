using UnityEngine;
using Fusion;
using UnityEngine.UI;

/// <summary>
/// �Z�J���a���
/// �e�ᥪ�k����
/// �����P�o�g�l�u
/// </summary>
public class PlayerContorl : NetworkBehaviour
{
    #region ���
    [Header("���ʳt��"), Range(0, 100)]
    public float speed = 7.5f;
    [Header("�o�g�l�u���j"), Range(0, 1.5f)]
    public float intervalFire = 0.35f;
    [Header("�l�u����")]
    public Bullet bullet;
    [Header("�l�u�ͦ���m")]
    public Transform PointFire;
    [Header("����")]
    public Transform traTowe;

    // ��ѿ�J�ϰ�
    private InputField inputMessage;
    private Text textAllMessage;

    private NetworkCharacterController ncc;
    #endregion

    #region �ݩ�
    /// <summary>
    /// �}�j���j�p�ɾ�
    /// </summary>
    public TickTimer interval { get; set; }
    #endregion

    #region �ƥ�

    private void Awake()
    {
        ncc = GetComponent<NetworkCharacterController>();
        textAllMessage = GameObject.Find("��ѰT��").GetComponent<Text>();
        inputMessage = GameObject.Find("��ѿ�J�ϰ�").GetComponent<InputField>();
        inputMessage.onEndEdit.AddListener((string message) => { });
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("�l�u")) Destroy(gameObject);
    }

    #endregion

    #region ��k
    /// <summary>
    /// ��J�T���P�P�B�T��
    /// </summary>
    /// <param name="message">��J���</param>
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

    // Fusion �T�w��s�ƥ� ������ unity fixed updata
    public override void FixedUpdateNetwork()
    {
        Move();
        Fire();
    }

    private void Move()
    {
        //�p�G �� ��J���
        if(GetInput(out NetworkInputData dataInput))
        {
            // �s�u���ⱱ�.����(�t�� * ��V * �s�u�@�V�ɶ�)
            ncc.Move(speed * dataInput.direction * Runner.DeltaTime);

            Vector3 positionMouse = dataInput.positionMouse;
            positionMouse.y = traTowe.transform.position.y;

            traTowe.forward = positionMouse - transform.position;
        }
    }
    /// <summary>
    /// �}�j
    /// </summary>
    private void Fire()
    {
        if(GetInput(out NetworkInputData dataInput)) // �p�G ���a����J���
        {
            if (interval.ExpiredOrNotRunning(Runner))   //�p�G �}�j���j�� �L���ΨS���b����
            {
                if (dataInput.inputFire)                // �p�G ��J��ƬO�}�j����
                {
                    interval = TickTimer.CreateFromSeconds(Runner, intervalFire);  //�إ߭p�ɾ�

                    Runner.Spawn(                       // �s�u.�͸�(�s�u����.�y��.����.��J�v��.�ΦW�禡(���澹.�ͦ�����))
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
