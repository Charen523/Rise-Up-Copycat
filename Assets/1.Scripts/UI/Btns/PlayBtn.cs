using UnityEngine;

public class PlayBtn : MonoBehaviour
{
    public void OnBtnClicked()
    {
        GameManager.isGameStart = true;
        Time.timeScale = 1f;
        GameManager.Instance.StartGame();
    }
}
