using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class TextUpdate : MonoBehaviour
    {
        public enum UpdateSource { Gold, Life, Mana }
        public UpdateSource source = UpdateSource.Gold;
        
        private Text m_Text;
        void Start()
        {
            m_Text = GetComponent<Text>();
            switch (source)
            {
                case UpdateSource.Gold:
                    TDPlayer.Instance.GoldUpdateSubscribe(UpdateText);
                break;
                
                case UpdateSource.Life:
                    TDPlayer.Instance.LifeUpdateSubscribe(UpdateText);
                break;
                
                case UpdateSource.Mana:
                    TDPlayer.Instance.ManaUpdateSubscribe(UpdateText);
                    break;
            }
        }
       
        void UpdateText(int money)
        {
            m_Text.text = money.ToString();
        }

        private void OnDestroy()
        {
            TDPlayer.Instance.OnGoldUpdate -= UpdateText;
            TDPlayer.Instance.OnLifeUpdate -= UpdateText;
            TDPlayer.Instance.OnManaUpdate -= UpdateText;
        }
    }
}
