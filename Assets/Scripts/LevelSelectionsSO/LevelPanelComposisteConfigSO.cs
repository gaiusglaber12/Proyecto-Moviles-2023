using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelPanelComposisteConfig", menuName = "Configs/LevelPanelComposisteConfigSO")]
public class LevelPanelComposisteConfigSO : ScriptableObject
{
    public int number = 0;
    public LevelConfigSO levelConfig = null;
}
