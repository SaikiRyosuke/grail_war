using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMoveOperation : PathOperation
{
   
    protected override bool Condition1()
    {
        return !unitExists;
    }
    protected override bool Condition2()
    {
        return cursorUnit.PlayerIndex != startTileUnit.PlayerIndex;
    }
    protected override bool Condition3()
    {
        return cursorUnit.PlayerIndex == startTileUnit.PlayerIndex;
    }
    protected override void Operate()
    {
        //MoveHandlerをユニットの子オブジェクトとして生成しユニットの座標をうごかしてもらう
        MoveHandler mover = Instantiate(moveHandler);
        mover.transform.position = startTileUnit.transform.position;
        mover.transform.SetParent(startTileUnit.transform);
        mover.Activate(startTileUnit, pathway);
    }
    protected override void ActivateNextOperation()
    {
        //BasicOperationをアクティブ化する
        basicOperation.enabled = true;
        basicOperation.Activate();
    }
}
