using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float JUMP_AMOUNT = 15f;
    private float BOOST = 3f;
    private Rigidbody2D rigidbody2D;
    private bool P;

    public static bool dead;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        P = false;
        dead = false;
        animator.SetBool("isDead", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (TestInput())
        {
            animator.SetBool("isFalling", false);
            if (rigidbody2D.gravityScale == JUMP_AMOUNT * BOOST) {
                rigidbody2D.gravityScale = -JUMP_AMOUNT;
            }else{
                rigidbody2D.gravityScale = -JUMP_AMOUNT * BOOST;
            }
        }
        else
        {
            animator.SetBool("isFalling", true);
            if (rigidbody2D.gravityScale == -JUMP_AMOUNT * BOOST)
            {
                rigidbody2D.gravityScale = JUMP_AMOUNT;
            }
            else
            {
                rigidbody2D.gravityScale = JUMP_AMOUNT * BOOST;
            }
        }
        transform.eulerAngles = new Vector3(0, 0, rigidbody2D.velocity.y * .30f);
    }
    
    void Pause() {
        if (P) {
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            rigidbody2D.bodyType = RigidbodyType2D.Static;
        }
    }

    private bool TestInput()
    {
        return
            Input.GetKey(KeyCode.Space) ||
            Input.GetMouseButton(0) ||
            Input.touchCount > 0;
    }

    //Colider
    private void OnTriggerEnter2D(Collider2D collider)
    {
        animator.SetBool("isDead", true);
        GameAssets.getInstance().playDeath();
        Debug.Log("Dead");
        Pause();
        dead = true;
    }
}