using UnityEngine.UI;
using UnityEngine;
using TowerDefense;

public class CircleMouse : MonoBehaviour
{
    public Image targetCircle;
    public float circleSize = 50f;
    public float damageRadius = 5f;
    [SerializeField] private int m_Mana;
    [SerializeField] private int m_Damage;

    void Update()
    {
        // Обновляем положение круга в соответствии с положением мыши
        Vector3 mousePos = Input.mousePosition;
        targetCircle.rectTransform.position = mousePos;

        // Обновляем размер круга
        targetCircle.rectTransform.sizeDelta = new Vector2(circleSize, circleSize);

        // Проверяем клик мыши
        if (Input.GetMouseButtonDown(0))
        {
            Use();
        }
    }

    public void Use()
    {
        // Уменьшаем ману игрока
        TDPlayer.Instance.ChangeMana(-m_Mana);

        // Активируем защиту от случайных кликов
        ClickProtection.Instance.Activate((Vector2 v) =>
        {
            // Получаем позицию курсора в мировых координатах
            Vector3 position = v;
            position.z = -Camera.main.transform.position.z;
            position = Camera.main.ScreenToWorldPoint(position);

            // Находим все коллайдеры в радиусе круга
            foreach (var collider in Physics2D.OverlapCircleAll(position, damageRadius))
            {
                // Если коллайдер имеет компонент Enemy, наносим урон
                if (collider.transform.parent.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.TakeDamage(m_Damage, TDProjectile.DamageType.Archer);
                }
            }
        });
    }
}

