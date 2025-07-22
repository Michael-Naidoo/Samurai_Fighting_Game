using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class DummyStats : MonoBehaviour
    {
        public float HP;
        public Slider HPDislay;

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