using UnityEngine;
using UnityEngine.UI;
namespace TowerDefense
{
    public class GoldAbility : Ability
    {
        [SerializeField] private int m_GoldAmount;

        protected override void Use()
        {
            TDPlayer.Instance.ChangeMana(-m_Mana);
            TDPlayer.Instance.ChangeGold(m_GoldAmount);
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





