using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core; //TODO consider Re-wiring
using RPG.Weapons; //TODO consider Re-wiring

using UnityStandardAssets.Characters.ThirdPerson;

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour, Idamageable
    {

        [SerializeField] float maxHealthPoints = 100;
        [SerializeField] float attackRadius = 4f;

        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] float secondBetweenShots = 0.5f;

        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0f, 1f, 0f);

        bool isAttacking = false;

        float currentHealthPoints;
        AICharacterControl aiCharacterControl = null;
        GameObject player;

        private void Start()
        {
            currentHealthPoints = maxHealthPoints;
            player = GameObject.FindGameObjectWithTag("Player");
            aiCharacterControl = GetComponent<AICharacterControl>();
        }

        private void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                InvokeRepeating("FireProjectile", 0f, secondBetweenShots); //TODO switch to Coroutines
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

        // TODO start separate character firing logic
        void FireProjectile()
        {
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot);
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSPeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
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
            if (currentHealthPoints <= 0) { Destroy(gameObject); }
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
}