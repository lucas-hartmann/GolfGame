using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private float par;

    public int score = 0;
    private int stroke = 0;

    public TMP_Text scoreBar;
    public TMP_Text strokesBar;
    public TMP_Text result;

    public GameObject gameVictoryUI;
    public GameObject gameOverUI;
    public GameObject buttonUI;



    void Start()
    {
        strokesBar.text = "STROKES: " + stroke + "/6";
        scoreBar.text = "SCORE:" + score;
    }


    public void AddScore(int amount)
        {
            score += amount;
            scoreBar.text = "SCORE:" + score;
        }

    public void AddStroke(int amount)
    {
        stroke += amount;
        strokesBar.text = "STROKES: " + stroke + "/6";
        if (stroke == 7){
            GameOver();
        }
    }

    public void GameOver(){
        gameOverUI.SetActive(true);
        buttonUI.SetActive(true);

    }

    public void EndGame(){
        gameVictoryUI.SetActive(true);
        buttonUI.SetActive(true);
        CalcResText();
        //result.text = "In " + stroke + " stroke(s)!";

    }

    public void CalcResText()
    {
        int diff = stroke - (int)par;
        string resultText = "";

        if (stroke == 1)
        {
            resultText = "Hole-in-One!";
        }
        else
        {
            switch (diff)
            {
                case -2:
                    resultText = "Eagle!";
                    break;
                case -1:
                    resultText = "Birdie!";
                    break;
                case 0:
                    resultText = "Par!";
                    break;
                case 1:
                    resultText = "Bogey!";
                    break;
                default:
                    if (diff > 1)
                        resultText = diff + " over Par";
                    else
                        resultText = (-diff) + " under Par";
                    break;
            }
        }

        //result.text = resultText + "\nIn " + stroke + " stroke(s)!";
        result.text = resultText;

    }
}
