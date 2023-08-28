using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UnitGeneralに初期値を設定します。
/// </summary>
public class US000 : MonoBehaviour
{
    [SerializeField] UnitGeneral unitGeneral;

    //ユニット生成時に確実に初期設定を終える
    void Awake()
    {
        unitGeneral.UnitID = "UP000";
    }
}
