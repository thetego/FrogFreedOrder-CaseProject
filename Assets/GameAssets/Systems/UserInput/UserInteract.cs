using UnityEngine;

public class UserInteract : MonoBehaviour
{
    void Update()
    {
        HandleRightClick();
    }

    void HandleRightClick()
    {
        if (Input.GetMouseButtonDown(1)||Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Node node = hit.collider.GetComponent<Node>();
                if (node != null && node.GetCells().Count > 0)
                {
                    InteractWithNode(node);
                }
            }
        }
    }

    void InteractWithNode(Node node)
    {
        node.GetActiveCell.cellContent.Interact(true);
    }
}
