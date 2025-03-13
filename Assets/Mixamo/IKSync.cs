using Photon.Pun;
using UnityEngine;

public class IKSync : MonoBehaviourPun, IPunObservable
{
    public Transform IKtarget;

    private Vector3 networkedPosition;
    private Quaternion networkedRotation;

    void Start()
    {
        if (!photonView.IsMine)
        {
            // 他のプレイヤーのターゲットを同期するための初期値を設定
            networkedPosition = IKtarget.position;
            networkedRotation = IKtarget.rotation;
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // 自分のターゲットをコントロールする場合は特に何もしない
        }
        else
        {
            // 他のプレイヤーのターゲットを徐々にネットワークの値に近づける
            IKtarget.position = Vector3.Lerp(IKtarget.position, networkedPosition, Time.deltaTime * 10);
            IKtarget.rotation = Quaternion.Lerp(IKtarget.rotation, networkedRotation, Time.deltaTime * 10);
        }
    }

    // ネットワークを通じてデータを同期する
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 自分のデータを送信
            stream.SendNext(IKtarget.position);
            stream.SendNext(IKtarget.rotation);
        }
        else
        {
            // 他プレイヤーからデータを受信
            networkedPosition = (Vector3)stream.ReceiveNext();
            networkedRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
