using Photon.Pun;
using UnityEngine;

public class Ballet : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// /��ɕt����X�N���v�g
    /// </summary>

    //�A�^�b�`�����Ώۂ�RightBody
    [SerializeField]private Rigidbody rb;
    //��̃x�[�X�̑���
    [SerializeField] private float speed;
    //�ڐG����p�̃R���C�_�[��ݒ肵�Ă���
    [SerializeField] public SphereCollider hitCollider;
    public Vector3 gravityForce;
    
    //�x�[�X�_���[�W
    [SerializeField] public int damage;
    //�������ԁi�����j
    [SerializeField] float life = 20;

    [SerializeField] TrailRenderer trailRenderer;
    // �e��ID��Ԃ��v���p�e�B
    public int Id { get; private set; }
    // �e�𔭎˂����v���C���[��ID��Ԃ��v���p�e�B
    public int OwnerId { get; private set; }
    // �����e���ǂ�����ID�Ŕ��肷�郁�\�b�h
    public bool Equals(int id, int ownerId) => id == Id && ownerId == OwnerId;

    //�����ݒ�
    public void Init(int id, int ownerId,Vector3 gravityangle,Vector3 pos,Quaternion rotate,float DAMAGE)
    {

        Id = id;
        OwnerId = ownerId;
        gravityForce = gravityangle;
        transform.position = pos;
        transform.rotation = rotate;
        damage = (int)DAMAGE;
        rb.AddForce(transform.forward * speed,ForceMode.Impulse);
        ChengeTrailColor(damage);
    }
    //�����ݒ�(�o���G�[�V����)
    public void Init(int id, int ownerId, Vector3 gravityangle, Vector3 pos, Quaternion rotate, float DAMAGE,float SPEED)
    {
        Id = id;
        OwnerId = ownerId;
        gravityForce = gravityangle;
        transform.position = pos;
        transform.rotation = rotate;
        damage = (int)DAMAGE;
        rb.AddForce(transform.forward * speed*SPEED, ForceMode.Impulse);
        ChengeTrailColor(damage);

    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        Gravity();
        life-=Time.deltaTime;
        if(life < 0 )
        {
            Destroy(this.gameObject);
        }
    }

    private void Gravity()
    {
        rb.AddForce(gravityForce, ForceMode.Acceleration);
    }
    //�_���[�W�ɂ���ċO����ύX����
    private void ChengeTrailColor(int DAMAGE)
    {
        if (DAMAGE >= 100)
        {
            trailRenderer.startColor = Color.red;
            trailRenderer.endColor = Color.red;
        }
        else if (DAMAGE >= 50)
        {
            trailRenderer.startColor = Color.white;
            trailRenderer.endColor = Color.white;
        }
        else
        {
            trailRenderer.startColor = Color.blue;
            trailRenderer.endColor = Color.blue;
        }
    }

    public void StopBullet()
    {
        rb.isKinematic = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //rb.velocity = Vector3.zero;
            // �p���x���[���ɂ��ĉ�]����~
            //rb.angularVelocity = Vector3.zero;
            // �K�v�ɉ�����Rigidbody�̓�������S�ɒ�~����

            rb.isKinematic = true;
        }
    }
}
