using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UnitGeneral�ɏ����l��ݒ肵�܂��B
/// </summary>
public class US000 : UnitGeneral
{
    //���j�b�g�������Ɋm���ɏ����ݒ���I����
    //public readonly string unitID = "UP000";
    
    //public override void SetUnitID()
    void Start()
    {
        UnitID = "UP000";
        HP = "1000";
    }

    public override bool UseSkill()
    {
        Debug.Log("say hoo!");
        return true;
    }
}
