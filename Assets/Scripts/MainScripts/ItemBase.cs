using Photon.Pun;
using UnityEngine;
public class ItemBase : MonoBehaviourPunCallbacks
{

    [SerializeField] private AudioSource AudioSource;

    private void Start()
    {
        transform.position = transform.position + transform.up * 1.0f;
    }

    void Update()
    {
        this.transform.Rotate(Vector3.up, 0.1f);
    }
    
    void OnTriggerEnter(Collider collider)
    {


        if (collider.tag == "Player")
        {
            ItemActivate(collider);
        }
    }

    private void ItemActivate(Collider co)
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
                    //����[
                    parentComponent.GetSupplyItem();

                    Debug.Log("�e�K�w�� PlayerController ��������܂���: " + current.name);
                    //�A�C�e���̍폜
                    photonView.RPC(nameof(DestroyNetWorkObject), RpcTarget.MasterClient);
                    return;
                }
            }
        }

        Debug.Log("�e�K�w�� PlayerController �͑��݂��܂���or�����̃v���C���[�ł͂���܂���");
    }

    [PunRPC]

    public void DestroyNetWorkObject()
    {
        //�A�C�e���J�E���g���ւ炵�Ă���폜
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.ItemCount--;
        }
        PhotonNetwork.Destroy(this.gameObject);

    }
}
