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
    void Awake()
    {
        unitGeneral.UnitID = "UP000";
    }
}
