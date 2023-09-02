using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float hp;
    public float moveSpeed;


    //Targeting variables
    public float attackRange;
    public float visionDistance;
    private bool targetNearby;
    private Transform target;
    public LayerMask playerLayer;

    //Attack Variables
    public float cooldown;
    private float currentCooldown;
    public float attackCooldown;
    public int damage;
    public float attackLength;
    public float attackSpeed;
    public GameObject firingPosition;
    public GameObject Aimer;
    public GameObject bullet;


    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Move();
        CheckforPlayer();
        AttackCooldown();
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
        if(targetNearby)
        {
            // transform.LookAt(target);

            if(Vector2.Distance(transform.position, target.position) > attackRange)
            {
                transform.position=Vector2.MoveTowards(transform.position, target.position, moveSpeed*Time.deltaTime);
            }
            else
            {
                Attack();
            }
        }
        else
        {

        }
    }

    private void Attack()
    {
        if(currentCooldown>=cooldown)
        {
            GameObject projectile = Instantiate(bullet, firingPosition.transform.position, Aimer.transform.rotation);
            projectile.GetComponent<EnemyMeleeProjectileScript>().SetStats(damage, attackLength, attackSpeed);
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
}
