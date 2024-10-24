using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// �������� �������. ������� ����� �������� ��� ������ ����������� ��� ��������.
    /// ������� �� ������� ������ ������ SpaceShip ��� ��������� �������� � �������.
    /// </summary>
    public class Turret : MonoBehaviour
    {
        /// <summary>
        /// ��� ������, ��������� ��� ���������.
        /// </summary>
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        /// <summary>
        /// ������� ������� � ������.
        /// </summary>
        [SerializeField] private TurretProperties m_TurretProperties;

        /// <summary>
        /// ������ ���������� ��������.
        /// </summary>
        private float m_RefireTimer;

        /// <summary>
        /// �������� �����? 
        /// </summary>
        public bool CanFire => m_RefireTimer <= 0;

        /// <summary>
        /// ������������ ������ �� ������������ ���.
        /// </summary>
        private SpaceShip m_Ship;

        #region Unity events

        private void Start()
        {
            m_Ship = transform.root.GetComponent<SpaceShip>();
        }

        private void Update()
        {
            if (m_RefireTimer > 0)
            {
                m_RefireTimer -= Time.deltaTime;
            }
            
            else if (Mode == TurretMode.Auto)
            {
                Fire();
            }
        }

        #endregion

        #region Public API

        /// <summary>
        /// ����� �������� �������. 
        /// </summary>
        public void Fire()
        {
            if (m_RefireTimer > 0)
                return;

            if (m_TurretProperties == null)
                return;
            
            if (m_Ship)
            {
                // ������ �������
                if (!m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage))
                    return;

                // ������ �������
                if (!m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage))
                    return;
            }
            
            // ������������ ���������� ������� ��� ��� �������.
            var projectile = Instantiate(m_TurretProperties.ProjectilePrefab.gameObject).GetComponent<Projectile>();
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;

            // ����� ����������� ������ ����������� � ��� ��� ������� ��� ���������� �� ��������� � ������ ����
            projectile.SetParentShooter(m_Ship);

            m_RefireTimer = m_TurretProperties.RateOfFire;

            {
                // SFX �� �������
            }
        }

        /// <summary>
        /// ��������� ������� ������. ����� ������������ � ���������� ��� ���������.
        /// </summary>
        /// <param name="props"></param>
        public void AssignLoadout(TurretProperties props)
        {
            if (m_Mode != props.Mode)
                return;

            m_TurretProperties = props;
            m_RefireTimer = 0;
        }

        #endregion
    }
}
