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
        //�󂯎��ĊJ
        PhotonNetwork.CurrentRoom.IsOpen = true;  // ���[�����J������i�}�X�^�[�N���C�A���g�Ɍ��肷�ׂ�����
        //���g�̃X�R�A��������
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
        //�S��Ready�Ȃ�Q�[���X�^�[�g
        //���[���ɎQ�����Ă��Ȃ���΂��Ȃ��������̓}�X�^�[����Ȃ�
        if (!PhotonNetwork.InRoom || !PhotonNetwork.IsMasterClient) { return; }

        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.2f)
        {
            elapsedTime = 0f;
            CheckReady();
        }
    }

    private void CheckReady()//�S�Ẵv���C���[��Ready�Ȃ�Q�[���J�n
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

        PhotonNetwork.CurrentRoom.IsOpen = false;  // ���[������ē����ł��Ȃ��悤�ɂ���
        photonView.RPC(nameof(LoadScene), RpcTarget.All);//�V�[�������[�h

    }

    [PunRPC]
    public void LoadScene()
    {
        PhotonNetwork.IsMessageQueueRunning = false;    //�V�[�����ǂݍ��܂��܂Ńl�b�g���[�N���b�Z�[�W���~
        SceneManager.LoadScene("Main2");
    }

    public void PressedReady()
    {
        //���g�̃t���O��true��
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
