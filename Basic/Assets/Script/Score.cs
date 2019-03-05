using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    void Awake()
    {
        Score.text = null;
        Score.score = -1;
        Score.scorePreStr = "得分 : ";
    }

    // Use this for initialization
    void Start() {
        if(Score.text == null)
        {
            //Debug.Log(this.GetComponent<Transform>().GetChild(0).name);
            Score.text = this.GetComponent<Transform>().GetChild(0).GetComponent<Text>();
        }
        //Debug.Log(text.ToString());
    }

    // Update is called once per frame
    void Update() {
        Score.text.text = Score.scorePreStr + Score.score.ToString();
    }

    static public void AddScore()
    {
        Score.score++;
    }

    static public int GetScore
    {
        set
        {
            Score.score = value;
        }
        get
        {
            return Score.score;
        }
    }

    static Text text;
    static int score;
    static string scorePreStr;
}
