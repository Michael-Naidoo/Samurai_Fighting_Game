using Unity.Mathematics;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;
    [SerializeField] private GameObject playerPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player1 = Instantiate(playerPrefab, transform.position, quaternion.identity, gameObject.transform);
        player2 = Instantiate(playerPrefab, transform.position, quaternion.identity, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
