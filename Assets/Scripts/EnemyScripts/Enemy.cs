using UnityEngine;
using SpaceShooter;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace TowerDefense
{
    [RequireComponent(typeof(Destructible))]
    [RequireComponent(typeof(TDPatrolController))]
    public class Enemy : MonoBehaviour
    {
        public enum ArmorType
        {
            Arch = 0, Mag = 1, War = 2
        }

        private static Func<int, TDProjectile.DamageType, int, int>[] ArmorDamageFunctions = 
        { 
            (int power, TDProjectile.DamageType type, int armor) =>
            {
                // ArmorType.Archer
                switch (type)
                {
                    case TDProjectile.DamageType.Magic: return power;
                    default: return Mathf.Max(power - armor, 1);
                }
            },
            
            (int power, TDProjectile.DamageType type, int armor) =>
            {
                // ArmorType.Magic
                if (TDProjectile.DamageType.Archer == type)
                 armor = armor / 2; 

                return Mathf.Max(power - armor, 1);
            },
        };


        [SerializeField] private int m_Damage = 1;
        [SerializeField] private int m_Gold = 1;
        [SerializeField] private int m_Mana = 1;
        [SerializeField] private int m_Armor = 1;
        [SerializeField] private ArmorType m_ArmorType;

        private Destructible m_Destructible;

        private void Awake()
        {
            m_Destructible = GetComponent<Destructible>();
        }

        public event Action OnEnd;
        private void OnDestroy() 
        {  
            OnEnd?.Invoke(); 
        }
      
        public void Use(EnemyAsset asset)
        {
            // Настройки спрайта
            var sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();
            sr.color = asset.color;
            sr.transform.localScale = new Vector3(asset.spriteScale.x, asset.spriteScale.y, 1);

            // Настройки аниматора
            sr.GetComponent<Animator>().runtimeAnimatorController = asset.animations;

            // Настройки hp и score
            GetComponent<SpaceShip>().Use(asset);

            // Настройки коллайдера
            GetComponentInChildren<CircleCollider2D>().radius = asset.radius;

            m_Damage = asset.damage;
            m_Armor = (int) asset.armor;
            m_ArmorType = asset.armorType;
            m_Gold = asset.gold;
            m_Mana= asset.mana;
        }

        public void DamagePlayer()
        {
            TDPlayer.Instance.ReduceLife(m_Damage);
        }

        public void GivePlayerGold()
        {
            TDPlayer.Instance.ChangeGold(m_Gold);
        }

        public void GivePlayerMana()
        {
            TDPlayer.Instance.ChangeMana(m_Mana);
        }

        public void TakeDamage(int damage, TDProjectile.DamageType damageType)
        {
            m_Destructible.ApplyDamage(ArmorDamageFunctions[(int)m_ArmorType](damage, damageType, m_Armor));
        }
        
        #if UNITY_EDITOR
        [CustomEditor(typeof(Enemy))]
        public class  EnemyInspector : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                
                EnemyAsset a = EditorGUILayout.ObjectField(null, typeof(EnemyAsset), false) as EnemyAsset;
                if (a)
                {
                    (target as Enemy).Use(a);
                }
            }
        }
        #endif
    }
}

