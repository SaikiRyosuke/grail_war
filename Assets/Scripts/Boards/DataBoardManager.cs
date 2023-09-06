using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBoardManager : MonoBehaviour
{
    //�Q��
    [SerializeField] PhysicalBoard physicalBoard;

    //���j�b�g�̈ʒu��c������f�[�^��̃{�[�h
    //�Q�[���͂��̃{�[�h��ŗ��U�I�ɂ����Ȃ�
    private UnitGeneral[,] dataBoard = new UnitGeneral[Methods.TILE_X, Methods.TILE_Y];

    //�Z�b�^�[�E�Q�b�^�[
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
    //�w�肳�ꂽ�{�[�h���W�Ƀ��j�b�g�����邩���ʂ���
    public bool JudgeExist(Vector2Int position)
    {
        if (dataBoard[position.x, position.y] != null) return true;
        return false;
    }

    //�U���\�ȃ{�[�h���W�Q��n��
    //�U���̃����W�ƍU���ʒu���w�肵���ꍇ
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
    //���j�b�g���w�肵���ꍇ
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
