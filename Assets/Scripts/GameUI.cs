using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Slider _curCharacterHP;
    [SerializeField] private TextMeshProUGUI _alivePlayerText;
    [SerializeField] private TextMeshProUGUI _winnerText;

    private static GameUI _singleton;

    private void Awake()
    {
        _singleton = this;
        _curCharacterHP.maxValue = 100;
        _winnerText.enabled = false;
    }

    public void UpdateAlivePlayers(int alivePlayers)
    {
        _alivePlayerText.text = $"Alive Players : {alivePlayers}";
        if (alivePlayers == 1)
        {
            _alivePlayerText.enabled = false;
            _winnerText.enabled = true;
        }
        else if (alivePlayers == 0)
        {
            _alivePlayerText.enabled = false;
            _winnerText.enabled = true;
            _winnerText.text = "All are dead";
        }
    }

    public void SetHP(float hp)
    {
        _curCharacterHP.value = hp;
    }

    public static GameUI Get()
    {
        return _singleton;
    }
}
