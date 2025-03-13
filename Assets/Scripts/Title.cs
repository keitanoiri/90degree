using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class Title : MonoBehaviourPunCallbacks
{ 
    [SerializeField] private TextMeshProUGUI RoomID;
    [SerializeField] private TextMeshProUGUI PlayerName;
    [SerializeField] private TextMeshProUGUI Messagetext;
    [SerializeField] AudioMixer AudioMixer;
    private string Check1;
    private string Check2;
    private bool IsConectMuster;

    [SerializeField] private GameObject OptionMenu;
    [SerializeField] private GameObject HowToPlay;

    public void Start()
    {
        // �E�B���h�E���[�h��ݒ肵�ċN��
        FullScreenMode screenmode;
        int x, y;
        switch (PlayerPrefs.GetInt("ScreenMode", 0))
        {
            case 0:
                screenmode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                screenmode = FullScreenMode.Windowed;
                break;
            case 2:
                screenmode = FullScreenMode.MaximizedWindow;
                break;
            default:
                screenmode = FullScreenMode.FullScreenWindow;
                break;
        }
        switch (PlayerPrefs.GetInt("ScreenSize", 0))
        {
            case 0:
                x = 1920; y = 1080; break;
            case 1:
                x = 1920; y = 1200; break;
            case 2:
                x = 2560; y = 1080; break;
            case 3:
                x = 1600; y = 1200; break;
            default:
                x = 1920; y = 1080; break;
        }
        Screen.SetResolution(x, y, screenmode);


        float db1 = Mathf.Lerp(-40f, 20f, PlayerPrefs.GetFloat("MasterVolume", 50) / 100);
        if (db1 != -20)
        {
            AudioMixer.SetFloat("MasterParam", db1);
        }
        else
        {
            AudioMixer.SetFloat("MasterParam", -80);
        }
        float db2 = Mathf.Lerp(-40f, 20f, PlayerPrefs.GetFloat("BGMVolume", 50) / 100);
        if (db2 != -20)
        {
            AudioMixer.SetFloat("BGMParam", db2);
        }
        else
        {
            AudioMixer.SetFloat("BGMParam", -80);
        }
        float db3 = Mathf.Lerp(-40f, 20f, PlayerPrefs.GetFloat("SEVolune", 50) / 100);
        if (db3 != -20)
        {
            AudioMixer.SetFloat("SEParam", db3);
        }
        else
        {
            AudioMixer.SetFloat("SEParam", -80);
        }
        float db4 = Mathf.Lerp(-40f, 20f, PlayerPrefs.GetFloat("VoiceVolume", 50) / 100);
        if (db4 != -20)
        {
            AudioMixer.SetFloat("VoiceParam", db4);
        }
        else
        {
            AudioMixer.SetFloat("VoiceParam", -80);
        }



        //roomID�����炤
        Check1 = RoomID.text;
        Check2 = PlayerName.text;
        //�V�[���J�ڂ𓯊�
        PhotonNetwork.IsMessageQueueRunning = true;
        PhotonNetwork.UseRpcMonoBehaviourCache = true;

        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();

    }
    public void Play()
    {

        //���͂���Ă��Ȃ���΂��߁i��������j�Ȃ̂œ����v���������Ȃ��悤��
        if (!string.Equals(RoomID.text,Check1)&& !string.Equals(PlayerName.text, Check2)&&IsConectMuster==true)//���O��ID�����͂���Ă���
        {
            //�ő�l����ݒ�
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 6;

            // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
            PhotonNetwork.JoinOrCreateRoom(RoomID.text, roomOptions, TypedLobby.Default);
        }
        else
        {
            Messagetext.text = "room�ɎQ���ł��܂���ł����B\n�v���C���̃��[�����Q���l������̉\��������܂�";
            //�x���\��
        }

    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        IsConectMuster = true;
    }
    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        PhotonNetwork.NickName = PlayerName.text;
        //�󂯎����ꎞ���f
        PhotonNetwork.IsMessageQueueRunning = false;
        //�}�b�`��ʂ֑J��
        PhotonNetwork.LoadLevel("Roby");
    }


    public void Options()
    {
        Instantiate(OptionMenu);
    }

    public void HowTo()
    {
        Instantiate(HowToPlay);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
