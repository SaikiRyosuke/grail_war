using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

public class DebugDisplay : MonoBehaviour
{

    //管理スクリプト各種
    [SerializeField] InputManager inputManager;
    [SerializeField] PrimaryUnitOperator primaryUnitOperator;
    [SerializeField] SceneManager sceneManager;
    [SerializeField] BasicOperation basicOperation;
    [SerializeField] PathOperation pathOperation;
    [SerializeField] AttackOperation attackOperation;
    [SerializeField] ChoiceOperation choiceOperation;

    TextMeshProUGUI mousePositionBoardText;
    TextMeshProUGUI mousePositionUnitText;
    TextMeshProUGUI boardDisplacement;
    TextMeshProUGUI primaryUnit;
    TextMeshProUGUI cursorOperation;

    private void Start()
    {
        mousePositionBoardText = this.transform.Find("MousePositionBoard").gameObject.GetComponent<TextMeshProUGUI>();
        mousePositionUnitText = this.transform.Find("MousePositionUnit").gameObject.GetComponent<TextMeshProUGUI>();
        boardDisplacement = this.transform.Find("BoardDisplacement").gameObject.GetComponent<TextMeshProUGUI>();
        primaryUnit = this.transform.Find("PrimaryUnit").gameObject.GetComponent<TextMeshProUGUI>();
        cursorOperation = this.transform.Find("CursorOperation").gameObject.GetComponent<TextMeshProUGUI>();
       
    }

    //DebugDisplayの表示・非表示
    public void OnDebugDisplay()
    {
        this.gameObject.SetActive(true);
    }
    public void OffDebugDisplay()
    {
        this.gameObject.SetActive(true);
    }
    void Update()
    {
        mousePositionBoardText.text = "MouseBoard :" + inputManager.mousePositionBoard.x.ToString() + " : " + inputManager.mousePositionBoard.y.ToString();
        mousePositionUnitText.text = "MouseUnit :" + inputManager.mousePositionUnit.x.ToString() + " : " + inputManager.mousePositionUnit.y.ToString();
        boardDisplacement.text = "BoardDisplacement :" + inputManager.Displacement();
        if(primaryUnitOperator.PrimaryUnit != null)primaryUnit.text = "PrimaryUnit ID :" + primaryUnitOperator.PrimaryUnit.UnitID;
        cursorOperation.text = "CursorOperation : " + sceneManager.operationType;
        //Operationの数の判定
        if (Convert.ToInt32(basicOperation.enabled) + Convert.ToInt32(pathOperation.enabled) + Convert.ToInt32(attackOperation.enabled) + Convert.ToInt32(choiceOperation.enabled) != 1)
        {
            cursorOperation.text = "CursorOperation is not determined!!";
        }
    }
}
