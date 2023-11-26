using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class DificultyModel
{
    #region PRIVATE_FIELDS
    [JsonRequired] private string dificulty = string.Empty;
    [JsonRequired] private int reachedStars = 0;
    [JsonRequired] private int maxScore = 0;
    #endregion

    #region PROPERTIES
    [JsonIgnore] public string Dificulty { get => dificulty; set => dificulty = value; }
    [JsonIgnore] public int ReachedStars { get => reachedStars; set => reachedStars = value; }
    [JsonIgnore] public int MaxScore { get => maxScore; set => maxScore = value; }
    #endregion
}

[Serializable]
public class LevelPlayedModel
{
    #region PRIVATE_FIELDS
    [JsonRequired] private int level = 0;
    [JsonRequired] private List<DificultyModel> dificulties = null;
    #endregion

    #region PROPERTIES
    [JsonIgnore] public int Level { get => level; set => level = value; }
    [JsonIgnore] public List<DificultyModel> Dificulties { get => dificulties; set => dificulties = value; }
    #endregion
}

[Serializable]
public class LevelsPlayedModel
{
    #region PRIVATE_FIELDS
    [JsonRequired] private List<LevelPlayedModel> levelsPlayedModels = null;
    #endregion

    #region PROPERTIES
    [JsonIgnore] public List<LevelPlayedModel> LevelsPlayedModels { get => levelsPlayedModels; set => levelsPlayedModels = value; }
    #endregion
}
