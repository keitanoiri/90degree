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
        //�^�C���X�P�[����߂�(���O�̃V�[���Œx�����Ă���j
        Time.timeScale = 1.0f;

        //���҂��N���\������
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
        winnernameBuilder.AppendLine($"{winnickname}" +"�̏����I");
        winnername.text = winnernameBuilder.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //���[�_�[�{�[�h�X�V���~
        RB.enabled = false;
    }

    //�����鏈��
    public void quit()
    {
        PhotonNetwork.LeaveRoom();
    }
    //���[�����甲������^�C�g����
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("Title");
    }

    //
    public void remutch()
    {
        //rematch����ۂɂ��ׂẴl�b�g���[�N�I�u�W�F�N�g��j�󂷂邱�Ƃœ����Y����h��
        PhotonNetwork.DestroyAll();
        //���r�[��ʂ����[�h
        PhotonNetwork.LoadLevel("Roby");
    }

}
