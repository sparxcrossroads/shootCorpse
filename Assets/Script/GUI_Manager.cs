using UnityEngine;
using System.Collections;

public class GUI_Manager : MonoBehaviour {

    public static GUI_Manager Instance = null;

    public int m_score = 0;
    public static int m_maxscore = 0;
    public int m_ball = 100;
    FPSPlayer m_player;

    // UI name
    GUIText text_Ball;
    GUIText text_Life;
    GUIText text_Score;
    GUIText text_MaxScore;

	// Use this for initialization
	void Start () {
        Instance = this;

        // get player
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<FPSPlayer>();
        
        //get ui component
        text_Ball = this.transform.FindChild("text_Ball").GetComponent<GUIText>();
        text_Life = this.transform.FindChild("text_Life").GetComponent<GUIText>();
        text_Score = this.transform.FindChild("text_Score").GetComponent<GUIText>();
        text_MaxScore = this.transform.FindChild("text_MaxScore").GetComponent<GUIText>();

        text_Life.text = "生命: " + m_player.m_life.ToString();
        text_Ball.text = "子弹: " + m_ball.ToString() +"/100";
        text_Score.text = "得 分:" + m_score;
        text_MaxScore.text = "最高分: " + m_maxscore;
	}
	
    //update score
    public void SetScore(int score)
    {
        m_score += score;
        if (m_score > m_maxscore)
            m_maxscore = m_score;

        text_Score.text = "得分: " + m_score;
        text_MaxScore.text = "最高分: " + m_maxscore;

    }

    //update ball
    public void SetBall(int ball)
    {
        m_ball -= ball;
        //if m_ball=0 reset ball
        if (m_ball <= 0)
            m_ball = 100 - m_ball;
        text_Ball.text = "子弹: " + m_ball.ToString() + "/100"; 

    }

    //update life
    public void SetLife(int life)
    {
        text_Life.text = "生命: " + life.ToString();
    }

	// Update is called once per frame
	void Update () {
	
	}
    void OnGUI()
    {
        if (m_player.m_life <= 0)
        {
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.label.fontSize = 40;
            GUI.Label(new Rect(0,0, Screen.width, Screen.height), "you died");

            //restart
            GUI.skin.label.fontSize = 30;
            if(GUI.Button(new Rect(Screen.width*0.5f-150,Screen.height*0.75f,300,40),"continue"))
                Application.LoadLevel(Application.loadedLevelName);
        }
    }
}
