using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public abstract class Spawner : MonoBehaviour
    {
        public enum SpawnMode // �������� ������ Spawner ��� ������ ��� ������������ (���������)
        { 
            Start,
            Loop
        }

        protected abstract GameObject GenerateSpawnedEntity();

        [SerializeField] private CircleArea m_Area; // ���� � ������� Spawner ����� �������� �������

        [SerializeField] private SpawnMode m_SpawnMode; // ������ �� ������� Spawner

        [SerializeField] private int m_NumSpawns; // ����������� ��������, ������� ����� ���������� Spawner

        [SerializeField] private float m_RespawnTime; // ������� ���������� Spawner

        private float m_Timer;

        private void Start()
        {
            if (m_SpawnMode == SpawnMode.Start) // ���� ������� ������ Spawner = ���������
            {
                SpawnEntities();
            }
           
            m_Timer = m_RespawnTime; // �������� ������ ���������� Spawner
        }

        private void Update()
        {
            if (m_Timer > 0) // ���� ������ > 0, �� �������� � ������� ������� ���������� �����
                m_Timer -= Time.deltaTime;

            // ���� ������� ������ Spawner = ����������� � ����� ���������� ������, �� ������� ������
            if (m_SpawnMode == SpawnMode.Loop && m_Timer <= 0)
            {
                SpawnEntities();

                m_Timer = m_RespawnTime;
            }
        }

        /// <summary>
        /// ������� ��������
        /// </summary>
        private void SpawnEntities()
        {
            for (int i = 0; i < m_NumSpawns; i++) // ���� ������ ��������� �� 0 �� ������� ����������
            {
                var e = GenerateSpawnedEntity();
                e.transform.position = m_Area.RandomInsideZone;
            }
        }
    }
}

