using UnityEngine;

public class MapSelectActivation : MonoBehaviour
{
    public static MapSelectActivation Instance { get; private set; }

    [SerializeField] private GameObject[] mapIndicators;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    public void ChangeMapIndicator(int map)
    {
        foreach (var t in mapIndicators)
        {
            t.SetActive(false);
        }
        mapIndicators[map - 1].SetActive(true);
    }
}
