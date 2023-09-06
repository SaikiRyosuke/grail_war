using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceOperation : MonoBehaviour
{
    //参照
    [SerializeField] SceneManager sceneManager;
    [SerializeField] InputManager inputManager;
    [SerializeField] BasicOperation basicOperation;
    [SerializeField] GameObject unitChoiceUI;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Camera mainCamera;

    const int UNIT_CHOICE_MAX = 5;
    const float RADIUS = 90f;
    float BUTTON_SCALE;

    public void Activate(Vector2Int unitPosition)
    {
        //SceneManagerに操作状況を伝える
        sceneManager.operationType = SceneManager.OperationType.Choice;

        //TODOスケールの変換
        //100はボタンのもとのサイズ
        BUTTON_SCALE = 2 * RADIUS * Mathf.Sin(Mathf.PI / ((UNIT_CHOICE_MAX - 1) * 2)) / 100;

        Debug.Log(Methods.BoardToUnitVector(unitPosition));
        //ボード座標をUIスクリーン座標に変換
        unitChoiceUI.transform.position = mainCamera.WorldToScreenPoint(Methods.BoardToUnitVector(unitPosition));

        InstantiateUnitChoiceButton(unitChoiceUI, "hello", 0);
        InstantiateUnitChoiceButton(unitChoiceUI, "hello", 1);
        InstantiateUnitChoiceButton(unitChoiceUI, "hello", 2);
        InstantiateUnitChoiceButton(unitChoiceUI, "hello", 3);
        InstantiateUnitChoiceButton(unitChoiceUI, "hello", 4);
    }


    void Update()
    {
        

        if (Input.GetMouseButtonUp(0))
        {
            EndChoice();
        }
        //デバッグ用
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            EndChoice();
        }
    }

    public void InstantiateUnitChoiceButton(GameObject parentObject, string name, int order)
    {
        GameObject button = Instantiate(this.buttonPrefab, OrderToPosition(order) , Quaternion.identity);

        button.name = name;
        button.transform.SetParent(parentObject.transform, false);
        button.transform.localScale = Vector3.one * BUTTON_SCALE;


    }

    public Vector2 OrderToPosition(int order)
    {
        float angle = UNIT_CHOICE_MAX == 1 ? 0 :  -Mathf.PI * (float)order / ((float) UNIT_CHOICE_MAX - 1); 
        //magic number y座標の調整
        return new Vector2(RADIUS * Mathf.Cos(angle), RADIUS * Mathf.Sin(angle) + 20);
    }
    void EndChoice()
    {

        //MainMoveOperationを非アクティブ化する
        this.enabled = false;

        //BasicOperationをアクティブ化する
        basicOperation.enabled = true;
        basicOperation.Activate();
    }
}
