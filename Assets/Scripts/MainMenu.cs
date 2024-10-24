using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        private void Start()
        {
            continueButton.interactable = FileHandler.HasFile(MapCompletion.filename);
        }
        
        public void NewGame()
        {
            FileHandler.Reset(MapCompletion.filename);
            FileHandler.Reset(Upgrade.filename);
            SceneManager.LoadScene(1);
        }

        public void Continue()
        {
            SceneManager.LoadScene(1);
        }
        
        public void Quit()
        {
            Application.Quit();
        }
    }
}

