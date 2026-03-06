using UnityEngine;

public class Fire : MonoBehaviour
{
    public float speed = 10f;
    [SerializeField] private GameObject destroyEffect;

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    void Start()
    {
        Destroy (gameObject,3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Obstacle") || collision.CompareTag("Whale"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            Instantiate(destroyEffect,transform.position, transform.rotation);
            GameManager.Instance.addkill();
        }
    }
}
