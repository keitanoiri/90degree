using ExitGames.Client.Photon;
using Photon.Realtime;
public static class CustomReady
{
    private const string IsReadyKey = "IsReady";

    private static readonly Hashtable propsToSet = new Hashtable();

    //プレイヤーの準備状態を取得
    public static string GetIsReady(this Player player)
    {
        if((player.CustomProperties[IsReadyKey] is bool isready)? isready : false)
        {
            return "Ready";
        }
        else
        {
            return "";
        }
    }

    //プレイヤーの準備状態を変更
    public static void SetIsReady(this Player player,bool ready)
    {
        propsToSet[IsReadyKey] = ready;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }
}
