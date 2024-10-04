using UnityEngine;
using System.ComponentModel;

public class MoveRightViewModel : INotifyPropertyChanged
{
    private MoveRightModel _moveRighModel;
    public event PropertyChangedEventHandler PropertyChanged;

    public int RemainingMoves => _moveRighModel.RemainingMoves;

    public MoveRightViewModel(MoveRightModel model)
    {
        _moveRighModel = model;
    }

    public void AttemptMove()
    {
        if (_moveRighModel.CanMove())
        {
            _moveRighModel.UseMove();
            OnPropertyChanged("RemainingMoves");
        }
        else
        {
            return;
        }
    }

    public void ResetMoves()
    {
        _moveRighModel.ResetMoves();
        OnPropertyChanged("RemainingMoves");
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}