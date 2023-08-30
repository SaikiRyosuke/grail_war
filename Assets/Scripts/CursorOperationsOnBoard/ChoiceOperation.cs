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
		//for分にしておこう
		//unitChoiceUIはプレファブ
		//これは嘘。Canvasの子オブジェクト。これを自分自身に変更してUnitの子オブジェクトとしてボタンを生成する。
		selectedUnit = dataBoardManager.GetFromDataBoard(unitPosition);
		for(int i = 0; i < UNIT_CHOICE_MAX; i++){
			InstantiateUnitChoiceButton(selectedUnit.gameObject, i);
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

        //MoveOperationを非アクティブ化する
        this.enabled = false;

        //BasicOperationをアクティブ化する
        basicOperation.enabled = true;
        basicOperation.Activate();
    }
}
