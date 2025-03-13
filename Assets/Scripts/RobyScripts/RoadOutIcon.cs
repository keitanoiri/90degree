using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoadOutIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    //クリック時に変更オーバーレイ時に効果表示
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
        sb.AppendLine("コスト:"+skillbase.DrowCost.ToString()+" 攻撃速度："+skillbase.DrowRate.ToString()+" 攻撃力補正:"+skillbase.DamageReflectionRate.ToString());
        expraintext = sb.ToString();

}

    public void OnPointerEnter(PointerEventData eventData)
    {
        //ポップアップが出てくる処理
        RectTransform thisRectTransform = GetComponent<RectTransform>();
        temptext = Instantiate(instanceText);
        temptext.GetComponent<Temptext>().Init(expraintext,parentPanel ,thisRectTransform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //ポップアップが消える処理
        Destroy(temptext);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //切り替える役割
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
