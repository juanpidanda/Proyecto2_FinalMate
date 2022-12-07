using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourDeMate : MonoBehaviour
{
    public float speed;
    public Transform[] wayPoints;
    private int wpIndex = 0;
    private int wpNum = 0;
    public int BulletVelocity = 0;
    public Transform target;
    public GameObject bulletPrefab;

    public enum State { patrolling, seeking, attacking };
    public State enemyState;

    public float seekRange, attackRange;

    void Start()
    {
        wpNum = wayPoints.Length;
        enemyState = State.patrolling;
        seekRange = .8f * transform.Find("SeekRange").localScale.x;
        attackRange = 1.2f * transform.Find("AttackRange").localScale.x;
    }

    void Update()
    {
        float angle = 60;
        DrawCone(60);
        Vector3 PE = (target.position - transform.position).normalized;
        float dotprod = Vector3.Dot(PE, transform.forward);
        float coseno = Mathf.Cos(angle * Mathf.Deg2Rad);
        float dist = Vector3.Distance(target.position, transform.position);
        ChangeState();

        // if(coseno < dotprod && dist < 20f)

        if (enemyState == State.patrolling)
            Patroll();

        if (enemyState == State.seeking)
            Seek();

        if (enemyState == State.attacking)
            Attack();

    }

    private void GoTo(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        float dt = Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction.normalized, transform.up);
        transform.Translate(transform.forward * dt * speed, Space.World);

    }

    private void Patroll()
    {
        Vector3 targetPosition = wayPoints[wpIndex].position;
        GoTo(targetPosition);
        float distance = Vector3.Distance(targetPosition, transform.position);
        if (distance < 0.5f)
        {
            wpIndex++;
            if (wpIndex == wpNum) { wpIndex = 0; }
        }
    }

    private void Attack()
    {
        if (GameObject.FindWithTag("EnemyAttack") == null)
        {
            Vector3 direction = target.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction.normalized, transform.up);
            GameObject gO = Instantiate(bulletPrefab, transform.position, transform.rotation);
            gO.GetComponent<Rigidbody>().velocity = BulletVelocity * transform.forward;
            Destroy(gO, 2f);
        }
    }

    /*  private void HeavyAttack()
      {
          if (GameObject.FindWithTag("EnemyAttack") == null)
          {
              Vector3 direction = target.position - transform.position;
              transform.rotation = Quaternion.LookRotation(direction.normalized, transform.up);
              GameObject gO = Instantiate(bulletPrefab, transform.position, transform.rotation);
              gO.GetComponent<Rigidbody>().velocity = BulletVelocity * transform.forward;
              gO.GetComponent.localaScale
              Destroy(gO, 2f);
          }
      }*/
    private void Seek()
    {
        Vector3 targetPosition = target.position;
        GoTo(targetPosition);
    }

    private void ChangeState()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > seekRange)
        {
            enemyState = State.patrolling;
        }

        if (distance < seekRange && distance > attackRange)
        {
            enemyState = State.seeking;
        }

        if (distance < attackRange)
        {
            enemyState = State.attacking;
        }
    }
    void DrawCone(float angle)
    {
        angle *= Mathf.Deg2Rad;
        Vector3 right = transform.forward * Mathf.Cos(angle) + transform.right * Mathf.Sin(angle);
        Vector3 left = transform.forward * Mathf.Cos(angle) + transform.right * Mathf.Sin(angle);
        Debug.DrawRay(transform.position, 50 * right.normalized);
        Debug.DrawRay(transform.position, 50 * left.normalized);
    }
}