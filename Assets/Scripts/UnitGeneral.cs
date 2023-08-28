using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// ���I�u�W�F�N�g����̌Ăяo���ɉ����邱�Ƃ��ł��镁�ՓI�ȃf�[�^
/// </summary>
public class UnitGeneral : MonoBehaviour
{
    //�Q��
    //�e��Ǘ��X�N���v�g
    GameObject controller;
    InputManager inputManager;
    MoveOperation moveOperation;
    PhysicalBoard physicalBoard;
    DataBoardManager dataBoardManager;


    //�u�������v
    //���̃��j�b�g��ID
    public string UnitID { get; set; }
    //�\�͒l
    public float atkRange;

    //�u�O�����v
    //(�ÓI���j
    //���̃��j�b�g�̏��L�v���C���[
    public int PlayerIndex { get; set; }

    //(���I���j
    //���̃��j�b�g�̃{�[�h���W
    public Vector2Int Position { get; set; }
    //���̃��j�b�g�̉摜�\���̑O��֌W�iorder)
    public int DisplayOrder { get; set; } = 0;
    //���̃��j�b�g��Move��Ԃł��邩
    public bool IsMoving { get; set; } = false;
    //���̃��j�b�g��Attack��Ԃł��邩
    public bool IsAttacking { get; set; } = false;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag(Methods.CONTROLLER);
        inputManager = controller.GetComponent<InputManager>();
        moveOperation = controller.GetComponent<MoveOperation>();

    }

    private void Update()
    {
        //���j�b�g�Ԃ̉摜�̑O��֌W��y���W�Ɉˑ����Ă��߂� 
        DisplayOrder = Methods.TILE_Y - Position.y - 1;
        this.gameObject.GetComponent <SortingGroup>().sortingOrder = DisplayOrder;
        //()  .sortingOrder = DisplayOrder;

        this.gameObject.transform.position = Methods.BoardToUnitDisplay(Position);
        
    }
}
