using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] float projectileSpeed; //Note oher classes can set
    [SerializeField] GameObject shooter; //So can be inspected when pause

    const float DESTROY_DELAY = 0.02f;
    float damageCaused;

    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;
    }

    public void SetDamage(float damage)
    {
        damageCaused = damage;
    }

    public float GetDefaultLaunchSPeed()
    {
        return projectileSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var layerCollidedWith = collision.gameObject.layer;
        if (layerCollidedWith != shooter.layer)
        {
            DamageIfDamageable(collision);
        }   
    }

    private void DamageIfDamageable(Collision collision)
    {
        Component damageableComponent = collision.gameObject.GetComponent(typeof(Idamageable));

        if (damageableComponent)
        {
            (damageableComponent as Idamageable).TakeDamage(damageCaused);
        }
        Destroy(gameObject, DESTROY_DELAY);
    }
}
