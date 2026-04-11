using UnityEngine;

// Animation effect that appear after destroying the objects such as Asteroids and Whales
public class Boom : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        Destroy(gameObject,animator.GetCurrentAnimatorStateInfo(0).length);
    }

}
