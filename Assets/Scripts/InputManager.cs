using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// �}�E�X����̓��͂��Ǘ����܂��B
/// 
/// ���͒l�͂�����ł��ς����邽��public�ɂȂ��Ă��܂��B
/// </summary>
public class InputManager : MonoBehaviour
{
    //�Q��
    //PhysicalBoard. �{�[�h���ɂȂ��ꍇ�̃}�E�X���͂𔻒肷��
    [SerializeField] PhysicalBoard physicalBoard;

    //�}�E�X�̍��W
    //���j�b�g���W
    public Vector2 mousePositionUnit; //������2�ʂ܂ŕ\������


    //�{�[�h���W

    //�{�[�h�̊O���ł�physicalBoard��readonly�Ő錾�����OUTSIDE 
    //�����l�����l�Ɂ@OUTSIDE
    public Vector2Int mousePositionBoard;

    //1�t���[���O�̃{�[�h���W
    public Vector2Int mousePositionBoardBefore;
    //�{�[�h���W�̕ψʂ𔻒�
    //PhysicalBoard����J�[�\���̗L���𔻒肷�邱�Ƃ���߂��̂ł���Ȃ�����
    public bool Displacement()
    {
        if (mousePositionBoard == mousePositionBoardBefore)return false;
        return true;
    }


    void Start()
    {
        mousePositionBoard = mousePositionBoardBefore = physicalBoard.OUTSIDE;
    }

   

    void Update()
    {
        //1�t���[���O�̃{�[�h���W����
        mousePositionBoardBefore = mousePositionBoard;


        //�}�E�X������͂��ꂽ���W��2�����x�N�g���ɕϊ�����
        Vector2 mouse = Input.mousePosition;

        //���j�b�g��̍��W�ɕϊ�����B
        Vector2 mouseU = Camera.main.ScreenToWorldPoint(mouse);
        //������2�ʂɊۂ߂�B�i�l�̌ܓ��j
        float mousePositionUnitX = Mathf.Round(mouseU.x * 100) / 100;
        float mousePositionUnitY = Mathf.Round(mouseU.y * 100) / 100;

        //���j�b�g���W
        mousePositionUnit = new Vector2(mousePositionUnitX, mousePositionUnitY);

        //�{�[�h���W�B
        mousePositionBoard = Methods.UnitToBoardVector(mouseU);
        //�{�[�h�O���ł�OUTSIDE�ɂ���B
        if(!physicalBoard.JudgeOnBoard(mousePositionBoard))
        {
            mousePositionBoard = physicalBoard.OUTSIDE;
        }

       
    }
}
