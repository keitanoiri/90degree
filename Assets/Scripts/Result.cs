using Photon.Pun;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviourPunCallbacks
{

    [SerializeField] TextMeshProUGUI winnername;
    private StringBuilder winnernameBuilder;
    private string winnickname;
    private int bestkill;

    // Start is called before the first frame update
    void Start()
    {
        //タイムスケールを戻す
        Time.timeScale = 1.0f;

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

        winnernameBuilder.AppendLine($"{winnickname}" +" Win");
        winnername.text = winnernameBuilder.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void remutch()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
        }
        PhotonNetwork.LoadLevel("Roby");
    }

}
