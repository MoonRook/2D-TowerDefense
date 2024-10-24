using System;
using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;
using System.Collections;

namespace TowerDefense
{
    public class Abilitys : SingletonBase<Abilitys>
    {
        [SerializeField] private FireAbility m_FireAbility;
        [SerializeField] private TimeAbility m_TimeAbility;
        [SerializeField] private GoldAbility m_GoldAbility;
        [SerializeField] private RangeAbility m_RangeAbility;

        [SerializeField] 
        private Button m_FireButton, m_TimeButton, m_GoldButton, m_RangeButton;

        [SerializeField] 
        private Image m_TargetCircle;

        [SerializeField] 
        private UpgradeAsset m_FireUpgrade, m_SlowUpgrade, m_GoldUpgrade, m_RangeUpgrade;
        
        [SerializeField] 
        private GameObject m_FireGameObject, m_SlowGameObject, m_GoldGameObject, m_RangeGameObject;

        
        private Text m_FireText, m_SlowText, m_GoldText, m_RangeText;

        [SerializeField]
        private Text m_FireManaText, m_SlowManaText, m_GoldManaText, m_RangeManaText;

        public float circleSize = 50f;

        public float damageRadius = 1f;

        private void Start()
        {
            var levelFireUpgrade = Upgrade.GetUpgradeLevel(m_FireUpgrade);
            var levelSlowUpgrade = Upgrade.GetUpgradeLevel(m_SlowUpgrade);
            var levelGoldUpgrade = Upgrade.GetUpgradeLevel(m_GoldUpgrade);
            var levelRangeUpgrade = Upgrade.GetUpgradeLevel(m_RangeUpgrade);

            if (levelFireUpgrade < 1)
                m_FireGameObject.SetActive(false);
            if (levelSlowUpgrade < 1)
                m_SlowGameObject.SetActive(false);
            if (levelGoldUpgrade < 1)
                m_GoldGameObject.SetActive(false);
            if (levelRangeUpgrade < 1)
                m_RangeGameObject.SetActive(false);

            m_FireAbility.AddUpgrade(levelFireUpgrade);
            m_TimeAbility.AddUpgrade(levelSlowUpgrade);
            m_GoldAbility.AddUpgrade(levelGoldUpgrade);
            m_RangeAbility.AddUpgrade(levelRangeUpgrade);

            m_FireAbility.UpdateText();
            m_TimeAbility.UpdateText();
            m_GoldAbility.UpdateText();
            m_RangeAbility.UpdateText();
        }
        private void Update()
        {
            m_FireAbility.CheckResources();
            m_TimeAbility.CheckResources();
            m_GoldAbility.CheckResources();
            m_RangeAbility.CheckResources();
            
        }

       /* private void FixedUpdate()
        {
            // Проверяем клик мыши
            if (Input.GetMouseButtonDown(0))
            {
                UseFireAbility();
            }
            // Обновляем положение круга в соответствии с положением мыши
            Vector3 mousePos = Input.mousePosition;
            m_TargetCircle.rectTransform.position = mousePos;

            // Обновляем размер круга
            m_TargetCircle.rectTransform.sizeDelta = new Vector2(circleSize, circleSize);
        }*/
        
        [Serializable]
        public class FireAbility
        {
            [SerializeField] private int m_Mana;
            [SerializeField] private int m_Damage;
            
            public void Use()
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

            public void AddUpgrade(int bonus)
            {
                m_Mana += bonus;
                m_Damage *= bonus;
            }

            public void UpdateText()
            {
                Instance.m_FireManaText.text = m_Mana.ToString();
            }

            public void CheckResources()
            {
                if (TDPlayer.Instance.Mana < m_Mana)
                    Instance.FireButtonOff();
                else
                    Instance.FireButtonOn();
            }
        }

        [Serializable]
        public class TimeAbility
        {
            [SerializeField] private int m_Mana;
            [SerializeField] private float m_Cooldown;
            [SerializeField] private float m_Duration;

            private bool IsCoroutine = false;

            public void Use()
            {
                TDPlayer.Instance.ChangeMana(-m_Mana);
                foreach (var ship in FindObjectsOfType<SpaceShip>())
                    ship.HalfMaxLinearVelocity();

                EnemyWavesManager.OnEnemySpawn += Slow;
                Instance.StartCoroutine(Restore());
                Instance.StartCoroutine(TimeAbilityButton());
            }

            private void Slow(Enemy ship)
            {
                ship.GetComponent<SpaceShip>().HalfMaxLinearVelocity();
            }

            IEnumerator Restore()
            {
                yield return new WaitForSeconds(m_Duration);
                foreach (var ship in FindObjectsOfType<SpaceShip>())
                    ship.RestoreMaxLinearVelocity();

                EnemyWavesManager.OnEnemySpawn -= Slow;
            }

            IEnumerator TimeAbilityButton()
            {
                Instance.TimeButtonOff();
                IsCoroutine = true;
                yield return new WaitForSeconds(m_Cooldown);
                Instance.TimeButtonOn();
                IsCoroutine = false;
            }

            public void AddUpgrade(int bonus)
            {
                m_Mana += bonus;
                m_Duration *= bonus;
                m_Cooldown *= bonus;
            }

            public void UpdateText()
            {
                Instance.m_SlowManaText.text = m_Mana.ToString();
            }

            public void CheckResources()
            {
                if (TDPlayer.Instance.Mana < m_Mana || IsCoroutine)
                    Instance.TimeButtonOff();
                else
                    Instance.TimeButtonOn();
            }
        }
        [Serializable]
        public class GoldAbility
        {
            [SerializeField] private int m_Mana;
            [SerializeField] private int m_GoldAmount;
            public void Use()
            {
                TDPlayer.Instance.ChangeMana(-m_Mana);
                TDPlayer.Instance.ChangeGold(m_GoldAmount);
            }

            public void AddUpgrade(int bonus)
            {
                m_Mana += bonus;
            }
            public void UpdateText()
            {
                Instance.m_GoldManaText.text = m_Mana.ToString();
            }
            public void CheckResources()
            {
                if (TDPlayer.Instance.Mana < m_Mana)
                    Instance.GoldButtonOff();
                else
                    Instance.GoldButtonOn();
            }
        }
        [Serializable]
        public class RangeAbility
        {
            [SerializeField] private int m_Mana;
            private Tower m_Tower;
            public void Use(Tower tower)
            {
                m_Tower = tower;
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

            public void AddUpgrade(int bonus)
            {
                m_Mana += bonus;
            }
            public void UpdateText()
            {
                Instance.m_RangeManaText.text = m_Mana.ToString();
            }
            public void CheckResources()
            {
                if (TDPlayer.Instance.Mana < m_Mana)
                    Instance.RangeButtonOff();
                else
                    Instance.RangeButtonOn();
            }
        }

        public void UseFireAbility() => m_FireAbility.Use();
        public void UseTimeAbility() => m_TimeAbility.Use();
        public void UseGoldAbility() => m_GoldAbility.Use();
        public void UseRangeAbility(Tower tower) => m_RangeAbility.Use(tower);

        public void ChangeText(Text text)
        {
            m_FireText.text = text.ToString();
        }

        public void FireButtonOn() { m_FireButton.interactable = true; }
        public void FireButtonOff() { m_FireButton.interactable = false; }
        
        public void GoldButtonOn() { m_GoldButton.interactable = true; }
        public void GoldButtonOff() { m_GoldButton.interactable = false; }

        public void TimeButtonOn() { m_TimeButton.interactable = true; }
        public void TimeButtonOff() { m_TimeButton.interactable = false; }
        
        public void RangeButtonOn() { m_RangeButton.interactable = true; }
        public void RangeButtonOff() { m_RangeButton.interactable = false; }
    }
}
