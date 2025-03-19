using System.Threading.Tasks;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isInit = false;
    public static bool isGameStart = false;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 30;
        Application.runInBackground = true;
    }

    private void OnDestroy()
    {
        isInit = false;
    }

    public async Task InitApp()
    {
#if UNITY_EDITOR
        ICLogger.isDevelop = true;
#else
        ICLogger.isDevelop = false;
#endif

        await InitManagers();
    }

    private async Task InitManagers()
    {
        // Initialize ResourceManager
        await ResourceManager.Instance.Init();

        // Initialize UIManager
        await UIManager.Instance.Init();
    }
}