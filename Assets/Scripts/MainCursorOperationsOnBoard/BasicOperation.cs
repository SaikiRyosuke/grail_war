using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicOperation : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    [SerializeField] PhysicalBoard physicalBoard;
    [SerializeField] DataBoardManager dataBoardManager;
    [SerializeField] MainMoveOperation mainMoveOperation;
    [SerializeField] MainAttackOperation attackOperation;
    [SerializeField] SceneManager sceneManager;
    [SerializeField] TileColor colorTile;

    [SerializeField] PathOperation pathOperation;


    public void Activate()
    {
        //SceneManager�ɑ���󋵂�`����
        sceneManager.operationType = SceneManager.OperationType.Basic;   
    }
    
    void Update()
    {
        //�J�[�\�����{�[�h��ɂ��邩�̔���
        bool isOnBoard = inputManager.mousePositionBoard != physicalBoard.OUTSIDE;
        //�J�[�\��������^�C���Ƀ��j�b�g�����݂��邩�̔���
        bool unitExists = isOnBoard && dataBoardManager.JudgeExist(inputManager.mousePositionBoard);
        //�J�[�\��������^�C���̃��j�b�g
        UnitGeneral cursorUnit = null;
        if(unitExists)
        {
            cursorUnit = dataBoardManager.GetFromDataBoard(inputManager.mousePositionBoard);
        }


        //�J�[�\��������^�C�����ω�������ړ��O�̃^�C�������̐F�ɂ��ǂ�
        //�{�[�h�O����N�������ꍇ������
        if (inputManager.Displacement() && inputManager.mousePositionBoardBefore != physicalBoard.OUTSIDE)
        {
            physicalBoard.ChangeTileColor(inputManager.mousePositionBoardBefore, TileColor.basicOriginal);
        }

        //�J�[�\��������^�C���̐F��ω�������(�J�[�\�����{�[�h��ɂȂ��Ƃ��͂Ȃɂ����Ȃ�)
        if (isOnBoard)  
        {
            //�^�C���Ƀ��j�b�g�����݂���Ƃ��@���邭����
            //�^�C���Ƀ��j�b�g�����݂��Ȃ��Ƃ��@�Â�����
            if (unitExists)
            {
                physicalBoard.ChangeTileColor(inputManager.mousePositionBoard, TileColor.basicPointUnit);
            }
            else
            {
                physicalBoard.ChangeTileColor(inputManager.mousePositionBoard, TileColor.basicPointTile);
            }
        }

        //���j�b�g�����Ȃ��Ȃ�N���b�N�̔�����s��Ȃ�
        if (!unitExists) return;


        //�E�N���b�N
        //BasicOperation�@���@MoveOperation
        //���ɑΏۃ��j�b�g�������Ă���ꍇ
        //BasicOperation �̂܂܁@Move���L�����Z��
        if (Input.GetMouseButtonDown(1))
        {
            //���j�b�g�����ɓ����Ă���Ƃ�
            if(cursorUnit.IsMoving)
            {
                //���j�b�g�̎q�I�u�W�F�N�g��MoveHandler��j��B
                GameObject mover = cursorUnit.transform.Find("MoveHandler").gameObject;
                Destroy(mover);
                cursorUnit.IsMoving = false;
            }
            //TEMPORARY �J���ғI�ɂ͑���L�����������������̂ŁB

            //���j�b�g�������Ă��Ȃ��Ƃ�
            else
            {
                //BasciOperation���A�N�e�B�u���EMoveOperation���A�N�e�B�u���E������
                this.enabled = false;
                //��xmoveOoperation��practice�ɂ���
                mainMoveOperation.enabled = true;
                mainMoveOperation.Activate(inputManager.mousePositionBoard);
                //pathOperation.enabled = true;
                //pathOperation.Activate(inputManager.mousePositionBoard);
            }
        }

        //���N���b�N
        //BasicOperation ���@AttackOperation
        if (Input.GetMouseButtonDown(0))
        {
            //BasciOperation���A�N�e�B�u���EAttackOperation���A�N�e�B�u���E������
            this.enabled = false;
            attackOperation.enabled = true;

            UnitGeneral clickedUnit = dataBoardManager.GetFromDataBoard(inputManager.mousePositionBoard);
            attackOperation.Activate(inputManager.mousePositionBoard, clickedUnit.atkRange );
        }

        //�f�o�b�O�p�@
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            this.enabled = false;
        }
    }
}
