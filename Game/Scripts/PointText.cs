using Godot;
using Util.ExtensionMethods;


public class PointText : Node2D
{
    [Export]
    private NodePath _pointVisualPath;
    private Sprite _pointVisualReference;
    public Sprite PointVisualReference
    {
        get { return _pointVisualReference; }
    }

    public override void _Ready()
    {
        SetNodeReferences();
    }

    private void SetNodeReferences()
    {
        _pointVisualReference = GetNode<Sprite>(_pointVisualPath);
    }

    public void OnAnimationFinished(string anim_name)
    {
        if (anim_name != "point_float")
        {
            return;
        }
        this.SafeQueueFree();
    }
}
