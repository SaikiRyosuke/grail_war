using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// マウスからの入力を管理します。
/// 
/// 入力値はいくらでも変えられるためpublicになっています。
/// </summary>
public class InputManager : MonoBehaviour
{
    //参照
    //PhysicalBoard. ボード内にない場合のマウス入力を判定する
    [SerializeField] PhysicalBoard physicalBoard;

    //マウスの座標
    //ユニット座標
    public Vector2 mousePositionUnit; //少数第2位まで表示する


    //ボード座標

    //ボードの外部ではphysicalBoardでreadonlyで宣言されるOUTSIDE 
    //初期値も同様に　OUTSIDE
    public Vector2Int mousePositionBoard;

    //1フレーム前のボード座標
    public Vector2Int mousePositionBoardBefore;
    //ボード座標の変位を判定
    //PhysicalBoardからカーソルの有無を判定することをやめたのでいらないかも
    public bool Displacement()
    {
        if (mousePositionBoard == mousePositionBoardBefore)return false;
        return true;
    }


    void Start()
    {
        mousePositionBoard = mousePositionBoardBefore = physicalBoard.OUTSIDE;
    }

   

    void Update()
    {
        //1フレーム前のボード座標を代入
        mousePositionBoardBefore = mousePositionBoard;


        //マウスから入力された座標を2次元ベクトルに変換する
        Vector2 mouse = Input.mousePosition;

        //ユニット上の座標に変換する。
        Vector2 mouseU = Camera.main.ScreenToWorldPoint(mouse);
        //小数第2位に丸める。（四捨五入）
        float mousePositionUnitX = Mathf.Round(mouseU.x * 100) / 100;
        float mousePositionUnitY = Mathf.Round(mouseU.y * 100) / 100;

        //ユニット座標
        mousePositionUnit = new Vector2(mousePositionUnitX, mousePositionUnitY);

        //ボード座標。
        mousePositionBoard = Methods.UnitToBoardVector(mouseU);
        //ボード外部ではOUTSIDEにする。
        if(!physicalBoard.JudgeOnBoard(mousePositionBoard))
        {
            mousePositionBoard = physicalBoard.OUTSIDE;
        }

       
    }
}
