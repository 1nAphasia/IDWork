using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionStatsManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private GameObject MissionListScrollViewContent;
    [SerializeField]
    private GameObject MissionDetailScrollViewContent;
    [SerializeField]
    private GameObject MissionListButtonPrefab;
    [SerializeField]
    private GameObject MissionPanelPrefab;
    private List<Mission> MissionInfoList;
    void Start()
    {
        MissionInfoList = GameDataManager.I.MissionService.GetAllMissions();
        var missionButtonMap = GenerateTaskButtons();
        var currentMission = GameDataManager.I.MissionService.CurrentMission;
        if (currentMission != null)
        {
            if (missionButtonMap.TryGetValue(currentMission.Value, out var btnGO))
            {
                // 选中按钮
                EventSystem.current.SetSelectedGameObject(btnGO);
                // 可选：高亮或调用按钮的选中方法
                var missionBtn = btnGO.GetComponent<MissionItemButton>();
                if (missionBtn != null)
                    missionBtn.OnClick();
            }
        }
    }
    void OnEnable()
    {
        MissionInfoList = GameDataManager.I.MissionService.GetAllMissions();
        GenerateTaskButtons();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private Dictionary<Mission, GameObject> GenerateTaskButtons()
    {
        var missionButtonMap = new Dictionary<Mission, GameObject>();
        foreach (Transform child in MissionListScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var mission in MissionInfoList)
        {
            var buttonGO = Instantiate(MissionListButtonPrefab, MissionListScrollViewContent.transform);
            var taskButton = buttonGO.GetComponent<MissionItemButton>();
            taskButton.Initialize(this, mission);
            missionButtonMap[mission] = buttonGO;
        }
        return missionButtonMap;

    }
    public void SelectTask(Mission missionInfo)
    {
        UpdateTaskDetails(missionInfo);
    }
    public GameObject GetSelectedMissionButton()
    {
        var selectedObj = EventSystem.current.currentSelectedGameObject;
        if (selectedObj == null) return null;
        return selectedObj;
    }
    private void UpdateTaskDetails(Mission missionInfo)
    {
        foreach (Transform child in MissionDetailScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
        var panelGO = Instantiate(MissionPanelPrefab, MissionDetailScrollViewContent.transform);
        var buttons = panelGO.GetComponentsInChildren<Button>(true);
        var MissionTextList = new Stack<string>();
        MissionTextList.Push(missionInfo.missionDetail);
        MissionTextList.Push(missionInfo.missionGoal);
        MissionTextList.Push(missionInfo.missionName);
        foreach (var btn in buttons)
        {
            var tmp = btn.GetComponentInChildren<TMP_Text>(true);
            if (tmp != null && btn.name != "Button")
            {
                // 这里举例设置文本内容和颜色

                tmp.text = MissionTextList.Pop();
                // 你可以根据 missionInfo 或按钮索引分别设置
            }
        }
    }
}
