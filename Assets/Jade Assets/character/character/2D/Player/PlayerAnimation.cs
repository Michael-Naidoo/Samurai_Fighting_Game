using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;

    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool isWalking = false;
    
   

    public UnityEvent OnLandEvent;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("walkSpeed", Mathf.Abs(horizontalMove));
        isWalking = true;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            //jump = true;
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("Jump", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("LightAttack", true);
        }
        else
        {
            animator.SetBool("LightAttack", false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("Blocking", true);
        }
        else
        {
            animator.SetBool("Blocking", false);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetBool("LowAttack", true);
        }
        else
        {
            animator.SetBool("LowAttack", false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetBool("Parry", true);
        }
        else
        {
            animator.SetBool("Parry", false);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetBool("HighDagger", true);
        }
        else
        {
            animator.SetBool("HighDagger", false);
        }

        //if (isWalking && Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    animator.SetBool("Running", true);
        //}
        //else
        //{
        //   animator.SetBool("Running", false);
        //}

        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //{
        // animator.SetFloat("runSpeed", Mathf.Abs(horizontalMove));
        //}

    }

    //public void OnLanding()
    //{
       // animator.SetBool("Jump", false);
   // }

    //public void AttackEnd()
    //{
    //    animator.SetBool("LightAttack", false);
    //}
}