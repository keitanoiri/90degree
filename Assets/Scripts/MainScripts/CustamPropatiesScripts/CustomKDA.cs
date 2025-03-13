using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Globalization;

public static class CustomKDA
{
    private const string Killkey = "Kill";
    private const string Deathkey = "Death";
    private const string Scorekey = "Score";
    private const string HPkey = "HP";

    private static readonly Hashtable propsToset = new Hashtable();

    public static int GetKill(this Player player)
    {
        return(player.CustomProperties[Killkey] is int kill) ?kill: 0;
    }

    public static void SetKill(this Player player, int kill)
    {
        propsToset[Killkey] = kill;
        player.SetCustomProperties(propsToset);
        propsToset.Clear();
    }

    public static int GetDeath(this Player player)
    {
        return (player.CustomProperties[Deathkey] is int death) ? death : 0;
    }

    public static void SetDeath(this Player player, int death)
    {
        propsToset[Deathkey] = death;
        player.SetCustomProperties(propsToset);
        propsToset.Clear();
    }
    public static int GetScore(this Player player)
    {
        return (player.CustomProperties[Scorekey] is int score) ? score : 0;
    }

    public static void SetScore(this Player player, int score)
    {
        propsToset[Scorekey] = score;
        player.SetCustomProperties(propsToset);
        propsToset.Clear();
    }

    public static int GetHP(this Player player)
    {
        return (player.CustomProperties[HPkey] is int hp) ? hp : 0;
    }

    public static void SetHP(this Player player, int hp)
    {
        propsToset[HPkey] = hp;
        player.SetCustomProperties(propsToset);
        propsToset.Clear();
    }
}

