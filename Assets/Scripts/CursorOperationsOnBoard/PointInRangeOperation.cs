
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInRangeOperation : MonoBehaviour
{
    //�Q��
    [SerializeField] SceneManager sceneManager;
    [SerializeField] BasicOperation basicOperation;
    [SerializeField] ChoiceOperation choiceOperation;
    [SerializeField] PhysicalBoard physicalBoard;
    [SerializeField] InputManager inputManager;
    [SerializeField] DataBoardManager dataBoardManager;
    [SerializeField] TileColor colorTile;

    //�ŏ��ɑI�΂ꂽ�^�C��
    protected Vector2Int startTilePosition;
    //���̃��j�b�g
    protected UnitGeneral startTileUnit;

    //�����W
    float range;
    //�͈͓��̃^�C���̈ʒu�G
    List<Vector2Int> InRangePoisitions = new List<Vector2Int>();
    //�͈͓��̖������j�b�g�̈ʒu
    List<Vector2Int> myPositions;
    //�͈͓��̓G���j�b�g�̈ʒu
    List<Vector2Int> enemyPositions;

    //�J�[�\�����{�[�h��ɂ��邩�̔���
    protected bool isOnBoard = false;
    //�J�[�\���������W���ɂ��邩�̔���
    protected bool isInRange = false;
    //�J�[�\��������^�C���Ƀ��j�b�g�����݂��邩�̔���
    protected bool unitExists = false;
    //�J�[�\��������^�C���̃��j�b�g
    protected UnitGeneral cursorUnit = null;
    //�J�[�\���̂���^�C��
    protected Tile tileWithCursor = null;

    //�F�̃p�^�[������
    Color original = TileColor.original;
    Color inside = TileColor.paleDark;

    Color noneCursor_Self = TileColor.original;
    Color noneCursor_None = TileColor.paleDark;
    Color noneCursor_Target = TileColor.lightYelow;
    //Color noneCursor_NotApplicable = TileColor.red;

    //self��none�Ɠ�������
    Color withCursor_Self = TileColor.lightBlue;
    Color withCursor_None = TileColor.dark;
    Color withCursor_Target = TileColor.orange;
    //Color withCursor_NotApplicable = TileColor.red;

    public void Activate(Vector2Int firstTilePosition, float rangeFromFirstTile)
    {
        //SceneManager�ɑ���󋵂�`����
        sceneManager.operationType = SceneManager.OperationType.Attack;

        //�^�C���̐F�����ɖ߂�
        physicalBoard.DyeAllTilesTo(TileColor.attackRangeOutside);

        //�͈͂�������
        InRangePoisitions = new List<Vector2Int>();
        myPositions = new List<Vector2Int>();
        enemyPositions = new List<Vector2Int>();
        //�I�΂ꂽ�^�C���̍��W���擾
        startTilePosition = firstTilePosition;
        //�U����̂̃��j�b�g���擾
        startTileUnit = dataBoardManager.GetFromDataBoard(firstTilePosition);
        //�͈͂��擾
        range = rangeFromFirstTile;
        //�͈͂����X�g�ɂ���
        foreach(Vector2Int tilePosition in dataBoardManager.GetRangeWithRangeNumberAndPositions(range, startTilePosition))
        {
            InRangePoisitions.Add(tilePosition);
        }

    }

    void Update()
    {
        //Range���̓G�𖾎�
        foreach (Vector2Int tilePosition in InRangePoisitions)
        {
            if (dataBoardManager.JudgeExist(tilePosition) && dataBoardManager.GetFromDataBoard(tilePosition).PlayerIndex != startTileUnit.PlayerIndex)
            {
                enemyPositions.Add(tilePosition);
                physicalBoard.GetTile(tilePosition).gameObject.GetComponent<SpriteRenderer>().color = TileColor.attackRangeENEMYUnit;
            }
            else
            {
                myPositions.Add(tilePosition);
                physicalBoard.GetTile(tilePosition).gameObject.GetComponent<SpriteRenderer>().color = TileColor.attackRangeInside;
            }

        }
        //�J�[�\�����{�[�h��ɂ��邩�̔���
        isOnBoard = inputManager.mousePositionBoard != physicalBoard.OUTSIDE;
        //�J�[�\���������W���ɂ��邩�̔���
        isInRange = InRangePoisitions.Contains(inputManager.mousePositionBoard);
        //�J�[�\��������^�C���Ƀ��j�b�g�����݂��邩�̔���
        unitExists = isOnBoard && dataBoardManager.JudgeExist(inputManager.mousePositionBoard);
        //�J�[�\��������^�C���̃��j�b�g
        UnitGeneral cursorUnit = null;
        if (unitExists)
        {
            cursorUnit = dataBoardManager.GetFromDataBoard(inputManager.mousePositionBoard);
        }
        //�J�[�\���̂���^�C��
        Tile tileWithCursor = null;
        if (isOnBoard)
        {
            tileWithCursor = physicalBoard.GetTile(inputManager.mousePositionBoard);
        }

        if(startTilePosition != startTileUnit.Position)
        {
            Activate(startTileUnit.Position, range);
            return;
        }

        //���N���b�N���I������Ƃ�
        if (Input.GetMouseButtonUp(0))
        {
            //�^�C���̐F�����ɖ߂�
            foreach (Vector2Int tilePosition in InRangePoisitions)
            {
                physicalBoard.ChangeTileColor(tilePosition, TileColor.attackRangeOutside);
            }

            //�͈͊O�ŗ��ꂽ�ꍇ
            if (!isInRange)
            {
                //BasicOperation�ɖ߂�
                EndAttackBeginBasic();
            }
            //�{�[�h���A�͈͓��A�����ʒu�ŗ��ꂽ�ꍇ
            else if (inputManager.mousePositionBoard == startTilePosition)
            {
                //ChoiceOperation�ɂ�����
                EndAttackBeginChoice();
            }

            //�{�[�h���A�͈͓��A�����ʒu�ȊO�A���j�b�g�Ȃ��ŗ��ꂽ�ꍇ
            else if (!unitExists)
            {
                //BasicOperation�ɖ߂�
                EndAttackBeginBasic();
            }

            //�{�[�h���A�͈͓��A�����ʒu�ȊO�A���j�b�g����ŗ��ꂽ�ꍇ
            else if (unitExists)
            {
                //BasicOperation�ɖ߂�
                EndAttackBeginBasic();
            }
            return;
        }
        //�ŏ��̃^�C���������邭����
        physicalBoard.ChangeTileColor(startTilePosition, TileColor.attackAttacker);


        

        //�J�[�\��������^�C�����ω�������ړ��O�̃^�C�������̐F�ɂ��ǂ�
        //�{�[�h�O����N�������ꍇ������
        if (inputManager.Displacement() && InRangePoisitions.Contains(inputManager.mousePositionBoardBefore))
        {
            //YOCHI �����̐F
            if(enemyPositions.Contains(inputManager.mousePositionBoardBefore))
            {
                physicalBoard.GetTile(inputManager.mousePositionBoardBefore).gameObject.GetComponent<SpriteRenderer>().color = TileColor.attackRangeENEMYUnit;
            }
            else
            {
                //physicalBoard.DarkenTile(inputManager.mousePositionBoardBefore);
                physicalBoard.ChangeTileColor(inputManager.mousePositionBoardBefore, TileColor.attackRangeInside);
            }
            
        }

        if (!isInRange) return;
        //���N���b�N����Ă���Œ�
        if (Input.GetMouseButton(0))
        {
            if (Condition1())
            {
                tileWithCursor.gameObject.GetComponent<SpriteRenderer>().color = TileColor.attackCursorTile;
            }
            else if (Condition3())
            {
                tileWithCursor.gameObject.GetComponent<SpriteRenderer>().color = TileColor.attackCursorMEUnit;
            }
            else if (Condition2())
            {
                tileWithCursor.gameObject.GetComponent<SpriteRenderer>().color = TileColor.attackCursorENEMYUnit;
            }
        }
        
    }
    void EndAttackBeginBasic()
    {

        //AttackOperation���A�N�e�B�u������
        this.enabled = false;

        //BasicOperation���A�N�e�B�u������
        basicOperation.enabled = true;
        basicOperation.Activate();
    }void EndAttackBeginChoice()
    {

        //AttackOperation���A�N�e�B�u������
        this.enabled = false;

        //ChoiceOperation���A�N�e�B�u������
        choiceOperation.enabled = true;
        choiceOperation.Activate(inputManager.mousePositionBoard);
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
    /// ���s���郁�\�b�h
    /// </summary>
    virtual protected void Operate()
    {
        Debug.Log(nameof(Operate) + " is not overriden.");
    }
}
