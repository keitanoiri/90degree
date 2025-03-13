using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE_Player : MonoBehaviour
{
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip drow;
    [SerializeField] private AudioClip shot;
    [SerializeField] private AudioClip[] works;
    [SerializeField] private AudioClip gravitySE;
    [SerializeField] private AudioClip damageSE;
    [SerializeField] private AudioClip ItemSE;
    [SerializeField] private AudioClip hitSE;
    [SerializeField] private AudioClip killSE;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SEdrow()
    {
        PlaySoundEffect(drow);
    }

    public void SEshot()
    {
        PlaySoundEffect(shot);
    }

    public void SEgravity()
    {
        AudioSource.PlayOneShot(gravitySE);
    }

    public void SEwork()
    {
        int randIndex = Random.Range(0, works.Length);
        AudioSource.PlayOneShot(works[randIndex]);
    }

    public void SEdamage()
    {
        PlaySoundEffect(damageSE);
    }

    public void SEhit()
    {
        PlaySoundEffect(hitSE);
    }

    public void SEkill()
    {
        PlaySoundEffect(killSE);
    }

    public void SEItem()
    {
        PlaySoundEffect(ItemSE);
    }

    // �T�E���h���㏑�����čĐ����郁�\�b�h
    public void PlaySoundEffect(AudioClip clip)
    {
        // AudioSource �̃N���b�v��ύX
        AudioSource.clip = clip;

        // ���ݍĐ����̃T�E���h���㏑�����čĐ�
        AudioSource.Play();
    }

}
