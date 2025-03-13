using Photon.Pun;
using UnityEngine;
using Cysharp.Threading.Tasks; // UniTask
using UnityEngine.AI;
using System;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject MyCharacter;
    [SerializeField] private GameObject ReaderBoard;
    [SerializeField] private GameObject ESCmenu;
    [SerializeField] public float RespownTime;
    [SerializeField] private int WinKill;
    [SerializeField] private GameObject[] respawnAreas;
    [SerializeField] private double spawnInterval;
    [SerializeField] private int MaxItem;

    public bool pose = false;
    public int ItemCount;
    GameObject nowESCmenu = null;


    public float searchRadius = 10f; // �����_���n�_��T���͈�
    public int maxAttempts = 10; // ���s�񐔂̏��

    bool isNotRespowning;

    public Sprite Skill0Sprite;
    public Sprite Skill1Sprite;
    public Sprite Skill2Sprite;

    private void Awake()
    {
        //�X�L���C���[�W�擾
        int Skill0Index = PlayerPrefs.GetInt("Skill0", 0);
        Skill0Sprite = Resources.Load<Sprite>($"Sprites/Skills/Skill_{Skill0Index}");
        int Skill1Index = PlayerPrefs.GetInt("Skill1", 1);
        Skill1Sprite = Resources.Load<Sprite>($"Sprites/Skills/Skill_{Skill1Index}");
        int Skill2Index = PlayerPrefs.GetInt("Skill2", 2);
        Skill2Sprite = Resources.Load<Sprite>($"Sprites/Skills/Skill_{Skill2Index}");

    }

    // Start is called before the first frame update
    async void Start()
    {

        PhotonNetwork.IsMessageQueueRunning = true;
        // �J�[�\�������b�N���ĉ�ʂ̒��S�ɌŒ�
        Cursor.lockState = CursorLockMode.Locked;

        //���[�_�[�{�[�h��\����
        ReaderBoard.SetActive(false);

        isNotRespowning = true;

        SpawnPlayer();

        pose = false;
        ItemCount = 0;
        if (PhotonNetwork.IsMasterClient)
        {
            await SpawnItemsAsync();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (pose)
        {
            return;
        }
        //tab�������Ă���ԃ��[�_�[�{�[�h��\������
        if (Input.GetKey(KeyCode.Tab))
        {
            ReaderBoard.SetActive(true);
        }
        else
        {
            ReaderBoard.SetActive(false);
        }

        //esc�Ń��j���[�\��
        if(Input.GetKeyDown(KeyCode.M)&& nowESCmenu == null)
        {
            nowESCmenu=Instantiate(ESCmenu);
            anenableToMove();
        }

        wincheck();
    }

    public void enableToMove()
    {
        if (MyCharacter != null)
        {
            PlayerController playerController = MyCharacter.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.canMove = true;
                //MyCharacter.GetComponentInChildren<AnimationController>().enabled = true;
            }
            else
            {
                Debug.LogWarning("PlayerController �R���|�[�l���g��������܂���ł����B");
            }
        }
        else
        {
            Debug.LogWarning("MyCharacter �� null �ł��B");
        }
    }
    public void anenableToMove()
    {
        if (MyCharacter != null)
        {
            PlayerController playerController = MyCharacter.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.canMove = false;
                //MyCharacter.GetComponentInChildren<AnimationController>().enabled = false;
            }
            else
            {
                Debug.LogWarning("PlayerController �R���|�[�l���g��������܂���ł����B");
            }
        }
        else
        {
            Debug.LogWarning("MyCharacter �� null �ł��B");
        }
    }

    public void SpawnPlayer()
    {
        //�A�o�^�[����
        GameObject selectedArea = respawnAreas[UnityEngine.Random.Range(0, respawnAreas.Length)];
        Vector3 size = selectedArea.GetComponent<BoxCollider>().size;
        Vector3 center = selectedArea.GetComponent<BoxCollider>().center;

        // �R���C�_�[�̃��[�J����Ԃł̃����_���Ȉʒu���v�Z
        float randomX = UnityEngine.Random.Range(-size.x / 2, size.x / 2);
        float randomY = UnityEngine.Random.Range(-size.y / 2, size.y / 2);
        float randomZ = UnityEngine.Random.Range(-size.z / 2, size.z / 2);

        // ���[�J����Ԃ̈ʒu���烏�[���h��Ԃ̈ʒu�ɕϊ�
        Vector3 spownPos = selectedArea.transform.TransformPoint(center + new Vector3(randomX, randomY, randomZ));

        NavMeshHit hit;
        if (NavMesh.SamplePosition(spownPos, out hit, searchRadius, NavMesh.AllAreas))
        {
            MyCharacter = PhotonNetwork.Instantiate("Player", hit.position, selectedArea.transform.rotation);
        }
        else
        {
            Debug.Log("Randam���s");
        }
    }
    public async void RespownPlayer()
    {
        if (!isNotRespowning)
        {
            return;
        }
        //���X�|�[�����͑��̃^�X�N�͎n�߂Ȃ�
        isNotRespowning = false;

        var token = this.GetCancellationTokenOnDestroy(); // GameObject ���j�󂳂ꂽ��L�����Z��
        try {
            await UniTask.Delay((int)(RespownTime*1000), cancellationToken: token);
            SpawnPlayer();
            isNotRespowning=true;
        }
        catch (OperationCanceledException)
        {
            Debug.Log("�I�u�W�F�N�g���j�󂳂ꂽ���߃^�X�N���L�����Z�����܂���");
        }
    }
    
    private void wincheck()
    {
        if (PhotonNetwork.IsMasterClient) {
            var players = PhotonNetwork.PlayerList;
            foreach (var player in players)
            {
                if (player.GetKill() >= WinKill)
                {
                    photonView.RPC(nameof(win),RpcTarget.All,player);
                }
            }
        }
    }

    private async UniTask SpawnItemsAsync()
    {
        GameObject[] gm = GameObject.FindGameObjectsWithTag("Item");
        ItemCount = gm.Length;

        bool isSpawning = true;
        while (isSpawning)
        {
            if (ItemCount <= MaxItem)
            {
                //�A�C�e������
                GameObject selectedArea = respawnAreas[UnityEngine.Random.Range(0, respawnAreas.Length)];
                Vector3 size = selectedArea.GetComponent<BoxCollider>().size;
                Vector3 center = selectedArea.GetComponent<BoxCollider>().center;

                // �R���C�_�[�̃��[�J����Ԃł̃����_���Ȉʒu���v�Z
                float randomX = UnityEngine.Random.Range(-size.x / 2, size.x / 2);
                float randomY = UnityEngine.Random.Range(-size.y / 2, size.y / 2);
                float randomZ = UnityEngine.Random.Range(-size.z / 2, size.z / 2);

                // ���[�J����Ԃ̈ʒu���烏�[���h��Ԃ̈ʒu�ɕϊ�
                Vector3 spownPos = selectedArea.transform.TransformPoint(center + new Vector3(randomX, randomY, randomZ));

                NavMeshHit hit;
                if (NavMesh.SamplePosition(spownPos, out hit, searchRadius, NavMesh.AllAreas))
                {
                    GameObject Item = PhotonNetwork.Instantiate("Item", hit.position, selectedArea.transform.rotation);
                    //Item.name = selectedArea.name+"Item";
                    ItemCount++;
                }
                else
                {
                    Debug.Log("Randam���s");
                }
            }
            // �ҋ@

            var token = this.GetCancellationTokenOnDestroy(); // GameObject ���j�󂳂ꂽ��L�����Z��
            try
            {
                await UniTask.Delay(System.TimeSpan.FromSeconds(spawnInterval),cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("�I�u�W�F�N�g���j�󂳂ꂽ���߃^�X�N���L�����Z�����܂���");
                isSpawning=false;
            }


            
        }
    }


    [PunRPC]
    async public void win(Photon.Realtime.Player player)
    {
        //�����Ȃ�����
        anenableToMove();
        pose = true;//update���~
        // �J�[�\�������b�N���ĉ�ʂ̒��S�ɌŒ������
        Cursor.lockState = CursorLockMode.None;
        //�X���[�ɂ���
        Time.timeScale = 0.2f;
        //�X���[�ɂ��Ĉ�b��ɉ�ʑJ��
        await UniTask.Delay(System.TimeSpan.FromSeconds(1));
        PhotonNetwork.LoadLevel("Result");
    }

    async public override void OnMasterClientSwitched(Player newMasterClient)
    {
        await SpawnItemsAsync();
    }

}
