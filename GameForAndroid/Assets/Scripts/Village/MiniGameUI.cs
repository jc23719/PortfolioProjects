using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum MiniGameType {Rats, Apples};

public class MiniGameUI : MonoBehaviour
{
    public MiniGameType miniGameType;
    public GameObject panel;

    public GameObject ratPrefab;
    public RectTransform ratContainer;
    public int ratCount = 5;
    private int ratsRemaining;

    public GameObject treePrefab;
    public RectTransform treeContainer;
    public int treeCount = 5;
    private int applesRemaining;

    public void OpenMiniGame()
    {
        panel.SetActive(true);
        if (miniGameType == MiniGameType.Rats)
            StartRatMiniGame();
        else if (miniGameType == MiniGameType.Apples)
            StartAppleMiniGame();
    }

    private void StartRatMiniGame()
    {
        ratsRemaining = ratCount;

        for (int i = 0; i < ratCount; i++)
        {
            GameObject rat = Instantiate(ratPrefab, ratContainer);
            RectTransform rt = rat.GetComponent<RectTransform>();

            float x = Random.Range(-ratContainer.rect.width / 2f, ratContainer.rect.width / 2f);
            float y = Random.Range(-ratContainer.rect.height / 2f, ratContainer.rect.height / 2f);

            rt.anchoredPosition = new Vector2(x, y);

            rat.GetComponent<Rat>().Init(this);
        }

    }

    public void RatCleared()
    {
        ratsRemaining--;

        if (ratsRemaining <= 0)
        {
            CompleteMiniGame();
        }
    }

    private void StartAppleMiniGame()
    {
        applesRemaining = 0;

        List<Vector2> usedPositions = new List<Vector2>();
        float minDistance = 150f;

        for (int i = 0; i < treeCount; i++)
        {
            Vector2 spawnPos;

            int attempts = 0;
            do
            {
                float x = Random.Range(-treeContainer.rect.width / 2f, treeContainer.rect.width / 2f);
                float y = Random.Range(-treeContainer.rect.height / 2f, treeContainer.rect.height / 2f);
                spawnPos = new Vector2(x, y);

                attempts++;

            } while (IsOverlapping(spawnPos, usedPositions, minDistance) && attempts < 20);

            usedPositions.Add(spawnPos);

            GameObject tree = Instantiate(treePrefab, treeContainer);
            RectTransform rt = tree.GetComponent<RectTransform>();
            rt.anchoredPosition = spawnPos;

            int appleCount = Random.Range(2, 5);
            applesRemaining += appleCount;

            tree.GetComponent<Tree>().Init(this, appleCount);
        }
    }

    private bool IsOverlapping(Vector2 newPos, List<Vector2> usedPositions, float minDist)
    {
        foreach (Vector2 pos in usedPositions)
        {
            if (Vector2.Distance(newPos, pos) < minDist)
                return true;
        }
        return false;
    }


    public void AppleCollected()
    {
        applesRemaining--;

        if (applesRemaining <= 0)
            CompleteMiniGame();
    }

    private void CompleteMiniGame()
    {
        string scene = SceneManager.GetActiveScene().name;
        if (scene == "VILLAGE") {
            InventoryManager.Instance.AddItem(GadgetType.Fan, 1);
            InventoryManager.Instance.AddItem(GadgetType.JumpPad, 2);
            InventoryManager.Instance.AddItem(GadgetType.Plug, 1);
            InventoryManager.Instance.AddItem(GadgetType.StickyWall, 2);
        } else if (scene == "VILLAGE2") {
            InventoryManager.Instance.AddItem(GadgetType.Fan, 2);
            InventoryManager.Instance.AddItem(GadgetType.JumpPad, 1);
            InventoryManager.Instance.AddItem(GadgetType.Plug, 1);
            InventoryManager.Instance.AddItem(GadgetType.StickyWall, 2);
        }
        panel.SetActive(false);

        foreach (Transform child in ratContainer)
            Destroy(child.gameObject);
    }
}
