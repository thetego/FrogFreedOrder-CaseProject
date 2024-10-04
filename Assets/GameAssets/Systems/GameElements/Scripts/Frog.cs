using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class Frog : GameElement, IFrogObserver
{

    private List<Node> _nodesTongueCrossed= new List<Node>();

    private Vector3 _currentDirection;
    private Animator _animator;

    //private Spline _tongueRenderer;

    [Header("Tongue Parameters")]
    [SerializeField] private float _tongueMoveDurationPerNode;
    [SerializeField] private TongueRenderer _tongueRenderer;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _currentDirection = cell._cellData.DetermineDirection();
        _colorType = cell._cellData.color;

    }

    public override void Interact(bool isUserInteract)
    {
        if (!isUserInteract||!isInteractable)
            return;

        
        GameManager.Instance.UseMove();
        isInteractable=false;
        _animator.SetTrigger(Constants.AnimationsParameters.MouthOpen);
        InteractAnim();

        _currentDirection = cell._cellData.DetermineDirection();
        _nodesTongueCrossed.Add(cell.currentNode);
        

        StartCoroutine(MoveTongueRoutine(_tongueMoveDurationPerNode,NodeManager.Instance.GetNeighbor(cell.currentNode,_currentDirection)));
    }

    public void ContinueEat(Node nextNode)
    {
        StartCoroutine(MoveTongueRoutine(_tongueMoveDurationPerNode,nextNode));
    }


    private IEnumerator MoveTongueRoutine(float duration, Node nextNode)
    {
        if (nextNode)
        {
            float elapsedTime = 0f;

            _tongueRenderer.AddTonguePoint(_tongueRenderer.tongueFollowPoint.localPosition);
        

            Vector3 nextPos = transform.InverseTransformPoint(nextNode.GetActiveCell.tonguePos.position);
            _tongueRenderer.tongueFollowPoint.DOLocalMove(nextPos,duration).SetEase(Ease.Linear);
            

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
        
                _tongueRenderer.spline.SetKnot(_tongueRenderer.spline.Count - 1, new BezierKnot(_tongueRenderer.tongueFollowPoint.localPosition));

                yield return null;
            }
            
            nextNode.GetActiveCell.cellContent.AddObserver(this);
            nextNode.GetActiveCell.cellContent.Interact(false,_colorType);
        }
    }

    private IEnumerator RetractTongueRoutine(float duration,bool success)
    {
        yield return new WaitForSeconds(.3f); 

        _nodesTongueCrossed.Reverse(); //it should be reversed because when tongue retract logic is based on
        
        for (int i = 0; i < _nodesTongueCrossed.Count-1; i++)
        {
            Vector3 nextPos = transform.InverseTransformPoint(_nodesTongueCrossed[i+1].GetActiveCell.cellContent.transform.position);
            _tongueRenderer.tongueFollowPoint.DOLocalMove(nextPos,duration).SetEase(Ease.Linear);

            if (_tongueRenderer.spline.Count>1)
            {
                float elapsedTime = 0f;
                
                if (success) //if tongue retracting succesfully (all berries eaten) berries will be following tongue
                {
                    if (_nodesTongueCrossed[i].GetActiveCell.cellContent is Berry berry) //elements will follow tongue only if berry
                    {
                        Vector3[] path = _tongueRenderer.GetTonguePath();
                        Array.Reverse(path);
                        
                        path = ConvertTonguePositionsToWorldPos(path);
                        
                        //path = ConvertTonguePositionsToWorldPos(path); //convert local line renderer positions to world space
                        float totalDistance = _tongueRenderer.CalculateTotalDistance(path); //calculate total distance current berry to destination

                        float delayBetweenBerries = 1f;
                        float berryDuration = (totalDistance * duration) + (delayBetweenBerries / _nodesTongueCrossed.Count);

                        GameManager.Instance.RemoveBerry(berry);
                        berry.transform.SetParent(this.transform);
                        _nodesTongueCrossed[i].GetActiveCell.Deactivate();

                        berry.BerryEatenVisual(berryDuration-.2f, berryDuration-.1f, path);
                        
                    }
                    else 
                    {
                        _nodesTongueCrossed[i].GetActiveCell.Deactivate(); //also deactivate the arrow cells too if retracting successufully
                    }
                }
                else  //if tongue hit wrong element
                {
                    _nodesTongueCrossed[i].GetActiveCell.cellContent.isInteractable=true;
                }

                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / duration;

                    _tongueRenderer.spline.SetKnot(_tongueRenderer.spline.Count - 1, new BezierKnot(_tongueRenderer.tongueFollowPoint.localPosition));

                    yield return null;
                } 

                _tongueRenderer.spline.RemoveAt(_tongueRenderer.spline.Count - 1);
            }
        }
        
        _nodesTongueCrossed.Clear();
        isInteractable=true;
        _animator.SetTrigger(Constants.AnimationsParameters.Idle);
        LevelManager.Instance.CheckForLevelState();

        if (success)
        {Finish();}
    }
    
        //converts the line renderer positions to world space
    Vector3[] ConvertTonguePositionsToWorldPos(Vector3[] positions) //converts the line renderer positions to world space
    {
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = transform.TransformPoint(positions[i]);
        }
        return positions;
    }

    public void OnBerryEaten(Node currentNode)//logic when continue eating berries (eaten correct berry previously)
    {
        AudioManager.Instance.PlaySound(Resources.Load<AudioClip>(Constants.AudioClips.BerryCollect));
        _nodesTongueCrossed.Add(currentNode);
        currentNode.GetActiveCell.cellContent.isInteractable=false;
        Node nextNode = NodeManager.Instance.GetNeighbor(currentNode,_currentDirection);
        if (nextNode)
        {
            ContinueEat(nextNode);
        }
        else
        {
            StartCoroutine(RetractTongueRoutine(_tongueMoveDurationPerNode,true));
        }
    }
    
    public void OnWrongElement(Node currentNode)//logic when there's a wrong type of cell on way
    {
        _nodesTongueCrossed.Add(currentNode);
        currentNode.GetActiveCell.cellContent.isInteractable=false;
        StartCoroutine(RetractTongueRoutine(_tongueMoveDurationPerNode,false));
    }
    
    public void OnDirectionChange(Node currentNode,Vector3 newDirection)//logic tongue hits true arrow cells
    {
        _nodesTongueCrossed.Add(currentNode);
        _currentDirection = newDirection;
        Node nextNode = NodeManager.Instance.GetNeighbor(currentNode,_currentDirection);
        if (nextNode)
        {
            ContinueEat(nextNode);
        }
    }
    
    public override void Finish() //when this fron eaten all berries successfully
    {
        AudioManager.Instance.PlaySound(Resources.Load<AudioClip>(Constants.AudioClips.BerryEat));
        cell.Deactivate();
    }
}
