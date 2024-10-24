using UnityEngine;

namespace TowerDefense
{
    public class EntitySpawner : Spawner
    {
        [SerializeField] private GameObject[] m_EntityPrefab;

        protected override GameObject GenerateSpawnedEntity()
        {
            return Instantiate(m_EntityPrefab[Random.Range(0, m_EntityPrefab.Length)]);
        }
    }
}   
        
