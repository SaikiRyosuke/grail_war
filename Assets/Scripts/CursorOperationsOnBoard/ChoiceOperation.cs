using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceOperation : MonoBehaviour
{
    //参照
    [SerializeField] SceneManager sceneManager;
    [SerializeField] InputManager inputManager;
    [SerializeField] BasicOperation basicOperation;
    [SerializeField] GameObject unitChoiceUI;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Camera mainCamera;
    [SerializeField] DataBoardManager dataBoardManager;

    const int UNIT_CHOICE_MAX = 5;
    const float RADIUS = 1f;
    float BUTTON_SCALE;
	UnitGeneral selectedUnit;
	GameObject buttons = null;
	

    public void Activate(Vector2Int unitPosition)
    {
        //SceneManagerに操作状況を伝える
        sceneManager.operationType = SceneManager.OperationType.Choice;

        //TODOスケールの変換
        //ここの係数は本画像が用意できたときに改めて変更しよう
        BUTTON_SCALE = 2 * RADIUS * Mathf.Sin(Mathf.PI / ((UNIT_CHOICE_MAX - 1) * 2))/4;

        Debug.Log(Methods.BoardToUnitVector(unitPosition));
        //ボード座標をUIスクリーン座標に変換
        // unitChoiceUI.transform.position = mainCamera.WorldToScreenPoint(Methods.BoardToUnitVector(unitPosition));
		
		//同じユニットで複数回チョイスフェーズに移行した場合、ボタンが複数生成されないようにする
		//各ユニットの子オブジェクトに空のオブジェクトを生成し、その中にボタンを生成する。
		//チョイスフェーズ離脱時にその子オブジェクトを破壊することによって実現。
		//Destroy処理は、重なると重くなるという噂がある。。。
		//理想としてはGeneralUnit内でActivate したときにこれらのオブジェクトを生成して、このスクリプトではそのactive非アクティブだけをいじる。
		selectedUnit = dataBoardManager.GetFromDataBoard(unitPosition);
		buttons = new GameObject("buttons");
		buttons.transform.parent = selectedUnit.gameObject.transform;
		buttons.transform.position = selectedUnit.gameObject.transform.position;
		for(int i = 0; i < UNIT_CHOICE_MAX; i++){
			InstantiateUnitChoiceButton(buttons, i);
		}
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

    public void InstantiateUnitChoiceButton(GameObject parentObject, int order)
    {
        GameObject button = Instantiate(this.buttonPrefab, OrderToPosition(order) , Quaternion.identity);

        button.name = selectedUnit.gameObject.name + "skill_" + order.ToString();
        button.transform.SetParent(parentObject.transform, false);
        button.transform.localScale = Vector3.one * BUTTON_SCALE;
		
		EventTrigger trigger = button.GetComponent<EventTrigger>();
		if(trigger == null){
			Debug.Log("EventTrigger　is not attached to skill button");
			return;
		}
		EventTrigger.Entry  entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		
		entry.callback.AddListener(_ => Skill(order));
		trigger.triggers.Add(entry);
    }
	
	
	public void Skill(int order)
	{
		//order番目のスキルを使用するようGeneralUnitに指示する。
		Debug.Log(selectedUnit.gameObject.name + "の" + "skill_" + order.ToString() + "を発動！");
	}

    public Vector2 OrderToPosition(int order)
    {
        float angle = UNIT_CHOICE_MAX == 1 ? 0 :  -Mathf.PI * (float)order / ((float) UNIT_CHOICE_MAX - 1); 
        //magic number y座標の調整
        return new Vector2(RADIUS * Mathf.Cos(angle), RADIUS * Mathf.Sin(angle));
		//ｙ座標の+20を消しました。
    }
    void EndChoice()
    {
		//ボタンの消去
		Destroy(buttons);
        //MoveOperationを非アクティブ化する
        this.enabled = false;

        //BasicOperationをアクティブ化する
        basicOperation.enabled = true;
        basicOperation.Activate();
    }
}
