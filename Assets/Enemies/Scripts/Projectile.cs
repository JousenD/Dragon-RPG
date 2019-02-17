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

    private void OnTriggerEnter(Collider collider)
    {
        Component damageableComponent = collider.gameObject.GetComponent(typeof(Idamageable));
        print("damageableComponent= " + damageableComponent);
        if (damageableComponent)
        {
            (damageableComponent as Idamageable).TakeDamage(damageCaused);
        }
    }
}
