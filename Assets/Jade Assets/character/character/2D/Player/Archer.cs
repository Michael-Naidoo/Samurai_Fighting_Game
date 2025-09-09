using UnityEngine;

public class Archer : MonoBehaviour
{
    public Animator animator;

    float horizontalMove = 0f;
    public float runSpeed = 40f;


    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("isWalking", Mathf.Abs(horizontalMove));

        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetBool("BowAttack", true);
        }
        else
        {
            animator.SetBool("BowAttack", false);

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("LowSwordAttack", true);
        }
        else
        {
            animator.SetBool("LowSwordAttack", false);

        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetBool("HighSwordAttack", true);
        }
        else
        {
            animator.SetBool("HighSwordAttack", false);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("Jump", false);

        }
    }
}
