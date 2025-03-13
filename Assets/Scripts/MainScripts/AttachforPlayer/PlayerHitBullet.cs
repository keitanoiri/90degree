using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
public class PlayerHitBullet : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject Killcam;
    [SerializeField] GameObject Effect;
    [SerializeField] GameObject Player;
    [SerializeField] SE_Player SE;
    [SerializeField] PlayerController ControllingPlayer;
    private int lasthitID=999;

    private PlayerController thisPC;

    private static bool isQuiting = false;

    private void Awake()
    {
        thisPC = GetComponent<PlayerController>();
    }
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


    [PunRPC]
    public void HitBullet(int id, int ownerId)
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

    [PunRPC]
    public void ShowDeadEffect(Vector3 pos,Quaternion q)
    {
        Instantiate(Effect, pos, q);
    }



}
