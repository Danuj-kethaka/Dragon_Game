using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 playerDirection;
    [SerializeField] private float moveSpeed;
    public float boost =1f;
    private bool boosting = false;
    private float boostPower = 5f;
    public bool canMove = false;
    [SerializeField]private float energy;
    [SerializeField]private float maxEnergy;
    [SerializeField]private float energyRegen;
     [SerializeField]private float health;
    [SerializeField]private float maxHealth;
    [SerializeField] private GameObject destroyEffect;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        energy = maxEnergy;
        BackgroundController.Instance.UpdateEnergySlider(energy,maxEnergy);
        health = maxHealth;
        BackgroundController.Instance.UpdateHealthSlider(health,maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
       if(!canMove) return;

       float directionX = Input.GetAxisRaw("Horizontal");
       float directionY = Input.GetAxisRaw("Vertical");
       animator.SetFloat("moveX",directionX);
       animator.SetFloat("moveY",directionY);
       playerDirection = new Vector2(directionX,directionY).normalized;

       if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2"))
        {
            EnterBoost();
        }
        else if(Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire2")){
            ExitBoost();
        }
    }

    void FixedUpdate()
    {
        if(!canMove) return;
        
        rb.linearVelocity = new Vector2(playerDirection.x * moveSpeed,playerDirection.y * moveSpeed);
        if (boosting)
        {
            if(energy>=0.2f)energy -= 0.2f;
            else
            {
                ExitBoost();
            }
        }
        else
        {
            if(energy<maxEnergy)
            {
                energy += energyRegen;
            }
        }
        BackgroundController.Instance.UpdateEnergySlider(energy,maxEnergy);
    }

    private void EnterBoost()
    {
        if (energy > 10)
        {
            animator.SetBool("boosting",true);
            boost = boostPower;
            boosting = true;
        }
    }

    private void ExitBoost()
    {
         animator.SetBool("boosting",false);
         boost = 1f;
         boosting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Whale"))
        {
            TakeDamage(1);
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        BackgroundController.Instance.UpdateHealthSlider(health,maxHealth);
        if(health<= 0)
        {
            boost = 0f;
            gameObject.SetActive(false);
            Instantiate(destroyEffect,transform.position, transform.rotation);
            APIGameManager.Instance.TriggerQuiz();
        }
    }

    public void RestoreHealth()
    {
        health = maxHealth;
        BackgroundController.Instance.UpdateHealthSlider(health,maxHealth);
        gameObject.SetActive(true);
    }
}
