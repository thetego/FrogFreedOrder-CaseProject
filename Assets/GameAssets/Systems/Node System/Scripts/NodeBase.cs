using DG.Tweening;
using UnityEngine;

public abstract class NodeBase : MonoBehaviour
{
    public bool IsBorderNode()
    {
        return NodeManager.Instance.IsBorderNode(this as Node);
    }

    public void Deactivate()
    {
        transform.DOScale(Vector3.zero,.5f).OnComplete(()=>Destroy(gameObject));
    }
}
