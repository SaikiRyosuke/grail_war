using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// �S�Ẵ��j�b�g�ɋ��ʂɂ���f�[�^
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

    [SerializeField] GameObject hpBar;

    //�u�������v
    //TOASK public private�{���ɕ�����Ȃ��B
    /// <summary>
    /// ������������unit��id�Ƃ����̂͌ʂɌ��܂��ĂĂ��ĊO������͓ǂݎ�肵���ł��Ȃ��悤�ɂ������B
    /// US000.cs��readonly�ŏ����̂���Ԗ���H
    /// �C���X�y�N�^�̓f�[�^���������Ŏg�������Ȃ�
    /// US000�ŏ�����������
    /// </summary>
    //���̃��j�b�g��ID
    public string UnitID { get; set; } = null;
    //�\�͏����l
    protected float INIT_HP;
    //�\�͒l
    //�U���˒�
    public float atkRange;
    //�̗�
    public float HP { get; set; } = 0;
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

        this.gameObject.transform.position = Methods.BoardToUnitDisplay(Position);

        DisplayHPBar();
        //UpdateInner();

    }
    virtual protected void UpdateInner(){
        // ��������Update�^�C�~���O�ł�肽���Ƃ������Ƃ��p����ł����ɏ���
    }
    
    // �ŗL�\�͂��g�p�A���s�����Ƃ�false��Ԃ�
    virtual public bool UseSkill()
    {
        // �p�������N���X�œ��e���L�q
        //throw new NotImplementedException();
        return false;
    }
    
    //HP���Q�[�W�ɔ��f
    private bool DisplayHPBar()
    {
        float leftHPScale;
        if(HP >= 0)
        {
            leftHPScale = HP / INIT_HP;
        }
        else
        {
            leftHPScale = 0;
            Debug.Log("HP less than 0");
        }

        Vector3 scale = hpBar.transform.localScale;
        scale.x = leftHPScale;
        hpBar.transform.localScale = scale;
        Vector3 position = hpBar.transform.parent.position;
        //magic number
        position.x -= 0.75f * (1 - leftHPScale) / 2;
        hpBar.transform.position = position;

        return true;
    }
}
