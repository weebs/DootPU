namespace PGA
open System
open System.Text

type PGA2D(?f: float32, ?idx: int) =
    // just for debug and print output, the basis names
    static let _basis = [| "1";"e0";"e1";"e2";"e01";"e20";"e12";"e012" |]
    let _mVec = Array.zeroCreate<float32> 8
    // public float this[int idx]
    // {
    //     get { return _mVec[idx] }
    //     set { _mVec[idx] <- value; }
    // }
    do
        match f, idx with
        | Some value, Some index ->
            _mVec[index] <- value
        | _ -> ()
    let f = defaultArg f 0f
    let idx = defaultArg idx 0
    member this.Item with get index = _mVec[index] and set index value = _mVec[index] <- value


    /// <summary>
    /// PGA2D.Reverse : res = ~a
    /// Reverse the order of the basis blades.
    /// </summary>
    static member (~~~) (a: PGA2D) =
        let res = PGA2D()
        res[0] <- a[0]
        res[1] <- a[1]
        res[2] <- a[2]
        res[3] <- a[3]
        res[4] <- -a[4]
        res[5] <- -a[5]
        res[6] <- -a[6]
        res[7] <- -a[7]
        res

    /// <summary>
    /// PGA2D.Dual : res = !a
    /// Poincare duality operator.
    /// </summary>
    static member (!!!) (a: PGA2D) =
        let res = PGA2D()
        res[0] <- a[7]
        res[1] <- a[6]
        res[2] <- a[5]
        res[3] <- a[4]
        res[4] <- a[3]
        res[5] <- a[2]
        res[6] <- a[1]
        res[7] <- a[0]
        res

    /// <summary>
    /// PGA2D.Conjugate : res = a.Conjugate()
    /// Clifford Conjugation
    /// </summary>
    member this.Conjugate () =
        let res = PGA2D()
        res[0] <- this[0]
        res[1] <- -this[1]
        res[2] <- -this[2]
        res[3] <- -this[3]
        res[4] <- -this[4]
        res[5] <- -this[5]
        res[6] <- -this[6]
        res[7] <- this[7]
        res

    /// <summary>
    /// PGA2D.Involute : res = a.Involute()
    /// Main involution
    /// </summary>
    member this.Involute () =
        let res = PGA2D()
        res[0] <- this[0]
        res[1] <- -this[1]
        res[2] <- -this[2]
        res[3] <- -this[3]
        res[4] <- this[4]
        res[5] <- this[5]
        res[6] <- this[6]
        res[7] <- -this[7]
        res

    /// <summary>
    /// PGA2D.Mul : res = a * b
    /// The geometric product.
    /// </summary>
    static member (*) (a: PGA2D, b: PGA2D) =
        let res = PGA2D()
        res[0] <- b[0]*a[0]+b[2]*a[2]+b[3]*a[3]-b[6]*a[6]
        res[1] <- b[1]*a[0]+b[0]*a[1]-b[4]*a[2]+b[5]*a[3]+b[2]*a[4]-b[3]*a[5]-b[7]*a[6]-b[6]*a[7]
        res[2] <- b[2]*a[0]+b[0]*a[2]-b[6]*a[3]+b[3]*a[6]
        res[3] <- b[3]*a[0]+b[6]*a[2]+b[0]*a[3]-b[2]*a[6]
        res[4] <- b[4]*a[0]+b[2]*a[1]-b[1]*a[2]+b[7]*a[3]+b[0]*a[4]+b[6]*a[5]-b[5]*a[6]+b[3]*a[7]
        res[5] <- b[5]*a[0]-b[3]*a[1]+b[7]*a[2]+b[1]*a[3]-b[6]*a[4]+b[0]*a[5]+b[4]*a[6]+b[2]*a[7]
        res[6] <- b[6]*a[0]+b[3]*a[2]-b[2]*a[3]+b[0]*a[6]
        res[7] <- b[7]*a[0]+b[6]*a[1]+b[5]*a[2]+b[4]*a[3]+b[3]*a[4]+b[2]*a[5]+b[1]*a[6]+b[0]*a[7]
        res

    /// <summary>
    /// PGA2D.Wedge : res = a ^ b
    /// The outer product. (MEET)
    /// </summary>
    static member (^^^) (a: PGA2D, b: PGA2D) =
        let res = PGA2D()
        res[0] <- b[0]*a[0]
        res[1] <- b[1]*a[0]+b[0]*a[1]
        res[2] <- b[2]*a[0]+b[0]*a[2]
        res[3] <- b[3]*a[0]+b[0]*a[3]
        res[4] <- b[4]*a[0]+b[2]*a[1]-b[1]*a[2]+b[0]*a[4]
        res[5] <- b[5]*a[0]-b[3]*a[1]+b[1]*a[3]+b[0]*a[5]
        res[6] <- b[6]*a[0]+b[3]*a[2]-b[2]*a[3]+b[0]*a[6]
        res[7] <- b[7]*a[0]+b[6]*a[1]+b[5]*a[2]+b[4]*a[3]+b[3]*a[4]+b[2]*a[5]+b[1]*a[6]+b[0]*a[7]
        res

    /// <summary>
    /// PGA2D.Vee : res = a & b
    /// The regressive product. (JOIN)
    /// </summary>
    static member (&&&) (a: PGA2D, b: PGA2D) =
        let res = PGA2D()
        res[7] <- 1f*(a[7]*b[7])
        res[6] <- 1f*(a[6]*b[7]+a[7]*b[6])
        res[5] <- 1f*(a[5]*b[7]+a[7]*b[5])
        res[4] <- 1f*(a[4]*b[7]+a[7]*b[4])
        res[3] <- 1f*(a[3]*b[7]+a[5]*b[6]-a[6]*b[5]+a[7]*b[3])
        res[2] <- 1f*(a[2]*b[7]-a[4]*b[6]+a[6]*b[4]+a[7]*b[2])
        res[1] <- 1f*(a[1]*b[7]+a[4]*b[5]-a[5]*b[4]+a[7]*b[1])
        res[0] <- 1f*(a[0]*b[7]+a[1]*b[6]+a[2]*b[5]+a[3]*b[4]+a[4]*b[3]+a[5]*b[2]+a[6]*b[1]+a[7]*b[0])
        res

    /// <summary>
    /// PGA2D.Dot : res = a | b
    /// The inner product.
    /// </summary>
    static member (|||) (a: PGA2D, b: PGA2D) =
        let res = PGA2D()
        res[0] <- b[0]*a[0]+b[2]*a[2]+b[3]*a[3]-b[6]*a[6]
        res[1] <- b[1]*a[0]+b[0]*a[1]-b[4]*a[2]+b[5]*a[3]+b[2]*a[4]-b[3]*a[5]-b[7]*a[6]-b[6]*a[7]
        res[2] <- b[2]*a[0]+b[0]*a[2]-b[6]*a[3]+b[3]*a[6]
        res[3] <- b[3]*a[0]+b[6]*a[2]+b[0]*a[3]-b[2]*a[6]
        res[4] <- b[4]*a[0]+b[7]*a[3]+b[0]*a[4]+b[3]*a[7]
        res[5] <- b[5]*a[0]+b[7]*a[2]+b[0]*a[5]+b[2]*a[7]
        res[6] <- b[6]*a[0]+b[0]*a[6]
        res[7] <- b[7]*a[0]+b[0]*a[7]
        res

    /// <summary>
    /// PGA2D.Add : res = a + b
    /// Multivector addition
    /// </summary>
    static member (+) (a: PGA2D, b: PGA2D) =
        let res = PGA2D()
        res[0] <- a[0]+b[0]
        res[1] <- a[1]+b[1]
        res[2] <- a[2]+b[2]
        res[3] <- a[3]+b[3]
        res[4] <- a[4]+b[4]
        res[5] <- a[5]+b[5]
        res[6] <- a[6]+b[6]
        res[7] <- a[7]+b[7]
        res

    /// <summary>
    /// PGA2D.Sub : res = a - b
    /// Multivector subtraction
    /// </summary>
    static member (-) (a: PGA2D, b: PGA2D) =
        let res = PGA2D()
        res[0] <- a[0]-b[0]
        res[1] <- a[1]-b[1]
        res[2] <- a[2]-b[2]
        res[3] <- a[3]-b[3]
        res[4] <- a[4]-b[4]
        res[5] <- a[5]-b[5]
        res[6] <- a[6]-b[6]
        res[7] <- a[7]-b[7]
        res

    /// <summary>
    /// PGA2D.smul : res = a * b
    /// scalar/multivector multiplication
    /// </summary>
    static member (*) (a: float32, b: PGA2D) =
        let res = PGA2D()
        res[0] <- a*b[0]
        res[1] <- a*b[1]
        res[2] <- a*b[2]
        res[3] <- a*b[3]
        res[4] <- a*b[4]
        res[5] <- a*b[5]
        res[6] <- a*b[6]
        res[7] <- a*b[7]
        res

    /// <summary>
    /// PGA2D.muls : res = a * b
    /// multivector/scalar multiplication
    /// </summary>
    static member (*) (a: PGA2D, b: float32) =
        let res = PGA2D()
        res[0] <- a[0]*b;
        res[1] <- a[1]*b;
        res[2] <- a[2]*b;
        res[3] <- a[3]*b;
        res[4] <- a[4]*b;
        res[5] <- a[5]*b;
        res[6] <- a[6]*b;
        res[7] <- a[7]*b;
        res

    /// <summary>
    /// PGA2D.sadd : res = a + b
    /// scalar/multivector addition
    /// </summary>
    static member (+) (a: float32, b: PGA2D) =
        let res = PGA2D()
        res[0] <- a+b[0]
        res[1] <- b[1]
        res[2] <- b[2]
        res[3] <- b[3]
        res[4] <- b[4]
        res[5] <- b[5]
        res[6] <- b[6]
        res[7] <- b[7]
        res

    /// <summary>
    /// PGA2D.adds : res = a + b
    /// multivector/scalar addition
    /// </summary>
    static member (+) (a: PGA2D, b: float32) =
        let res = PGA2D()
        res[0] <- a[0]+b;
        res[1] <- a[1]
        res[2] <- a[2]
        res[3] <- a[3]
        res[4] <- a[4]
        res[5] <- a[5]
        res[6] <- a[6]
        res[7] <- a[7]
        res

    /// <summary>
    /// PGA2D.ssub : res = a - b
    /// scalar/multivector subtraction
    /// </summary>
    static member (-) (a: float32, b: PGA2D) =
        let res = PGA2D()
        res[0] <- a-b[0]
        res[1] <- -b[1]
        res[2] <- -b[2]
        res[3] <- -b[3]
        res[4] <- -b[4]
        res[5] <- -b[5]
        res[6] <- -b[6]
        res[7] <- -b[7]
        res

    /// <summary>
    /// PGA2D.subs : res = a - b
    /// multivector/scalar subtraction
    /// </summary>
    static member (-) (a: PGA2D, b: float32) =
        let res = PGA2D()
        res[0] <- a[0]-b;
        res[1] <- a[1]
        res[2] <- a[2]
        res[3] <- a[3]
        res[4] <- a[4]
        res[5] <- a[5]
        res[6] <- a[6]
        res[7] <- a[7]
        res

                /// <summary>
                /// PGA2D.norm()
                /// Calculate the Euclidean norm. (strict positive).
                /// </summary>
    member this.norm() = float32 (MathF.Sqrt(Math.Abs((this*this.Conjugate())[0])))
    
    /// <summary>
    /// PGA2D.inorm()
    /// Calculate the Ideal norm. (signed)
    /// </summary>
    member this.inorm() =
        if this[1] <> 0f then this[1]
        // todo: elif this[15] <> 0f then this[15]
        elif this[7] <> 0f then this[7]
        else (!!!this).norm()
    
    /// <summary>
    /// PGA2D.normalized()
    /// Returns a normalized (Euclidean) element.
    /// </summary>
    member this.normalized() = this*(1f/this.norm())
    
    
    // The basis blades
    static member e0 = PGA2D(1f, 1)
    static member e1 = PGA2D(1f, 2)
    static member e2 = PGA2D(1f, 3)
    static member e01 = PGA2D(1f, 4)
    static member e20 = PGA2D(1f, 5)
    static member e12 = PGA2D(1f, 6)
    static member e012 = PGA2D(1f, 7)
    
    static member point (x, y) = !!!(PGA2D.e0 + x * PGA2D.e1 + y * PGA2D.e2)
    static member direction (x: float32, y: float32) = (PGA2D.e012 + x * PGA2D.e01 + y * PGA2D.e20)
    member this.X = this[4] / this[6]
    member this.Y = this[5] / this[6]
    
    override this.ToString() =
        let sb = StringBuilder()
        let mutable n = 0
        for i in 0..7 do
            if _mVec[i] <> 0f then
                sb.Append $"{_mVec[i]}{if i = 0 then String.Empty else _basis[i]}"
                |> ignore
                n <- n + 1
            if n = 0 then
                sb.Append "0"
                |> ignore
        sb.ToString()
    /// string cast
//     override this.ToString() =
//     {
//         var sb = StringBuilder()
//         var n=0;
//         for (int i = 0; i < 8; ++i) 
//         if (_mVec[i] != 0.0f) {
//             sb.Append($"{_mVec[i]}{(i == 0 ? string.Empty : _basis[i])} + ")
//             n++;
//                 }
//         if (n==0) sb.Append("0")
//         return sb.ToString().TrimEnd(' ', '+')
//     }
//     }
//
//     class Program
//     {
//             
//
//     static void Main(string[] args)
//     {
//     
//         Console.WriteLine("e0*e0         : "+e0*e0)
//         Console.WriteLine("pss           : "+e012)
//         Console.WriteLine("pss*pss       : "+e012*e012)
//
//     }
//     }
// }
//
module PGA2D =
    let distance (a: PGA2D) (b: PGA2D) =
        MathF.Sqrt((a.X - b.X) ** 2f + (a.Y - b.Y) ** 2f)
    let isOnLine (point_a: PGA2D) point_b point =
        let length = distance point_a point_b
        // printfn "line length = %A" length
        let dist_a = distance point_a point
        // printfn "dist a = %A" dist_a
        dist_a <= length && (distance point_b point) <= length

