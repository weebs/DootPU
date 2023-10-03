namespace PGA
// Written by a generator written by enki.
// using System;
// using System.Text
open System
open System.Text
// using static PGA.PGA3D; // static variable acces

// namespace PGA
// (* { *)
type PGA3D(?f: float32, ?idx: int) =
    // just for debug and print output, the basis names
    let _mVec: float32[] = Array.zeroCreate 16
    static let _basis = [|  "1"; "e0"; "e1"; "e2"; "e3"; "e01"; "e02"; "e03"; "e12"; "e31"; "e23"; "e021"; "e013"; "e032"; "e123"; "e0123"  |]
    do
        match f, idx with
        | Some value, Some index ->
            _mVec[index] <- value
        | _ -> ()
    let f = defaultArg f 0f
    let idx = defaultArg idx 0

    member this.Item with get index = _mVec[index] and set index value = _mVec[index] <- value
    
    /// <summary>
    /// PGA3D.Reverse : res = ~a
    /// Reverse the order of the basis blades.
    /// </summary>
    static member (~~~) (a: PGA3D) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- a[0]
        res.[1] <- a[1]
        res.[2] <- a[2]
        res.[3] <- a[3]
        res.[4] <- a[4]
        res.[5] <- -a[5]
        res.[6] <- -a[6]
        res.[7] <- -a[7]
        res.[8] <- -a[8]
        res.[9] <- -a[9]
        res.[10] <- -a[10]
        res.[11] <- -a[11]
        res.[12] <- -a[12]
        res.[13] <- -a[13]
        res.[14] <- -a[14]
        res.[15] <- a[15]
        res
    
    static member (>>>) (a: PGA3D, b: PGA3D) =
        a * b * ~~~a
        
    /// <summary>
    /// PGA3D.Dual : res = !a
    /// Poincare duality operator.
    /// </summary>
    static member (!!!) (a: PGA3D) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- a[15]
        res.[1] <- a[14]
        res.[2] <- a[13]
        res.[3] <- a[12]
        res.[4] <- a[11]
        res.[5] <- a[10]
        res.[6] <- a[9]
        res.[7] <- a[8]
        res.[8] <- a[7]
        res.[9] <- a[6]
        res.[10] <- a[5]
        res.[11] <- a[4]
        res.[12] <- a[3]
        res.[13] <- a[2]
        res.[14] <- a[1]
        res.[15] <- a[0]
        res
    (* } *)
    
    /// <summary>
    /// PGA3D.Conjugate : res = a.Conjugate()
    /// Clifford Conjugation
    /// </summary>
    member this.Conjugate () =
    (* { *)
        let res = new PGA3D();
        res.[0] <- this[0]
        res.[1] <- -this[1]
        res.[2] <- -this[2]
        res.[3] <- -this[3]
        res.[4] <- -this[4]
        res.[5] <- -this[5]
        res.[6] <- -this[6]
        res.[7] <- -this[7]
        res.[8] <- -this[8]
        res.[9] <- -this[9]
        res.[10] <- -this[10]
        res.[11] <- this[11]
        res.[12] <- this[12]
        res.[13] <- this[13]
        res.[14] <- this[14]
        res.[15] <- this[15]
        res
    (* } *)
    
    /// <summary>
    /// PGA3D.Involute : res = a.Involute()
    /// Main involution
    /// </summary>
    member this.Involute () =
    (* { *)
        let res = new PGA3D();
        res.[0] <- this[0]
        res.[1] <- -this[1]
        res.[2] <- -this[2]
        res.[3] <- -this[3]
        res.[4] <- -this[4]
        res.[5] <- this[5]
        res.[6] <- this[6]
        res.[7] <- this[7]
        res.[8] <- this[8]
        res.[9] <- this[9]
        res.[10] <- this[10]
        res.[11] <- -this[11]
        res.[12] <- -this[12]
        res.[13] <- -this[13]
        res.[14] <- -this[14]
        res.[15] <- this[15]
        res
// module PGA3D =    
    /// <summary>
    /// PGA3D.Mul : res = a * b
    /// The geometric product.
    /// </summary>
    static member mult (a: PGA3D) (b: PGA3D) =
        let res = new PGA3D();
        res[0] <- b[0] * a[0] + b[2] * a[2] + b[3] * a[3] + b[4] * a[4] - b[8] * a[8] - b[9] * a[9] - b[10] * a[10] - b[14] * a[14]
        res[1] <- b[1] * a[0] + b[0] * a[1] - b[5] * a[2] - b[6] * a[3] - b[7] * a[4] + b[2] * a[5] + b[3] * a[6] + b[4] * a[7] + b[11] * a[8] + b[12] * a[9] + b[13] * a[10] + b[8] * a[11] + b[9] * a[12] + b[10] * a[13] + b[15] * a[14] - b[14] * a[15]
        res.[2] <- b[2] * a[0] + b[0] * a[2] - b[8] * a[3] + b[9] * a[4] + b[3] * a[8] - b[4] * a[9] - b[14] * a[10] - b[10] * a[14]
        res.[3] <- b[3] * a[0] + b[8] * a[2] + b[0] * a[3] - b[10] * a[4] - b[2] * a[8] - b[14] * a[9] + b[4] * a[10] - b[9] * a[14]
        res.[4] <- b[4] * a[0] - b[9] * a[2] + b[10] * a[3] + b[0] * a[4] - b[14] * a[8] + b[2] * a[9] - b[3] * a[10] - b[8] * a[14]
        res.[5] <- b[5] * a[0] + b[2] * a[1] - b[1] * a[2] - b[11] * a[3] + b[12] * a[4] + b[0] * a[5] - b[8] * a[6] + b[9] * a[7] + b[6] * a[8] - b[7] * a[9] - b[15] * a[10] - b[3] * a[11] + b[4] * a[12] + b[14] * a[13] - b[13] * a[14] - b[10] * a[15]
        res.[6] <- b[6] * a[0] + b[3] * a[1] + b[11] * a[2] - b[1] * a[3] - b[13] * a[4] + b[8] * a[5] + b[0] * a[6] - b[10] * a[7] - b[5] * a[8] - b[15] * a[9] + b[7] * a[10] + b[2] * a[11] + b[14] * a[12] - b[4] * a[13] - b[12] * a[14] - b[9] * a[15]
        res.[7] <- b[7] * a[0] + b[4] * a[1] - b[12] * a[2] + b[13] * a[3] - b[1] * a[4] - b[9] * a[5] + b[10] * a[6] + b[0] * a[7] - b[15] * a[8] + b[5] * a[9] - b[6] * a[10] + b[14] * a[11] - b[2] * a[12] + b[3] * a[13] - b[11] * a[14] - b[8] * a[15]
        res.[8] <- b[8] * a[0] + b[3] * a[2] - b[2] * a[3] + b[14] * a[4] + b[0] * a[8] + b[10] * a[9] - b[9] * a[10] + b[4] * a[14]
        res.[9] <- b[9] * a[0] - b[4] * a[2] + b[14] * a[3] + b[2] * a[4] - b[10] * a[8] + b[0] * a[9] + b[8] * a[10] + b[3] * a[14]
        res.[10] <- b[10] * a[0] + b[14] * a[2] + b[4] * a[3] - b[3] * a[4] + b[9] * a[8] - b[8] * a[9] + b[0] * a[10] + b[2] * a[14]
        res.[11] <- b[11] * a[0] - b[8] * a[1] + b[6] * a[2] - b[5] * a[3] + b[15] * a[4] - b[3] * a[5] + b[2] * a[6] - b[14] * a[7] - b[1] * a[8] + b[13] * a[9] - b[12] * a[10] + b[0] * a[11] + b[10] * a[12] - b[9] * a[13] + b[7] * a[14] - b[4] * a[15]
        res.[12] <- b[12] * a[0] - b[9] * a[1] - b[7] * a[2] + b[15] * a[3] + b[5] * a[4] + b[4] * a[5] - b[14] * a[6] - b[2] * a[7] - b[13] * a[8] - b[1] * a[9] + b[11] * a[10] - b[10] * a[11] + b[0] * a[12] + b[8] * a[13] + b[6] * a[14] - b[3] * a[15]
        res.[13] <- b[13] * a[0] - b[10] * a[1] + b[15] * a[2] + b[7] * a[3] - b[6] * a[4] - b[14] * a[5] - b[4] * a[6] + b[3] * a[7] + b[12] * a[8] - b[11] * a[9] - b[1] * a[10] + b[9] * a[11] - b[8] * a[12] + b[0] * a[13] + b[5] * a[14] - b[2] * a[15]
        res.[14] <- b[14] * a[0] + b[10] * a[2] + b[9] * a[3] + b[8] * a[4] + b[4] * a[8] + b[3] * a[9] + b[2] * a[10] + b[0] * a[14]
        res.[15] <- b[15] * a[0] + b[14] * a[1] + b[13] * a[2] + b[12] * a[3] + b[11] * a[4] + b[10] * a[5] + b[9] * a[6] + b[8] * a[7] + b[7] * a[8] + b[6] * a[9] + b[5] * a[10] - b[4] * a[11] - b[3] * a[12] - b[2] * a[13] - b[1] * a[14] + b[0] * a[15]
        res
    
    /// <summary>
    /// PGA3D.Wedge : res = a ^^^ b
    /// The outer product. (MEET)
    /// </summary>
    static member (^^^) (a: PGA3D, b: PGA3D) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- b[0] * a[0]
        res.[1] <- b[1] * a[0] + b[0] * a[1]
        res.[2] <- b[2] * a[0] + b[0] * a[2]
        res.[3] <- b[3] * a[0] + b[0] * a[3]
        res.[4] <- b[4] * a[0] + b[0] * a[4]
        res.[5] <- b[5] * a[0] + b[2] * a[1] - b[1] * a[2] + b[0] * a[5]
        res.[6] <- b[6] * a[0] + b[3] * a[1] - b[1] * a[3] + b[0] * a[6]
        res.[7] <- b[7] * a[0] + b[4] * a[1] - b[1] * a[4] + b[0] * a[7]
        res.[8] <- b[8] * a[0] + b[3] * a[2] - b[2] * a[3] + b[0] * a[8]
        res.[9] <- b[9] * a[0] - b[4] * a[2] + b[2] * a[4] + b[0] * a[9]
        res.[10] <- b[10] * a[0] + b[4] * a[3] - b[3] * a[4] + b[0] * a[10]
        res.[11] <- b[11] * a[0] - b[8] * a[1] + b[6] * a[2] - b[5] * a[3] - b[3] * a[5] + b[2] * a[6] - b[1] * a[8] + b[0] * a[11]
        res.[12] <- b[12] * a[0] - b[9] * a[1] - b[7] * a[2] + b[5] * a[4] + b[4] * a[5] - b[2] * a[7] - b[1] * a[9] + b[0] * a[12]
        res.[13] <- b[13] * a[0] - b[10] * a[1] + b[7] * a[3] - b[6] * a[4] - b[4] * a[6] + b[3] * a[7] - b[1] * a[10] + b[0] * a[13]
        res.[14] <- b[14] * a[0] + b[10] * a[2] + b[9] * a[3] + b[8] * a[4] + b[4] * a[8] + b[3] * a[9] + b[2] * a[10] + b[0] * a[14]
        res.[15] <- b[15] * a[0] + b[14] * a[1] + b[13] * a[2] + b[12] * a[3] + b[11] * a[4] + b[10] * a[5] + b[9] * a[6] + b[8] * a[7] + b[7] * a[8] + b[6] * a[9] + b[5] * a[10] - b[4] * a[11] - b[3] * a[12] - b[2] * a[13] - b[1] * a[14] + b[0] * a[15]
        res
    (* } *)
    
    /// <summary>
    /// PGA3D.Vee : res = a &&& b
    /// The regressive product. (JOIN)
    /// </summary>
    static member (&&&) (a: PGA3D, b: PGA3D) =
    (* { *)
        let res = new PGA3D();
        res.[15] <- 1f * (a[15] * b[15]);
        res.[14] <- -1f * (a[14] * -1f * b[15] + a[15] * b[14] * -1f);
        res.[13] <- -1f * (a[13] * -1f * b[15] + a[15] * b[13] * -1f);
        res.[12] <- -1f * (a[12] * -1f * b[15] + a[15] * b[12] * -1f);
        res.[11] <- -1f * (a[11] * -1f * b[15] + a[15] * b[11] * -1f);
        res.[10] <- 1f * (a[10] * b[15] + a[13] * -1f * b[14] * -1f - a[14] * -1f * b[13] * -1f + a[15] * b[10]);
        res.[9] <- 1f * (a[9] * b[15] + a[12] * -1f * b[14] * -1f - a[14] * -1f * b[12] * -1f + a[15] * b[9]);
        res.[8] <- 1f * (a[8] * b[15] + a[11] * -1f * b[14] * -1f - a[14] * -1f * b[11] * -1f + a[15] * b[8]);
        res.[7] <- 1f * (a[7] * b[15] + a[12] * -1f * b[13] * -1f - a[13] * -1f * b[12] * -1f + a[15] * b[7]);
        res.[6] <- 1f * (a[6] * b[15] - a[11] * -1f * b[13] * -1f + a[13] * -1f * b[11] * -1f + a[15] * b[6]);
        res.[5] <- 1f * (a[5] * b[15] + a[11] * -1f * b[12] * -1f - a[12] * -1f * b[11] * -1f + a[15] * b[5]);
        res.[4] <- 1f * (a[4] * b[15] - a[7] * b[14] * -1f + a[9] * b[13] * -1f - a[10] * b[12] * -1f - a[12] * -1f * b[10] + a[13] * -1f * b[9] - a[14] * -1f * b[7] + a[15] * b[4]);
        res.[3] <- 1f * (a[3] * b[15] - a[6] * b[14] * -1f - a[8] * b[13] * -1f + a[10] * b[11] * -1f + a[11] * -1f * b[10] - a[13] * -1f * b[8] - a[14] * -1f * b[6] + a[15] * b[3]);
        res.[2] <- 1f * (a[2] * b[15] - a[5] * b[14] * -1f + a[8] * b[12] * -1f - a[9] * b[11] * -1f - a[11] * -1f * b[9] + a[12] * -1f * b[8] - a[14] * -1f * b[5] + a[15] * b[2]);
        res.[1] <- 1f * (a[1] * b[15] + a[5] * b[13] * -1f + a[6] * b[12] * -1f + a[7] * b[11] * -1f + a[11] * -1f * b[7] + a[12] * -1f * b[6] + a[13] * -1f * b[5] + a[15] * b[1]);
        res.[0] <- 1f * (a[0] * b[15] + a[1] * b[14] * -1f + a[2] * b[13] * -1f + a[3] * b[12] * -1f + a[4] * b[11] * -1f + a[5] * b[10] + a[6] * b[9] + a[7] * b[8] + a[8] * b[7] + a[9] * b[6] + a[10] * b[5] - a[11] * -1f * b[4] - a[12] * -1f * b[3] - a[13] * -1f * b[2] - a[14] * -1f * b[1] + a[15] * b[0]);
        res
    // (* } *)
    //
    // static member op_Amp(a: PGA3D, b: PGA3D) =
    // // (* { *)
        // a &&& b;
    // // (* } *)
    //
    /// <summary>
    /// PGA3D.Dot : res = a | b
    /// The inner product.
    /// </summary>
    static member (|||) (a: PGA3D, b: PGA3D) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- b[0] * a[0] + b[2] * a[2] + b[3] * a[3] + b[4] * a[4] - b[8] * a[8] - b[9] * a[9] - b[10] * a[10] - b[14] * a[14]
        res.[1] <- b[1] * a[0] + b[0] * a[1] - b[5] * a[2] - b[6] * a[3] - b[7] * a[4] + b[2] * a[5] + b[3] * a[6] + b[4] * a[7] + b[11] * a[8] + b[12] * a[9] + b[13] * a[10] + b[8] * a[11] + b[9] * a[12] + b[10] * a[13] + b[15] * a[14] - b[14] * a[15]
        res.[2] <- b[2] * a[0] + b[0] * a[2] - b[8] * a[3] + b[9] * a[4] + b[3] * a[8] - b[4] * a[9] - b[14] * a[10] - b[10] * a[14]
        res.[3] <- b[3] * a[0] + b[8] * a[2] + b[0] * a[3] - b[10] * a[4] - b[2] * a[8] - b[14] * a[9] + b[4] * a[10] - b[9] * a[14]
        res.[4] <- b[4] * a[0] - b[9] * a[2] + b[10] * a[3] + b[0] * a[4] - b[14] * a[8] + b[2] * a[9] - b[3] * a[10] - b[8] * a[14]
        res.[5] <- b[5] * a[0] - b[11] * a[3] + b[12] * a[4] + b[0] * a[5] - b[15] * a[10] - b[3] * a[11] + b[4] * a[12] - b[10] * a[15]
        res.[6] <- b[6] * a[0] + b[11] * a[2] - b[13] * a[4] + b[0] * a[6] - b[15] * a[9] + b[2] * a[11] - b[4] * a[13] - b[9] * a[15]
        res.[7] <- b[7] * a[0] - b[12] * a[2] + b[13] * a[3] + b[0] * a[7] - b[15] * a[8] - b[2] * a[12] + b[3] * a[13] - b[8] * a[15]
        res.[8] <- b[8] * a[0] + b[14] * a[4] + b[0] * a[8] + b[4] * a[14]
        res.[9] <- b[9] * a[0] + b[14] * a[3] + b[0] * a[9] + b[3] * a[14]
        res.[10] <- b[10] * a[0] + b[14] * a[2] + b[0] * a[10] + b[2] * a[14]
        res.[11] <- b[11] * a[0] + b[15] * a[4] + b[0] * a[11] - b[4] * a[15]
        res.[12] <- b[12] * a[0] + b[15] * a[3] + b[0] * a[12] - b[3] * a[15]
        res.[13] <- b[13] * a[0] + b[15] * a[2] + b[0] * a[13] - b[2] * a[15]
        res.[14] <- b[14] * a[0] + b[0] * a[14]
        res.[15] <- b[15] * a[0] + b[0] * a[15]
        res
    (* } *)
    //
    /// <summary>
    /// PGA3D.Add : res = a + b
    /// Multivector addition
    /// </summary>
    static member (+) (a: PGA3D, b: PGA3D) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- a[0] + b[0]
        res.[1] <- a[1] + b[1]
        res.[2] <- a[2] + b[2]
        res.[3] <- a[3] + b[3]
        res.[4] <- a[4] + b[4]
        res.[5] <- a[5] + b[5]
        res.[6] <- a[6] + b[6]
        res.[7] <- a[7] + b[7]
        res.[8] <- a[8] + b[8]
        res.[9] <- a[9] + b[9]
        res.[10] <- a[10] + b[10]
        res.[11] <- a[11] + b[11]
        res.[12] <- a[12] + b[12]
        res.[13] <- a[13] + b[13]
        res.[14] <- a[14] + b[14]
        res.[15] <- a[15] + b[15]
        res
    (* } *)
    
    
    // /// <summary>
    // /// PGA3D.Vee : res = a &&& b
    // /// The regressive product. (JOIN)
    // /// </summary>
    // static member op_Amp (a: PGA3D, b: PGA3D) =
    // (* { *)
    //     let res = new PGA3D();
    //     res.[15] <- 1f * (a[15] * b[15]);
    //     res.[14] <- -1f * (a[14] * -1f * b[15] + a[15] * b[14] * -1f);
    //     res.[13] <- -1f * (a[13] * -1f * b[15] + a[15] * b[13] * -1f);
    //     res.[12] <- -1f * (a[12] * -1f * b[15] + a[15] * b[12] * -1f);
    //     res.[11] <- -1f * (a[11] * -1f * b[15] + a[15] * b[11] * -1f);
    //     res.[10] <- 1f * (a[10] * b[15] + a[13] * -1f * b[14] * -1f - a[14] * -1f * b[13] * -1f + a[15] * b[10]);
    //     res.[9] <- 1f * (a[9] * b[15] + a[12] * -1f * b[14] * -1f - a[14] * -1f * b[12] * -1f + a[15] * b[9]);
    //     res.[8] <- 1f * (a[8] * b[15] + a[11] * -1f * b[14] * -1f - a[14] * -1f * b[11] * -1f + a[15] * b[8]);
    //     res.[7] <- 1f * (a[7] * b[15] + a[12] * -1f * b[13] * -1f - a[13] * -1f * b[12] * -1f + a[15] * b[7]);
    //     res.[6] <- 1f * (a[6] * b[15] - a[11] * -1f * b[13] * -1f + a[13] * -1f * b[11] * -1f + a[15] * b[6]);
    //     res.[5] <- 1f * (a[5] * b[15] + a[11] * -1f * b[12] * -1f - a[12] * -1f * b[11] * -1f + a[15] * b[5]);
    //     res.[4] <- 1f * (a[4] * b[15] - a[7] * b[14] * -1f + a[9] * b[13] * -1f - a[10] * b[12] * -1f - a[12] * -1f * b[10] + a[13] * -1f * b[9] - a[14] * -1f * b[7] + a[15] * b[4]);
    //     res.[3] <- 1f * (a[3] * b[15] - a[6] * b[14] * -1f - a[8] * b[13] * -1f + a[10] * b[11] * -1f + a[11] * -1f * b[10] - a[13] * -1f * b[8] - a[14] * -1f * b[6] + a[15] * b[3]);
    //     res.[2] <- 1f * (a[2] * b[15] - a[5] * b[14] * -1f + a[8] * b[12] * -1f - a[9] * b[11] * -1f - a[11] * -1f * b[9] + a[12] * -1f * b[8] - a[14] * -1f * b[5] + a[15] * b[2]);
    //     res.[1] <- 1f * (a[1] * b[15] + a[5] * b[13] * -1f + a[6] * b[12] * -1f + a[7] * b[11] * -1f + a[11] * -1f * b[7] + a[12] * -1f * b[6] + a[13] * -1f * b[5] + a[15] * b[1]);
    //     res.[0] <- 1f * (a[0] * b[15] + a[1] * b[14] * -1f + a[2] * b[13] * -1f + a[3] * b[12] * -1f + a[4] * b[11] * -1f + a[5] * b[10] + a[6] * b[9] + a[7] * b[8] + a[8] * b[7] + a[9] * b[6] + a[10] * b[5] - a[11] * -1f * b[4] - a[12] * -1f * b[3] - a[13] * -1f * b[2] - a[14] * -1f * b[1] + a[15] * b[0]);
    //     res
    // (* } *)
    //
    // // static member op_Amp(a: PGA3D, b: PGA3D) =
    // // (* { *)
    // //     = a &&& b;
    // // (* } *)
    //
    /// <summary>
    /// PGA3D.Dot : res = a | b
    /// The inner product.
    /// </summary>
    // static member (|||) (a: PGA3D, b: PGA3D) =
    // (* { *)
    //     let res = new PGA3D();
    //     res.[0] <- b[0] * a[0] + b[2] * a[2] + b[3] * a[3] + b[4] * a[4] - b[8] * a[8] - b[9] * a[9] - b[10] * a[10] - b[14] * a[14]
    //     res.[1] <- b[1] * a[0] + b[0] * a[1] - b[5] * a[2] - b[6] * a[3] - b[7] * a[4] + b[2] * a[5] + b[3] * a[6] + b[4] * a[7] + b[11] * a[8] + b[12] * a[9] + b[13] * a[10] + b[8] * a[11] + b[9] * a[12] + b[10] * a[13] + b[15] * a[14] - b[14] * a[15]
    //     res.[2] <- b[2] * a[0] + b[0] * a[2] - b[8] * a[3] + b[9] * a[4] + b[3] * a[8] - b[4] * a[9] - b[14] * a[10] - b[10] * a[14]
    //     res.[3] <- b[3] * a[0] + b[8] * a[2] + b[0] * a[3] - b[10] * a[4] - b[2] * a[8] - b[14] * a[9] + b[4] * a[10] - b[9] * a[14]
    //     res.[4] <- b[4] * a[0] - b[9] * a[2] + b[10] * a[3] + b[0] * a[4] - b[14] * a[8] + b[2] * a[9] - b[3] * a[10] - b[8] * a[14]
    //     res.[5] <- b[5] * a[0] - b[11] * a[3] + b[12] * a[4] + b[0] * a[5] - b[15] * a[10] - b[3] * a[11] + b[4] * a[12] - b[10] * a[15]
    //     res.[6] <- b[6] * a[0] + b[11] * a[2] - b[13] * a[4] + b[0] * a[6] - b[15] * a[9] + b[2] * a[11] - b[4] * a[13] - b[9] * a[15]
    //     res.[7] <- b[7] * a[0] - b[12] * a[2] + b[13] * a[3] + b[0] * a[7] - b[15] * a[8] - b[2] * a[12] + b[3] * a[13] - b[8] * a[15]
    //     res.[8] <- b[8] * a[0] + b[14] * a[4] + b[0] * a[8] + b[4] * a[14]
    //     res.[9] <- b[9] * a[0] + b[14] * a[3] + b[0] * a[9] + b[3] * a[14]
    //     res.[10] <- b[10] * a[0] + b[14] * a[2] + b[0] * a[10] + b[2] * a[14]
    //     res.[11] <- b[11] * a[0] + b[15] * a[4] + b[0] * a[11] - b[4] * a[15]
    //     res.[12] <- b[12] * a[0] + b[15] * a[3] + b[0] * a[12] - b[3] * a[15]
    //     res.[13] <- b[13] * a[0] + b[15] * a[2] + b[0] * a[13] - b[2] * a[15]
    //     res.[14] <- b[14] * a[0] + b[0] * a[14]
    //     res.[15] <- b[15] * a[0] + b[0] * a[15]
    //     res
    (* } *)
    
    /// <summary>
    /// PGA3D.Add : res = a + b
    /// Multivector addition
    /// </summary>
    // static member (+) (a: PGA3D, b: PGA3D) =
    // (* { *)
    //     let res = new PGA3D();
    //     res.[0] <- a[0] + b[0]
    //     res.[1] <- a[1] + b[1]
    //     res.[2] <- a[2] + b[2]
    //     res.[3] <- a[3] + b[3]
    //     res.[4] <- a[4] + b[4]
    //     res.[5] <- a[5] + b[5]
    //     res.[6] <- a[6] + b[6]
    //     res.[7] <- a[7] + b[7]
    //     res.[8] <- a[8] + b[8]
    //     res.[9] <- a[9] + b[9]
    //     res.[10] <- a[10] + b[10]
    //     res.[11] <- a[11] + b[11]
    //     res.[12] <- a[12] + b[12]
    //     res.[13] <- a[13] + b[13]
    //     res.[14] <- a[14] + b[14]
    //     res.[15] <- a[15] + b[15]
    //     res
    (* } *)
    
    /// <summary>
    /// PGA3D.Sub : res = a - b
    /// Multivector subtraction
    /// </summary>
    static member (-) (a: PGA3D, b: PGA3D) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- a[0] - b[0]
        res.[1] <- a[1] - b[1]
        res.[2] <- a[2] - b[2]
        res.[3] <- a[3] - b[3]
        res.[4] <- a[4] - b[4]
        res.[5] <- a[5] - b[5]
        res.[6] <- a[6] - b[6]
        res.[7] <- a[7] - b[7]
        res.[8] <- a[8] - b[8]
        res.[9] <- a[9] - b[9]
        res.[10] <- a[10] - b[10]
        res.[11] <- a[11] - b[11]
        res.[12] <- a[12] - b[12]
        res.[13] <- a[13] - b[13]
        res.[14] <- a[14] - b[14]
        res.[15] <- a[15] - b[15]
        res
    (* } *)
    /// <summary>
    /// PGA3D.Mul : res = a * b
    /// The geometric product.
    /// </summary>
    static member (*) (a: PGA3D, b: PGA3D) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- b[0]*a[0]+b[2]*a[2]+b[3]*a[3]+b[4]*a[4]-b[8]*a[8]-b[9]*a[9]-b[10]*a[10]-b[14]*a[14]
        res.[1] <- b[1]*a[0]+b[0]*a[1]-b[5]*a[2]-b[6]*a[3]-b[7]*a[4]+b[2]*a[5]+b[3]*a[6]+b[4]*a[7]+b[11]*a[8]+b[12]*a[9]+b[13]*a[10]+b[8]*a[11]+b[9]*a[12]+b[10]*a[13]+b[15]*a[14]-b[14]*a[15]
        res.[2] <- b[2]*a[0]+b[0]*a[2]-b[8]*a[3]+b[9]*a[4]+b[3]*a[8]-b[4]*a[9]-b[14]*a[10]-b[10]*a[14]
        res.[3] <- b[3]*a[0]+b[8]*a[2]+b[0]*a[3]-b[10]*a[4]-b[2]*a[8]-b[14]*a[9]+b[4]*a[10]-b[9]*a[14]
        res.[4] <- b[4]*a[0]-b[9]*a[2]+b[10]*a[3]+b[0]*a[4]-b[14]*a[8]+b[2]*a[9]-b[3]*a[10]-b[8]*a[14]
        res.[5] <- b[5]*a[0]+b[2]*a[1]-b[1]*a[2]-b[11]*a[3]+b[12]*a[4]+b[0]*a[5]-b[8]*a[6]+b[9]*a[7]+b[6]*a[8]-b[7]*a[9]-b[15]*a[10]-b[3]*a[11]+b[4]*a[12]+b[14]*a[13]-b[13]*a[14]-b[10]*a[15]
        res.[6] <- b[6]*a[0]+b[3]*a[1]+b[11]*a[2]-b[1]*a[3]-b[13]*a[4]+b[8]*a[5]+b[0]*a[6]-b[10]*a[7]-b[5]*a[8]-b[15]*a[9]+b[7]*a[10]+b[2]*a[11]+b[14]*a[12]-b[4]*a[13]-b[12]*a[14]-b[9]*a[15]
        res.[7] <- b[7]*a[0]+b[4]*a[1]-b[12]*a[2]+b[13]*a[3]-b[1]*a[4]-b[9]*a[5]+b[10]*a[6]+b[0]*a[7]-b[15]*a[8]+b[5]*a[9]-b[6]*a[10]+b[14]*a[11]-b[2]*a[12]+b[3]*a[13]-b[11]*a[14]-b[8]*a[15]
        res.[8] <- b[8]*a[0]+b[3]*a[2]-b[2]*a[3]+b[14]*a[4]+b[0]*a[8]+b[10]*a[9]-b[9]*a[10]+b[4]*a[14]
        res.[9] <- b[9]*a[0]-b[4]*a[2]+b[14]*a[3]+b[2]*a[4]-b[10]*a[8]+b[0]*a[9]+b[8]*a[10]+b[3]*a[14]
        res.[10] <- b[10]*a[0]+b[14]*a[2]+b[4]*a[3]-b[3]*a[4]+b[9]*a[8]-b[8]*a[9]+b[0]*a[10]+b[2]*a[14]
        res.[11] <- b[11]*a[0]-b[8]*a[1]+b[6]*a[2]-b[5]*a[3]+b[15]*a[4]-b[3]*a[5]+b[2]*a[6]-b[14]*a[7]-b[1]*a[8]+b[13]*a[9]-b[12]*a[10]+b[0]*a[11]+b[10]*a[12]-b[9]*a[13]+b[7]*a[14]-b[4]*a[15]
        res.[12] <- b[12]*a[0]-b[9]*a[1]-b[7]*a[2]+b[15]*a[3]+b[5]*a[4]+b[4]*a[5]-b[14]*a[6]-b[2]*a[7]-b[13]*a[8]-b[1]*a[9]+b[11]*a[10]-b[10]*a[11]+b[0]*a[12]+b[8]*a[13]+b[6]*a[14]-b[3]*a[15]
        res.[13] <- b[13]*a[0]-b[10]*a[1]+b[15]*a[2]+b[7]*a[3]-b[6]*a[4]-b[14]*a[5]-b[4]*a[6]+b[3]*a[7]+b[12]*a[8]-b[11]*a[9]-b[1]*a[10]+b[9]*a[11]-b[8]*a[12]+b[0]*a[13]+b[5]*a[14]-b[2]*a[15]
        res.[14] <- b[14]*a[0]+b[10]*a[2]+b[9]*a[3]+b[8]*a[4]+b[4]*a[8]+b[3]*a[9]+b[2]*a[10]+b[0]*a[14]
        res.[15] <- b[15]*a[0]+b[14]*a[1]+b[13]*a[2]+b[12]*a[3]+b[11]*a[4]+b[10]*a[5]+b[9]*a[6]+b[8]*a[7]+b[7]*a[8]+b[6]*a[9]+b[5]*a[10]-b[4]*a[11]-b[3]*a[12]-b[2]*a[13]-b[1]*a[14]+b[0]*a[15]
        res
    
    /// <summary>
    /// PGA3D.smul : res = a * b
    /// scalar/multivector multiplication
    /// </summary>
    static member (*) (a: float32, b: PGA3D) =
    (* { *) 
        let res = new PGA3D();
        res.[0] <- a * b[0]
        res.[1] <- a * b[1]
        res.[2] <- a * b[2]
        res.[3] <- a * b[3]
        res.[4] <- a * b[4]
        res.[5] <- a * b[5]
        res.[6] <- a * b[6]
        res.[7] <- a * b[7]
        res.[8] <- a * b[8]
        res.[9] <- a * b[9]
        res.[10] <- a * b[10]
        res.[11] <- a * b[11]
        res.[12] <- a * b[12]
        res.[13] <- a * b[13]
        res.[14] <- a * b[14]
        res.[15] <- a * b[15]
        res
    (* } *)
    
    /// <summary>
    /// PGA3D.muls : res = a * b
    /// multivector/scalar multiplication
    /// </summary>
    static member (*) (a: PGA3D, b) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- a[0] * b;
        res.[1] <- a[1] * b;
        res.[2] <- a[2] * b;
        res.[3] <- a[3] * b;
        res.[4] <- a[4] * b;
        res.[5] <- a[5] * b;
        res.[6] <- a[6] * b;
        res.[7] <- a[7] * b;
        res.[8] <- a[8] * b;
        res.[9] <- a[9] * b;
        res.[10] <- a[10] * b;
        res.[11] <- a[11] * b;
        res.[12] <- a[12] * b;
        res.[13] <- a[13] * b;
        res.[14] <- a[14] * b;
        res.[15] <- a[15] * b;
        res
    (* } *)
    
    /// <summary>
    /// PGA3D.sadd : res = a + b
    /// scalar/multivector addition
    /// </summary>
    static member (+) (a: float32, b: PGA3D) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- a + b[0]
        res.[1] <- b[1]
        res.[2] <- b[2]
        res.[3] <- b[3]
        res.[4] <- b[4]
        res.[5] <- b[5]
        res.[6] <- b[6]
        res.[7] <- b[7]
        res.[8] <- b[8]
        res.[9] <- b[9]
        res.[10] <- b[10]
        res.[11] <- b[11]
        res.[12] <- b[12]
        res.[13] <- b[13]
        res.[14] <- b[14]
        res.[15] <- b[15]
        res
    (* } *)
    
    /// <summary>
    /// PGA3D.adds : res = a + b
    /// multivector/scalar addition
    /// </summary>
    static member (+) (a: PGA3D, b) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- a[0] + b;
        res.[1] <- a[1]
        res.[2] <- a[2]
        res.[3] <- a[3]
        res.[4] <- a[4]
        res.[5] <- a[5]
        res.[6] <- a[6]
        res.[7] <- a[7]
        res.[8] <- a[8]
        res.[9] <- a[9]
        res.[10] <- a[10]
        res.[11] <- a[11]
        res.[12] <- a[12]
        res.[13] <- a[13]
        res.[14] <- a[14]
        res.[15] <- a[15]
        res
    (* } *)
    
    /// <summary>
    /// PGA3D.ssub : res = a - b
    /// scalar/multivector subtraction
    /// </summary>
    static member (-) (a, b: PGA3D) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- a - b[0]
        res.[1] <- -b[1]
        res.[2] <- -b[2]
        res.[3] <- -b[3]
        res.[4] <- -b[4]
        res.[5] <- -b[5]
        res.[6] <- -b[6]
        res.[7] <- -b[7]
        res.[8] <- -b[8]
        res.[9] <- -b[9]
        res.[10] <- -b[10]
        res.[11] <- -b[11]
        res.[12] <- -b[12]
        res.[13] <- -b[13]
        res.[14] <- -b[14]
        res.[15] <- -b[15]
        res
    (* } *)
    
    /// <summary>
    /// PGA3D.subs : res = a - b
    /// multivector/scalar subtraction
    /// </summary>
    static member (-) (a: PGA3D, b) =
    // (* { *)
        let res = new PGA3D();
        res.[0] <- a[0] - b;
        res.[1] <- a[1]
        res.[2] <- a[2]
        res.[3] <- a[3]
        res.[4] <- a[4]
        res.[5] <- a[5]
        res.[6] <- a[6]
        res.[7] <- a[7]
        res.[8] <- a[8]
        res.[9] <- a[9]
        res.[10] <- a[10]
        res.[11] <- a[11]
        res.[12] <- a[12]
        res.[13] <- a[13]
        res.[14] <- a[14]
        res.[15] <- a[15]
        res
    // (* } *)
    
    /// <summary>
    /// PGA3D.norm()
    /// Calculate the Euclidean norm. (strict positive).
    /// </summary>
    member this.norm () (* { *) =
        let b = this.Conjugate ()
        let foo: PGA3D = this * b
        let n = foo[0]
        MathF.Sqrt(MathF.Abs(n))
    
    /// <summary>
    /// PGA3D.inorm()
    /// Calculate the Ideal norm. (signed)
    /// </summary>
    member this.inorm () (* { *) =
        if this[1] <> 0.0f then this[1]
        elif this[15] <> 0.0f then this[15]
        else (!!!this).norm()
        //this[1]!=0.0f
        //? this[1]
        //:
            //this[15] != 0.0f
            //?
                //this[15]
                //:(!this).norm();(* } *)
    
    /// <summary>
    /// PGA3D.normalized()
    /// =s a normalized (Euclidean) element.
    /// </summary>
    member this.normalized() (* { *) = this*(1f/this.norm()); (* } *)
    
    /// string cast
    override this.ToString() =
        let sb = new StringBuilder();
        let mutable n= 0
        for i in 0..15 do
            if (_mVec[i] <> 0.0f) then (* { *)
                sb.Append($"{_mVec[i]}{(if i = 0 then String.Empty else _basis[i])} + ")
                |> ignore
                n <- n + 1
        if (n = 0) then
            sb.Append("0"[0])
            |> ignore
        sb.ToString().TrimEnd(' ', '+');
    //
    //
    // PGA is plane based. Vectors are planes. (think linear functionals)
    static member e0 = new PGA3D(1f, 1);
    static member e1 = new PGA3D(1f, 2);
    static member e2 = new PGA3D(1f, 3);
    static member e3 = new PGA3D(1f, 4);
// open type PGA3D
// let inline (^^^) a b = PGA3D.(^^^) (a, b)
// type PGA3D with
    member this.X = this.normalized()[13]
    member this.Y = this.normalized()[12]
    member this.Z = this.normalized()[11]
    member this.Vector =
        let this = this.normalized()
        (this.X, this.Y, this.Z)
    member this.AsDirection =
        this.normalized() - PGA3D.e123
    
    
    member this.ToPoint = $"({this.X}, {this.Y}, {this.Z})"
    // PGA lines are bivectors.
    static member e01 = PGA3D.e0 ^^^ PGA3D.e1; 
    static member e02 = PGA3D.e0^^^PGA3D.e2;
    static member e03 = PGA3D.e0^^^PGA3D.e3;
    static member e12 = PGA3D.e1^^^PGA3D.e2; 
    static member e31 = PGA3D.e3^^^PGA3D.e1;
    static member e23 = PGA3D.e2^^^PGA3D.e3;
    
    // PGA points are trivectors.
    static member e123 = PGA3D.e1 ^^^ PGA3D.e2 ^^^ PGA3D.e3; // the origin
    static member e032 = PGA3D.e0 ^^^ PGA3D.e3 ^^^ PGA3D.e2;
    static member e013 = PGA3D.e0 ^^^ PGA3D.e1 ^^^ PGA3D.e3;
    static member e021 = PGA3D.e0 ^^^ PGA3D.e2 ^^^ PGA3D.e1;
    
    /// <summary>
    /// PGA3D.plane(a,b,c,d)
    /// A plane is defined using its homogenous equation ax + by + cz + d = 0
    /// </summary>
    static member plane (a, b, c, d) (* { *) = a * PGA3D.e1 + b * PGA3D.e2 + c * PGA3D.e3 + d * PGA3D.e0; (* } *)
    
    /// <summary>
    /// PGA3D.point(x,y,z)
    /// A point is just a homogeneous point, euclidean coordinates plus the origin
    /// </summary>
    static member point (x: float32, y: float32, z: float32) (* { *) = PGA3D.e123 + x * PGA3D.e032 + y * PGA3D.e013 + z * PGA3D.e021; (* } *)
    static member direction (x: float32, y: float32, z: float32) =
        // PGA3D.e123 +
        x * PGA3D.e032 + y * PGA3D.e013 + z * PGA3D.e021; (* } *)
        
    static member distance (a: PGA3D, b: PGA3D) = (a.normalized() &&& b.normalized()).norm()
        
    /// <summary>
    /// Rotors (euclidean lines) and translators (ideal lines)
    /// </summary>
    static member rotor (angle: float32, line: PGA3D) (* { *) =
        (MathF.Cos(angle/2.0f)) +  (MathF.Sin(angle/2.0f)) * line.normalized() (* } *)
    static member rotate(element: PGA3D, rotation: PGA3D) =
        rotation * element * ~~~rotation
    static member translator(dist: float32, line: PGA3D) (* { *) = 1.0f + (dist/2.0f) * line (* } *)
    static member translate(point: PGA3D, direction: PGA3D) =
        point.normalized() + direction
    
    // for our toy problem (generate points on the surface of a torus)
    // we start with a function that generates motors.
    // circle(t) with t going from 0 to 1.
    static member circle(t, radius, line: PGA3D) (* { *)
        = PGA3D.rotor(t*2.0f * MathF.PI,line) * PGA3D.translator(radius,PGA3D.e1*PGA3D.e0);
    (* } *)
    
    // a torus is now the product of two circles. 
    static member torus(s, t, r1, l1: PGA3D, r2, l2: PGA3D) (* { *)
        = PGA3D.circle (s,r2,l2) * PGA3D.circle(t,r1,l1);
    (* } *)
    
    // and to sample its points we simply sandwich the origin ..
    static member point_on_torus(s, t) = (* { *)
        let _to = PGA3D.torus(s,t,0.25f,PGA3D.e12,0.6f,PGA3D.e31);
        _to * PGA3D.e123 * ~~~_to;
    // (* } *)
    
