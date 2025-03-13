using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ESCCanvas : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject OptionsCanvas;
    // Start is called before the first frame update
    private void Start()
    {
        // カーソルをロックを解除
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartButton();
        }
    }

    public void StartButton()
    {
        // カーソルをロック(再び）
        Cursor.lockState = CursorLockMode.Locked;
        Destroy(gameObject);
        GameManager gm = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        if (gm!=null )
        {
            gm.enableToMove();
        }
    }

    public void OptionsButton() {
        Instantiate(OptionsCanvas);
    }
    public void QuitButton() { PhotonNetwork.LeaveRoom(); }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("Title");
    }
}
