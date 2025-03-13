using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    public string skillName;
    public string skillExprain;

    public int DrowCost;
    public float DrowRate;
    public float DamageReflectionRate;

    public PlayerController pc;
    public AnimationController anicon;
    //
    private void Start()
    {
        if (TryGetComponent<PlayerController>(out pc))
        {
            Debug.Log("Succsess Get PlayerController");
        }
        else {
            Debug.Log("Fail Get PlayerController");
        };

        if (anicon = GetComponentInChildren<AnimationController>())
        {
            Debug.Log("Succsess Get AnimationController");
        }
        else
        {
            Debug.Log("Fail Get AnimationController");
        };
    }

    // 抽象メソッド：各武器で実装
    public abstract void Shoot();
    public abstract void Drow();
}
