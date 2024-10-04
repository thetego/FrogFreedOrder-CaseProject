using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Berry : GameElement
{
    void Start()
    {
        _colorType = cell._cellData.color;
    }
    public override void Interact(bool isUserInteract, ColorType colorType)
    {
        if (isUserInteract||!isInteractable)
        {
            NotifyObservers(false);
            return;
        }

        if (colorType.Equals(_colorType))
        {
            NotifyObservers(true);
            InteractAnim();
        }
        else 
        {
            NotifyObservers(false);
        }
    }

    public void BerryEatenVisual(float delay,float speed, Vector3[] path)
    {
        StartCoroutine(BerryEatenVisualCoroutine(delay,speed,path));
    }
    IEnumerator BerryEatenVisualCoroutine(float delay, float duration, Vector3[] path)
    {   
        transform.DOPath(path,duration,PathType.Linear).SetEase(Ease.Linear);
        yield return new WaitForSeconds(delay);
        transform.DOScale(Vector3.zero,0.5f);
    }
    private void NotifyObservers(bool success)
    {
        foreach (var observer in observers)
        {
            if (success) 
            {
                observer.OnBerryEaten(cell.currentNode);
            }

            else 
            {
                
                observer.OnWrongElement(cell.currentNode);
            }
                
        }
        observers.Clear(); //observers needs to be cleaned after execute to prevent bugs
    }
}
