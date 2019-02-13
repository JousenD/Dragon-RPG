using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, Idamageable {

    [SerializeField] float maxHealthPoints = 100;
    [SerializeField] float attackRadius = 4f;

    [SerializeField] float damagePerShot = 9f;
    [SerializeField] float chaseRadius = 6f;
    [SerializeField] float secondBetweenShots = 0.5f;

    [SerializeField] GameObject projectileToUse;
    [SerializeField] GameObject projectileSocket;

    bool isAttacking = false;

    float currentHealthPoints = 100f;
    AICharacterControl aiCharacterControl = null;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= attackRadius && !isAttacking)
        {
            isAttacking = true;
            InvokeRepeating("SpawnProjectile", 0f, secondBetweenShots); //TODO switch to Coroutines
            //SpawnProjectile(); //TODO Slow this down
        }

        if (distanceToPlayer > attackRadius)
        {
            CancelInvoke();
            isAttacking = false;
        }


        if (distanceToPlayer <= chaseRadius)
        {
            aiCharacterControl.SetTarget(player.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }
    }

    void SpawnProjectile()
    {
        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.SetDamage(damagePerShot);

        Vector3 unitVectorToPlayer = (player.transform.position - projectileSocket.transform.position).normalized;
        float projectileSpeed = projectileComponent.projectileSpeed;
        newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;

        print(projectileSpeed);
        print(unitVectorToPlayer);
        print(unitVectorToPlayer * projectileSpeed);
    }

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / (float)maxHealthPoints;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
    }

    private void OnDrawGizmos()
    {
        ////Draw Movement Gizmos
        //Gizmos.color = Color.black;
        //Gizmos.DrawLine(transform.position, clickpoint);
        //Gizmos.DrawSphere(currentDestination, 0.15f);
        //Gizmos.DrawSphere(clickpoint, 0.1f);

        //Draw attack Sphere
        Gizmos.color = new Color(255f, 0f, 0f, 0.7f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        //Draw chase Sphere
        Gizmos.color = new Color(0, 0f, 255f, 0.7f);
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }


}
