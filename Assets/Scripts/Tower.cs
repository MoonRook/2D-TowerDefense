using SpaceShooter;
using UnityEngine;

namespace TowerDefense
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] public float m_Radius = 5f;
        [SerializeField] public UpgradeAsset m_RangeUpgrade;
        private float m_Lead = 0.3f;
        private Turret[] turrets;
        private Rigidbody2D target = null;

        private void Start()
        {
            int level = Upgrade.GetUpgradeLevel(m_RangeUpgrade);
            m_Radius = m_RangeUpgrade.FloatValues[level];
        }
        
        public void Use(TowerAsset asset)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = asset.sprite;
            turrets = GetComponentsInChildren<Turret>();
            foreach (var turret in turrets)
            {
                turret.AssignLoadout(asset.turretProperties);
            }

            GetComponentInChildren<BuildSite>().SetBuildableTowers(asset.m_UpgradeTo);
        }

        private void Update()
        {
            if (target)
            {
                if (Vector3.Distance(target.transform.position, transform.position) <= m_Radius)
                {
                    foreach (var turret in turrets)
                    {
                        turret.transform.up = target.transform.position 
                            - turret.transform.position + (Vector3)target.velocity * m_Lead;
                        turret.Fire();
                    }
                }

                else
                {
                    target = null;
                }
            }

            else
            {
                var enter = Physics2D.OverlapCircle(transform.position, m_Radius);

                if (enter)
                {
                    target = enter.transform.root.GetComponent<Rigidbody2D>();
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan; 
            Gizmos.DrawWireSphere(transform.position, m_Radius);
        }
    }
}
