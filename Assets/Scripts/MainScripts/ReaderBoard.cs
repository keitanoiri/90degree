using Photon.Pun;
using System.Text;
using TMPro;
using UnityEngine;

public class ReaderBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playername;
    [SerializeField] private TextMeshProUGUI killcount;
    [SerializeField] private TextMeshProUGUI deathcount;
    [SerializeField]private TextMeshProUGUI scorecount;
    private StringBuilder nameBuilder;
    private StringBuilder killBuilder;
    private StringBuilder deathBuilder;
    private StringBuilder scoreBuilder;
    private float elapsedTime;
    // Start is called before the first frame update
    void Start()
    {
        nameBuilder = new StringBuilder();
        killBuilder = new StringBuilder();
        deathBuilder = new StringBuilder();
        scoreBuilder = new StringBuilder();
        elapsedTime = 0f;

        UpdateLabel();
    }

    // Update is called once per frame
    void Update()
    {
        //���[���ɎQ�����Ă��Ȃ���΂��Ȃ�
        if (!PhotonNetwork.InRoom) { return; }

        //��莞�Ԃ��ƂɍX�V�i�j
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.2f)
        {
            elapsedTime = 0f;
            UpdateLabel();
        }
    }

    //���͂�ύX����֐�
    private void UpdateLabel() {
        var players = PhotonNetwork.PlayerList;
        nameBuilder.Clear();
        killBuilder.Clear();
        deathBuilder.Clear();
        scoreBuilder.Clear();

        foreach (var player in players)
        {
            nameBuilder.AppendLine($"{player.NickName}");
            killBuilder.AppendLine($"{player.GetKill()}");
            deathBuilder.AppendLine($"{player.GetDeath()}");
            scoreBuilder.AppendLine($"{player.GetScore()}");
        }
        playername.text = nameBuilder.ToString();
        killcount.text = killBuilder.ToString();
        deathcount.text = deathBuilder.ToString();
        scorecount.text = scoreBuilder.ToString();
    }
}
