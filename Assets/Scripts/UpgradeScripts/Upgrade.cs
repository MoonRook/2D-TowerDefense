using System;
using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public class Upgrade : SingletonBase<Upgrade>
    {
        public const string filename = "upgrades.dat";
        [Serializable]
        private class UpgradeSave
        {
            public UpgradeAsset asset;
            public int level = 0;
        }

        [SerializeField] private UpgradeSave[] save;

        private new void Awake()
        {
            base.Awake();
            Saver<UpgradeSave[]>.TryLoad(filename, ref save);
        }

        public static void BuyUpgrade(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.save)
            {
                if (upgrade.asset == asset)
                {
                    upgrade.level += 1;
                    Saver<UpgradeSave[]>.Save(filename, Instance.save);
                }
            }
        }

        public static void UpgradeAssetsInitialize(BuyUpgrade[] assets)
        {
            if (Instance.save.Length < assets.Length)
            {
                Array.Resize(ref Instance.save, assets.Length);
            }

            for (int i = 0; i < assets.Length; i++)
            {
                Instance.save[i].asset = assets[i].Asset;
            }
        }

        public static int GetTotalCost()
        {
            int result = 0;
            foreach (var upgrade in Instance.save)
            {
                for (int i = 0; i < upgrade.level; i++)
                {
                    result += upgrade.asset.costBuyLevel[i];
                }
            }
            return result;
        }

        public static int GetUpgradeLevel(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.save)
            {
                if (upgrade.asset == asset)
                {
                    return upgrade.level; 
                }
            }

            return 0;
        }
    }
}





