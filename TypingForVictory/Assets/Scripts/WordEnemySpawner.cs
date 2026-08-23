using UnityEngine;
using TMPro;

public class WordEnemySpawner : MonoBehaviour {
    public GameObject wordEnemyPrefab;
    public Transform playerTransform;
    public Transform[] spawnPoints; 
    public string[] wordPool = { "fire", "water", "earth", "air", "demon", "goblin", "pixel", "dragon" };

    public void SpawnUniqueWordEnemies(int count) {
        string[] words = GenerateUniqueWords(count);
        float minDistance = 5f;

        for (int i = 0; i < words.Length; i++) {
        Vector3 spawnPos;
        int attempts = 0;

        do {
            Vector3 randomOffset = Random.insideUnitSphere * 15f;
            randomOffset.y = 0;
            spawnPos = playerTransform.position + randomOffset;
            attempts++;
        } while (Vector3.Distance(spawnPos, playerTransform.position) < minDistance && attempts < 10);

        GameObject enemy = Instantiate(wordEnemyPrefab, spawnPos, Quaternion.identity);

        WordEnemy wordScript = enemy.GetComponent<WordEnemy>();
        wordScript.assignedWord = words[i];

        LetterDisplay display = enemy.GetComponentInChildren<LetterDisplay>();
        if (display != null) {
            display.letterText.text = words[i];
        }
    }
}

    string[] GenerateUniqueWords(int count) {
        string[] shuffled = (string[])wordPool.Clone();
        System.Random rng = new System.Random();
        for (int i = shuffled.Length - 1; i > 0; i--) {
            int swapIndex = rng.Next(i + 1);
            (shuffled[i], shuffled[swapIndex]) = (shuffled[swapIndex], shuffled[i]);
        }

        string[] result = new string[Mathf.Min(count, shuffled.Length)];
        System.Array.Copy(shuffled, result, result.Length);
        return result;
}
}
