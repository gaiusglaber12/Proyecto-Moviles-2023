using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig")]
public class LevelConfigSO : ScriptableObject
{
    #region ENUM
    public enum DIFICULTY { EASY, NORMAL, HARD }
    #endregion

    #region EXPOSED_FIELDS
    [SerializeField] private int id = 0;
    [SerializeField] private DIFICULTY dificulty = DIFICULTY.EASY;
    [SerializeField] private BallEntity ballPrefab = null;
    [SerializeField] private int secondsToComplete = 0;

    [SerializeField] private int minScore = 0;
    [SerializeField] private int oneStarScore = 0;
    [SerializeField] private int twoStarScore = 0;
    [SerializeField] private int threeStarScore = 0;

    [SerializeField] private int minAnimalsPerCage = 1;
    [SerializeField] private int maxAnimalsPerCage = 1;

    [SerializeField] private int minCagesPerChunk = 1;
    [SerializeField] private int maxCagesPerChunk = 1;
    #endregion

    #region PROPERTIES
    public BallEntity BallPrefab { get => ballPrefab; set => ballPrefab = value; }
    public DIFICULTY Dificulty { get => dificulty; set => dificulty = value; }
    public int Id { get => id; set => id = value; }
    #endregion
}