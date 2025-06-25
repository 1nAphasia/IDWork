using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{

    public GameObject PlayerStatsContainer;
    public GameObject StatItemPrefab;
    void OnEnable()
    {
        RefreshPlayerStatUI();
    }
    public void RefreshPlayerStatUI()
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
}