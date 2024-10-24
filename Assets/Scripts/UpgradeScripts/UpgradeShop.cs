using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class UpgradeShop : MonoBehaviour
    {
        [SerializeField] private int money;
        [SerializeField] private Text moneyText;
        [SerializeField] private BuyUpgrade[] sales;

        private void Start()
        {
            
            foreach (var slot in sales)
            {
                slot.Initialize();
                slot.transform.Find("Button Upgrade").GetComponent<Button>().onClick.AddListener(UpdateMoney);
            }
            UpdateMoney();
            Upgrade.UpgradeAssetsInitialize(sales);
        }

        public void UpdateMoney()
        {
            money = MapCompletion.Instance.TotalScore;
            money -= Upgrade.GetTotalCost();
            moneyText.text = money.ToString();
            
            foreach (var slot in sales)
            {
                slot.CheckCost(money);
            }
        }
    }
}

