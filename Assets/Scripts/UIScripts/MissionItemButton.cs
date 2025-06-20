using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class MissionItemButton : MonoBehaviour
{
    public Mission MissionInfo; // 关联的任务数据

    [Header("UI References")]
    public TextMeshProUGUI titleText;
    private MissionStatsManager MissionUIManager;

    public void Initialize(MissionStatsManager missionUIManager, Mission data)
    {
        MissionUIManager = missionUIManager;
        MissionInfo = data;

        // 初始化文本
        titleText.text = MissionInfo.missionName;
        // 添加点击事件
        GetComponent<Button>().onClick.AddListener(OnClick);
    }


    public void OnClick()
    {
        // 通知列表视图切换任务
        MissionUIManager.SelectTask(MissionInfo);
        GameDataManager.I.MissionService.CurrentMission.Value = MissionInfo;
    }


}