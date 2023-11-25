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
    [SerializeField] private GameObject cageAnimal = null;
    [SerializeField] private bool isAquatic = false;
    [SerializeField] private int secondsToComplete = 0;
    [SerializeField] private float speed = 0.0f;
    [SerializeField] private int seconds = 60;

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
    public int MinAnimalsPerCage { get => minAnimalsPerCage; set => minAnimalsPerCage = value; }
    public int MaxAnimalsPerCage { get => maxAnimalsPerCage; set => maxAnimalsPerCage = value; }
    public int MinCagesPerChunk { get => minCagesPerChunk; set => minCagesPerChunk = value; }
    public int MaxCagesPerChunk { get => maxCagesPerChunk; set => maxCagesPerChunk = value; }
    public GameObject CageAnimal { get => cageAnimal; set => cageAnimal = value; }
    public bool IsAquatic { get => isAquatic; set => isAquatic = value; }
    public float Speed { get => speed; set => speed = value; }
    public int Seconds { get => seconds; set => seconds = value; }
    public int MinScore { get => minScore; set => minScore = value; }
    public int OneStarScore { get => oneStarScore; set => oneStarScore = value; }
    public int TwoStarScore { get => twoStarScore; set => twoStarScore = value; }
    public int ThreeStarScore { get => threeStarScore; set => threeStarScore = value; }
    #endregion
}