using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, Idamageable {

    [SerializeField] int enemyLayer = 9;
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float damagePerHit = 10f;
    [SerializeField] float minTimeBetweenHits = 0.5f;
    [SerializeField] float maxAttackRange = 2f;


    GameObject currentTarget;
    float currentHealthPoints;
    float lastHitTime = 0f;
    CameraRaycaster cameraRaycaster;

    

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / (float)maxHealthPoints;
        }
    }

    void Start()
    {
        currentHealthPoints = maxHealthPoints;
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
    }

    void OnMouseClick(RaycastHit raycastHit, int layerHit)
    {
        if (layerHit == enemyLayer)
        {
            var enemy = raycastHit.collider.gameObject;

            if ((enemy.transform.position - transform.position).magnitude > maxAttackRange)
            {
                return;
            }
            currentTarget = enemy;
            var enemyComponent = enemy.GetComponent<Enemy>();
            if (Time.time - lastHitTime > minTimeBetweenHits)
            {
                enemyComponent.TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }

        }
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        //if (currentHealthPoints <= 0) { Destroy(gameObject); } //TODO Endgame Condition
    }
	
}
