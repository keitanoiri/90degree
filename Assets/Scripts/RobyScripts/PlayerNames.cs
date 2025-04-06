 using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerNames : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TextMeshProUGUI playernameboard;
    [SerializeField] private TextMeshProUGUI playercount;
    [SerializeField] private TextMeshProUGUI playerisready;
    private StringBuilder builder;
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        builder = new StringBuilder();
        elapsedTime = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        //ルームに参加していなければやらない
        if (!PhotonNetwork.InRoom) { return; }

        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.2f)
        {
            elapsedTime = 0f;
            UpdateLabel();
        }
    }

    private void UpdateLabel()
    {
        //プレイヤー名をロビーで表示する
        var players = PhotonNetwork.PlayerList;
        
        builder.Clear();
        foreach (var player in players) {
            builder.AppendLine($"{player.NickName}({player.ActorNumber})");
        }
        playernameboard.text = builder.ToString();

        builder.Clear();
        foreach (var player in players)
        {
            builder.AppendLine($"{player.GetIsReady()}");
        }
        playerisready.text = builder.ToString();

        playercount.text = $"{PhotonNetwork.CurrentRoom.PlayerCount}/6";

        roomName.text = PhotonNetwork.CurrentRoom.Name;
    }
}
