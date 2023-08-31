using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UnitGeneralに初期値を設定します。
/// </summary>
public class US000 : UnitGeneral
{
    //ユニット生成時に確実に初期設定を終える

    
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
