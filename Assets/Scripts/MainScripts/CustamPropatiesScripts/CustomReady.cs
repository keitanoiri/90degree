using ExitGames.Client.Photon;
using Photon.Realtime;
public static class CustomReady
{
    private const string IsReadyKey = "IsReady";

    private static readonly Hashtable propsToSet = new Hashtable();

    //�v���C���[�̏�����Ԃ��擾
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

    //�v���C���[�̏�����Ԃ�ύX
    public static void SetIsReady(this Player player,bool ready)
    {
        propsToSet[IsReadyKey] = ready;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }
}
