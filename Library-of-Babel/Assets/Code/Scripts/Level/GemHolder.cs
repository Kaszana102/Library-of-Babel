using UnityEngine;

abstract public class GemHolder : MonoBehaviour
{
    public Gem gem = null;
    public bool reserved = false;
    public void PassGemToOtherHolder(GemHolder gemHolder)
    {
        gemHolder.gem = gem;
        gem = null;
        gemHolder.OnPassGem();
    }

    public void ReceiveGem(Gem gem)
    {
        this.gem = gem;
        OnPassGem();
    }

    protected abstract void OnPassGem();

    public bool Occupied()
    {
        return gem != null;
    }
}
