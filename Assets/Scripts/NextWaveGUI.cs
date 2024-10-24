using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class NextWaveGUI : MonoBehaviour
    {
        [SerializeField] private Text bonusAmount;
        
        private EnemyWavesManager manager;
        private float timeToNextWave;

        void Start()
        {
            manager = FindObjectOfType<EnemyWavesManager>();
            EnemyWave.OnWavePrepare += (float time) =>
            {
                timeToNextWave = time;
            };
        }

        public void CallWave()
        {
            manager.ForceNextWave();
        }

        private void Update()
        {
            var bonus = (int)timeToNextWave;
            if (bonus < 0) bonus = 0;
            bonusAmount.text = bonus.ToString();
            timeToNextWave -= Time.deltaTime;
        }
    }
}

