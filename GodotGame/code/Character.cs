using Godot;
using System;

public static class Key
{
    public static bool Pressed(KeyList key)
    {
        return Input.IsKeyPressed((int)key);
    }
}

public class Character : KinematicBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private Vector3 _linearVelocity = Vector3.Zero;
    private Camera _camera;
    private RayCast _raycast;
    [Export] public float cameraSens = 1.0f;
    [Export] public float speed = 10.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _camera = GetNode<Camera>("Camera");
        _raycast = GetNode<RayCast>("RayCast");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionJustPressed("capture_cursor"))
        {
            if (Input.MouseMode == Input.MouseModeEnum.Captured)
                Input.MouseMode = Input.MouseModeEnum.Visible;
            else
                Input.MouseMode = Input.MouseModeEnum.Captured;
        }
        _linearVelocity = Vector3.Zero;
        if (Key.Pressed(KeyList.W))
            _linearVelocity += Vector3.Forward;
        if (Key.Pressed(KeyList.S))
            _linearVelocity += Vector3.Back;
        if (Key.Pressed(KeyList.A))
            _linearVelocity += Vector3.Left;
        if (Key.Pressed(KeyList.D))
            _linearVelocity += Vector3.Right;
        if (Key.Pressed(KeyList.E))
            _linearVelocity += Vector3.Up;
        if (Key.Pressed(KeyList.Q))
            _linearVelocity += Vector3.Down;
        if (Key.Pressed(KeyList.Z))
            this.RotateY(delta * cameraSens);
        if (Key.Pressed(KeyList.X))
            this.RotateY(-delta * cameraSens);

        if (Key.Pressed(KeyList.Space))
        {
            var collider = (StaticBody)_raycast.GetCollider();
            GD.Print("test");
        }
        
        this.MoveAndSlide(this.Transform.basis.Xform(_linearVelocity) * speed);
    }
}
