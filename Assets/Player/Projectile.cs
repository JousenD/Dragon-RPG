using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] float damageCaused = 10f;

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
