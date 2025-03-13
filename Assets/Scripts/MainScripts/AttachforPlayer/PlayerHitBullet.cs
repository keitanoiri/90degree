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
            //シールドがアクティブならダメージ判定はなし
            if (thisPC.P_Shield.activeSelf) return;
            
            if (other.collider.TryGetComponent<Ballet>(out var bullet))
            {
                if(bullet.OwnerId != PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    SE.SEdamage();
                    //弾に当たった
                    lasthitID = bullet.OwnerId;

                    int nowHP = PhotonNetwork.LocalPlayer.GetHP();
                    PhotonNetwork.LocalPlayer.SetHP(nowHP - bullet.damage);

                    photonView.RPC(nameof(HitBullet), RpcTarget.All, bullet.Id, bullet.OwnerId);
                    Player targetPlayer = PhotonNetwork.CurrentRoom.Players[bullet.OwnerId];

                    GameObject manager = GameObject.Find("GameManager");
                    if (manager.TryGetComponent<GameManager>(out GameManager gm))
                    {
                        if (0 <= nowHP && nowHP <= bullet.damage)//判定前のHPがダメージより小さかったらダブルキルを防げる？
                        {//プレイヤーが死ぬ処理
                            photonView.RPC(nameof(ShowKillEffect), targetPlayer);

                            PhotonNetwork.LocalPlayer.SetDeath(PhotonNetwork.LocalPlayer.GetDeath() + 1);//death加算
                            PhotonNetwork.CurrentRoom.Players[bullet.OwnerId].SetKill(PhotonNetwork.CurrentRoom.Players[bullet.OwnerId].GetKill() + 1);//kill加算

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
                        Debug.Log("ゲームマネージャーがみつからないよ");
                    }


                    


                }
            }else if (other.gameObject.tag == "DamageArea")//damage壁に触れたら破壊してデス数を加算する
            {
                if(lasthitID != 999){
                    PhotonNetwork.CurrentRoom.Players[lasthitID].SetKill(PhotonNetwork.CurrentRoom.Players[lasthitID].GetKill() + 1);//kill加算
                }
                PhotonNetwork.LocalPlayer.SetHP(0);
                PhotonNetwork.LocalPlayer.SetDeath(PhotonNetwork.LocalPlayer.GetDeath() + 1);//death加算
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
        //自分の操作キャラを探す
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
        //自分の操作キャラを探す
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
