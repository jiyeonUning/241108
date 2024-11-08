using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] GameObject PlayerPrefab;

    [Range(0, 10)]
    [SerializeField] float randomRange;

    public void PlayerJoined(PlayerRef player)
    {
        Debug.Log("플레이어 참가");

        if (player != Runner.LocalPlayer) return;

        // 플레이어 스폰
        Vector3 spawnPos = new Vector3(Random.Range(-randomRange, randomRange), 0, Random.Range(-randomRange, randomRange));

        // Instantiate(PlayerPrefab, spawnPos, Quaternion.identity); // 혼자 만들기
        Runner.Spawn(PlayerPrefab, spawnPos, Quaternion.identity);   // 네트워크에서 다 같이 만들기
    }

    public void PlayerLeft(PlayerRef player)
    {
        Debug.Log("플레이어 나감");
    }
}
