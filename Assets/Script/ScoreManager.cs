using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int Score { get; set; }
    public int Combo;
    public int B2B;

    public GameObject Score_Text;
    public GameObject Target_Text;
    public GameObject B2B_Text;
    public GameObject Combo_Text;
    public GameObject Level_Text;
    public GameObject TSpin_Text;

    private void Awake()
    {
        this.Score = 0;
        this.Combo = 0;
        this.B2B = 0;

        Score_Update();
        Color TTextColor = TSpin_Text.GetComponent<TextMeshPro>().color;
        TTextColor.a = 0.0f;
        TSpin_Text.GetComponent<TextMeshPro>().color = TTextColor;
    }

    public int Score_PerfectClear(int current_level)
    {
        int perfect_score = (Global.TargetScore[current_level - 1] - Score) / 2;
        this.Score += perfect_score;
        return perfect_score;
    }

    public int Score_Full_Line(int linecount, bool is_TSpin, bool is_Combo)
    {
        float multiplier_quadplus = 1.0f;
        while (linecount > 5)
        {
            multiplier_quadplus *= 1.5f;
            linecount--;
        }
        float linescore = Global.Score_Line_Data[linecount] * multiplier_quadplus;

        if (is_TSpin)
        {
            if (TSpin_Text.GetComponentInParent<Animation>().isPlaying)
                TSpin_Text.GetComponentInParent<Animation>().Stop();
            string text = (linecount == 1 ? "Single" : (linecount == 2 ? "Double" : (linecount == 3 ? "Triple" : null)));
            Assert.IsNotNull(text);
            TSpin_Text.GetComponent<TextMeshPro>().text = "T-Spin\n" + text;
            TSpin_Text.GetComponentInParent<Animation>().Play("TSpinText_Invisible");


            linescore += Global.Score_TSpin_Data[linecount];
        }

        if (linecount >= 4 || is_TSpin)
        {
            this.B2B++;
            linescore += B2B_Counter_Score();
        }
        else
            this.B2B = 0;
        if (is_Combo)
        {
            this.Combo++;
            linescore *= Combo_Counter_Score();
        }
        else
        {
            this.Combo= 0;
        }
        this.Score += (int)linescore;
        return (int)linescore;
    }

    private int B2B_Counter_Score()
    {
        int index = 0;
        while (index < Global.Score_B2B_Data.Count - 1 && this.B2B >= Global.Score_B2B_Data.ElementAt(index+1).Key)
            index++;
        return this.B2B * Global.Score_B2B_Data.ElementAt(index).Value;
    }

    private float Combo_Counter_Score()
    {
        int index = 0;
        while (index < Global.Score_Combo_Data.Count - 1 && this.B2B >= Global.Score_Combo_Data.ElementAt(index + 1).Key)
            index++;
        return Global.Score_Combo_Data.ElementAt(index).Value;
    }

    public void Score_Update()
    {
        Score_Text.GetComponent<TextMeshPro>().text = this.Score.ToString().PadLeft(8,'0');
        Combo_Text.GetComponent<TextMeshPro>().text = this.Combo.ToString();
        B2B_Text.GetComponent<TextMeshPro>().text = this.B2B.ToString();
    }
    public void Level_Update(int cur_level, int max_level)
    {
        Level_Text.GetComponent<TextMeshPro>().text = cur_level.ToString() + " / " + max_level.ToString();
        Target_Text.GetComponent<TextMeshPro>().text = Global.TargetScore[cur_level - 1].ToString().PadLeft(8, '0');
    }
}
