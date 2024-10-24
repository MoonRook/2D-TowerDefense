using UnityEngine;
using System.Collections.Generic;

namespace TowerDefense
{
    public class BuyControl : MonoBehaviour
    {
        [SerializeField] private TowerBuyControll m_TowerBuyPrefab;
        private List <TowerBuyControll> m_ActiveControll;
        private RectTransform m_RectTransform;

        #region Unity events
        private void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
            BuildSite.OnClicEvent += MoveToBuildSite;
            gameObject.SetActive(false);
        }
        
        private void OnDestroy()
        {
            BuildSite.OnClicEvent -= MoveToBuildSite;
        }
        #endregion
        private void MoveToBuildSite(BuildSite buildSite) 
        {
            if (buildSite)
            {
                var position = Camera.main.WorldToScreenPoint(buildSite.transform.root.position);
                m_RectTransform.anchoredPosition = position;
                m_ActiveControll = new List<TowerBuyControll>();
                
                foreach (var asset in buildSite.BuildableTowers)
                {
                    if (asset.IsAvailable())
                    {
                        var newControll = Instantiate(m_TowerBuyPrefab, transform);
                        m_ActiveControll.Add(newControll);
                        newControll.SetTowerAsset(asset);
                    }
                }

                if (m_ActiveControll.Count > 0)
                {
                    gameObject.SetActive(true);
                    var angle = 360 / m_ActiveControll.Count;
                    for (int i = 0; i < m_ActiveControll.Count; i++)
                    {
                        var offset = Quaternion.AngleAxis(angle * i, Vector3.forward) * (Vector3.left * 140);
                        m_ActiveControll[i].transform.position += offset;
                    }
                    
                    foreach (var tbc in GetComponentsInChildren<TowerBuyControll>())
                    {
                        tbc.SetBuildSite(buildSite.transform.root);
                    }
                }
            }
            
            else
            {
                if (m_ActiveControll != null)
                {
                    foreach (var controll in m_ActiveControll) Destroy(controll.gameObject);
                    m_ActiveControll.Clear();
                }
               
                gameObject.SetActive(false);
            }
        }
    }
}
