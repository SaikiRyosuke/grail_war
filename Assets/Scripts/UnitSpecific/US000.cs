using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UnitGeneral�ɏ����l��ݒ肵�܂��B
/// </summary>
public class US000 : UnitGeneral
{
    //���j�b�g�������Ɋm���ɏ����ݒ���I����

    
    //public override void SetUnitID()
    void Start()
    {
        UnitID = "UP000";
        INIT_HP = 1000;
        HP = INIT_HP;
    }

    public override bool UseSkill()
    {
        Debug.Log("say hoo!");
        return true;
    }
}
