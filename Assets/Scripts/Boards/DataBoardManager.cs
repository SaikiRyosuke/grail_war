using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBoardManager : MonoBehaviour
{
    //参照
    [SerializeField] PhysicalBoard physicalBoard;

    //ユニットの位置を把握するデータ上のボード
    //ゲームはこのボード上で離散的におこなう
    private UnitGeneral[,] dataBoard = new UnitGeneral[Methods.TILE_X, Methods.TILE_Y];

    //セッター・ゲッター
    public void SetToDataBoard(UnitGeneral unit)
    {
        dataBoard[unit.Position.x, unit.Position.y] = unit;
    }
    public UnitGeneral GetFromDataBoard(Vector2Int positionBoard)
    {
        return dataBoard[positionBoard.x, positionBoard.y];
    }
    public void MoveUnit(UnitGeneral unit, Vector2Int previousPosition)
    {
        if (dataBoard[unit.Position.x, unit.Position.y] != null)
        {
            Debug.Log("someone already exists.");
            return;
        }
        dataBoard[previousPosition.x, previousPosition.y] = null;
        SetToDataBoard(unit);

    }
    //指定されたボード座標にユニットがいるか判別する
    public bool JudgeExist(Vector2Int position)
    {
        if (dataBoard[position.x, position.y] != null) return true;
        return false;
    }

    //攻撃可能なボード座標群を渡す
    //攻撃のレンジと攻撃位置を指定した場合
    public List<Vector2Int> GetRangeWithRangeNumberAndPositions(float range, Vector2Int originPosition)
    {
        List<Vector2Int> attackablePositions = new List<Vector2Int>();
        for(int i = 0; i < Methods.TILE_X; i++)
        {
            for(int j = 0; j < Methods.TILE_Y; j++)
            {
                float distance = Methods.EuclidDistanceBetween(originPosition, new Vector2Int(i, j));
                if(distance <= range)
                {
                    attackablePositions.Add(new Vector2Int(i, j));
                }
            }
        }
        return attackablePositions;
    }
    //ユニットを指定した場合
    public List<Vector2Int> AttackablePositionsWithUnit(UnitGeneral unit)
    {
        return GetRangeWithRangeNumberAndPositions(unit.atkRange, unit.Position);
    }

    public void Activate()
    {

    }

    private void Update()
    {
        
    }
}
