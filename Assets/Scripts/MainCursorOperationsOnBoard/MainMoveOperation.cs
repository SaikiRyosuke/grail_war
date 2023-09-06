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
        //MoveHandler�����j�b�g�̎q�I�u�W�F�N�g�Ƃ��Đ��������j�b�g�̍��W�����������Ă��炤
        MoveHandler mover = Instantiate(moveHandler);
        mover.transform.position = startTileUnit.transform.position;
        mover.transform.SetParent(startTileUnit.transform);
        mover.Activate(startTileUnit, pathway);
    }
    protected override void ActivateNextOperation()
    {
        //BasicOperation���A�N�e�B�u������
        basicOperation.enabled = true;
        basicOperation.Activate();
    }
}
