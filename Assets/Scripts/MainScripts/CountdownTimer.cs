using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviourPun
{
    public TextMeshProUGUI countdownText;   // �J�E���g�_�E����\������UI�e�L�X�g
    public int countdownDuration = 10;  // �J�E���g�_�E���̕b��
    private double countdownStartTime; // �J�E���g�_�E���J�n����


    // Start is called before the first frame update
    void Start()
    {
        //�X�^�[�g�̃J�E���g�_�E��
        if (PhotonNetwork.IsMasterClient)
        {
            StartCountdown();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    private void StartCountdown()
    {
        countdownStartTime = PhotonNetwork.Time;
        photonView.RPC(nameof(SyncCountdownStartTime), RpcTarget.Others, countdownStartTime);
    }

    [PunRPC]
    public void SyncCountdownStartTime(double startTime)
    {
        countdownStartTime = startTime;
    }

    private void Timer()
    {
        if (countdownStartTime > 0)
        {
            double elapsed = PhotonNetwork.Time - countdownStartTime;
            int remainingTime = countdownDuration - Mathf.FloorToInt((float)elapsed);

            if (remainingTime >= 0)
            {
                countdownText.text = remainingTime.ToString();

                GameManager gm = GetComponent<GameManager>();
                if(gm != null)gm.anenableToMove();
            }
            else
            {
                countdownText.text = "Start!";
                GetComponent<GameManager>().enableToMove();
                enabled = false;
            }
        }
    }
}
