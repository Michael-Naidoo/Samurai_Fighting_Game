using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ArrowScript : MonoBehaviour
    {
        public float initialTime;
        public float currentTime;
        public float elapsedTime;
        public float gravity;
        public float InitialVelocityMagnitude;
        public float angle;
        public float damage;
        public float multiplyer;
        public float speed;
        
        public Collider2D thisPlayer;
        private Collider2D enemyPlayer;
        
        public Vector2 initialPosition;
        [SerializeField] private Vector2 size;
        [SerializeField] private LayerMask player1Layer;
        [SerializeField] private LayerMask player2Layer;

        private void Update()
        {
            currentTime = Time.time;
            elapsedTime = currentTime - initialTime;
            
            transform.Translate(new Vector2(angle * speed * Time.deltaTime, InitialVelocityMagnitude * multiplyer + gravity * elapsedTime));
            DetectCollision();
        }

        void DetectCollision()
        {
            enemyPlayer = Physics2D.OverlapBox(transform.position, size, 0, player1Layer | player2Layer);
            if (enemyPlayer && enemyPlayer != thisPlayer)
            {
                enemyPlayer.GetComponent<DummyStats>().DecrementHP(damage, DummyStats.AttackType.High);
                Destroy(gameObject);
            }
        }
    }
}