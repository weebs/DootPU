module Raycast.Compute.ComputeShaders

open Dootverse.WebGPU
open System
open System.Collections.Concurrent
open System.Diagnostics
open System.Reflection
open System.Threading.Tasks
open Dootverse.WebGPU
open Dootverse.WebGPU.Wgsl
open Microsoft.AspNetCore.Components
open Microsoft.JSInterop
open Silk.NET.WebGPU

// type U =
//     type A = class end

open type Wgsl

[<ReflectedDefinition>]
module Shaders =
    type Config =
        { gridSize: int
          widthPixels: int
          heightPixels: int
          cameraX: float32
          cameraY: float32
          cameraZ: float32
          voxelGridScale: float32
          debugX: int
          debugY: int
          blue: int
          purple: int }

    [<Struct>]
    type RaycastResult =
        { distance: float32
          t: int
          color: int
        // resultX: float32; resultY: float32; resultZ: float32
        // deltaX: float32
        // deltaY: float32
        // deltaZ: float32
        // maskX: float32
        // maskY: float32
        // maskZ: float32
        // posX: float32; posY: float32
        // pixelX: int
        // pixelY: int
        }

    [<Struct>]
    type Steps =
        { posX: float32
          posY: float32
          posZ: float32
          voxelX: float32
          voxelY: float32
          voxelZ: float32 }

    [<Struct>]
    type Result =
        { nextPos: vec3f
          voxel: vec3f
          offsetVoxel: vec3f }

    type Raycaster(cfg: Config, grid: int[], output: RaycastResult[], steps: Steps[]) =
        // type Raycaster(cfg: Config, grid: int[], output: RaycastResult[]) =
        let dda (rayPos: vec3f) (rayDir1: vec3f) =
            let rayDir = rayDir1 * 100f
            let mapPos = floor (rayPos + 0f)
            let len = length (rayDir)
            // let mutable deltaDist = abs(vec3(len) / rayDir)
            let mutable deltaDist = abs (1f / normalize (rayDir))

            if deltaDist.x = (1f / 0f) then
                deltaDist.x <- 0.00000000001f

            if deltaDist.y = (1f / 0f) then
                deltaDist.y <- 0.00000000001f

            if deltaDist.z = (1f / 0f) then
                deltaDist.z <- 0.00000000001f

            let mutable fixedDistance = vec3 (0f)
            let epsilon = 0.00000001f

            if abs (rayDir.x) > epsilon then
                // fixedDistance.x <- abs(len / rayDir.x)
                fixedDistance.x <- abs (rayDir.x / len)

            if abs (rayDir.y) > epsilon then
                // fixedDistance.y <- abs(len / rayDir.y)
                fixedDistance.y <- abs (rayDir.y / len)

            if abs (rayDir.z) > epsilon then
                // fixedDistance.z <- abs(len / rayDir.z)
                fixedDistance.z <- abs (rayDir.z / len)

            let rayStep = sign (rayDir)

            let sideDist =
                (sign (rayDir) * (mapPos - rayPos) + (sign (rayDir) * 0.5f) + 0.5f) * deltaDist

            let mask = lessThanEqual (sideDist, min (sideDist.yzx, sideDist.zxy))
            let maskf = vec3 (float32 (mask.x), float32 (mask.y), float32 (mask.z))
            let correctedVector1 = sideDist * fixedDistance * maskf
            let correctedVector = sideDist * fixedDistance * maskf * deltaDist
            let c = sideDist * maskf

            let correctDistance1 =
                max (abs (correctedVector.x), max (abs (correctedVector.y), abs (correctedVector.z)))

            let correctDistance = max (abs (c.x), max (abs (c.y), abs (c.z)))
            let correctedOffset = fixedDistance * correctDistance * rayStep
            let toReturn = rayPos + correctedOffset
            let correctedOffset2 = correctDistance * rayDir / len
            // todo : what about when there's a multi step?

            let next = maskf * rayStep + mapPos
            let nextOffset = next - rayPos
            let pointDistances = nextOffset * fixedDistance * maskf

            let distance =
                max (abs (pointDistances.x), max (abs (pointDistances.y), abs (pointDistances.z)))

            let offset = rayDir * distance
            let scaledOffset = offset / length (rayDir)

            // let nextTileOffset = fixedDistance * ((mapPos + (maskf * rayStep)) - rayPos)
            // let nextTileDistance = max(nextTileOffset.x, max(nextTileOffset.y, nextTileOffset.z))
            // let offsetVec = nextTileDistance * rayDir / length(rayDir)
            let p = rayPos
            let toReturn2 = rayPos + scaledOffset + (rayStep * epsilon * maskf)
            // toReturn
            // { nextPos = toReturn; voxel = floor(mapPos + (maskf * rayStep) * 1.5f) }
            // { nextPos = toReturn; voxel = floor(rayPos + (maskf * rayStep)) }
            { nextPos = toReturn
              // voxel = floor (toReturn + (rayStep * epsilon * maskf))
              voxel = mapPos + (maskf * rayStep)
              offsetVoxel = floor toReturn - mapPos
            // voxel = floor(rayPos + (maskf * rayStep) + (0.5f * maskf * rayStep))
            }
        // member this.raycast (pixelX, pixelY) =
        //     let screenWidthMeters = 1f
        //     let pcX = float32 pixelX / float32 cfg.widthPixels
        //     let pcY = float32 pixelY / float32 cfg.heightPixels
        //     let offsetX = (pcX - 0.5f) * screenWidthMeters
        //     let offsetY = (pcY - 0.5f) * screenWidthMeters
        //     let mutable rayhit = 0
        //     let mutable i = 0
        //     let mutable posX = offsetX + cfg.cameraX
        //     let mutable posY = offsetY + cfg.cameraY
        //     let mutable posZ = 1f + cfg.cameraZ
        //     let maxIterations = 4000
        //     while rayhit = 0 && i < maxIterations &&
        //           int posX < cfg.gridSize &&
        //           int posZ < cfg.gridSize do
        //         i <- i + 1
        //         posX <- (offsetX * 0.005f) + posX
        //         posY <- (offsetY * 0.005f) + posY
        //         posZ <- 0.005f + posZ
        //         let arrayIndex = (int posX) + (int posZ * cfg.gridSize)
        //         if posY > 1f then
        //             i <- maxIterations
        //         elif posY < -1f then
        //             i <- maxIterations
        //         elif grid[arrayIndex] <> 0 then
        //             rayhit <- grid[arrayIndex]
        //     let dx = posX - cfg.cameraX
        //     let dy = posY - cfg.cameraY
        //     {
        //         distance = sqrt((dx * dx) + (dy * dy))
        //         t = i
        //         dirX = offsetY - cfg.cameraX
        //         dirY = 1f
        //         dirZ = 0f
        //         pixelX = pixelX
        //         pixelY = pixelY
        //         posX = 0fa
        //         posY = 0f
        //         resultX = 0f
        //         resultY = 0f
        //         resultZ = 0f
        //     }
        let toIndex (voxel: vec3f) =
            int voxel.x + int voxel.z * cfg.gridSize

        let logStep (i, pos, voxel) =
            steps[i] <-
                { posX = pos.x
                  posY = pos.y
                  posZ = pos.z
                  voxelX = voxel.x
                  voxelY = voxel.y
                  voxelZ = voxel.z }

        let checkPos (pos: vec3f) =
            let voxel = floor pos
            let diff = abs (pos - voxel)
            let epsilon = 0.00000001f
            let xOffset = voxel - vec3 (1f, 0f, 0f)
            let yOffset = voxel - vec3 (0f, 1f, 0f)
            let zOffset = voxel - vec3 (0f, 0f, 1f)

            if grid[toIndex voxel] <> 0 then
                grid[toIndex voxel]
            elif diff.x <= epsilon && grid[toIndex xOffset] <> 0 then
                grid[toIndex xOffset]
            elif diff.y <= epsilon && grid[toIndex yOffset] <> 0 then
                grid[toIndex yOffset]
            elif diff.z <= epsilon && grid[toIndex zOffset] <> 0 then
                grid[toIndex zOffset]
            else
                0

        let fastDda (rayPos: vec3f) (rayDir: vec3f) =
            let mapPos = floor rayPos
            // let deltaDist = abs(1f / normalize(rayDir))
            let quantized = sign (rayDir)
            // let mutable distancePerSide = quantized + rayPos
            // if quantized.x < 0f then
            //     distancePerSide.x <- ceil distancePerSide.x
            // else
            //     distancePerSide.x <- floor distancePerSide.x
            // if quantized.y < 0f then
            //     distancePerSide.y <- ceil distancePerSide.y
            // else
            //     distancePerSide.y <- floor distancePerSide.y
            // if quantized.z < 0f then
            //     distancePerSide.z <- ceil distancePerSide.z
            // else
            //     distancePerSide.z <- floor distancePerSide.z
            let bGreater = greaterThan (quantized, vec3 (0f))
            let bLess = lessThan (quantized, vec3 (0f))

            let greater =
                vec3 (float32 (bGreater.x), float32 (bGreater.y), float32 (bGreater.z))

            let lesser = vec3 (float32 (bLess.x), float32 (bLess.y), float32 (bLess.z))

            let nextVoxel =
                (floor (quantized + rayPos) * greater) + (ceil (quantized + rayPos) * lesser)

            let distancePerSide = abs (nextVoxel - rayPos)
            let closestVoxelDir = sign (rayDir) + mapPos
            // let distancePerSide = abs(closestVoxelDir - rayPos)
            let maxVal = 1000000000f
            let mutable rayLengthPerSide = length (rayDir) / abs (rayDir)

            if rayLengthPerSide.x = 1f / 0f then
                rayLengthPerSide.x <- maxVal

            if rayLengthPerSide.y = 1f / 0f then
                rayLengthPerSide.y <- maxVal

            if rayLengthPerSide.z = 1f / 0f then
                rayLengthPerSide.z <- maxVal

            let sidePerLength = abs (rayDir) / length (rayDir)
            let scaledPerSide = distancePerSide * rayLengthPerSide
            let mask = lessThanEqual (scaledPerSide, min (scaledPerSide.yzx, scaledPerSide.zxy))
            let maskf = vec3 (float32 (mask.x), float32 (mask.y), float32 (mask.z))
            let intermediateResult = scaledPerSide * maskf
            // todo : max
            let toTravel =
                max (intermediateResult.x, max (intermediateResult.y, intermediateResult.z))

            let offset = toTravel * sidePerLength * sign (rayDir)
            let result = rayPos + offset
            let offsetVoxel = floor result - mapPos

            { nextPos = result
              voxel = floor result
              offsetVoxel = offsetVoxel }

        // let closestVoxel = sign(rayDir)
        // if grid[arrayIndex voxel] <> 0 then

        member this.raycast(pixelX, pixelY) =
            let screenWidthMeters = 1f
            let pcX = float32 pixelX / float32 cfg.widthPixels
            let pcY = float32 pixelY / float32 cfg.heightPixels
            let offsetX = (pcX - 0.5f) * screenWidthMeters
            let offsetY = (pcY + 0.5f) * screenWidthMeters

            let dirY =
                ((float32 cfg.heightPixels * 0.5f) - float32 pixelY) / float32 cfg.heightPixels

            let dirX =
                (float32 pixelX - (float32 cfg.widthPixels / 2f)) / float32 cfg.widthPixels

            let maxIndex = cfg.gridSize * cfg.gridSize
            let mutable pos = vec3 (dirX + cfg.cameraX, dirY + cfg.cameraY, 1f + cfg.cameraZ)
            let mutable voxel = floor (pos)
            let mutable deltaDir = vec3 (0f)
            let mutable mask = vec3 (0f)
            let mutable index = toIndex voxel
            let mutable rayhit = -1

            if index < maxIndex && grid[index] <> 0 then
                rayhit <- grid[index]

            let mutable i = 0
            let dir = vec3 (dirX, dirY, 1f)
            let maxIterations = 200

            if pixelX = cfg.debugX && pixelY = cfg.debugY then
                logStep (0, pos, voxel)

            while rayhit = -1
                  && i < maxIterations
                  && int pos.x < cfg.gridSize
                  && int pos.z < cfg.gridSize do
                i <- i + 1
                // let ddaResult = dda pos dir
                let ddaResult = fastDda pos dir
                pos <- ddaResult.nextPos
                voxel <- ddaResult.voxel
                index <- toIndex voxel
                // let newVoxel = floor(pos)
                // let arrayIndex = (int newVoxel.x) + (int newVoxel.z * cfg.gridSize)
                if pixelX = cfg.debugX && pixelY = cfg.debugY then
                    logStep (i, pos, voxel)

                if pos.y >= 1f then
                    i <- maxIterations
                    rayhit <- 0
                elif pos.y <= 0f then
                    i <- maxIterations
                    rayhit <- 0
                // elif checkPos pos <> 0 then
                elif checkPos pos <> 0 then
                    rayhit <- checkPos pos
                    rayhit <- cfg.blue
            // elif index < maxIndex && grid[index] <> 0 then
            // rayhit <- checkPos pos
            let dx = pos.x - cfg.cameraX
            let dy = pos.y - cfg.cameraY

            { distance = sqrt ((dx * dx) + (dy * dy))
              t = rayhit
              color = rayhit
            // resultX = pos.x
            // resultY = pos.y
            // resultZ = pos.z
            // pixelX = pixelX
            // pixelY = pixelY
            // posX = offsetX
            // posY = offsetY
            // deltaX = deltaDir.x
            // deltaY = deltaDir.y
            // deltaZ = deltaDir.z
            // maskX = mask.x
            // maskY = mask.y
            // maskZ = mask.z
            }

        [<Compute; WorkgroupSize(1, 64, 1)>]
        member this.main([<BuiltIn(Builtin'.global_invocation_id)>] globalId: vec3<uint>) =
            let index = int globalId.x + (cfg.widthPixels * int globalId.y)
            let result = this.raycast (int globalId.x, int globalId.y)
            let mutable error = 0

            if index > (cfg.widthPixels * cfg.heightPixels) then
                error <- 1

            output[index] <- result
// if int globalId.x = cfg.debugX then
// output[index] <- { distance = 0f; t = 8675309; color = cfg.purple }
// vec4(0f, 1f, 1f, 1f)
