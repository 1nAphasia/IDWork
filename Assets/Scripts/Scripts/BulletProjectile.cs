using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody bulletRigidbody;
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        float speed = 90f;
        bulletRigidbody.linearVelocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BulletTarget>() != null)
        {
            //Hit Target
            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
        }
        else
        {
            //Hit Something Else
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
