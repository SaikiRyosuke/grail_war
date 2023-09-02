using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PathOperation : MonoBehaviour
{

    [SerializeField] protected InputManager inputManager;
    [SerializeField] PhysicalBoard physicalBoard;
    [SerializeField] DataBoardManager dataBoardManager;
    [SerializeField] PrimaryUnitOperator primaryUnitOperator;
    [SerializeField] BasicOperation basicOperation;
    [SerializeField] SceneManager sceneManager;
    [SerializeField] MoveHandler moveHandler;
    [SerializeField] TileColor colorTile;

    //�w��o�H
    protected List<Vector2Int> pathway;

    //�N���b�N�����ŏ��̃^�C���̈ʒu
    protected Vector2Int startTilePosition;
    //�ŏ��̃^�C���̈ʒu�̃��j�b�g
    protected UnitGeneral startTileUnit;

    //�ԃ}�X�i�֎~�}�X�j�ɃJ�[�\�������邩
    bool MOVE_PROHIBITED;

    //�J�[�\�����{�[�h��ɂ��邩�̔���
    protected bool isOnBoard = false;
    //�J�[�\��������^�C���Ƀ��j�b�g�����݂��邩�̔���
    protected bool unitExists = false;

    //�J�[�\���̂���^�C��
    protected Tile tileWithCursor = null;
    //�O��J�[�\�����������^�C��
    protected Tile tileWithCursorBefore = null;

    //�J�[�\��������^�C���̃��j�b�g
    protected UnitGeneral cursorUnit = null;


    //�����Ԃ̉��C
    //�F�̃p�^�[������
    Color original;

    Color noneCursor_Self;
    Color noneCursor_None;
    Color noneCursor_Target;
    Color noneCursor_NotApplicable;
    
    
    Color withCursor_Self;
    Color withCursor_None;
    Color withCursor_Target;
    Color withCursor_NotApplicable;
    

    
    public void Activate(Vector2Int initTilePosition)
    {
        //������
        startTilePosition = initTilePosition;
        startTileUnit = dataBoardManager.GetFromDataBoard(startTilePosition);

        //�����Ԃ�Move�ɂ�����
        sceneManager.operationType = SceneManager.OperationType.Move;

        //�ړ��o�H�ɍŏ��̃^�C���̈ʒu��ǉ�
        pathway = new List<Vector2Int> { startTilePosition };
        //�����������
        MOVE_PROHIBITED = false;


    }


    void Update()
    {
        //�J�[�\�����{�[�h��ɂ��邩�̔���
        isOnBoard = inputManager.mousePositionBoard != physicalBoard.OUTSIDE;
        //�J�[�\��������^�C���Ƀ��j�b�g�����݂��邩�̔���
        unitExists = isOnBoard && dataBoardManager.JudgeExist(inputManager.mousePositionBoard);

        //�J�[�\���̂���^�C��
        tileWithCursor = null;
        //�O��J�[�\�����������^�C��
        tileWithCursorBefore = null;

        //�J�[�\��������^�C���̃��j�b�g
        cursorUnit = null;
        if (unitExists)
        {
            cursorUnit = dataBoardManager.GetFromDataBoard(inputManager.mousePositionBoard);
        }


        //�{�[�h�O�ɏo�����̏���
        if (!isOnBoard)
        {
            EndMove();
            return;
        }

        //�{�[�h���̏���

        tileWithCursor = physicalBoard.GetTile(inputManager.mousePositionBoard);
        tileWithCursorBefore = physicalBoard.GetTile(inputManager.mousePositionBoardBefore);
        
        

        //�E�N���b�N���I������Ƃ�
        if (Input.GetMouseButtonUp(1))
        {
            EndMove();
            //YOCHI �ړ��o�H���o�͂���
            //�ԃ}�X�ŏI�������ꍇ
            if (MOVE_PROHIBITED)
            {
                Debug.Log("end with red tile");
                return;
            }
            //�N���b�N�݂̂������ꍇ
            if(pathway.Count == 1)
            {
                Debug.Log("0 length move path");
                return;
            }

            //MoveHandler�����j�b�g�̎q�I�u�W�F�N�g�Ƃ��Đ��������j�b�g�̍��W�����������Ă��炤
            MoveHandler mover = Instantiate(moveHandler);
            mover.transform.position = startTileUnit.transform.position;
            mover.transform.SetParent(startTileUnit.transform);
            mover.Activate(startTileUnit, pathway);
        }

        if (MOVE_PROHIBITED) return;


        //�E�N���b�N����Ă���Œ�
        #region �E�N���b�N����Ă���Œ�
        if (Input.GetMouseButton(1))
        {
            //�J�[�\�������e�ʒu�ɂ��邩�ŐF��ς���B

            //���j�b�g�����݂��Ȃ��ꍇ!unitExists
            if(Condition1())
            {
                //�J�[�\���̂���^�C���𖾂邭����
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, TileColor.movePointTile);
            }
            //�G���j�b�g�����݂���ꍇcursorUnit.PlayerIndex != startTileUnit.PlayerIndex
            else if (Condition2())
            {
                //�J�[�\���̂���^�C����Ԃ�����
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, TileColor.movePointENEMYUnit);

                //�t���O��true�ɂ���B
                MOVE_PROHIBITED = true;
               
            }
            //�������j�b�g�����݂��E�����ʒu(�������g�j�ł͂Ȃ��ꍇcursorUnit.PlayerIndex == startTileUnit.PlayerIndex && inputManager.mousePositionBoard != startTilePosition
            else if (Condition3())
            {
                //�J�[�\���̂���^�C����Ԃ�����
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, TileColor.movePointMEUnit);
            }
            

            //�J�[�\�����^�C���Ԃ��ړ������ꍇ
            if (inputManager.Displacement())
            {
                //�V�����^�C�����ړ��o�H�ɒǉ������
                pathway.Add(tileWithCursor.PositionBoard);

                //�������J�[�\�����ԃ}�X����Ȃ������ꍇ
                if (tileWithCursorBefore.gameObject.GetComponent<SpriteRenderer>().color != TileColor.movePointENEMYUnit)
                {
                    //�J�[�\�����������^�C���͈Â�����
                    physicalBoard.ChangeTileColor(tileWithCursorBefore.PositionBoard, TileColor.movePassedTile);
                }
                

            }
        }
        #endregion
        

    }

    //�I������
    void EndMove()
    {
        //�^�C���̐F�����ɖ߂��B
        foreach (Vector2Int tileOnPath in pathway)
        {
            physicalBoard.ChangeTileColor(tileOnPath, TileColor.moveOriginal);
        }

        //MoveOperation���A�N�e�B�u������
        this.enabled = false;

        //BasicOperation���A�N�e�B�u������
        basicOperation.enabled = true;
        basicOperation.Activate();
    }
    virtual protected bool Condition1()
    {
        Debug.Log("Condition1 is not overwritten.");
        return false;
    }
    virtual protected bool Condition2()
    {
        Debug.Log("Condition2 is not overwritten.");
        return false;
    }
    virtual protected bool Condition3()
    {
        Debug.Log("Condition3 is not overwritten.");
        return false;
    }
}
