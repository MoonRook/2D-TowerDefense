using SpaceShooter;
using UnityEngine;
using System;

namespace TowerDefense
{
    public class TDPlayer : Player
    {
        [SerializeField] private int m_Gold;
        [SerializeField] private int m_Mana;
        [SerializeField] private Tower m_TowerPrefab;
        [SerializeField] private UpgradeAsset healthUpgrade;
        public static new TDPlayer Instance
        {
            get
            {
                return Player.Instance as TDPlayer;
            }
        }

        private float m_MaxMana;

        public int Gold => m_Gold;
        public float Mana => m_Mana;
        public float MaxMana => m_MaxMana;

        private void Start()
        {
            m_MaxMana = m_Mana;

            var levelHealth = Upgrade.GetUpgradeLevel(healthUpgrade);
            TakeDamage(-levelHealth);
        }
       
        public event Action<int> OnGoldUpdate;
        public void GoldUpdateSubscribe(Action<int> act)
        {
            OnGoldUpdate += act;
            act?.Invoke(Instance.m_Gold);
        }
        public event Action<int> OnLifeUpdate;
        

        public void LifeUpdateSubscribe(Action<int> act)
        {
            OnLifeUpdate += act;
            act?.Invoke(Instance.NumLives);

        }
        
        public event Action<int> OnManaUpdate;
        public void ManaUpdateSubscribe(Action<int> act)
        {
            OnManaUpdate += act;
            act?.Invoke(Instance.m_Mana);

        }
        public void ChangeGold(int change)
        {
            m_Gold += change;
            OnGoldUpdate?.Invoke(m_Gold);
        }
        public void ReduceLife(int change)
        {
            TakeDamage(change);
            OnLifeUpdate?.Invoke(NumLives);
        }

        public void ChangeMana(int change)
        {
            m_Mana += change;
            OnManaUpdate?.Invoke(m_Mana);
        }

        public void TryBuild(TowerAsset towerAsset, Transform buildSite)
        {
            if (towerAsset.goldCost > m_Gold)
            {
                return;
            }
            
            ChangeGold(-towerAsset.goldCost);
            var tower = Instantiate(m_TowerPrefab, buildSite.position, Quaternion.identity);
            tower.Use(towerAsset);
            Destroy(buildSite.gameObject);
        }
    }
}
