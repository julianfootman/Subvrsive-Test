using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Slider _curCharacterHP;
    [SerializeField] private TextMeshProUGUI _alivePlayerText;
    [SerializeField] private TextMeshProUGUI _winnerText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _menuButton;

    private static GameUI _singleton;

    private void Awake()
    {
        _singleton = this;
        _curCharacterHP.maxValue = 100;
        _winnerText.enabled = false;
        _restartButton.onClick.AddListener(() => { SceneManager.LoadScene(1);});
        _menuButton.onClick.AddListener(() => { SceneManager.LoadScene(0); });
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
