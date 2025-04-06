using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadOutMenu : MonoBehaviour
{
    public int SerectedIconNum;
    [SerializeField] private int[] skills;
    [SerializeField] Sprite[] Sprites;

    [SerializeField]public Image[] IconImages;
    [SerializeField]public RectTransform skillWaku;

    private int mod;

    // Start is called before the first frame update
    void Start()
    {
        ////�����ݒ�(���݂̐ݒ肳�ꂽ�X�L���A�C�R����\��)
        skills[0] = PlayerPrefs.GetInt("Skill0", 0);
        skills[1] = PlayerPrefs.GetInt("Skill1", 1);
        skills[2] = PlayerPrefs.GetInt("Skill2", 2);
        mod = PlayerPrefs.GetInt("Mod", 0);

        for(int i=0;i<3; i++)
        {
            //�X�L���ɑΉ������A�C�R����
            ChengeIconImage(i, Sprites[skills[i]]);
        }

        SerectedIconNum = 0;
    }

    public void CloseMenu()
    {
        Destroy(this.gameObject);
    }

    public void ChengeIconImage(int IconNum, Sprite ChengeSprite)
    {
        //Icon�������ւ�
        IconImages[IconNum].sprite = ChengeSprite;
    }
}
