using System;
using System.Collections;
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
                        StartCoroutine(ChangeColor());
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

        IEnumerator ChangeColor()
{
    // 1. Get the SpriteRenderer component (This is the object we need to modify)
    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
    
    // Check if the component exists before proceeding
    if (spriteRenderer == null)
    {
        Debug.LogError("SpriteRenderer component not found!");
        yield break; 
    }

    // 2. Fade In: Pass the SpriteRenderer, not the Sprite
    // Assuming you want to fade to fully opaque (alpha = 1)
    yield return StartCoroutine(FadeSpriteRenderer(spriteRenderer, 1f, 0.01f));
    
    // 3. Display: Wait for the specified duration
    yield return new WaitForSeconds(0.1f);

    // 4. Fade Out: Pass the SpriteRenderer to fade to fully transparent (alpha = 0)
    yield return StartCoroutine(FadeSpriteRenderer(spriteRenderer, 0f, 0.01f));
}

// ----------------------------------------------------

// Change the parameter type from 'Sprite' to 'SpriteRenderer'
IEnumerator FadeSpriteRenderer(SpriteRenderer renderer, float targetAlpha, float duration)
{
    // Get the alpha from the Renderer's color
    float startAlpha = renderer.color.a; 
    float time = 0;

    while (time < duration)
    {
        time += Time.deltaTime;
        
        // Calculate the interpolation value (normalized time)
        float t = time / duration;
        
        // You can add an ease function here, e.g., t = Mathf.SmoothStep(0f, 1f, t);

        // Lerp the alpha value
        float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);

        // Apply the new color to the SpriteRenderer
        Color currentColor = renderer.color;
        renderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
        
        yield return null;
    }

    // Ensure the final color is exactly the target color to prevent floating-point errors
    Color finalColor = renderer.color;
    renderer.color = new Color(finalColor.r, finalColor.g, finalColor.b, targetAlpha);
}

        private void Update()
        {
            if (HP <= 0)
            {
                Die();
                HP = 100;
            }

            if (lowParryTimer > 0)
            {
                lowParryTimer -= Time.deltaTime;
            }
            else
            {
                lowParry = false;
                highParry = false;
            }
            if (highParryTimer > 0)
            {
                highParryTimer -= Time.deltaTime;
            }
            else
            {
                lowParry = false;
                highParry = false;
            }

            HPDislay.value = HP / 100;
        }

        public void LowParry()
        {
            lowParry = true;
            highParry = true;
            lowParryTimer = maxParryTimer;
            highParryTimer = maxParryTimer;
        }
        public void HighParry()
        {
            highParry = true;
            lowParry = true;
            highParryTimer = maxParryTimer;
            lowParryTimer = maxParryTimer;
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