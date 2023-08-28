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

    //�^�C���̕\���F
    //memo �����ɏ����Ă�����
    Color tileOriginal = new Color32(255, 255, 255, 100);
    Color tileDark = new Color32(255, 255, 255, 50);
    Color tileBright = new Color32(150, 200, 255, 200);
    public Color tileRed = new Color32(255, 150, 150, 200);

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

    //TODO Move�ɂ���Ă͂���Ȃ�
    //�ړ����̃��[�g
    public List<Vector2Int> MovingPath { get; set; } = new List<Vector2Int>();




    //��������B��ȓ���B �Q�[���J�n���Ɉ�x�Ă΂��
    public void Activate()
    {
        //�Q�[���{�[�h�𐶐�����B
        MakeGameBoard();

        //�J�[�\���������{����(Basic Operation)�ɂ���
        basicOperation.enabled = true;
        basicOperation.Activate();
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


    //�w�肵���^�C�������̐F�ɖ߂�
    public Tile OriginateTileColor(Vector2Int position)
    {
        Tile normalTile = GetTile(position);
        normalTile.gameObject.GetComponent<SpriteRenderer>().color = tileOriginal;
        return normalTile;
    }
    //�w�肵���^�C���𔖈Â�����i�\���̕⏕�j
    //TOASK�@�F��ς�������Ƃ������@�B
    public Tile DarkenTile(Vector2Int position)
    {
        Tile dimTile = GetTile(position);
        dimTile.gameObject.GetComponent<SpriteRenderer>().color = tileDark;
        return dimTile;
    }
    //�w�肵���^�C���𖾂邭����(�I��\���j
    public Tile LightenTile(Vector2Int position)
    {
        Tile brightTile = GetTile(position);
        brightTile.gameObject.GetComponent<SpriteRenderer>().color = tileBright;
        return brightTile;
    }
    //�w�肵���^�C����Ԃ�����i�x���\���j
    public Tile DyeTileRed(Vector2Int position)
    {
        Tile redTile = GetTile(position);
        redTile.gameObject.GetComponent<SpriteRenderer>().color = tileRed;
        return redTile;
    }

    //�S�Ẵ^�C���̐F���w�肵���F�ɂ���
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

    //�����x�N�g�����{�[�h�̒��ɂɂ��邩���肷��
    public bool JudgeOnBoard(Vector2Int position)
    {
        if (-1 < position.x && position.x < Methods.TILE_X && -1 < position.y && position.y < Methods.TILE_Y) return true;
        return false;
    }

}
