
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAttackOperation : PointInRangeOperation
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
}
