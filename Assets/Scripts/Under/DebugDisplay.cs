using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugDisplay : MonoBehaviour
{

    //管理スクリプト各種
    [SerializeField] InputManager inputManager;
    [SerializeField] PrimaryUnitOperator primaryUnitOperator;
    [SerializeField] SceneManager sceneManager;

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
        primaryUnit.text = "PrimaryUnit :" + primaryUnitOperator.PrimaryUnit;
        cursorOperation.text = "CursorOperation : " + sceneManager.operationType;
    }
}
