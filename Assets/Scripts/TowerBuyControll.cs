using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class TowerBuyControll : MonoBehaviour
    {
        [SerializeField] private TowerAsset m_TowerAsset;
        public void SetTowerAsset(TowerAsset asset) { m_TowerAsset = asset; }
        
        [SerializeField] private Text m_Text;
        [SerializeField] private Button m_Buton;
        [SerializeField] private Transform buildSite;
        public void SetBuildSite(Transform value) 
        {  
            buildSite = value; 
        }

        private void Start()
        {
            TDPlayer.Instance.GoldUpdateSubscribe(GoldStatusCheck);
        
            m_Text.text = m_TowerAsset.goldCost.ToString();
            m_Buton.GetComponent<Image>().sprite = m_TowerAsset.GUISprite;
        }
        private void OnDestroy()
        {
            TDPlayer.Instance.OnGoldUpdate -= GoldStatusCheck;
        }

        private void GoldStatusCheck(int golg)
        {
            if (golg >= m_TowerAsset.goldCost != m_Buton.interactable)
            {
                m_Buton.interactable = !m_Buton.interactable;
                m_Text.color = m_Buton.interactable ? Color.white : Color.red;
            }
        }
        
        public void Buy()
        {
            TDPlayer.Instance.TryBuild(m_TowerAsset, buildSite);
            BuildSite.HideControls();
        }
    }
}

