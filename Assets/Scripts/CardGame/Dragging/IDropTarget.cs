public interface IDropTarget
{
    bool CanAcceptDrop(DraggingActions dragged);
    void AcceptDrop(DraggingActions dragged);
}