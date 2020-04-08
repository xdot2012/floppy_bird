using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float JUMP_AMOUNT = 30f;
    private Rigidbody2D rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TestInput())
        {
            rigidbody2D.gravityScale = -JUMP_AMOUNT;
        }
        else
        {
            rigidbody2D.gravityScale = JUMP_AMOUNT;
        }
        transform.eulerAngles = new Vector3(0, 0, rigidbody2D.velocity.y * .30f);
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
        Debug.Log("Dead");

    }
}