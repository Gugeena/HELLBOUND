using UnityEngine;
using Steamworks;

public class AchivementScript : MonoBehaviour
{
    public static AchivementScript instance;

    private void Start()
    {
        instance = this;
    }
        
    public void UnlockAchivement(string achivementAPIName)
    {
        if (!SteamManager.Initialized) return;

        bool success = SteamUserStats.SetAchievement(achivementAPIName);
        if (!success) return;

        SteamUserStats.StoreStats();
    }

    public void FillStat(string statAPIName, int value)
    {
        if (!SteamManager.Initialized) return;

        if (!SteamUserStats.GetStat(statAPIName, out int currentValue)) return;

        if (currentValue == value) return;

        bool success = SteamUserStats.SetStat(statAPIName, value);
        if (!success) return;

        SteamUserStats.StoreStats();
    }
}

