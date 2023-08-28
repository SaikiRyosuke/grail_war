using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryUnitOperator : MonoBehaviour
{
    [SerializeField] DataBoardManager dataBoardManager;
    [SerializeField] PhysicalBoard physicalBoard;
    [SerializeField] InputManager inputManager;


    //PrimaryUnit. 選択中で行動の命令をだすことができるユニット
    public UnitGeneral PrimaryUnit { get; set; } = null;
    public void Activate()
    {

    }

    private void Update()
    {
        Vector2Int mouse = inputManager.mousePositionBoard;
        //自分のユニットを選択したらPrimaryUnitに設定する
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && physicalBoard.JudgeOnBoard(mouse) && dataBoardManager.JudgeExist(mouse) && dataBoardManager.GetFromDataBoard(mouse).PlayerIndex == Methods.PLAYER_ME)
        {
            PrimaryUnit = dataBoardManager.GetFromDataBoard(mouse);
        }
    }
}
