using UnityEngine;
using System;

namespace SpaceShooter
{
    public class Player : SingletonBase<Player>
    {
        [SerializeField] public int m_NumLives;

        public int NumLives { get { return m_NumLives; } }

        public event Action OnPlayerDead;

        [SerializeField] private SpaceShip m_Ship;
        public SpaceShip ActiveShip => m_Ship;

        [SerializeField] private SpaceShip m_PlayerShipPrefab;

        //[SerializeField] private CameraController m_CameraController;
        //[SerializeField] private MovementController m_MovementController;

        private void Start()
        {
            if (m_Ship)
            {
                m_Ship.EventOnDeath.AddListener(OnShipDeath);
            }
        }

        private void OnShipDeath()
        {
            m_NumLives--;

            if (m_NumLives > 0)
                Respawn();
            else
                LevelSequnecesController.Instance.FinishCurrentLevel(false);
        }
        protected void TakeDamage(int m_Damage)
        {
            m_NumLives -= m_Damage;
            if (m_NumLives <= 0)
            {
                m_NumLives = 0;
                OnPlayerDead?.Invoke();
            }
        }

        private void Respawn()
        {
            var newPlayerShip = Instantiate(m_PlayerShipPrefab.gameObject);

            m_Ship = newPlayerShip.GetComponent<SpaceShip>();

            //m_CameraController.SetTarget(m_Ship.transform);
            //m_MovementController.SetTargetShip(m_Ship);

            m_Ship.EventOnDeath.AddListener(OnShipDeath);
        }

        #region Score (current level only)

        public int Score { get; private set; }

        public int NumKills { get; private set; }

        public void AddKill()
        {
            NumKills++;
        }

        public void AddScore(int num)
        {
            Score += num;
        }
        #endregion
    }
}
