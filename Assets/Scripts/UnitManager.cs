using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���j�b�g�����Ɣj����s���܂��B
/// �ׂ�����UnitGenerator��UnitDestroyer���Ăяo���܂��B
/// </summary>
public class UnitManager : MonoBehaviour
{
    //�Q��
    //�����f�[�^
    //�Ǘ��X�N���v�g�e��
    [SerializeField] InitData initData;
    [SerializeField] UnitGenerator unitGenerator;
    [SerializeField] DataBoardManager dataBoardManager;



    //�v���C���[�̃��j�b�g�̃��X�g
    //YOCHI ���X�g�̔z��ɂ���\������
    List<List<UnitGeneral>> playerTroops = new List<List<UnitGeneral>>();


    //�Q�[���J�n���Ɉ�x���s
    public void Activate()
    {
        //TODO
        //�O�̃V�[���őI�����ꂽ���j�b�g�Ə����z�u���󂯎��B
        //��肩���킩��Ȃ�
        //�I�����j�b�g�E�����z�u��ʃt�@�C���ɒu����
        //���ꂼ��̃v���C���[�ɂ��Ă������烆�j�b�g�𐶐�����B

        //�w�胆�j�b�g�����ׂĐ���
        for( int i = 0; i < 2; i++)
        {
            //��l�̃v���C���[�̃��j�b�g
            List<UnitGeneral> playerUnits = new List<UnitGeneral>();

            for(int j = 0; j < Methods.UNIT_MAX; j++)
            {
                //���j�b�g��ID�Ə����z�u
                string unitID = initData.initUnitsID[i][j];
                Vector2Int initPosition = initData.initPositions[i][j];
                //���j�b�g�𐶐�
                UnitGeneral unit = CreateUnit(unitID, initPosition, i);
                playerUnits.Add(unit);
                //�f�[�^�{�[�h�ɔ��f
                dataBoardManager.SetToDataBoard(unit);
            }
            playerTroops.Add(playerUnits);
        }
        
    }

    //ID���烆�j�b�g�𐶐��B�K�v�ȊO������S�ĕt�^
    //�i�O�����j�E�����ʒu�E�v���C���[���ʔԍ�
    public UnitGeneral CreateUnit(string unitID, Vector2Int generatingPosition, int playerIndex)
    {
        //����
        UnitGeneral unit = unitGenerator.GenerateUnitByName(unitID);

        //�O�����t�^
        unit.Position = generatingPosition;
        unit.PlayerIndex = playerIndex;

        //�{�[�h���W��\���p�̍��W�ɕϊ��E�ݒ�
        Vector3 DisplayPosition = Methods.BoardToUnitDisplay(generatingPosition);
        unit.transform.position = DisplayPosition;
        
        //�v���C���[���ʐF
        unitGenerator.SetColor(unit);
        //
        //unit.Activate();

        return unit;

    }
}
