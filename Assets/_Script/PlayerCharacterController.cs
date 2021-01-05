using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{

    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;

    List<int> wew;

    [SerializeField]
    float speed = 5;

    [SerializeField]
    Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _Move(); 
    }
    private void _Move()
    {
        Vector2 movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movementVector *= speed;
        // transform.Translate(movementVector);
        rigidBody.velocity = movementVector;

        if (Input.GetAxis("Horizontal") >= 0.1f)
        {
            //walk right
            m_animator.SetInteger("AnimParam",2);
        }
        else if(Input.GetAxis("Horizontal") <= -0.1f)
        {
            //walk left
            m_animator.SetInteger("AnimParam",3);
        }
        else if(Input.GetAxis("Vertical") >= 0.1f)
        {
            //walk up
            m_animator.SetInteger("AnimParam", 4);
        }
        else if(Input.GetAxis("Vertical") <= -0.1f)
        {
            //walk down
            m_animator.SetInteger("AnimParam", 1);
        }
        else
        {
            //idle
            m_animator.SetInteger("AnimParam", 0);
        }
    }
}