using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviourPunCallbacks
{
    //HP表示用スクリプト(ダメージに応じて画面を赤くしてる)
    private int MaxHP;
    [SerializeField] private TextMeshProUGUI playername;
    [SerializeField] private Image Damage;
    private float elapsedTime;
    private Color chengeColor;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0f;
        PlayerController pl = GameObject.FindObjectOfType<PlayerController>();
        MaxHP = pl.MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        //ルームに参加していなければやらない
        if (!PhotonNetwork.InRoom) { return; }

        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.2f)
        {
            elapsedTime = 0f;
            UpdateLabel();
        }
    }

    private void UpdateLabel()
    {
        playername.text = $"{PhotonNetwork.LocalPlayer.GetHP()}";
        chengeColor = Damage.color;
        float alpha = Mathf.Lerp(0, 1, (MaxHP - PhotonNetwork.LocalPlayer.GetHP()) / (float)MaxHP);
        //float alpha = Mathf.Clamp((MaxHP - PhotonNetwork.LocalPlayer.GetHP()) / MaxHP, 0,1);

        chengeColor.a = alpha;

        Damage.color = chengeColor;

        //Debug.Log(alpha);
    }
}
