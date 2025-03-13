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
            //activeカメラがない時停止する
            return;
        }

        //Cameraからプレイヤーに向けたベクトルの計算
        Vector3 directionToCamera = activeCamera.transform.position-PlayerTransformCenter.position;
        RaycastHit hit;

        if (Physics.Raycast(PlayerTransformCenter.position,directionToCamera,out hit))//thisplayer position からcamのtransformへrayを飛ばす
        {
            if (hit.collider.gameObject.tag !="Player")//hitがplayerでないなら
            {
                //Debug.Log("障害がある");
                target.SetActive(false);
                return;
            }
        }

        //プレイヤーの位置をカメラから見た位置に変換
        // プレイヤーの頭上の位置にネームタグを表示
        Vector3 screenPosition = activeCamera.WorldToScreenPoint(PlayerTransformCenter.position);

        // カメラの視界内にいるか確認
        bool isInView = screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height;


        if (isInView)//cameraの範囲内か？
        {
            target.SetActive(true);
            //nameの位置を移動
            target.transform.position = screenPosition;
        }
        else
        {
            target.SetActive(false); 
        }
    }

}
