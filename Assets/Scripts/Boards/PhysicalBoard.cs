using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhysicalBoard : MonoBehaviour
{
    //�Q��
    //�Ǘ��X�N���v�g�e��
    [SerializeField] InputManager inputManager;
    [SerializeField] MoveOperation moveOperation;
    [SerializeField] BasicOperation basicOperation;

    //�{�[�h1�}�X���̃^�C���v���t�@�u
    [SerializeField] GameObject tilePrefab;

    //���j�b�g�̈ʒu����Ȃǂ̃f�[�^����������{�[�h
    [SerializeField] DataBoardManager dataBoardManager;


    //�����o�ϐ�
    //�{�[�h���W�O��\���ʒu�x�N�g���B��������ɑ��p
    public readonly Vector2Int OUTSIDE = new Vector2Int(-1, -1);

    //�^�C���̈ʒu��c������f�[�^��̃{�[�h
    private Tile[,] tileBoard = new Tile [Methods.TILE_X, Methods.TILE_Y]; 
    //�Q�b�^�[
    public Tile GetTile(Vector2Int position)
    {
        if(position == OUTSIDE)
        {
            Debug.Log("vector is out of range.");
            return null;
        }
        return tileBoard[position.x, position.y];
    }


    //��������B��ȓ���B �Q�[���J�n���Ɉ�x�Ă΂��
    public void Activate()
    {
        //�Q�[���{�[�h�i���ԁj�𐶐�����B
        MakeGameBoard();

        //DataBaord�ɃA�N�Z�X����
        dataBoardManager.Activate();
    }

    //�Q�[���{�[�h���̍��W���w�肵�ă^�C���i�{�[�h��1�}�X���j�𐶐�����֐�
    //(����)�Q�[���J�n���ɂ����g��Ȃ��̂Ńx�N�g�����g��Ȃ��B
    public GameObject GenerateATile(int BoardX, int BoardY)
    {
        //��������^�C���̈ʒu�x�N�g�������
        Vector3 tileTransformPosition = Methods.BoardToUnitVector( new (BoardX, BoardY));
        tileTransformPosition.z = Methods.BOARD_Z;
        
        //�^�C���𐶐�
        GameObject tile = Instantiate(tilePrefab, tileTransformPosition,Quaternion.identity);

        //�Q�[���{�[�h�̑傫���ɂ���ă^�C���̑傫����ς���
        float size = (float)Methods.GAMEBOARD_MAX / (float)(Mathf.Max(Methods.TILE_Y, Methods.TILE_X));
        tile.transform.localScale = new Vector3 (size, size, 0);
        //�Q�[���{�[�h�I�u�W�F�N�g�̎q�I�u�W�F�N�g�ɂ���
        tile.transform.SetParent(this.transform);
        return tile;
    }

    //�Q�[���{�[�h��TILE_X * TILE_Y�����^�C�����쐬����B
    public void MakeGameBoard()
    {
        for(int i = 0; i < Methods.TILE_X; i++)
        {
            for(int j = 0; j< Methods.TILE_Y; j++)
            {
                //(i, j) �ɐ���
                GameObject tile = GenerateATile(i, j);
                tile.name = "Tile[" + i + "][" + j + "]"; 

                //�^�C�����̂Ƀ{�[�h���W����������
                Tile tileScript = tile.GetComponent<Tile>();
                tileScript.PositionBoard = new Vector2Int(i, j);

                //�^�C����Activate����
                tileScript.Activate();

                //�^�C�����^�C���{�[�h�i�{�[�h�S�̂��^�C���I�u�W�F�N�g�̈ʒu����肷��j�ɑ������
                tileBoard[i, j] = tileScript;
            }
        }
    }


    //�����x�N�g�����{�[�h�̒��ɂ��邩���肷��
    public bool JudgeOnBoard(Vector2Int position)
    {
        if (-1 < position.x && position.x < Methods.TILE_X && -1 < position.y && position.y < Methods.TILE_Y) return true;
        return false;
    }


    //�^�C���̐F�ύX���\�b�h
    //�x�N�g���w��B�F�̕ύX
    public bool ChangeTileColor(Vector2Int position, Color color)
    {
        GetTile(position).GetComponent<SpriteRenderer>().color = color;
        return true;
    }
    //�^�C���S�Ă̐F��ύX���郁�\�b�h
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
