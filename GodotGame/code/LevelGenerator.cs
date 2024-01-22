using Godot;
using System;

public class LevelGenerator : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private Spatial _level;
    private bool generated = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!generated)
        {
            generated = true;
            _level = this.GetParent<Spatial>();
            var gen = new System.Random();
            for (int i = 0; i < 10000; i++)
            {
                var randomBox = new CSGBox();
                var id = randomBox.GetInstanceId();
                var x = gen.Next(-1000, 1000);
                var y = gen.Next(-1000, 1000);
                var z = gen.Next(-1000, 1000);
                randomBox.GlobalTranslate(new Vector3(x, 4, z));
                randomBox.UseCollision = true;
                randomBox.SetScript(ResourceLoader.Load("code/Random.gd"));
                var item = (Node)GD.InstanceFromId(id);
                _level.AddChild(item);
                GD.Print("");
            }
        }
    }
}
