using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTile : MonoBehaviour
{
    [SerializeField] PhysicalBoard physicalBoard;

    //タイルの色を定義します。
    //透明を使っていることがよくないかもしれない
    //参照ページ
    //https://www.rapidtables.org/ja/web/color/color-wheel.html
    //https://color.adobe.com/ja/create/color-wheel
    /*
    灰色標準     = new Color32(255, 255, 255, 100);
    暗め         = new Color32(255, 255, 255, 50);
    黒め         = new Color32(100, 100, 100, 100);

    水色明るめ   = new Color32(150, 200, 255, 200);
    水色         = new Color32(135, 190, 255, 200);

    赤           = new Color32(255, 150, 150, 200);
    明るいミント = new Color32(149, 254, 255, 240);
    */

    //操作状況ごとにわける

    //Basic
    //標準の色
    public static Color basicOriginal = new Color32(255, 255, 255, 100);
    //カーソルの色。ユニットがいない場合。
    public static Color basicPointTile = new Color32(255, 255, 255, 50);
    //カーソルの色。ユニットがいる場合。
    public static Color basicPointUnit = new Color32(150, 200, 255, 200);



    //Move
    //標準の色
    public static Color moveOriginal = new Color32(255, 255, 255, 100);
    //TODO 移動主体のユニットの色
    //移動主体のユニットのタイルの色
    public static Color moveMovingUnit;
    //移動経路の色。
    public static Color movePassedTile = new Color32(255, 255, 255, 50);
    //カーソルの色。
    public static Color movePointTile = new Color32(150, 200, 255, 200);
    //TODO　味方と敵の色の識別
    //移動経路またはカーソルがある箇所に味方ユニットがいる場合
    public static Color movePointMEUnit = new Color32(255, 150, 150, 200);
    //移動経路またはカーソルがある箇所に敵ユニットがいる場合
    public static Color movePointENEMYUnit = new Color32(255, 150, 150, 200);

    //Attack
    //レンジ範囲外
    public static Color attackOutside = new Color32(255, 255, 255, 100);
    //レンジ範囲内
    //レンジ範囲内の標準の色
    public static Color attackInside = new Color32(255, 255, 255, 50);
    //攻撃主体の色
    public static Color attacker = new Color32(150, 200, 255, 200);
    //TODO 味方ユニット　色
    //味方ユニットの色
    public static Color attackIndicateMEUnit;
    //敵ユニットの色
    public static Color attackIndicateENEMYUnit = new Color32(135, 190, 255, 200);
    //カーソルの色。
    public static Color attackPointTile = new Color32(100, 100, 100, 100);
    //カーソルの色。味方ユニットがいる場合。
    public static Color attackPointMEUnit = new Color32(255, 150, 150, 200);
    //カーソルの色。敵ユニットがいる場合。
    public static Color attackPointENEMYUnit = new Color32(149, 254, 255, 240);

    //Choice
    //まだマウスによるタイル操作がない

    //色の変更メソッド
    public bool ChangeTileColor(Vector2Int position, Color color)
    {
        physicalBoard.GetTile(position).GetComponent<SpriteRenderer>().color = color;
        return true;
    }

    public void DyeAllTilesTo(Color color)
    {
        for (int i = 0; i < Methods.TILE_X; i++)
        {
            for (int j = 0; j < Methods.TILE_Y; j++)
            {
                physicalBoard.GetTile(new Vector2Int(i, j)).gameObject.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
}
