using TMPro;
using UnityEngine;

public class SlotScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTxt;
    
    public void SetScore(int score)
    {
        scoreTxt.text = score.ToString();
    }
}
