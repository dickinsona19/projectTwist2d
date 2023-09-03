using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float hp;
    public float moveSpeed;


    //Targeting variables
    public float attackRange;
    public float visionDistance;
    public float closingDistance;
    private bool targetNearby;
    private Transform target;
    public LayerMask playerLayer;

    //Attack Variables
    public float cooldown;
    private float currentCooldown;
    public float attackCooldown;
    public int damage;
    private bool Attacking;
    public float attackLength;
    private float currentRunTime;
    public float attackSpeed;
    public GameObject firingPosition;
    public GameObject Aimer;


    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Move();
        CheckforPlayer();
        AttackCooldown();
        // CheckforDay();
    }

    private void Hurt(int takenDamage)
    {
        hp-=takenDamage;
    }

    // Update is called once per frame
    void Update()
    {
        target=GameObject.FindWithTag("Player").transform;

        if(hp<=0)
        {
            Destroy(gameObject);
        }

    }

    //Move Script
    private void Move()
    {
        if(!Attacking)
        {
            if(targetNearby)
            {
                if(Vector2.Distance(transform.position, target.position) > closingDistance)
                {
                    transform.position=Vector2.MoveTowards(transform.position, target.position, moveSpeed*Time.deltaTime);
                }

                if(Vector2.Distance(transform.position, target.position) < attackRange)
                {
                    Attack();
                }
            }
        }
        else
        {
            if(currentRunTime<attackLength)
            {
                transform.position=Vector2.MoveTowards(transform.position, firingPosition.transform.position, attackSpeed*Time.fixedDeltaTime);
                currentRunTime+=Time.fixedDeltaTime;
            }
            else
            {
                Aimer.SendMessage("UnFreeze");
                Attacking=false;
                currentRunTime = 0;
            }
        }
        
        
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(Attacking)
        {
            if(other.gameObject.tag=="Enemy")
            {
                other.gameObject.SendMessage("Hurt", damage);
            }

            Aimer.SendMessage("UnFreeze");
            Attacking=false;
            currentRunTime = 0;
        }
        
    }

    private void Attack()
    {
        if(currentCooldown>=cooldown)
        {
            Aimer.SendMessage("Freeze");
            Attacking=true;
            currentCooldown=0;
        }
    }

    private void AttackCooldown()
    {
        if(currentCooldown<cooldown)
            currentCooldown+=Time.fixedDeltaTime;
    }

    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
    }

    private void CheckforPlayer()
    {
        targetNearby=Physics2D.OverlapCircle(transform.position, visionDistance, playerLayer);
    }

    // private void CheckforDay()
    // {
    //     if(MoonTimer.Instance.isday)
    //     {
    //         Destroy(gameObject);
    //     }
    // }
}
