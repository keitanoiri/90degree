using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoadOutIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    //�N���b�N���ɕύX�I�[�o�[���C���Ɍ��ʕ\��
    [SerializeField]GameObject instanceText;
    private RoadOutMenu roadOutMenu;
    private GameObject temptext;

    [SerializeField] string expraintext;

    [SerializeField] int skillNum;
    [SerializeField] Sprite sprite;
    [SerializeField] RectTransform parentPanel;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = sprite;
        SkillBase skillbase = GetComponent<SkillBase>();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(skillbase.skillName);
        sb.AppendLine(skillbase.skillExprain);
        sb.AppendLine("�R�X�g:"+skillbase.DrowCost.ToString()+" �U�����x�F"+skillbase.DrowRate.ToString()+" �U���͕␳:"+skillbase.DamageReflectionRate.ToString());
        expraintext = sb.ToString();

}

    public void OnPointerEnter(PointerEventData eventData)
    {
        //�|�b�v�A�b�v���o�Ă��鏈��
        RectTransform thisRectTransform = GetComponent<RectTransform>();
        temptext = Instantiate(instanceText);
        temptext.GetComponent<Temptext>().Init(expraintext,parentPanel ,thisRectTransform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //�|�b�v�A�b�v�������鏈��
        Destroy(temptext);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //�؂�ւ������
        RoadOutMenu roadOutMenu = GameObject.FindObjectOfType<RoadOutMenu>();
        if(roadOutMenu != null )
        {
            string key = "Skill"+ roadOutMenu.SerectedIconNum;
            PlayerPrefs.SetInt(key, skillNum);
            roadOutMenu.ChengeIconImage(roadOutMenu.SerectedIconNum, sprite);

        }
        else
        {
            Debug.Log("Nulltyekku ");
        }
    }

}
