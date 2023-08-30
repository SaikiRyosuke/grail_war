using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileColor : MonoBehaviour
{
    //タイルの色を定義します。
    //TODO 透明を使っていることがよくないかもしれない
    //参照ページ
    //https://www.rapidtables.org/ja/web/color/color-wheel.html
    //https://color.adobe.com/ja/create/color-wheel

    //使用する色。外部から参照不可。
    static Color original        = new Color32(255, 255, 255, 100);//灰色標準
    static Color paleDark        = new Color32(255, 255, 255, 50);//暗め
    static Color dark            = new Color32(100, 100, 100, 100);//黒め

    static Color bibidLightBlue  = new Color32(150, 200, 255, 200);//水色明るめ
    static Color lightBlue       = new Color32(135, 190, 255, 200);//水色

    static Color red             = new Color32(255, 150, 150, 200);//赤
    static Color lightMint       = new Color32(149, 254, 255, 240);//明るいミント

    //操作状況ごとにわける

    //Basic
    //標準の色
    readonly public static Color basicOriginal = original;
    //カーソルの色。ユニットがいない場合。
    readonly public static Color basicPointTile = paleDark;
    //カーソルの色。ユニットがいる場合。
    readonly public static Color basicPointUnit = bibidLightBlue;

    //Move
    //標準の色
    readonly public static Color moveOriginal = original;
    //TODO 移動主体のユニットの色
    //移動主体のユニットのタイルの色
    readonly public static Color moveMovingUnit;
    //移動経路の色。
    readonly public static Color movePassedTile = paleDark;
    //カーソルの色。
    readonly public static Color movePointTile = bibidLightBlue;
    //TODO　味方と敵の色の識別
    //移動経路またはカーソルがある箇所に味方ユニットがいる場合
    readonly public static Color movePointMEUnit = red;
    //移動経路またはカーソルがある箇所に敵ユニットがいる場合
    readonly public static Color movePointENEMYUnit = red;

    //Attack
    //レンジ範囲外
    readonly public static Color attackRangeOutside = original;
    //レンジ範囲内
    //レンジ範囲内の標準の色
    readonly public static Color attackRangeInside = paleDark;
    //攻撃主体の色
    readonly public static Color attackAttacker = original;
    //TODO 味方ユニット　色
    //味方ユニットの色
    readonly public static Color attackRangeMEUnit;
    //敵ユニットの色
    readonly public static Color attackRangeENEMYUnit = lightBlue;
    //カーソルの色。
    readonly public static Color attackCursorTile = dark;
    //カーソルの色。味方ユニットがいる場合。
    readonly public static Color attackCursorMEUnit = red;
    //カーソルの色。敵ユニットがいる場合。
    readonly public static Color attackCursorENEMYUnit = lightMint;

    //Choice
    //まだマウスによるタイル操作がない

}