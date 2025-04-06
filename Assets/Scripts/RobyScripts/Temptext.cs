using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Temptext : MonoBehaviour
{
    //ロードアウトアイコンの一次テキスト用スクリプト
    [SerializeField]private TextMeshProUGUI textMeshProUGUI;
    //[SerializeField]private RectTransform IconsCanvas;

    public void Init(string text,RectTransform parenttransform,RectTransform cPosition)
    {
        RectTransform thistransform = GetComponent<RectTransform>();

        thistransform.SetParent(parenttransform);
        thistransform.position = cPosition.position+Vector3.left*-300;
        
        textMeshProUGUI.text = text;
    }

}
