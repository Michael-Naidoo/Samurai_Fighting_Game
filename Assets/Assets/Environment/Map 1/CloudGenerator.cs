using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject[] clouds;

    [SerializeField]
    float spawnInterval;

    [SerializeField]
    GameObject endPoint;

    Vector3 startPoint;


    void Start()
    {
        startPoint = transform.position;

        Invoke("AttemptSpawn", spawnInterval);
    }

    void SpawnCloud()
    {
        int randomIndex = UnityEngine.Random.Range(0, clouds.Length);
        GameObject cloud = Instantiate(clouds[randomIndex]);

        cloud.transform.position = startPoint;
        float speed = UnityEngine.Random.Range(0.5f, 1.5f);
        cloud.GetComponent<CloudScript>().StartFloating(speed, endPoint.transform.position.x);
         
    }

    void AttemptSpawn()
    {

        SpawnCloud();

        Invoke("AttemptSpawn", spawnInterval);
    }

    
}
