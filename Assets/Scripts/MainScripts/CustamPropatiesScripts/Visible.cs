using UnityEngine;

public class Visible : MonoBehaviour
{
    //オブジェクトがカメラに映るか判定する
    public bool IsVisivle { get; private set; }

    private void OnBecameVisible()
    {
        IsVisivle = true;
    }

    private void OnBecameInvisible()
    {
        IsVisivle = false;
    }
}
