using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionSliderAndInputField : MonoBehaviour
{

    [SerializeField] Slider Slider;
    [SerializeField] TMP_InputField Field;

    private string previousText = ""; // �O��̗L���ȓ��͒l
    // Start is called before the first frame update
    void Start()
    {
        // onValueChanged�Ƀ��X�i�[��ǉ�
        Field.onValueChanged.AddListener(OnTMPInputFieldChanged);
        Slider.onValueChanged.AddListener(OnSliderChenged);
        Field.text = Slider.value.ToString();
    }

    // TMP_InputField�̃e�L�X�g���ύX���ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    void OnTMPInputFieldChanged(string input)
    {
        if(float.TryParse(input,out float inputfloat))
        {
            inputfloat = Mathf.Clamp(inputfloat,0,100);
            inputfloat = Mathf.Floor(inputfloat * 10) / 10;
            previousText = inputfloat.ToString();
            previousText = Field.text;
            Slider.value = inputfloat;
        }
        else
        {
            Field.text = previousText;
        }
    }

    void OnSliderChenged(float input)
    {
        float chenge = Mathf.Floor(input * 10) / 10;
        Field.text = chenge.ToString();
    }

    // �I�u�W�F�N�g���j�������Ƃ��Ƀ��X�i�[������
    void OnDestroy()
    {
        Field.onValueChanged.RemoveListener(OnTMPInputFieldChanged);
        Slider.onValueChanged.RemoveListener(OnSliderChenged);
    }
}
