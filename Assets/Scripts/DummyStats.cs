using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class DummyStats : MonoBehaviour
    {
        public float HP;
        public Slider HPDislay;

        private void Start()
        {
            if (gameObject.CompareTag("Player1"))
            {
                HPDislay = GameObject.FindWithTag("Player1Health").GetComponent<Slider>();
            }
            else if (gameObject.CompareTag("Player2"))
            {
                HPDislay = GameObject.FindWithTag("Player2Health").GetComponent<Slider>();
            }
        }

        public void DecrementHP(float damage)
        {
            HP -= damage;
        }

        private void Update()
        {
            if (HP <= 0)
            {
                HP = 100;
            }

            HPDislay.value = HP / 100;
        }
    }
}