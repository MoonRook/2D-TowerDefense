using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class FireAbility : Ability
    {
        public bool isClickProtectionActive { get; set; }

        [SerializeField] private Image m_TargetCircle;
        [SerializeField] private int m_Damage;
        public float circleSize = 50f;

        public float damageRadius = 1f;
        //private bool isClickProtectionActive = false;

        protected override void Use()
        {
            TDPlayer.Instance.ChangeMana(-m_Mana);
            ClickProtection.Instance.Activate((Vector2 v) =>
            {
                Vector3 position = v;
                position.z = -Camera.main.transform.position.z;
                position = Camera.main.ScreenToWorldPoint(position);

                foreach (var collider in Physics2D.OverlapCircleAll(position, 5))
                {
                    if (collider.transform.parent.TryGetComponent<Enemy>(out var enemy))
                    {
                        enemy.TakeDamage(m_Damage, TDProjectile.DamageType.Archer);
                    }
                }
            });
        }

        protected override void AddUpgrade(int bonus)
        {
            m_Mana += bonus;
            m_Damage *= bonus;
        }

        protected override void UpdateText(Text manaText)
        {
            manaText.text = m_Mana.ToString();
        }

        protected override void CheckResources(Button button)
        {
            button.interactable = TDPlayer.Instance.Mana >= m_Mana;
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                UseAbility(AbilityTypeEnum.Fire);
            }

            if (isClickProtectionActive)
            {
                Vector3 mousePos = Input.mousePosition;
                m_TargetCircle.rectTransform.position = mousePos;
                m_TargetCircle.rectTransform.sizeDelta = new Vector2(circleSize, circleSize);
                m_TargetCircle.gameObject.SetActive(true);
            }
            else
            {
                m_TargetCircle.gameObject.SetActive(false);
            }
        }
    }
}
