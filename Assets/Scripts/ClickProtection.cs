using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;
using System;
using UnityEngine.EventSystems;


namespace TowerDefense
{
    public class ClickProtection : SingletonBase<ClickProtection>, IPointerClickHandler
    {
        private Image blocker;
        private FireAbility fireAbility;

        private void Start()
        {
            blocker = GetComponent<Image>();
            fireAbility = FindObjectOfType<FireAbility>();
        }

        private Action<Vector2> m_OnClickAction;
        public void Activate(Action<Vector2> mouseAction)
        {
            blocker.enabled = true;
            fireAbility.isClickProtectionActive = true;
            m_OnClickAction = mouseAction;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            blocker.enabled = false;
            fireAbility.isClickProtectionActive = false;
            m_OnClickAction(eventData.pressPosition);
            m_OnClickAction = null;
        }
    }
}


