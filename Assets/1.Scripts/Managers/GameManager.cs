using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isInit = false; //앱 초기화
    public static bool isGameStart = false; //게임 시작

    private Coroutine LoadMapCoroutine;
    private const float loadMapElapse = 15f;

    #region Unity Life Cycles
    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
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
        //Time.timeScale = 0;

        isInit = true;
    }

    public void StartGame()
    {
        //Fade in & out

        //풍선 위치 : -14
        //카메라 위치 : -12
        //TimeScale = 1

        //커서 On

        //카메라&풍선 이동 연출 (풍선 먼저 떠오르기 시작)
        //적정 위치에 stop
        //풍선 위치 : -2.5
        //카메라 : 0

        //맵 소환술
        LoadMapCoroutine = StartCoroutine(LoadMap());
    }

    public void GameOver()
    {
        //맵 생성 정지
        StopCoroutine(LoadMapCoroutine);

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

    private IEnumerator LoadMap()
    {
        WaitForSeconds mapElapse = new(loadMapElapse);
        while (true)
        {
            yield return mapElapse;
        }
    }
    #endregion
}