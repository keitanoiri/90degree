using UnityEngine;

public class Visible : MonoBehaviour
{
    //�I�u�W�F�N�g���J�����ɉf�邩���肷��
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
