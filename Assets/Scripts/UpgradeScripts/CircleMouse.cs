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
        // ��������� ��������� ����� � ������������ � ���������� ����
        Vector3 mousePos = Input.mousePosition;
        targetCircle.rectTransform.position = mousePos;

        // ��������� ������ �����
        targetCircle.rectTransform.sizeDelta = new Vector2(circleSize, circleSize);

        // ��������� ���� ����
        if (Input.GetMouseButtonDown(0))
        {
            Use();
        }
    }

    public void Use()
    {
        // ��������� ���� ������
        TDPlayer.Instance.ChangeMana(-m_Mana);

        // ���������� ������ �� ��������� ������
        ClickProtection.Instance.Activate((Vector2 v) =>
        {
            // �������� ������� ������� � ������� �����������
            Vector3 position = v;
            position.z = -Camera.main.transform.position.z;
            position = Camera.main.ScreenToWorldPoint(position);

            // ������� ��� ���������� � ������� �����
            foreach (var collider in Physics2D.OverlapCircleAll(position, damageRadius))
            {
                // ���� ��������� ����� ��������� Enemy, ������� ����
                if (collider.transform.parent.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.TakeDamage(m_Damage, TDProjectile.DamageType.Archer);
                }
            }
        });
    }
}

