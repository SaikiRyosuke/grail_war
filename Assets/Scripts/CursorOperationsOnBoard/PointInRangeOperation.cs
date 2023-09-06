
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInRangeOperation : MonoBehaviour
{
    //参照
    [SerializeField] SceneManager sceneManager;
    [SerializeField] BasicOperation basicOperation;
    [SerializeField] ChoiceOperation choiceOperation;
    [SerializeField] PhysicalBoard physicalBoard;
    [SerializeField] InputManager inputManager;
    [SerializeField] DataBoardManager dataBoardManager;
    [SerializeField] TileColor colorTile;

    //最初に選ばれたタイル
    protected Vector2Int startTilePosition;
    //そのユニット
    protected UnitGeneral startTileUnit;

    //レンジ
    float range;
    //範囲内のタイルの位置；
    List<Vector2Int> InRangePoisitions = new List<Vector2Int>();
    //範囲内の味方ユニットの位置
    List<Vector2Int> myPositions;
    //範囲内の敵ユニットの位置
    List<Vector2Int> enemyPositions;

    //カーソルがボード上にあるかの判定
    protected bool isOnBoard = false;
    //カーソルがレンジ内にあるかの判定
    protected bool isInRange = false;
    //カーソルがあるタイルにユニットが存在するかの判定
    protected bool unitExists = false;
    //カーソルがあるタイルのユニット
    protected UnitGeneral cursorUnit = null;
    //カーソルのあるタイル
    protected Tile tileWithCursor = null;

    //色のパターン分け
    Color original = TileColor.original;
    Color inside = TileColor.paleDark;

    Color noneCursor_Self = TileColor.original;
    Color noneCursor_None = TileColor.paleDark;
    Color noneCursor_Target = TileColor.lightYelow;
    //Color noneCursor_NotApplicable = TileColor.red;

    //selfはnoneと同じ挙動
    Color withCursor_Self = TileColor.lightBlue;
    Color withCursor_None = TileColor.dark;
    Color withCursor_Target = TileColor.orange;
    //Color withCursor_NotApplicable = TileColor.red;

    public void Activate(Vector2Int firstTilePosition, float rangeFromFirstTile)
    {
        //SceneManagerに操作状況を伝える
        sceneManager.operationType = SceneManager.OperationType.Attack;

        //タイルの色を元に戻す
        physicalBoard.DyeAllTilesTo(TileColor.attackRangeOutside);

        //範囲を初期化
        InRangePoisitions = new List<Vector2Int>();
        myPositions = new List<Vector2Int>();
        enemyPositions = new List<Vector2Int>();
        //選ばれたタイルの座標を取得
        startTilePosition = firstTilePosition;
        //攻撃主体のユニットを取得
        startTileUnit = dataBoardManager.GetFromDataBoard(firstTilePosition);
        //範囲を取得
        range = rangeFromFirstTile;
        //範囲をリストにする
        foreach(Vector2Int tilePosition in dataBoardManager.GetRangeWithRangeNumberAndPositions(range, startTilePosition))
        {
            InRangePoisitions.Add(tilePosition);
        }

    }

    void Update()
    {
        //Range中の敵を明示
        foreach (Vector2Int tilePosition in InRangePoisitions)
        {
            if (dataBoardManager.JudgeExist(tilePosition) && dataBoardManager.GetFromDataBoard(tilePosition).PlayerIndex != startTileUnit.PlayerIndex)
            {
                enemyPositions.Add(tilePosition);
                physicalBoard.GetTile(tilePosition).gameObject.GetComponent<SpriteRenderer>().color = TileColor.attackRangeENEMYUnit;
            }
            else
            {
                myPositions.Add(tilePosition);
                physicalBoard.GetTile(tilePosition).gameObject.GetComponent<SpriteRenderer>().color = TileColor.attackRangeInside;
            }

        }
        //カーソルがボード上にあるかの判定
        isOnBoard = inputManager.mousePositionBoard != physicalBoard.OUTSIDE;
        //カーソルがレンジ内にあるかの判定
        isInRange = InRangePoisitions.Contains(inputManager.mousePositionBoard);
        //カーソルがあるタイルにユニットが存在するかの判定
        unitExists = isOnBoard && dataBoardManager.JudgeExist(inputManager.mousePositionBoard);
        //カーソルがあるタイルのユニット
        UnitGeneral cursorUnit = null;
        if (unitExists)
        {
            cursorUnit = dataBoardManager.GetFromDataBoard(inputManager.mousePositionBoard);
        }
        //カーソルのあるタイル
        Tile tileWithCursor = null;
        if (isOnBoard)
        {
            tileWithCursor = physicalBoard.GetTile(inputManager.mousePositionBoard);
        }

        if(startTilePosition != startTileUnit.Position)
        {
            Activate(startTileUnit.Position, range);
            return;
        }

        //左クリックが終わったとき
        if (Input.GetMouseButtonUp(0))
        {
            //タイルの色を元に戻す
            foreach (Vector2Int tilePosition in InRangePoisitions)
            {
                physicalBoard.ChangeTileColor(tilePosition, TileColor.attackRangeOutside);
            }

            //範囲外で離れた場合
            if (!isInRange)
            {
                //BasicOperationに戻る
                EndAttackBeginBasic();
            }
            //ボード内、範囲内、初期位置で離れた場合
            else if (inputManager.mousePositionBoard == startTilePosition)
            {
                //ChoiceOperationにかえる
                EndAttackBeginChoice();
            }

            //ボード内、範囲内、初期位置以外、ユニットなしで離れた場合
            else if (!unitExists)
            {
                //BasicOperationに戻る
                EndAttackBeginBasic();
            }

            //ボード内、範囲内、初期位置以外、ユニットありで離れた場合
            else if (unitExists)
            {
                //BasicOperationに戻る
                EndAttackBeginBasic();
            }
            return;
        }
        //最初のタイルだけ明るくする
        physicalBoard.ChangeTileColor(startTilePosition, TileColor.attackAttacker);


        

        //カーソルがあるタイルが変化したら移動前のタイルを元の色にもどす
        //ボード外から侵入した場合を除く
        if (inputManager.Displacement() && InRangePoisitions.Contains(inputManager.mousePositionBoardBefore))
        {
            //YOCHI 味方の色
            if(enemyPositions.Contains(inputManager.mousePositionBoardBefore))
            {
                physicalBoard.GetTile(inputManager.mousePositionBoardBefore).gameObject.GetComponent<SpriteRenderer>().color = TileColor.attackRangeENEMYUnit;
            }
            else
            {
                //physicalBoard.DarkenTile(inputManager.mousePositionBoardBefore);
                physicalBoard.ChangeTileColor(inputManager.mousePositionBoardBefore, TileColor.attackRangeInside);
            }
            
        }

        if (!isInRange) return;
        //左クリックされている最中
        if (Input.GetMouseButton(0))
        {
            if (Condition1())
            {
                tileWithCursor.gameObject.GetComponent<SpriteRenderer>().color = TileColor.attackCursorTile;
            }
            else if (Condition3())
            {
                tileWithCursor.gameObject.GetComponent<SpriteRenderer>().color = TileColor.attackCursorMEUnit;
            }
            else if (Condition2())
            {
                tileWithCursor.gameObject.GetComponent<SpriteRenderer>().color = TileColor.attackCursorENEMYUnit;
            }
        }
        
    }
    void EndAttackBeginBasic()
    {

        //AttackOperationを非アクティブ化する
        this.enabled = false;

        //BasicOperationをアクティブ化する
        basicOperation.enabled = true;
        basicOperation.Activate();
    }void EndAttackBeginChoice()
    {

        //AttackOperationを非アクティブ化する
        this.enabled = false;

        //ChoiceOperationをアクティブ化する
        choiceOperation.enabled = true;
        choiceOperation.Activate(inputManager.mousePositionBoard);
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
    /// 実行するメソッド
    /// </summary>
    virtual protected void Operate()
    {
        Debug.Log(nameof(Operate) + " is not overriden.");
    }
}
