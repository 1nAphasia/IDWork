using System;
using System.Collections.Generic;

public class MissionManageService
{
    public List<Mission> MissionList = new();
    public ObservableProperty<Mission> CurrentMission;
    public MissionManageService()
    {

        var mission1 = new Mission()
        {
            missionName = "Operation 1",
            missionGoal = "Complete Operation 1",
            missionStatus = MissionStatus.Completed,
            missionDetail = "Start as a new recruit and eliminate all enemies in the simulation space to complete your first mission."
        };
        var mission2 = new Mission()
        {
            missionName = "First Safe House",
            missionGoal = "Ensure the safety of the surrounding area of the safe house",
            missionStatus = MissionStatus.Active,
            missionDetail = "This is your first task since starting. Clean up the surroundings of the safe house to ensure its safety."
        };
        var mission3 = new Mission()
        {
            missionName = "Checking out on safe house",
            missionGoal = "Have a chat with Vendal",
            missionStatus = MissionStatus.InActive,
            missionDetail = "Welcome to your new house.You can treat it as your new home.Meet your Friend."
        };
        MissionList.Add(mission1);
        MissionList.Add(mission2);
        MissionList.Add(mission3);
        CurrentMission = new ObservableProperty<Mission>();
        CurrentMission.Value = MissionList[0];
    }
    public List<Mission> GetAllMissions()
    {
        return MissionList;
    }
    public void UpdateMissionStatus(int idx, MissionStatus status)
    {
        var mission = MissionList[idx];
        mission.missionStatus = status;
        MissionList[idx] = mission;
    }
}