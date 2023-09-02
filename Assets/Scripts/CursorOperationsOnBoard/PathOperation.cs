using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// �{�[�h���}�E�X����
/// �o�H�̎w��ɗp����B�o�H�w���Ƀ��\�b�h�����s����B
///�E�X�^�[�g�}�X
///�E�Ώۂ̂Ȃ��}�X
///�E�Ώۂ̂���}�X
///�E�ΏۊO�̂���}�X
///�ɋ�ʂ��ĐF��\������B
///�{�[�h�O�ɂłĂ����Ƃ������I�ɏI������B
/// </summary>
public class PathOperation : MonoBehaviour
{

    [SerializeField] protected InputManager inputManager;
    [SerializeField] protected PhysicalBoard physicalBoard;
    [SerializeField] protected DataBoardManager dataBoardManager;
    [SerializeField] protected PrimaryUnitOperator primaryUnitOperator;
    [SerializeField] protected BasicOperation basicOperation;
    [SerializeField] protected SceneManager sceneManager;
    [SerializeField] protected MoveHandler moveHandler;
    [SerializeField] protected TileColor colorTile;

    //�w��o�H
    protected List<Vector2Int> pathway;

    //�N���b�N�����ŏ��̃^�C���̈ʒu
    protected Vector2Int startTilePosition;
    //�ŏ��̃^�C���̈ʒu�̃��j�b�g
    protected UnitGeneral startTileUnit;

    //�����I������
    protected bool ACT_PROHIBITED;

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
    Color original = TileColor.original;

    //Color noneCursor_Self;
    Color noneCursor_None = TileColor.paleDark;
    Color noneCursor_Target = TileColor.lightYelow;
    Color noneCursor_NotApplicable = TileColor.red;
    
    //self��none�Ɠ�������
    Color withCursor_Self = TileColor.bibidLightBlue;
    Color withCursor_None = TileColor.bibidLightBlue;
    Color withCursor_Target = TileColor.lightYelow;
    Color withCursor_NotApplicable = TileColor.red;
    

    //������
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
        ACT_PROHIBITED = false;

    }

    //�J�[�\���ʒu�Ń^�C���̐F��ύX����
    void Update()
    {
        //�J�[�\���̂���}�X
        //�{�[�h��ɂ��邩�̔���
        isOnBoard = inputManager.mousePositionBoard != physicalBoard.OUTSIDE;
        //���j�b�g�����݂��邩�̔���
        unitExists = isOnBoard && dataBoardManager.JudgeExist(inputManager.mousePositionBoard);

        //�J�[�\���̂���^�C��
        tileWithCursor = null;
        //�O��J�[�\�����������^�C��
        tileWithCursorBefore = null;

        //�J�[�\��������^�C���̃��j�b�g
        if (unitExists)
        {
            cursorUnit = dataBoardManager.GetFromDataBoard(inputManager.mousePositionBoard);
        }


        //�{�[�h�O�ɃJ�[�\�����o���ꍇ
        //�I������
        if (!isOnBoard)
        {
            EndPathOperation();
            return;
        }


        //�{�[�h��

        tileWithCursor = physicalBoard.GetTile(inputManager.mousePositionBoard);
        tileWithCursorBefore = physicalBoard.GetTile(inputManager.mousePositionBoardBefore);
        
        

        //�E�N���b�N���I������Ƃ�
        if (Input.GetMouseButtonUp(1))
        {
            EndPathOperation();
            //YOCHI �ړ��o�H���o�͂���

            //�s���֎~�����𖞂����Ă���ꍇ
            if (ACT_PROHIBITED)
            {
                Debug.Log("prohibited path");
                return;
            }
            //�N���b�N�݂̂������ꍇ
            if(pathway.Count == 1)
            {
                Debug.Log("0 length path");
                return;
            }
            //�ړI�ƂȂ郁�\�b�h�����s����
            Operate();
        }

        //�֎~���쎞�͈ȉ��̃N���b�N�r���̔�����s��Ȃ�
        if (ACT_PROHIBITED) return;


        #region �E�N���b�N����Ă���Œ�
        if (Input.GetMouseButton(1))
        {
            //�J�[�\���̈ʒu�̐F

            //�����ʒu�̏ꍇ
            if(inputManager.mousePositionBoard == startTilePosition)
            {
                //���邭����
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, withCursor_Self);
            }
            //�Ώۂ����݂��Ȃ��ꍇ
            else if(Condition1())
            {
                //���邭����
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, withCursor_None);
            }
            //�Ώۂ����݂���ꍇ
            else if (Condition2())
            {
                //���F������
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, withCursor_Target);

                //�t���O��true�ɂ���B
                ACT_PROHIBITED = true;
               
            }
            //���O�Ώۂ̏ꍇ
            else if (Condition3())
            {
                //�Ԃ�����
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, withCursor_NotApplicable);
            }
            

            //�J�[�\�����^�C���Ԃ��ړ������ꍇ
            if (inputManager.Displacement())
            {
                //�V�����^�C�����ړ��o�H�ɒǉ������
                pathway.Add(tileWithCursor.PositionBoard);

                ////�������J�[�\�����s���I���}�X����Ȃ������ꍇ
                //if (tileWithCursorBefore.gameObject.GetComponent<SpriteRenderer>().color != TileColor.movePointENEMYUnit)
                //{
                //    //�J�[�\�����������^�C���͈Â�����
                //    physicalBoard.ChangeTileColor(tileWithCursorBefore.PositionBoard, TileColor.movePassedTile);
                //}
                
                //�߂����^�C���̐F����Ԃɉ����Ė߂��B
                //�����ł͐F�����邢�̏ꍇ�̂ݖ߂��B
                if(tileWithCursorBefore.gameObject.GetComponent<SpriteRenderer>().color == withCursor_None)
                {
                    physicalBoard.ChangeTileColor(tileWithCursorBefore.PositionBoard, noneCursor_None);
                }
            }
        }
        #endregion
        

    }

    //�I������
    void EndPathOperation()
    {
        //�^�C���̐F�����ɖ߂��B
        foreach (Vector2Int tileOnPath in pathway)
        {
            physicalBoard.ChangeTileColor(tileOnPath, TileColor.moveOriginal);
        }

        //PathOperation���A�N�e�B�u�ɂ���
        this.enabled = false;
        //���̑�����@���A�N�e�B�u�ɂ���
        ActivateNextOperation();
    }

    /// <summary>
    /// ���ɑ����Activate
    /// enabled��true�ɂ��AActivate()�����s����
    /// </summary>
    virtual protected void ActivateNextOperation()
    {
        Debug.Log(nameof(ActivateNextOperation) + " is not overriden");
    }

    /// <summary>
    /// �Ώۂ����Ȃ��^�C���̏���
    /// </summary>
    virtual protected bool Condition1()
    {
        Debug.Log(nameof(Condition1) + " is not overriden.");
        return false;
    }

    /// <summary>
    /// �Ώۂ�����^�C���̏���
    /// </summary>
    /// <returns></returns>
    virtual protected bool Condition2()
    {
        Debug.Log(nameof(Condition2) + " is not overriden.");
        return false;
    }

    /// <summary>
    /// �ΏۊO�ł���^�C���̏���
    /// </summary>
    /// <returns></returns>
    virtual protected bool Condition3()
    {
        Debug.Log(nameof(Condition3) + " is not overriden.");
        return false;
    }
    /// <summary>
    /// �o�H�w���A���s���郁�\�b�h
    /// </summary>
    virtual protected void Operate()
    {
        Debug.Log(nameof(Operate) + " is not overriden.");
    }
}
