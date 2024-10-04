using System.Collections.Generic;
using DG.Tweening;
using UnityEngine; 
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    
    private List<Berry> _berriesToEat = new List<Berry>();
    [SerializeField]private UnityEvent OnFail,OnSuccess;
    public float offset;
    public bool SmoothTongueTurns;

    public  MoveRightViewModel moveViewModel;

    public int RemainingMoveRights => moveViewModel.RemainingMoves;
    public int BerryCount => _berriesToEat.Count;
    
    void Start()
    {
        DOTween.Init();
    }
    public void StartGame()
    {
        LevelManager.Instance.GenerateLevel();

        MoveRightView moveView = FindObjectOfType<MoveRightView>(true);
        moveView.SetViewModel(moveViewModel);

        moveViewModel.ResetMoves();
    }
    public void Retry()
    {
        StartGame();
    }
    public void NextLevel()
    {
        StartGame();
    }
    public void AddBerry(Berry grape)
    {
        _berriesToEat.Add(grape);
    }
    public void RemoveBerry(Berry grape)
    {
        _berriesToEat.Remove(grape);
    }
    public void ClearBerries()
    {
        _berriesToEat.Clear();
    }

    public void UseMove()
    {
        moveViewModel.AttemptMove();
    }
    public void Win()
    {
        
        OnSuccess?.Invoke();
    }
    public void Fail()
    {
        OnFail?.Invoke();
    }



}
