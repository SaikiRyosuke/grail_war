using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandler : MonoBehaviour
{
    //�e��Ǘ��X�N���v�g
    GameObject controller;
    DataBoardManager dataBoardManager;

    //�ړ��Ώۂ̃��j�b�g�B�ړ��o�H�B
    UnitGeneral movingUnit = null;
    List<Vector2Int> movePath;
    //�ړ��o�H��̃}�[�J�[
    //�ړ��o�H�̂������Ԗڂ̃^�C���ɂ��邩������
    int moveIndex;

    //�J��Ԃ������̃X�p���E�o�ߎ���
    public float span;
    private float currentTime;

    //���������\�b�h
    public void Activate(UnitGeneral unit, List<Vector2Int> moveRoot)
    {
        //�ǂݍ���
        controller = GameObject.FindGameObjectWithTag(Methods.CONTROLLER);
        dataBoardManager = controller.GetComponent<DataBoardManager>();

        //������
        movingUnit = unit;
        movePath = moveRoot;
        moveIndex = 1;

        span = 1f;
        currentTime = 0f;

        //�Ώۃ��j�b�g��isMoveing��true�ɂ���
        movingUnit.IsMoving = true;
        //���̃I�u�W�F�N�g����ω�������
        this.gameObject.name = "MoveHandler";

        //movingUnit.UseSkill();
    }

    

    void Update()
    {
        //span�b���ƂɌJ��Ԃ�
        currentTime += Time.deltaTime;
        if (currentTime > span)
        {
            
            //1��O�̌J��Ԃ����̃��j�b�g�̈ʒu
            Vector2Int unitBefore = movingUnit.Position;
            //����̌J��Ԃ����̃��j�b�g�̈ʒu
            Vector2Int unitNow = movePath[moveIndex];
            //unitNow�ɂ����郆�j�b�g�̗L��
            bool unitExists = dataBoardManager.JudgeExist(unitNow);


            //unitNow�ɖ���������ꍇ
            if(unitExists && dataBoardManager.GetFromDataBoard(unitNow).PlayerIndex == Methods.PLAYER_ME)
            {
                //�������ɏI������
                Destroy(this.gameObject);

                //���j�b�g��IsMoving��false�ɂ���
                movingUnit.IsMoving = false;

                return;
            }
            //unitNow�ɓG������ꍇ
            else if(unitExists && dataBoardManager.GetFromDataBoard(unitNow).PlayerIndex == Methods.PLAYER_ENEMY)
            {
                //TODO Move���I�����Đ퓬�ɂ͂���
                Destroy(this.gameObject);

                //���j�b�g��IsMoving��false�ɂ���
                movingUnit.IsMoving = false;

                return;
            }



            //���j�b�g�̈ʒu��ύX����
            movingUnit.Position = unitNow;
            //�{�[�h��ł̃��j�b�g�ʒu�̕ύX
            dataBoardManager.MoveUnit(movingUnit, unitBefore);


            //�o�H�̏I������
            moveIndex++;
            if( moveIndex >= movePath.Count)
            {
                Destroy(this.gameObject);

                //���j�b�g��IsMoving��false�ɂ���
                movingUnit.IsMoving = false;
            }

            //�o�ߎ��Ԃ�0�ɖ߂��ČJ��Ԃ�
            currentTime = 0f;
        }
    }
}
