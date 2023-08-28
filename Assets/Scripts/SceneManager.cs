using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    //�Q��
    [SerializeField] PhysicalBoard physicalBoard;
    [SerializeField] UnitManager unitManager;
    [SerializeField] PrimaryUnitOperator primaryUnitOperator;
    [SerializeField] DataBoardManager dataBoardManager;
    [SerializeField] InputManager inputManager;

    //���݂̑�����
    public 
        enum OperationType
    {
        Basic,
        Move,
        Attack,
        Choice
    }

    public OperationType operationType;

    void Start()
    {
        
        //�Q�[���{�[�h���쐬����B
        physicalBoard.Activate();

        
        //���j�b�g�̏����ݒ������B
        unitManager.Activate();

        //�v���C���[�����j�b�g�𑀍�ł���悤�ɂ���
        primaryUnitOperator.Activate();
    }


    void Update()
    {
        
    }
}
