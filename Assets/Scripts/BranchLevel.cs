using System;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    
    [RequireComponent (typeof(Maplevel))]
    public class BranchLevel : MonoBehaviour
    {
        [SerializeField] private Maplevel m_RootLevel;
        [SerializeField] private Text m_PointText;
        [SerializeField] private int m_NeedPoints = 3;

        
        /// <summary>
        /// Активирует ответвленный уровень
        /// Требует наличия очков и выполнения предыдущего уровня
        /// </summary>
        public void TryActivate()
        {
            gameObject.SetActive(m_RootLevel.IsComplete);
            
            if (m_NeedPoints > MapCompletion.Instance.TotalScore)
            {
                m_PointText.text = m_NeedPoints.ToString();
            }

            else
            {
                m_PointText.transform.parent.gameObject.SetActive(false);
                GetComponent<Maplevel>().Initialise();
            }
        }
    }
}

