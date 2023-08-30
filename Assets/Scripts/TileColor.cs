using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileColor : MonoBehaviour
{
    //�^�C���̐F���`���܂��B
    //TODO �������g���Ă��邱�Ƃ��悭�Ȃ���������Ȃ�
    //�Q�ƃy�[�W
    //https://www.rapidtables.org/ja/web/color/color-wheel.html
    //https://color.adobe.com/ja/create/color-wheel

    //�g�p����F�B�O������Q�ƕs�B
    static Color original        = new Color32(255, 255, 255, 100);//�D�F�W��
    static Color paleDark        = new Color32(255, 255, 255, 50);//�Â�
    static Color dark            = new Color32(100, 100, 100, 100);//����

    static Color bibidLightBlue  = new Color32(150, 200, 255, 200);//���F�����
    static Color lightBlue       = new Color32(135, 190, 255, 200);//���F

    static Color red             = new Color32(255, 150, 150, 200);//��
    static Color lightMint       = new Color32(149, 254, 255, 240);//���邢�~���g

    //����󋵂��Ƃɂ킯��

    //Basic
    //�W���̐F
    readonly public static Color basicOriginal = original;
    //�J�[�\���̐F�B���j�b�g�����Ȃ��ꍇ�B
    readonly public static Color basicPointTile = paleDark;
    //�J�[�\���̐F�B���j�b�g������ꍇ�B
    readonly public static Color basicPointUnit = bibidLightBlue;

    //Move
    //�W���̐F
    readonly public static Color moveOriginal = original;
    //TODO �ړ���̂̃��j�b�g�̐F
    //�ړ���̂̃��j�b�g�̃^�C���̐F
    readonly public static Color moveMovingUnit;
    //�ړ��o�H�̐F�B
    readonly public static Color movePassedTile = paleDark;
    //�J�[�\���̐F�B
    readonly public static Color movePointTile = bibidLightBlue;
    //TODO�@�����ƓG�̐F�̎���
    //�ړ��o�H�܂��̓J�[�\��������ӏ��ɖ������j�b�g������ꍇ
    readonly public static Color movePointMEUnit = red;
    //�ړ��o�H�܂��̓J�[�\��������ӏ��ɓG���j�b�g������ꍇ
    readonly public static Color movePointENEMYUnit = red;

    //Attack
    //�����W�͈͊O
    readonly public static Color attackRangeOutside = original;
    //�����W�͈͓�
    //�����W�͈͓��̕W���̐F
    readonly public static Color attackRangeInside = paleDark;
    //�U����̂̐F
    readonly public static Color attackAttacker = original;
    //TODO �������j�b�g�@�F
    //�������j�b�g�̐F
    readonly public static Color attackRangeMEUnit;
    //�G���j�b�g�̐F
    readonly public static Color attackRangeENEMYUnit = lightBlue;
    //�J�[�\���̐F�B
    readonly public static Color attackCursorTile = dark;
    //�J�[�\���̐F�B�������j�b�g������ꍇ�B
    readonly public static Color attackCursorMEUnit = red;
    //�J�[�\���̐F�B�G���j�b�g������ꍇ�B
    readonly public static Color attackCursorENEMYUnit = lightMint;

    //Choice
    //�܂��}�E�X�ɂ��^�C�����삪�Ȃ�

}