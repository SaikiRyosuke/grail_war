using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ユニット生成と破壊を行います。
/// 細かくはUnitGeneratorとUnitDestroyerを呼び出します。
/// </summary>
public class UnitManager : MonoBehaviour
{
    //参照
    //初期データ
    //管理スクリプト各種
    [SerializeField] InitData initData;
    [SerializeField] UnitGenerator unitGenerator;
    [SerializeField] DataBoardManager dataBoardManager;



    //プレイヤーのユニットのリスト
    //YOCHI リストの配列にする可能性あり
    List<List<UnitGeneral>> playerTroops = new List<List<UnitGeneral>>();


    //ゲーム開始時に一度実行
    public void Activate()
    {
        //TODO
        //前のシーンで選択されたユニットと初期配置を受け取る。
        //やりかたわからない
        //選択ユニット・初期配置を別ファイルに置いた
        //それぞれのプレイヤーについてここからユニットを生成する。

        //指定ユニットをすべて生成
        for( int i = 0; i < 2; i++)
        {
            //一人のプレイヤーのユニット
            List<UnitGeneral> playerUnits = new List<UnitGeneral>();

            for(int j = 0; j < Methods.UNIT_MAX; j++)
            {
                //ユニットのIDと初期配置
                string unitID = initData.initUnitsID[i][j];
                Vector2Int initPosition = initData.initPositions[i][j];
                //ユニットを生成
                UnitGeneral unit = CreateUnit(unitID, initPosition, i);
                playerUnits.Add(unit);
                //データボードに反映
                dataBoardManager.SetToDataBoard(unit);
            }
            playerTroops.Add(playerUnits);
        }
        
    }

    //IDからユニットを生成。必要な外部情報を全て付与
    //（外部情報）・生成位置・プレイヤー識別番号
    public UnitGeneral CreateUnit(string unitID, Vector2Int generatingPosition, int playerIndex)
    {
        //生成
        UnitGeneral unit = unitGenerator.GenerateUnitByName(unitID);

        //外部情報付与
        unit.Position = generatingPosition;
        unit.PlayerIndex = playerIndex;

        //ボード座標を表示用の座標に変換・設定
        Vector3 DisplayPosition = Methods.BoardToUnitDisplay(generatingPosition);
        unit.transform.position = DisplayPosition;
        
        //プレイヤー判別色
        unitGenerator.SetColor(unit);
        //
        //unit.Activate();

        return unit;

    }
}
