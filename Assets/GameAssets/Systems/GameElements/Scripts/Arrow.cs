
public class Arrow : GameElement
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
        }
        else 
        {
            NotifyObservers(false);
        }
    }
    private void NotifyObservers(bool success)
    {
        foreach (var observer in observers)
        {
            if (success)
                observer.OnDirectionChange(cell.currentNode,cell._cellData.DetermineDirection());
            else 
                observer.OnWrongElement(cell.currentNode);
        }
        observers.Clear(); //observers needs to be cleaned after execute to prevent bugs
    }
}
