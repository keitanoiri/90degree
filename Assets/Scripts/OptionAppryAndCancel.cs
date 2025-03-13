
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionAppryAndCancel : MonoBehaviour
{
    [SerializeField] Slider MasterSlider;
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SESlider;
    [SerializeField] Slider VoiceSlider;
    [SerializeField] TMP_Dropdown ScreenMode;
    [SerializeField] TMP_Dropdown ScreenSize;
    [SerializeField] Slider MouceSencitivityVartical;
    [SerializeField] Slider MouceSencitivityHorizontal;
    [SerializeField] AudioMixer AudioMixer;
    // Start is called before the first frame update
    void Awake()
    {
        //各値を初期設定(Cancelを押したときと同じ)
        OnClickCancel();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickClose();
        }
    }

    // Update is called once per frame

    public void OnClickAppry()
    {
        PlayerPrefs.SetFloat("MasterVolume", MasterSlider.value);
        PlayerPrefs.SetFloat("BGMVolume", BGMSlider.value);
        PlayerPrefs.SetFloat("SEVolune", SESlider.value);
        PlayerPrefs.SetFloat("VoiceVolume", VoiceSlider.value);
        PlayerPrefs.SetInt("ScreenMode", ScreenMode.value);
        PlayerPrefs.SetInt("ScreenSize", ScreenSize.value);
        PlayerPrefs.SetFloat("Vartical", MouceSencitivityVartical.value);
        PlayerPrefs.SetFloat("Horizontal", MouceSencitivityHorizontal.value);

        FullScreenMode screenmode;
        int x, y;
        switch (ScreenMode.value)
        {
            case 0:
                screenmode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                screenmode = FullScreenMode.Windowed;
                break;
            case 2:
                screenmode = FullScreenMode.MaximizedWindow;
                break;
            default:
                screenmode = FullScreenMode.FullScreenWindow;
                break;
        }
        switch (ScreenSize.value)
        {
            case 0:
                x= 1920; y = 1080; break;
            case 1:
                x= 1920; y= 1200; break;
            case 2:
                x= 2560; y= 1080; break;
            case 3:
                x= 1600; y= 1200; break;
            default:
                x= 1920; y= 1080; break;
        }
        Screen.SetResolution(x, y, screenmode);

        float db1 =　Mathf.Lerp(-40f,20f,MasterSlider.value/100);
        if (db1 != -20)
        {
            AudioMixer.SetFloat("MasterParam", db1);
        }
        else
        {
            AudioMixer.SetFloat("MasterParam", -80);
        }
        float db2 = Mathf.Lerp(-40f, 20f, BGMSlider.value / 100);
        if (db2 != -20)
        {
            AudioMixer.SetFloat("BGMParam", db2);
        }
        else
        {
            AudioMixer.SetFloat("BGMParam", -80);
        }
        float db3 = Mathf.Lerp(-40f, 20f, SESlider.value / 100);
        if (db3 != -20)
        {
            AudioMixer.SetFloat("SEParam", db3);
        }else
        {
            AudioMixer.SetFloat("SEParam", -80);
        }
        float db4 = Mathf.Lerp(-40f, 20f, VoiceSlider.value / 100);
        if (db4 != -20)
        {
            AudioMixer.SetFloat("VoiceParam", db4);
        }
        else
        {
            AudioMixer.SetFloat("VoiceParam", -80);
        }
        
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();  // 全ての保存データを削除
        OnClickCancel ();//再設定
    }

    public void OnClickCancel()//初期設定でもある
    {
        MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 50);
        BGMSlider.value = PlayerPrefs.GetFloat("BGMVolume", 50);
        SESlider.value = PlayerPrefs.GetFloat("SEVolune", 50);
        VoiceSlider.value = PlayerPrefs.GetFloat("VoiceVolume", 50);
        ScreenMode.value = PlayerPrefs.GetInt("ScreenMode", 0);
        ScreenSize.value = PlayerPrefs.GetInt("ScreenSize", 0);
        MouceSencitivityVartical.value = PlayerPrefs.GetFloat("Vartical", 50);
        MouceSencitivityHorizontal.value = PlayerPrefs.GetFloat("Horizontal", 50);

    }

    public void OnClickClose()
    {
        Destroy(this.gameObject);
    }
}
