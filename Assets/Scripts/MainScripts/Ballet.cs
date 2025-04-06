using Photon.Pun;
using UnityEngine;

public class Ballet : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// /矢に付けるスクリプト
    /// </summary>

    //アタッチした対象のRightBody
    [SerializeField]private Rigidbody rb;
    //矢のベースの速さ
    [SerializeField] private float speed;
    //接触判定用のコライダーを設定しておく
    [SerializeField] public SphereCollider hitCollider;
    public Vector3 gravityForce;
    
    //ベースダメージ
    [SerializeField] public int damage;
    //持続時間（寿命）
    [SerializeField] float life = 20;

    [SerializeField] TrailRenderer trailRenderer;
    // 弾のIDを返すプロパティ
    public int Id { get; private set; }
    // 弾を発射したプレイヤーのIDを返すプロパティ
    public int OwnerId { get; private set; }
    // 同じ弾かどうかをIDで判定するメソッド
    public bool Equals(int id, int ownerId) => id == Id && ownerId == OwnerId;

    //初期設定
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
    //初期設定(バリエーション)
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
    //ダメージによって軌道を変更する
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
            // 角速度もゼロにして回転も停止
            //rb.angularVelocity = Vector3.zero;
            // 必要に応じてRigidbodyの動作を完全に停止する

            rb.isKinematic = true;
        }
    }
}
