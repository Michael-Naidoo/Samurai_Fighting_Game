using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectActivation : MonoBehaviour
{
    public static CharacterSelectActivation Instance { get; private set; }

    [SerializeField] private GameObject[] p1Indicators;
    [SerializeField] private GameObject[] p2Indicators;
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

    public void ChangeP1Indicator(int character)
    {
        foreach (var t in p1Indicators)
        {
            t.SetActive(false);
        }
        p1Indicators[character - 1].SetActive(true);
    }
    
    public void ChangeP2Indicator(int character)
    {
        foreach (var t in p2Indicators)
        {
            t.SetActive(false);
        }
        p2Indicators[character - 1].SetActive(true);
    }
}
