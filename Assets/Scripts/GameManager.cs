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
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private Weapon[] _weaponPrefabs;
    [SerializeField] private KeyCode _nextPlayerCamera;

    private List<Player> _spawnedPlayers = new List<Player>();
    public List<Player> SpanwedPlayers => _spawnedPlayers;

    private int _currentViewIndex;

    private void Start()
    {
        for (int i = 0; i < 10; i ++)
        {
            SpawnPlayer();
        }

        ViewNextPlayer();
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
        _spawnedPlayers.Add(spawnedPlayer);
    }

    private void ViewNextPlayer()
    {
        _currentViewIndex++;
        if (_currentViewIndex == _spawnedPlayers.Count)
        {
            _currentViewIndex = 0;
        }

        _virtualCamera.Follow = _spawnedPlayers[_currentViewIndex].FirstPersonCameraFollow;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_nextPlayerCamera))
        {
            ViewNextPlayer();
        }
    }
}
