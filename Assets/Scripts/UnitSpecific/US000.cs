using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UnitGeneral�ɏ����l��ݒ肵�܂��B
/// </summary>
public class US000 : MonoBehaviour
{
    [SerializeField] UnitGeneral unitGeneral;

    //���j�b�g�������Ɋm���ɏ����ݒ���I����
    //public override void SetUnitID()
    void Start()
    {
        Debug.Log("�Ă΂�Ă��`");
        unitGeneral.UnitID = "UP000";
    }
}
