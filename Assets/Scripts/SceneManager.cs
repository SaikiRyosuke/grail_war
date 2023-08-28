using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    //参照
    [SerializeField] PhysicalBoard physicalBoard;
    [SerializeField] UnitManager unitManager;
    [SerializeField] PrimaryUnitOperator primaryUnitOperator;
    [SerializeField] DataBoardManager dataBoardManager;
    [SerializeField] InputManager inputManager;

    //現在の操作状態
    public 
        enum OperationType
    {
        Basic,
        Move,
        Attack,
        Choice
    }

    public OperationType operationType;

    void Start()
    {
        
        //ゲームボードを作成する。
        physicalBoard.Activate();

        
        //ユニットの初期設定をする。
        unitManager.Activate();

        //プレイヤーがユニットを操作できるようにする
        primaryUnitOperator.Activate();
    }


    void Update()
    {
        
    }
}
