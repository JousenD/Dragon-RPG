using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//TODO Consider Re-wiring
using RPG.CameraUI; 
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, Idamageable
    {

        [SerializeField] int enemyLayer = 9;
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float damagePerHit = 10f;
        [SerializeField] float minTimeBetweenHits = 0.5f;
        [SerializeField] float maxAttackRange = 2f;

        [SerializeField] Weapon weaponInUse;



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
            RegisterForMouseClick();
            currentHealthPoints = maxHealthPoints;
            PutWeaponInHand();
        }

        private void PutWeaponInHand()
        {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominantHand.transform);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on Player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHands found on Player, remove all but one");
            return dominantHands[0].gameObject;
        }

        private void RegisterForMouseClick()
        {
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
}