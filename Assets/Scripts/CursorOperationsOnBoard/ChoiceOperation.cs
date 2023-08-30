using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceOperation : MonoBehaviour
{
    //�Q��
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
        //SceneManager�ɑ���󋵂�`����
        sceneManager.operationType = SceneManager.OperationType.Choice;

        //TODO�X�P�[���̕ϊ�
        //�����̌W���͖{�摜���p�ӂł����Ƃ��ɉ��߂ĕύX���悤
        BUTTON_SCALE = 2 * RADIUS * Mathf.Sin(Mathf.PI / ((UNIT_CHOICE_MAX - 1) * 2))/4;

        Debug.Log(Methods.BoardToUnitVector(unitPosition));
        //�{�[�h���W��UI�X�N���[�����W�ɕϊ�
        // unitChoiceUI.transform.position = mainCamera.WorldToScreenPoint(Methods.BoardToUnitVector(unitPosition));
		//for���ɂ��Ă�����
		//unitChoiceUI�̓v���t�@�u
		//����͉R�BCanvas�̎q�I�u�W�F�N�g�B������������g�ɕύX����Unit�̎q�I�u�W�F�N�g�Ƃ��ă{�^���𐶐�����B
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
        //�f�o�b�O�p
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
			Debug.Log("EventTrigger�@is not attached to skill button");
			return;
		}
		EventTrigger.Entry  entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		
		entry.callback.AddListener(_ => Skill(order));
		trigger.triggers.Add(entry);
    }
	
	
	public void Skill(int order)
	{
		//order�Ԗڂ̃X�L�����g�p����悤GeneralUnit�Ɏw������B
		Debug.Log(selectedUnit.gameObject.name + "��" + "skill_" + order.ToString() + "�𔭓��I");
	}

    public Vector2 OrderToPosition(int order)
    {
        float angle = UNIT_CHOICE_MAX == 1 ? 0 :  -Mathf.PI * (float)order / ((float) UNIT_CHOICE_MAX - 1); 
        //magic number y���W�̒���
        return new Vector2(RADIUS * Mathf.Cos(angle), RADIUS * Mathf.Sin(angle));
		//�����W��+20�������܂����B
    }
    void EndChoice()
    {

        //MoveOperation���A�N�e�B�u������
        this.enabled = false;

        //BasicOperation���A�N�e�B�u������
        basicOperation.enabled = true;
        basicOperation.Activate();
    }
}
