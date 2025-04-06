using Photon.Pun;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviourPunCallbacks
{

    [SerializeField] TextMeshProUGUI winnername;
    [SerializeField] ReaderBoard RB;
    private StringBuilder winnernameBuilder;
    private string winnickname;
    private int bestkill;

    // Start is called before the first frame update
    void Start()
    {
        //タイムスケールを戻す(直前のシーンで遅くしている）
        Time.timeScale = 1.0f;

        //勝者が誰か表示する
        winnernameBuilder = new StringBuilder();
        var players = PhotonNetwork.PlayerList;
        foreach (var player in players)
        {
            if (winnickname == null)
            {
                winnickname = player.NickName;
                bestkill = player.GetKill();
            }
            else if (bestkill < player.GetKill()) 
            {
                winnickname = player.NickName;
                bestkill = player.GetKill();
            }
        }
        winnernameBuilder.AppendLine($"{winnickname}" +"の勝ち！");
        winnername.text = winnernameBuilder.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //リーダーボード更新を停止
        RB.enabled = false;
    }

    //抜ける処理
    public void quit()
    {
        PhotonNetwork.LeaveRoom();
    }
    //ルームから抜けたらタイトルへ
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("Title");
    }

    //
    public void remutch()
    {
        //rematchする際にすべてのネットワークオブジェクトを破壊することで同期ズレを防ぐ
        PhotonNetwork.DestroyAll();
        //ロビー画面をロード
        PhotonNetwork.LoadLevel("Roby");
    }

}
