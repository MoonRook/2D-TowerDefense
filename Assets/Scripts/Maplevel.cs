using UnityEngine;
using SpaceShooter;
using UnityEngine.UI;

namespace TowerDefense
{
    public class Maplevel : MonoBehaviour
    {
        [SerializeField] private Episode m_Episode;
        
        [SerializeField] private RectTransform resultPanel;
        [SerializeField] private Image[] resultImage;

        public bool IsComplete { get { return gameObject.activeSelf 
                    && resultPanel.gameObject.activeSelf; } }

        public void LoadLevel()
        {
            LevelSequnecesController.Instance.StartEpisode(m_Episode); 
        }
       
        public int Initialise()
        {
            var score = MapCompletion.Instance.GetEpisodeScore(m_Episode);

            resultPanel.gameObject.SetActive(score > 0);
            for (int i = 0; i < score; i++)
            {
                resultImage[i].color = Color.white;
            }
            return score;
        }
    }
}
