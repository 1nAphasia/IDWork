using UnityEngine;
using TMPro;

public class MissionPanel : MonoBehaviour
{
    public TMP_Text MissionName;
    public TMP_Text MissionGoal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameDataManager.I.MissionService.CurrentMission.OnValueChanged += UpdateMissionDisplay;
        UpdateMissionDisplay(GameDataManager.I.MissionService.CurrentMission.Value);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void UpdateMissionDisplay(Mission mission)
    {
        MissionName.text = mission.missionName;
        MissionGoal.text = mission.missionGoal;
    }
}
