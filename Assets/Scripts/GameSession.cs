using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public GameOverScreen gameOverScreen;
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public TextMeshProUGUI topScoreText;
    [SerializeField] public TextMeshProUGUI levelText;
    [SerializeField] public TextMeshProUGUI linesText;
    [SerializeField] public TextMeshProUGUI textS;
    [SerializeField] public TextMeshProUGUI textZ;
    [SerializeField] public TextMeshProUGUI textL;
    [SerializeField] public TextMeshProUGUI textJ;
    [SerializeField] public TextMeshProUGUI textT;
    [SerializeField] public TextMeshProUGUI textO;
    [SerializeField] public TextMeshProUGUI textI;

    private int valueS = 0;
    private int valueZ = 0;
    private int valueL = 0;
    private int valueJ = 0;
    private int valueT = 0;
    private int valueO = 0;
    private int valueI = 0;

    private int bestScore = 0;
    private int linesForLevel = 0;
    private int currentScore = 0;
    private int currentLevel = 0;
    private int cleaningLines = 0;

    private int scoreMin = 0;
    private int scoreMax = 999_999;
    private int linesMin = 0;
    private int linesMax = 999;
    private int levelMin = 0;
    private int levelMax = 99;
    private int tetrominoesMin = 0;
    private int tetrominoesMax = 999;

    // Start is called before the first frame update
    void Start()
    {
        bestScore = PlayerPrefs.GetInt("HighScore", 0);
        ShowCleaningLines(cleaningLines);
        ShowLevel(currentLevel);
        ShowScore(currentScore);
        ShowBestScore(bestScore);
    }

    public void AddCurrentTetromino(ShapeData shapeData)
    {
        switch(shapeData.shape)
        {
            case Shape.S:
                AddShapeS();
                break;
            case Shape.Z:
                AddShapeZ();
                break;
            case Shape.L:
                AddShapeL();
                break;
            case Shape.J:
                AddShapeJ();
                break;
            case Shape.T:
                AddShapeT();
                break;
            case Shape.O:
                AddShapeO();
                break;
            case Shape.I:
                AddShapeI();
                break;

        }
    }

    private void AddShapeS()
    {
        valueS = Mathf.Clamp(valueS + 1, tetrominoesMin, tetrominoesMax);
        textS.text = (valueS + 1000).ToString().Substring(1);
    }
    private void AddShapeZ()
    {
        valueZ = Mathf.Clamp(valueZ + 1, tetrominoesMin, tetrominoesMax);
        textZ.text = (valueZ + 1000).ToString().Substring(1);
    }
    private void AddShapeL()
    {
        valueL = Mathf.Clamp(valueL + 1, tetrominoesMin, tetrominoesMax);
        textL.text = (valueL + 1000).ToString().Substring(1);
    }
    private void AddShapeJ()
    {
        valueJ = Mathf.Clamp(valueJ + 1, tetrominoesMin, tetrominoesMax);
        textJ.text = (valueJ + 1000).ToString().Substring(1);
    }
    private void AddShapeT()
    {
        valueT = Mathf.Clamp(valueT + 1, tetrominoesMin, tetrominoesMax);
        textT.text = (valueT + 1000).ToString().Substring(1);
    }
    private void AddShapeO()
    {
        valueO = Mathf.Clamp(valueO + 1, tetrominoesMin, tetrominoesMax);
        textO.text = (valueO + 1000).ToString().Substring(1);
    }
    private void AddShapeI()
    {
        valueI = Mathf.Clamp(valueI + 1, tetrominoesMin, tetrominoesMax);
        textI.text = (valueI + 1000).ToString().Substring(1);
    }

    public void AddScore(int countLines)
    {
        AddCountCleaningLines(countLines);
        switch (countLines)
        {
            case 1:
                currentScore = Mathf.Clamp(currentScore + 40 * (currentLevel + 1), scoreMin, scoreMax);
                break;
            case 2:
                currentScore = Mathf.Clamp(currentScore + 100 * (currentLevel + 1), scoreMin, scoreMax);
                break;
            case 3:
                currentScore = Mathf.Clamp(currentScore + 300 * (currentLevel + 1), scoreMin, scoreMax);
                break;
            case 4:
                currentScore = Mathf.Clamp(currentScore + 1200 * (currentLevel + 1), scoreMin, scoreMax);
                break;
        }
        ShowScore(currentScore);
    }

    private void AddCountCleaningLines(int countLines)
    {
        if (linesForLevel + countLines >= 10)
        {
            NextLevel();
            linesForLevel = linesForLevel+countLines-10;
            cleaningLines = Mathf.Clamp(cleaningLines + countLines, linesMin, linesMax);
        }
        else
        {
            linesForLevel += countLines;
            cleaningLines = Mathf.Clamp(cleaningLines + countLines, linesMin, linesMax);
        }
        ShowCleaningLines(cleaningLines);
    }

    private void ShowCleaningLines(int cleaningLines)
    {
        if (cleaningLines < 10) { linesText.text = "00" + cleaningLines.ToString(); }
        else if (cleaningLines < 100) { linesText.text = "0" + cleaningLines.ToString(); }
        else if (cleaningLines < 1000) { linesText.text = cleaningLines.ToString(); }
        else { linesText.text = "999"; }
    }

    private void NextLevel()
    {
        currentLevel = Mathf.Clamp(currentLevel + 1, levelMin, levelMax);
        ShowLevel(currentLevel);
    }

    private void ShowLevel(int currentLevel)
    {
        if (currentLevel<100)
        {
            levelText.text = (100 + currentLevel).ToString().Substring(1);
        }
        else
        {
            currentLevel = 99;
            levelText.text = currentLevel.ToString();
        }
    }

    private void ShowScore(int score)
    {
        if(currentScore<1_000_000)
        {
            scoreText.text = (1_000_000 + currentScore).ToString().Substring(1);
        }
        else
        {
            score = 999999;
            scoreText.text = score.ToString();
        }
    }

    private void ShowBestScore(int bestScore)
    {
        topScoreText.text=(1000000+bestScore).ToString().Substring(1);
    }

    public void GameOver()
    {
        gameOverScreen.Setup(scoreText.text);
        BestScoreResult();
    }

    private void BestScoreResult()
    {
        if(currentScore> bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("HighScore", currentScore);
        }
    }
}
