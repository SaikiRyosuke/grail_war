using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// �Q�[���J�n���Ɉ�x����UnitManager�ɂ�胆�j�b�g����������܂�
/// ���̍ۂɎ�Ɉ�x�����g�p����p�r�̃��\�b�h���W�߂��̂�UnitGenerator�ł�
/// </summary>

public class UnitGenerator : MonoBehaviour
{
    //�C���X�y�N�^�w��
    //���j�b�g�̃v���t�@�u��ID
    [SerializeField] List<GameObject> units;
    [SerializeField] List<string> unitIDs = new List<string>();



    //�萔�ł������Ă���̂ł����ɂ����܂�
    //�����̐F
    Color ME_COLOR = new Color32(48, 64, 255, 200);
    //�G�̐F
    Color ENEMY_COLOR = new Color32(246, 20, 118, 200);
    //�v���C���[�̐F�̔z��
    Color[] PLAYER_COLORS = { new Color32(48, 64, 255, 200), new Color32(246, 20, 118, 200) };


    //���j�b�g��ID���w�肵�ĊY�����郆�j�b�g�̃I�u�W�F�N�g�𐶐��i�������̂݁j
    public UnitGeneral GenerateUnitByName(string unitID)
    {
        int index = unitIDs.FindIndex(a => a == unitID);
        if (index == -1)
        {
            Debug.LogError("����Ȗ��O�̃��j�b�g�͂��Ȃ�: " + unitID);
            return Instantiate(units[0]).GetComponent<UnitGeneral>();
        }
        return Instantiate(units[index]).GetComponent<UnitGeneral>();
    }

    //���j�b�g�Ƀv���C���[���ʗp�ɐF��t����
    public void SetColor(UnitGeneral unit)
    {
        //���j�b�g��HPBar����uFrame�v���擾
        //���j�b�g�̑����̑ȉ~�uIdentifier�v���擾
        GameObject bar = unit.transform.Find("HPBar").transform.Find("Frame").gameObject;  
        GameObject identifier = unit.transform.Find("Identifier").gameObject;

        //unit�̕ێ�����PlayerIndex�ɉ����ĐF��ς���B
        bar.GetComponent<SpriteRenderer>().color = PLAYER_COLORS[unit.PlayerIndex];
        identifier.GetComponent<SpriteRenderer>().color = PLAYER_COLORS[unit.PlayerIndex];
    }
}
