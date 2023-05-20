using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float mySpeed = 5;
    public float jumpStrength = 12;

    private float horizontal;
    private bool isFacingRight = true;

    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Animator myAnimator;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        rb2d.velocity = new Vector2(horizontal * mySpeed, rb2d.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpStrength);
            myAnimator.SetBool("goingUp", true);
        }

        if (Input.GetButtonUp("Jump") && rb2d.velocity.y > 0f)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.5f);
        }

        FlipPlayer();

        // walk check
        if (horizontal == 0)
        {
            myAnimator.SetBool("isWalking", false);
        } else { myAnimator.SetBool("isWalking", true); }

        // jump check
        if (rb2d.velocity.y > 0)
        {
            myAnimator.SetBool("goingUp", true);
            myAnimator.SetBool("goingDown", false);
        } else if (rb2d.velocity.y < 0)
        {
            myAnimator.SetBool("goingDown", true);
            myAnimator.SetBool("goingUp", false);

        } else
        {
            myAnimator.SetBool("goingDown", false);
            myAnimator.SetBool("goingUp", false);
        }
        Debug.Log(rb2d.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            restartScene();
            Debug.Log("collided");
        }
    }



    private void FlipPlayer()
    {
        if (horizontal < 0 && isFacingRight || horizontal > 0 && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void restartScene()
    {
        SceneManager.LoadScene(0);
    }

}
