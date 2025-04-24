using Godot;

[GlobalClass]
public partial class MeshToCollision : Node3D
{
    [Export] public NodePath MeshInstancePath;

    public override void _Ready()
    {
        var meshInstance = GetNode<MeshInstance3D>(MeshInstancePath);
        var mesh = meshInstance.Mesh;

        if (mesh == null)
        {
            GD.PrintErr("Mesh is null!");
            return;
        }

        var shape = mesh.CreateTrimeshShape();

        var staticBody = new StaticBody3D();
        var collisionShape = new CollisionShape3D();
        collisionShape.Shape = shape;

        staticBody.AddChild(collisionShape);
        AddChild(staticBody);

        staticBody.GlobalTransform = meshInstance.GlobalTransform;

        //GD.Print("Collision shape created successfully.");
    }
}
