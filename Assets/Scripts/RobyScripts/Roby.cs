using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Roby : MonoBehaviourPunCallbacks
{
    private float elapsedTime;
    [SerializeField] GameObject OptionMenu;
    [SerializeField] GameObject RoadOutMenu;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0f;
        //受け取り再開
        PhotonNetwork.CurrentRoom.IsOpen = true;  // ルームを開放する（マスタークライアントに限定すべきかも
        //自身のスコアを初期化
        var players = PhotonNetwork.PlayerList;
        foreach (var player in players)
        {
            player.SetKill(0);
            player.SetDeath(0);
            player.SetScore(0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //全員Readyならゲームスタート
        //ルームに参加していなければやらないもしくはマスターじゃない
        if (!PhotonNetwork.InRoom || !PhotonNetwork.IsMasterClient) { return; }

        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.2f)
        {
            elapsedTime = 0f;
            CheckReady();
        }
    }

    private void CheckReady()//全てのプレイヤーがReadyならゲーム開始
    {
        var players = PhotonNetwork.PlayerList;
        foreach (var player in players)
        {
            if (player.GetIsReady() == string.Empty)
            {
                return;
            }
        }

        foreach (var player in players)
        {
            player.SetIsReady(false);
        }

        PhotonNetwork.CurrentRoom.IsOpen = false;  // ルームを閉じて入室できないようにする
        photonView.RPC(nameof(LoadScene), RpcTarget.All);//シーンをロード

    }

    [PunRPC]
    public void LoadScene()
    {
        PhotonNetwork.IsMessageQueueRunning = false;    //シーンが読み込まれるまでネットワークメッセージを停止
        SceneManager.LoadScene("Main2");
    }

    public void PressedReady()
    {
        //自身のフラグをtrueに
        PhotonNetwork.LocalPlayer.SetIsReady(true);
    }

    public void quit()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        SceneManager.LoadScene("Title");
    }

    public void ShowRoadOutMenu()
    {
        Instantiate(RoadOutMenu);
    }

    public void ShowOptionMenu()
    {
        Instantiate(OptionMenu);
    }

}
