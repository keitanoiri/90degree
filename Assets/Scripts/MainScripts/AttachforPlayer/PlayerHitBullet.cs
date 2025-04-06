using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using UnityEngine;
public class PlayerHitBullet : MonoBehaviourPunCallbacks
{
    //�v���C���[�̃_���[�W������Ǘ�����X�N���v�g

    [SerializeField] GameObject Killcam;
    [SerializeField] GameObject Effect;
    [SerializeField] GameObject Player;
    [SerializeField] SE_Player SE;
    [SerializeField] PlayerController ControllingPlayer;
    private int lasthitID = 999;

    private PlayerController thisPC;

    //private static bool isQuiting = false;

    GameObject manager;
    GameManager gm;

    private void Awake()
    {
        thisPC = GetComponent<PlayerController>();
        //�������ɃQ�[���}�l�[�W���[��T���Ă���
        manager = GameObject.Find("GameManager");
        if (manager.TryGetComponent<GameManager>(out gm))
        {
            Debug.Log("�Q�[���}�l�[�W���[���݂�������");
        }
        else
        {
            Debug.Log("�Q�[���}�l�[�W���[���݂���Ȃ���");
        }
    }

    //���Ă����Ŕ���
    private void OnCollisionEnter(Collision other)
    {
        //���g�̃L�����N�^�[�ł͂Ȃ��Ȃ�
        if (!photonView.IsMine)
        {
            Debug.Log("��������");
            //�V�[���h���A�N�e�B�u�Ȃ�_���[�W����͂Ȃ�
            if (thisPC.P_Shield.activeSelf) return;
            Debug.Log("�V�[���h����A�N�e�B�u");
            //���������̂��e�ۂł���Ȃ�
            if (other.collider.TryGetComponent<Ballet>(out var bullet))
            {
                //�e�ۂ�ID�Ƃ��̊��̎傪�����Ȃ�_���[�W������s��
                if (bullet.OwnerId == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    //����������̏���
                    photonView.RPC(nameof(DestroyBullet), RpcTarget.All, bullet.Id, bullet.OwnerId);
                    //�����œ��Ă�ꂽ���̔��������B(�_���[�W�v�Z���͓͂��Ă�ꂽ���ŏ�������)
                    Player targetPlayer = PhotonNetwork.CurrentRoom.Players[photonView.OwnerActorNr];
                    photonView.RPC(nameof(HitBullet), targetPlayer, bullet.OwnerId,bullet.damage,bullet.transform.position);
                }
            }
        }//���̃I�u�W�F�N�g�����g�̑���L�����ł���A�_���[�W�ǂɂ���������
        else if (other.gameObject.tag == "DamageArea")//damage�ǂɐG�ꂽ��j�󂵂ăf�X�������Z����
        {
            //�Ō�ɓ��������Ώۂ��f�t�H���g�̂܂܂ł���Ȃ��(�������[���Ŗ����ɑΐ킷��ƃG���[�ɂȂ�Ǝ㐫����)
            if (lasthitID != 999)
            {
                PhotonNetwork.CurrentRoom.Players[lasthitID].SetKill(PhotonNetwork.CurrentRoom.Players[lasthitID].GetKill() + 1);//kill���Z
                //�Ō�ɍU���𓖂Ă��l�ɓ|�������Ƃ�`����
                Player targetPlayer = PhotonNetwork.CurrentRoom.Players[lasthitID];
                photonView.RPC(nameof(ShowKillEffect), targetPlayer);
            }
            PhotonNetwork.LocalPlayer.SetHP(0);
            PhotonNetwork.LocalPlayer.SetDeath(PhotonNetwork.LocalPlayer.GetDeath() + 1);//death���Z

            photonView.RPC(nameof(ShowDeadEffect), RpcTarget.All, transform.position, transform.rotation);
            gm.RespownPlayer();

            GameObject killcam = Instantiate(Killcam);
            killcam.transform.position = transform.position;
            PhotonNetwork.Destroy(Player);
        }

    }

    //���Ă�ꂽ���Ŕ���
    /*
    private void OnCollisionEnter(Collision other)
    {
        if (photonView.IsMine)
        {
            //�V�[���h���A�N�e�B�u�Ȃ�_���[�W����͂Ȃ�
            if (thisPC.P_Shield.activeSelf) return;
            
            if (other.collider.TryGetComponent<Ballet>(out var bullet))
            {
                if(bullet.OwnerId != PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    SE.SEdamage();
                    //�e�ɓ�������
                    lasthitID = bullet.OwnerId;

                    int nowHP = PhotonNetwork.LocalPlayer.GetHP();
                    PhotonNetwork.LocalPlayer.SetHP(nowHP - bullet.damage);

                    photonView.RPC(nameof(HitBullet), RpcTarget.All, bullet.Id, bullet.OwnerId);
                    Player targetPlayer = PhotonNetwork.CurrentRoom.Players[bullet.OwnerId];

                    GameObject manager = GameObject.Find("GameManager");
                    if (manager.TryGetComponent<GameManager>(out GameManager gm))
                    {
                        if (0 <= nowHP && nowHP <= bullet.damage)//����O��HP���_���[�W��菬����������_�u���L����h����H
                        {//�v���C���[�����ʏ���
                            photonView.RPC(nameof(ShowKillEffect), targetPlayer);

                            PhotonNetwork.LocalPlayer.SetDeath(PhotonNetwork.LocalPlayer.GetDeath() + 1);//death���Z
                            PhotonNetwork.CurrentRoom.Players[bullet.OwnerId].SetKill(PhotonNetwork.CurrentRoom.Players[bullet.OwnerId].GetKill() + 1);//kill���Z

                            photonView.RPC(nameof(ShowDeadEffect), RpcTarget.All, transform.position, transform.rotation);
                            gm.RespownPlayer();

                            GameObject killcam = Instantiate(Killcam);
                            killcam.transform.position = bullet.transform.position;

                            PhotonNetwork.Destroy(Player);
                        }
                        else
                        {
                            photonView.RPC(nameof(ShowHitEffect), targetPlayer);
                        }
                    }
                    else
                    {
                        Debug.Log("�Q�[���}�l�[�W���[���݂���Ȃ���");
                    }


                    


                }
            }else if (other.gameObject.tag == "DamageArea")//damage�ǂɐG�ꂽ��j�󂵂ăf�X�������Z����
            {
                if(lasthitID != 999){
                    PhotonNetwork.CurrentRoom.Players[lasthitID].SetKill(PhotonNetwork.CurrentRoom.Players[lasthitID].GetKill() + 1);//kill���Z
                }
                PhotonNetwork.LocalPlayer.SetHP(0);
                PhotonNetwork.LocalPlayer.SetDeath(PhotonNetwork.LocalPlayer.GetDeath() + 1);//death���Z
                GameObject manager = GameObject.Find("GameManager");
                if (manager.TryGetComponent<GameManager>(out GameManager gm))
                {
                    photonView.RPC(nameof(ShowDeadEffect),RpcTarget.All, transform.position,transform.rotation);
                    gm.RespownPlayer();
                }
                GameObject killcam = Instantiate(Killcam);
                killcam.transform.position = transform.position;
                PhotonNetwork.Destroy(Player);
            }
        }
    }

    */
    [PunRPC]
    public void HitBullet(int OwnerId,int Damage,Vector3 BulletPos)
    {
        //�e�ɓ�������
        lasthitID = OwnerId;
        int nowHP = PhotonNetwork.LocalPlayer.GetHP();
        PhotonNetwork.LocalPlayer.SetHP(nowHP - Damage);
        Player targetPlayer = PhotonNetwork.CurrentRoom.Players[OwnerId];

        if (0 <= nowHP && nowHP <= Damage)//����O��HP���_���[�W��菬����������_�u���L����h����H
        {//�v���C���[�����ʏ���
            photonView.RPC(nameof(ShowKillEffect), targetPlayer);

            PhotonNetwork.LocalPlayer.SetDeath(PhotonNetwork.LocalPlayer.GetDeath() + 1);//death���Z
            PhotonNetwork.CurrentRoom.Players[OwnerId].SetKill(PhotonNetwork.CurrentRoom.Players[OwnerId].GetKill() + 1);//kill���Z

            //�j�󂳂ꂽ�G�t�F�N�g�̐���
            photonView.RPC(nameof(ShowDeadEffect), RpcTarget.All, transform.position, transform.rotation);
            gm.RespownPlayer();

            GameObject killcam = Instantiate(Killcam);
            killcam.transform.position = BulletPos;

            PhotonNetwork.Destroy(Player);
        }
        else
        {
            //�U���𓖂Ă��v���C���[��hit�m�F������B
            photonView.RPC(nameof(ShowHitEffect), targetPlayer);
        }
    }

    //�U�������v���C���[�ɍU���������������Ƃ�`����
    [PunRPC]
    public void ShowHitEffect()
    {
        //�����̑���L������T��
        if (ControllingPlayer == null)
        {
            var players = FindObjectsOfType<PlayerController>();
            foreach (var player in players)
            {
                if (player.photonView.IsMine)
                {
                    ControllingPlayer = player;
                    break;
                }
            }
        }
        ControllingPlayer.HitEffect();
    }

    //�U�������v���C���[�ɑ��肪���񂾂��Ƃ�`����
    [PunRPC]
    public void ShowKillEffect()
    {
        //�����̑���L������T��
        if (ControllingPlayer == null)
        {
            var players = FindObjectsOfType<PlayerController>();
            foreach (var player in players)
            {
                if (player.photonView.IsMine)
                {
                    ControllingPlayer = player;
                    break;
                }
            }
        }
        ControllingPlayer.KillEffect();
    }

    //�e�ۂ̔j��
    [PunRPC]
    public void DestroyBullet(int id, int ownerId)
    {
        Debug.Log("hit");
        var bullets = FindObjectsOfType<Ballet>();
        foreach (var ballet in bullets)
        {
            if (ballet.Equals(id, ownerId))
            {
                Destroy(ballet.gameObject);
                break;
            }
        }
    }

    //�j�󂳂ꂽ�G�t�F�N�g��\��
    [PunRPC]
    public void ShowDeadEffect(Vector3 pos, Quaternion q)
    {
        Instantiate(Effect, pos, q);
    }



}
