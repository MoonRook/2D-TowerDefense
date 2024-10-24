using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace TowerDefense
{
    public class LevelDisplayController : MonoBehaviour
    {
        [SerializeField] private Maplevel[] levels;
        [SerializeField] private BranchLevel[] branchlevels;

        void Start()
        {
            var drawLevel = 0;
            var score = 1;
            
            while (score != 0 && drawLevel < levels.Length)
            {
                score = levels[drawLevel].Initialise();
                drawLevel += 1;
            }
            
            for (int i = drawLevel; i < levels.Length; i++)
            {
                levels[i].gameObject.SetActive(false);
            }
            
            for (int i = 0; i < branchlevels.Length; i++)
            {
                branchlevels[i].TryActivate();
            }
        }
    }
}


                