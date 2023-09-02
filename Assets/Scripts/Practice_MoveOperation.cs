using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Practice_MoveOperation : PathOperation
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
        return cursorUnit.PlayerIndex == startTileUnit.PlayerIndex && inputManager.mousePositionBoard != startTilePosition;
    }
}
