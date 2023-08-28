using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// static�ȃ��\�b�h��萔�⃊�e�������Ȃ�ׂ��W�߂܂����B
/// static�̐錾�͂��̃X�N���v�g�ɂ�������܂���B
/// </summary>
public class Methods : MonoBehaviour
{
    //�Q�[���{�[�h�̑傫���̍ő�l�i���j�b�g�j
    public const int GAMEBOARD_MAX = 8;
    //�}�X�ڂ̐��B����8*8;
    public const int TILE_X = 8;
    public const int TILE_Y = 8;

    //�v���C���[�̎��ʔԍ�
    public const int PLAYER_ME = 0;
    public const int PLAYER_ENEMY = 1;
    public readonly int[] PLAYER_INDEXES = {0, 1};
    //���j�b�g�̍ő�l
    public const int UNIT_MAX = 7;

    //�I�u�W�F�N�g���C���X�^���X������Ƃ���z���W
    // �n���h���[�F1, �{�[�h�F-1, �p�l���F-2, �L�����F-3
    public const float HANDLER_Z = 1;
    public const float BOARD_Z = -1;
    public const float PAPNEL_Z = -2;
    public const float UNIT_Z = -3;

    //���j�b�g�̑I�����̐�
    public const int UNIT_CHOICE_MAX = 5;

    //�I�u�W�F�N�g�̃^�O
    public const string CONTROLLER = "GameController";


    //�{�[�h���W�𔽓]����B
    public static Vector2Int ReverseXY(Vector2Int pos)
    {
        return new Vector2Int(TILE_X - pos.x - 1, TILE_Y - pos.y - 1);
    }


    //�{�[�h�ƃ��j�e�B�Ԃ̍��W�Ԋ�
    public static Vector2 BoardToUnitVector(Vector2Int vectorB)
    {
        //���s�ړ��̕ϊ�
        float a = (float)-(TILE_X - 1) / 2;
        float b = (float)-(TILE_Y - 1) / 2;

        //�X�P�[���̕ϊ�
        float TilesCount = Mathf.Max(TILE_X, TILE_Y);
        float ScaleChange = (float)GAMEBOARD_MAX / TilesCount;

        Vector2 vectorU = new Vector2(ScaleChange * (vectorB.x + a), ScaleChange * (vectorB.y + b));
        return vectorU;

    }
    public static Vector2Int UnitToBoardVector(Vector2 vectorU)
    {
        //���s�ړ��̕ϊ�(���Ƀ{�[�h���W�B�X�P�[���ϊ��s�K�v�j
        float a = (float)(TILE_X - 1) / 2;
        float b = (float)(TILE_Y - 1) / 2;

        //�X�P�[���̕ϊ�
        float TilesCount = Mathf.Max(TILE_X, TILE_Y);
        float ScaleChange = TilesCount / (float)GAMEBOARD_MAX;

        Vector2Int vectorB = new Vector2Int((int)Mathf.Round((ScaleChange * vectorU.x + a)), (int)Mathf.Round((ScaleChange * vectorU.y + b)));
        return vectorB;

    }

    //�{�[�h���W�̎w������j�b�g�\���p�̍��W�ɒ����ϊ�
    public static Vector3 BoardToUnitDisplay(Vector2Int position)
    {
        //�X�P�[���̕ϊ�
        float TilesCount = Mathf.Max(TILE_X, TILE_Y);
        float ScaleChange = (float)GAMEBOARD_MAX / TilesCount;
        //y���W�𔼃}�X���₷
        Vector3 displayPosition = BoardToUnitVector(position) + new Vector2(0, 1) * ScaleChange / 2;
        //z���W���Z�b�g����
        displayPosition.z = UNIT_Z; 
        return displayPosition;
    }
   
    //���[�N���b�h�����̑���
    public static float EuclidDistanceBetween(Vector2Int positionA, Vector2Int positionB)
    {
        float distance = (positionA.x - positionB.x) * (positionA.x - positionB.x) + (positionA.y - positionB.y) * (positionA.y - positionB.y);
        distance = Mathf.Sqrt(distance);
        return distance;
    }
}
