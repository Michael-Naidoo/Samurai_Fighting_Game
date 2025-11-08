using UnityEngine;

public class CloudScript : MonoBehaviour
{

    public float _speed = 0.5f;
    public float _endPosX;


    void Start()
    {
        
    }

    public void StartFloating(float speed, float endPosX)
    {
        _speed = speed;
        _endPosX = endPosX;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * (Time.deltaTime * _speed));

        if (transform.position.x > _endPosX)
        {
            Destroy(gameObject);
        }
    }
}
