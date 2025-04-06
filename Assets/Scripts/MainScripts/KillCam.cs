using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KillCam : MonoBehaviourPunCallbacks
{

    //倒されたときに生成するカメラ(ここでリスポーン処理をGMにさせている)
    GameManager GM;
    float yRotation=0;
    float xRotation=0;
    float mouseSensitivity = 100;

    // Start is called before the first frame update
    async void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        var token = this.GetCancellationTokenOnDestroy(); // GameObject が破壊されたらキャンセル
        try
        {
            await UniTask.Delay((int)(GM.RespownTime * 1000), cancellationToken: token);
            Destroy(gameObject);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("オブジェクトが破壊されたためタスクをキャンセルしました");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // マウス入力の取得と体の向きの変更
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        yRotation -= mouseY;
        xRotation += mouseX;

        transform.rotation = Quaternion.Euler(yRotation, xRotation, 0);

    }
}
