using UnityEngine;
using SpaceShooter;
using System;

namespace TowerDefense
{
    public class MapCompletion : SingletonBase<MapCompletion>
    {
        public const string filename = "completion.dat";
        
        [Serializable]
        private class EpisodeScore
        {
            public Episode episode;
            public int score;
        }

        [SerializeField] private EpisodeScore[] completionData;
        public int TotalScore { private set; get; }

        private new void Awake()
        {
            base.Awake();
            Saver<EpisodeScore[]>.TryLoad(filename, ref completionData);

            foreach (var episodeScore in completionData)
            {
                TotalScore += episodeScore.score;
            }
        }
        
        public static void SaveEpisodeResult(int levelScore)
        {
            if (Instance)
            {
                foreach (var item in Instance.completionData)
                {
                    // Сохраняет новые очки прохождения уровня
                    if (item.episode == LevelSequnecesController.Instance.CurrentEpisode)
                    {
                        if (levelScore > item.score)
                        {
                            Instance.TotalScore += levelScore - item.score;
                            item.score = levelScore;
                            Saver<EpisodeScore[]>.Save(filename, Instance.completionData);
                        }
                    }
                }
            }

            else 
            {
                Debug.Log($"Episode complete with score {levelScore}");
            }
        }
       
        public int GetEpisodeScore(Episode m_Episode)
        {
            foreach (var data in completionData)
            {
                if (data.episode == m_Episode)
                    return data.score;
            }
            return 0;
        }
    }
}

