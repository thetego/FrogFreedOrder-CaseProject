using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class LevelManager : Singleton<LevelManager>
{   
    private int levelIndex = 0;
    private int levelDisplayNumber = 1;
    [SerializeField]private LevelData[] _levels;
    private LevelData _currentLevel;

    [SerializeField] private bool _loopLevels;
    [SerializeField] private TMP_Text _levelText;

    public UnityEvent OnFinishCreatingLevel;

    void Start()
    {
        _levels = Resources.LoadAll<LevelData>(Constants.Levels.LevelsPath);

        if (!PlayerPrefs.HasKey(Constants.Levels.LevelPrefKey))
            PlayerPrefs.SetInt(Constants.Levels.LevelPrefKey, levelIndex);
        else 
            levelIndex = PlayerPrefs.GetInt(Constants.Levels.LevelPrefKey);


        if (!PlayerPrefs.HasKey(Constants.Levels.LevelDisplayPrefKey))
            PlayerPrefs.SetInt(Constants.Levels.LevelDisplayPrefKey, levelDisplayNumber);
        else 
            levelDisplayNumber = PlayerPrefs.GetInt(Constants.Levels.LevelDisplayPrefKey);
        
    }

    public void GenerateLevel()
    {
        if (_loopLevels)
        {
            if (levelIndex > _levels.Length-1)
            {
                levelIndex = 0;
            }
        }

        _currentLevel = _levels[levelIndex];
        NodeManager.Instance.GenerateNodes();

        UpdateLevelDisplay();

        MoveRightModel moveModel = new MoveRightModel(_currentLevel.moveLimit);
        GameManager.Instance.moveViewModel = new MoveRightViewModel(moveModel);
        
        for (int i = 0; i < NodeManager.Instance.GetNodes().Count; i++)
        {
            for (int b = 0; b < _currentLevel.layers.Count; b++)
            {
                if (_currentLevel.layers[b].cells[i].type != CellType.Empty)
                {
                    if (_currentLevel.layers[b].cells[i].column.Equals(NodeManager.Instance.GetNodes()[i].GetColumn))
                    {
                        if (_currentLevel.layers[b].cells[i].row.Equals(NodeManager.Instance.GetNodes()[i].GetRow))
                        {
                            NodeManager.Instance.GetNodes()[i].AddCell(_currentLevel.layers[b].cells[i]);
                        }
                    }
                }

            }
        }

        OnFinishCreatingLevel?.Invoke();
    }

    public void UpdateLevelDisplay()
    {
        _levelText.text = "Level "+levelDisplayNumber.ToString();
    }

    public void CheckForLevelState()
    {
        if (IsLevelFinished())
        {
            StartCoroutine(EndLevelCoroutine(true));
        }
        else 
        {
            if (GameManager.Instance.RemainingMoveRights<1)
            {
                StartCoroutine(EndLevelCoroutine(false));
            }
        }  
    }
    public bool IsLevelFinished()
    {
        return GameManager.Instance.BerryCount == 0;
    }

    public LevelData GetCurrentLevel()
    {
        return _currentLevel;
    }
    IEnumerator EndLevelCoroutine(bool success)
    {
        yield return new WaitForSeconds(1f);
        if (success)
        {
            GameManager.Instance.Win();
            levelIndex++;
            levelDisplayNumber++;
        }
        else 
        {
            GameManager.Instance.Fail();
        }  
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(Constants.Levels.LevelPrefKey,levelIndex);
        PlayerPrefs.SetInt(Constants.Levels.LevelDisplayPrefKey,levelDisplayNumber);
    }
}
