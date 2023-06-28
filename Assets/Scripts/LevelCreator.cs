using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] LevelSO[] levels;
    [SerializeField] GameObject endChunk;
    [SerializeField] GameObject deathCube;

    [SerializeField] float minDistanceBetweenPlatforms = 1.3f;
    [SerializeField] float maxDistanceBetweenPlatforms = 2.2f;

    void Start()
    {
        totalScoreText.text =  ScorePopupController.totalScore.ToString();
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        int currentLevel = GetLevel();
        currentLevel = currentLevel % levels.Length;

        LevelSO level = levels[currentLevel];

        CreateLevel(level.chunks);
    }

    private void CreateLevel(GameObject[] levelChunks)
    {
        Vector3 chunkPosition = Vector3.zero;

        for (int i = 0; i < levelChunks.Length; i++)
        {
            GameObject chunkToCreate = levelChunks[i];

            if (i > 0)
            {
                chunkPosition.x += chunkToCreate.transform.localScale.x / 2;
            }

            GameObject chunkInstance = Instantiate(chunkToCreate, chunkPosition, Quaternion.identity, transform);

            float offsetX = Random.Range(minDistanceBetweenPlatforms, maxDistanceBetweenPlatforms);

            chunkPosition.x += chunkInstance.transform.localScale.x / 2 + offsetX;
        }

        // Adding end chunk after creating all desired chunks
        chunkPosition.x += endChunk.transform.localScale.x / 2;
        endChunk.transform.position = chunkPosition;
        endChunk.transform.parent = transform;
        chunkPosition.x += endChunk.transform.localScale.x / 2;

        // Adjusting death cube length based on all level chunks length
        deathCube.transform.localScale = new Vector3(chunkPosition.x + 20,1,3);
        deathCube.transform.position = new Vector3(chunkPosition.x / 2, -6, 0);
    }

    public int GetLevel()
    {
        return PlayerPrefs.GetInt("level", 0);
    }
}
