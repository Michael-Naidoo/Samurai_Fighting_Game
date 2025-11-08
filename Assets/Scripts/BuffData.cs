// NEW SCRIPT: BuffData.cs (Unity Asset)
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "TUM/Buff Data")]
public class BuffData : ScriptableObject
{
    public string buffName;
    [TextArea(3, 5)]
    public string description;
    
    // The value to apply (e.g., 0.15 for 15% increase)
    public float effectValue;

    public enum BuffEffectType { Strength, Speed, Jump, StaminaCostReduction }
    public BuffEffectType effectType;
}