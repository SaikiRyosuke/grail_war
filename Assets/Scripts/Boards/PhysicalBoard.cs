using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhysicalBoard : MonoBehaviour
{
    //参照
    //管理スクリプト各種
    [SerializeField] InputManager inputManager;
    [SerializeField] MoveOperation moveOperation;
    [SerializeField] BasicOperation basicOperation;

    //ボード1マス分のタイルプレファブ
    [SerializeField] GameObject tilePrefab;

    //ユニットの位置特定などのデータ処理をするボード
    [SerializeField] DataBoardManager dataBoardManager;


    //メンバ変数
    //ボード座標外を表す位置ベクトル。判定条件に多用
    public readonly Vector2Int OUTSIDE = new Vector2Int(-1, -1);

    //タイルの位置を把握するデータ上のボード
    private Tile[,] tileBoard = new Tile [Methods.TILE_X, Methods.TILE_Y]; 
    //ゲッター
    public Tile GetTile(Vector2Int position)
    {
        if(position == OUTSIDE)
        {
            Debug.Log("vector is out of range.");
            return null;
        }
        return tileBoard[position.x, position.y];
    }


    //初期動作。主な動作。 ゲーム開始時に一度呼ばれる
    public void Activate()
    {
        //ゲームボード（実態）を生成する。
        MakeGameBoard();

        //DataBaordにアクセスする
        dataBoardManager.Activate();
    }

    //ゲームボード内の座標を指定してタイル（ボードの1マス分）を生成する関数
    //(メモ)ゲーム開始時にしか使わないのでベクトルを使わない。
    public GameObject GenerateATile(int BoardX, int BoardY)
    {
        //生成するタイルの位置ベクトルを作る
        Vector3 tileTransformPosition = Methods.BoardToUnitVector( new (BoardX, BoardY));
        tileTransformPosition.z = Methods.BOARD_Z;
        
        //タイルを生成
        GameObject tile = Instantiate(tilePrefab, tileTransformPosition,Quaternion.identity);

        //ゲームボードの大きさによってタイルの大きさを変える
        float size = (float)Methods.GAMEBOARD_MAX / (float)(Mathf.Max(Methods.TILE_Y, Methods.TILE_X));
        tile.transform.localScale = new Vector3 (size, size, 0);
        //ゲームボードオブジェクトの子オブジェクトにする
        tile.transform.SetParent(this.transform);
        return tile;
    }

    //ゲームボードにTILE_X * TILE_Yだけタイルを作成する。
    public void MakeGameBoard()
    {
        for(int i = 0; i < Methods.TILE_X; i++)
        {
            for(int j = 0; j< Methods.TILE_Y; j++)
            {
                //(i, j) に生成
                GameObject tile = GenerateATile(i, j);
                tile.name = "Tile[" + i + "][" + j + "]"; 

                //タイル自体にボード座標をもたせる
                Tile tileScript = tile.GetComponent<Tile>();
                tileScript.PositionBoard = new Vector2Int(i, j);

                //タイルをActivateする
                tileScript.Activate();

                //タイルをタイルボード（ボード全体がタイルオブジェクトの位置を特定する）に代入する
                tileBoard[i, j] = tileScript;
            }
        }
    }


    //整数ベクトルがボードの中にあるか判定する
    public bool JudgeOnBoard(Vector2Int position)
    {
        if (-1 < position.x && position.x < Methods.TILE_X && -1 < position.y && position.y < Methods.TILE_Y) return true;
        return false;
    }


    //タイルの色変更メソッド
    //ベクトル指定。色の変更
    public bool ChangeTileColor(Vector2Int position, Color color)
    {
        GetTile(position).GetComponent<SpriteRenderer>().color = color;
        return true;
    }
    //タイル全ての色を変更するメソッド
    public void DyeAllTilesTo(Color color)
    {
        for (int i = 0; i < Methods.TILE_X; i++)
        {
            for (int j = 0; j < Methods.TILE_Y; j++)
            {
                GetTile(new Vector2Int(i, j)).gameObject.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
}
