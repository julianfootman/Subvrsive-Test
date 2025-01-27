using Cinemachine;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Transform _spawnCenter;
    [SerializeField] private float _spawnRadius = 10;
    [SerializeField] private int _maxAttempts = 10;
    [SerializeField] private Weapon[] _weaponPrefabs;
    [SerializeField] private KeyCode _nextPlayerCamera;
    [SerializeField] private KeyCode _cameraSwitchButton;

    private List<Player> _alivePlayers = new List<Player>();

    private int _currentViewIndex;
    private CameraMode _currentCameraMode;
    private Player _activePlayer;

    private static GameManager _singleton;
    private enum CameraMode
    {
        FPS = 0, TPS = 1
    }

    private void Awake()
    {
        _singleton = this;
        _currentCameraMode = CameraMode.FPS;
    }

    private void Start()
    {
        int playerCount = PlayerPrefs.GetInt("PlayerCount", 10);
        for (int i = 0; i < playerCount; i ++)
        {
            SpawnPlayer();
        }

        ViewCurrentPlayer();
    }

    private void SpawnPlayer()
    {
        Vector3 randomPosition = Vector3.zero;
        bool validPositionFound = false;

        for (int i = 0; i < _maxAttempts; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * _spawnRadius;
            randomPoint.y = _spawnCenter.position.y;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                randomPosition = hit.position;
                validPositionFound = true;
                break;
            }
        }

        if (!validPositionFound)
        {
            randomPosition = _spawnCenter.position;
        }

        Player spawnedPlayer = Instantiate(_playerPrefab, randomPosition, Quaternion.identity);
        int randomWeaponIdx = Random.Range(0, _weaponPrefabs.Length);
        spawnedPlayer.EquipWeapon(_weaponPrefabs[randomWeaponIdx]);
        _alivePlayers.Add(spawnedPlayer);
    }

    private void ViewCurrentPlayer()
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain.ActiveVirtualCamera != null)
        {
            brain.ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false);
        }


        if (_alivePlayers.Count == 1)
        {
            _alivePlayers[0].EnterWinnerState();
            _alivePlayers[0].ThirdPersonCamera.gameObject.SetActive(true);
        }
        else
        {
            if (_currentCameraMode == CameraMode.FPS)
            {
                _alivePlayers[_currentViewIndex].FirstPersonCamera.gameObject.SetActive(true);
            }
            else
            {
                _alivePlayers[_currentViewIndex].ThirdPersonCamera.gameObject.SetActive(true);
            }
        }        

        _activePlayer = _alivePlayers[_currentViewIndex];

        GameUI.Get().SetHP(_alivePlayers[_currentViewIndex].CurrentHealth);
        GameUI.Get().UpdateAlivePlayers(_alivePlayers.Count);
    }

    private void Update()
    {
        if (Input.GetKeyDown(_nextPlayerCamera))
        {
            _currentViewIndex++;
            if (_alivePlayers.Count == _currentViewIndex)
            {
                _currentViewIndex = 0;
            }
            ViewCurrentPlayer();
        }

        if (Input.GetKeyDown(_cameraSwitchButton))
        {
            _currentCameraMode = (_currentCameraMode == CameraMode.TPS ? CameraMode.FPS : CameraMode.TPS);
            ViewCurrentPlayer();
        }
    }

    public Player GetActivePlayer()
    {
        return _activePlayer;
    }

    public static GameManager Get()
    {
        return _singleton;
    }

    public void HandlePlayerDeath(Player player)
    {
        _alivePlayers.Remove(player);
        GameUI.Get().UpdateAlivePlayers(_alivePlayers.Count);

        if (player == _activePlayer)
        {
            _currentViewIndex = Random.Range(0, _alivePlayers.Count);
        }

        ViewCurrentPlayer();
    }
}
