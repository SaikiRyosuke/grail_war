using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveOperation : MonoBehaviour
{

    [SerializeField] InputManager inputManager;
    [SerializeField] PhysicalBoard physicalBoard;
    [SerializeField] DataBoardManager dataBoardManager;
    [SerializeField] PrimaryUnitOperator primaryUnitOperator;
    [SerializeField] BasicOperation basicOperation;
    [SerializeField] SceneManager sceneManager;
    [SerializeField] MoveHandler moveHandler;
    [SerializeField] TileColor colorTile;

    //移動経路
    List<Vector2Int> movePath;

    //クリックした最初のタイルの位置
    Vector2Int startTilePosition;
    //最初のタイルの位置のユニット
    UnitGeneral startTileUnit;

    //赤マス（禁止マス）にカーソルがあるか
    bool MOVE_PROHIBITED;

    public void Activate(Vector2Int initTilePosition)
    {
        //初期化
        startTilePosition = initTilePosition;
        startTileUnit = dataBoardManager.GetFromDataBoard(startTilePosition);

        //操作状態をMoveにかえる
        sceneManager.operationType = SceneManager.OperationType.Move;

        //移動経路に最初のタイルの位置を追加
        movePath = new List<Vector2Int> { startTilePosition};
        //判定を初期化
        MOVE_PROHIBITED = false;

        
    }


    void Update()
    {
        //カーソルがボード上にあるかの判定
        bool isOnBoard = inputManager.mousePositionBoard != physicalBoard.OUTSIDE;
        //カーソルがあるタイルにユニットが存在するかの判定
        bool unitExists = isOnBoard && dataBoardManager.JudgeExist(inputManager.mousePositionBoard);

        //カーソルのあるタイル
        Tile tileWithCursor = null;
        //前回カーソルがあったタイル
        Tile tileWithCursorBefore = null;

        //カーソルがあるタイルのユニット
        UnitGeneral cursorUnit = null;
        if (unitExists)
        {
            cursorUnit = dataBoardManager.GetFromDataBoard(inputManager.mousePositionBoard);
        }


        //ボード外に出た時の処理
        if (!isOnBoard)
        {
            EndMove();
            return;
        }

        //ボード内の処理

        tileWithCursor = physicalBoard.GetTile(inputManager.mousePositionBoard);
        tileWithCursorBefore = physicalBoard.GetTile(inputManager.mousePositionBoardBefore);
        
        

        //右クリックが終わったとき
        if (Input.GetMouseButtonUp(1))
        {
            EndMove();
            //YOCHI 移動経路を出力する
            //赤マスで終了した場合
            if (MOVE_PROHIBITED)
            {
                Debug.Log("end with red tile");
                return;
            }
            //クリックのみだった場合
            if(movePath.Count == 1)
            {
                Debug.Log("0 length move path");
                return;
            }

            //MoveHandlerをユニットの子オブジェクトとして生成しユニットの座標をうごかしてもらう
            MoveHandler mover = Instantiate(moveHandler);
            mover.transform.position = startTileUnit.transform.position;
            mover.transform.SetParent(startTileUnit.transform);
            mover.Activate(startTileUnit, movePath);
        }

        if (MOVE_PROHIBITED) return;


        //右クリックされている最中
        #region 右クリックされている最中
        if (Input.GetMouseButton(1))
        {
            //カーソルが許容位置にあるかで色を変える。

            //ユニットが存在しない場合
            if(!unitExists)
            {
                //カーソルのあるタイルを明るくする
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, TileColor.movePointTile);
            }
            //敵ユニットが存在する場合
            else if (cursorUnit.PlayerIndex != startTileUnit.PlayerIndex)
            {
                //カーソルのあるタイルを赤くする
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, TileColor.movePointENEMYUnit);

                //フラグをtrueにする。
                MOVE_PROHIBITED = true;
               
            }
            //味方ユニットが存在かつ・初期位置(自分自身）ではない場合
            else if (cursorUnit.PlayerIndex == startTileUnit.PlayerIndex && inputManager.mousePositionBoard != startTilePosition)
            {
                //カーソルのあるタイルを赤くする
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, TileColor.movePointMEUnit);
            }
            

            //カーソルがタイル間を移動した場合
            if (inputManager.Displacement())
            {
                //新しいタイルが移動経路に追加される
                movePath.Add(tileWithCursor.PositionBoard);

                //すぎたカーソルが赤マスじゃなかった場合
                if (tileWithCursorBefore.gameObject.GetComponent<SpriteRenderer>().color != TileColor.movePointENEMYUnit)
                {
                    //カーソルがすぎたタイルは暗くする
                    physicalBoard.ChangeTileColor(tileWithCursorBefore.PositionBoard, TileColor.movePassedTile);
                }
                

            }
        }
        #endregion
        

    }

    //終了操作
    void EndMove()
    {
        //タイルの色を元に戻す。
        foreach (Vector2Int tileOnPath in movePath)
        {
            physicalBoard.ChangeTileColor(tileOnPath, TileColor.moveOriginal);
        }

        //MoveOperationを非アクティブ化する
        this.enabled = false;

        //BasicOperationをアクティブ化する
        basicOperation.enabled = true;
        basicOperation.Activate();
    }
}
