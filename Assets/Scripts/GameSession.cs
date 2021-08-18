using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI linesText;

    [SerializeField] int currentScore = 0;
    [SerializeField] int currentLevel = 0;
    [SerializeField] int cleaningLines = 0;
    [SerializeField] int linesForLevel = 0;
    // Start is called before the first frame update
    void Start()
    {
        ShowCleaningLines(cleaningLines);
        ShowLevel(currentLevel);
        ShowScore(currentScore);
    }

    public void AddScore(int countLines)
    {
        AddCountCleaningLines(countLines);
        switch (countLines)
        {
            case 1:
                currentScore += 40 * (currentLevel + 1);
                break;
            case 2:
                currentScore += 100 * (currentLevel + 1);
                break;
            case 3:
                currentScore += 300 * (currentLevel + 1);
                break;
            case 4:
                currentScore += 1200 * (currentLevel + 1);
                break;
        }
        ShowScore(currentScore);
    }

    private void AddCountCleaningLines(int countLines)
    {
        if (cleaningLines + countLines >= 10)
        {
            NextLevel();
            linesForLevel += countLines - 10;
            cleaningLines += countLines;
        }
        else
        {
            cleaningLines += countLines;
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
        currentLevel++;
        ShowLevel(currentLevel);
    }

    private void ShowLevel(int currentLevel)
    {
        if (currentLevel < 10) { levelText.text = "0" + currentLevel.ToString(); }
        else { levelText.text = currentLevel.ToString(); }
    }

    private void ShowScore(int score)
    {
        if(score<10) { scoreText.text = "00000"+currentScore.ToString(); }
        else if(score<100) { scoreText.text = "0000" + currentScore.ToString(); }
        else if(score<1000) { scoreText.text = "000" + currentScore.ToString(); }
        else if(score<10000) { scoreText.text = "00" + currentScore.ToString(); }
        else if(score<100000) { scoreText.text = "0" + currentScore.ToString(); }
        else if(score<1000000) { scoreText.text = currentScore.ToString(); }
        else { scoreText.text = "999999"; }
    }
}
