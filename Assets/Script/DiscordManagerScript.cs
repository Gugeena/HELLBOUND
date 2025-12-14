using Discord;
using UnityEngine;
using UnityEngine.Rendering;

public class DiscordManagerScript : MonoBehaviour
{
    public long appID = 1449483408744124538;
    private Discord.Discord discord;
    private ActivityManager activityManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        discord = new Discord.Discord(appID, (ulong) CreateFlags.NoRequireDiscord);
        activityManager = discord.GetActivityManager();
        SetPresence("Slaying Demons", "");
    }

    // Update is called once per frame
    void Update()
    {
        if (discord != null)
            discord.RunCallbacks();
    }

    public void SetPresence(string state, string details)
    {
        Activity activity = new Activity
        {
            State = state,
            Details = details,
            Assets =
            {
                LargeImage = "discordicon",
                LargeText = "HELLBOUND"
            }
        };

        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res != Result.Ok)
                Debug.Log("Discord RPC Failed");
        });
    }

    private void OnApplicationQuit()
    {
        discord.Dispose();
    }
}
