using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 playerDirection;
    [SerializeField] private float moveSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       float directionX = Input.GetAxisRaw("Horizontal");
       float directionY = Input.GetAxisRaw("Vertical");
       animator.SetFloat("moveX",directionX);
       animator.SetFloat("moveY",directionY);
       playerDirection = new Vector2(directionX,directionY).normalized;

       if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2"))
        {
            animator.SetBool("boosting",true);
        }
        else if(Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire2")){
             animator.SetBool("boosting",false);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(playerDirection.x * moveSpeed,playerDirection.y * moveSpeed);
    }
}
