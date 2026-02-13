namespace Dragging
{
    public interface IAttackable
    {
    
        string UniqueID { get; }

        Player Owner { get; }

        void ReceiveAttack(string attackerId);
    }
}