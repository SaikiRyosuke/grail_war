
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOperation : MonoBehaviour
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
    Vector2Int firstPosition;
    //そのユニット
    UnitGeneral attacker;

    //攻撃範囲のタイルの位置；
    List<Vector2Int> Range = new List<Vector2Int>();
    //攻撃範囲の味方ユニットの位置
    List<Vector2Int> myPositions;
    //攻撃範囲の敵ユニットの位置
    List<Vector2Int> enemyPositions;

    Color tileOriginal = new Color32(255, 255, 255, 100);//標準
    Color tileRange = new Color32(255, 255, 255, 50);//攻撃範囲の色
    Color tileAttackable = new Color32(135, 190, 255, 200);//攻撃可能の色
    Color tileMouseDark = new Color32(100, 100, 100, 100);//攻撃範囲中のマウスの色
    Color tileMouseAttackable = new Color32(149, 254, 255, 240);//攻撃可能中のマウスの色
    Color tileRed = new Color32(255, 150, 150, 50);

    public void Activate(Vector2Int firstTilePosition)
    {
        //SceneManagerに操作状況を伝える
        sceneManager.operationType = SceneManager.OperationType.Attack;

        //タイルの色を元に戻す
        physicalBoard.DyeAllTilesTo(TileColor.attackRangeOutside);

        //攻撃範囲を初期化
        Range = new List<Vector2Int>();
        myPositions = new List<Vector2Int>();
        enemyPositions = new List<Vector2Int>();
        //選ばれたタイルの座標を取得
        firstPosition = firstTilePosition;
        //攻撃主体のユニットを取得
        attacker = dataBoardManager.GetFromDataBoard(firstTilePosition);

        //攻撃範囲をリストにする
        foreach(Vector2Int tilePosition in dataBoardManager.AttackablePositionsWithUnit(attacker))
        {
            Range.Add(tilePosition);
        }

    }
    // Update is called once per frame
    void Update()
    {
        //Range中の敵を明示
        foreach (Vector2Int tilePosition in Range)
        {
            if (dataBoardManager.JudgeExist(tilePosition) && dataBoardManager.GetFromDataBoard(tilePosition).PlayerIndex != attacker.PlayerIndex)
            {
                enemyPositions.Add(tilePosition);
                physicalBoard.GetTile(tilePosition).gameObject.GetComponent<SpriteRenderer>().color = tileAttackable;
            }
            else
            {
                myPositions.Add(tilePosition);
                physicalBoard.GetTile(tilePosition).gameObject.GetComponent<SpriteRenderer>().color = tileRange;
            }

        }
        //カーソルがボード上にあるかの判定
        bool isOnBoard = inputManager.mousePositionBoard != physicalBoard.OUTSIDE;
        //カーソルがレンジ内にあるかの判定
        bool isInRange = Range.Contains(inputManager.mousePositionBoard);
        //カーソルがあるタイルにユニットが存在するかの判定
        bool unitExists = isOnBoard && dataBoardManager.JudgeExist(inputManager.mousePositionBoard);
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

        if(firstPosition != attacker.Position)
        {
            Activate(attacker.Position);
            return;
        }

        //左クリックが終わったとき
        if (Input.GetMouseButtonUp(0))
        {
            //タイルの色を元に戻す
            foreach (Vector2Int tilePosition in Range)
            {
                //physicalBoard.OriginateTileColor(tilePosition);
                physicalBoard.ChangeTileColor(tilePosition, TileColor.attackRangeOutside);
            }

            ////ボード外で離れた場合
            //if (!isOnBoard)
            //{
            //    //BasicOperationに戻る
            //    EndAttackBeginBasic();
            //}
            //ボード内、攻撃範囲外で離れた場合
            if (!isInRange)
            {
                //BasicOperationに戻る
                EndAttackBeginBasic();
            }
            //ボード内、攻撃範囲内、初期位置で離れた場合
            else if (inputManager.mousePositionBoard == firstPosition)
            {
                //ChoiceOperationにかえる
                EndAttackBeginChoice();
            }

            //ボード内、攻撃範囲内、初期位置以外、ユニットなしで離れた場合
            else if (!unitExists)
            {
                //BasicOperationに戻る
                EndAttackBeginBasic();
            }

            //ボード内、攻撃範囲内、初期位置以外、ユニットありで離れた場合
            else if (unitExists)
            {
                //BasicOperationに戻る
                EndAttackBeginBasic();
            }
            return;
        }
        //最初のタイルだけ明るくする
        physicalBoard.ChangeTileColor(firstPosition, TileColor.attackAttacker);


        

        //カーソルがあるタイルが変化したら移動前のタイルを元の色にもどす
        //ボード外から侵入した場合を除く
        if (inputManager.Displacement() && Range.Contains(inputManager.mousePositionBoardBefore))
        {
            //YOCHI 味方の色
            if(enemyPositions.Contains(inputManager.mousePositionBoardBefore))
            {
                physicalBoard.GetTile(inputManager.mousePositionBoardBefore).gameObject.GetComponent<SpriteRenderer>().color = tileAttackable;
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
            if (!unitExists)
            {
                tileWithCursor.gameObject.GetComponent<SpriteRenderer>().color = tileMouseDark;
            }
            else if (cursorUnit.PlayerIndex == attacker.PlayerIndex)
            {
                tileWithCursor.gameObject.GetComponent<SpriteRenderer>().color = tileRed;
            }
            else if (cursorUnit.PlayerIndex != attacker.PlayerIndex)
            {
                tileWithCursor.gameObject.GetComponent<SpriteRenderer>().color = tileMouseAttackable;
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
}
