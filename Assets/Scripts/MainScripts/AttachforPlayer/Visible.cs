using UnityEngine;

public class Visible : MonoBehaviour
{
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
