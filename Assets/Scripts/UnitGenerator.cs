using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// ゲーム開始時に一度だけUnitManagerによりユニットが生成されます
/// この際に主に一度だけ使用する用途のメソッドを集めたのがUnitGeneratorです
/// </summary>

public class UnitGenerator : MonoBehaviour
{
    //インスペクタ指定
    //ユニットのプレファブとID
    [SerializeField] List<GameObject> units;
    [SerializeField] List<string> unitIDs = new List<string>();



    //定数ですが閉じているのでここにかきます
    //味方の色
    Color ME_COLOR = new Color32(48, 64, 255, 200);
    //敵の色
    Color ENEMY_COLOR = new Color32(246, 20, 118, 200);
    //プレイヤーの色の配列
    Color[] PLAYER_COLORS = { new Color32(48, 64, 255, 200), new Color32(246, 20, 118, 200) };


    //ユニットのIDを指定して該当するユニットのオブジェクトを生成（内部情報のみ）
    public UnitGeneral GenerateUnitByName(string unitID)
    {
        int index = unitIDs.FindIndex(a => a == unitID);
        if (index == -1)
        {
            Debug.LogError("そんな名前のユニットはいない: " + unitID);
            return Instantiate(units[0]).GetComponent<UnitGeneral>();
        }
        return Instantiate(units[index]).GetComponent<UnitGeneral>();
    }

    //ユニットにプレイヤー識別用に色を付ける
    public void SetColor(UnitGeneral unit)
    {
        //ユニットのHPBarから「Frame」を取得
        //ユニットの足元の楕円「Identifier」を取得
        GameObject bar = unit.transform.Find("HPBar").transform.Find("Frame").gameObject;  
        GameObject identifier = unit.transform.Find("Identifier").gameObject;

        //unitの保持するPlayerIndexに応じて色を変える。
        bar.GetComponent<SpriteRenderer>().color = PLAYER_COLORS[unit.PlayerIndex];
        identifier.GetComponent<SpriteRenderer>().color = PLAYER_COLORS[unit.PlayerIndex];
    }
}
