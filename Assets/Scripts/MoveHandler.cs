using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandler : MonoBehaviour
{
    //各種管理スクリプト
    GameObject controller;
    DataBoardManager dataBoardManager;

    //移動対象のユニット。移動経路。
    UnitGeneral movingUnit = null;
    List<Vector2Int> movePath;
    //移動経路上のマーカー
    //移動経路のうち何番目のタイルにいるかを示す
    int moveIndex;

    //繰り返し処理のスパン・経過時間
    public float span;
    private float currentTime;

    //初期化メソッド
    public void Activate(UnitGeneral unit, List<Vector2Int> moveRoot)
    {
        //読み込み
        controller = GameObject.FindGameObjectWithTag(Methods.CONTROLLER);
        dataBoardManager = controller.GetComponent<DataBoardManager>();

        //初期化
        movingUnit = unit;
        movePath = moveRoot;
        moveIndex = 1;

        span = 1f;
        currentTime = 0f;

        //対象ユニットのisMoveingをtrueにする
        movingUnit.IsMoving = true;
        //このオブジェクト名を変化させる
        this.gameObject.name = "MoveHandler";

        //movingUnit.UseSkill();
    }

    

    void Update()
    {
        //span秒ごとに繰り返し
        currentTime += Time.deltaTime;
        if (currentTime > span)
        {
            
            //1回前の繰り返し時のユニットの位置
            Vector2Int unitBefore = movingUnit.Position;
            //今回の繰り返し時のユニットの位置
            Vector2Int unitNow = movePath[moveIndex];
            //unitNowにおけるユニットの有無
            bool unitExists = dataBoardManager.JudgeExist(unitNow);


            //unitNowに味方がいる場合
            if(unitExists && dataBoardManager.GetFromDataBoard(unitNow).PlayerIndex == Methods.PLAYER_ME)
            {
                //動かずに終了する
                Destroy(this.gameObject);

                //ユニットのIsMovingをfalseにする
                movingUnit.IsMoving = false;

                return;
            }
            //unitNowに敵がいる場合
            else if(unitExists && dataBoardManager.GetFromDataBoard(unitNow).PlayerIndex == Methods.PLAYER_ENEMY)
            {
                //TODO Moveを終了して戦闘にはいる
                Destroy(this.gameObject);

                //ユニットのIsMovingをfalseにする
                movingUnit.IsMoving = false;

                return;
            }



            //ユニットの位置を変更する
            movingUnit.Position = unitNow;
            //ボード上でのユニット位置の変更
            dataBoardManager.MoveUnit(movingUnit, unitBefore);


            //経路の終了判定
            moveIndex++;
            if( moveIndex >= movePath.Count)
            {
                Destroy(this.gameObject);

                //ユニットのIsMovingをfalseにする
                movingUnit.IsMoving = false;
            }

            //経過時間を0に戻して繰り返し
            currentTime = 0f;
        }
    }
}
