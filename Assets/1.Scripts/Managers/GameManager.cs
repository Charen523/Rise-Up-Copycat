using System.Threading.Tasks;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isInit = false; //앱 초기화
    public static bool isGameStart = false; //게임 시작



    #region Unity Life Cycles
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
    #endregion

    #region Main Methods
    public async Task InitApp()
    {
#if UNITY_EDITOR
        ICLogger.isDevelop = true;
#else
        ICLogger.isDevelop = false;
#endif

        await InitManagers();
        Time.timeScale = 0;

        isInit = true;
    }

    public void StartGame()
    {
        //커서 On
        //풍선 위치 : -14
        //카메라 위치 : -12
        //TimeScale = 1
        
        //카메라&풍선 이동 연출
        //적정 위치에 stop
        //맵 소환술
    }

    public void GameOver()
    {
        //커서 Off
        //풍선 터지는 모션
        //TimeScale = 0
    }
    #endregion

    #region Sub Methods
    private async Task InitManagers()
    {
        await ResourceManager.Instance.Init();
        await UIManager.Instance.Init();
        await MapManager.Instance.Init();
    }
    #endregion
}