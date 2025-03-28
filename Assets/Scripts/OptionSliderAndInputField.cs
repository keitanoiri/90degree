using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionSliderAndInputField : MonoBehaviour
{

    [SerializeField] Slider Slider;
    [SerializeField] TMP_InputField Field;

    private string previousText = ""; // 前回の有効な入力値
    // Start is called before the first frame update
    void Start()
    {
        // onValueChangedにリスナーを追加
        Field.onValueChanged.AddListener(OnTMPInputFieldChanged);
        Slider.onValueChanged.AddListener(OnSliderChenged);
        Field.text = Slider.value.ToString();
    }

    // TMP_InputFieldのテキストが変更されたときに呼び出されるメソッド
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

    // オブジェクトが破棄されるときにリスナーを解除
    void OnDestroy()
    {
        Field.onValueChanged.RemoveListener(OnTMPInputFieldChanged);
        Slider.onValueChanged.RemoveListener(OnSliderChenged);
    }
}
