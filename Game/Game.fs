module Dootverse.Game

open System
open System.Collections.Generic
open Browser.Types
open Doot.Maths.Voxel.Traversal
open Dootverse
open Dootverse.Network
open Dootverse.Models
open Fable.Core
open Browser
open Feliz
open Fable.Core.JsInterop
open Thoth.Json
open Dootverse.Client
open query_pipeline

type Component = struct end
// type Entity = struct end
// type GameWorld(world: world.World) =
//     let entities = Map<Entity, Component list>

type GameObject = { // todo: GameWorld
    id: int
    sprite: threejs.__objects_Sprite.Sprite
    collider: Rapier.collider.Collider
    // typ: Network.EntityType
    entity: GameEntity
}
// type Scene(world: world.World, scene: threejs.__scenes_Scene.Scene) =
    // let world = GameWorld(world)
    // member this.LoadScene (scene: Network.Scene) =
    //     let treesTexture = three.TextureLoader.Create().load "textures/ForestTrees.png"
    //     let rsTreeTexture = three.TextureLoader.Create().load "textures/rs/yewtree.png"
    //     for wall in scene.Walls do
    //         let x, y = wall.Key
    //         this.Scene.AddCube(
    //             true,
    //             { x = 1
    //               y = if Math.Abs x = 100 || Math.Abs y = 100 then 40 else 1
    //               z = 1 },
    //             { x = float x - 0.5
    //               y = if Math.Abs x = 100 || Math.Abs y = 100 then 20 else 0.5
    //               z = float y + 0.5 },
    //             {| map = treesTexture |}
    //         ) |> ignore
    //     for kv in scene.GameObjects do
    //         let m = three.SpriteMaterial.Create(box {| map = rsTreeTexture |} :?> _)
    //         let pos = snd kv.Value
    //         let sprite = three.Sprite.Create m
    //         let collider =
    //             this.World.createCollider(
    //                 RAPIER.ColliderDesc.cuboid(0.1, 1, 0.1),
    //                 this.World.createRigidBody(
    //                     RAPIER.RigidBodyDesc.newStatic()
    //                         .setTranslation(pos.x, pos.y, pos.z)))
    //         sprite.position.x <- pos.x
    //         sprite.position.y <- pos.y
    //         sprite.position.z <- pos.z
    //         let instance = this.ThreeJsScene.add sprite
    //         // todo
    //         ()
            // spriteMap.current.Add(kv.Key, sprite)
            // colliderMap.current.Add(kv.Key, collider)
            // idMap.current.Add(collider, kv.Key)
type Game(world: world.World, width, height, server) =
    // let mutable scene = { Walls = Map.empty; GameObjects = Map.empty }
    let loader = three.TextureLoader.Create()
    let gfxScene =
        let scene = three.Scene.Create()
        scene.background <- box (three.Color.Create(1, 1, 1)) :?> _
        scene
    // let scene = Scene(world, gfxScene)
    let fov, nearClip, farClip = 50, 0.1, 10000
    let camera = three.PerspectiveCamera.Create(fov, width / height, nearClip, farClip)
    let renderer = three.WebGLRenderer.Create()
    let geometry = three.BufferGeometry.Create()
    // let mutable players = Map.empty
    let lines =
        let material = three.LineBasicMaterial.Create(box {| color = 0x888888; vertexColors = true |} :?> _)
        let lines = three.Line.Create(geometry, material)
        gfxScene.add lines |> ignore
        lines.visible <- true
        lines
    let objects = Dictionary()
    let colliderMap = Dictionary()
    let mutable walls = Map.empty
    member val DebugPhysics = false with get, set
    member this.Renderer = renderer
    member this.Camera = camera
    member this.ThreeJsScene = gfxScene
    member this.World = world
    member this.Scene () =
        {
            Walls = walls
            GameObjects = Map.ofSeq (objects |> Seq.map (fun kv ->
                kv.Key, {
                    position = kv.Value.entity.position
                    data = kv.Value.entity.data
                    sprite = kv.Value.entity.sprite
                }
            )) 
        }
    member this.Create (entity: GameEntity) =
        let texture = loader.load (defaultArg entity.sprite "")
        let kv = {| Value = entity.position |}
        let m = three.SpriteMaterial.Create(box {| map = texture |} :?> _)
        let sprite = three.Sprite.Create m
        let collider =
            this.World.createCollider(
                RAPIER.ColliderDesc.cuboid(0.1, 1, 0.1),
                this.World.createRigidBody(RAPIER.RigidBodyDesc.newStatic().setTranslation(
                    kv.Value.x, kv.Value.y, kv.Value.z)))
        sprite.position.x <- kv.Value.x
        sprite.position.y <- kv.Value.y
        sprite.position.z <- kv.Value.z
        let id = objects |> Seq.maxBy _.Key |> _.Key |> (+) 1
        objects[id] <- { id = id; entity = entity; collider = collider; sprite = sprite; }
        id
    member this.Entities = objects :> IReadOnlyDictionary<_,_>
    member this.AddCube(staticPos, size: Network.float3, pos: Network.float3, ?meshProps: obj) =
        let desc = if staticPos then RAPIER.RigidBodyDesc.newStatic() else RAPIER.RigidBodyDesc.newDynamic()
        let rigidBody = world.createRigidBody(desc.setTranslation(pos.Tuple))
        let rapierSize = size / 2.0
        let geometry = three.BoxGeometry.Create(size.x, size.y, size.z)
        let material = three.MeshBasicMaterial.Create(box (if meshProps.IsSome then meshProps.Value else {| color = "blue" |}) :?> _)
        let collider = world.createCollider(RAPIER.ColliderDesc.cuboid(rapierSize.Tuple), rigidBody)
        let mesh = three.Mesh.Create(geometry, material)
        mesh.position.x <- pos.x
        mesh.position.y <- pos.y
        mesh.position.z <- pos.z
        gfxScene.add mesh |> ignore
        rigidBody, collider, mesh
    member this.Raycast(dir) =
        // let dir = RAPIER.Vector3.Create(x, 0, z)
        let ray = RAPIER.Ray.Create(RAPIER.Vector3.Create(camera.position.x, camera.position.y, camera.position.z), dir)
        let raycast = this.World.castRay(ray, 10000, true, QueryFilterFlags.ALL_SHAPES)
        raycast
        
    member this.Fire() =
        let x = -1.0 * Math.Sin camera.rotation.y
        let z = -1.0 * Math.Cos camera.rotation.y
        let dir = RAPIER.Vector3.Create(x, 0, z)
        let ray = RAPIER.Ray.Create(RAPIER.Vector3.Create(camera.position.x, camera.position.y, camera.position.z), dir)
        let raycast = this.World.castRay(ray, 10000, true, QueryFilterFlags.ALL_SHAPES)
        match raycast with
        | Some raycast ->
            let hitPoint = ray.pointAt(raycast.toi)
            // this.AddCube(true, { x = 0.02; y = 0.02; z = 0.02 }, { x = hitPoint.x; y = hitPoint.y; z = hitPoint.z }, {| color = "blue" |})
            // |> ignore
            console.log ("camera position =", camera.position.x, camera.position.y, camera.position.z)
            console.log ("hit point =", hitPoint)
            console.log ("dir = ", x, z)
            console.log raycast
            if colliderMap.ContainsKey raycast.collider then
                let id = colliderMap[raycast.collider]
                let ent = objects[id]
                Some (id, ent)
            else
                None
        | None -> None
    member this.MoveEntity (id, pos: Network.float3) =
        // if not server then console.log id
        match objects.TryGetValue id with
        | true, ent ->
            ent.collider.setTranslation(RAPIER.Vector3.Create(pos.x, pos.y, pos.z))
            ent.sprite.position.x <- pos.x
            ent.sprite.position.y <- pos.y
            ent.sprite.position.z <- pos.z
            objects[id] <- { ent with entity.position = pos }
            // let state = { scene.GameObjects[id] with position = pos }
            // scene <- { scene with GameObjects = scene.GameObjects.Add(id, state) }
        | _ -> ()
    member this.Delete id =
        // scene <- { scene with GameObjects = scene.GameObjects.Remove(id) }
        let collider = objects[id].collider
        let sprite = objects[id].sprite
        objects.Remove id |> ignore
        gfxScene.remove sprite |> ignore
        world.removeCollider (collider, false)
    member this.Load_Scene(world: Network.Scene) =
        // scene <- world
        walls <- world.Walls
        let treesTexture = loader.load "textures/ForestTrees.png"
        let rsTreeTexture = "textures/rs/yewtree.png"
        let zombieStanding = loader.load "textures/rs/zombie_standing.png"
        for wall in world.Walls do
            let x, y = wall.Key
            let _, _, mesh = this.AddCube(
                true,
                { x = 1
                  y = if Math.Abs x = 100 || Math.Abs y = 100 then 40 else 1
                  z = 1 },
                { x = float x - 0.5
                  y = if Math.Abs x = 100 || Math.Abs y = 100 then 20 else 0.5
                  z = float y + 0.5 },
                {| map = treesTexture |}
            )
            ()
        // for (entity, objects) in world.GameObjects |> Map.toArray |> Array.groupBy (snd >> fst) do
        for kv in world.GameObjects do
            let id = kv.Key
            let entity = kv.Value
            // todo: optimize loading assets
            let texture =
                match entity.data with
                | Network.Tree ->
                    loader.load rsTreeTexture
                | Network.Enemy (enemyType) ->
                    loader.load kv.Value.sprite.Value
                | Network.Player playerName ->
                    loader.load "textures/doom/guy.png"
            let kv = {| Key = id, entity; Value = entity.position |}
            let m = three.SpriteMaterial.Create(box {| map = texture |} :?> _)
            let sprite = three.Sprite.Create m
            let collider =
                this.World.createCollider(
                    RAPIER.ColliderDesc.cuboid(0.1, 1, 0.1),
                    this.World.createRigidBody(RAPIER.RigidBodyDesc.newStatic().setTranslation(
                        kv.Value.x, kv.Value.y, kv.Value.z)))
            sprite.position.x <- kv.Value.x
            sprite.position.y <- kv.Value.y
            sprite.position.z <- kv.Value.z
            match entity.data with
            | Network.Enemy _ ->
                sprite.scale.y <- 1
                sprite.scale.x <- 1
                sprite.scale.z <- 1
                sprite.position.y <- 0.25
                let healthbarTexture = loader.load "textures/healthbar.png"
                let healthbar = three.Sprite.Create(three.SpriteMaterial.Create(box {| map = healthbarTexture |} :?> _))
                healthbar.scale.x <- 1.0
                healthbar.scale.y <- 34.0 / 200.0
                healthbar.position.y <- 0.5
                // healthbar.position.x <- kv.Value.x
                // healthbar.position.y <- kv.Value.y
                // healthbar.position.z <- kv.Value.z
                // scene.add healthbar
                sprite.add healthbar
                |> ignore
            | _ -> ()
            let instance = this.ThreeJsScene.add sprite
            objects.Add(fst kv.Key, { entity = entity; id = fst kv.Key; sprite = sprite; collider = collider; })
            colliderMap.Add(collider, fst kv.Key)
    
    
    member this.RenderPhysics () =
        let debugInfo = world.debugRender()
        geometry.setAttribute(!^ "position", U2.Case1 (three.BufferAttribute.Create(box debugInfo.vertices :?> _, 3))) |> ignore
        // geometry.setAttribute(!^ "position", U2.Case1 (three.BufferAttribute.Create(box [| 0f; 0f; 0f; 10f; 10f; 10f |] :?> _, 3))) |> ignore
        geometry.setAttribute(!^ "color", !^ three.BufferAttribute.Create(box (debugInfo.colors.map(fun c -> c)) :?> _, 4)) |> ignore
    member this.Step (time, physicsUpdate, gfxUpdate) =
        world.step()
        physicsUpdate time
        gfxUpdate time
        if this.DebugPhysics then this.RenderPhysics()
        renderer.render(gfxScene, camera)
        
        Engine.endInputFrame ()
    // member this.Update (id, value) =
        // scene <- { scene with GameObjects = scene.GameObjects.Add(id, value) }
    member this.ApplyEvent (event: GameEvent) =
        try
            match event with
            | PlayerJoined (id, name) ->
                let loader = three.TextureLoader.Create()
                let texture = loader.load "textures/doom/guy.png"
                let m = three.SpriteMaterial.Create(box {| map = texture |} :?> _)
                let sprite = three.Sprite.Create m
                sprite.scale.set(0.5, 0.5, 0.5) |> ignore
                let collider =
                    this.World.createCollider(
                        RAPIER.ColliderDesc.cuboid(0.1, 1, 0.1),
                        this.World.createRigidBody(RAPIER.RigidBodyDesc.newStatic().setTranslation(
                            0, 0, 0)))
                gfxScene.add sprite |> ignore
                let data = {
                    position = float3.From sprite.position; sprite = None
                    data = Player {
                        Name = name
                        Health = 100.0
                        Status = true
                        Rotation = 0
                    }
                }
                objects.Add (id, { entity = data; collider = collider; sprite = sprite; id = id; })
                // scene <- { scene with GameObjects = scene.GameObjects.Add(id, { position = float3.From sprite.position; sprite = None; data = Player name }) }
            | EntityDestroyed id ->
                this.Delete id
            | EnemyDamaged id ->
                match objects.TryGetValue id with
                | true, entity ->
                    match entity.entity.data with
                    | Enemy enemy ->
                        // match enemy.typ with
                        // | Zombie state ->
                        let state = { enemy with health = enemy.health - 50.0 }
                        let entity = { entity with entity.data = Enemy state }
                        objects[id] <- entity
                        // scene <- { scene with GameObjects = scene.GameObjects.Add(id, entity) }
                    | _ -> ()
                | _ -> ()
            | PlayerDisconnected id ->
                this.Delete id
            | EntityMoved(id, position) ->
                this.MoveEntity (id, position)
            | PlayerMoved(i, position, rotation) ->
                match objects.TryGetValue i with
                | true, value ->
                    let sprite = value.sprite
                    sprite.position.x <- position.x
                    sprite.position.y <- position.y
                    sprite.position.z <- position.z
                    sprite.rotation.y <- rotation
                    objects[i] <- { value with entity.position = position }
                    // scene <- { scene with GameObjects = scene.GameObjects.Add(i, { scene.GameObjects[i] with position = state.Position }) }
                | _else -> ()
            | EntityUpdated (id, state) ->
                // todo: update sprite and collider
                objects[id] <- { objects[id] with entity = state }
            | PlayerDamaged(id, amount) ->
                match objects.TryGetValue id with
                | true, object ->
                    match object.entity.data with
                    | Player playerState ->
                        // if playerState.Health <= amount then
                            // ()
                        // else
                            // ()
                        objects[id] <- {
                            object with entity.data = Player { playerState with Health = playerState.Health - amount }
                        }
                    | _ -> ()
                | _ -> ()
            | PlayerDown id ->
                match objects.TryGetValue id with
                | true, object ->
                    match object.entity.data with
                    | Player playerState ->
                        objects[id] <- {
                            object with entity.data = Player { playerState with Status = false }
                        }
                    | _ -> ()
                | _ -> ()
                gfxScene.remove objects[id].sprite |> ignore
                // world.removeCollider (objects[id].collider, false)
            | PlayerSpawned (id, position) ->
                // todo: Moving camera and stuff for player
                match objects.TryGetValue id with
                | true, object ->
                    match object.entity.data with
                    | Player playerState ->
                        let state = Player {
                            playerState with
                                Health = 100
                                Status = true
                        }
                        objects[id] <- { object with entity.data = state; entity.position = position }
                        gfxScene.add objects[id].sprite |> ignore
                        objects[id].sprite.position.x <- position.x
                        objects[id].sprite.position.y <- position.y
                        objects[id].sprite.position.z <- position.z
                        objects[id].collider.setTranslation(RAPIER.Vector3.Create(position.Tuple))
                    | _ -> ()
                | _ -> ()
            | UpdateZombie (id, state) ->
                match objects.TryGetValue id with
                | true, object ->
                    match object.entity.data with
                    | Enemy enemy ->
                        match enemy.typ with
                        | Zombie zombie ->
                            objects[id] <- { object with entity.data = Enemy { enemy with typ = Zombie { state = state } } }
                        | _ -> ()
                    | _ -> ()
                | _ -> ()
        with error ->
            console.log error