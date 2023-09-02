using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PathOperation : MonoBehaviour
{

    [SerializeField] protected InputManager inputManager;
    [SerializeField] PhysicalBoard physicalBoard;
    [SerializeField] DataBoardManager dataBoardManager;
    [SerializeField] PrimaryUnitOperator primaryUnitOperator;
    [SerializeField] BasicOperation basicOperation;
    [SerializeField] SceneManager sceneManager;
    [SerializeField] MoveHandler moveHandler;
    [SerializeField] TileColor colorTile;

    //指定経路
    protected List<Vector2Int> pathway;

    //クリックした最初のタイルの位置
    protected Vector2Int startTilePosition;
    //最初のタイルの位置のユニット
    protected UnitGeneral startTileUnit;

    //赤マス（禁止マス）にカーソルがあるか
    bool MOVE_PROHIBITED;

    //カーソルがボード上にあるかの判定
    protected bool isOnBoard = false;
    //カーソルがあるタイルにユニットが存在するかの判定
    protected bool unitExists = false;

    //カーソルのあるタイル
    protected Tile tileWithCursor = null;
    //前回カーソルがあったタイル
    protected Tile tileWithCursorBefore = null;

    //カーソルがあるタイルのユニット
    protected UnitGeneral cursorUnit = null;


    //操作状態の改修
    //色のパターン分け
    Color original;

    Color noneCursor_Self;
    Color noneCursor_None;
    Color noneCursor_Target;
    Color noneCursor_NotApplicable;
    
    
    Color withCursor_Self;
    Color withCursor_None;
    Color withCursor_Target;
    Color withCursor_NotApplicable;
    

    
    public void Activate(Vector2Int initTilePosition)
    {
        //初期化
        startTilePosition = initTilePosition;
        startTileUnit = dataBoardManager.GetFromDataBoard(startTilePosition);

        //操作状態をMoveにかえる
        sceneManager.operationType = SceneManager.OperationType.Move;

        //移動経路に最初のタイルの位置を追加
        pathway = new List<Vector2Int> { startTilePosition };
        //判定を初期化
        MOVE_PROHIBITED = false;


    }


    void Update()
    {
        //カーソルがボード上にあるかの判定
        isOnBoard = inputManager.mousePositionBoard != physicalBoard.OUTSIDE;
        //カーソルがあるタイルにユニットが存在するかの判定
        unitExists = isOnBoard && dataBoardManager.JudgeExist(inputManager.mousePositionBoard);

        //カーソルのあるタイル
        tileWithCursor = null;
        //前回カーソルがあったタイル
        tileWithCursorBefore = null;

        //カーソルがあるタイルのユニット
        cursorUnit = null;
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
            if(pathway.Count == 1)
            {
                Debug.Log("0 length move path");
                return;
            }

            //MoveHandlerをユニットの子オブジェクトとして生成しユニットの座標をうごかしてもらう
            MoveHandler mover = Instantiate(moveHandler);
            mover.transform.position = startTileUnit.transform.position;
            mover.transform.SetParent(startTileUnit.transform);
            mover.Activate(startTileUnit, pathway);
        }

        if (MOVE_PROHIBITED) return;


        //右クリックされている最中
        #region 右クリックされている最中
        if (Input.GetMouseButton(1))
        {
            //カーソルが許容位置にあるかで色を変える。

            //ユニットが存在しない場合!unitExists
            if(Condition1())
            {
                //カーソルのあるタイルを明るくする
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, TileColor.movePointTile);
            }
            //敵ユニットが存在する場合cursorUnit.PlayerIndex != startTileUnit.PlayerIndex
            else if (Condition2())
            {
                //カーソルのあるタイルを赤くする
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, TileColor.movePointENEMYUnit);

                //フラグをtrueにする。
                MOVE_PROHIBITED = true;
               
            }
            //味方ユニットが存在かつ・初期位置(自分自身）ではない場合cursorUnit.PlayerIndex == startTileUnit.PlayerIndex && inputManager.mousePositionBoard != startTilePosition
            else if (Condition3())
            {
                //カーソルのあるタイルを赤くする
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, TileColor.movePointMEUnit);
            }
            

            //カーソルがタイル間を移動した場合
            if (inputManager.Displacement())
            {
                //新しいタイルが移動経路に追加される
                pathway.Add(tileWithCursor.PositionBoard);

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
        foreach (Vector2Int tileOnPath in pathway)
        {
            physicalBoard.ChangeTileColor(tileOnPath, TileColor.moveOriginal);
        }

        //MoveOperationを非アクティブ化する
        this.enabled = false;

        //BasicOperationをアクティブ化する
        basicOperation.enabled = true;
        basicOperation.Activate();
    }
    virtual protected bool Condition1()
    {
        Debug.Log("Condition1 is not overwritten.");
        return false;
    }
    virtual protected bool Condition2()
    {
        Debug.Log("Condition2 is not overwritten.");
        return false;
    }
    virtual protected bool Condition3()
    {
        Debug.Log("Condition3 is not overwritten.");
        return false;
    }
}
