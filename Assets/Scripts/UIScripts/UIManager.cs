using UnityEngine;


public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Canvas overlayCanvas;
    public static UIManager Instance { get; private set; }
    public RenderTexture myBlurResult;
    [SerializeField] GameObject PlayerStatsContainer;
    [SerializeField] GameObject MissionItemContainer;
    [SerializeField] GameObject MissionDetailContainer;

    [SerializeField] GameObject StatItemPrefab;
    [SerializeField] GameObject MissionItemPrefab;
    [SerializeField] GameObject MissionDetailPrefab;



    public float blurSize = 5.0f;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 保证只有一个实例
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);


    }
    void Start()
    {
        GameDataManager.I.StatsService.OnStatsChanged += RefreshPlayerStatUI;
        RefreshPlayerStatUI();
    }

    // Update is called once per frame
    void Update()
    {
        //Graphics.Blit(frostedBlurResult.rt, myBlurResult);
    }

    void RefreshPlayerStatUI()
    {
        foreach (Transform child in PlayerStatsContainer.transform)
            Destroy(child.gameObject);
        var allStats = GameDataManager.I.StatsService.GetAllStats();
        foreach (var pair in allStats)
        {
            var go = Instantiate(StatItemPrefab, PlayerStatsContainer.transform);
            // 2. 获取TMPText组件并赋值
            var tmps = go.GetComponentsInChildren<TMPro.TMP_Text>();
            if (tmps.Length >= 2)
            {
                tmps[0].text = pair.Item1.ToString();
                tmps[1].text = pair.Item2.ToString();
            }
        }
    }
    void RefreshMissionPanelUI()
    {

    }
}
