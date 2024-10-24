using System;
using UnityEngine;
using UnityEngine.UI;


namespace TowerDefense
{
    public class Ability : MonoBehaviour
    {
        protected int m_Mana { get; set; }
        protected virtual void Use()
        {

        }
        
        protected virtual void AddUpgrade(int bonus)
        {
            m_Mana += bonus;
        }
        protected virtual void UpdateText(Text manaText)
        {
            manaText.text = m_Mana.ToString();
        }
        protected virtual void CheckResources(Button button)
        {
            button.interactable = TDPlayer.Instance.Mana >= m_Mana;
        }

        [SerializeField] private Ability[] m_Abilities;

        [SerializeField] private Button[] m_AbilityButtons;

        [SerializeField] private UpgradeAsset[] m_UpgradeAssets;

        [SerializeField] private GameObject[] m_AbilityGameObjects;
        
        [SerializeField] private Text m_FireManaText, m_SlowManaText, m_GoldManaText, m_RangeManaText;

        private Text[] m_AbilityManaTexts;
       

        private void Start()
        {
            m_AbilityManaTexts = new Text[m_Abilities.Length];
            for (int i = 0; i < m_Abilities.Length; i++)
            {
                int level = Upgrade.GetUpgradeLevel(m_UpgradeAssets[i]);
                m_Abilities[i].AddUpgrade(level);
                m_AbilityGameObjects[i].SetActive(level > 0);
                m_AbilityManaTexts[i] = GetManaText(i);
                m_Abilities[i].UpdateText(m_AbilityManaTexts[i]);
            }
        }

        private void Update()
        {
            for (int i = 0; i < m_Abilities.Length; i++)
            {
                m_Abilities[i].CheckResources(m_AbilityButtons[i]);
            }
        }
      
        public void UseAbility(AbilityTypeEnum type)
        {
            m_Abilities[(int)type].Use();
        }

        private Text GetManaText(int index)
        {
            switch ((AbilityTypeEnum)index)
            {
                case AbilityTypeEnum.Fire:
                    return m_FireManaText;
                case AbilityTypeEnum.Slow:
                    return m_SlowManaText;
                case AbilityTypeEnum.Gold:
                    return m_GoldManaText;
                case AbilityTypeEnum.Range:
                    return m_RangeManaText;
                default:
                    throw new ArgumentException($"Invalid ability index: {index}");
            }
        }
    }
}
