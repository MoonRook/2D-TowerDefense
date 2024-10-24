using UnityEngine;
using UnityEngine.UI;
using static TowerDefense.Abilitys;

namespace TowerDefense
{
    public class BuyUpgrade : MonoBehaviour
    {
        [SerializeField] private UpgradeAsset asset;
        [SerializeField] private Image upgradeIcon;
        private int costNumber = 0;
        [SerializeField] private Text level, costText;
        [SerializeField] private Button buyButton;

        public UpgradeAsset Asset => asset;

        public void Initialize()
        {
            upgradeIcon.sprite = asset.sprite;
            var savedlevel = Upgrade.GetUpgradeLevel(asset);

            if (savedlevel >= asset.costBuyLevel.Length)
            {
                level.text = $"Óð.: {savedlevel} (Max)";
                buyButton.interactable = false;
                buyButton.transform.Find("Image Cost").gameObject.SetActive(false);
                buyButton.transform.Find("Text Buy").gameObject.SetActive(false);
                costText.text = "X";
                costNumber = int.MaxValue;
            }
            
            else
            {
                level.text = $"Óð.: {savedlevel + 1}";
                costNumber = asset.costBuyLevel[savedlevel];
                costText.text = costNumber.ToString();
            }
        }

        public void Buy()
        {
            Upgrade.BuyUpgrade(asset);
            Initialize();
        }

        public void CheckCost(int money)
        {
            buyButton.interactable = money >= costNumber;
        }
    }
}

