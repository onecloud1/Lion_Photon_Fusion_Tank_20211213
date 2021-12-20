using UnityEngine;
using Fusion;

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
    public GameObject bullet;


    private NetworkCharacterController ncc;
    #endregion

    #region �ƥ�

    private void Awake()
    {
        ncc = GetComponent<NetworkCharacterController>();
    }
    #endregion

    #region ��k

    // Fusion �T�w��s�ƥ� ������ unity fixed updata
    public override void FixedUpdateNetwork()
    {
        Move();
    }

    private void Move()
    {
        //�p�G �� ��J���
        if(GetInput(out NetworkInputData dataInput))
        {
            // �s�u���ⱱ�.����(�t�� * ��V * �s�u�@�V�ɶ�)
            ncc.Move(speed * dataInput.direction * Runner.DeltaTime);
        }
    }
    #endregion
}
