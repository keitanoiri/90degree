using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoadOutSerectedIcon : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    //���[�h�A�E�g�̉����̃A�C�R���p�̃X�N���v�g
    [SerializeField] int iconindex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //�|�b�v�A�b�v���o�Ă��鏈��
        //RectTransform thisRectTransform = GetComponent<RectTransform>();
        //temptext = Instantiate(instanceText);
        //temptext.GetComponent<Temptext>().Init(expraintext, parentPanel, thisRectTransform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //�|�b�v�A�b�v�������鏈��
        //Destroy(temptext);
    }

    //���̃A�C�R�����I�����ꂽ���Ƃ�����
    public void OnPointerClick(PointerEventData eventData)
    {
        //�؂�ւ������
        RoadOutMenu roadOutMenu = GameObject.FindObjectOfType<RoadOutMenu>();
        if (roadOutMenu != null)
        {
            roadOutMenu.SerectedIconNum = iconindex;
            roadOutMenu.skillWaku.position = this.GetComponent<RectTransform>().position;
        }
        else
        {
            Debug.Log("Nulltyekku ");
        }
    }

}
