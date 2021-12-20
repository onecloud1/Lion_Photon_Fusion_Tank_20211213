using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Bullet : NetworkBehaviour
{
    #region ���
    [Header("���ʳt��"), Range(0, 100)]
    public float speed = 5;
    [Header("�s���ɶ�"), Range(0, 10)]
    public float lifeTime = 5;
    #endregion

    #region �ݩ�
    /// <summary>
    /// �s���p�ɾ�
    /// </summary>
    [Networked] //�s�u���ݩʸ��
    private TickTimer lift { get; set; }
    #endregion

    #region ��k
    /// <summary>
    /// ��l���
    /// </summary>
    public void Init()
    {
        //�s���p�ɾ� = �p�ɾ�.�q��ƫإ�(�s�u���澹.�s���ɶ�)
        lift = TickTimer.CreateFromSeconds(Runner, lifeTime);
    }
    #endregion
}
