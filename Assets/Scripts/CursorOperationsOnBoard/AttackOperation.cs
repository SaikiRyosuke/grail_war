
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOperation : MonoBehaviour
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
    Vector2Int firstPosition;
    //���̃��j�b�g
    UnitGeneral attacker;

    //�U���͈͂̃^�C���̈ʒu�G
    List<Vector2Int> Range = new List<Vector2Int>();
    //�U���͈̖͂������j�b�g�̈ʒu
    List<Vector2Int> myPositions;
    //�U���͈͂̓G���j�b�g�̈ʒu
    List<Vector2Int> enemyPositions;

    Color tileOriginal = new Color32(255, 255, 255, 100);//�W��
    Color tileRange = new Color32(255, 255, 255, 50);//�U���͈͂̐F
    Color tileAttackable = new Color32(135, 190, 255, 200);//�U���\�̐F
    Color tileMouseDark = new Color32(100, 100, 100, 100);//�U���͈͒��̃}�E�X�̐F
    Color tileMouseAttackable = new Color32(149, 254, 255, 240);//�U���\���̃}�E�X�̐F
    Color tileRed = new Color32(255, 150, 150, 50);

    public void Activate(Vector2Int firstTilePosition)
    {
        //SceneManager�ɑ���󋵂�`����
        sceneManager.operationType = SceneManager.OperationType.Attack;

        //�^�C���̐F�����ɖ߂�
        physicalBoard.DyeAllTilesTo(TileColor.attackRangeOutside);

        //�U���͈͂�������
        Range = new List<Vector2Int>();
        myPositions = new List<Vector2Int>();
        enemyPositions = new List<Vector2Int>();
        //�I�΂ꂽ�^�C���̍��W���擾
        firstPosition = firstTilePosition;
        //�U����̂̃��j�b�g���擾
        attacker = dataBoardManager.GetFromDataBoard(firstTilePosition);

        //�U���͈͂����X�g�ɂ���
        foreach(Vector2Int tilePosition in dataBoardManager.AttackablePositionsWithUnit(attacker))
        {
            Range.Add(tilePosition);
        }

    }
    // Update is called once per frame
    void Update()
    {
        //Range���̓G�𖾎�
        foreach (Vector2Int tilePosition in Range)
        {
            if (dataBoardManager.JudgeExist(tilePosition) && dataBoardManager.GetFromDataBoard(tilePosition).PlayerIndex != attacker.PlayerIndex)
            {
                enemyPositions.Add(tilePosition);
                physicalBoard.GetTile(tilePosition).gameObject.GetComponent<SpriteRenderer>().color = tileAttackable;
            }
            else
            {
                myPositions.Add(tilePosition);
                physicalBoard.GetTile(tilePosition).gameObject.GetComponent<SpriteRenderer>().color = tileRange;
            }

        }
        //�J�[�\�����{�[�h��ɂ��邩�̔���
        bool isOnBoard = inputManager.mousePositionBoard != physicalBoard.OUTSIDE;
        //�J�[�\���������W���ɂ��邩�̔���
        bool isInRange = Range.Contains(inputManager.mousePositionBoard);
        //�J�[�\��������^�C���Ƀ��j�b�g�����݂��邩�̔���
        bool unitExists = isOnBoard && dataBoardManager.JudgeExist(inputManager.mousePositionBoard);
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

        if(firstPosition != attacker.Position)
        {
            Activate(attacker.Position);
            return;
        }

        //���N���b�N���I������Ƃ�
        if (Input.GetMouseButtonUp(0))
        {
            //�^�C���̐F�����ɖ߂�
            foreach (Vector2Int tilePosition in Range)
            {
                //physicalBoard.OriginateTileColor(tilePosition);
                physicalBoard.ChangeTileColor(tilePosition, TileColor.attackRangeOutside);
            }

            ////�{�[�h�O�ŗ��ꂽ�ꍇ
            //if (!isOnBoard)
            //{
            //    //BasicOperation�ɖ߂�
            //    EndAttackBeginBasic();
            //}
            //�{�[�h���A�U���͈͊O�ŗ��ꂽ�ꍇ
            if (!isInRange)
            {
                //BasicOperation�ɖ߂�
                EndAttackBeginBasic();
            }
            //�{�[�h���A�U���͈͓��A�����ʒu�ŗ��ꂽ�ꍇ
            else if (inputManager.mousePositionBoard == firstPosition)
            {
                //ChoiceOperation�ɂ�����
                EndAttackBeginChoice();
            }

            //�{�[�h���A�U���͈͓��A�����ʒu�ȊO�A���j�b�g�Ȃ��ŗ��ꂽ�ꍇ
            else if (!unitExists)
            {
                //BasicOperation�ɖ߂�
                EndAttackBeginBasic();
            }

            //�{�[�h���A�U���͈͓��A�����ʒu�ȊO�A���j�b�g����ŗ��ꂽ�ꍇ
            else if (unitExists)
            {
                //BasicOperation�ɖ߂�
                EndAttackBeginBasic();
            }
            return;
        }
        //�ŏ��̃^�C���������邭����
        physicalBoard.ChangeTileColor(firstPosition, TileColor.attackAttacker);


        

        //�J�[�\��������^�C�����ω�������ړ��O�̃^�C�������̐F�ɂ��ǂ�
        //�{�[�h�O����N�������ꍇ������
        if (inputManager.Displacement() && Range.Contains(inputManager.mousePositionBoardBefore))
        {
            //YOCHI �����̐F
            if(enemyPositions.Contains(inputManager.mousePositionBoardBefore))
            {
                physicalBoard.GetTile(inputManager.mousePositionBoardBefore).gameObject.GetComponent<SpriteRenderer>().color = tileAttackable;
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
            if (!unitExists)
            {
                tileWithCursor.gameObject.GetComponent<SpriteRenderer>().color = tileMouseDark;
            }
            else if (cursorUnit.PlayerIndex == attacker.PlayerIndex)
            {
                tileWithCursor.gameObject.GetComponent<SpriteRenderer>().color = tileRed;
            }
            else if (cursorUnit.PlayerIndex != attacker.PlayerIndex)
            {
                tileWithCursor.gameObject.GetComponent<SpriteRenderer>().color = tileMouseAttackable;
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
}
