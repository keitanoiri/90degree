using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using UnityEngine;
public class PlayerHitBullet : MonoBehaviourPunCallbacks
{
    //プレイヤーのダメージ判定を管理するスクリプト

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
        //生成時にゲームマネージャーを探しておく
        manager = GameObject.Find("GameManager");
        if (manager.TryGetComponent<GameManager>(out gm))
        {
            Debug.Log("ゲームマネージャーがみつかったよ");
        }
        else
        {
            Debug.Log("ゲームマネージャーがみつからないよ");
        }
    }

    //当てた側で判定
    private void OnCollisionEnter(Collision other)
    {
        //自身のキャラクターではないなら
        if (!photonView.IsMine)
        {
            Debug.Log("当たった");
            //シールドがアクティブならダメージ判定はなし
            if (thisPC.P_Shield.activeSelf) return;
            Debug.Log("シールドが非アクティブ");
            //当たったのが弾丸であるなら
            if (other.collider.TryGetComponent<Ballet>(out var bullet))
            {
                //弾丸のIDとその環境の主が同じならダメージ判定を行う
                if (bullet.OwnerId == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    //当たった矢の消去
                    photonView.RPC(nameof(DestroyBullet), RpcTarget.All, bullet.Id, bullet.OwnerId);
                    //ここで当てられた側の判定を入れる。(ダメージ計算等はは当てられた側で処理する)
                    Player targetPlayer = PhotonNetwork.CurrentRoom.Players[photonView.OwnerActorNr];
                    photonView.RPC(nameof(HitBullet), targetPlayer, bullet.OwnerId,bullet.damage,bullet.transform.position);
                }
            }
        }//このオブジェクトが自身の操作キャラであり、ダメージ壁にあたったら
        else if (other.gameObject.tag == "DamageArea")//damage壁に触れたら破壊してデス数を加算する
        {
            //最後に当たった対象がデフォルトのままであるならば(同じルームで無限に対戦するとエラーになる脆弱性あり)
            if (lasthitID != 999)
            {
                PhotonNetwork.CurrentRoom.Players[lasthitID].SetKill(PhotonNetwork.CurrentRoom.Players[lasthitID].GetKill() + 1);//kill加算
                //最後に攻撃を当てた人に倒したことを伝える
                Player targetPlayer = PhotonNetwork.CurrentRoom.Players[lasthitID];
                photonView.RPC(nameof(ShowKillEffect), targetPlayer);
            }
            PhotonNetwork.LocalPlayer.SetHP(0);
            PhotonNetwork.LocalPlayer.SetDeath(PhotonNetwork.LocalPlayer.GetDeath() + 1);//death加算

            photonView.RPC(nameof(ShowDeadEffect), RpcTarget.All, transform.position, transform.rotation);
            gm.RespownPlayer();

            GameObject killcam = Instantiate(Killcam);
            killcam.transform.position = transform.position;
            PhotonNetwork.Destroy(Player);
        }

    }

    //当てられた側で判定
    /*
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

    */
    [PunRPC]
    public void HitBullet(int OwnerId,int Damage,Vector3 BulletPos)
    {
        //弾に当たった
        lasthitID = OwnerId;
        int nowHP = PhotonNetwork.LocalPlayer.GetHP();
        PhotonNetwork.LocalPlayer.SetHP(nowHP - Damage);
        Player targetPlayer = PhotonNetwork.CurrentRoom.Players[OwnerId];

        if (0 <= nowHP && nowHP <= Damage)//判定前のHPがダメージより小さかったらダブルキルを防げる？
        {//プレイヤーが死ぬ処理
            photonView.RPC(nameof(ShowKillEffect), targetPlayer);

            PhotonNetwork.LocalPlayer.SetDeath(PhotonNetwork.LocalPlayer.GetDeath() + 1);//death加算
            PhotonNetwork.CurrentRoom.Players[OwnerId].SetKill(PhotonNetwork.CurrentRoom.Players[OwnerId].GetKill() + 1);//kill加算

            //破壊されたエフェクトの生成
            photonView.RPC(nameof(ShowDeadEffect), RpcTarget.All, transform.position, transform.rotation);
            gm.RespownPlayer();

            GameObject killcam = Instantiate(Killcam);
            killcam.transform.position = BulletPos;

            PhotonNetwork.Destroy(Player);
        }
        else
        {
            //攻撃を当てたプレイヤーにhit確認させる。
            photonView.RPC(nameof(ShowHitEffect), targetPlayer);
        }
    }

    //攻撃したプレイヤーに攻撃が当たったことを伝える
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

    //攻撃したプレイヤーに相手が死んだことを伝える
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

    //弾丸の破壊
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

    //破壊されたエフェクトを表示
    [PunRPC]
    public void ShowDeadEffect(Vector3 pos, Quaternion q)
    {
        Instantiate(Effect, pos, q);
    }



}
