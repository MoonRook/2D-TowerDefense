using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using TowerDefense;

namespace SpaceShooter
{
    [RequireComponent (typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        [SerializeField] private Sprite m_PreviewImage;

        /// <summary>
        /// ����� ��� �������������� ��������� � rigidbody
        /// </summary>
        [Header("Space ship")]
        [SerializeField] private float m_Mass;

        /// <summary>
        /// ����, ��������� ������ 
        /// </summary>
        [SerializeField] private float m_Thrust;

        /// <summary>
        /// ���� ��������� 
        /// </summary>
        [SerializeField] private float m_Mobility;

        /// <summary>
        /// ����������� �������� ��������
        /// </summary>
        [SerializeField] private float m_MaxLinearVelocity;
        private float m_MaxVelocityBackup;
        
        public void HalfMaxLinearVelocity() 
        { 
            m_MaxVelocityBackup = m_MaxLinearVelocity; 
            m_MaxLinearVelocity /= 2;
        }

        public void RestoreMaxLinearVelocity() { m_MaxLinearVelocity = m_MaxVelocityBackup; }

        /// <summary>
        /// ����������� ������������ �������� ��������/���
        /// </summary>
        [SerializeField] private float m_MaxAngularVelocity;

        private Rigidbody2D m_Rigid; // ������ �� ��������� Rigidbody

        public float MaxLinearVelocity => m_MaxLinearVelocity;
        public float MaxAngularVelocity => m_MaxAngularVelocity;
        public Sprite PreviewImage => m_PreviewImage;

        #region Public API
        /// <summary>
        /// ���������� �������� ����� -1.0 �� +1.0
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// ���������� ������������ ����� -1.0 �� +1.0
        /// </summary>
        public float TorqueControl { get; set; }
        #endregion

        #region Unity Event
        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertia = 1;

            // InitOffensive();
        }

        private void FixedUpdate()
        {
            UpdateRigidBody();
            
            // UpdateEnergyRegen();

            UpdateInvincibility(); // ��������� ������������
        }
        #endregion

        /// <summary>
        /// ����� ���������� ��� ��� �������� �������
        /// </summary>
        private void UpdateRigidBody()
        {
            m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);
           
            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        /// <summary>
        /// TODO: ��������� ����� ��������. ������������ ��������
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool DrawEnergy(int count)
        {
            return true;
        }

        /// <summary>
        /// TODO: ��������� ����� ��������. ������������ ��������
        /// </summary>
        /// <param name="count"></param>
        /// <returns>true ���� ������� ���� ���������</returns>
        public bool DrawAmmo(int count)
        {
            return true;
        }
        
        /// <summary>
        /// TODO: ��������� ����� ��������. ������������ AI
        /// </summary>
        /// <param name="mode"></param>
        public void Fire(TurretMode mode)
        {
            return;
        }
        
        /*
        # region Offfensive
        [SerializeField] private Turret[] m_Turrets; // ������ �������
        [SerializeField] private int m_MaxEnergy; // ��� ���������� �������
        [SerializeField] private int m_MaxAmmo; // ��� ���������� ��������

        [SerializeField] private int m_EnergyRegenPersecond; // ����������� ������� ���������������� � �������
        
        private float m_PrimaryEnergy; // ������� ���������� �������
        private int m_SecondaryAmmo; // ������� ���������� ��������

        public void AddEnergy(int e)
        {
            // ������� ���������� ������� = ��������������� ����������� �� 0 �� ��� ���������� �������
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + e, 0, m_MaxEnergy);
        }

        public void AddAmmo(int ammo)
        {
            // ������� ���������� �������� = ��������������� ����������� �� 0 �� ��� ���������� ��������
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        }

        private void InitOffensive()
        {
            m_PrimaryEnergy = m_MaxEnergy;
            m_SecondaryAmmo = m_MaxAmmo;
        }

        /// <summary>
        /// �������������� �������
        /// </summary>
        private void UpdateEnergyRegen()
        {
            m_PrimaryEnergy += (float) m_EnergyRegenPersecond * Time.fixedDeltaTime;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
        }

        /// <summary>
        /// ��������� ������������ �������� �������
        /// </summary>
        /// <param name="props"></param>
        public void AssignWeapon(TurretProperties props)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                m_Turrets[i].AssignLoadout(props);
            }
        }
        #endregion
        */

        /// <summary>
        /// ������������
        /// </summary>
        private bool m_IsInvincible;
        private float m_InvincibilityTimer;

        public bool IsInvincible => m_IsInvincible;

        public void EnableInvincibility(float duration)
        {
            m_IsInvincible = true;
            m_InvincibilityTimer = duration;
        }
        private void UpdateInvincibility()
        {
            if (m_IsInvincible)
            {
                m_InvincibilityTimer -= Time.fixedDeltaTime;
                if (m_InvincibilityTimer <= 0f)
                {
                    m_IsInvincible = false;
                }
            }
        }

        public new void Use(EnemyAsset asset)
        {
            m_MaxLinearVelocity = asset.moveSpeed;
            base.Use(asset);
        }
    }
}

