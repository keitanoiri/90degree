using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : MonoBehaviour
{
    //閉じるボタン
    public void OnClickedClose()
    {
        Destroy(gameObject);
    }

    //ページの変更用(今はまだ使わない)
    public void NextPage()
    {

    }

    public void PreviousPage() { }
}
