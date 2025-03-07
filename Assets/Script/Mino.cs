using UnityEngine;
public class Mino : MonoBehaviour
{
    public Global.Mino_Type m_type = Global.Mino_Type.Normal;

    private void Awake()
    {
        this.GetComponent<Transform>().localScale *= Global.scale_background;
    }
}
