using System;
using System.Collections;
using System.Collections.Generic;
using Game.Character;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class AiGenerator : MonoBehaviour
{
    [Unit(Units.Meter)] public float radius = 5;
    public int count = 5;
    public List<Character> characterPrefabs;

    private void Start()
    {
        Create();
    }

    public void Create()
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 randomPos = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * radius;
            Character randomPrefab = characterPrefabs[new Random().Next(characterPrefabs.Count)];

            var ai = GameObject.Instantiate(randomPrefab, randomPos, Quaternion.identity);
            ai.transform.SetParent(gameObject.transform);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
        Handles.Label(transform.position, "Mob Spawner");
    }
}
