using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] private List<Transform> parents = new();
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField pwInput;

    private IEnumerator Start()
    {
        //call Managers
        yield return SceneManager.LoadSceneAsync("DontDestroy", LoadSceneMode.Additive);
        //set UIManager-Parents
        UIManager.SetParents(parents);
        //init all managers + @
        yield return GameManager.Instance.InitApp();

        //wait until "Game Start" input
        yield return new WaitUntil(() => GameManager.isGameStart);

        //TODO : 게임시작 구현
    }
}
