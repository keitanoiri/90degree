using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoadOutSerectedIcon : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    //ロードアウトの下側のアイコン用のスクリプト
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
        //ポップアップが出てくる処理
        //RectTransform thisRectTransform = GetComponent<RectTransform>();
        //temptext = Instantiate(instanceText);
        //temptext.GetComponent<Temptext>().Init(expraintext, parentPanel, thisRectTransform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //ポップアップが消える処理
        //Destroy(temptext);
    }

    //このアイコンが選択されたことを示す
    public void OnPointerClick(PointerEventData eventData)
    {
        //切り替える役割
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
