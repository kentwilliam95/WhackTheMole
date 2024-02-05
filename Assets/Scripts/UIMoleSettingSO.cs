using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Mole Setting",fileName = "Setting")]
public class UIMoleSettingSO : ScriptableObject
{
    public enum Preset
    {
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
    }
    
    [System.Serializable]
    public struct Range
    {
        public float min;
        public float max;

        public float Get()
        {
            return Random.Range(min, max);   
        }
    }
    
    public Preset _preset;
    public float _gameDuration = 25f;
    public float _successHitPoint = 100;
    
    [Range(0, 100)]
    public float _successHitAccuracy = 60f;
    [Space]
    
    public Range _showUpDuration;
    public Range _delayAfterShowUp;
    public Range _delayBeforeShowUp;
    
}
