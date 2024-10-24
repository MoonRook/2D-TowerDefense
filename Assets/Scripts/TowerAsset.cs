using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    [CreateAssetMenu]
    public class TowerAsset : ScriptableObject
    {
        public int goldCost = 15;
        public Sprite sprite;
        public Color color = Color.white;
        public Sprite GUISprite;
        public TurretProperties turretProperties;
        [SerializeField] private UpgradeAsset requiredUpgrade;
        [SerializeField] private int requiredUpgradeLevel;
        public bool IsAvailable() => !requiredUpgrade || 
            requiredUpgradeLevel <= Upgrade.GetUpgradeLevel(requiredUpgrade);
        public TowerAsset[] m_UpgradeTo;
    }
}

