using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    [CreateAssetMenu]
    public sealed class UpgradeAsset : ScriptableObject 
    {
        public Sprite sprite;
        public int[] costBuyLevel = { 3 };
        public int[] IntValues = { 3 };
        public float[] FloatValues = { 3 };
    }
}

