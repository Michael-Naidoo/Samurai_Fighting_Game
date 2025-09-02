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

            transform.Translate(CalculateNextPosition(elapsedTime));
            DetectCollision(); }

        Vector2 CalculateNextPosition(float elapsedTime)
        {
            float launchAngleRad = angle * Mathf.Deg2Rad;
    
            float initialVelocityX = InitialVelocityMagnitude * Mathf.Cos(launchAngleRad);
            float initialVelocityY = InitialVelocityMagnitude * Mathf.Sin(launchAngleRad);
    
            float newX = CalculateHorizontalPosition(initialVelocityX, elapsedTime) * Time.deltaTime;
            float newY = CalculateVerticalPosition(initialVelocityY, elapsedTime) * Time.deltaTime;
            
            Debug.Log(newX);
            Debug.Log(newY);

            return new Vector2(newX, newY);
        }
        float CalculateHorizontalPosition(float initialVelocityX, float elapsedTime)
        {
            return initialPosition.x + (initialVelocityX * elapsedTime);
        }

        float CalculateVerticalPosition(float initialVelocityY, float elapsedTime)
        {
            return initialPosition.y + (initialVelocityY * elapsedTime) + (0.5f * gravity * elapsedTime * elapsedTime);
        }

        void DetectCollision()
        {
            enemyPlayer = Physics2D.OverlapBox(transform.position, size, 0, player1Layer | player2Layer);
            if (enemyPlayer != thisPlayer)
            {
                enemyPlayer.GetComponent<DummyStats>().DecrementHP(damage, DummyStats.AttackType.High);
            }
        }
    }
}