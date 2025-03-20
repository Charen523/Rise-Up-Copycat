using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] private List<Transform> parents = new();
    //[SerializeField] private TMP_InputField emailInput;
    //[SerializeField] private TMP_InputField pwInput;

    private IEnumerator Start()
    {
        yield return SceneManager.LoadSceneAsync("DontDestroy", LoadSceneMode.Additive);
        UIManager.SetParents(parents);
        yield return GameManager.Instance.InitApp();

        yield return new WaitUntil(() => GameManager.isGameStart);

        //TODO : 게임시작 신호보내기.
    }
}
