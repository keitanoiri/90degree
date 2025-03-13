using Photon.Pun;
using TMPro;
using UnityEngine;

public class NameTag : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject nameObject;
    private TextMeshProUGUI nametext;
    [SerializeField] Transform PlayerTransformCenter;
    //private RectTransform namepos;

    private Camera activeCamera;
    
    void Start()
    {
        nametext = nameObject.GetComponent<TextMeshProUGUI>();
        nametext.text = photonView.Owner.NickName;
    }

    void Update()
    {
        if (activeCamera == null)
        {
            foreach (Camera cam in Camera.allCameras)
            {
                if (cam.isActiveAndEnabled)
                {
                    activeCamera = cam;
                    break;
                }
            }
            //active�J�������Ȃ�����~����
            return;
        }

        //Camera����v���C���[�Ɍ������x�N�g���̌v�Z
        Vector3 directionToCamera = activeCamera.transform.position-PlayerTransformCenter.position;
        RaycastHit hit;

        if (Physics.Raycast(PlayerTransformCenter.position,directionToCamera,out hit))//thisplayer position ����cam��transform��ray���΂�
        {
            if (hit.collider.gameObject.tag !="Player")//hit��player�łȂ��Ȃ�
            {
                //Debug.Log("��Q������");
                target.SetActive(false);
                return;
            }
        }

        //�v���C���[�̈ʒu���J�������猩���ʒu�ɕϊ�
        // �v���C���[�̓���̈ʒu�Ƀl�[���^�O��\��
        Vector3 screenPosition = activeCamera.WorldToScreenPoint(PlayerTransformCenter.position);

        // �J�����̎��E���ɂ��邩�m�F
        bool isInView = screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height;


        if (isInView)//camera�͈͓̔����H
        {
            target.SetActive(true);
            //name�̈ʒu���ړ�
            target.transform.position = screenPosition;
        }
        else
        {
            target.SetActive(false); 
        }
    }

}
