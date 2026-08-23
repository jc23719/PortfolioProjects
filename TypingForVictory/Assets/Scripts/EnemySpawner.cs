using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyPrefab;
    public Transform playerTransform;
    public Transform[] spawnPoints;

    public void SpawnUniqueEnemies(int count) {
        char[] letters = GenerateUniqueLetters(10);
        float minDistance = 5f;

        for (int i = 0; i < letters.Length; i++) {
            Vector3 spawnPos;
            int attempts = 0;

            do {
                Vector3 randomOffset = Random.insideUnitSphere * 15f;
                randomOffset.y = 0;
                spawnPos = playerTransform.position + randomOffset;
                attempts++;
            } while (Vector3.Distance(spawnPos, playerTransform.position) < minDistance && attempts < 10);

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.assignedLetter = letters[i];

            LetterDisplay display = enemy.GetComponentInChildren<LetterDisplay>();
            if (display != null) {
                display.letterText.text = letters[i].ToString();
            }
        }
    }

    char[] GenerateUniqueLetters(int count) {
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        char[] shuffled = alphabet.ToCharArray();
        System.Random rng = new System.Random();
        for (int i = shuffled.Length - 1; i > 0; i--) {
            int swapIndex = rng.Next(i + 1);
            (shuffled[i], shuffled[swapIndex]) = (shuffled[swapIndex], shuffled[i]);
        }
        char[] result = new char[count];
        System.Array.Copy(shuffled, result, count);
        return result;
    }
}
