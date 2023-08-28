using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitData : MonoBehaviour
{
    //�I�����j�b�g�̃��X�g
    public string[][] initUnitsID = new string[2][];
    //�����z�u
    public Vector2Int[][] initPositions = new Vector2Int[2][];

    private void Start()
    {
        //�W���O�z��̏�����
        for(int i = 0; i < initUnitsID.Length; i++)
        {
            initUnitsID[i] = new string[7];
            initPositions[i] = new Vector2Int[7];
        }
        //���R
        initUnitsID[Methods.PLAYER_ME][0] = "UP000";
        initUnitsID[Methods.PLAYER_ME][1] = "UP000";
        initUnitsID[Methods.PLAYER_ME][2] = "UP000";
        initUnitsID[Methods.PLAYER_ME][3] = "UP000";
        initUnitsID[Methods.PLAYER_ME][4] = "UP000";
        initUnitsID[Methods.PLAYER_ME][5] = "UP000";
        initUnitsID[Methods.PLAYER_ME][6] = "UP000";
        
        //�G�R
        initUnitsID[Methods.PLAYER_ENEMY][0] = "UP000";
        initUnitsID[Methods.PLAYER_ENEMY][1] = "UP000";
        initUnitsID[Methods.PLAYER_ENEMY][2] = "UP000";
        initUnitsID[Methods.PLAYER_ENEMY][3] = "UP000";
        initUnitsID[Methods.PLAYER_ENEMY][4] = "UP000";
        initUnitsID[Methods.PLAYER_ENEMY][5] = "UP000";
        initUnitsID[Methods.PLAYER_ENEMY][6] = "UP000";

        //�����̏����ʒu
        initPositions[Methods.PLAYER_ME][0] = new Vector2Int(1, 3);
        initPositions[Methods.PLAYER_ME][1] = new Vector2Int(0, 1);
        initPositions[Methods.PLAYER_ME][2] = new Vector2Int(3, 3);
        initPositions[Methods.PLAYER_ME][3] = new Vector2Int(2, 4);
        initPositions[Methods.PLAYER_ME][4] = new Vector2Int(5, 0);
        initPositions[Methods.PLAYER_ME][5] = new Vector2Int(7, 1);
        initPositions[Methods.PLAYER_ME][6] = new Vector2Int(6, 3);

        //�G�̏����ʒu
        initPositions[Methods.PLAYER_ENEMY][0] = new Vector2Int(3, 4);
        initPositions[Methods.PLAYER_ENEMY][1] = new Vector2Int(4, 7);
        initPositions[Methods.PLAYER_ENEMY][2] = new Vector2Int(4, 4);
        initPositions[Methods.PLAYER_ENEMY][3] = new Vector2Int(2, 6);
        initPositions[Methods.PLAYER_ENEMY][4] = new Vector2Int(5, 7);
        initPositions[Methods.PLAYER_ENEMY][5] = new Vector2Int(6, 5);
        initPositions[Methods.PLAYER_ENEMY][6] = new Vector2Int(5, 4);
    }
}
