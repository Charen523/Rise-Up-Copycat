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
        UIManager.SetParents(parents);

        yield return GameManager.Instance.InitApp();
        yield return new WaitUntil(() => GameManager.Instance.isInit);

        UIManager.Instance.ToHalfTransparent();

        yield return new WaitUntil(() => GameManager.isGameStart);

        UIManager.Hide<UIStart>();
    }
}
