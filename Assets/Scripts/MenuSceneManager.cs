using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _playerCount;

    public void OnClickStartGame()
    {
        PlayerPrefs.SetInt("PlayerCount", int.Parse(_playerCount.text));
        SceneManager.LoadScene("Game");
    }
}
