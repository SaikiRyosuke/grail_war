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

    //タイルの表示色
    //memo ここに書いていいか
    Color tileOriginal = new Color32(255, 255, 255, 100);
    Color tileDark = new Color32(255, 255, 255, 50);
    Color tileBright = new Color32(150, 200, 255, 200);
    public Color tileRed = new Color32(255, 150, 150, 200);

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

    //TODO Moveによってはいらない
    //移動時のルート
    public List<Vector2Int> MovingPath { get; set; } = new List<Vector2Int>();




    //初期動作。主な動作。 ゲーム開始時に一度呼ばれる
    public void Activate()
    {
        //ゲームボードを生成する。
        MakeGameBoard();

        //カーソル操作を基本操作(Basic Operation)にする
        basicOperation.enabled = true;
        basicOperation.Activate();
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


    //指定したタイルを元の色に戻す
    public Tile OriginateTileColor(Vector2Int position)
    {
        Tile normalTile = GetTile(position);
        normalTile.gameObject.GetComponent<SpriteRenderer>().color = tileOriginal;
        return normalTile;
    }
    //指定したタイルを薄暗くする（表示の補助）
    //TOASK　色を変えるもっといい方法。
    public Tile DarkenTile(Vector2Int position)
    {
        Tile dimTile = GetTile(position);
        dimTile.gameObject.GetComponent<SpriteRenderer>().color = tileDark;
        return dimTile;
    }
    //指定したタイルを明るくする(選択表示）
    public Tile LightenTile(Vector2Int position)
    {
        Tile brightTile = GetTile(position);
        brightTile.gameObject.GetComponent<SpriteRenderer>().color = tileBright;
        return brightTile;
    }
    //指定したタイルを赤くする（警告表示）
    public Tile DyeTileRed(Vector2Int position)
    {
        Tile redTile = GetTile(position);
        redTile.gameObject.GetComponent<SpriteRenderer>().color = tileRed;
        return redTile;
    }

    //全てのタイルの色を指定した色にする
    public void DyeAllTilesTo(Color color)
    {
        for(int i = 0; i < Methods.TILE_X; i++)
        {
            for(int j = 0; j < Methods.TILE_Y; j++)
            {
                tileBoard[i, j].gameObject.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }

    //整数ベクトルがボードの中ににあるか判定する
    public bool JudgeOnBoard(Vector2Int position)
    {
        if (-1 < position.x && position.x < Methods.TILE_X && -1 < position.y && position.y < Methods.TILE_Y) return true;
        return false;
    }

}
