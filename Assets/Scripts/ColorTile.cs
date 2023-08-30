using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTile : MonoBehaviour
{
    [SerializeField] PhysicalBoard physicalBoard;

    //�^�C���̐F���`���܂��B
    //�������g���Ă��邱�Ƃ��悭�Ȃ���������Ȃ�
    //�Q�ƃy�[�W
    //https://www.rapidtables.org/ja/web/color/color-wheel.html
    //https://color.adobe.com/ja/create/color-wheel
    /*
    �D�F�W��     = new Color32(255, 255, 255, 100);
    �Â�         = new Color32(255, 255, 255, 50);
    ����         = new Color32(100, 100, 100, 100);

    ���F�����   = new Color32(150, 200, 255, 200);
    ���F         = new Color32(135, 190, 255, 200);

    ��           = new Color32(255, 150, 150, 200);
    ���邢�~���g = new Color32(149, 254, 255, 240);
    */

    //����󋵂��Ƃɂ킯��

    //Basic
    //�W���̐F
    public static Color basicOriginal = new Color32(255, 255, 255, 100);
    //�J�[�\���̐F�B���j�b�g�����Ȃ��ꍇ�B
    public static Color basicPointTile = new Color32(255, 255, 255, 50);
    //�J�[�\���̐F�B���j�b�g������ꍇ�B
    public static Color basicPointUnit = new Color32(150, 200, 255, 200);



    //Move
    //�W���̐F
    public static Color moveOriginal = new Color32(255, 255, 255, 100);
    //TODO �ړ���̂̃��j�b�g�̐F
    //�ړ���̂̃��j�b�g�̃^�C���̐F
    public static Color moveMovingUnit;
    //�ړ��o�H�̐F�B
    public static Color movePassedTile = new Color32(255, 255, 255, 50);
    //�J�[�\���̐F�B
    public static Color movePointTile = new Color32(150, 200, 255, 200);
    //TODO�@�����ƓG�̐F�̎���
    //�ړ��o�H�܂��̓J�[�\��������ӏ��ɖ������j�b�g������ꍇ
    public static Color movePointMEUnit = new Color32(255, 150, 150, 200);
    //�ړ��o�H�܂��̓J�[�\��������ӏ��ɓG���j�b�g������ꍇ
    public static Color movePointENEMYUnit = new Color32(255, 150, 150, 200);

    //Attack
    //�����W�͈͊O
    public static Color attackOutside = new Color32(255, 255, 255, 100);
    //�����W�͈͓�
    //�����W�͈͓��̕W���̐F
    public static Color attackInside = new Color32(255, 255, 255, 50);
    //�U����̂̐F
    public static Color attacker = new Color32(150, 200, 255, 200);
    //TODO �������j�b�g�@�F
    //�������j�b�g�̐F
    public static Color attackIndicateMEUnit;
    //�G���j�b�g�̐F
    public static Color attackIndicateENEMYUnit = new Color32(135, 190, 255, 200);
    //�J�[�\���̐F�B
    public static Color attackPointTile = new Color32(100, 100, 100, 100);
    //�J�[�\���̐F�B�������j�b�g������ꍇ�B
    public static Color attackPointMEUnit = new Color32(255, 150, 150, 200);
    //�J�[�\���̐F�B�G���j�b�g������ꍇ�B
    public static Color attackPointENEMYUnit = new Color32(149, 254, 255, 240);

    //Choice
    //�܂��}�E�X�ɂ��^�C�����삪�Ȃ�

    //�F�̕ύX���\�b�h
    public bool ChangeTileColor(Vector2Int position, Color color)
    {
        physicalBoard.GetTile(position).GetComponent<SpriteRenderer>().color = color;
        return true;
    }

    public void DyeAllTilesTo(Color color)
    {
        for (int i = 0; i < Methods.TILE_X; i++)
        {
            for (int j = 0; j < Methods.TILE_Y; j++)
            {
                physicalBoard.GetTile(new Vector2Int(i, j)).gameObject.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
}
