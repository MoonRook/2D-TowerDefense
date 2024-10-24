using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class FinishMenu : MonoBehaviour
    {
        public void NewGame()
        {
            FileHandler.Reset(MapCompletion.filename);
            FileHandler.Reset(Upgrade.filename);
            SceneManager.LoadScene(0);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}

