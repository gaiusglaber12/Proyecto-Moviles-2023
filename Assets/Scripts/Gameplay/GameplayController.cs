using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameplayController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private LevelConfigSO debugConfig = null;
    [SerializeField] private LevelConfigSO[] levels = null;
    [SerializeField] private SlingSkinConfigsSO[] skins = null;
    [SerializeField] private SlingController slingController = null;

    [Header("Chunk Settings")]
    [SerializeField] private ChunkView[] chunkViewPrefabs = null;
    [SerializeField] private float depthChunkOffset = 0;
    [SerializeField] private Vector3 initialChunkPosition = Vector3.zero;
    [SerializeField] private int maxPoolSize = 10;
    [SerializeField] private int initialChunks = 0;
    #endregion

    #region PRIVATE_FIELDS
    private LevelConfigSO currLevel = null;

    private ObjectPool<ChunkView> pool = null;
    private Vector3 finalChunkPosition = Vector3.zero;
    private Vector3 currChunkPosition = Vector3.zero;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        currLevel = debugConfig; //TEMP
        pool = new ObjectPool<ChunkView>(CreateChunkView, GetChunkView, ReleaseChunkView, DestroyChunkView, true, maxPoolSize, maxPoolSize);

        if (debugConfig != null)
        {
            slingController.InitGameplay(debugConfig.BallPrefab);
        }

        string selectedSlingerId = PlayerPrefs.GetString("selectedSlinger", string.Empty);
        if (selectedSlingerId == string.Empty)
        {
            selectedSlingerId = "slinger_0";
        }
        selectedSlingerId = selectedSlingerId.ToLower();
        for (int i = 0; i < skins.Length; i++)
        {
            if (skins[i].RemoteId == selectedSlingerId)
            {
                slingController.SetSkin(skins[i].WoodMaterial, skins[i].SlingMaterial);
            }
        }
        currChunkPosition = initialChunkPosition;
        finalChunkPosition = initialChunkPosition;
        for (int i = 0; i < initialChunks; i++)
        {
            pool.Get();
        }
    }

    private void Update()
    {
        if (slingController.transform.position.z > currChunkPosition.z)
        {
            pool.Get();
            currChunkPosition.z += depthChunkOffset;
        }
    }
    #endregion

    #region PRIVATE_METHODS
    private void DestroyChunkView(ChunkView chunkView)
    {
        chunkView.DeInit();
        Destroy(chunkView.gameObject);
    }
    private ChunkView CreateChunkView()
    {
        finalChunkPosition.z += depthChunkOffset;
        GameObject go = Instantiate(chunkViewPrefabs[Random.Range(0, chunkViewPrefabs.Length)].gameObject, finalChunkPosition, Quaternion.identity);
        ChunkView chunkView = go.GetComponent<ChunkView>();
        chunkView.Init(Random.Range(currLevel.MinCagesPerChunk, currLevel.MaxCagesPerChunk), currLevel.MinAnimalsPerCage, currLevel.MaxAnimalsPerCage, currLevel.CageAnimal, currLevel.IsAquatic);
        return chunkView;
    }

    private void GetChunkView(ChunkView chunkView)
    {
        chunkView.gameObject.SetActive(true);
        chunkView.transform.position = finalChunkPosition;
    }

    private void ReleaseChunkView(ChunkView chunkView)
    {
        chunkView.gameObject.SetActive(false);
    }
    #endregion
}
