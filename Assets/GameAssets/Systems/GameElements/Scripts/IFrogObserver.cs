using UnityEngine;

public interface IFrogObserver
{
    void OnBerryEaten(Node nextNode);
    void OnWrongElement(Node currentNode);
    void OnDirectionChange(Node nextNode,Vector3 newDirection);
}
