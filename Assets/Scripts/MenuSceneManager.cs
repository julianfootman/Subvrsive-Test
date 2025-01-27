using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _playerCount;

    private void Start()
    {
        _playerCount.text = "10";
    }

    public void OnClickStartGame()
    {
        PlayerPrefs.SetInt("PlayerCount", int.Parse(_playerCount.text));
        SceneManager.LoadScene("Game");
    }
}
