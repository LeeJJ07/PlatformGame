using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    public void UpdateCoinText(int coin)
    {
        coinText.text = coin.ToString();
    }
}
