using Steamworks;
using UnityEngine;

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
}
