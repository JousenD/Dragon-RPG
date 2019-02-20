using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float projectileSpeed; //Note oher classes can set
    float damageCaused;

    public void SetDamage(float damage)
    {
        damageCaused = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Component damageableComponent = collision.gameObject.GetComponent(typeof(Idamageable));
        print("damageableComponent= " + damageableComponent);
        if (damageableComponent)
        {
            (damageableComponent as Idamageable).TakeDamage(damageCaused);
        }
        Destroy(gameObject, 0.02f);
    }
}
