using Photon.Pun;
using UnityEngine;
public class ItemBase : MonoBehaviourPunCallbacks
{
    //ベースといいつつ現状弾丸補充アイテム専用になっている(要拡張)
    [SerializeField] private AudioSource AudioSource;

    private void Start()
    {
        transform.position = transform.position + transform.up * 1.0f;
    }

    void Update()
    {
        this.transform.Rotate(Vector3.up, 0.1f);//回転させる
    }
    
    void OnTriggerEnter(Collider collider)//接触したのがプレイヤーなら(Ismineに限定すべきかも要件等)
    {
        if (collider.tag == "Player")
        {
            ItemActivate(collider);
        }
    }

    private void ItemActivate(Collider co)//アイテム効果を発動
    {
        Transform current = co.gameObject.transform;

        while (current.parent != null)
        {
            current = current.parent;
            PlayerController parentComponent = current.GetComponent<PlayerController>();
            PhotonView otherPhotonView = current.GetComponent<PhotonView>();
            if (parentComponent != null && otherPhotonView != null)
            {
                if (otherPhotonView.IsMine)
                {
                    //矢を補充
                    parentComponent.GetSupplyItem();

                    Debug.Log("親階層で PlayerController が見つかりました: " + current.name);
                    //アイテムの削除
                    photonView.RPC(nameof(DestroyNetWorkObject), RpcTarget.MasterClient);
                    return;
                }
            }
        }

        Debug.Log("親階層に PlayerController は存在しませんor自分のプレイヤーではありません");
    }

    [PunRPC]

    public void DestroyNetWorkObject()
    {
        //アイテムカウントをへらしてから削除
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.ItemCount--;
        }
        PhotonNetwork.Destroy(this.gameObject);

    }
}
