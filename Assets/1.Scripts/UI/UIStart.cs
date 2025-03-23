using TMPro;
using UnityEngine;

public class UIStart : UIBase
{
    [SerializeField] TextMeshProUGUI bestScore;

    public void SetBestScore(int score)
    {
        bestScore.text = $"Best Score : {score}";
    }
}