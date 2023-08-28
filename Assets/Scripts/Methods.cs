using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// staticなメソッドや定数やリテラルをなるべく集めました。
/// staticの宣言はこのスクリプトにしかありません。
/// </summary>
public class Methods : MonoBehaviour
{
    //ゲームボードの大きさの最大値（ユニット）
    public const int GAMEBOARD_MAX = 8;
    //マス目の数。今は8*8;
    public const int TILE_X = 8;
    public const int TILE_Y = 8;

    //プレイヤーの識別番号
    public const int PLAYER_ME = 0;
    public const int PLAYER_ENEMY = 1;
    public readonly int[] PLAYER_INDEXES = {0, 1};
    //ユニットの最大値
    public const int UNIT_MAX = 7;

    //オブジェクトをインスタンス化するときのz座標
    // ハンドラー：1, ボード：-1, パネル：-2, キャラ：-3
    public const float HANDLER_Z = 1;
    public const float BOARD_Z = -1;
    public const float PAPNEL_Z = -2;
    public const float UNIT_Z = -3;

    //ユニットの選択肢の数
    public const int UNIT_CHOICE_MAX = 5;

    //オブジェクトのタグ
    public const string CONTROLLER = "GameController";


    //ボード座標を反転する。
    public static Vector2Int ReverseXY(Vector2Int pos)
    {
        return new Vector2Int(TILE_X - pos.x - 1, TILE_Y - pos.y - 1);
    }


    //ボードとユニティ間の座標返還
    public static Vector2 BoardToUnitVector(Vector2Int vectorB)
    {
        //平行移動の変換
        float a = (float)-(TILE_X - 1) / 2;
        float b = (float)-(TILE_Y - 1) / 2;

        //スケールの変換
        float TilesCount = Mathf.Max(TILE_X, TILE_Y);
        float ScaleChange = (float)GAMEBOARD_MAX / TilesCount;

        Vector2 vectorU = new Vector2(ScaleChange * (vectorB.x + a), ScaleChange * (vectorB.y + b));
        return vectorU;

    }
    public static Vector2Int UnitToBoardVector(Vector2 vectorU)
    {
        //平行移動の変換(既にボード座標。スケール変換不必要）
        float a = (float)(TILE_X - 1) / 2;
        float b = (float)(TILE_Y - 1) / 2;

        //スケールの変換
        float TilesCount = Mathf.Max(TILE_X, TILE_Y);
        float ScaleChange = TilesCount / (float)GAMEBOARD_MAX;

        Vector2Int vectorB = new Vector2Int((int)Mathf.Round((ScaleChange * vectorU.x + a)), (int)Mathf.Round((ScaleChange * vectorU.y + b)));
        return vectorB;

    }

    //ボード座標の指定をユニット表示用の座標に直す変換
    public static Vector3 BoardToUnitDisplay(Vector2Int position)
    {
        //スケールの変換
        float TilesCount = Mathf.Max(TILE_X, TILE_Y);
        float ScaleChange = (float)GAMEBOARD_MAX / TilesCount;
        //y座標を半マス増やす
        Vector3 displayPosition = BoardToUnitVector(position) + new Vector2(0, 1) * ScaleChange / 2;
        //z座標をセットする
        displayPosition.z = UNIT_Z; 
        return displayPosition;
    }
   
    //ユークリッド距離の測定
    public static float EuclidDistanceBetween(Vector2Int positionA, Vector2Int positionB)
    {
        float distance = (positionA.x - positionB.x) * (positionA.x - positionB.x) + (positionA.y - positionB.y) * (positionA.y - positionB.y);
        distance = Mathf.Sqrt(distance);
        return distance;
    }
}
