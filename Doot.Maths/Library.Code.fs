module PGA
// Written by a generator written by enki.
// using System;
// using System.Text
open System
open System.Text
// using static PGA.PGA3D; // static variable acces

// namespace PGA
// (* { *)
type PGA3D(?f: float, ?idx: int) =
    // just for debug and print output, the basis names
    let foo = ()
    let _basis = [|  "1"; "e0"; "e1"; "e2"; "e3"; "e01"; "e02"; "e03"; "e12"; "e31"; "e23"; "e021"; "e013"; "e032"; "e123"; "e0123"  |]
    let _mVec = Array.zeroCreate 16
    let f = defaultArg f 1
    let idx = defaultArg idx 1

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
    (* } *)

    /// <summary>
    /// PGA3D.Dual : res = !a
    /// Poincare duality operator.
    /// </summary>
    static member (!) (a: PGA3D) =
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
    (* } *)

    /// <summary>
    /// PGA3D.Wedge : res = a ^^^ b
    /// The outer product. (MEET)
    /// </summary>
    static member (^^^) (a: PGA3D, b: PGA3D) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- b[0]*a[0]
        res.[1] <- b[1]*a[0]+b[0]*a[1]
        res.[2] <- b[2]*a[0]+b[0]*a[2]
        res.[3] <- b[3]*a[0]+b[0]*a[3]
        res.[4] <- b[4]*a[0]+b[0]*a[4]
        res.[5] <- b[5]*a[0]+b[2]*a[1]-b[1]*a[2]+b[0]*a[5]
        res.[6] <- b[6]*a[0]+b[3]*a[1]-b[1]*a[3]+b[0]*a[6]
        res.[7] <- b[7]*a[0]+b[4]*a[1]-b[1]*a[4]+b[0]*a[7]
        res.[8] <- b[8]*a[0]+b[3]*a[2]-b[2]*a[3]+b[0]*a[8]
        res.[9] <- b[9]*a[0]-b[4]*a[2]+b[2]*a[4]+b[0]*a[9]
        res.[10] <- b[10]*a[0]+b[4]*a[3]-b[3]*a[4]+b[0]*a[10]
        res.[11] <- b[11]*a[0]-b[8]*a[1]+b[6]*a[2]-b[5]*a[3]-b[3]*a[5]+b[2]*a[6]-b[1]*a[8]+b[0]*a[11]
        res.[12] <- b[12]*a[0]-b[9]*a[1]-b[7]*a[2]+b[5]*a[4]+b[4]*a[5]-b[2]*a[7]-b[1]*a[9]+b[0]*a[12]
        res.[13] <- b[13]*a[0]-b[10]*a[1]+b[7]*a[3]-b[6]*a[4]-b[4]*a[6]+b[3]*a[7]-b[1]*a[10]+b[0]*a[13]
        res.[14] <- b[14]*a[0]+b[10]*a[2]+b[9]*a[3]+b[8]*a[4]+b[4]*a[8]+b[3]*a[9]+b[2]*a[10]+b[0]*a[14]
        res.[15] <- b[15]*a[0]+b[14]*a[1]+b[13]*a[2]+b[12]*a[3]+b[11]*a[4]+b[10]*a[5]+b[9]*a[6]+b[8]*a[7]+b[7]*a[8]+b[6]*a[9]+b[5]*a[10]-b[4]*a[11]-b[3]*a[12]-b[2]*a[13]-b[1]*a[14]+b[0]*a[15]
        res
    (* } *)

    /// <summary>
    /// PGA3D.Vee : res = a &&& b
    /// The regressive product. (JOIN)
    /// </summary>
    static member (&&&) (a: PGA3D, b: PGA3D) =
    (* { *)
        let res = new PGA3D();
        res.[15] <- 1*(a[15]*b[15]);
        res.[14] <- -1*(a[14]*-1*b[15]+a[15]*b[14]*-1);
        res.[13] <- -1*(a[13]*-1*b[15]+a[15]*b[13]*-1);
        res.[12] <- -1*(a[12]*-1*b[15]+a[15]*b[12]*-1);
        res.[11] <- -1*(a[11]*-1*b[15]+a[15]*b[11]*-1);
        res.[10] <- 1*(a[10]*b[15]+a[13]*-1*b[14]*-1-a[14]*-1*b[13]*-1+a[15]*b[10]);
        res.[9] <- 1*(a[9]*b[15]+a[12]*-1*b[14]*-1-a[14]*-1*b[12]*-1+a[15]*b[9]);
        res.[8] <- 1*(a[8]*b[15]+a[11]*-1*b[14]*-1-a[14]*-1*b[11]*-1+a[15]*b[8]);
        res.[7] <- 1*(a[7]*b[15]+a[12]*-1*b[13]*-1-a[13]*-1*b[12]*-1+a[15]*b[7]);
        res.[6] <- 1*(a[6]*b[15]-a[11]*-1*b[13]*-1+a[13]*-1*b[11]*-1+a[15]*b[6]);
        res.[5] <- 1*(a[5]*b[15]+a[11]*-1*b[12]*-1-a[12]*-1*b[11]*-1+a[15]*b[5]);
        res.[4] <- 1*(a[4]*b[15]-a[7]*b[14]*-1+a[9]*b[13]*-1-a[10]*b[12]*-1-a[12]*-1*b[10]+a[13]*-1*b[9]-a[14]*-1*b[7]+a[15]*b[4]);
        res.[3] <- 1*(a[3]*b[15]-a[6]*b[14]*-1-a[8]*b[13]*-1+a[10]*b[11]*-1+a[11]*-1*b[10]-a[13]*-1*b[8]-a[14]*-1*b[6]+a[15]*b[3]);
        res.[2] <- 1*(a[2]*b[15]-a[5]*b[14]*-1+a[8]*b[12]*-1-a[9]*b[11]*-1-a[11]*-1*b[9]+a[12]*-1*b[8]-a[14]*-1*b[5]+a[15]*b[2]);
        res.[1] <- 1*(a[1]*b[15]+a[5]*b[13]*-1+a[6]*b[12]*-1+a[7]*b[11]*-1+a[11]*-1*b[7]+a[12]*-1*b[6]+a[13]*-1*b[5]+a[15]*b[1]);
        res.[0] <- 1*(a[0]*b[15]+a[1]*b[14]*-1+a[2]*b[13]*-1+a[3]*b[12]*-1+a[4]*b[11]*-1+a[5]*b[10]+a[6]*b[9]+a[7]*b[8]+a[8]*b[7]+a[9]*b[6]+a[10]*b[5]-a[11]*-1*b[4]-a[12]*-1*b[3]-a[13]*-1*b[2]-a[14]*-1*b[1]+a[15]*b[0]);
        res
    (* } *)

    // static member op_Amp(a: PGA3D, b: PGA3D) =
    // (* { *)
    //     = a &&& b;
    // (* } *)

    /// <summary>
    /// PGA3D.Dot : res = a | b
    /// The inner product.
    /// </summary>
    static member (|||) (a: PGA3D, b: PGA3D) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- b[0]*a[0]+b[2]*a[2]+b[3]*a[3]+b[4]*a[4]-b[8]*a[8]-b[9]*a[9]-b[10]*a[10]-b[14]*a[14]
        res.[1] <- b[1]*a[0]+b[0]*a[1]-b[5]*a[2]-b[6]*a[3]-b[7]*a[4]+b[2]*a[5]+b[3]*a[6]+b[4]*a[7]+b[11]*a[8]+b[12]*a[9]+b[13]*a[10]+b[8]*a[11]+b[9]*a[12]+b[10]*a[13]+b[15]*a[14]-b[14]*a[15]
        res.[2] <- b[2]*a[0]+b[0]*a[2]-b[8]*a[3]+b[9]*a[4]+b[3]*a[8]-b[4]*a[9]-b[14]*a[10]-b[10]*a[14]
        res.[3] <- b[3]*a[0]+b[8]*a[2]+b[0]*a[3]-b[10]*a[4]-b[2]*a[8]-b[14]*a[9]+b[4]*a[10]-b[9]*a[14]
        res.[4] <- b[4]*a[0]-b[9]*a[2]+b[10]*a[3]+b[0]*a[4]-b[14]*a[8]+b[2]*a[9]-b[3]*a[10]-b[8]*a[14]
        res.[5] <- b[5]*a[0]-b[11]*a[3]+b[12]*a[4]+b[0]*a[5]-b[15]*a[10]-b[3]*a[11]+b[4]*a[12]-b[10]*a[15]
        res.[6] <- b[6]*a[0]+b[11]*a[2]-b[13]*a[4]+b[0]*a[6]-b[15]*a[9]+b[2]*a[11]-b[4]*a[13]-b[9]*a[15]
        res.[7] <- b[7]*a[0]-b[12]*a[2]+b[13]*a[3]+b[0]*a[7]-b[15]*a[8]-b[2]*a[12]+b[3]*a[13]-b[8]*a[15]
        res.[8] <- b[8]*a[0]+b[14]*a[4]+b[0]*a[8]+b[4]*a[14]
        res.[9] <- b[9]*a[0]+b[14]*a[3]+b[0]*a[9]+b[3]*a[14]
        res.[10] <- b[10]*a[0]+b[14]*a[2]+b[0]*a[10]+b[2]*a[14]
        res.[11] <- b[11]*a[0]+b[15]*a[4]+b[0]*a[11]-b[4]*a[15]
        res.[12] <- b[12]*a[0]+b[15]*a[3]+b[0]*a[12]-b[3]*a[15]
        res.[13] <- b[13]*a[0]+b[15]*a[2]+b[0]*a[13]-b[2]*a[15]
        res.[14] <- b[14]*a[0]+b[0]*a[14]
        res.[15] <- b[15]*a[0]+b[0]*a[15]
        res
    (* } *)

    /// <summary>
    /// PGA3D.Add : res = a + b
    /// Multivector addition
    /// </summary>
    static member (+) (a: PGA3D, b: PGA3D) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- a[0]+b[0]
        res.[1] <- a[1]+b[1]
        res.[2] <- a[2]+b[2]
        res.[3] <- a[3]+b[3]
        res.[4] <- a[4]+b[4]
        res.[5] <- a[5]+b[5]
        res.[6] <- a[6]+b[6]
        res.[7] <- a[7]+b[7]
        res.[8] <- a[8]+b[8]
        res.[9] <- a[9]+b[9]
        res.[10] <- a[10]+b[10]
        res.[11] <- a[11]+b[11]
        res.[12] <- a[12]+b[12]
        res.[13] <- a[13]+b[13]
        res.[14] <- a[14]+b[14]
        res.[15] <- a[15]+b[15]
        res
    (* } *)

    /// <summary>
    /// PGA3D.Sub : res = a - b
    /// Multivector subtraction
    /// </summary>
    static member (-) (a: PGA3D, b: PGA3D) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- a[0]-b[0]
        res.[1] <- a[1]-b[1]
        res.[2] <- a[2]-b[2]
        res.[3] <- a[3]-b[3]
        res.[4] <- a[4]-b[4]
        res.[5] <- a[5]-b[5]
        res.[6] <- a[6]-b[6]
        res.[7] <- a[7]-b[7]
        res.[8] <- a[8]-b[8]
        res.[9] <- a[9]-b[9]
        res.[10] <- a[10]-b[10]
        res.[11] <- a[11]-b[11]
        res.[12] <- a[12]-b[12]
        res.[13] <- a[13]-b[13]
        res.[14] <- a[14]-b[14]
        res.[15] <- a[15]-b[15]
        res
    (* } *)

    /// <summary>
    /// PGA3D.smul : res = a * b
    /// scalar/multivector multiplication
    /// </summary>
    static member (*) (a: float, b: PGA3D) =
    (* { *) 
        let res = new PGA3D();
        res.[0] <- a*b[0]
        res.[1] <- a*b[1]
        res.[2] <- a*b[2]
        res.[3] <- a*b[3]
        res.[4] <- a*b[4]
        res.[5] <- a*b[5]
        res.[6] <- a*b[6]
        res.[7] <- a*b[7]
        res.[8] <- a*b[8]
        res.[9] <- a*b[9]
        res.[10] <- a*b[10]
        res.[11] <- a*b[11]
        res.[12] <- a*b[12]
        res.[13] <- a*b[13]
        res.[14] <- a*b[14]
        res.[15] <- a*b[15]
        res
    (* } *)

    /// <summary>
    /// PGA3D.muls : res = a * b
    /// multivector/scalar multiplication
    /// </summary>
    static member (*) (PGA3D a, float b) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- a[0]*b;
        res.[1] <- a[1]*b;
        res.[2] <- a[2]*b;
        res.[3] <- a[3]*b;
        res.[4] <- a[4]*b;
        res.[5] <- a[5]*b;
        res.[6] <- a[6]*b;
        res.[7] <- a[7]*b;
        res.[8] <- a[8]*b;
        res.[9] <- a[9]*b;
        res.[10] <- a[10]*b;
        res.[11] <- a[11]*b;
        res.[12] <- a[12]*b;
        res.[13] <- a[13]*b;
        res.[14] <- a[14]*b;
        res.[15] <- a[15]*b;
        res
    (* } *)

    /// <summary>
    /// PGA3D.sadd : res = a + b
    /// scalar/multivector addition
    /// </summary>
    static member (+) (float a, PGA3D b) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- a+b[0]
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
    static member (+) (PGA3D a, float b) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- a[0]+b;
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
    static member (-) (float a, PGA3D b) =
    (* { *)
        let res = new PGA3D();
        res.[0] <- a-b[0]
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
    static member (-) (PGA3D a, float b) =
    // (* { *)
        let res = new PGA3D();
        res.[0] <- a[0]-b;
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
        // float <| Math.Sqrt(Math.Abs((this*this.Conjugate())[0]))
        ()
    
    /// <summary>
    /// PGA3D.inorm()
    /// Calculate the Ideal norm. (signed)
    /// </summary>
    member this.inorm () (* { *) =
        if this[1] <> 0.0f then this[1]
        elif this[15] <> 0.0f then this[15]
        else (!this).norm()
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
    member this.normalized() (* { *) = this*(1/this.norm()); (* } *)
    
    
    // PGA is plane based. Vectors are planes. (think linear functionals)
    static member e0 = new PGA3D(1f, 1);
    static member e1 = new PGA3D(1f, 2);
    static member e2 = new PGA3D(1f, 3);
    static member e3 = new PGA3D(1f, 4);
    
    // PGA lines are bivectors.
    static member e01 = e0^^^e1; 
    static member e02 = e0^^^e2;
    static member e03 = e0^^^e3;
    static member e12 = e1^^^e2; 
    static member e31 = e3^^^e1;
    static member e23 = e2^^^e3;
    
    // PGA points are trivectors.
    static member e123 = e1^^^e2^^^e3; // the origin
    static member e032 = e0^^^e3^^^e2;
    static member e013 = e0^^^e1^^^e3;
    static member e021 = e0^^^e2^^^e1;

    /// <summary>
    /// PGA3D.plane(a,b,c,d)
    /// A plane is defined using its homogenous equation ax + by + cz + d = 0
    /// </summary>
    static member plane (a, b, c, d) (* { *) = a*e1 + b*e2 + c*e3 + d*e0; (* } *)
    
    /// <summary>
    /// PGA3D.point(x,y,z)
    /// A point is just a homogeneous point, euclidean coordinates plus the origin
    /// </summary>
    static member point (x, y, z) (* { *) = e123 + x*e032 + y*e013 + z*e021; (* } *)
    
    /// <summary>
    /// Rotors (euclidean lines) and translators (ideal lines)
    /// </summary>
    // static member rotor (angle: float, line: PGA3D) (* { *) =
    //     (float Math.Cos(angle/2.0f)) +  (float Math.Sin(angle/2.0f)) * line.normalized() (* } *)
    static member translator(float dist, PGA3D line) (* { *) = 1.0f + (dist/2.0f) * line (* } *)

    // for our toy problem (generate points on the surface of a torus)
    // we start with a function that generates motors.
    // circle(t) with t going from 0 to 1.
    static member circle(float t, float radius, PGA3D line) (* { *)
      = rotor(t*2.0f*(float) Math.PI,line) * translator(radius,e1*e0);
    (* } *)
    
    // a torus is now the product of two circles. 
    static member torus(float s, float t, float r1, PGA3D l1, float r2, PGA3D l2) (* { *)
      = circle(s,r2,l2)*circle(t,r1,l1);
    (* } *)
    
    // and to sample its points we simply sandwich the origin ..
    static member point_on_torus(float s, float t) = (* { *)
      let _to = torus(s,t,0.25f,e12,0.6f,e31);
      _to * e123 * ~~~_to;
    (* } *)

    
    /// string cast
    override this.ToString() =
        let sb = new StringBuilder();
        let n=0
        for i in 0..15 do
            if (_mVec[i] <> 0.0f) then (* { *)
                sb.Append($"{_mVec[i]}{(if i == 0 then string.Empty else _basis[i])} + ");
                // n++
                ()
        if (n==0) then
            sb.Append("0"[0])
            |> ignore
        sb.ToString().TrimEnd(' ', '+');

//     class Program
//     (* { *)
//             
//
//         static void Main(string[] args)
//         (* { *)
//         
//             // Elements of the even subalgebra (scalar + bivector + pss) of unit length are motors
//             let rot = rotor((float) Math.PI/2.0f,e1*e2);
//             
//             // The outer product ^^^ is the MEET. Here we intersect the yz (x=0) and xz (y=0) planes.
//             let ax_z = e1 ^^^ e2;
//             
//             // line and plane meet in point. We intersect the line along the z-axis (x=0,y=0) with the xy (z=0) plane.
//             let orig = ax_z ^^^ e3;
//             
//             // We can also easily create points and join them into a line using the regressive (vee, &&&) product.
//             let px = point(1,0,0);
//             let py = point(0,1,0);
//             let pz = point(0,0,1);
//             let pxyz = point(1, 1, 1);
//             
//             let line = orig &&& px;
//             
//             // Lets also create the plane with equation 2x + z - 3 = 0
//             let p = plane(2,0,1,-3);
//             
//             // rotations work on all elements
//             let rotated_plane = rot * p * ~rot;
//             let rotated_line  = rot * line * ~rot;
//             let rotated_point = rot * px * ~rot;
//             
//             // See the 3D PGA Cheat sheet for a huge collection of useful formulas
//             let point_on_plane = (p | px) * p;
//             
//             // Some output
//             Console.WriteLine("a point       : "+px);
//             Console.WriteLine("a line        : "+line);
//             Console.WriteLine("a plane       : "+p);
//             Console.WriteLine("a rotor       : "+rot);
//             Console.WriteLine("rotated line  : "+rotated_line);
//             Console.WriteLine("rotated point : "+rotated_point);
//             Console.WriteLine("rotated plane : "+rotated_plane);
//             Console.WriteLine("point on plane: "+point_on_plane.normalized());
//             Console.WriteLine("point on torus: "+point_on_torus(0.0f,0.0f));
//
//         (* } *)
//     (* } *)
// (* } *)

