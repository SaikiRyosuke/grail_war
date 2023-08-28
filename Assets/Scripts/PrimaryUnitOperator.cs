using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryUnitOperator : MonoBehaviour
{
    [SerializeField] DataBoardManager dataBoardManager;
    [SerializeField] PhysicalBoard physicalBoard;
    [SerializeField] InputManager inputManager;


    //PrimaryUnit. �I�𒆂ōs���̖��߂��������Ƃ��ł��郆�j�b�g
    public UnitGeneral PrimaryUnit { get; set; } = null;
    public void Activate()
    {

    }

    private void Update()
    {
        Vector2Int mouse = inputManager.mousePositionBoard;
        //�����̃��j�b�g��I��������PrimaryUnit�ɐݒ肷��
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && physicalBoard.JudgeOnBoard(mouse) && dataBoardManager.JudgeExist(mouse) && dataBoardManager.GetFromDataBoard(mouse).PlayerIndex == Methods.PLAYER_ME)
        {
            PrimaryUnit = dataBoardManager.GetFromDataBoard(mouse);
        }
    }
}
