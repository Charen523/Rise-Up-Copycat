using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isInit = false; //앱 초기화
    public static bool isGameStart = false; //게임 시작

    private Coroutine LoadMapCoroutine;
    private const float loadMapElapse = 7f;

    [SerializeField] private Transform mainCam;
    [SerializeField] private SpriteRenderer balloon;
    private Vector3 balloonSize = new(1f, 1.1f, 1f);
    [SerializeField] private GameObject pointer;



    #region Unity Life Cycles
    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
        DOTween.Init();
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

        pointer.SetActive(false);
        await InitManagers();
        
        isInit = true;
        ICLogger.Log("초기화 완료");
    }

    public async void StartGame()
    {
        UIManager.Instance.ToBlack();
        await Task.Delay(1000);
        MapManager.Instance.ResetPattern();

        if (UIManager.IsOpened<UIResult>())
        {
            UIManager.Hide<UIResult>();
        }

        ResetBalloon();
        Vector3 pos = balloon.transform.position;
        pos.y = -14;
        balloon.transform.position = pos;

        pos = mainCam.position;
        pos.y = -12;
        mainCam.position = pos;

        await Task.Delay(1000);
        UIManager.Instance.ToTransparent();
        await UIManager.Show<UIScore>();

        //카메라&풍선 이동 연출 (풍선 먼저 떠오르기 시작)
        balloon.transform.DOMoveY(-2.5f, 3f);
        mainCam.DOMoveY(0f, 2.5f);
        pointer.SetActive(true);

        //맵 소환
        LoadMapCoroutine = StartCoroutine(LoadMap());
    }

    public void GameOver()
    {
        if (isGameStart)
        {
            isGameStart = false;
            StopCoroutine(LoadMapCoroutine);
            StartCoroutine(GameOverCoroutine());
        }
    }
    #endregion

    #region Sub Methods
    private async Task InitManagers()
    {
        await ResourceManager.Instance.Init();
        await UIManager.Instance.Init();
        await SaveManager.Instance.Init();
        await MapManager.Instance.Init();
    }

    private IEnumerator LoadMap()
    {
        WaitForSeconds mapElapse = new(loadMapElapse);
        while (true)
        {
            MapManager.Instance.ShowRandomMap();
            yield return mapElapse;
        }
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return UIManager.Show<UIResult>();

        pointer.SetActive(false);
        BalloonPop();
        UIManager.Instance.ToHalfTransparent();
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
    }

    private void ResetBalloon()
    {
        balloon.color = Color.white;
        var col = balloon.color;
        col.a = 1f;
        balloon.color = col;
        transform.localScale = balloonSize;
    }

    private void BalloonPop()
    {
        float scale1 = 1.1f;
        float scale2 = 3f;

        Color originalColor = balloon.color;
        Vector3 originalScale = transform.localScale;

        Sequence seq = DOTween.Sequence().SetUpdate(true);
        seq.Append(balloon.DOColor(new Color(1f, 0.3f, 0.3f, 1f), 0.5f));
        seq.Join(transform.DOScale(originalScale * scale1, 0.5f));

        seq.Join(transform.DOScale(originalScale * scale2, 0.1f));

        seq.OnComplete(() =>
        {
            balloon.color = originalColor;
            transform.localScale = originalScale;
        });
    }
    #endregion
}