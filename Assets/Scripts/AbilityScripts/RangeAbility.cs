using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class RangeAbility : Ability
    {
        private Tower m_Tower;

        protected override void Use()
        {
            if (TDPlayer.Instance.Mana >= m_Mana)
            {
                TDPlayer.Instance.ChangeMana(-m_Mana);
                IncreaseRange();
            }
        }

        private void IncreaseRange()
        {
            int level = Upgrade.GetUpgradeLevel(m_Tower.m_RangeUpgrade);
            m_Tower.m_Radius = m_Tower.m_RangeUpgrade.FloatValues[level];
        }

        protected override void AddUpgrade(int bonus)
        {
            m_Mana += bonus;
        }

        protected override void UpdateText(Text manaText)
        {
            manaText.text = m_Mana.ToString();
        }

        protected override void CheckResources(Button button)
        {
            button.interactable = TDPlayer.Instance.Mana >= m_Mana;
        }
    }
}


    
    
