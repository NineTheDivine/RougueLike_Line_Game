using NUnit.Framework;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Piece : MonoBehaviour
{
    //number of minos in piece
    public int block_count;
    //list of mino in piece
    [SerializeField]
    public Tile[] mino_list;
    //Type of Piece
    [SerializeField]
    public Vector3Int[] mino_pos;

    [SerializeField]
    public Global.Piece_Type piece_type;

    private void Start()
    {
        if (!mino_list.Any())
            Assert.True(true, "There is no mino in Piece");
    }
}
