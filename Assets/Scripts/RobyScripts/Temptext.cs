using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Temptext : MonoBehaviour
{
    //���[�h�A�E�g�A�C�R���̈ꎟ�e�L�X�g�p�X�N���v�g
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
