using System.Threading.Tasks;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private const string SCORE_KEY = "BestScore";

    private int bestScore;

    public async Task Init()
    {
        UIStart uiStart = await UIManager.Show<UIStart>();

        bestScore = PlayerPrefs.GetInt(SCORE_KEY, 0);
        uiStart.SetBestScore(bestScore);
    }

    public void SaveScore(int score)
    {
        bestScore = (bestScore < score) ? score : bestScore;
        PlayerPrefs.SetInt(SCORE_KEY, bestScore);
    }
}