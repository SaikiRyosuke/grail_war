using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //�Ǘ��X�N���v�g�e��
    GameObject controller;
    InputManager inputManager;
    MoveOperation moveOperation;
    PhysicalBoard physicalBoard;
    DataBoardManager dataBoardManager;

    //�{�[�h��̍��W
    public Vector2Int PositionBoard { get; set; }

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag(Methods.CONTROLLER);
        inputManager = controller.GetComponent<InputManager>();
        moveOperation = controller.GetComponent<MoveOperation>();

        physicalBoard = this.transform.parent.GetComponent<PhysicalBoard>();
        dataBoardManager = this.transform.parent.GetComponent<DataBoardManager>();
    }

    public void Update()
    {

    }

}
