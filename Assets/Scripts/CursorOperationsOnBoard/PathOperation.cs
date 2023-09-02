using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// ボード内マウス操作
/// 経路の指定に用いる。経路指定後にメソッドを実行する。
///・スタートマス
///・対象のないマス
///・対象のいるマス
///・対象外のいるマス
///に区別して色を表示する。
///ボード外にでてきたとき強制的に終了する。
/// </summary>
public class PathOperation : MonoBehaviour
{

    [SerializeField] protected InputManager inputManager;
    [SerializeField] protected PhysicalBoard physicalBoard;
    [SerializeField] protected DataBoardManager dataBoardManager;
    [SerializeField] protected PrimaryUnitOperator primaryUnitOperator;
    [SerializeField] protected BasicOperation basicOperation;
    [SerializeField] protected SceneManager sceneManager;
    [SerializeField] protected MoveHandler moveHandler;
    [SerializeField] protected TileColor colorTile;

    //指定経路
    protected List<Vector2Int> pathway;

    //クリックした最初のタイルの位置
    protected Vector2Int startTilePosition;
    //最初のタイルの位置のユニット
    protected UnitGeneral startTileUnit;

    //強制終了条件
    protected bool ACT_PROHIBITED;

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
    Color original = TileColor.original;

    //Color noneCursor_Self;
    Color noneCursor_None = TileColor.paleDark;
    Color noneCursor_Target = TileColor.lightYelow;
    Color noneCursor_NotApplicable = TileColor.red;
    
    //selfはnoneと同じ挙動
    Color withCursor_Self = TileColor.bibidLightBlue;
    Color withCursor_None = TileColor.bibidLightBlue;
    Color withCursor_Target = TileColor.lightYelow;
    Color withCursor_NotApplicable = TileColor.red;
    

    //初期化
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
        ACT_PROHIBITED = false;

    }

    //カーソル位置でタイルの色を変更する
    void Update()
    {
        //カーソルのあるマス
        //ボード上にあるかの判定
        isOnBoard = inputManager.mousePositionBoard != physicalBoard.OUTSIDE;
        //ユニットが存在するかの判定
        unitExists = isOnBoard && dataBoardManager.JudgeExist(inputManager.mousePositionBoard);

        //カーソルのあるタイル
        tileWithCursor = null;
        //前回カーソルがあったタイル
        tileWithCursorBefore = null;

        //カーソルがあるタイルのユニット
        if (unitExists)
        {
            cursorUnit = dataBoardManager.GetFromDataBoard(inputManager.mousePositionBoard);
        }


        //ボード外にカーソルが出た場合
        //終了する
        if (!isOnBoard)
        {
            EndPathOperation();
            return;
        }


        //ボード内

        tileWithCursor = physicalBoard.GetTile(inputManager.mousePositionBoard);
        tileWithCursorBefore = physicalBoard.GetTile(inputManager.mousePositionBoardBefore);
        
        

        //右クリックが終わったとき
        if (Input.GetMouseButtonUp(1))
        {
            EndPathOperation();
            //YOCHI 移動経路を出力する

            //行動禁止条件を満たしている場合
            if (ACT_PROHIBITED)
            {
                Debug.Log("prohibited path");
                return;
            }
            //クリックのみだった場合
            if(pathway.Count == 1)
            {
                Debug.Log("0 length path");
                return;
            }
            //目的となるメソッドを実行する
            Operate();
        }

        //禁止操作時は以下のクリック途中の判定を行わない
        if (ACT_PROHIBITED) return;


        #region 右クリックされている最中
        if (Input.GetMouseButton(1))
        {
            //カーソルの位置の色

            //初期位置の場合
            if(inputManager.mousePositionBoard == startTilePosition)
            {
                //明るくする
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, withCursor_Self);
            }
            //対象が存在しない場合
            else if(Condition1())
            {
                //明るくする
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, withCursor_None);
            }
            //対象が存在する場合
            else if (Condition2())
            {
                //黄色くする
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, withCursor_Target);

                //フラグをtrueにする。
                ACT_PROHIBITED = true;
               
            }
            //除外対象の場合
            else if (Condition3())
            {
                //赤くする
                physicalBoard.ChangeTileColor(tileWithCursor.PositionBoard, withCursor_NotApplicable);
            }
            

            //カーソルがタイル間を移動した場合
            if (inputManager.Displacement())
            {
                //新しいタイルが移動経路に追加される
                pathway.Add(tileWithCursor.PositionBoard);

                ////すぎたカーソルが行動終了マスじゃなかった場合
                //if (tileWithCursorBefore.gameObject.GetComponent<SpriteRenderer>().color != TileColor.movePointENEMYUnit)
                //{
                //    //カーソルがすぎたタイルは暗くする
                //    physicalBoard.ChangeTileColor(tileWithCursorBefore.PositionBoard, TileColor.movePassedTile);
                //}
                
                //過ぎたタイルの色を状態に応じて戻す。
                //ここでは色が明るい青の場合のみ戻す。
                if(tileWithCursorBefore.gameObject.GetComponent<SpriteRenderer>().color == withCursor_None)
                {
                    physicalBoard.ChangeTileColor(tileWithCursorBefore.PositionBoard, noneCursor_None);
                }
            }
        }
        #endregion
        

    }

    //終了操作
    void EndPathOperation()
    {
        //タイルの色を元に戻す。
        foreach (Vector2Int tileOnPath in pathway)
        {
            physicalBoard.ChangeTileColor(tileOnPath, TileColor.moveOriginal);
        }

        //PathOperationを非アクティブにする
        this.enabled = false;
        //次の操作方法をアクティブにする
        ActivateNextOperation();
    }

    /// <summary>
    /// 次に操作のActivate
    /// enabledをtrueにし、Activate()を実行する
    /// </summary>
    virtual protected void ActivateNextOperation()
    {
        Debug.Log(nameof(ActivateNextOperation) + " is not overriden");
    }

    /// <summary>
    /// 対象がいないタイルの条件
    /// </summary>
    virtual protected bool Condition1()
    {
        Debug.Log(nameof(Condition1) + " is not overriden.");
        return false;
    }

    /// <summary>
    /// 対象がいるタイルの条件
    /// </summary>
    /// <returns></returns>
    virtual protected bool Condition2()
    {
        Debug.Log(nameof(Condition2) + " is not overriden.");
        return false;
    }

    /// <summary>
    /// 対象外であるタイルの条件
    /// </summary>
    /// <returns></returns>
    virtual protected bool Condition3()
    {
        Debug.Log(nameof(Condition3) + " is not overriden.");
        return false;
    }
    /// <summary>
    /// 経路指定後、実行するメソッド
    /// </summary>
    virtual protected void Operate()
    {
        Debug.Log(nameof(Operate) + " is not overriden.");
    }
}
