using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 全てのユニットに共通にするデータ
/// </summary>
public class UnitGeneral : MonoBehaviour
{
    //参照
    //各種管理スクリプト
    GameObject controller;
    InputManager inputManager;
    MoveOperation moveOperation;
    PhysicalBoard physicalBoard;
    DataBoardManager dataBoardManager;

    [SerializeField] GameObject hpBar;

    //「内部情報」
    //TOASK public private本当に分からない。
    /// <summary>
    /// ここだったらunitのidというのは個別に決まってていて外部からは読み取りしかできないようにしたい。
    /// US000.csにreadonlyで書くのが一番無難？
    /// インスペクタはデータ消えそうで使いたくない
    /// US000で初期化したい
    /// </summary>
    //このユニットのID
    public string UnitID { get; set; } = null;
    //能力初期値
    protected float INIT_HP;
    //能力値
    //攻撃射程
    public float atkRange;
    //体力
    public float HP { get; set; } = 0;
    //「外部情報」
    //(静的情報）
    //このユニットの所有プレイヤー
    public int PlayerIndex { get; set; }

    //(動的情報）
    //このユニットのボード座標
    public Vector2Int Position { get; set; }
    //このユニットの画像表示の前後関係（order)
    public int DisplayOrder { get; set; } = 0;
    //このユニットがMove状態であるか
    public bool IsMoving { get; set; } = false;
    //このユニットがAttack状態であるか
    public bool IsAttacking { get; set; } = false;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag(Methods.CONTROLLER);
        inputManager = controller.GetComponent<InputManager>();
        moveOperation = controller.GetComponent<MoveOperation>();

    }

    private void Update()
    {
        //ユニット間の画像の前後関係をy座標に依存してきめる 
        DisplayOrder = Methods.TILE_Y - Position.y - 1;
        this.gameObject.GetComponent <SortingGroup>().sortingOrder = DisplayOrder;

        this.gameObject.transform.position = Methods.BoardToUnitDisplay(Position);

        DisplayHPBar();
        //UpdateInner();

    }
    virtual protected void UpdateInner(){
        // こいつだけUpdateタイミングでやりたいということを継承先でここに書く
    }
    
    // 固有能力を使用、失敗したときfalseを返す
    virtual public bool UseSkill()
    {
        // 継承したクラスで内容を記述
        //throw new NotImplementedException();
        return false;
    }
    
    //HPをゲージに反映
    private bool DisplayHPBar()
    {
        float leftHPScale;
        if(HP >= 0)
        {
            leftHPScale = HP / INIT_HP;
        }
        else
        {
            leftHPScale = 0;
            Debug.Log("HP less than 0");
        }

        Vector3 scale = hpBar.transform.localScale;
        scale.x = leftHPScale;
        hpBar.transform.localScale = scale;
        Vector3 position = hpBar.transform.parent.position;
        //magic number
        position.x -= 0.75f * (1 - leftHPScale) / 2;
        hpBar.transform.position = position;

        return true;
    }
}
