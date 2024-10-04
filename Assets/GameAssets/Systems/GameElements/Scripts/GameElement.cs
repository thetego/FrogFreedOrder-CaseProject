using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class GameElement : MonoBehaviour
{
    [SerializeField]private protected List<IFrogObserver> observers = new List<IFrogObserver>();
    private Vector3 _initialSize;

    public bool isInteractable;
    
    public Cell cell;
    protected ColorType _colorType;

    void Awake()
    {
        Activate();
        isInteractable=false;
    }
    
    public virtual void Interact(bool isUserInteract){}
    
    public virtual void Interact(bool isUserInteract, ColorType colorType){}
    
    public virtual void Finish(){}
    
    public virtual void InvalidInteraction()
    {
        transform.DOShakeScale(.5f);
    }
    
    public void InteractAnim()
    {
        transform.DOPunchScale(transform.localScale*1.2f,.1f,2);
    }
    
    public virtual void AddObserver(IFrogObserver observer)
    {
        observers.Add(observer);
    }

    
    public virtual void RemoveObserver(IFrogObserver observer)
    {
        observers.Remove(observer);
    }
    
    public void Activate ()
    {
        _initialSize=transform.localScale;
        transform.localScale=Vector3.zero;
        transform.DOScale(_initialSize,.5f).SetEase(Ease.Linear).OnComplete(()=>isInteractable=true);
    }
   
    public void Deactivate()
    {
        Destroy(gameObject);
        transform.DOKill();
    }
}
