using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private CircleArea startArea;
        public CircleArea StartArea { get { return startArea; } }

        [SerializeField] private AIPointerPatrol[] points;
        public int Length { get => points.Length; }
        public AIPointerPatrol this[int i] { get => points[i]; }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = UnityEngine.Color.red;

            for (int i = 0; i < points.Length; i++)
            {
                if (i > 0)
                {
                    Gizmos.DrawLine(points[i - 1].transform.position, points[i].transform.position);
                    Gizmos.DrawSphere(points[i].transform.position, points[i].Radius);
                }
            }
        }
    }
}

