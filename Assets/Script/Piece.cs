using NUnit.Framework;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    //number of minos in piece
    public int block_count;
    //The center of the Spin point. usually (0,0)
    [SerializeField]
    public Vector3 mid_point;
    //list of mino in piece
    [SerializeField]
    public Mino[] mino_list;
    //Type of Piece
    [SerializeField]
    public Vector3[] mino_pos;

    [SerializeField]
    public Global.Piece_Type piece_type;

    private void Start()
    {
        if (!mino_list.Any())
            Assert.True(true, "There is no mino in Piece");
        var temp_Color = Global.Piece_Color_Dict[piece_type];
        temp_Color.a = 0.3f;
        for (int i = 0; i < this.block_count; i++)
        {
            Mino m = Instantiate(mino_list[i], this.transform);
            m.transform.GetChild(0).GetComponent<SpriteRenderer>().color = temp_Color;
            m.transform.localPosition = mino_pos[i] * Global.scale_background;
        }
    }
}
