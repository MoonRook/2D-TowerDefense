using SpaceShooter;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class SlowAbility : Ability
    {
        [SerializeField] private float m_Cooldown;
        [SerializeField] private float m_Duration;

        private bool IsCoroutine = false;

        protected override void Use()
        {
            TDPlayer.Instance.ChangeMana(-m_Mana);
            foreach (var ship in FindObjectsOfType<SpaceShip>())
                ship.HalfMaxLinearVelocity();

            EnemyWavesManager.OnEnemySpawn += Slow;
            StartCoroutine(Restore());
            StartCoroutine(TimeAbilityButton());
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
            Abilitys.Instance.TimeButtonOff();
            IsCoroutine = true;
            yield return new WaitForSeconds(m_Cooldown);
            Abilitys.Instance.TimeButtonOn();
            IsCoroutine = false;
        }

        protected override void AddUpgrade(int bonus)
        {
            m_Mana += bonus;
            m_Duration *= bonus;
            m_Cooldown *= bonus;
        }

        protected override void UpdateText(Text manaText)
        {
            manaText.text = m_Mana.ToString();
        }

        protected override void CheckResources(Button button)
        {
            button.interactable = TDPlayer.Instance.Mana >= m_Mana && !IsCoroutine;
        }
    }

   

}


