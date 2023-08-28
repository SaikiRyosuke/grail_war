using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicOperation : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    [SerializeField] PhysicalBoard physicalBoard;
    [SerializeField] DataBoardManager dataBoardManager;
    [SerializeField] MoveOperation moveOperation;
    [SerializeField] AttackOperation attackOperation;
    [SerializeField] SceneManager sceneManager;


    public void Activate()
    {
        //SceneManagerに操作状況を伝える
        sceneManager.operationType = SceneManager.OperationType.Basic;   
    }
    
    void Update()
    {
        //カーソルがボード上にあるかの判定
        bool isOnBoard = inputManager.mousePositionBoard != physicalBoard.OUTSIDE;
        //カーソルがあるタイルにユニットが存在するかの判定
        bool unitExists = isOnBoard && dataBoardManager.JudgeExist(inputManager.mousePositionBoard);
        //カーソルがあるタイルのユニット
        UnitGeneral cursorUnit = null;
        if(unitExists)
        {
            cursorUnit = dataBoardManager.GetFromDataBoard(inputManager.mousePositionBoard);
        }


        //カーソルがあるタイルが変化したら移動前のタイルを元の色にもどす
        //ボード外から侵入した場合を除く
        if (inputManager.Displacement() && inputManager.mousePositionBoardBefore != physicalBoard.OUTSIDE)
        {
            physicalBoard.OriginateTileColor(inputManager.mousePositionBoardBefore);
        }

        //カーソルがあるタイルの色を変化させる(カーソルがボード上にないときはなにもしない)
        if (isOnBoard)  
        {
            //タイルにユニットが存在するとき　明るくする
            //タイルにユニットが存在しないとき　暗くする
            if (unitExists)
            {
                physicalBoard.LightenTile(inputManager.mousePositionBoard);
            }
            else
            {
                physicalBoard.DarkenTile(inputManager.mousePositionBoard);
            }
        }

        //ユニットがいないならクリックの判定を行わない
        if (!unitExists) return;


        //右クリック
        //BasicOperation　→　MoveOperation
        //既に対象ユニットが動いている場合
        //BasicOperation のまま　Moveをキャンセル
        if (Input.GetMouseButtonDown(1))
        {
            //ユニットが既に動いているとき
            if(cursorUnit.IsMoving)
            {
                //ユニットの子オブジェクトのMoveHandlerを破壊。
                GameObject mover = cursorUnit.transform.Find("MoveHandler").gameObject;
                Destroy(mover);
                cursorUnit.IsMoving = false;
            }
            //TEMPORARY 開発者的には相手キャラも動かしたいので。
            //else if (dataBoardManager.GetFromDataBoard(inputManager.mousePositionBoard).PlayerIndex == Methods.PLAYER_ENEMY)
            //{
            //    Debug.Log("you cannot move opponent unit.");

            //}
            //else if(dataBoardManager.GetFromDataBoard(inputManager.mousePositionBoard).PlayerIndex == Methods.PLAYER_ME)

            //ユニットが動いていないとき
            else
            {
                //BasciOperationを非アクティブ化・MoveOperationをアクティブ化・初期化
                this.enabled = false;
                moveOperation.enabled = true;
                moveOperation.Activate(inputManager.mousePositionBoard);
            }
        }

        //左クリック
        //BasicOperation →　AttackOperation
        if (Input.GetMouseButtonDown(0))
        {
            //BasciOperationを非アクティブ化・AttackOperationをアクティブ化・初期化
            this.enabled = false;
            attackOperation.enabled = true;
            attackOperation.Activate(inputManager.mousePositionBoard);
        }

        //デバッグ用　
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            this.enabled = false;
        }
    }
}
