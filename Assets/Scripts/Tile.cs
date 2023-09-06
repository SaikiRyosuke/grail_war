using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //管理スクリプト各種
    GameObject controller;
    InputManager inputManager;
    MainMoveOperation mainMoveOperation;
    PhysicalBoard physicalBoard;
    DataBoardManager dataBoardManager;

    //ボード上の座標
    public Vector2Int PositionBoard { get; set; }

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag(Methods.CONTROLLER);
        inputManager = controller.GetComponent<InputManager>();
        mainMoveOperation = controller.GetComponent<MainMoveOperation>();

        physicalBoard = this.transform.parent.GetComponent<PhysicalBoard>();
        dataBoardManager = this.transform.parent.GetComponent<DataBoardManager>();
    }

    public void Update()
    {

    }

}
