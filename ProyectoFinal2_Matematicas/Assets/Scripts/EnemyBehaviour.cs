using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyStates
{
    SETTING,
    READY,
    IDLE,
    PATROL,
    FOLLOW,
    ATTACK,
    DEATH
}
public class EnemyBehaviour : MonoBehaviour
{
    public EnemyStates enemyState;

    [Header("Enemy Variables")]
    public EnemyStats enemyStats;
    public Transform bulletSpawner;
    public GameObject bulletPrefab;
    public float bulletSpeed;

    [Header("Player Variables")]
    public GameObject player;
    private float distanceFromPlayer;

    [Header("Patrolling Points")]
    public bool enemyCanPatrol;
    public GameObject[] waypoints;
    private int nextWaypoint;

    [Header("Nav Mesh")]
    private NavMeshController navMeshController;

    private void Awake()
    {
        SetNewEnemyState(EnemyStates.SETTING);
    }

    void SetEnemy()
    {
        navMeshController = GetComponent<NavMeshController>();
        navMeshController.SetAgentSpeed(enemyStats.enemySpeed);

        player = GameObject.FindGameObjectWithTag("Player");
        if (enemyCanPatrol)
        {
            waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        }
        SetNewEnemyState(EnemyStates.READY);
    }

    void SetNewEnemyState(EnemyStates stateToSet)
    {
        EnemyStateEndPhase();
        enemyState = stateToSet;
        EnemyStateStartPhase();

    }

    void EnemyStateStartPhase()
    {
        switch (enemyState)
        {
            case EnemyStates.SETTING:
                SetEnemy();
                break;
            case EnemyStates.READY:
                gameObject.SetActive(true);
                SetNewEnemyState(EnemyStates.IDLE);
                break;
            case EnemyStates.IDLE:
                break;
            case EnemyStates.PATROL:
                SetRandomWaypoint();
                break;
            case EnemyStates.FOLLOW:
                break;
            case EnemyStates.ATTACK:
                break;
            case EnemyStates.DEATH:
                break;
        }
    }

    void EnemyStateEndPhase()
    {
        switch (enemyState)
        {
            case EnemyStates.SETTING:
                break;
            case EnemyStates.READY:
                break;
            case EnemyStates.IDLE:
                break;
            case EnemyStates.PATROL:
                navMeshController.StopAgent();
                break;
            case EnemyStates.FOLLOW:
                navMeshController.StopAgent();
                break;
            case EnemyStates.ATTACK:
                break;
            case EnemyStates.DEATH:
                navMeshController.StopAgent();
                break;
        }
    }


    private void Update()
    {
        if(player != null)
        {
            distanceFromPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position);
        }

        EnemyStateMachineManager();
    }

    void EnemyStateMachineManager()
    {
        switch (enemyState)
        {
            case EnemyStates.IDLE:
                break;
            case EnemyStates.PATROL:
                if (navMeshController.HasArrived())
                {
                    SetRandomWaypoint();
                }
                break;
            case EnemyStates.FOLLOW:
                FollowPlayer();
                break;
            case EnemyStates.ATTACK:
                EnemyAttack();
                break;
        }

        CheckStateConditions();
    }

    void SetRandomWaypoint()
    {
        nextWaypoint = Random.Range(0, waypoints.Length);
        navMeshController.SetNextAgentObjective(waypoints[nextWaypoint]);
    }

    void FollowPlayer()
    {
        navMeshController.SetNextAgentObjective(player);
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }

    void CheckStateConditions()
    {
        switch (enemyState)
        {
            case EnemyStates.IDLE:
                if (enemyCanPatrol)
                {
                    SetNewEnemyState(EnemyStates.PATROL);
                }
                CheckIfPlayerIsNearVisionRange();
                break;
            case EnemyStates.PATROL:
                CheckIfPlayerIsNearVisionRange();
                break;
            case EnemyStates.FOLLOW:
                if (PlayerGetAway())
                {
                    SetNewEnemyState(EnemyStates.IDLE);
                }
                if (PlayerAtRange())
                {
                    SetNewEnemyState(EnemyStates.ATTACK);
                }
                break;
            case EnemyStates.ATTACK:
                if (!PlayerAtRange())
                {
                    SetNewEnemyState(EnemyStates.FOLLOW);
                }
                break;
        }

        if(enemyStats.enemyHitPoints <= 0)
        {
            SetNewEnemyState(EnemyStates.DEATH);
        }
    }

    void CheckIfPlayerIsNearVisionRange()
    {
        if(distanceFromPlayer <= enemyStats.enemyVisionRange)
        {
            SetNewEnemyState(EnemyStates.FOLLOW);
        }
    }

    void EnemyAttack()
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        if (enemyStats.attackCooldown <= 0)
        {
            enemyStats.attackCooldown = enemyStats.attackPerSecond;
            ShotBullet();
        }
        enemyStats.attackCooldown -= Time.deltaTime;
    }

    void ShotBullet()
    {
        Vector3 shotDirection = player.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(shotDirection.normalized, transform.up);
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawner.position, transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpeed * transform.forward;
        Debug.Log("Bullet Shoted");
    }

    bool PlayerAtRange()
    {
        if(distanceFromPlayer <= enemyStats.enemyAttackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool PlayerGetAway()
    {
        if(distanceFromPlayer > enemyStats.enemyVisionRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
[System.Serializable]
public class EnemyStats
{
    public int enemyHitPoints;
    public float enemySpeed;
    public float enemyVisionRange;
    public float enemyAttackRange;
    public float attackPerSecond;
    [HideInInspector]
    public float attackCooldown;
}
