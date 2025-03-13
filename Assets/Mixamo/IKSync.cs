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
            // ���̃v���C���[�̃^�[�Q�b�g�𓯊����邽�߂̏����l��ݒ�
            networkedPosition = IKtarget.position;
            networkedRotation = IKtarget.rotation;
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // �����̃^�[�Q�b�g���R���g���[������ꍇ�͓��ɉ������Ȃ�
        }
        else
        {
            // ���̃v���C���[�̃^�[�Q�b�g�����X�Ƀl�b�g���[�N�̒l�ɋ߂Â���
            IKtarget.position = Vector3.Lerp(IKtarget.position, networkedPosition, Time.deltaTime * 10);
            IKtarget.rotation = Quaternion.Lerp(IKtarget.rotation, networkedRotation, Time.deltaTime * 10);
        }
    }

    // �l�b�g���[�N��ʂ��ăf�[�^�𓯊�����
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // �����̃f�[�^�𑗐M
            stream.SendNext(IKtarget.position);
            stream.SendNext(IKtarget.rotation);
        }
        else
        {
            // ���v���C���[����f�[�^����M
            networkedPosition = (Vector3)stream.ReceiveNext();
            networkedRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
