using UnityEngine;
using System.Collections;

namespace TowerDefense
{
    public class OnEnabledSound : MonoBehaviour
    {
        [SerializeField] private Sound m_Sound;

        private void OnEnable()
        {
            StartCoroutine(PlaySoundWithDelay());
        }

        private IEnumerator PlaySoundWithDelay()
        {
            yield return new WaitForSeconds(1f); // �������� � 3 �������
            m_Sound.Play();
        }
    }
}

