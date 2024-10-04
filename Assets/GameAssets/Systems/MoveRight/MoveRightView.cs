using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveRightView : MonoBehaviour
{
    [SerializeField]private TMP_Text moveText;
    private MoveRightViewModel _moveViewModel;

    public void SetViewModel(MoveRightViewModel viewModel)
    {
        _moveViewModel = viewModel;
        _moveViewModel.PropertyChanged += OnViewModelPropertyChanged;
        UpdateMoveText(_moveViewModel.RemainingMoves);
    }

    private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "RemainingMoves")
        {
            UpdateMoveText(_moveViewModel.RemainingMoves);
        }
    }

    private void UpdateMoveText(int remainingMoves)
    {
        moveText.text = "Moves Left: " + remainingMoves;
    }
}
