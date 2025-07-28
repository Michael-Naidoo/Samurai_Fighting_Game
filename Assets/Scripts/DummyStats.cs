using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class DummyStats : MonoBehaviour
    {
        public float HP;
        public Slider HPDislay;
        [SerializeField] private bool lowParry;
        [SerializeField] private bool highParry;
        private float lowParryTimer = 0.2f;
        private float highParryTimer = 0.2f;
        [SerializeField] private float maxParryTimer = 0.2f;
        public GameManager gameManager;

        public enum AttackType
        {
            Low,
            High,
            General
        }

        public AttackType attackType;

        private void Awake()
        {
            
        }

        private void Start()
        {
            gameManager = GameManager.instance;
            if (gameObject.CompareTag("Player1"))
            {
                HPDislay = GameObject.FindWithTag("Player1Health").GetComponent<Slider>();
            }
            else if (gameObject.CompareTag("Player2"))
            {
                HPDislay = GameObject.FindWithTag("Player2Health").GetComponent<Slider>();
            }
        }

        public void DecrementHP(float damage, AttackType attackType)
        {
            switch (attackType)
            {
                case AttackType.Low:
                    if (!lowParry)
                    {
                        HP -= damage;
                    }
                    break;
                case AttackType.High:
                    if (!highParry)
                    {
                        HP -= damage;
                    }
                    break;
                case AttackType.General:
                    if (!lowParry || !highParry)
                    {
                        HP -= damage;
                    }
                    break;
                return;
            }
        }

        private void Update()
        {
            if (HP <= 0)
            {
                Die();
            }

            if (lowParryTimer > 0)
            {
                lowParryTimer -= Time.deltaTime;
            }
            else
            {
                lowParry = false;
            }
            if (highParryTimer > 0)
            {
                highParryTimer -= Time.deltaTime;
            }
            else
            {
                highParry = false;
            }

            HPDislay.value = HP / 100;
        }

        public void LowParry()
        {
            lowParry = true;
            lowParryTimer = maxParryTimer;
        }
        public void HighParry()
        {
            highParry = true;
            highParryTimer = maxParryTimer;
        }

        public void Die()
        {
            if (gameObject.CompareTag("Player1"))
            {
                gameManager.PlayerDied(0);
            }
            else if (gameObject.CompareTag("Player2"))
            {
                gameManager.PlayerDied(1);
            }
        }
    }
}