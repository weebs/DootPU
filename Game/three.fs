module rec threejs

#nowarn "3390" // disable warnings for invalid XML comments
#nowarn "0044" // disable warnings for `Obsolete` usage

open System
open Fable.Core
open Fable.Core.JS
open Browser.Types

// module MathUtils = __math_MathUtils

type Array<'T> = System.Collections.Generic.IList<'T>
type ArrayLike<'T> = System.Collections.Generic.IList<'T>
type Error = System.Exception
type RegExp = System.Text.RegularExpressions.Regex
type [<AllowNullLiteral>] GainNode = interface end
type [<AllowNullLiteral>] AudioNode = interface end
type [<AllowNullLiteral>] AudioContext = interface end
type [<AllowNullLiteral>] AudioBuffer = interface end
type [<AllowNullLiteral>] AudioBufferSourceNode = interface end // MediaStream PannerNode Iterable WebGLBuffer 
type [<AllowNullLiteral>] MediaStream = interface end
type [<AllowNullLiteral>] Iterable<'t> = interface end
type [<AllowNullLiteral>] PannerNode = interface end
type [<AllowNullLiteral>] BufferSource = interface end
type [<AllowNullLiteral>] ImageBitmap = interface end
type IUniform = __renderers_shaders_UniformsLib.IUniform<obj>
type WebGL2RenderingContext = inherit WebGLRenderingContext
type [<AllowNullLiteral>] WebGLShader = interface end
type [<AllowNullLiteral>] DOMHighResTimeStamp = interface end
type [<AllowNullLiteral>] GamepadMappingType = interface end
type [<AllowNullLiteral>] XRAnchorSet = interface end
type [<AllowNullLiteral>] XRPlaneSet = interface end
type [<AllowNullLiteral>] DOMPointReadOnly = interface end

module Constants =

    type [<AllowNullLiteral>] IExports =
        abstract REVISION: string
        abstract CullFaceNone: CullFace
        abstract CullFaceBack: CullFace
        abstract CullFaceFront: CullFace
        abstract CullFaceFrontBack: CullFace
        abstract BasicShadowMap: ShadowMapType
        abstract PCFShadowMap: ShadowMapType
        abstract PCFSoftShadowMap: ShadowMapType
        abstract VSMShadowMap: ShadowMapType
        abstract FrontSide: Side
        abstract BackSide: Side
        abstract DoubleSide: Side
        abstract FlatShading: Shading
        abstract SmoothShading: Shading
        abstract NoBlending: Blending
        abstract NormalBlending: Blending
        abstract AdditiveBlending: Blending
        abstract SubtractiveBlending: Blending
        abstract MultiplyBlending: Blending
        abstract CustomBlending: Blending
        abstract AddEquation: BlendingEquation
        abstract SubtractEquation: BlendingEquation
        abstract ReverseSubtractEquation: BlendingEquation
        abstract MinEquation: BlendingEquation
        abstract MaxEquation: BlendingEquation
        abstract ZeroFactor: BlendingDstFactor
        abstract OneFactor: BlendingDstFactor
        abstract SrcColorFactor: BlendingDstFactor
        abstract OneMinusSrcColorFactor: BlendingDstFactor
        abstract SrcAlphaFactor: BlendingDstFactor
        abstract OneMinusSrcAlphaFactor: BlendingDstFactor
        abstract DstAlphaFactor: BlendingDstFactor
        abstract OneMinusDstAlphaFactor: BlendingDstFactor
        abstract DstColorFactor: BlendingDstFactor
        abstract OneMinusDstColorFactor: BlendingDstFactor
        abstract SrcAlphaSaturateFactor: BlendingSrcFactor
        abstract NeverDepth: DepthModes
        abstract AlwaysDepth: DepthModes
        abstract LessDepth: DepthModes
        abstract LessEqualDepth: DepthModes
        abstract EqualDepth: DepthModes
        abstract GreaterEqualDepth: DepthModes
        abstract GreaterDepth: DepthModes
        abstract NotEqualDepth: DepthModes
        abstract MultiplyOperation: Combine
        abstract MixOperation: Combine
        abstract AddOperation: Combine
        abstract NoToneMapping: ToneMapping
        abstract LinearToneMapping: ToneMapping
        abstract ReinhardToneMapping: ToneMapping
        abstract CineonToneMapping: ToneMapping
        abstract ACESFilmicToneMapping: ToneMapping
        abstract UVMapping: Mapping
        abstract CubeReflectionMapping: Mapping
        abstract CubeRefractionMapping: Mapping
        abstract EquirectangularReflectionMapping: Mapping
        abstract EquirectangularRefractionMapping: Mapping
        abstract CubeUVReflectionMapping: Mapping
        abstract CubeUVRefractionMapping: Mapping
        abstract RepeatWrapping: Wrapping
        abstract ClampToEdgeWrapping: Wrapping
        abstract MirroredRepeatWrapping: Wrapping
        abstract NearestFilter: TextureFilter
        abstract NearestMipmapNearestFilter: TextureFilter
        abstract NearestMipMapNearestFilter: TextureFilter
        abstract NearestMipmapLinearFilter: TextureFilter
        abstract NearestMipMapLinearFilter: TextureFilter
        abstract LinearFilter: TextureFilter
        abstract LinearMipmapNearestFilter: TextureFilter
        abstract LinearMipMapNearestFilter: TextureFilter
        abstract LinearMipmapLinearFilter: TextureFilter
        abstract LinearMipMapLinearFilter: TextureFilter
        abstract UnsignedByteType: TextureDataType
        abstract ByteType: TextureDataType
        abstract ShortType: TextureDataType
        abstract UnsignedShortType: TextureDataType
        abstract IntType: TextureDataType
        abstract UnsignedIntType: TextureDataType
        abstract FloatType: TextureDataType
        abstract HalfFloatType: TextureDataType
        abstract UnsignedShort4444Type: TextureDataType
        abstract UnsignedShort5551Type: TextureDataType
        abstract UnsignedShort565Type: TextureDataType
        abstract UnsignedInt248Type: TextureDataType
        abstract AlphaFormat: PixelFormat
        abstract RGBFormat: PixelFormat
        abstract RGBAFormat: PixelFormat
        abstract LuminanceFormat: PixelFormat
        abstract LuminanceAlphaFormat: PixelFormat
        abstract RGBEFormat: PixelFormat
        abstract DepthFormat: PixelFormat
        abstract DepthStencilFormat: PixelFormat
        abstract RedFormat: PixelFormat
        abstract RedIntegerFormat: PixelFormat
        abstract RGFormat: PixelFormat
        abstract RGIntegerFormat: PixelFormat
        abstract RGBIntegerFormat: PixelFormat
        abstract RGBAIntegerFormat: PixelFormat
        abstract RGB_S3TC_DXT1_Format: CompressedPixelFormat
        abstract RGBA_S3TC_DXT1_Format: CompressedPixelFormat
        abstract RGBA_S3TC_DXT3_Format: CompressedPixelFormat
        abstract RGBA_S3TC_DXT5_Format: CompressedPixelFormat
        abstract RGB_PVRTC_4BPPV1_Format: CompressedPixelFormat
        abstract RGB_PVRTC_2BPPV1_Format: CompressedPixelFormat
        abstract RGBA_PVRTC_4BPPV1_Format: CompressedPixelFormat
        abstract RGBA_PVRTC_2BPPV1_Format: CompressedPixelFormat
        abstract RGB_ETC1_Format: CompressedPixelFormat
        abstract RGB_ETC2_Format: CompressedPixelFormat
        abstract RGBA_ETC2_EAC_Format: CompressedPixelFormat
        abstract RGBA_ASTC_4x4_Format: CompressedPixelFormat
        abstract RGBA_ASTC_5x4_Format: CompressedPixelFormat
        abstract RGBA_ASTC_5x5_Format: CompressedPixelFormat
        abstract RGBA_ASTC_6x5_Format: CompressedPixelFormat
        abstract RGBA_ASTC_6x6_Format: CompressedPixelFormat
        abstract RGBA_ASTC_8x5_Format: CompressedPixelFormat
        abstract RGBA_ASTC_8x6_Format: CompressedPixelFormat
        abstract RGBA_ASTC_8x8_Format: CompressedPixelFormat
        abstract RGBA_ASTC_10x5_Format: CompressedPixelFormat
        abstract RGBA_ASTC_10x6_Format: CompressedPixelFormat
        abstract RGBA_ASTC_10x8_Format: CompressedPixelFormat
        abstract RGBA_ASTC_10x10_Format: CompressedPixelFormat
        abstract RGBA_ASTC_12x10_Format: CompressedPixelFormat
        abstract RGBA_ASTC_12x12_Format: CompressedPixelFormat
        abstract SRGB8_ALPHA8_ASTC_4x4_Format: CompressedPixelFormat
        abstract SRGB8_ALPHA8_ASTC_5x4_Format: CompressedPixelFormat
        abstract SRGB8_ALPHA8_ASTC_5x5_Format: CompressedPixelFormat
        abstract SRGB8_ALPHA8_ASTC_6x5_Format: CompressedPixelFormat
        abstract SRGB8_ALPHA8_ASTC_6x6_Format: CompressedPixelFormat
        abstract SRGB8_ALPHA8_ASTC_8x5_Format: CompressedPixelFormat
        abstract SRGB8_ALPHA8_ASTC_8x6_Format: CompressedPixelFormat
        abstract SRGB8_ALPHA8_ASTC_8x8_Format: CompressedPixelFormat
        abstract SRGB8_ALPHA8_ASTC_10x5_Format: CompressedPixelFormat
        abstract SRGB8_ALPHA8_ASTC_10x6_Format: CompressedPixelFormat
        abstract SRGB8_ALPHA8_ASTC_10x8_Format: CompressedPixelFormat
        abstract SRGB8_ALPHA8_ASTC_10x10_Format: CompressedPixelFormat
        abstract SRGB8_ALPHA8_ASTC_12x10_Format: CompressedPixelFormat
        abstract SRGB8_ALPHA8_ASTC_12x12_Format: CompressedPixelFormat
        abstract RGBA_BPTC_Format: CompressedPixelFormat
        abstract LoopOnce: AnimationActionLoopStyles
        abstract LoopRepeat: AnimationActionLoopStyles
        abstract LoopPingPong: AnimationActionLoopStyles
        abstract InterpolateDiscrete: InterpolationModes
        abstract InterpolateLinear: InterpolationModes
        abstract InterpolateSmooth: InterpolationModes
        abstract ZeroCurvatureEnding: InterpolationEndingModes
        abstract ZeroSlopeEnding: InterpolationEndingModes
        abstract WrapAroundEnding: InterpolationEndingModes
        abstract NormalAnimationBlendMode: AnimationBlendMode
        abstract AdditiveAnimationBlendMode: AnimationBlendMode
        abstract TrianglesDrawMode: TrianglesDrawModes
        abstract TriangleStripDrawMode: TrianglesDrawModes
        abstract TriangleFanDrawMode: TrianglesDrawModes
        abstract LinearEncoding: TextureEncoding
        abstract sRGBEncoding: TextureEncoding
        abstract GammaEncoding: TextureEncoding
        abstract RGBEEncoding: TextureEncoding
        abstract LogLuvEncoding: TextureEncoding
        abstract RGBM7Encoding: TextureEncoding
        abstract RGBM16Encoding: TextureEncoding
        abstract RGBDEncoding: TextureEncoding
        abstract BasicDepthPacking: DepthPackingStrategies
        abstract RGBADepthPacking: DepthPackingStrategies
        abstract TangentSpaceNormalMap: NormalMapTypes
        abstract ObjectSpaceNormalMap: NormalMapTypes
        abstract ZeroStencilOp: StencilOp
        abstract KeepStencilOp: StencilOp
        abstract ReplaceStencilOp: StencilOp
        abstract IncrementStencilOp: StencilOp
        abstract DecrementStencilOp: StencilOp
        abstract IncrementWrapStencilOp: StencilOp
        abstract DecrementWrapStencilOp: StencilOp
        abstract InvertStencilOp: StencilOp
        abstract NeverStencilFunc: StencilFunc
        abstract LessStencilFunc: StencilFunc
        abstract EqualStencilFunc: StencilFunc
        abstract LessEqualStencilFunc: StencilFunc
        abstract GreaterStencilFunc: StencilFunc
        abstract NotEqualStencilFunc: StencilFunc
        abstract GreaterEqualStencilFunc: StencilFunc
        abstract AlwaysStencilFunc: StencilFunc
        abstract StaticDrawUsage: Usage
        abstract DynamicDrawUsage: Usage
        abstract StreamDrawUsage: Usage
        abstract StaticReadUsage: Usage
        abstract DynamicReadUsage: Usage
        abstract StreamReadUsage: Usage
        abstract StaticCopyUsage: Usage
        abstract DynamicCopyUsage: Usage
        abstract StreamCopyUsage: Usage
        abstract GLSL1: GLSLVersion
        abstract GLSL3: GLSLVersion

    type [<RequireQualifiedAccess>] MOUSE =
        | LEFT = 0
        | MIDDLE = 1
        | RIGHT = 2
        | ROTATE = 0
        | DOLLY = 1
        | PAN = 2

    type [<RequireQualifiedAccess>] TOUCH =
        | ROTATE = 0
        | PAN = 1
        | DOLLY_PAN = 2
        | DOLLY_ROTATE = 3

    type CullFace = interface end

    type ShadowMapType = interface end

    type Side = interface end

    type Shading = interface end

    type Blending = interface end

    type BlendingEquation = interface end

    type BlendingDstFactor = interface end

    type BlendingSrcFactor = interface end

    type DepthModes = interface end

    type Combine = interface end

    type ToneMapping = interface end

    type Mapping = interface end

    type Wrapping = interface end

    type TextureFilter = interface end

    type TextureDataType = interface end

    type PixelFormat = interface end

    type [<StringEnum>] [<RequireQualifiedAccess>] PixelFormatGPU =
        | [<CompiledName("ALPHA")>] ALPHA
        | [<CompiledName("RGB")>] RGB
        | [<CompiledName("RGBA")>] RGBA
        | [<CompiledName("LUMINANCE")>] LUMINANCE
        | [<CompiledName("LUMINANCE_ALPHA")>] LUMINANCE_ALPHA
        | [<CompiledName("RED_INTEGER")>] RED_INTEGER
        | [<CompiledName("R8")>] R8
        | [<CompiledName("R8_SNORM")>] R8_SNORM
        | [<CompiledName("R8I")>] R8I
        | [<CompiledName("R8UI")>] R8UI
        | [<CompiledName("R16I")>] R16I
        | [<CompiledName("R16UI")>] R16UI
        | [<CompiledName("R16F")>] R16F
        | [<CompiledName("R32I")>] R32I
        | [<CompiledName("R32UI")>] R32UI
        | [<CompiledName("R32F")>] R32F
        | [<CompiledName("RG8")>] RG8
        | [<CompiledName("RG8_SNORM")>] RG8_SNORM
        | [<CompiledName("RG8I")>] RG8I
        | [<CompiledName("RG8UI")>] RG8UI
        | [<CompiledName("RG16I")>] RG16I
        | [<CompiledName("RG16UI")>] RG16UI
        | [<CompiledName("RG16F")>] RG16F
        | [<CompiledName("RG32I")>] RG32I
        | [<CompiledName("RG32UI")>] RG32UI
        | [<CompiledName("RG32F")>] RG32F
        | [<CompiledName("RGB565")>] RGB565
        | [<CompiledName("RGB8")>] RGB8
        | [<CompiledName("RGB8_SNORM")>] RGB8_SNORM
        | [<CompiledName("RGB8I")>] RGB8I
        | [<CompiledName("RGB8UI")>] RGB8UI
        | [<CompiledName("RGB16I")>] RGB16I
        | [<CompiledName("RGB16UI")>] RGB16UI
        | [<CompiledName("RGB16F")>] RGB16F
        | [<CompiledName("RGB32I")>] RGB32I
        | [<CompiledName("RGB32UI")>] RGB32UI
        | [<CompiledName("RGB32F")>] RGB32F
        | [<CompiledName("RGB9_E5")>] RGB9_E5
        | [<CompiledName("SRGB8")>] SRGB8
        | [<CompiledName("R11F_G11F_B10F")>] R11F_G11F_B10F
        | [<CompiledName("RGBA4")>] RGBA4
        | [<CompiledName("RGBA8")>] RGBA8
        | [<CompiledName("RGBA8_SNORM")>] RGBA8_SNORM
        | [<CompiledName("RGBA8I")>] RGBA8I
        | [<CompiledName("RGBA8UI")>] RGBA8UI
        | [<CompiledName("RGBA16I")>] RGBA16I
        | [<CompiledName("RGBA16UI")>] RGBA16UI
        | [<CompiledName("RGBA16F")>] RGBA16F
        | [<CompiledName("RGBA32I")>] RGBA32I
        | [<CompiledName("RGBA32UI")>] RGBA32UI
        | [<CompiledName("RGBA32F")>] RGBA32F
        | [<CompiledName("RGB5_A1")>] RGB5_A1
        | [<CompiledName("RGB10_A2")>] RGB10_A2
        | [<CompiledName("RGB10_A2UI")>] RGB10_A2UI
        | [<CompiledName("SRGB8_ALPHA8")>] SRGB8_ALPHA8
        | [<CompiledName("DEPTH_COMPONENT16")>] DEPTH_COMPONENT16
        | [<CompiledName("DEPTH_COMPONENT24")>] DEPTH_COMPONENT24
        | [<CompiledName("DEPTH_COMPONENT32F")>] DEPTH_COMPONENT32F
        | [<CompiledName("DEPTH24_STENCIL8")>] DEPTH24_STENCIL8
        | [<CompiledName("DEPTH32F_STENCIL8")>] DEPTH32F_STENCIL8

    type CompressedPixelFormat = interface end

    type AnimationActionLoopStyles = interface end

    type InterpolationModes = interface end

    type InterpolationEndingModes = interface end

    type AnimationBlendMode = interface end

    type TrianglesDrawModes = interface end

    type TextureEncoding = interface end

    type DepthPackingStrategies = interface end

    type NormalMapTypes = interface end

    type StencilOp = interface end

    type StencilFunc = interface end

    type Usage = interface end

    type GLSLVersion = interface end

    type [<StringEnum>] [<RequireQualifiedAccess>] BuiltinShaderAttributeName =
        | Position
        | Normal
        | Uv
        | Color
        | SkinIndex
        | SkinWeight
        | InstanceMatrix
        | MorphTarget0
        | MorphTarget1
        | MorphTarget2
        | MorphTarget3
        | MorphTarget4
        | MorphTarget5
        | MorphTarget6
        | MorphTarget7
        | MorphNormal0
        | MorphNormal1
        | MorphNormal2
        | MorphNormal3

module Utils =
    type Color = __math_Color.Color

    type ColorRepresentation =
        U3<Color, string, float>

module __animation_AnimationAction =
    type AnimationMixer = __animation_AnimationMixer.AnimationMixer
    type AnimationClip = __animation_AnimationClip.AnimationClip
    type AnimationActionLoopStyles = Constants.AnimationActionLoopStyles
    type AnimationBlendMode = Constants.AnimationBlendMode
    type Object3D = __core_Object3D.Object3D

    type [<AllowNullLiteral>] IExports =
        abstract AnimationAction: AnimationActionStatic

    type [<AllowNullLiteral>] AnimationAction =
        abstract blendMode: AnimationBlendMode with get, set
        /// <default>THREE.LoopRepeat</default>
        abstract loop: AnimationActionLoopStyles with get, set
        /// <default>0</default>
        abstract time: float with get, set
        /// <default>1</default>
        abstract timeScale: float with get, set
        /// <default>1</default>
        abstract weight: float with get, set
        /// <default>Infinity</default>
        abstract repetitions: float with get, set
        /// <default>false</default>
        abstract paused: bool with get, set
        /// <default>true</default>
        abstract enabled: bool with get, set
        /// <default>false</default>
        abstract clampWhenFinished: bool with get, set
        /// <default>true</default>
        abstract zeroSlopeAtStart: bool with get, set
        /// <default>true</default>
        abstract zeroSlopeAtEnd: bool with get, set
        abstract play: unit -> AnimationAction
        abstract stop: unit -> AnimationAction
        abstract reset: unit -> AnimationAction
        abstract isRunning: unit -> bool
        abstract isScheduled: unit -> bool
        abstract startAt: time: float -> AnimationAction
        abstract setLoop: mode: AnimationActionLoopStyles * repetitions: float -> AnimationAction
        abstract setEffectiveWeight: weight: float -> AnimationAction
        abstract getEffectiveWeight: unit -> float
        abstract fadeIn: duration: float -> AnimationAction
        abstract fadeOut: duration: float -> AnimationAction
        abstract crossFadeFrom: fadeOutAction: AnimationAction * duration: float * warp: bool -> AnimationAction
        abstract crossFadeTo: fadeInAction: AnimationAction * duration: float * warp: bool -> AnimationAction
        abstract stopFading: unit -> AnimationAction
        abstract setEffectiveTimeScale: timeScale: float -> AnimationAction
        abstract getEffectiveTimeScale: unit -> float
        abstract setDuration: duration: float -> AnimationAction
        abstract syncWith: action: AnimationAction -> AnimationAction
        abstract halt: duration: float -> AnimationAction
        abstract warp: statTimeScale: float * endTimeScale: float * duration: float -> AnimationAction
        abstract stopWarping: unit -> AnimationAction
        abstract getMixer: unit -> AnimationMixer
        abstract getClip: unit -> AnimationClip
        abstract getRoot: unit -> Object3D

    type [<AllowNullLiteral>] AnimationActionStatic =
        [<EmitConstructor>] abstract Create: mixer: AnimationMixer * clip: AnimationClip * ?localRoot: Object3D * ?blendMode: AnimationBlendMode -> AnimationAction

module __animation_AnimationClip =
    type KeyframeTrack = __animation_KeyframeTrack.KeyframeTrack
    type Vector3 = __math_Vector3.Vector3
    type Bone = __objects_Bone.Bone
    type AnimationBlendMode = Constants.AnimationBlendMode

    type [<AllowNullLiteral>] IExports =
        abstract AnimationClip: AnimationClipStatic

    type [<AllowNullLiteral>] MorphTarget =
        abstract name: string with get, set
        abstract vertices: ResizeArray<Vector3> with get, set

    type [<AllowNullLiteral>] AnimationClip =
        abstract name: string with get, set
        abstract tracks: ResizeArray<KeyframeTrack> with get, set
        /// <default>THREE.NormalAnimationBlendMode</default>
        abstract blendMode: AnimationBlendMode with get, set
        /// <default>-1</default>
        abstract duration: float with get, set
        abstract uuid: string with get, set
        abstract results: ResizeArray<obj option> with get, set
        abstract resetDuration: unit -> AnimationClip
        abstract trim: unit -> AnimationClip
        abstract validate: unit -> bool
        abstract optimize: unit -> AnimationClip
        abstract clone: unit -> AnimationClip
        abstract toJSON: clip: AnimationClip -> obj option

    type [<AllowNullLiteral>] AnimationClipStatic =
        [<EmitConstructor>] abstract Create: ?name: string * ?duration: float * ?tracks: ResizeArray<KeyframeTrack> * ?blendMode: AnimationBlendMode -> AnimationClip
        abstract CreateFromMorphTargetSequence: name: string * morphTargetSequence: ResizeArray<MorphTarget> * fps: float * noLoop: bool -> AnimationClip
        abstract findByName: clipArray: ResizeArray<AnimationClip> * name: string -> AnimationClip
        abstract CreateClipsFromMorphTargetSequences: morphTargets: ResizeArray<MorphTarget> * fps: float * noLoop: bool -> ResizeArray<AnimationClip>
        abstract parse: json: obj option -> AnimationClip
        abstract parseAnimation: animation: obj option * bones: ResizeArray<Bone> -> AnimationClip
        abstract toJSON: clip: AnimationClip -> obj option

module __animation_AnimationMixer =
    type AnimationClip = __animation_AnimationClip.AnimationClip
    type AnimationAction = __animation_AnimationAction.AnimationAction
    type AnimationBlendMode = Constants.AnimationBlendMode
    type EventDispatcher = __core_EventDispatcher.EventDispatcher
    type Object3D = __core_Object3D.Object3D
    type AnimationObjectGroup = __animation_AnimationObjectGroup.AnimationObjectGroup

    type [<AllowNullLiteral>] IExports =
        abstract AnimationMixer: AnimationMixerStatic

    type [<AllowNullLiteral>] AnimationMixer =
        inherit EventDispatcher
        /// <default>0</default>
        abstract time: float with get, set
        /// <default>1.0</default>
        abstract timeScale: float with get, set
        abstract clipAction: clip: AnimationClip * ?root: U2<Object3D, AnimationObjectGroup> * ?blendMode: AnimationBlendMode -> AnimationAction
        abstract existingAction: clip: AnimationClip * ?root: U2<Object3D, AnimationObjectGroup> -> AnimationAction option
        abstract stopAllAction: unit -> AnimationMixer
        abstract update: deltaTime: float -> AnimationMixer
        abstract setTime: timeInSeconds: float -> AnimationMixer
        abstract getRoot: unit -> U2<Object3D, AnimationObjectGroup>
        abstract uncacheClip: clip: AnimationClip -> unit
        abstract uncacheRoot: root: U2<Object3D, AnimationObjectGroup> -> unit
        abstract uncacheAction: clip: AnimationClip * ?root: U2<Object3D, AnimationObjectGroup> -> unit

    type [<AllowNullLiteral>] AnimationMixerStatic =
        [<EmitConstructor>] abstract Create: root: U2<Object3D, AnimationObjectGroup> -> AnimationMixer

module __animation_AnimationObjectGroup =

    type [<AllowNullLiteral>] IExports =
        abstract AnimationObjectGroup: AnimationObjectGroupStatic

    type [<AllowNullLiteral>] AnimationObjectGroup =
        abstract uuid: string with get, set
        abstract stats: {| bindingsPerObject: float; objects: {| total: float; inUse: float |} |} with get, set
        abstract isAnimationObjectGroup: bool
        abstract add: [<ParamArray>] args: obj option[] -> unit
        abstract remove: [<ParamArray>] args: obj option[] -> unit
        abstract uncache: [<ParamArray>] args: obj option[] -> unit

    type [<AllowNullLiteral>] AnimationObjectGroupStatic =
        [<EmitConstructor>] abstract Create: [<ParamArray>] args: obj option[] -> AnimationObjectGroup

module __animation_AnimationUtils =
    type AnimationClip = __animation_AnimationClip.AnimationClip
    let [<Import("AnimationUtils","three/animation/AnimationUtils")>] animationUtils: AnimationUtils.IExports = jsNative

    module AnimationUtils =

        type [<AllowNullLiteral>] IExports =
            abstract arraySlice: array: obj option * from: float * ``to``: float -> obj option
            abstract convertArray: array: obj option * ``type``: obj option * forceClone: bool -> obj option
            abstract isTypedArray: object: obj option -> bool
            abstract getKeyFrameOrder: times: ResizeArray<float> -> ResizeArray<float>
            abstract sortedArray: values: ResizeArray<obj option> * stride: float * order: ResizeArray<float> -> ResizeArray<obj option>
            abstract flattenJSON: jsonKeys: ResizeArray<string> * times: ResizeArray<obj option> * values: ResizeArray<obj option> * valuePropertyName: string -> unit
            /// <param name="sourceClip" />
            /// <param name="name" />
            /// <param name="startFrame" />
            /// <param name="endFrame" />
            /// <param name="fps" />
            abstract subclip: sourceClip: AnimationClip * name: string * startFrame: float * endFrame: float * ?fps: float -> AnimationClip
            /// <param name="targetClip" />
            /// <param name="referenceFrame" />
            /// <param name="referenceClip" />
            /// <param name="fps" />
            abstract makeClipAdditive: targetClip: AnimationClip * ?referenceFrame: float * ?referenceClip: AnimationClip * ?fps: float -> AnimationClip

module __animation_KeyframeTrack =
    type DiscreteInterpolant = __math_interpolants_DiscreteInterpolant.DiscreteInterpolant
    type LinearInterpolant = __math_interpolants_LinearInterpolant.LinearInterpolant
    type CubicInterpolant = __math_interpolants_CubicInterpolant.CubicInterpolant
    type InterpolationModes = Constants.InterpolationModes

    type [<AllowNullLiteral>] IExports =
        abstract KeyframeTrack: KeyframeTrackStatic

    type [<AllowNullLiteral>] KeyframeTrack =
        abstract name: string with get, set
        abstract times: Float32Array with get, set
        abstract values: Float32Array with get, set
        abstract ValueTypeName: string with get, set
        abstract TimeBufferType: Float32Array with get, set
        abstract ValueBufferType: Float32Array with get, set
        /// <default>THREE.InterpolateLinear</default>
        abstract DefaultInterpolation: InterpolationModes with get, set
        abstract InterpolantFactoryMethodDiscrete: result: obj option -> DiscreteInterpolant
        abstract InterpolantFactoryMethodLinear: result: obj option -> LinearInterpolant
        abstract InterpolantFactoryMethodSmooth: result: obj option -> CubicInterpolant
        abstract setInterpolation: interpolation: InterpolationModes -> KeyframeTrack
        abstract getInterpolation: unit -> InterpolationModes
        abstract getValueSize: unit -> float
        abstract shift: timeOffset: float -> KeyframeTrack
        abstract scale: timeScale: float -> KeyframeTrack
        abstract trim: startTime: float * endTime: float -> KeyframeTrack
        abstract validate: unit -> bool
        abstract optimize: unit -> KeyframeTrack
        abstract clone: unit -> KeyframeTrack

    type [<AllowNullLiteral>] KeyframeTrackStatic =
        /// <param name="name" />
        /// <param name="times" />
        /// <param name="values" />
        /// <param name="interpolation" />
        [<EmitConstructor>] abstract Create: name: string * times: ArrayLike<obj option> * values: ArrayLike<obj option> * ?interpolation: InterpolationModes -> KeyframeTrack
        abstract toJSON: track: KeyframeTrack -> obj option


module __cameras_ArrayCamera =
    type PerspectiveCamera = __cameras_PerspectiveCamera.PerspectiveCamera

    type [<AllowNullLiteral>] IExports =
        abstract ArrayCamera: ArrayCameraStatic

    type [<AllowNullLiteral>] ArrayCamera =
        inherit PerspectiveCamera
        /// <default>[]</default>
        abstract cameras: ResizeArray<PerspectiveCamera> with get, set
        abstract isArrayCamera: bool

    type [<AllowNullLiteral>] ArrayCameraStatic =
        [<EmitConstructor>] abstract Create: ?cameras: ResizeArray<PerspectiveCamera> -> ArrayCamera

module __cameras_Camera =
    type Matrix4 = __math_Matrix4.Matrix4
    type Vector3 = __math_Vector3.Vector3
    type Object3D = __core_Object3D.Object3D

    type [<AllowNullLiteral>] IExports =
        /// Abstract base class for cameras. This class should always be inherited when you build a new camera.
        abstract Camera: CameraStatic

    /// Abstract base class for cameras. This class should always be inherited when you build a new camera.
    type [<AllowNullLiteral>] Camera =
        inherit Object3D
        /// <summary>This is the inverse of matrixWorld. MatrixWorld contains the Matrix which has the world transform of the Camera.</summary>
        /// <default>new THREE.Matrix4()</default>
        abstract matrixWorldInverse: Matrix4 with get, set
        /// <summary>This is the matrix which contains the projection.</summary>
        /// <default>new THREE.Matrix4()</default>
        abstract projectionMatrix: Matrix4 with get, set
        /// <summary>This is the inverse of projectionMatrix.</summary>
        /// <default>new THREE.Matrix4()</default>
        abstract projectionMatrixInverse: Matrix4 with get, set
        abstract isCamera: bool
        abstract getWorldDirection: target: Vector3 -> Vector3
        /// Updates global transform of the object and its children.
        abstract updateMatrixWorld: ?force: bool -> unit

    /// Abstract base class for cameras. This class should always be inherited when you build a new camera.
    type [<AllowNullLiteral>] CameraStatic =
        /// This constructor sets following properties to the correct type: matrixWorldInverse, projectionMatrix and projectionMatrixInverse.
        [<EmitConstructor>] abstract Create: unit -> Camera

module __cameras_CubeCamera =
    type WebGLCubeRenderTarget = __renderers_WebGLCubeRenderTarget.WebGLCubeRenderTarget
    type Scene = __scenes_Scene.Scene
    type WebGLRenderer = __renderers_WebGLRenderer.WebGLRenderer
    type Object3D = __core_Object3D.Object3D

    type [<AllowNullLiteral>] IExports =
        abstract CubeCamera: CubeCameraStatic

    type [<AllowNullLiteral>] CubeCamera =
        inherit Object3D
        abstract ``type``: string with get, set
        abstract renderTarget: WebGLCubeRenderTarget with get, set
        abstract update: renderer: WebGLRenderer * scene: Scene -> unit

    type [<AllowNullLiteral>] CubeCameraStatic =
        [<EmitConstructor>] abstract Create: near: float * far: float * renderTarget: WebGLCubeRenderTarget -> CubeCamera

module __cameras_OrthographicCamera =
    type Camera = __cameras_Camera.Camera

    type [<AllowNullLiteral>] IExports =
        /// <summary>
        /// Camera with orthographic projection
        /// 
        /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/cameras/OrthographicCamera.js">src/cameras/OrthographicCamera.js</see>
        /// </summary>
        /// <example>
        /// const camera = new THREE.OrthographicCamera( width / - 2, width / 2, height / 2, height / - 2, 1, 1000 );
        /// scene.add( camera );
        /// </example>
        abstract OrthographicCamera: OrthographicCameraStatic

    /// <summary>
    /// Camera with orthographic projection
    /// 
    /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/cameras/OrthographicCamera.js">src/cameras/OrthographicCamera.js</see>
    /// </summary>
    /// <example>
    /// const camera = new THREE.OrthographicCamera( width / - 2, width / 2, height / 2, height / - 2, 1, 1000 );
    /// scene.add( camera );
    /// </example>
    type [<AllowNullLiteral>] OrthographicCamera =
        inherit Camera
        abstract ``type``: string with get, set
        abstract isOrthographicCamera: bool
        /// <default>1</default>
        abstract zoom: float with get, set
        /// <default>null</default>
        abstract view: OrthographicCameraView option with get, set
        /// <summary>Camera frustum left plane.</summary>
        /// <default>-1</default>
        abstract left: float with get, set
        /// <summary>Camera frustum right plane.</summary>
        /// <default>1</default>
        abstract right: float with get, set
        /// <summary>Camera frustum top plane.</summary>
        /// <default>1</default>
        abstract top: float with get, set
        /// <summary>Camera frustum bottom plane.</summary>
        /// <default>-1</default>
        abstract bottom: float with get, set
        /// <summary>Camera frustum near plane.</summary>
        /// <default>0.1</default>
        abstract near: float with get, set
        /// <summary>Camera frustum far plane.</summary>
        /// <default>2000</default>
        abstract far: float with get, set
        /// Updates the camera projection matrix. Must be called after change of parameters.
        abstract updateProjectionMatrix: unit -> unit
        abstract setViewOffset: fullWidth: float * fullHeight: float * offsetX: float * offsetY: float * width: float * height: float -> unit
        abstract clearViewOffset: unit -> unit
        abstract toJSON: ?meta: obj -> obj option

    /// <summary>
    /// Camera with orthographic projection
    /// 
    /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/cameras/OrthographicCamera.js">src/cameras/OrthographicCamera.js</see>
    /// </summary>
    /// <example>
    /// const camera = new THREE.OrthographicCamera( width / - 2, width / 2, height / 2, height / - 2, 1, 1000 );
    /// scene.add( camera );
    /// </example>
    type [<AllowNullLiteral>] OrthographicCameraStatic =
        /// <param name="left">Camera frustum left plane.</param>
        /// <param name="right">Camera frustum right plane.</param>
        /// <param name="top">Camera frustum top plane.</param>
        /// <param name="bottom">Camera frustum bottom plane.</param>
        /// <param name="near">Camera frustum near plane.</param>
        /// <param name="far">Camera frustum far plane.</param>
        [<EmitConstructor>] abstract Create: left: float * right: float * top: float * bottom: float * ?near: float * ?far: float -> OrthographicCamera

    type [<AllowNullLiteral>] OrthographicCameraView =
        abstract enabled: bool with get, set
        abstract fullWidth: float with get, set
        abstract fullHeight: float with get, set
        abstract offsetX: float with get, set
        abstract offsetY: float with get, set
        abstract width: float with get, set
        abstract height: float with get, set

module __cameras_PerspectiveCamera =
    type Camera = __cameras_Camera.Camera

    type [<AllowNullLiteral>] IExports =
        /// <summary>Camera with perspective projection.</summary>
        abstract PerspectiveCamera: PerspectiveCameraStatic

    /// <summary>Camera with perspective projection.</summary>
    type [<AllowNullLiteral>] PerspectiveCamera =
        inherit Camera
        abstract ``type``: string with get, set
        abstract isPerspectiveCamera: bool
        /// <default>1</default>
        abstract zoom: float with get, set
        /// <summary>Camera frustum vertical field of view, from bottom to top of view, in degrees.</summary>
        /// <default>50</default>
        abstract fov: float with get, set
        /// <summary>Camera frustum aspect ratio, window width divided by window height.</summary>
        /// <default>1</default>
        abstract aspect: float with get, set
        /// <summary>Camera frustum near plane.</summary>
        /// <default>0.1</default>
        abstract near: float with get, set
        /// <summary>Camera frustum far plane.</summary>
        /// <default>2000</default>
        abstract far: float with get, set
        /// <default>10</default>
        abstract focus: float with get, set
        /// <default>null</default>
        abstract view: PerspectiveCameraView option with get, set
        /// <default>35</default>
        abstract filmGauge: float with get, set
        /// <default>0</default>
        abstract filmOffset: float with get, set
        abstract setFocalLength: focalLength: float -> unit
        abstract getFocalLength: unit -> float
        abstract getEffectiveFOV: unit -> float
        abstract getFilmWidth: unit -> float
        abstract getFilmHeight: unit -> float
        /// <summary>
        /// Sets an offset in a larger frustum. This is useful for multi-window or multi-monitor/multi-machine setups.
        /// For example, if you have 3x2 monitors and each monitor is 1920x1080 and the monitors are in grid like this:
        /// 
        /// +---+---+---+
        /// | A | B | C |
        /// +---+---+---+
        /// | D | E | F |
        /// +---+---+---+
        /// 
        /// then for each monitor you would call it like this:
        /// 
        /// const w = 1920;
        /// const h = 1080;
        /// const fullWidth = w * 3;
        /// const fullHeight = h * 2;
        /// 
        /// // A
        /// camera.setViewOffset( fullWidth, fullHeight, w * 0, h * 0, w, h );
        /// // B
        /// camera.setViewOffset( fullWidth, fullHeight, w * 1, h * 0, w, h );
        /// // C
        /// camera.setViewOffset( fullWidth, fullHeight, w * 2, h * 0, w, h );
        /// // D
        /// camera.setViewOffset( fullWidth, fullHeight, w * 0, h * 1, w, h );
        /// // E
        /// camera.setViewOffset( fullWidth, fullHeight, w * 1, h * 1, w, h );
        /// // F
        /// camera.setViewOffset( fullWidth, fullHeight, w * 2, h * 1, w, h ); Note there is no reason monitors have to be the same size or in a grid.
        /// </summary>
        /// <param name="fullWidth">full width of multiview setup</param>
        /// <param name="fullHeight">full height of multiview setup</param>
        /// <param name="x">horizontal offset of subcamera</param>
        /// <param name="y">vertical offset of subcamera</param>
        /// <param name="width">width of subcamera</param>
        /// <param name="height">height of subcamera</param>
        abstract setViewOffset: fullWidth: float * fullHeight: float * x: float * y: float * width: float * height: float -> unit
        abstract clearViewOffset: unit -> unit
        /// Updates the camera projection matrix. Must be called after change of parameters.
        abstract updateProjectionMatrix: unit -> unit
        abstract toJSON: ?meta: obj -> obj option
        [<Obsolete("Use {@link PerspectiveCamera#setFocalLength .setFocalLength()} and {@link PerspectiveCamera#filmGauge .filmGauge} instead.")>]
        abstract setLens: focalLength: float * ?frameHeight: float -> unit

    /// <summary>Camera with perspective projection.</summary>
    type [<AllowNullLiteral>] PerspectiveCameraStatic =
        /// <param name="fov">Camera frustum vertical field of view. Default value is 50.</param>
        /// <param name="aspect">Camera frustum aspect ratio. Default value is 1.</param>
        /// <param name="near">Camera frustum near plane. Default value is 0.1.</param>
        /// <param name="far">Camera frustum far plane. Default value is 2000.</param>
        [<EmitConstructor>] abstract Create: ?fov: float * ?aspect: float * ?near: float * ?far: float -> PerspectiveCamera

    type [<AllowNullLiteral>] PerspectiveCameraView =
        abstract enabled: bool with get, set
        abstract fullWidth: float with get, set
        abstract fullHeight: float with get, set
        abstract offsetX: float with get, set
        abstract offsetY: float with get, set
        abstract width: float with get, set
        abstract height: float with get, set

module __cameras_StereoCamera =
    type PerspectiveCamera = __cameras_PerspectiveCamera.PerspectiveCamera
    type Camera = __cameras_Camera.Camera

    type [<AllowNullLiteral>] IExports =
        abstract StereoCamera: StereoCameraStatic

    type [<AllowNullLiteral>] StereoCamera =
        inherit Camera
        abstract ``type``: string with get, set
        /// <default>1</default>
        abstract aspect: float with get, set
        /// <default>0.064</default>
        abstract eyeSep: float with get, set
        abstract cameraL: PerspectiveCamera with get, set
        abstract cameraR: PerspectiveCamera with get, set
        abstract update: camera: PerspectiveCamera -> unit

    type [<AllowNullLiteral>] StereoCameraStatic =
        [<EmitConstructor>] abstract Create: unit -> StereoCamera

module __core_BufferAttribute =
    type Usage = Constants.Usage
    type Matrix3 = __math_Matrix3.Matrix3
    type Matrix4 = __math_Matrix4.Matrix4

    type [<AllowNullLiteral>] IExports =
        /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/BufferAttribute.js">src/core/BufferAttribute.js</see></summary>
        abstract BufferAttribute: BufferAttributeStatic
        [<Obsolete("THREE.Int8Attribute has been removed. Use new THREE.Int8BufferAttribute() instead.")>]
        abstract Int8Attribute: Int8AttributeStatic
        [<Obsolete("THREE.Uint8Attribute has been removed. Use new THREE.Uint8BufferAttribute() instead.")>]
        abstract Uint8Attribute: Uint8AttributeStatic
        [<Obsolete("THREE.Uint8ClampedAttribute has been removed. Use new THREE.Uint8ClampedBufferAttribute() instead.")>]
        abstract Uint8ClampedAttribute: Uint8ClampedAttributeStatic
        [<Obsolete("THREE.Int16Attribute has been removed. Use new THREE.Int16BufferAttribute() instead.")>]
        abstract Int16Attribute: Int16AttributeStatic
        [<Obsolete("THREE.Uint16Attribute has been removed. Use new THREE.Uint16BufferAttribute() instead.")>]
        abstract Uint16Attribute: Uint16AttributeStatic
        [<Obsolete("THREE.Int32Attribute has been removed. Use new THREE.Int32BufferAttribute() instead.")>]
        abstract Int32Attribute: Int32AttributeStatic
        [<Obsolete("THREE.Uint32Attribute has been removed. Use new THREE.Uint32BufferAttribute() instead.")>]
        abstract Uint32Attribute: Uint32AttributeStatic
        [<Obsolete("THREE.Float32Attribute has been removed. Use new THREE.Float32BufferAttribute() instead.")>]
        abstract Float32Attribute: Float32AttributeStatic
        [<Obsolete("THREE.Float64Attribute has been removed. Use new THREE.Float64BufferAttribute() instead.")>]
        abstract Float64Attribute: Float64AttributeStatic
        abstract Int8BufferAttribute: Int8BufferAttributeStatic
        abstract Uint8BufferAttribute: Uint8BufferAttributeStatic
        abstract Uint8ClampedBufferAttribute: Uint8ClampedBufferAttributeStatic
        abstract Int16BufferAttribute: Int16BufferAttributeStatic
        abstract Uint16BufferAttribute: Uint16BufferAttributeStatic
        abstract Int32BufferAttribute: Int32BufferAttributeStatic
        abstract Uint32BufferAttribute: Uint32BufferAttributeStatic
        abstract Float16BufferAttribute: Float16BufferAttributeStatic
        abstract Float32BufferAttribute: Float32BufferAttributeStatic
        abstract Float64BufferAttribute: Float64BufferAttributeStatic

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/BufferAttribute.js">src/core/BufferAttribute.js</see></summary>
    type [<AllowNullLiteral>] BufferAttribute =
        /// <default>''</default>
        abstract name: string with get, set
        abstract array: ArrayLike<float> with get, set
        abstract itemSize: float with get, set
        /// <default>THREE.StaticDrawUsage</default>
        abstract usage: Usage with get, set
        /// <default>{ offset: number; count: number }</default>
        abstract updateRange: {| offset: float; count: float |} with get, set
        /// <default>0</default>
        abstract version: float with get, set
        /// <default>false</default>
        abstract normalized: bool with get, set
        /// <default>0</default>
        abstract count: float with get, set
        abstract needsUpdate: bool with set
        abstract isBufferAttribute: bool
        abstract onUploadCallback: (unit -> unit) with get, set
        abstract onUpload: callback: (unit -> unit) -> BufferAttribute
        abstract setUsage: usage: Usage -> BufferAttribute
        abstract clone: unit -> BufferAttribute
        abstract copy: source: BufferAttribute -> BufferAttribute
        abstract copyAt: index1: float * attribute: BufferAttribute * index2: float -> BufferAttribute
        abstract copyArray: array: ArrayLike<float> -> BufferAttribute
        abstract copyColorsArray: colors: Array<{| r: float; g: float; b: float |}> -> BufferAttribute
        abstract copyVector2sArray: vectors: Array<{| x: float; y: float |}> -> BufferAttribute
        abstract copyVector3sArray: vectors: Array<{| x: float; y: float; z: float |}> -> BufferAttribute
        abstract copyVector4sArray: vectors: Array<{| x: float; y: float; z: float; w: float |}> -> BufferAttribute
        abstract applyMatrix3: m: Matrix3 -> BufferAttribute
        abstract applyMatrix4: m: Matrix4 -> BufferAttribute
        abstract applyNormalMatrix: m: Matrix3 -> BufferAttribute
        abstract transformDirection: m: Matrix4 -> BufferAttribute
        abstract set: value: U2<ArrayLike<float>, ArrayBufferView> * ?offset: float -> BufferAttribute
        abstract getX: index: float -> float
        abstract setX: index: float * x: float -> BufferAttribute
        abstract getY: index: float -> float
        abstract setY: index: float * y: float -> BufferAttribute
        abstract getZ: index: float -> float
        abstract setZ: index: float * z: float -> BufferAttribute
        abstract getW: index: float -> float
        abstract setW: index: float * z: float -> BufferAttribute
        abstract setXY: index: float * x: float * y: float -> BufferAttribute
        abstract setXYZ: index: float * x: float * y: float * z: float -> BufferAttribute
        abstract setXYZW: index: float * x: float * y: float * z: float * w: float -> BufferAttribute
        abstract toJSON: unit -> {| itemSize: float; ``type``: string; array: ResizeArray<float>; normalized: bool |}

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/BufferAttribute.js">src/core/BufferAttribute.js</see></summary>
    type [<AllowNullLiteral>] BufferAttributeStatic =
        [<EmitConstructor>] abstract Create: array: ArrayLike<float> * itemSize: float * ?normalized: bool -> BufferAttribute

    [<Obsolete("THREE.Int8Attribute has been removed. Use new THREE.Int8BufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Int8Attribute =
        inherit BufferAttribute

    [<Obsolete("THREE.Int8Attribute has been removed. Use new THREE.Int8BufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Int8AttributeStatic =
        [<EmitConstructor>] abstract Create: array: obj option * itemSize: float -> Int8Attribute

    [<Obsolete("THREE.Uint8Attribute has been removed. Use new THREE.Uint8BufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Uint8Attribute =
        inherit BufferAttribute

    [<Obsolete("THREE.Uint8Attribute has been removed. Use new THREE.Uint8BufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Uint8AttributeStatic =
        [<EmitConstructor>] abstract Create: array: obj option * itemSize: float -> Uint8Attribute

    [<Obsolete("THREE.Uint8ClampedAttribute has been removed. Use new THREE.Uint8ClampedBufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Uint8ClampedAttribute =
        inherit BufferAttribute

    [<Obsolete("THREE.Uint8ClampedAttribute has been removed. Use new THREE.Uint8ClampedBufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Uint8ClampedAttributeStatic =
        [<EmitConstructor>] abstract Create: array: obj option * itemSize: float -> Uint8ClampedAttribute

    [<Obsolete("THREE.Int16Attribute has been removed. Use new THREE.Int16BufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Int16Attribute =
        inherit BufferAttribute

    [<Obsolete("THREE.Int16Attribute has been removed. Use new THREE.Int16BufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Int16AttributeStatic =
        [<EmitConstructor>] abstract Create: array: obj option * itemSize: float -> Int16Attribute

    [<Obsolete("THREE.Uint16Attribute has been removed. Use new THREE.Uint16BufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Uint16Attribute =
        inherit BufferAttribute

    [<Obsolete("THREE.Uint16Attribute has been removed. Use new THREE.Uint16BufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Uint16AttributeStatic =
        [<EmitConstructor>] abstract Create: array: obj option * itemSize: float -> Uint16Attribute

    [<Obsolete("THREE.Int32Attribute has been removed. Use new THREE.Int32BufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Int32Attribute =
        inherit BufferAttribute

    [<Obsolete("THREE.Int32Attribute has been removed. Use new THREE.Int32BufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Int32AttributeStatic =
        [<EmitConstructor>] abstract Create: array: obj option * itemSize: float -> Int32Attribute

    [<Obsolete("THREE.Uint32Attribute has been removed. Use new THREE.Uint32BufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Uint32Attribute =
        inherit BufferAttribute

    [<Obsolete("THREE.Uint32Attribute has been removed. Use new THREE.Uint32BufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Uint32AttributeStatic =
        [<EmitConstructor>] abstract Create: array: obj option * itemSize: float -> Uint32Attribute

    [<Obsolete("THREE.Float32Attribute has been removed. Use new THREE.Float32BufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Float32Attribute =
        inherit BufferAttribute

    [<Obsolete("THREE.Float32Attribute has been removed. Use new THREE.Float32BufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Float32AttributeStatic =
        [<EmitConstructor>] abstract Create: array: obj option * itemSize: float -> Float32Attribute

    [<Obsolete("THREE.Float64Attribute has been removed. Use new THREE.Float64BufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Float64Attribute =
        inherit BufferAttribute

    [<Obsolete("THREE.Float64Attribute has been removed. Use new THREE.Float64BufferAttribute() instead.")>]
    type [<AllowNullLiteral>] Float64AttributeStatic =
        [<EmitConstructor>] abstract Create: array: obj option * itemSize: float -> Float64Attribute

    type [<AllowNullLiteral>] Int8BufferAttribute =
        inherit BufferAttribute

    type [<AllowNullLiteral>] Int8BufferAttributeStatic =
        [<EmitConstructor>] abstract Create: array: U4<Iterable<float>, ArrayLike<float>, ArrayBuffer, float> * itemSize: float * ?normalized: bool -> Int8BufferAttribute

    type [<AllowNullLiteral>] Uint8BufferAttribute =
        inherit BufferAttribute

    type [<AllowNullLiteral>] Uint8BufferAttributeStatic =
        [<EmitConstructor>] abstract Create: array: U4<Iterable<float>, ArrayLike<float>, ArrayBuffer, float> * itemSize: float * ?normalized: bool -> Uint8BufferAttribute

    type [<AllowNullLiteral>] Uint8ClampedBufferAttribute =
        inherit BufferAttribute

    type [<AllowNullLiteral>] Uint8ClampedBufferAttributeStatic =
        [<EmitConstructor>] abstract Create: array: U4<Iterable<float>, ArrayLike<float>, ArrayBuffer, float> * itemSize: float * ?normalized: bool -> Uint8ClampedBufferAttribute

    type [<AllowNullLiteral>] Int16BufferAttribute =
        inherit BufferAttribute

    type [<AllowNullLiteral>] Int16BufferAttributeStatic =
        [<EmitConstructor>] abstract Create: array: U4<Iterable<float>, ArrayLike<float>, ArrayBuffer, float> * itemSize: float * ?normalized: bool -> Int16BufferAttribute

    type [<AllowNullLiteral>] Uint16BufferAttribute =
        inherit BufferAttribute

    type [<AllowNullLiteral>] Uint16BufferAttributeStatic =
        [<EmitConstructor>] abstract Create: array: U4<Iterable<float>, ArrayLike<float>, ArrayBuffer, float> * itemSize: float * ?normalized: bool -> Uint16BufferAttribute

    type [<AllowNullLiteral>] Int32BufferAttribute =
        inherit BufferAttribute

    type [<AllowNullLiteral>] Int32BufferAttributeStatic =
        [<EmitConstructor>] abstract Create: array: U4<Iterable<float>, ArrayLike<float>, ArrayBuffer, float> * itemSize: float * ?normalized: bool -> Int32BufferAttribute

    type [<AllowNullLiteral>] Uint32BufferAttribute =
        inherit BufferAttribute

    type [<AllowNullLiteral>] Uint32BufferAttributeStatic =
        [<EmitConstructor>] abstract Create: array: U4<Iterable<float>, ArrayLike<float>, ArrayBuffer, float> * itemSize: float * ?normalized: bool -> Uint32BufferAttribute

    type [<AllowNullLiteral>] Float16BufferAttribute =
        inherit BufferAttribute

    type [<AllowNullLiteral>] Float16BufferAttributeStatic =
        [<EmitConstructor>] abstract Create: array: U4<Iterable<float>, ArrayLike<float>, ArrayBuffer, float> * itemSize: float * ?normalized: bool -> Float16BufferAttribute

    type [<AllowNullLiteral>] Float32BufferAttribute =
        inherit BufferAttribute

    type [<AllowNullLiteral>] Float32BufferAttributeStatic =
        [<EmitConstructor>] abstract Create: array: U4<Iterable<float>, ArrayLike<float>, ArrayBuffer, float> * itemSize: float * ?normalized: bool -> Float32BufferAttribute

    type [<AllowNullLiteral>] Float64BufferAttribute =
        inherit BufferAttribute

    type [<AllowNullLiteral>] Float64BufferAttributeStatic =
        [<EmitConstructor>] abstract Create: array: U4<Iterable<float>, ArrayLike<float>, ArrayBuffer, float> * itemSize: float * ?normalized: bool -> Float64BufferAttribute

module __core_BufferGeometry =
    type BufferAttribute = __core_BufferAttribute.BufferAttribute
    type Box3 = __math_Box3.Box3
    type Sphere = __math_Sphere.Sphere
    type Matrix4 = __math_Matrix4.Matrix4
    type Quaternion = __math_Quaternion.Quaternion
    type Vector2 = __math_Vector2.Vector2
    type Vector3 = __math_Vector3.Vector3
    type EventDispatcher = __core_EventDispatcher.EventDispatcher
    type InterleavedBufferAttribute = __core_InterleavedBufferAttribute.InterleavedBufferAttribute
    type BuiltinShaderAttributeName = Constants.BuiltinShaderAttributeName

    type [<AllowNullLiteral>] IExports =
        /// <summary>
        /// This is a superefficent class for geometries because it saves all data in buffers.
        /// It reduces memory costs and cpu cycles. But it is not as easy to work with because of all the necessary buffer calculations.
        /// It is mainly interesting when working with static objects.
        /// 
        /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/BufferGeometry.js">src/core/BufferGeometry.js</see>
        /// </summary>
        abstract BufferGeometry: BufferGeometryStatic

    /// <summary>
    /// This is a superefficent class for geometries because it saves all data in buffers.
    /// It reduces memory costs and cpu cycles. But it is not as easy to work with because of all the necessary buffer calculations.
    /// It is mainly interesting when working with static objects.
    /// 
    /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/BufferGeometry.js">src/core/BufferGeometry.js</see>
    /// </summary>
    type [<AllowNullLiteral>] BufferGeometry =
        inherit EventDispatcher
        /// Unique number of this buffergeometry instance
        abstract id: float with get, set
        abstract uuid: string with get, set
        /// <default>''</default>
        abstract name: string with get, set
        /// <default>'BufferGeometry'</default>
        abstract ``type``: string with get, set
        /// <default>null</default>
        abstract index: BufferAttribute option with get, set
        /// <default>{}</default>
        abstract attributes: BufferGeometryAttributes with get, set
        /// <default>{}</default>
        abstract morphAttributes: BufferGeometryMorphAttributes with get, set
        /// <default>false</default>
        abstract morphTargetsRelative: bool with get, set
        /// <default>[]</default>
        abstract groups: Array<{| start: float; count: float; materialIndex: float option |}> with get, set
        /// <default>null</default>
        abstract boundingBox: Box3 option with get, set
        /// <default>null</default>
        abstract boundingSphere: Sphere option with get, set
        /// <default>{ start: 0, count: Infinity }</default>
        abstract drawRange: {| start: float; count: float |} with get, set
        /// <default>{}</default>
        abstract userData: BufferGeometryUserData with get, set
        abstract isBufferGeometry: bool
        abstract getIndex: unit -> BufferAttribute option
        abstract setIndex: index: U2<BufferAttribute, ResizeArray<float>> option -> BufferGeometry
        abstract setAttribute: name: U2<BuiltinShaderAttributeName, obj> * attribute: U2<BufferAttribute, InterleavedBufferAttribute> -> BufferGeometry
        abstract getAttribute: name: U2<BuiltinShaderAttributeName, obj> -> U2<BufferAttribute, InterleavedBufferAttribute>
        abstract deleteAttribute: name: U2<BuiltinShaderAttributeName, obj> -> BufferGeometry
        abstract hasAttribute: name: U2<BuiltinShaderAttributeName, obj> -> bool
        abstract addGroup: start: float * count: float * ?materialIndex: float -> unit
        abstract clearGroups: unit -> unit
        abstract setDrawRange: start: float * count: float -> unit
        /// Bakes matrix transform directly into vertex coordinates.
        abstract applyMatrix4: matrix: Matrix4 -> BufferGeometry
        abstract applyQuaternion: q: Quaternion -> BufferGeometry
        abstract rotateX: angle: float -> BufferGeometry
        abstract rotateY: angle: float -> BufferGeometry
        abstract rotateZ: angle: float -> BufferGeometry
        abstract translate: x: float * y: float * z: float -> BufferGeometry
        abstract scale: x: float * y: float * z: float -> BufferGeometry
        abstract lookAt: v: Vector3 -> unit
        abstract center: unit -> BufferGeometry
        abstract setFromPoints: points: U2<ResizeArray<Vector3>, ResizeArray<Vector2>> -> BufferGeometry
        /// Computes bounding box of the geometry, updating Geometry.boundingBox attribute.
        /// Bounding boxes aren't computed by default. They need to be explicitly computed, otherwise they are null.
        abstract computeBoundingBox: unit -> unit
        /// Computes bounding sphere of the geometry, updating Geometry.boundingSphere attribute.
        /// Bounding spheres aren't' computed by default. They need to be explicitly computed, otherwise they are null.
        abstract computeBoundingSphere: unit -> unit
        /// Computes and adds tangent attribute to this geometry.
        abstract computeTangents: unit -> unit
        /// Computes vertex normals by averaging face normals.
        abstract computeVertexNormals: unit -> unit
        abstract merge: geometry: BufferGeometry * ?offset: float -> BufferGeometry
        abstract normalizeNormals: unit -> unit
        abstract toNonIndexed: unit -> BufferGeometry
        abstract toJSON: unit -> obj option
        abstract clone: unit -> BufferGeometry
        abstract copy: source: BufferGeometry -> BufferGeometry
        /// Disposes the object from memory.
        /// You need to call this when you want the bufferGeometry removed while the application is running.
        abstract dispose: unit -> unit
        [<Obsolete("Use {@link BufferGeometry#groups .groups} instead.")>]
        abstract drawcalls: obj option with get, set
        [<Obsolete("Use {@link BufferGeometry#groups .groups} instead.")>]
        abstract offsets: obj option with get, set
        [<Obsolete("Use {@link BufferGeometry#setIndex .setIndex()} instead.")>]
        abstract addIndex: index: obj option -> unit
        [<Obsolete("Use {@link BufferGeometry#addGroup .addGroup()} instead.")>]
        abstract addDrawCall: start: obj option * count: obj option * ?indexOffset: obj -> unit
        [<Obsolete("Use {@link BufferGeometry#clearGroups .clearGroups()} instead.")>]
        abstract clearDrawCalls: unit -> unit
        [<Obsolete("Use {@link BufferGeometry#setAttribute .setAttribute()} instead.")>]
        abstract addAttribute: name: string * attribute: U2<BufferAttribute, InterleavedBufferAttribute> -> BufferGeometry
        abstract addAttribute: name: obj option * array: obj option * itemSize: obj option -> obj option
        [<Obsolete("Use {@link BufferGeometry#deleteAttribute .deleteAttribute()} instead.")>]
        abstract removeAttribute: name: string -> BufferGeometry

    /// <summary>
    /// This is a superefficent class for geometries because it saves all data in buffers.
    /// It reduces memory costs and cpu cycles. But it is not as easy to work with because of all the necessary buffer calculations.
    /// It is mainly interesting when working with static objects.
    /// 
    /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/BufferGeometry.js">src/core/BufferGeometry.js</see>
    /// </summary>
    type [<AllowNullLiteral>] BufferGeometryStatic =
        /// This creates a new BufferGeometry. It also sets several properties to an default value.
        [<EmitConstructor>] abstract Create: unit -> BufferGeometry
        abstract MaxIndex: float with get, set

    type [<AllowNullLiteral>] BufferGeometryAttributes =
        [<EmitIndexer>] abstract Item: name: string -> U2<BufferAttribute, InterleavedBufferAttribute> with get, set

    type [<AllowNullLiteral>] BufferGeometryMorphAttributes =
        [<EmitIndexer>] abstract Item: name: string -> Array<U2<BufferAttribute, InterleavedBufferAttribute>> with get, set

    type [<AllowNullLiteral>] BufferGeometryUserData =
        [<EmitIndexer>] abstract Item: key: string -> obj option with get, set

module __core_Clock =

    type [<AllowNullLiteral>] IExports =
        /// <summary>
        /// Object for keeping track of time.
        /// 
        /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/Clock.js">src/core/Clock.js</see>
        /// </summary>
        abstract Clock: ClockStatic

    /// <summary>
    /// Object for keeping track of time.
    /// 
    /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/Clock.js">src/core/Clock.js</see>
    /// </summary>
    type [<AllowNullLiteral>] Clock =
        /// <summary>If set, starts the clock automatically when the first update is called.</summary>
        /// <default>true</default>
        abstract autoStart: bool with get, set
        /// <summary>
        /// When the clock is running, It holds the starttime of the clock.
        /// This counted from the number of milliseconds elapsed since 1 January 1970 00:00:00 UTC.
        /// </summary>
        /// <default>0</default>
        abstract startTime: float with get, set
        /// <summary>
        /// When the clock is running, It holds the previous time from a update.
        /// This counted from the number of milliseconds elapsed since 1 January 1970 00:00:00 UTC.
        /// </summary>
        /// <default>0</default>
        abstract oldTime: float with get, set
        /// <summary>
        /// When the clock is running, It holds the time elapsed between the start of the clock to the previous update.
        /// This parameter is in seconds of three decimal places.
        /// </summary>
        /// <default>0</default>
        abstract elapsedTime: float with get, set
        /// <summary>This property keeps track whether the clock is running or not.</summary>
        /// <default>false</default>
        abstract running: bool with get, set
        /// Starts clock.
        abstract start: unit -> unit
        /// Stops clock.
        abstract stop: unit -> unit
        /// Get the seconds passed since the clock started.
        abstract getElapsedTime: unit -> float
        /// Get the seconds passed since the last call to this method.
        abstract getDelta: unit -> float

    /// <summary>
    /// Object for keeping track of time.
    /// 
    /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/Clock.js">src/core/Clock.js</see>
    /// </summary>
    type [<AllowNullLiteral>] ClockStatic =
        /// <param name="autoStart">Automatically start the clock.</param>
        [<EmitConstructor>] abstract Create: ?autoStart: bool -> Clock

module __core_EventDispatcher =

    type [<AllowNullLiteral>] IExports =
        /// <summary>JavaScript events for custom objects</summary>
        abstract EventDispatcher: EventDispatcherStatic

    /// Event object.
    type [<AllowNullLiteral>] Event =
        abstract ``type``: string with get, set
        abstract target: obj option with get, set
        [<EmitIndexer>] abstract Item: attachment: string -> obj option with get, set

    /// <summary>JavaScript events for custom objects</summary>
    type [<AllowNullLiteral>] EventDispatcher =
        /// <summary>Adds a listener to an event type.</summary>
        /// <param name="type">The type of event to listen to.</param>
        /// <param name="listener">The function that gets called when the event is fired.</param>
        abstract addEventListener: ``type``: string * listener: (Event -> unit) -> unit
        /// <summary>Checks if listener is added to an event type.</summary>
        /// <param name="type">The type of event to listen to.</param>
        /// <param name="listener">The function that gets called when the event is fired.</param>
        abstract hasEventListener: ``type``: string * listener: (Event -> unit) -> bool
        /// <summary>Removes a listener from an event type.</summary>
        /// <param name="type">The type of the listener that gets removed.</param>
        /// <param name="listener">The listener function that gets removed.</param>
        abstract removeEventListener: ``type``: string * listener: (Event -> unit) -> unit
        /// <summary>Fire an event type.</summary>
        /// <param name="type">The type of event that gets fired.</param>
        abstract dispatchEvent: ``event``: EventDispatcherDispatchEventEvent -> unit

    /// <summary>
    /// Typescript interface contains an <see href="https://www.typescriptlang.org/docs/handbook/2/objects.html#index-signatures">index signature</see> (like <c>{ [key:string]: string }</c>).
    /// Unlike an indexer in F#, index signatures index over a type's members.
    /// 
    /// As such an index signature cannot be implemented via regular F# Indexer (<c>Item</c> property),
    /// but instead by just specifying fields.
    /// 
    /// Easiest way to declare such a type is with an Anonymous Record and force it into the function.
    /// For example:
    /// <code lang="fsharp">
    /// type I =
    ///     [&lt;EmitIndexer&gt;]
    ///     abstract Item: string -&gt; string
    /// let f (i: I) = jsNative
    /// 
    /// let t = {| Value1 = "foo"; Value2 = "bar" |}
    /// f (!! t)
    /// </code>
    /// </summary>
    type [<AllowNullLiteral>] EventDispatcherDispatchEventEvent =
        abstract ``type``: string with get, set
        [<EmitIndexer>] abstract Item: attachment: string -> obj option with get, set

    /// <summary>JavaScript events for custom objects</summary>
    type [<AllowNullLiteral>] EventDispatcherStatic =
        /// Creates eventDispatcher object. It needs to be call with '.call' to add the functionality to an object.
        [<EmitConstructor>] abstract Create: unit -> EventDispatcher

module __core_GLBufferAttribute =

    type [<AllowNullLiteral>] IExports =
        /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/GLBufferAttribute.js">src/core/GLBufferAttribute.js</see></summary>
        abstract GLBufferAttribute: GLBufferAttributeStatic

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/GLBufferAttribute.js">src/core/GLBufferAttribute.js</see></summary>
    type [<AllowNullLiteral>] GLBufferAttribute =
        abstract buffer: WebGLBuffer with get, set
        abstract ``type``: float with get, set
        abstract itemSize: float with get, set
        abstract elementSize: GLBufferAttributeElementSize with get, set
        abstract count: float with get, set
        abstract version: float with get, set
        abstract isGLBufferAttribute: bool
        abstract needsUpdate: bool with set
        abstract setBuffer: buffer: WebGLBuffer -> GLBufferAttribute
        abstract setType: ``type``: float * elementSize: GLBufferAttributeElementSize -> GLBufferAttribute
        abstract setItemSize: itemSize: float -> GLBufferAttribute
        abstract setCount: count: float -> GLBufferAttribute

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/GLBufferAttribute.js">src/core/GLBufferAttribute.js</see></summary>
    type [<AllowNullLiteral>] GLBufferAttributeStatic =
        [<EmitConstructor>] abstract Create: buffer: WebGLBuffer * ``type``: float * itemSize: float * elementSize: GLBufferAttributeElementSize * count: float -> GLBufferAttribute

    type [<RequireQualifiedAccess>] GLBufferAttributeElementSize =
        | N1 = 1
        | N2 = 2
        | N4 = 4

module __core_InstancedBufferAttribute =
    type BufferGeometry = __core_BufferGeometry.BufferGeometry
    type BufferAttribute = __core_BufferAttribute.BufferAttribute
    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/examples/jsm/utils/BufferGeometryUtils.js">examples/jsm/utils/BufferGeometryUtils.js</see></summary>
    let [<Import("BufferGeometryUtils","three/core/InstancedBufferAttribute")>] bufferGeometryUtils: BufferGeometryUtils.IExports = jsNative
    [<Obsolete("")>]
    let [<Import("GeometryUtils","three/core/InstancedBufferAttribute")>] geometryUtils: GeometryUtils.IExports = jsNative

    type [<AllowNullLiteral>] IExports =
        /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/InstancedBufferAttribute.js">src/core/InstancedBufferAttribute.js</see></summary>
        abstract InstancedBufferAttribute: InstancedBufferAttributeStatic

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/examples/jsm/utils/BufferGeometryUtils.js">examples/jsm/utils/BufferGeometryUtils.js</see></summary>
    module BufferGeometryUtils =

        type [<AllowNullLiteral>] IExports =
            abstract mergeBufferGeometries: geometries: ResizeArray<BufferGeometry> -> BufferGeometry
            abstract computeTangents: geometry: BufferGeometry -> obj
            abstract mergeBufferAttributes: attributes: ResizeArray<BufferAttribute> -> BufferAttribute


    module GeometryUtils =

        type [<AllowNullLiteral>] IExports =
            [<Obsolete("Use {@link Geometry#merge geometry.merge( geometry2, matrix, materialIndexOffset )} instead.")>]
            abstract merge: geometry1: obj option * geometry2: obj option * ?materialIndexOffset: obj -> obj option
            [<Obsolete("Use {@link Geometry#center geometry.center()} instead.")>]
            abstract center: geometry: obj option -> obj option

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/InstancedBufferAttribute.js">src/core/InstancedBufferAttribute.js</see></summary>
    type [<AllowNullLiteral>] InstancedBufferAttribute =
        inherit BufferAttribute
        /// <default>1</default>
        abstract meshPerAttribute: float with get, set

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/InstancedBufferAttribute.js">src/core/InstancedBufferAttribute.js</see></summary>
    type [<AllowNullLiteral>] InstancedBufferAttributeStatic =
        [<EmitConstructor>] abstract Create: array: ArrayLike<float> * itemSize: float * ?normalized: bool * ?meshPerAttribute: float -> InstancedBufferAttribute

module __core_InstancedBufferGeometry =
    type BufferGeometry = __core_BufferGeometry.BufferGeometry

    type [<AllowNullLiteral>] IExports =
        /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/InstancedBufferGeometry.js">src/core/InstancedBufferGeometry.js</see></summary>
        abstract InstancedBufferGeometry: InstancedBufferGeometryStatic

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/InstancedBufferGeometry.js">src/core/InstancedBufferGeometry.js</see></summary>
    type [<AllowNullLiteral>] InstancedBufferGeometry =
        inherit BufferGeometry
        /// <default>'InstancedBufferGeometry</default>
        abstract ``type``: string with get, set
        abstract isInstancedBufferGeometry: bool with get, set
        abstract groups: Array<{| start: float; count: float; instances: float |}> with get, set
        /// <default>Infinity</default>
        abstract instanceCount: float with get, set
        abstract addGroup: start: float * count: float * instances: float -> unit

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/InstancedBufferGeometry.js">src/core/InstancedBufferGeometry.js</see></summary>
    type [<AllowNullLiteral>] InstancedBufferGeometryStatic =
        [<EmitConstructor>] abstract Create: unit -> InstancedBufferGeometry

module __core_InstancedInterleavedBuffer =
    type InterleavedBuffer = __core_InterleavedBuffer.InterleavedBuffer

    type [<AllowNullLiteral>] IExports =
        /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/InstancedInterleavedBuffer.js">src/core/InstancedInterleavedBuffer.js</see></summary>
        abstract InstancedInterleavedBuffer: InstancedInterleavedBufferStatic

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/InstancedInterleavedBuffer.js">src/core/InstancedInterleavedBuffer.js</see></summary>
    type [<AllowNullLiteral>] InstancedInterleavedBuffer =
        inherit InterleavedBuffer
        /// <default>1</default>
        abstract meshPerAttribute: float with get, set

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/InstancedInterleavedBuffer.js">src/core/InstancedInterleavedBuffer.js</see></summary>
    type [<AllowNullLiteral>] InstancedInterleavedBufferStatic =
        [<EmitConstructor>] abstract Create: array: ArrayLike<float> * stride: float * ?meshPerAttribute: float -> InstancedInterleavedBuffer

module __core_InterleavedBuffer =
    type InterleavedBufferAttribute = __core_InterleavedBufferAttribute.InterleavedBufferAttribute
    type Usage = Constants.Usage

    type [<AllowNullLiteral>] IExports =
        /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/InterleavedBuffer.js">src/core/InterleavedBuffer.js</see></summary>
        abstract InterleavedBuffer: InterleavedBufferStatic

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/InterleavedBuffer.js">src/core/InterleavedBuffer.js</see></summary>
    type [<AllowNullLiteral>] InterleavedBuffer =
        abstract array: ArrayLike<float> with get, set
        abstract stride: float with get, set
        /// <default>THREE.StaticDrawUsage</default>
        abstract usage: Usage with get, set
        /// <default>{ offset: number; count: number }</default>
        abstract updateRange: {| offset: float; count: float |} with get, set
        /// <default>0</default>
        abstract version: float with get, set
        abstract length: float with get, set
        /// <default>0</default>
        abstract count: float with get, set
        abstract needsUpdate: bool with get, set
        abstract uuid: string with get, set
        abstract setUsage: usage: Usage -> InterleavedBuffer
        abstract clone: data: obj -> InterleavedBuffer
        abstract copy: source: InterleavedBuffer -> InterleavedBuffer
        abstract copyAt: index1: float * attribute: InterleavedBufferAttribute * index2: float -> InterleavedBuffer
        abstract set: value: ArrayLike<float> * index: float -> InterleavedBuffer
        abstract toJSON: data: obj -> {| uuid: string; buffer: string; ``type``: string; stride: float |}

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/InterleavedBuffer.js">src/core/InterleavedBuffer.js</see></summary>
    type [<AllowNullLiteral>] InterleavedBufferStatic =
        [<EmitConstructor>] abstract Create: array: ArrayLike<float> * stride: float -> InterleavedBuffer

module __core_InterleavedBufferAttribute =
    type BufferAttribute = __core_BufferAttribute.BufferAttribute
    type InterleavedBuffer = __core_InterleavedBuffer.InterleavedBuffer
    type Matrix4 = __math_Matrix4.Matrix4
    type Matrix = __math_Matrix3.Matrix

    type [<AllowNullLiteral>] IExports =
        /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/InterleavedBufferAttribute.js">src/core/InterleavedBufferAttribute.js</see></summary>
        abstract InterleavedBufferAttribute: InterleavedBufferAttributeStatic

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/InterleavedBufferAttribute.js">src/core/InterleavedBufferAttribute.js</see></summary>
    type [<AllowNullLiteral>] InterleavedBufferAttribute =
        /// <default>''</default>
        abstract name: string with get, set
        abstract data: InterleavedBuffer with get, set
        abstract itemSize: float with get, set
        abstract offset: float with get, set
        /// <default>false</default>
        abstract normalized: bool with get, set
        abstract count: float
        abstract array: ArrayLike<float>
        abstract needsUpdate: bool with set
        abstract isInterleavedBufferAttribute: bool
        abstract applyMatrix4: m: Matrix4 -> InterleavedBufferAttribute
        abstract clone: ?data: obj -> BufferAttribute
        abstract getX: index: float -> float
        abstract setX: index: float * x: float -> InterleavedBufferAttribute
        abstract getY: index: float -> float
        abstract setY: index: float * y: float -> InterleavedBufferAttribute
        abstract getZ: index: float -> float
        abstract setZ: index: float * z: float -> InterleavedBufferAttribute
        abstract getW: index: float -> float
        abstract setW: index: float * z: float -> InterleavedBufferAttribute
        abstract setXY: index: float * x: float * y: float -> InterleavedBufferAttribute
        abstract setXYZ: index: float * x: float * y: float * z: float -> InterleavedBufferAttribute
        abstract setXYZW: index: float * x: float * y: float * z: float * w: float -> InterleavedBufferAttribute
        abstract toJSON: ?data: obj -> InterleavedBufferAttributeToJSONReturn
        abstract applyNormalMatrix: matrix: Matrix -> InterleavedBufferAttribute
        abstract transformDirection: matrix: Matrix -> InterleavedBufferAttribute

    type [<AllowNullLiteral>] InterleavedBufferAttributeToJSONReturn =
        abstract isInterleavedBufferAttribute: bool with get, set
        abstract itemSize: float with get, set
        abstract data: string with get, set
        abstract offset: float with get, set
        abstract normalized: bool with get, set

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/core/InterleavedBufferAttribute.js">src/core/InterleavedBufferAttribute.js</see></summary>
    type [<AllowNullLiteral>] InterleavedBufferAttributeStatic =
        [<EmitConstructor>] abstract Create: interleavedBuffer: InterleavedBuffer * itemSize: float * offset: float * ?normalized: bool -> InterleavedBufferAttribute

module __core_Layers =

    type [<AllowNullLiteral>] IExports =
        abstract Layers: LayersStatic

    type [<AllowNullLiteral>] Layers =
        /// <default>1 | 0</default>
        abstract mask: float with get, set
        abstract set: channel: float -> unit
        abstract enable: channel: float -> unit
        abstract enableAll: unit -> unit
        abstract toggle: channel: float -> unit
        abstract disable: channel: float -> unit
        abstract disableAll: unit -> unit
        abstract test: layers: Layers -> bool

    type [<AllowNullLiteral>] LayersStatic =
        [<EmitConstructor>] abstract Create: unit -> Layers

module __core_Object3D =
    type Vector3 = __math_Vector3.Vector3
    type Euler = __math_Euler.Euler
    type Quaternion = __math_Quaternion.Quaternion
    type Matrix4 = __math_Matrix4.Matrix4
    type Matrix3 = __math_Matrix3.Matrix3
    type Layers = __core_Layers.Layers
    type WebGLRenderer = __renderers_WebGLRenderer.WebGLRenderer
    type Scene = __scenes_Scene.Scene
    type Camera = __cameras_Camera.Camera
    type Material = __materials_Material.Material
    type Group = __objects_Group.Group
    type Intersection = __core_Raycaster.Intersection
    type Raycaster = __core_Raycaster.Raycaster
    type EventDispatcher = __core_EventDispatcher.EventDispatcher
    type BufferGeometry = __core_BufferGeometry.BufferGeometry
    type AnimationClip = __animation_AnimationClip.AnimationClip

    type [<AllowNullLiteral>] IExports =
        /// Base class for scene graph objects
        abstract Object3D: Object3DStatic

    /// Base class for scene graph objects
    type [<AllowNullLiteral>] Object3D =
        inherit EventDispatcher
        /// Unique number of this object instance.
        abstract id: float with get, set
        abstract uuid: string with get, set
        /// <summary>Optional name of the object (doesn't need to be unique).</summary>
        /// <default>''</default>
        abstract name: string with get, set
        /// <default>'Object3D'</default>
        abstract ``type``: string with get, set
        /// <summary>Object's parent in the scene graph.</summary>
        /// <default>null</default>
        abstract parent: Object3D option with get, set
        /// <summary>Array with object's children.</summary>
        /// <default>[]</default>
        abstract children: ResizeArray<Object3D> with get, set
        /// <summary>Up direction.</summary>
        /// <default>THREE.Object3D.DefaultUp.clone()</default>
        abstract up: Vector3 with get, set
        /// <summary>Object's local position.</summary>
        /// <default>new THREE.Vector3()</default>
        abstract position: Vector3
        /// <summary>Object's local rotation (Euler angles), in radians.</summary>
        /// <default>new THREE.Euler()</default>
        abstract rotation: Euler
        /// <summary>Object's local rotation as a Quaternion.</summary>
        /// <default>new THREE.Quaternion()</default>
        abstract quaternion: Quaternion
        /// <summary>Object's local scale.</summary>
        /// <default>new THREE.Vector3()</default>
        abstract scale: Vector3
        /// <default>new THREE.Matrix4()</default>
        abstract modelViewMatrix: Matrix4
        /// <default>new THREE.Matrix3()</default>
        abstract normalMatrix: Matrix3
        /// <summary>Local transform.</summary>
        /// <default>new THREE.Matrix4()</default>
        abstract matrix: Matrix4 with get, set
        /// <summary>The global transform of the object. If the Object3d has no parent, then it's identical to the local transform.</summary>
        /// <default>new THREE.Matrix4()</default>
        abstract matrixWorld: Matrix4 with get, set
        /// <summary>
        /// When this is set, it calculates the matrix of position, (rotation or quaternion) and scale every frame and also
        /// recalculates the matrixWorld property.
        /// </summary>
        /// <default>THREE.Object3D.DefaultMatrixAutoUpdate</default>
        abstract matrixAutoUpdate: bool with get, set
        /// <summary>When this is set, it calculates the matrixWorld in that frame and resets this property to false.</summary>
        /// <default>false</default>
        abstract matrixWorldNeedsUpdate: bool with get, set
        /// <default>new THREE.Layers()</default>
        abstract layers: Layers with get, set
        /// <summary>Object gets rendered if true.</summary>
        /// <default>true</default>
        abstract visible: bool with get, set
        /// <summary>Gets rendered into shadow map.</summary>
        /// <default>false</default>
        abstract castShadow: bool with get, set
        /// <summary>Material gets baked in shadow receiving.</summary>
        /// <default>false</default>
        abstract receiveShadow: bool with get, set
        /// <summary>
        /// When this is set, it checks every frame if the object is in the frustum of the camera before rendering the object.
        /// If set to false the object gets rendered every frame even if it is not in the frustum of the camera.
        /// </summary>
        /// <default>true</default>
        abstract frustumCulled: bool with get, set
        /// <summary>
        /// Overrides the default rendering order of scene graph objects, from lowest to highest renderOrder.
        /// Opaque and transparent objects remain sorted independently though.
        /// When this property is set for an instance of Group, all descendants objects will be sorted and rendered together.
        /// </summary>
        /// <default>0</default>
        abstract renderOrder: float with get, set
        /// <summary>Array with animation clips.</summary>
        /// <default>[]</default>
        abstract animations: ResizeArray<AnimationClip> with get, set
        /// <summary>An object that can be used to store custom data about the Object3d. It should not hold references to functions as these will not be cloned.</summary>
        /// <default>{}</default>
        abstract userData: Object3DUserData with get, set
        /// Custom depth material to be used when rendering to the depth map. Can only be used in context of meshes.
        /// When shadow-casting with a DirectionalLight or SpotLight, if you are (a) modifying vertex positions in
        /// the vertex shader, (b) using a displacement map, (c) using an alpha map with alphaTest, or (d) using a
        /// transparent texture with alphaTest, you must specify a customDepthMaterial for proper shadows.
        abstract customDepthMaterial: Material with get, set
        /// Same as customDepthMaterial, but used with PointLight.
        abstract customDistanceMaterial: Material with get, set
        /// Used to check whether this or derived classes are Object3Ds. Default is true.
        /// You should not change this, as it is used internally for optimisation.
        abstract isObject3D: bool
        /// Calls before rendering object
        abstract onBeforeRender: (WebGLRenderer -> Scene -> Camera -> BufferGeometry -> Material -> Group -> unit) with get, set
        /// Calls after rendering object
        abstract onAfterRender: (WebGLRenderer -> Scene -> Camera -> BufferGeometry -> Material -> Group -> unit) with get, set
        /// This updates the position, rotation and scale with the matrix.
        abstract applyMatrix4: matrix: Matrix4 -> unit
        abstract applyQuaternion: quaternion: Quaternion -> Object3D
        abstract setRotationFromAxisAngle: axis: Vector3 * angle: float -> unit
        abstract setRotationFromEuler: euler: Euler -> unit
        abstract setRotationFromMatrix: m: Matrix4 -> unit
        abstract setRotationFromQuaternion: q: Quaternion -> unit
        /// <summary>Rotate an object along an axis in object space. The axis is assumed to be normalized.</summary>
        /// <param name="axis">A normalized vector in object space.</param>
        /// <param name="angle">The angle in radians.</param>
        abstract rotateOnAxis: axis: Vector3 * angle: float -> Object3D
        /// <summary>Rotate an object along an axis in world space. The axis is assumed to be normalized. Method Assumes no rotated parent.</summary>
        /// <param name="axis">A normalized vector in object space.</param>
        /// <param name="angle">The angle in radians.</param>
        abstract rotateOnWorldAxis: axis: Vector3 * angle: float -> Object3D
        /// <param name="angle" />
        abstract rotateX: angle: float -> Object3D
        /// <param name="angle" />
        abstract rotateY: angle: float -> Object3D
        /// <param name="angle" />
        abstract rotateZ: angle: float -> Object3D
        /// <param name="axis">A normalized vector in object space.</param>
        /// <param name="distance">The distance to translate.</param>
        abstract translateOnAxis: axis: Vector3 * distance: float -> Object3D
        /// <summary>Translates object along x axis by distance.</summary>
        /// <param name="distance">Distance.</param>
        abstract translateX: distance: float -> Object3D
        /// <summary>Translates object along y axis by distance.</summary>
        /// <param name="distance">Distance.</param>
        abstract translateY: distance: float -> Object3D
        /// <summary>Translates object along z axis by distance.</summary>
        /// <param name="distance">Distance.</param>
        abstract translateZ: distance: float -> Object3D
        /// <summary>Updates the vector from local space to world space.</summary>
        /// <param name="vector">A local vector.</param>
        abstract localToWorld: vector: Vector3 -> Vector3
        /// <summary>Updates the vector from world space to local space.</summary>
        /// <param name="vector">A world vector.</param>
        abstract worldToLocal: vector: Vector3 -> Vector3
        /// <summary>Rotates object to face point in space.</summary>
        /// <param name="vector">A world vector to look at.</param>
        abstract lookAt: vector: U2<Vector3, float> * ?y: float * ?z: float -> unit
        /// Adds object as child of this object.
        abstract add: [<ParamArray>] object: Object3D[] -> Object3D
        /// Removes object as child of this object.
        abstract remove: [<ParamArray>] object: Object3D[] -> Object3D
        /// Removes this object from its current parent.
        abstract removeFromParent: unit -> Object3D
        /// Removes all child objects.
        abstract clear: unit -> Object3D
        /// Adds object as a child of this, while maintaining the object's world transform.
        abstract attach: object: Object3D -> Object3D
        /// <summary>Searches through the object's children and returns the first with a matching id.</summary>
        /// <param name="id">Unique number of the object instance</param>
        abstract getObjectById: id: float -> Object3D option
        /// <summary>Searches through the object's children and returns the first with a matching name.</summary>
        /// <param name="name">String to match to the children's Object3d.name property.</param>
        abstract getObjectByName: name: string -> Object3D option
        abstract getObjectByProperty: name: string * value: string -> Object3D option
        abstract getWorldPosition: target: Vector3 -> Vector3
        abstract getWorldQuaternion: target: Quaternion -> Quaternion
        abstract getWorldScale: target: Vector3 -> Vector3
        abstract getWorldDirection: target: Vector3 -> Vector3
        abstract raycast: raycaster: Raycaster * intersects: ResizeArray<Intersection> -> unit
        abstract traverse: callback: (Object3D -> obj option) -> unit
        abstract traverseVisible: callback: (Object3D -> obj option) -> unit
        abstract traverseAncestors: callback: (Object3D -> obj option) -> unit
        /// Updates local transform.
        abstract updateMatrix: unit -> unit
        /// Updates global transform of the object and its children.
        abstract updateMatrixWorld: ?force: bool -> unit
        abstract updateWorldMatrix: updateParents: bool * updateChildren: bool -> unit
        abstract toJSON: ?meta: {| geometries: obj option; materials: obj option; textures: obj option; images: obj option |} -> obj option
        abstract clone: ?recursive: bool -> Object3D
        /// <param name="object" />
        /// <param name="recursive" />
        abstract copy: source: Object3D * ?recursive: bool -> Object3D

    /// Base class for scene graph objects
    type [<AllowNullLiteral>] Object3DStatic =
        [<EmitConstructor>] abstract Create: unit -> Object3D
        abstract DefaultUp: Vector3 with get, set
        abstract DefaultMatrixAutoUpdate: bool with get, set

    type [<AllowNullLiteral>] Object3DUserData =
        [<EmitIndexer>] abstract Item: key: string -> obj option with get, set

module __core_Raycaster =
    type Vector3 = __math_Vector3.Vector3
    type Object3D = __core_Object3D.Object3D
    type Vector2 = __math_Vector2.Vector2
    type Ray = __math_Ray.Ray
    type Camera = __cameras_Camera.Camera
    type Layers = __core_Layers.Layers

    type [<AllowNullLiteral>] IExports =
        abstract Raycaster: RaycasterStatic

    type [<AllowNullLiteral>] Face =
        abstract a: float with get, set
        abstract b: float with get, set
        abstract c: float with get, set
        abstract normal: Vector3 with get, set
        abstract materialIndex: float with get, set

    type [<AllowNullLiteral>] Intersection =
        abstract distance: float with get, set
        abstract distanceToRay: float option with get, set
        abstract point: Vector3 with get, set
        abstract index: float option with get, set
        abstract face: Face option with get, set
        abstract faceIndex: float option with get, set
        abstract object: Object3D with get, set
        abstract uv: Vector2 option with get, set
        abstract instanceId: float option with get, set

    type [<AllowNullLiteral>] RaycasterParameters =
        abstract Mesh: obj option with get, set
        abstract Line: {| threshold: float |} option with get, set
        abstract LOD: obj option with get, set
        abstract Points: {| threshold: float |} option with get, set
        abstract Sprite: obj option with get, set

    type [<AllowNullLiteral>] Raycaster =
        /// The Ray used for the raycasting.
        abstract ray: Ray with get, set
        /// <summary>
        /// The near factor of the raycaster. This value indicates which objects can be discarded based on the
        /// distance. This value shouldn't be negative and should be smaller than the far property.
        /// </summary>
        /// <default>0</default>
        abstract near: float with get, set
        /// <summary>
        /// The far factor of the raycaster. This value indicates which objects can be discarded based on the
        /// distance. This value shouldn't be negative and should be larger than the near property.
        /// </summary>
        /// <default>Infinity</default>
        abstract far: float with get, set
        /// The camera to use when raycasting against view-dependent objects such as billboarded objects like Sprites. This field
        /// can be set manually or is set when calling "setFromCamera".
        abstract camera: Camera with get, set
        /// <summary>Used by Raycaster to selectively ignore 3D objects when performing intersection tests.</summary>
        /// <default>new THREE.Layers()</default>
        abstract layers: Layers with get, set
        /// <default>{ Mesh: {}, Line: { threshold: 1 }, LOD: {}, Points: { threshold: 1 }, Sprite: {} }</default>
        abstract ``params``: RaycasterParameters with get, set
        /// <summary>Updates the ray with a new origin and direction.</summary>
        /// <param name="origin">The origin vector where the ray casts from.</param>
        /// <param name="direction">The normalized direction vector that gives direction to the ray.</param>
        abstract set: origin: Vector3 * direction: Vector3 -> unit
        /// <summary>Updates the ray with a new origin and direction.</summary>
        /// <param name="coords">2D coordinates of the mouse, in normalized device coordinates (NDC)---X and Y components should be between -1 and 1.</param>
        /// <param name="camera">camera from which the ray should originate</param>
        abstract setFromCamera: coords: {| x: float; y: float |} * camera: Camera -> unit
        /// <summary>Checks all intersection between the ray and the object with or without the descendants. Intersections are returned sorted by distance, closest first.</summary>
        /// <param name="object">The object to check for intersection with the ray.</param>
        /// <param name="recursive">If true, it also checks all descendants. Otherwise it only checks intersecton with the object. Default is false.</param>
        /// <param name="optionalTarget">(optional) target to set the result. Otherwise a new Array is instantiated. If set, you must clear this array prior to each call (i.e., array.length = 0;).</param>
        abstract intersectObject: object: Object3D * ?recursive: bool * ?optionalTarget: ResizeArray<Intersection> -> ResizeArray<Intersection>
        /// <summary>
        /// Checks all intersection between the ray and the objects with or without the descendants.
        /// Intersections are returned sorted by distance, closest first.
        /// Intersections are of the same form as those returned by .intersectObject.
        /// </summary>
        /// <param name="objects">The objects to check for intersection with the ray.</param>
        /// <param name="recursive">If true, it also checks all descendants of the objects. Otherwise it only checks intersecton with the objects. Default is false.</param>
        /// <param name="optionalTarget">(optional) target to set the result. Otherwise a new Array is instantiated. If set, you must clear this array prior to each call (i.e., array.length = 0;).</param>
        abstract intersectObjects: objects: ResizeArray<Object3D> * ?recursive: bool * ?optionalTarget: ResizeArray<Intersection> -> ResizeArray<Intersection>

    type [<AllowNullLiteral>] RaycasterStatic =
        /// <summary>This creates a new raycaster object.</summary>
        /// <param name="origin">The origin vector where the ray casts from.</param>
        /// <param name="direction">The direction vector that gives direction to the ray. Should be normalized.</param>
        /// <param name="near">All results returned are further away than near. Near can't be negative. Default value is 0.</param>
        /// <param name="far">All results returned are closer then far. Far can't be lower then near . Default value is Infinity.</param>
        [<EmitConstructor>] abstract Create: ?origin: Vector3 * ?direction: Vector3 * ?near: float * ?far: float -> Raycaster



module __math_Box2 =
    type Vector2 = __math_Vector2.Vector2

    type [<AllowNullLiteral>] IExports =
        abstract Box2: Box2Static

    type [<AllowNullLiteral>] Box2 =
        /// <default>new THREE.Vector2( + Infinity, + Infinity )</default>
        abstract min: Vector2 with get, set
        /// <default>new THREE.Vector2( - Infinity, - Infinity )</default>
        abstract max: Vector2 with get, set
        abstract set: min: Vector2 * max: Vector2 -> Box2
        abstract setFromPoints: points: ResizeArray<Vector2> -> Box2
        abstract setFromCenterAndSize: center: Vector2 * size: Vector2 -> Box2
        abstract clone: unit -> Box2
        abstract copy: box: Box2 -> Box2
        abstract makeEmpty: unit -> Box2
        abstract isEmpty: unit -> bool
        abstract getCenter: target: Vector2 -> Vector2
        abstract getSize: target: Vector2 -> Vector2
        abstract expandByPoint: point: Vector2 -> Box2
        abstract expandByVector: vector: Vector2 -> Box2
        abstract expandByScalar: scalar: float -> Box2
        abstract containsPoint: point: Vector2 -> bool
        abstract containsBox: box: Box2 -> bool
        abstract getParameter: point: Vector2 * target: Vector2 -> Vector2
        abstract intersectsBox: box: Box2 -> bool
        abstract clampPoint: point: Vector2 * target: Vector2 -> Vector2
        abstract distanceToPoint: point: Vector2 -> float
        abstract intersect: box: Box2 -> Box2
        abstract union: box: Box2 -> Box2
        abstract translate: offset: Vector2 -> Box2
        abstract equals: box: Box2 -> bool
        [<Obsolete("Use {@link Box2#isEmpty .isEmpty()} instead.")>]
        abstract empty: unit -> obj option
        [<Obsolete("Use {@link Box2#intersectsBox .intersectsBox()} instead.")>]
        abstract isIntersectionBox: b: obj option -> obj option

    type [<AllowNullLiteral>] Box2Static =
        [<EmitConstructor>] abstract Create: ?min: Vector2 * ?max: Vector2 -> Box2

module __math_Box3 =
    type BufferAttribute = __core_BufferAttribute.BufferAttribute
    type Vector3 = __math_Vector3.Vector3
    type Object3D = __core_Object3D.Object3D
    type Sphere = __math_Sphere.Sphere
    type Plane = __math_Plane.Plane
    type Matrix4 = __math_Matrix4.Matrix4
    type Triangle = __math_Triangle.Triangle

    type [<AllowNullLiteral>] IExports =
        abstract Box3: Box3Static

    type [<AllowNullLiteral>] Box3 =
        /// <default>new THREE.Vector3( + Infinity, + Infinity, + Infinity )</default>
        abstract min: Vector3 with get, set
        /// <default>new THREE.Vector3( - Infinity, - Infinity, - Infinity )</default>
        abstract max: Vector3 with get, set
        abstract isBox3: bool
        abstract set: min: Vector3 * max: Vector3 -> Box3
        abstract setFromArray: array: ArrayLike<float> -> Box3
        abstract setFromBufferAttribute: bufferAttribute: BufferAttribute -> Box3
        abstract setFromPoints: points: ResizeArray<Vector3> -> Box3
        abstract setFromCenterAndSize: center: Vector3 * size: Vector3 -> Box3
        abstract setFromObject: object: Object3D -> Box3
        abstract clone: unit -> Box3
        abstract copy: box: Box3 -> Box3
        abstract makeEmpty: unit -> Box3
        abstract isEmpty: unit -> bool
        abstract getCenter: target: Vector3 -> Vector3
        abstract getSize: target: Vector3 -> Vector3
        abstract expandByPoint: point: Vector3 -> Box3
        abstract expandByVector: vector: Vector3 -> Box3
        abstract expandByScalar: scalar: float -> Box3
        abstract expandByObject: object: Object3D -> Box3
        abstract containsPoint: point: Vector3 -> bool
        abstract containsBox: box: Box3 -> bool
        abstract getParameter: point: Vector3 * target: Vector3 -> Vector3
        abstract intersectsBox: box: Box3 -> bool
        abstract intersectsSphere: sphere: Sphere -> bool
        abstract intersectsPlane: plane: Plane -> bool
        abstract intersectsTriangle: triangle: Triangle -> bool
        abstract clampPoint: point: Vector3 * target: Vector3 -> Vector3
        abstract distanceToPoint: point: Vector3 -> float
        abstract getBoundingSphere: target: Sphere -> Sphere
        abstract intersect: box: Box3 -> Box3
        abstract union: box: Box3 -> Box3
        abstract applyMatrix4: matrix: Matrix4 -> Box3
        abstract translate: offset: Vector3 -> Box3
        abstract equals: box: Box3 -> bool
        [<Obsolete("Use {@link Box3#isEmpty .isEmpty()} instead.")>]
        abstract empty: unit -> obj option
        [<Obsolete("Use {@link Box3#intersectsBox .intersectsBox()} instead.")>]
        abstract isIntersectionBox: b: obj option -> obj option
        [<Obsolete("Use {@link Box3#intersectsSphere .intersectsSphere()} instead.")>]
        abstract isIntersectionSphere: s: obj option -> obj option

    type [<AllowNullLiteral>] Box3Static =
        [<EmitConstructor>] abstract Create: ?min: Vector3 * ?max: Vector3 -> Box3

module __math_Color =
    type ColorRepresentation = Utils.ColorRepresentation
    type BufferAttribute = __core_BufferAttribute.BufferAttribute

    type [<AllowNullLiteral>] IExports =
        /// <summary>
        /// Represents a color. See also <see cref="ColorUtils" />.
        /// 
        /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/math/Color.js">src/math/Color.js</see>
        /// </summary>
        /// <example>const color = new THREE.Color( 0xff0000 );</example>
        abstract Color: ColorStatic

    type [<AllowNullLiteral>] HSL =
        abstract h: float with get, set
        abstract s: float with get, set
        abstract l: float with get, set

    /// <summary>
    /// Represents a color. See also <see cref="ColorUtils" />.
    /// 
    /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/math/Color.js">src/math/Color.js</see>
    /// </summary>
    /// <example>const color = new THREE.Color( 0xff0000 );</example>
    type [<AllowNullLiteral>] Color =
        abstract isColor: bool
        /// <summary>Red channel value between 0 and 1. Default is 1.</summary>
        /// <default>1</default>
        abstract r: float with get, set
        /// <summary>Green channel value between 0 and 1. Default is 1.</summary>
        /// <default>1</default>
        abstract g: float with get, set
        /// <summary>Blue channel value between 0 and 1. Default is 1.</summary>
        /// <default>1</default>
        abstract b: float with get, set
        abstract set: color: ColorRepresentation -> Color
        abstract setScalar: scalar: float -> Color
        abstract setHex: hex: float -> Color
        /// <summary>Sets this color from RGB values.</summary>
        /// <param name="r">Red channel value between 0 and 1.</param>
        /// <param name="g">Green channel value between 0 and 1.</param>
        /// <param name="b">Blue channel value between 0 and 1.</param>
        abstract setRGB: r: float * g: float * b: float -> Color
        /// <summary>
        /// Sets this color from HSL values.
        /// Based on MochiKit implementation by Bob Ippolito.
        /// </summary>
        /// <param name="h">Hue channel value between 0 and 1.</param>
        /// <param name="s">Saturation value channel between 0 and 1.</param>
        /// <param name="l">Value channel value between 0 and 1.</param>
        abstract setHSL: h: float * s: float * l: float -> Color
        /// <summary>Sets this color from a CSS context style string.</summary>
        /// <param name="contextStyle">Color in CSS context style format.</param>
        abstract setStyle: style: string -> Color
        /// <summary>
        /// Sets this color from a color name.
        /// Faster than <see cref="Color.setStyle">.setStyle()</see> method if you don't need the other CSS-style formats.
        /// </summary>
        /// <param name="style">Color name in X11 format.</param>
        abstract setColorName: style: string -> Color
        /// Clones this color.
        abstract clone: unit -> Color
        /// <summary>Copies given color.</summary>
        /// <param name="color">Color to copy.</param>
        abstract copy: color: Color -> Color
        /// <summary>Copies given color making conversion from gamma to linear space.</summary>
        /// <param name="color">Color to copy.</param>
        abstract copyGammaToLinear: color: Color * ?gammaFactor: float -> Color
        /// <summary>Copies given color making conversion from linear to gamma space.</summary>
        /// <param name="color">Color to copy.</param>
        abstract copyLinearToGamma: color: Color * ?gammaFactor: float -> Color
        /// Converts this color from gamma to linear space.
        abstract convertGammaToLinear: ?gammaFactor: float -> Color
        /// Converts this color from linear to gamma space.
        abstract convertLinearToGamma: ?gammaFactor: float -> Color
        /// <summary>Copies given color making conversion from sRGB to linear space.</summary>
        /// <param name="color">Color to copy.</param>
        abstract copySRGBToLinear: color: Color -> Color
        /// <summary>Copies given color making conversion from linear to sRGB space.</summary>
        /// <param name="color">Color to copy.</param>
        abstract copyLinearToSRGB: color: Color -> Color
        /// Converts this color from sRGB to linear space.
        abstract convertSRGBToLinear: unit -> Color
        /// Converts this color from linear to sRGB space.
        abstract convertLinearToSRGB: unit -> Color
        /// Returns the hexadecimal value of this color.
        abstract getHex: unit -> float
        /// Returns the string formated hexadecimal value of this color.
        abstract getHexString: unit -> string
        abstract getHSL: target: HSL -> HSL
        /// Returns the value of this color in CSS context style.
        /// Example: rgb(r, g, b)
        abstract getStyle: unit -> string
        abstract offsetHSL: h: float * s: float * l: float -> Color
        abstract add: color: Color -> Color
        abstract addColors: color1: Color * color2: Color -> Color
        abstract addScalar: s: float -> Color
        abstract sub: color: Color -> Color
        abstract multiply: color: Color -> Color
        abstract multiplyScalar: s: float -> Color
        abstract lerp: color: Color * alpha: float -> Color
        abstract lerpColors: color1: Color * color2: Color * alpha: float -> Color
        abstract lerpHSL: color: Color * alpha: float -> Color
        abstract equals: color: Color -> bool
        /// <summary>Sets this color's red, green and blue value from the provided array or array-like.</summary>
        /// <param name="array">the source array or array-like.</param>
        /// <param name="offset">(optional) offset into the array-like. Default is 0.</param>
        abstract fromArray: array: U2<ResizeArray<float>, ArrayLike<float>> * ?offset: float -> Color
        /// <summary>Returns an array [red, green, blue], or copies red, green and blue into the provided array.</summary>
        /// <param name="array">(optional) array to store the color to. If this is not provided, a new array will be created.</param>
        /// <param name="offset">(optional) optional offset into the array.</param>
        /// <returns>The created or provided array.</returns>
        abstract toArray: ?array: ResizeArray<float> * ?offset: float -> ResizeArray<float>
        /// <summary>Copies red, green and blue into the provided array-like.</summary>
        /// <param name="array">array-like to store the color to.</param>
        /// <param name="offset">(optional) optional offset into the array-like.</param>
        /// <returns>The provided array-like.</returns>
        abstract toArray: xyz: ArrayLike<float> * ?offset: float -> ArrayLike<float>
        abstract fromBufferAttribute: attribute: BufferAttribute * index: float -> Color

    /// <summary>
    /// Represents a color. See also <see cref="ColorUtils" />.
    /// 
    /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/math/Color.js">src/math/Color.js</see>
    /// </summary>
    /// <example>const color = new THREE.Color( 0xff0000 );</example>
    type [<AllowNullLiteral>] ColorStatic =
        [<EmitConstructor>] abstract Create: ?color: ColorRepresentation -> Color
        [<EmitConstructor>] abstract Create: r: float * g: float * b: float -> Color
        /// List of X11 color names.
        // abstract NAMES: Record<string, float> with get, set
        abstract NAMES: obj with get, set

module __math_Cylindrical =
    type Vector3 = __math_Vector3.Vector3

    type [<AllowNullLiteral>] IExports =
        abstract Cylindrical: CylindricalStatic

    type [<AllowNullLiteral>] Cylindrical =
        /// <default>1</default>
        abstract radius: float with get, set
        /// <default>0</default>
        abstract theta: float with get, set
        /// <default>0</default>
        abstract y: float with get, set
        abstract clone: unit -> Cylindrical
        abstract copy: other: Cylindrical -> Cylindrical
        abstract set: radius: float * theta: float * y: float -> Cylindrical
        abstract setFromVector3: vec3: Vector3 -> Cylindrical
        abstract setFromCartesianCoords: x: float * y: float * z: float -> Cylindrical

    type [<AllowNullLiteral>] CylindricalStatic =
        [<EmitConstructor>] abstract Create: ?radius: float * ?theta: float * ?y: float -> Cylindrical

module __math_Euler =
    type Matrix4 = __math_Matrix4.Matrix4
    type Quaternion = __math_Quaternion.Quaternion
    type Vector3 = __math_Vector3.Vector3

    type [<AllowNullLiteral>] IExports =
        abstract Euler: EulerStatic

    type [<AllowNullLiteral>] Euler =
        /// <default>0</default>
        abstract x: float with get, set
        /// <default>0</default>
        abstract y: float with get, set
        /// <default>0</default>
        abstract z: float with get, set
        /// <default>THREE.Euler.DefaultOrder</default>
        abstract order: string with get, set
        abstract isEuler: bool
        abstract _onChangeCallback: (unit -> unit) with get, set
        abstract set: x: float * y: float * z: float * ?order: string -> Euler
        abstract clone: unit -> Euler
        abstract copy: euler: Euler -> Euler
        abstract setFromRotationMatrix: m: Matrix4 * ?order: string * ?update: bool -> Euler
        abstract setFromQuaternion: q: Quaternion * ?order: string * ?update: bool -> Euler
        abstract setFromVector3: v: Vector3 * ?order: string -> Euler
        abstract reorder: newOrder: string -> Euler
        abstract equals: euler: Euler -> bool
        abstract fromArray: xyzo: ResizeArray<obj option> -> Euler
        abstract toArray: ?array: ResizeArray<float> * ?offset: float -> ResizeArray<float>
        abstract toVector3: ?optionalResult: Vector3 -> Vector3
        abstract _onChange: callback: (unit -> unit) -> Euler

    type [<AllowNullLiteral>] EulerStatic =
        [<EmitConstructor>] abstract Create: ?x: float * ?y: float * ?z: float * ?order: string -> Euler
        abstract RotationOrders: ResizeArray<string> with get, set
        abstract DefaultOrder: string with get, set

module __math_Frustum =
    type Plane = __math_Plane.Plane
    type Matrix4 = __math_Matrix4.Matrix4
    type Object3D = __core_Object3D.Object3D
    type Sprite = __objects_Sprite.Sprite
    type Sphere = __math_Sphere.Sphere
    type Box3 = __math_Box3.Box3
    type Vector3 = __math_Vector3.Vector3

    type [<AllowNullLiteral>] IExports =
        /// Frustums are used to determine what is inside the camera's field of view. They help speed up the rendering process.
        abstract Frustum: FrustumStatic

    /// Frustums are used to determine what is inside the camera's field of view. They help speed up the rendering process.
    type [<AllowNullLiteral>] Frustum =
        /// Array of 6 vectors.
        abstract planes: ResizeArray<Plane> with get, set
        abstract set: p0: Plane * p1: Plane * p2: Plane * p3: Plane * p4: Plane * p5: Plane -> Frustum
        abstract clone: unit -> Frustum
        abstract copy: frustum: Frustum -> Frustum
        abstract setFromProjectionMatrix: m: Matrix4 -> Frustum
        abstract intersectsObject: object: Object3D -> bool
        abstract intersectsSprite: sprite: Sprite -> bool
        abstract intersectsSphere: sphere: Sphere -> bool
        abstract intersectsBox: box: Box3 -> bool
        abstract containsPoint: point: Vector3 -> bool

    /// Frustums are used to determine what is inside the camera's field of view. They help speed up the rendering process.
    type [<AllowNullLiteral>] FrustumStatic =
        [<EmitConstructor>] abstract Create: ?p0: Plane * ?p1: Plane * ?p2: Plane * ?p3: Plane * ?p4: Plane * ?p5: Plane -> Frustum

module __math_Interpolant =

    type [<AllowNullLiteral>] IExports =
        abstract Interpolant: InterpolantStatic

    type [<AllowNullLiteral>] Interpolant =
        abstract parameterPositions: obj option with get, set
        abstract sampleValues: obj option with get, set
        abstract valueSize: float with get, set
        abstract resultBuffer: obj option with get, set
        abstract evaluate: time: float -> obj option

    type [<AllowNullLiteral>] InterpolantStatic =
        [<EmitConstructor>] abstract Create: parameterPositions: obj option * sampleValues: obj option * sampleSize: float * ?resultBuffer: obj -> Interpolant

module __math_Line3 =
    type Vector3 = __math_Vector3.Vector3
    type Matrix4 = __math_Matrix4.Matrix4

    type [<AllowNullLiteral>] IExports =
        abstract Line3: Line3Static

    type [<AllowNullLiteral>] Line3 =
        /// <default>new THREE.Vector3()</default>
        abstract start: Vector3 with get, set
        /// <default>new THREE.Vector3()</default>
        abstract ``end``: Vector3 with get, set
        abstract set: ?start: Vector3 * ?``end``: Vector3 -> Line3
        abstract clone: unit -> Line3
        abstract copy: line: Line3 -> Line3
        abstract getCenter: target: Vector3 -> Vector3
        abstract delta: target: Vector3 -> Vector3
        abstract distanceSq: unit -> float
        abstract distance: unit -> float
        abstract at: t: float * target: Vector3 -> Vector3
        abstract closestPointToPointParameter: point: Vector3 * ?clampToLine: bool -> float
        abstract closestPointToPoint: point: Vector3 * clampToLine: bool * target: Vector3 -> Vector3
        abstract applyMatrix4: matrix: Matrix4 -> Line3
        abstract equals: line: Line3 -> bool

    type [<AllowNullLiteral>] Line3Static =
        [<EmitConstructor>] abstract Create: ?start: Vector3 * ?``end``: Vector3 -> Line3

module __math_MathUtils =
    type Quaternion = __math_Quaternion.Quaternion

    type [<AllowNullLiteral>] IExports =
        /// <seealso href="https://github.com/mrdoob/three.js/blob/master/src/math/MathUtils.js">src/math/MathUtils.js</seealso>
        abstract DEG2RAD: float
        abstract RAD2DEG: float
        abstract generateUUID: unit -> string
        /// <summary>Clamps the x to be between a and b.</summary>
        /// <param name="value">Value to be clamped.</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value.</param>
        abstract clamp: value: float * min: float * max: float -> float
        abstract euclideanModulo: n: float * m: float -> float
        /// <summary>Linear mapping of x from range [a1, a2] to range [b1, b2].</summary>
        /// <param name="x">Value to be mapped.</param>
        /// <param name="a1">Minimum value for range A.</param>
        /// <param name="a2">Maximum value for range A.</param>
        /// <param name="b1">Minimum value for range B.</param>
        /// <param name="b2">Maximum value for range B.</param>
        abstract mapLinear: x: float * a1: float * a2: float * b1: float * b2: float -> float
        abstract smoothstep: x: float * min: float * max: float -> float
        abstract smootherstep: x: float * min: float * max: float -> float
        /// Random float from 0 to 1 with 16 bits of randomness.
        /// Standard Math.random() creates repetitive patterns when applied over larger space.
        [<Obsolete("Use {@link Math#random Math.random()}")>]
        abstract random16: unit -> float
        /// Random integer from low to high interval.
        abstract randInt: low: float * high: float -> float
        /// Random float from low to high interval.
        abstract randFloat: low: float * high: float -> float
        /// Random float from - range / 2 to range / 2 interval.
        abstract randFloatSpread: range: float -> float
        /// Deterministic pseudo-random float in the interval [ 0, 1 ].
        abstract seededRandom: ?seed: float -> float
        abstract degToRad: degrees: float -> float
        abstract radToDeg: radians: float -> float
        abstract isPowerOfTwo: value: float -> bool
        abstract inverseLerp: x: float * y: float * t: float -> float
        /// <summary>
        /// Returns a value linearly interpolated from two known points based
        /// on the given interval - t = 0 will return x and t = 1 will return y.
        /// </summary>
        /// <param name="x">Start point.</param>
        /// <param name="y">End point.</param>
        /// <param name="t">interpolation factor in the closed interval [0, 1]</param>
        abstract lerp: x: float * y: float * t: float -> float
        /// <summary>
        /// Smoothly interpolate a number from x toward y in a spring-like
        /// manner using the dt to maintain frame rate independent movement.
        /// </summary>
        /// <param name="x">Current point.</param>
        /// <param name="y">Target point.</param>
        /// <param name="lambda">A higher lambda value will make the movement more sudden, and a lower value will make the movement more gradual.</param>
        /// <param name="dt">Delta time in seconds.</param>
        abstract damp: x: float * y: float * lambda: float * dt: float -> float
        /// <summary>Returns a value that alternates between 0 and length.</summary>
        /// <param name="x">The value to pingpong.</param>
        /// <param name="length">The positive value the export function will pingpong to. Default is 1.</param>
        abstract pingpong: x: float * ?length: float -> float
        [<Obsolete("Use {@link Math#floorPowerOfTwo .floorPowerOfTwo()}")>]
        abstract nearestPowerOfTwo: value: float -> float
        [<Obsolete("Use {@link Math#ceilPowerOfTwo .ceilPowerOfTwo()}")>]
        abstract nextPowerOfTwo: value: float -> float
        abstract floorPowerOfTwo: value: float -> float
        abstract ceilPowerOfTwo: value: float -> float
        abstract setQuaternionFromProperEuler: q: Quaternion * a: float * b: float * c: float * order: string -> unit

module __math_Matrix3 =
    type Matrix4 = __math_Matrix4.Matrix4
    type Vector3 = __math_Vector3.Vector3

    type [<AllowNullLiteral>] IExports =
        /// ( class Matrix3 implements Matrix<Matrix3> )
        abstract Matrix3: Matrix3Static

    type Matrix3Tuple =
        float * float * float * float * float * float * float * float * float

    /// ( interface Matrix<T> )
    type [<AllowNullLiteral>] Matrix =
        /// Array with matrix values.
        abstract elements: ResizeArray<float> with get, set
        /// identity():T;
        abstract identity: unit -> Matrix
        /// copy(m:T):T;
        abstract copy: m: Matrix -> Matrix
        /// multiplyScalar(s:number):T;
        abstract multiplyScalar: s: float -> Matrix
        abstract determinant: unit -> float
        /// transpose():T;
        abstract transpose: unit -> Matrix
        /// invert():T;
        abstract invert: unit -> Matrix
        /// clone():T;
        abstract clone: unit -> Matrix

    /// ( class Matrix3 implements Matrix<Matrix3> )
    type [<AllowNullLiteral>] Matrix3 =
        inherit Matrix
        /// <summary>Array with matrix values.</summary>
        /// <default>[1, 0, 0, 0, 1, 0, 0, 0, 1]</default>
        abstract elements: ResizeArray<float> with get, set
        abstract set: n11: float * n12: float * n13: float * n21: float * n22: float * n23: float * n31: float * n32: float * n33: float -> Matrix3
        /// identity():T;
        abstract identity: unit -> Matrix3
        /// clone():T;
        abstract clone: unit -> Matrix3
        /// copy(m:T):T;
        abstract copy: m: Matrix3 -> Matrix3
        abstract extractBasis: xAxis: Vector3 * yAxis: Vector3 * zAxis: Vector3 -> Matrix3
        abstract setFromMatrix4: m: Matrix4 -> Matrix3
        /// multiplyScalar(s:number):T;
        abstract multiplyScalar: s: float -> Matrix3
        abstract determinant: unit -> float
        /// Inverts this matrix in place.
        abstract invert: unit -> Matrix3
        /// Transposes this matrix in place.
        abstract transpose: unit -> Matrix3
        abstract getNormalMatrix: matrix4: Matrix4 -> Matrix3
        /// Transposes this matrix into the supplied array r, and returns itself.
        abstract transposeIntoArray: r: ResizeArray<float> -> Matrix3
        abstract setUvTransform: tx: float * ty: float * sx: float * sy: float * rotation: float * cx: float * cy: float -> Matrix3
        abstract scale: sx: float * sy: float -> Matrix3
        abstract rotate: theta: float -> Matrix3
        abstract translate: tx: float * ty: float -> Matrix3
        abstract equals: matrix: Matrix3 -> bool
        /// <summary>Sets the values of this matrix from the provided array or array-like.</summary>
        /// <param name="array">the source array or array-like.</param>
        /// <param name="offset">(optional) offset into the array-like. Default is 0.</param>
        abstract fromArray: array: U2<ResizeArray<float>, ArrayLike<float>> * ?offset: float -> Matrix3
        /// <summary>Returns an array with the values of this matrix, or copies them into the provided array.</summary>
        /// <param name="array">(optional) array to store the matrix to. If this is not provided, a new array will be created.</param>
        /// <param name="offset">(optional) optional offset into the array.</param>
        /// <returns>The created or provided array.</returns>
        abstract toArray: ?array: ResizeArray<float> * ?offset: float -> ResizeArray<float>
        abstract toArray: ?array: Matrix3Tuple * ?offset: int -> Matrix3Tuple
        /// <summary>Copies he values of this matrix into the provided array-like.</summary>
        /// <param name="array">array-like to store the matrix to.</param>
        /// <param name="offset">(optional) optional offset into the array-like.</param>
        /// <returns>The provided array-like.</returns>
        abstract toArray: ?array: ArrayLike<float> * ?offset: float -> ArrayLike<float>
        /// Multiplies this matrix by m.
        abstract multiply: m: Matrix3 -> Matrix3
        abstract premultiply: m: Matrix3 -> Matrix3
        /// Sets this matrix to a x b.
        abstract multiplyMatrices: a: Matrix3 * b: Matrix3 -> Matrix3
        [<Obsolete("Use {@link Vector3.applyMatrix3 vector.applyMatrix3( matrix )} instead.")>]
        abstract multiplyVector3: vector: Vector3 -> obj option
        [<Obsolete("This method has been removed completely.")>]
        abstract multiplyVector3Array: a: obj option -> obj option
        [<Obsolete("Use {@link Matrix3#invert .invert()} instead.")>]
        abstract getInverse: matrix: Matrix4 * ?throwOnDegenerate: bool -> Matrix3
        abstract getInverse: matrix: Matrix -> Matrix
        [<Obsolete("Use {@link Matrix3#toArray .toArray()} instead.")>]
        abstract flattenToArrayOffset: array: ResizeArray<float> * offset: float -> ResizeArray<float>

    /// ( class Matrix3 implements Matrix<Matrix3> )
    type [<AllowNullLiteral>] Matrix3Static =
        /// Creates an identity matrix.
        [<EmitConstructor>] abstract Create: unit -> Matrix3

module __math_Matrix4 =
    type Vector3 = __math_Vector3.Vector3
    type Euler = __math_Euler.Euler
    type Quaternion = __math_Quaternion.Quaternion
    type Matrix = __math_Matrix3.Matrix
    type Matrix3 = __math_Matrix3.Matrix3

    type [<AllowNullLiteral>] IExports =
        /// <summary>A 4x4 Matrix.</summary>
        /// <example>
        /// // Simple rig for rotating around 3 axes
        /// const m = new THREE.Matrix4();
        /// const m1 = new THREE.Matrix4();
        /// const m2 = new THREE.Matrix4();
        /// const m3 = new THREE.Matrix4();
        /// const alpha = 0;
        /// const beta = Math.PI;
        /// const gamma = Math.PI/2;
        /// m1.makeRotationX( alpha );
        /// m2.makeRotationY( beta );
        /// m3.makeRotationZ( gamma );
        /// m.multiplyMatrices( m1, m2 );
        /// m.multiply( m3 );
        /// </example>
        abstract Matrix4: Matrix4Static

    type Matrix4Tuple =
        float * float * float * float * float * float * float * float * float * float * float * float * float * float * float * float

    /// <summary>A 4x4 Matrix.</summary>
    /// <example>
    /// // Simple rig for rotating around 3 axes
    /// const m = new THREE.Matrix4();
    /// const m1 = new THREE.Matrix4();
    /// const m2 = new THREE.Matrix4();
    /// const m3 = new THREE.Matrix4();
    /// const alpha = 0;
    /// const beta = Math.PI;
    /// const gamma = Math.PI/2;
    /// m1.makeRotationX( alpha );
    /// m2.makeRotationY( beta );
    /// m3.makeRotationZ( gamma );
    /// m.multiplyMatrices( m1, m2 );
    /// m.multiply( m3 );
    /// </example>
    type [<AllowNullLiteral>] Matrix4 =
        inherit Matrix
        /// <summary>Array with matrix values.</summary>
        /// <default>[1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1]</default>
        abstract elements: ResizeArray<float> with get, set
        /// Sets all fields of this matrix.
        abstract set: n11: float * n12: float * n13: float * n14: float * n21: float * n22: float * n23: float * n24: float * n31: float * n32: float * n33: float * n34: float * n41: float * n42: float * n43: float * n44: float -> Matrix4
        /// Resets this matrix to identity.
        abstract identity: unit -> Matrix4
        /// clone():T;
        abstract clone: unit -> Matrix4
        /// copy(m:T):T;
        abstract copy: m: Matrix4 -> Matrix4
        abstract copyPosition: m: Matrix4 -> Matrix4
        abstract extractBasis: xAxis: Vector3 * yAxis: Vector3 * zAxis: Vector3 -> Matrix4
        abstract makeBasis: xAxis: Vector3 * yAxis: Vector3 * zAxis: Vector3 -> Matrix4
        /// Copies the rotation component of the supplied matrix m into this matrix rotation component.
        abstract extractRotation: m: Matrix4 -> Matrix4
        abstract makeRotationFromEuler: euler: Euler -> Matrix4
        abstract makeRotationFromQuaternion: q: Quaternion -> Matrix4
        /// Constructs a rotation matrix, looking from eye towards center with defined up vector.
        abstract lookAt: eye: Vector3 * target: Vector3 * up: Vector3 -> Matrix4
        /// Multiplies this matrix by m.
        abstract multiply: m: Matrix4 -> Matrix4
        abstract premultiply: m: Matrix4 -> Matrix4
        /// Sets this matrix to a x b.
        abstract multiplyMatrices: a: Matrix4 * b: Matrix4 -> Matrix4
        /// Sets this matrix to a x b and stores the result into the flat array r.
        /// r can be either a regular Array or a TypedArray.
        [<Obsolete("This method has been removed completely.")>]
        abstract multiplyToArray: a: Matrix4 * b: Matrix4 * r: ResizeArray<float> -> Matrix4
        /// Multiplies this matrix by s.
        abstract multiplyScalar: s: float -> Matrix4
        /// <summary>
        /// Computes determinant of this matrix.
        /// Based on <see href="http://www.euclideanspace.com/maths/algebra/matrix/functions/inverse/fourD/index.htm" />
        /// </summary>
        abstract determinant: unit -> float
        /// Transposes this matrix.
        abstract transpose: unit -> Matrix4
        /// Sets the position component for this matrix from vector v.
        abstract setPosition: v: U2<Vector3, float> * ?y: float * ?z: float -> Matrix4
        /// Inverts this matrix.
        abstract invert: unit -> Matrix4
        /// Multiplies the columns of this matrix by vector v.
        abstract scale: v: Vector3 -> Matrix4
        abstract getMaxScaleOnAxis: unit -> float
        /// Sets this matrix as translation transform.
        abstract makeTranslation: x: float * y: float * z: float -> Matrix4
        /// <summary>Sets this matrix as rotation transform around x axis by theta radians.</summary>
        /// <param name="theta">Rotation angle in radians.</param>
        abstract makeRotationX: theta: float -> Matrix4
        /// <summary>Sets this matrix as rotation transform around y axis by theta radians.</summary>
        /// <param name="theta">Rotation angle in radians.</param>
        abstract makeRotationY: theta: float -> Matrix4
        /// <summary>Sets this matrix as rotation transform around z axis by theta radians.</summary>
        /// <param name="theta">Rotation angle in radians.</param>
        abstract makeRotationZ: theta: float -> Matrix4
        /// <summary>
        /// Sets this matrix as rotation transform around axis by angle radians.
        /// Based on <see href="http://www.gamedev.net/reference/articles/article1199.asp." />
        /// </summary>
        /// <param name="axis">Rotation axis.</param>
        /// <param name="theta">Rotation angle in radians.</param>
        abstract makeRotationAxis: axis: Vector3 * angle: float -> Matrix4
        /// Sets this matrix as scale transform.
        abstract makeScale: x: float * y: float * z: float -> Matrix4
        /// Sets this matrix as shear transform.
        abstract makeShear: xy: float * xz: float * yx: float * yz: float * zx: float * zy: float -> Matrix4
        /// Sets this matrix to the transformation composed of translation, rotation and scale.
        abstract compose: translation: Vector3 * rotation: Quaternion * scale: Vector3 -> Matrix4
        /// Decomposes this matrix into it's position, quaternion and scale components.
        abstract decompose: translation: Vector3 * rotation: Quaternion * scale: Vector3 -> Matrix4
        /// Creates a frustum matrix.
        abstract makePerspective: left: float * right: float * bottom: float * top: float * near: float * far: float -> Matrix4
        /// Creates a perspective projection matrix.
        abstract makePerspective: fov: float * aspect: float * near: float * far: float -> Matrix4
        /// Creates an orthographic projection matrix.
        abstract makeOrthographic: left: float * right: float * top: float * bottom: float * near: float * far: float -> Matrix4
        abstract equals: matrix: Matrix4 -> bool
        /// <summary>Sets the values of this matrix from the provided array or array-like.</summary>
        /// <param name="array">the source array or array-like.</param>
        /// <param name="offset">(optional) offset into the array-like. Default is 0.</param>
        abstract fromArray: array: U2<ResizeArray<float>, ArrayLike<float>> * ?offset: float -> Matrix4
        /// <summary>Returns an array with the values of this matrix, or copies them into the provided array.</summary>
        /// <param name="array">(optional) array to store the matrix to. If this is not provided, a new array will be created.</param>
        /// <param name="offset">(optional) optional offset into the array.</param>
        /// <returns>The created or provided array.</returns>
        abstract toArray: ?array: ResizeArray<float> * ?offset: float -> ResizeArray<float>
        abstract toArray: ?array: Matrix4Tuple * ?offset: int -> Matrix4Tuple
        /// <summary>Copies he values of this matrix into the provided array-like.</summary>
        /// <param name="array">array-like to store the matrix to.</param>
        /// <param name="offset">(optional) optional offset into the array-like.</param>
        /// <returns>The provided array-like.</returns>
        abstract toArray: ?array: ArrayLike<float> * ?offset: float -> ArrayLike<float>
        /// Set the upper 3x3 elements of this matrix to the values of the Matrix3 m.
        abstract setFromMatrix3: m: Matrix3 -> Matrix4
        [<Obsolete("Use {@link Matrix4#copyPosition .copyPosition()} instead.")>]
        abstract extractPosition: m: Matrix4 -> Matrix4
        [<Obsolete("Use {@link Matrix4#makeRotationFromQuaternion .makeRotationFromQuaternion()} instead.")>]
        abstract setRotationFromQuaternion: q: Quaternion -> Matrix4
        [<Obsolete("Use {@link Vector3#applyMatrix4 vector.applyMatrix4( matrix )} instead.")>]
        abstract multiplyVector3: v: obj option -> obj option
        [<Obsolete("Use {@link Vector4#applyMatrix4 vector.applyMatrix4( matrix )} instead.")>]
        abstract multiplyVector4: v: obj option -> obj option
        [<Obsolete("This method has been removed completely.")>]
        abstract multiplyVector3Array: array: ResizeArray<float> -> ResizeArray<float>
        [<Obsolete("Use {@link Vector3#transformDirection Vector3.transformDirection( matrix )} instead.")>]
        abstract rotateAxis: v: obj option -> unit
        [<Obsolete("Use {@link Vector3#applyMatrix4 vector.applyMatrix4( matrix )} instead.")>]
        abstract crossVector: v: obj option -> unit
        [<Obsolete("Use {@link Matrix4#toArray .toArray()} instead.")>]
        abstract flattenToArrayOffset: array: ResizeArray<float> * offset: float -> ResizeArray<float>
        [<Obsolete("Use {@link Matrix4#invert .invert()} instead.")>]
        abstract getInverse: matrix: Matrix -> Matrix

    /// <summary>A 4x4 Matrix.</summary>
    /// <example>
    /// // Simple rig for rotating around 3 axes
    /// const m = new THREE.Matrix4();
    /// const m1 = new THREE.Matrix4();
    /// const m2 = new THREE.Matrix4();
    /// const m3 = new THREE.Matrix4();
    /// const alpha = 0;
    /// const beta = Math.PI;
    /// const gamma = Math.PI/2;
    /// m1.makeRotationX( alpha );
    /// m2.makeRotationY( beta );
    /// m3.makeRotationZ( gamma );
    /// m.multiplyMatrices( m1, m2 );
    /// m.multiply( m3 );
    /// </example>
    type [<AllowNullLiteral>] Matrix4Static =
        [<EmitConstructor>] abstract Create: unit -> Matrix4

module __math_Plane =
    type Vector3 = __math_Vector3.Vector3
    type Sphere = __math_Sphere.Sphere
    type Line3 = __math_Line3.Line3
    type Box3 = __math_Box3.Box3
    type Matrix4 = __math_Matrix4.Matrix4
    type Matrix3 = __math_Matrix3.Matrix3

    type [<AllowNullLiteral>] IExports =
        abstract Plane: PlaneStatic

    type [<AllowNullLiteral>] Plane =
        /// <default>new THREE.Vector3( 1, 0, 0 )</default>
        abstract normal: Vector3 with get, set
        /// <default>0</default>
        abstract constant: float with get, set
        abstract isPlane: bool
        abstract set: normal: Vector3 * constant: float -> Plane
        abstract setComponents: x: float * y: float * z: float * w: float -> Plane
        abstract setFromNormalAndCoplanarPoint: normal: Vector3 * point: Vector3 -> Plane
        abstract setFromCoplanarPoints: a: Vector3 * b: Vector3 * c: Vector3 -> Plane
        abstract clone: unit -> Plane
        abstract copy: plane: Plane -> Plane
        abstract normalize: unit -> Plane
        abstract negate: unit -> Plane
        abstract distanceToPoint: point: Vector3 -> float
        abstract distanceToSphere: sphere: Sphere -> float
        abstract projectPoint: point: Vector3 * target: Vector3 -> Vector3
        abstract orthoPoint: point: Vector3 * target: Vector3 -> Vector3
        abstract intersectLine: line: Line3 * target: Vector3 -> Vector3 option
        abstract intersectsLine: line: Line3 -> bool
        abstract intersectsBox: box: Box3 -> bool
        abstract intersectsSphere: sphere: Sphere -> bool
        abstract coplanarPoint: target: Vector3 -> Vector3
        abstract applyMatrix4: matrix: Matrix4 * ?optionalNormalMatrix: Matrix3 -> Plane
        abstract translate: offset: Vector3 -> Plane
        abstract equals: plane: Plane -> bool
        [<Obsolete("Use {@link Plane#intersectsLine .intersectsLine()} instead.")>]
        abstract isIntersectionLine: l: obj option -> obj option

    type [<AllowNullLiteral>] PlaneStatic =
        [<EmitConstructor>] abstract Create: ?normal: Vector3 * ?constant: float -> Plane

module __math_Quaternion =
    type Euler = __math_Euler.Euler
    type Vector3 = __math_Vector3.Vector3
    type Matrix4 = __math_Matrix4.Matrix4

    type [<AllowNullLiteral>] IExports =
        /// <summary>Implementation of a quaternion. This is used for rotating things without incurring in the dreaded gimbal lock issue, amongst other advantages.</summary>
        /// <example>
        /// const quaternion = new THREE.Quaternion();
        /// quaternion.setFromAxisAngle( new THREE.Vector3( 0, 1, 0 ), Math.PI / 2 );
        /// const vector = new THREE.Vector3( 1, 0, 0 );
        /// vector.applyQuaternion( quaternion );
        /// </example>
        abstract Quaternion: QuaternionStatic

    /// <summary>Implementation of a quaternion. This is used for rotating things without incurring in the dreaded gimbal lock issue, amongst other advantages.</summary>
    /// <example>
    /// const quaternion = new THREE.Quaternion();
    /// quaternion.setFromAxisAngle( new THREE.Vector3( 0, 1, 0 ), Math.PI / 2 );
    /// const vector = new THREE.Vector3( 1, 0, 0 );
    /// vector.applyQuaternion( quaternion );
    /// </example>
    type [<AllowNullLiteral>] Quaternion =
        /// <default>0</default>
        abstract x: float with get, set
        /// <default>0</default>
        abstract y: float with get, set
        /// <default>0</default>
        abstract z: float with get, set
        /// <default>1</default>
        abstract w: float with get, set
        abstract isQuaternion: bool
        /// Sets values of this quaternion.
        abstract set: x: float * y: float * z: float * w: float -> Quaternion
        /// Clones this quaternion.
        abstract clone: unit -> Quaternion
        /// Copies values of q to this quaternion.
        abstract copy: q: Quaternion -> Quaternion
        /// Sets this quaternion from rotation specified by Euler angles.
        abstract setFromEuler: euler: Euler * ?update: bool -> Quaternion
        /// <summary>
        /// Sets this quaternion from rotation specified by axis and angle.
        /// Adapted from <see href="http://www.euclideanspace.com/maths/geometry/rotations/conversions/angleToQuaternion/index.htm." />
        /// Axis have to be normalized, angle is in radians.
        /// </summary>
        abstract setFromAxisAngle: axis: Vector3 * angle: float -> Quaternion
        /// <summary>Sets this quaternion from rotation component of m. Adapted from <see href="http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm." /></summary>
        abstract setFromRotationMatrix: m: Matrix4 -> Quaternion
        abstract setFromUnitVectors: vFrom: Vector3 * vTo: Vector3 -> Quaternion
        abstract angleTo: q: Quaternion -> float
        abstract rotateTowards: q: Quaternion * step: float -> Quaternion
        abstract identity: unit -> Quaternion
        /// Inverts this quaternion.
        abstract invert: unit -> Quaternion
        abstract conjugate: unit -> Quaternion
        abstract dot: v: Quaternion -> float
        abstract lengthSq: unit -> float
        /// Computes length of this quaternion.
        abstract length: unit -> float
        /// Normalizes this quaternion.
        abstract normalize: unit -> Quaternion
        /// Multiplies this quaternion by b.
        abstract multiply: q: Quaternion -> Quaternion
        abstract premultiply: q: Quaternion -> Quaternion
        /// <summary>
        /// Sets this quaternion to a x b
        /// Adapted from <see href="http://www.euclideanspace.com/maths/algebra/realNormedAlgebra/quaternions/code/index.htm." />
        /// </summary>
        abstract multiplyQuaternions: a: Quaternion * b: Quaternion -> Quaternion
        abstract slerp: qb: Quaternion * t: float -> Quaternion
        abstract slerpQuaternions: qa: Quaternion * qb: Quaternion * t: float -> Quaternion
        abstract equals: v: Quaternion -> bool
        /// <summary>Sets this quaternion's x, y, z and w value from the provided array or array-like.</summary>
        /// <param name="array">the source array or array-like.</param>
        /// <param name="offset">(optional) offset into the array. Default is 0.</param>
        abstract fromArray: array: U2<ResizeArray<float>, ArrayLike<float>> * ?offset: float -> Quaternion
        /// <summary>Returns an array [x, y, z, w], or copies x, y, z and w into the provided array.</summary>
        /// <param name="array">(optional) array to store the quaternion to. If this is not provided, a new array will be created.</param>
        /// <param name="offset">(optional) optional offset into the array.</param>
        /// <returns>The created or provided array.</returns>
        abstract toArray: ?array: ResizeArray<float> * ?offset: float -> ResizeArray<float>
        /// <summary>Copies x, y, z and w into the provided array-like.</summary>
        /// <param name="array">array-like to store the quaternion to.</param>
        /// <param name="offset">(optional) optional offset into the array.</param>
        /// <returns>The provided array-like.</returns>
        abstract toArray: array: ArrayLike<float> * ?offset: float -> ArrayLike<float>
        abstract _onChange: callback: (unit -> unit) -> Quaternion
        abstract _onChangeCallback: (unit -> unit) with get, set
        [<Obsolete("Use {@link Vector#applyQuaternion vector.applyQuaternion( quaternion )} instead.")>]
        abstract multiplyVector3: v: obj option -> obj option
        [<Obsolete("Use {@link Quaternion#invert .invert()} instead.")>]
        abstract inverse: unit -> Quaternion

    /// <summary>Implementation of a quaternion. This is used for rotating things without incurring in the dreaded gimbal lock issue, amongst other advantages.</summary>
    /// <example>
    /// const quaternion = new THREE.Quaternion();
    /// quaternion.setFromAxisAngle( new THREE.Vector3( 0, 1, 0 ), Math.PI / 2 );
    /// const vector = new THREE.Vector3( 1, 0, 0 );
    /// vector.applyQuaternion( quaternion );
    /// </example>
    type [<AllowNullLiteral>] QuaternionStatic =
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="z">z coordinate</param>
        /// <param name="w">w coordinate</param>
        [<EmitConstructor>] abstract Create: ?x: float * ?y: float * ?z: float * ?w: float -> Quaternion
        abstract slerpFlat: dst: ResizeArray<float> * dstOffset: float * src0: ResizeArray<float> * srcOffset: float * src1: ResizeArray<float> * stcOffset1: float * t: float -> Quaternion
        abstract multiplyQuaternionsFlat: dst: ResizeArray<float> * dstOffset: float * src0: ResizeArray<float> * srcOffset: float * src1: ResizeArray<float> * stcOffset1: float -> ResizeArray<float>
        [<Obsolete("Use qm.slerpQuaternions( qa, qb, t ) instead..")>]
        abstract slerp: qa: Quaternion * qb: Quaternion * qm: Quaternion * t: float -> float

module __math_Ray =
    type Vector3 = __math_Vector3.Vector3
    type Sphere = __math_Sphere.Sphere
    type Plane = __math_Plane.Plane
    type Box3 = __math_Box3.Box3
    type Matrix4 = __math_Matrix4.Matrix4

    type [<AllowNullLiteral>] IExports =
        abstract Ray: RayStatic

    type [<AllowNullLiteral>] Ray =
        /// <default>new THREE.Vector3()</default>
        abstract origin: Vector3 with get, set
        /// <default>new THREE.Vector3( 0, 0, - 1 )</default>
        abstract direction: Vector3 with get, set
        abstract set: origin: Vector3 * direction: Vector3 -> Ray
        abstract clone: unit -> Ray
        abstract copy: ray: Ray -> Ray
        abstract at: t: float * target: Vector3 -> Vector3
        abstract lookAt: v: Vector3 -> Ray
        abstract recast: t: float -> Ray
        abstract closestPointToPoint: point: Vector3 * target: Vector3 -> Vector3
        abstract distanceToPoint: point: Vector3 -> float
        abstract distanceSqToPoint: point: Vector3 -> float
        abstract distanceSqToSegment: v0: Vector3 * v1: Vector3 * ?optionalPointOnRay: Vector3 * ?optionalPointOnSegment: Vector3 -> float
        abstract intersectSphere: sphere: Sphere * target: Vector3 -> Vector3 option
        abstract intersectsSphere: sphere: Sphere -> bool
        abstract distanceToPlane: plane: Plane -> float
        abstract intersectPlane: plane: Plane * target: Vector3 -> Vector3 option
        abstract intersectsPlane: plane: Plane -> bool
        abstract intersectBox: box: Box3 * target: Vector3 -> Vector3 option
        abstract intersectsBox: box: Box3 -> bool
        abstract intersectTriangle: a: Vector3 * b: Vector3 * c: Vector3 * backfaceCulling: bool * target: Vector3 -> Vector3 option
        abstract applyMatrix4: matrix4: Matrix4 -> Ray
        abstract equals: ray: Ray -> bool
        [<Obsolete("Use {@link Ray#intersectsBox .intersectsBox()} instead.")>]
        abstract isIntersectionBox: b: obj option -> obj option
        [<Obsolete("Use {@link Ray#intersectsPlane .intersectsPlane()} instead.")>]
        abstract isIntersectionPlane: p: obj option -> obj option
        [<Obsolete("Use {@link Ray#intersectsSphere .intersectsSphere()} instead.")>]
        abstract isIntersectionSphere: s: obj option -> obj option

    type [<AllowNullLiteral>] RayStatic =
        [<EmitConstructor>] abstract Create: ?origin: Vector3 * ?direction: Vector3 -> Ray

module __math_Sphere =
    type Vector3 = __math_Vector3.Vector3
    type Box3 = __math_Box3.Box3
    type Plane = __math_Plane.Plane
    type Matrix4 = __math_Matrix4.Matrix4

    type [<AllowNullLiteral>] IExports =
        abstract Sphere: SphereStatic

    type [<AllowNullLiteral>] Sphere =
        /// <default>new Vector3()</default>
        abstract center: Vector3 with get, set
        /// <default>1</default>
        abstract radius: float with get, set
        abstract set: center: Vector3 * radius: float -> Sphere
        abstract setFromPoints: points: ResizeArray<Vector3> * ?optionalCenter: Vector3 -> Sphere
        abstract clone: unit -> Sphere
        abstract copy: sphere: Sphere -> Sphere
        abstract expandByPoint: point: Vector3 -> Sphere
        abstract isEmpty: unit -> bool
        abstract makeEmpty: unit -> Sphere
        abstract containsPoint: point: Vector3 -> bool
        abstract distanceToPoint: point: Vector3 -> float
        abstract intersectsSphere: sphere: Sphere -> bool
        abstract intersectsBox: box: Box3 -> bool
        abstract intersectsPlane: plane: Plane -> bool
        abstract clampPoint: point: Vector3 * target: Vector3 -> Vector3
        abstract getBoundingBox: target: Box3 -> Box3
        abstract applyMatrix4: matrix: Matrix4 -> Sphere
        abstract translate: offset: Vector3 -> Sphere
        abstract equals: sphere: Sphere -> bool
        abstract union: sphere: Sphere -> Sphere
        [<Obsolete("Use {@link Sphere#isEmpty .isEmpty()} instead.")>]
        abstract empty: unit -> obj option

    type [<AllowNullLiteral>] SphereStatic =
        [<EmitConstructor>] abstract Create: ?center: Vector3 * ?radius: float -> Sphere

module __math_Spherical =
    type Vector3 = __math_Vector3.Vector3

    type [<AllowNullLiteral>] IExports =
        abstract Spherical: SphericalStatic

    type [<AllowNullLiteral>] Spherical =
        /// <default>1</default>
        abstract radius: float with get, set
        /// <default>0</default>
        abstract phi: float with get, set
        /// <default>0</default>
        abstract theta: float with get, set
        abstract set: radius: float * phi: float * theta: float -> Spherical
        abstract clone: unit -> Spherical
        abstract copy: other: Spherical -> Spherical
        abstract makeSafe: unit -> Spherical
        abstract setFromVector3: v: Vector3 -> Spherical
        abstract setFromCartesianCoords: x: float * y: float * z: float -> Spherical

    type [<AllowNullLiteral>] SphericalStatic =
        [<EmitConstructor>] abstract Create: ?radius: float * ?phi: float * ?theta: float -> Spherical

module __math_SphericalHarmonics3 =
    type Vector3 = __math_Vector3.Vector3

    type [<AllowNullLiteral>] IExports =
        abstract SphericalHarmonics3: SphericalHarmonics3Static

    type [<AllowNullLiteral>] SphericalHarmonics3 =
        /// <default>
        /// [new THREE.Vector3(), new THREE.Vector3(), new THREE.Vector3(), new THREE.Vector3(),
        /// new THREE.Vector3(), new THREE.Vector3(), new THREE.Vector3(), new THREE.Vector3(), new THREE.Vector3()]
        /// </default>
        abstract coefficients: ResizeArray<Vector3> with get, set
        abstract isSphericalHarmonics3: bool
        abstract set: coefficients: ResizeArray<Vector3> -> SphericalHarmonics3
        abstract zero: unit -> SphericalHarmonics3
        abstract add: sh: SphericalHarmonics3 -> SphericalHarmonics3
        abstract addScaledSH: sh: SphericalHarmonics3 * s: float -> SphericalHarmonics3
        abstract scale: s: float -> SphericalHarmonics3
        abstract lerp: sh: SphericalHarmonics3 * alpha: float -> SphericalHarmonics3
        abstract equals: sh: SphericalHarmonics3 -> bool
        abstract copy: sh: SphericalHarmonics3 -> SphericalHarmonics3
        abstract clone: unit -> SphericalHarmonics3
        /// <summary>Sets the values of this spherical harmonics from the provided array or array-like.</summary>
        /// <param name="array">the source array or array-like.</param>
        /// <param name="offset">(optional) offset into the array. Default is 0.</param>
        abstract fromArray: array: U2<ResizeArray<float>, ArrayLike<float>> * ?offset: float -> SphericalHarmonics3
        /// <summary>Returns an array with the values of this spherical harmonics, or copies them into the provided array.</summary>
        /// <param name="array">(optional) array to store the spherical harmonics to. If this is not provided, a new array will be created.</param>
        /// <param name="offset">(optional) optional offset into the array.</param>
        /// <returns>The created or provided array.</returns>
        abstract toArray: ?array: ResizeArray<float> * ?offset: float -> ResizeArray<float>
        /// <summary>Returns an array with the values of this spherical harmonics, or copies them into the provided array-like.</summary>
        /// <param name="array">array-like to store the spherical harmonics to.</param>
        /// <param name="offset">(optional) optional offset into the array-like.</param>
        /// <returns>The provided array-like.</returns>
        abstract toArray: array: ArrayLike<float> * ?offset: float -> ArrayLike<float>
        abstract getAt: normal: Vector3 * target: Vector3 -> Vector3
        abstract getIrradianceAt: normal: Vector3 * target: Vector3 -> Vector3

    type [<AllowNullLiteral>] SphericalHarmonics3Static =
        [<EmitConstructor>] abstract Create: unit -> SphericalHarmonics3
        abstract getBasisAt: normal: Vector3 * shBasis: ResizeArray<float> -> unit

module __math_Triangle =
    type Vector2 = __math_Vector2.Vector2
    type Vector3 = __math_Vector3.Vector3
    type Plane = __math_Plane.Plane
    type Box3 = __math_Box3.Box3

    type [<AllowNullLiteral>] IExports =
        abstract Triangle: TriangleStatic

    type [<AllowNullLiteral>] Triangle =
        /// <default>new THREE.Vector3()</default>
        abstract a: Vector3 with get, set
        /// <default>new THREE.Vector3()</default>
        abstract b: Vector3 with get, set
        /// <default>new THREE.Vector3()</default>
        abstract c: Vector3 with get, set
        abstract set: a: Vector3 * b: Vector3 * c: Vector3 -> Triangle
        abstract setFromPointsAndIndices: points: ResizeArray<Vector3> * i0: float * i1: float * i2: float -> Triangle
        abstract clone: unit -> Triangle
        abstract copy: triangle: Triangle -> Triangle
        abstract getArea: unit -> float
        abstract getMidpoint: target: Vector3 -> Vector3
        abstract getNormal: target: Vector3 -> Vector3
        abstract getPlane: target: Plane -> Plane
        abstract getBarycoord: point: Vector3 * target: Vector3 -> Vector3
        abstract getUV: point: Vector3 * uv1: Vector2 * uv2: Vector2 * uv3: Vector2 * target: Vector2 -> Vector2
        abstract containsPoint: point: Vector3 -> bool
        abstract intersectsBox: box: Box3 -> bool
        abstract isFrontFacing: direction: Vector3 -> bool
        abstract closestPointToPoint: point: Vector3 * target: Vector3 -> Vector3
        abstract equals: triangle: Triangle -> bool

    type [<AllowNullLiteral>] TriangleStatic =
        [<EmitConstructor>] abstract Create: ?a: Vector3 * ?b: Vector3 * ?c: Vector3 -> Triangle
        abstract getNormal: a: Vector3 * b: Vector3 * c: Vector3 * target: Vector3 -> Vector3
        abstract getBarycoord: point: Vector3 * a: Vector3 * b: Vector3 * c: Vector3 * target: Vector3 -> Vector3
        abstract containsPoint: point: Vector3 * a: Vector3 * b: Vector3 * c: Vector3 -> bool
        abstract getUV: point: Vector3 * p1: Vector3 * p2: Vector3 * p3: Vector3 * uv1: Vector2 * uv2: Vector2 * uv3: Vector2 * target: Vector2 -> Vector2
        abstract isFrontFacing: a: Vector3 * b: Vector3 * c: Vector3 * direction: Vector3 -> bool

module __math_Vector2 =
    type Matrix3 = __math_Matrix3.Matrix3
    type BufferAttribute = __core_BufferAttribute.BufferAttribute

    type [<AllowNullLiteral>] IExports =
        /// 2D vector.
        /// 
        /// ( class Vector2 implements Vector<Vector2> )
        abstract Vector2: Vector2Static

    type Vector2Tuple =
        float * float

    /// <summary>
    /// ( interface Vector&lt;T&gt; )
    /// 
    /// Abstract interface of <see href="https://github.com/mrdoob/three.js/blob/master/src/math/Vector2.js">Vector2</see>,
    /// <see href="https://github.com/mrdoob/three.js/blob/master/src/math/Vector3.js">Vector3</see>
    /// and <see href="https://github.com/mrdoob/three.js/blob/master/src/math/Vector4.js">Vector4</see>.
    /// 
    /// Currently the members of Vector is NOT type safe because it accepts different typed vectors.
    /// 
    /// Those definitions will be changed when TypeScript innovates Generics to be type safe.
    /// </summary>
    /// <example>
    /// const v:THREE.Vector = new THREE.Vector3();
    /// v.addVectors(new THREE.Vector2(0, 1), new THREE.Vector2(2, 3)); // invalid but compiled successfully
    /// </example>
    type [<AllowNullLiteral>] Vector =
        abstract setComponent: index: float * value: float -> Vector
        abstract getComponent: index: float -> float
        abstract set: [<ParamArray>] args: float[] -> Vector
        abstract setScalar: scalar: float -> Vector
        /// copy(v:T):T;
        abstract copy: v: Vector -> Vector
        /// NOTE: The second argument is deprecated.
        /// 
        /// add(v:T):T;
        abstract add: v: Vector -> Vector
        /// addVectors(a:T, b:T):T;
        abstract addVectors: a: Vector * b: Vector -> Vector
        abstract addScaledVector: vector: Vector * scale: float -> Vector
        /// Adds the scalar value s to this vector's values.
        abstract addScalar: scalar: float -> Vector
        /// sub(v:T):T;
        abstract sub: v: Vector -> Vector
        /// subVectors(a:T, b:T):T;
        abstract subVectors: a: Vector * b: Vector -> Vector
        /// multiplyScalar(s:number):T;
        abstract multiplyScalar: s: float -> Vector
        /// divideScalar(s:number):T;
        abstract divideScalar: s: float -> Vector
        /// negate():T;
        abstract negate: unit -> Vector
        /// dot(v:T):T;
        abstract dot: v: Vector -> float
        /// lengthSq():number;
        abstract lengthSq: unit -> float
        /// length():number;
        abstract length: unit -> float
        /// normalize():T;
        abstract normalize: unit -> Vector
        /// NOTE: Vector4 doesn't have the property.
        /// 
        /// distanceTo(v:T):number;
        abstract distanceTo: v: Vector -> float
        /// NOTE: Vector4 doesn't have the property.
        /// 
        /// distanceToSquared(v:T):number;
        abstract distanceToSquared: v: Vector -> float
        /// setLength(l:number):T;
        abstract setLength: l: float -> Vector
        /// lerp(v:T, alpha:number):T;
        abstract lerp: v: Vector * alpha: float -> Vector
        /// equals(v:T):boolean;
        abstract equals: v: Vector -> bool
        /// clone():T;
        abstract clone: unit -> Vector

    /// 2D vector.
    /// 
    /// ( class Vector2 implements Vector<Vector2> )
    type [<AllowNullLiteral>] Vector2 =
        inherit Vector
        /// <default>0</default>
        abstract x: float with get, set
        /// <default>0</default>
        abstract y: float with get, set
        abstract width: float with get, set
        abstract height: float with get, set
        abstract isVector2: bool
        /// Sets value of this vector.
        abstract set: x: float * y: float -> Vector2
        /// Sets the x and y values of this vector both equal to scalar.
        abstract setScalar: scalar: float -> Vector2
        /// Sets X component of this vector.
        abstract setX: x: float -> Vector2
        /// Sets Y component of this vector.
        abstract setY: y: float -> Vector2
        /// Sets a component of this vector.
        abstract setComponent: index: float * value: float -> Vector2
        /// Gets a component of this vector.
        abstract getComponent: index: float -> float
        /// <summary>Returns a new Vector2 instance with the same <c>x</c> and <c>y</c> values.</summary>
        abstract clone: unit -> Vector2
        /// Copies value of v to this vector.
        abstract copy: v: Vector2 -> Vector2
        /// Adds v to this vector.
        abstract add: v: Vector2 * ?w: Vector2 -> Vector2
        /// Adds the scalar value s to this vector's x and y values.
        abstract addScalar: s: float -> Vector2
        /// Sets this vector to a + b.
        abstract addVectors: a: Vector2 * b: Vector2 -> Vector2
        /// Adds the multiple of v and s to this vector.
        abstract addScaledVector: v: Vector2 * s: float -> Vector2
        /// Subtracts v from this vector.
        abstract sub: v: Vector2 -> Vector2
        /// Subtracts s from this vector's x and y components.
        abstract subScalar: s: float -> Vector2
        /// Sets this vector to a - b.
        abstract subVectors: a: Vector2 * b: Vector2 -> Vector2
        /// Multiplies this vector by v.
        abstract multiply: v: Vector2 -> Vector2
        /// Multiplies this vector by scalar s.
        abstract multiplyScalar: scalar: float -> Vector2
        /// Divides this vector by v.
        abstract divide: v: Vector2 -> Vector2
        /// Divides this vector by scalar s.
        /// Set vector to ( 0, 0 ) if s == 0.
        abstract divideScalar: s: float -> Vector2
        /// Multiplies this vector (with an implicit 1 as the 3rd component) by m.
        abstract applyMatrix3: m: Matrix3 -> Vector2
        /// If this vector's x or y value is greater than v's x or y value, replace that value with the corresponding min value.
        abstract min: v: Vector2 -> Vector2
        /// If this vector's x or y value is less than v's x or y value, replace that value with the corresponding max value.
        abstract max: v: Vector2 -> Vector2
        /// <summary>
        /// If this vector's x or y value is greater than the max vector's x or y value, it is replaced by the corresponding value.
        /// If this vector's x or y value is less than the min vector's x or y value, it is replaced by the corresponding value.
        /// </summary>
        /// <param name="min">the minimum x and y values.</param>
        /// <param name="max">the maximum x and y values in the desired range.</param>
        abstract clamp: min: Vector2 * max: Vector2 -> Vector2
        /// <summary>
        /// If this vector's x or y values are greater than the max value, they are replaced by the max value.
        /// If this vector's x or y values are less than the min value, they are replaced by the min value.
        /// </summary>
        /// <param name="min">the minimum value the components will be clamped to.</param>
        /// <param name="max">the maximum value the components will be clamped to.</param>
        abstract clampScalar: min: float * max: float -> Vector2
        /// <summary>
        /// If this vector's length is greater than the max value, it is replaced by the max value.
        /// If this vector's length is less than the min value, it is replaced by the min value.
        /// </summary>
        /// <param name="min">the minimum value the length will be clamped to.</param>
        /// <param name="max">the maximum value the length will be clamped to.</param>
        abstract clampLength: min: float * max: float -> Vector2
        /// The components of the vector are rounded down to the nearest integer value.
        abstract floor: unit -> Vector2
        /// The x and y components of the vector are rounded up to the nearest integer value.
        abstract ceil: unit -> Vector2
        /// The components of the vector are rounded to the nearest integer value.
        abstract round: unit -> Vector2
        /// The components of the vector are rounded towards zero (up if negative, down if positive) to an integer value.
        abstract roundToZero: unit -> Vector2
        /// Inverts this vector.
        abstract negate: unit -> Vector2
        /// Computes dot product of this vector and v.
        abstract dot: v: Vector2 -> float
        /// Computes cross product of this vector and v.
        abstract cross: v: Vector2 -> float
        /// Computes squared length of this vector.
        abstract lengthSq: unit -> float
        /// Computes length of this vector.
        abstract length: unit -> float
        [<Obsolete("Use {@link Vector2#manhattanLength .manhattanLength()} instead.")>]
        abstract lengthManhattan: unit -> float
        /// <summary>
        /// Computes the Manhattan length of this vector.
        /// 
        /// see <see href="http://en.wikipedia.org/wiki/Taxicab_geometry">Wikipedia: Taxicab Geometry</see>
        /// </summary>
        abstract manhattanLength: unit -> float
        /// Normalizes this vector.
        abstract normalize: unit -> Vector2
        /// computes the angle in radians with respect to the positive x-axis
        abstract angle: unit -> float
        /// Computes distance of this vector to v.
        abstract distanceTo: v: Vector2 -> float
        /// Computes squared distance of this vector to v.
        abstract distanceToSquared: v: Vector2 -> float
        [<Obsolete("Use {@link Vector2#manhattanDistanceTo .manhattanDistanceTo()} instead.")>]
        abstract distanceToManhattan: v: Vector2 -> float
        /// <summary>
        /// Computes the Manhattan length (distance) from this vector to the given vector v
        /// 
        /// see <see href="http://en.wikipedia.org/wiki/Taxicab_geometry">Wikipedia: Taxicab Geometry</see>
        /// </summary>
        abstract manhattanDistanceTo: v: Vector2 -> float
        /// Normalizes this vector and multiplies it by l.
        abstract setLength: length: float -> Vector2
        /// <summary>Linearly interpolates between this vector and v, where alpha is the distance along the line - alpha = 0 will be this vector, and alpha = 1 will be v.</summary>
        /// <param name="v">vector to interpolate towards.</param>
        /// <param name="alpha">interpolation factor in the closed interval [0, 1].</param>
        abstract lerp: v: Vector2 * alpha: float -> Vector2
        /// <summary>Sets this vector to be the vector linearly interpolated between v1 and v2 where alpha is the distance along the line connecting the two vectors - alpha = 0 will be v1, and alpha = 1 will be v2.</summary>
        /// <param name="v1">the starting vector.</param>
        /// <param name="v2">vector to interpolate towards.</param>
        /// <param name="alpha">interpolation factor in the closed interval [0, 1].</param>
        abstract lerpVectors: v1: Vector2 * v2: Vector2 * alpha: float -> Vector2
        /// Checks for strict equality of this vector and v.
        abstract equals: v: Vector2 -> bool
        /// <summary>Sets this vector's x and y value from the provided array or array-like.</summary>
        /// <param name="array">the source array or array-like.</param>
        /// <param name="offset">(optional) offset into the array. Default is 0.</param>
        abstract fromArray: array: U2<ResizeArray<float>, ArrayLike<float>> * ?offset: float -> Vector2
        /// <summary>Returns an array [x, y], or copies x and y into the provided array.</summary>
        /// <param name="array">(optional) array to store the vector to. If this is not provided, a new array will be created.</param>
        /// <param name="offset">(optional) optional offset into the array.</param>
        /// <returns>The created or provided array.</returns>
        abstract toArray: ?array: ResizeArray<float> * ?offset: float -> ResizeArray<float>
        abstract toArray: ?array: Vector2Tuple * ?offset: int -> Vector2Tuple
        /// <summary>Copies x and y into the provided array-like.</summary>
        /// <param name="array">array-like to store the vector to.</param>
        /// <param name="offset">(optional) optional offset into the array.</param>
        /// <returns>The provided array-like.</returns>
        abstract toArray: array: ArrayLike<float> * ?offset: float -> ArrayLike<float>
        /// <summary>Sets this vector's x and y values from the attribute.</summary>
        /// <param name="attribute">the source attribute.</param>
        /// <param name="index">index in the attribute.</param>
        abstract fromBufferAttribute: attribute: BufferAttribute * index: float -> Vector2
        /// <summary>Rotates the vector around center by angle radians.</summary>
        /// <param name="center">the point around which to rotate.</param>
        /// <param name="angle">the angle to rotate, in radians.</param>
        abstract rotateAround: center: Vector2 * angle: float -> Vector2
        /// Sets this vector's x and y from Math.random
        abstract random: unit -> Vector2

    /// 2D vector.
    /// 
    /// ( class Vector2 implements Vector<Vector2> )
    type [<AllowNullLiteral>] Vector2Static =
        [<EmitConstructor>] abstract Create: ?x: float * ?y: float -> Vector2

module __math_Vector3 =
    type Euler = __math_Euler.Euler
    type Matrix3 = __math_Matrix3.Matrix3
    type Matrix4 = __math_Matrix4.Matrix4
    type Quaternion = __math_Quaternion.Quaternion
    type Camera = __cameras_Camera.Camera
    type Spherical = __math_Spherical.Spherical
    type Cylindrical = __math_Cylindrical.Cylindrical
    type BufferAttribute = __core_BufferAttribute.BufferAttribute
    type InterleavedBufferAttribute = __core_InterleavedBufferAttribute.InterleavedBufferAttribute
    type Vector = __math_Vector2.Vector

    type [<AllowNullLiteral>] IExports =
        /// <summary>
        /// 3D vector. ( class Vector3 implements Vector&lt;Vector3&gt; )
        /// 
        /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/math/Vector3.js" />
        /// </summary>
        /// <example>
        /// const a = new THREE.Vector3( 1, 0, 0 );
        /// const b = new THREE.Vector3( 0, 1, 0 );
        /// const c = new THREE.Vector3();
        /// c.crossVectors( a, b );
        /// </example>
        abstract Vector3: Vector3Static

    type Vector3Tuple =
        float * float * float

    /// <summary>
    /// 3D vector. ( class Vector3 implements Vector&lt;Vector3&gt; )
    /// 
    /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/math/Vector3.js" />
    /// </summary>
    /// <example>
    /// const a = new THREE.Vector3( 1, 0, 0 );
    /// const b = new THREE.Vector3( 0, 1, 0 );
    /// const c = new THREE.Vector3();
    /// c.crossVectors( a, b );
    /// </example>
    type [<AllowNullLiteral>] Vector3 =
        inherit Vector
        /// <default>0</default>
        abstract x: float with get, set
        /// <default>0</default>
        abstract y: float with get, set
        /// <default>0</default>
        abstract z: float with get, set
        abstract isVector3: bool
        /// Sets value of this vector.
        abstract set: x: float * y: float * z: float -> Vector3
        /// Sets all values of this vector.
        abstract setScalar: scalar: float -> Vector3
        /// Sets x value of this vector.
        abstract setX: x: float -> Vector3
        /// Sets y value of this vector.
        abstract setY: y: float -> Vector3
        /// Sets z value of this vector.
        abstract setZ: z: float -> Vector3
        abstract setComponent: index: float * value: float -> Vector3
        abstract getComponent: index: float -> float
        /// Clones this vector.
        abstract clone: unit -> Vector3
        /// Copies value of v to this vector.
        abstract copy: v: Vector3 -> Vector3
        /// Adds v to this vector.
        abstract add: v: Vector3 -> Vector3
        /// Adds the scalar value s to this vector's values.
        abstract addScalar: s: float -> Vector3
        abstract addScaledVector: v: Vector3 * s: float -> Vector3
        /// Sets this vector to a + b.
        abstract addVectors: a: Vector3 * b: Vector3 -> Vector3
        /// Subtracts v from this vector.
        abstract sub: a: Vector3 -> Vector3
        abstract subScalar: s: float -> Vector3
        /// Sets this vector to a - b.
        abstract subVectors: a: Vector3 * b: Vector3 -> Vector3
        abstract multiply: v: Vector3 -> Vector3
        /// Multiplies this vector by scalar s.
        abstract multiplyScalar: s: float -> Vector3
        abstract multiplyVectors: a: Vector3 * b: Vector3 -> Vector3
        abstract applyEuler: euler: Euler -> Vector3
        abstract applyAxisAngle: axis: Vector3 * angle: float -> Vector3
        abstract applyMatrix3: m: Matrix3 -> Vector3
        abstract applyNormalMatrix: m: Matrix3 -> Vector3
        abstract applyMatrix4: m: Matrix4 -> Vector3
        abstract applyQuaternion: q: Quaternion -> Vector3
        abstract project: camera: Camera -> Vector3
        abstract unproject: camera: Camera -> Vector3
        abstract transformDirection: m: Matrix4 -> Vector3
        abstract divide: v: Vector3 -> Vector3
        /// Divides this vector by scalar s.
        /// Set vector to ( 0, 0, 0 ) if s == 0.
        abstract divideScalar: s: float -> Vector3
        abstract min: v: Vector3 -> Vector3
        abstract max: v: Vector3 -> Vector3
        abstract clamp: min: Vector3 * max: Vector3 -> Vector3
        abstract clampScalar: min: float * max: float -> Vector3
        abstract clampLength: min: float * max: float -> Vector3
        abstract floor: unit -> Vector3
        abstract ceil: unit -> Vector3
        abstract round: unit -> Vector3
        abstract roundToZero: unit -> Vector3
        /// Inverts this vector.
        abstract negate: unit -> Vector3
        /// Computes dot product of this vector and v.
        abstract dot: v: Vector3 -> float
        /// Computes squared length of this vector.
        abstract lengthSq: unit -> float
        /// Computes length of this vector.
        abstract length: unit -> float
        /// <summary>
        /// Computes Manhattan length of this vector.
        /// <see href="http://en.wikipedia.org/wiki/Taxicab_geometry" />
        /// </summary>
        [<Obsolete("Use {@link Vector3#manhattanLength .manhattanLength()} instead.")>]
        abstract lengthManhattan: unit -> float
        /// <summary>
        /// Computes the Manhattan length of this vector.
        /// 
        /// see <see href="http://en.wikipedia.org/wiki/Taxicab_geometry">Wikipedia: Taxicab Geometry</see>
        /// </summary>
        abstract manhattanLength: unit -> float
        /// <summary>
        /// Computes the Manhattan length (distance) from this vector to the given vector v
        /// 
        /// see <see href="http://en.wikipedia.org/wiki/Taxicab_geometry">Wikipedia: Taxicab Geometry</see>
        /// </summary>
        abstract manhattanDistanceTo: v: Vector3 -> float
        /// Normalizes this vector.
        abstract normalize: unit -> Vector3
        /// Normalizes this vector and multiplies it by l.
        abstract setLength: l: float -> Vector3
        /// lerp(v:T, alpha:number):T;
        abstract lerp: v: Vector3 * alpha: float -> Vector3
        abstract lerpVectors: v1: Vector3 * v2: Vector3 * alpha: float -> Vector3
        /// Sets this vector to cross product of itself and v.
        abstract cross: a: Vector3 -> Vector3
        /// Sets this vector to cross product of a and b.
        abstract crossVectors: a: Vector3 * b: Vector3 -> Vector3
        abstract projectOnVector: v: Vector3 -> Vector3
        abstract projectOnPlane: planeNormal: Vector3 -> Vector3
        abstract reflect: vector: Vector3 -> Vector3
        abstract angleTo: v: Vector3 -> float
        /// Computes distance of this vector to v.
        abstract distanceTo: v: Vector3 -> float
        /// Computes squared distance of this vector to v.
        abstract distanceToSquared: v: Vector3 -> float
        [<Obsolete("Use {@link Vector3#manhattanDistanceTo .manhattanDistanceTo()} instead.")>]
        abstract distanceToManhattan: v: Vector3 -> float
        abstract setFromSpherical: s: Spherical -> Vector3
        abstract setFromSphericalCoords: r: float * phi: float * theta: float -> Vector3
        abstract setFromCylindrical: s: Cylindrical -> Vector3
        abstract setFromCylindricalCoords: radius: float * theta: float * y: float -> Vector3
        abstract setFromMatrixPosition: m: Matrix4 -> Vector3
        abstract setFromMatrixScale: m: Matrix4 -> Vector3
        abstract setFromMatrixColumn: matrix: Matrix4 * index: float -> Vector3
        abstract setFromMatrix3Column: matrix: Matrix3 * index: float -> Vector3
        /// Checks for strict equality of this vector and v.
        abstract equals: v: Vector3 -> bool
        /// <summary>Sets this vector's x, y and z value from the provided array or array-like.</summary>
        /// <param name="array">the source array or array-like.</param>
        /// <param name="offset">(optional) offset into the array. Default is 0.</param>
        abstract fromArray: array: U2<ResizeArray<float>, ArrayLike<float>> * ?offset: float -> Vector3
        /// <summary>Returns an array [x, y, z], or copies x, y and z into the provided array.</summary>
        /// <param name="array">(optional) array to store the vector to. If this is not provided, a new array will be created.</param>
        /// <param name="offset">(optional) optional offset into the array.</param>
        /// <returns>The created or provided array.</returns>
        abstract toArray: ?array: ResizeArray<float> * ?offset: float -> ResizeArray<float>
        abstract toArray: ?array: Vector3Tuple * ?offset: int -> Vector3Tuple
        /// <summary>Copies x, y and z into the provided array-like.</summary>
        /// <param name="array">array-like to store the vector to.</param>
        /// <param name="offset">(optional) optional offset into the array-like.</param>
        /// <returns>The provided array-like.</returns>
        abstract toArray: array: ArrayLike<float> * ?offset: float -> ArrayLike<float>
        abstract fromBufferAttribute: attribute: U2<BufferAttribute, InterleavedBufferAttribute> * index: float -> Vector3
        /// Sets this vector's x, y and z from Math.random
        abstract random: unit -> Vector3

    /// <summary>
    /// 3D vector. ( class Vector3 implements Vector&lt;Vector3&gt; )
    /// 
    /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/math/Vector3.js" />
    /// </summary>
    /// <example>
    /// const a = new THREE.Vector3( 1, 0, 0 );
    /// const b = new THREE.Vector3( 0, 1, 0 );
    /// const c = new THREE.Vector3();
    /// c.crossVectors( a, b );
    /// </example>
    type [<AllowNullLiteral>] Vector3Static =
        [<EmitConstructor>] abstract Create: ?x: float * ?y: float * ?z: float -> Vector3

module __math_Vector4 =
    type Matrix4 = __math_Matrix4.Matrix4
    type Quaternion = __math_Quaternion.Quaternion
    type BufferAttribute = __core_BufferAttribute.BufferAttribute
    type Vector = __math_Vector2.Vector

    type [<AllowNullLiteral>] IExports =
        /// 4D vector.
        /// 
        /// ( class Vector4 implements Vector<Vector4> )
        abstract Vector4: Vector4Static

    type Vector4Tuple =
        float * float * float * float

    /// 4D vector.
    /// 
    /// ( class Vector4 implements Vector<Vector4> )
    type [<AllowNullLiteral>] Vector4 =
        inherit Vector
        /// <default>0</default>
        abstract x: float with get, set
        /// <default>0</default>
        abstract y: float with get, set
        /// <default>0</default>
        abstract z: float with get, set
        /// <default>0</default>
        abstract w: float with get, set
        abstract width: float with get, set
        abstract height: float with get, set
        abstract isVector4: bool
        /// Sets value of this vector.
        abstract set: x: float * y: float * z: float * w: float -> Vector4
        /// Sets all values of this vector.
        abstract setScalar: scalar: float -> Vector4
        /// Sets X component of this vector.
        abstract setX: x: float -> Vector4
        /// Sets Y component of this vector.
        abstract setY: y: float -> Vector4
        /// Sets Z component of this vector.
        abstract setZ: z: float -> Vector4
        /// Sets w component of this vector.
        abstract setW: w: float -> Vector4
        abstract setComponent: index: float * value: float -> Vector4
        abstract getComponent: index: float -> float
        /// Clones this vector.
        abstract clone: unit -> Vector4
        /// Copies value of v to this vector.
        abstract copy: v: Vector4 -> Vector4
        /// Adds v to this vector.
        abstract add: v: Vector4 -> Vector4
        /// Adds the scalar value s to this vector's values.
        abstract addScalar: scalar: float -> Vector4
        /// Sets this vector to a + b.
        abstract addVectors: a: Vector4 * b: Vector4 -> Vector4
        abstract addScaledVector: v: Vector4 * s: float -> Vector4
        /// Subtracts v from this vector.
        abstract sub: v: Vector4 -> Vector4
        abstract subScalar: s: float -> Vector4
        /// Sets this vector to a - b.
        abstract subVectors: a: Vector4 * b: Vector4 -> Vector4
        abstract multiply: v: Vector4 -> Vector4
        /// Multiplies this vector by scalar s.
        abstract multiplyScalar: s: float -> Vector4
        abstract applyMatrix4: m: Matrix4 -> Vector4
        /// Divides this vector by scalar s.
        /// Set vector to ( 0, 0, 0 ) if s == 0.
        abstract divideScalar: s: float -> Vector4
        /// <summary><see href="http://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToAngle/index.htm" /></summary>
        /// <param name="q">is assumed to be normalized</param>
        abstract setAxisAngleFromQuaternion: q: Quaternion -> Vector4
        /// <summary><see href="http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToAngle/index.htm" /></summary>
        /// <param name="m">assumes the upper 3x3 of m is a pure rotation matrix (i.e, unscaled)</param>
        abstract setAxisAngleFromRotationMatrix: m: Matrix4 -> Vector4
        abstract min: v: Vector4 -> Vector4
        abstract max: v: Vector4 -> Vector4
        abstract clamp: min: Vector4 * max: Vector4 -> Vector4
        abstract clampScalar: min: float * max: float -> Vector4
        abstract floor: unit -> Vector4
        abstract ceil: unit -> Vector4
        abstract round: unit -> Vector4
        abstract roundToZero: unit -> Vector4
        /// Inverts this vector.
        abstract negate: unit -> Vector4
        /// Computes dot product of this vector and v.
        abstract dot: v: Vector4 -> float
        /// Computes squared length of this vector.
        abstract lengthSq: unit -> float
        /// Computes length of this vector.
        abstract length: unit -> float
        /// <summary>
        /// Computes the Manhattan length of this vector.
        /// 
        /// see <see href="http://en.wikipedia.org/wiki/Taxicab_geometry">Wikipedia: Taxicab Geometry</see>
        /// </summary>
        abstract manhattanLength: unit -> float
        /// Normalizes this vector.
        abstract normalize: unit -> Vector4
        /// Normalizes this vector and multiplies it by l.
        abstract setLength: length: float -> Vector4
        /// Linearly interpolate between this vector and v with alpha factor.
        abstract lerp: v: Vector4 * alpha: float -> Vector4
        abstract lerpVectors: v1: Vector4 * v2: Vector4 * alpha: float -> Vector4
        /// Checks for strict equality of this vector and v.
        abstract equals: v: Vector4 -> bool
        /// <summary>Sets this vector's x, y, z and w value from the provided array or array-like.</summary>
        /// <param name="array">the source array or array-like.</param>
        /// <param name="offset">(optional) offset into the array. Default is 0.</param>
        abstract fromArray: array: U2<ResizeArray<float>, ArrayLike<float>> * ?offset: float -> Vector4
        /// <summary>Returns an array [x, y, z, w], or copies x, y, z and w into the provided array.</summary>
        /// <param name="array">(optional) array to store the vector to. If this is not provided, a new array will be created.</param>
        /// <param name="offset">(optional) optional offset into the array.</param>
        /// <returns>The created or provided array.</returns>
        abstract toArray: ?array: ResizeArray<float> * ?offset: float -> ResizeArray<float>
        abstract toArray: ?array: Vector4Tuple * ?offset: int -> Vector4Tuple
        /// <summary>Copies x, y, z and w into the provided array-like.</summary>
        /// <param name="array">array-like to store the vector to.</param>
        /// <param name="offset">(optional) optional offset into the array-like.</param>
        /// <returns>The provided array-like.</returns>
        abstract toArray: array: ArrayLike<float> * ?offset: float -> ArrayLike<float>
        abstract fromBufferAttribute: attribute: BufferAttribute * index: float -> Vector4
        /// Sets this vector's x, y, z and w from Math.random
        abstract random: unit -> Vector4

    /// 4D vector.
    /// 
    /// ( class Vector4 implements Vector<Vector4> )
    type [<AllowNullLiteral>] Vector4Static =
        [<EmitConstructor>] abstract Create: ?x: float * ?y: float * ?z: float * ?w: float -> Vector4

module __math_interpolants_CubicInterpolant =
    type Interpolant = __math_Interpolant.Interpolant

    type [<AllowNullLiteral>] IExports =
        abstract CubicInterpolant: CubicInterpolantStatic

    type [<AllowNullLiteral>] CubicInterpolant =
        inherit Interpolant
        abstract interpolate_: i1: float * t0: float * t: float * t1: float -> obj option

    type [<AllowNullLiteral>] CubicInterpolantStatic =
        [<EmitConstructor>] abstract Create: parameterPositions: obj option * samplesValues: obj option * sampleSize: float * ?resultBuffer: obj -> CubicInterpolant

module __math_interpolants_DiscreteInterpolant =
    type Interpolant = __math_Interpolant.Interpolant

    type [<AllowNullLiteral>] IExports =
        abstract DiscreteInterpolant: DiscreteInterpolantStatic

    type [<AllowNullLiteral>] DiscreteInterpolant =
        inherit Interpolant
        abstract interpolate_: i1: float * t0: float * t: float * t1: float -> obj option

    type [<AllowNullLiteral>] DiscreteInterpolantStatic =
        [<EmitConstructor>] abstract Create: parameterPositions: obj option * samplesValues: obj option * sampleSize: float * ?resultBuffer: obj -> DiscreteInterpolant

module __math_interpolants_LinearInterpolant =
    type Interpolant = __math_Interpolant.Interpolant

    type [<AllowNullLiteral>] IExports =
        abstract LinearInterpolant: LinearInterpolantStatic

    type [<AllowNullLiteral>] LinearInterpolant =
        inherit Interpolant
        abstract interpolate_: i1: float * t0: float * t: float * t1: float -> obj option

    type [<AllowNullLiteral>] LinearInterpolantStatic =
        [<EmitConstructor>] abstract Create: parameterPositions: obj option * samplesValues: obj option * sampleSize: float * ?resultBuffer: obj -> LinearInterpolant

module __math_interpolants_QuaternionLinearInterpolant =
    type Interpolant = __math_Interpolant.Interpolant

    type [<AllowNullLiteral>] IExports =
        abstract QuaternionLinearInterpolant: QuaternionLinearInterpolantStatic

    type [<AllowNullLiteral>] QuaternionLinearInterpolant =
        inherit Interpolant
        abstract interpolate_: i1: float * t0: float * t: float * t1: float -> obj option

    type [<AllowNullLiteral>] QuaternionLinearInterpolantStatic =
        [<EmitConstructor>] abstract Create: parameterPositions: obj option * samplesValues: obj option * sampleSize: float * ?resultBuffer: obj -> QuaternionLinearInterpolant



module __scenes_Fog =
    type ColorRepresentation = Utils.ColorRepresentation
    type Color = __math_Color.Color

    type [<AllowNullLiteral>] IExports =
        /// This class contains the parameters that define linear fog, i.e., that grows linearly denser with the distance.
        abstract Fog: FogStatic

    type [<AllowNullLiteral>] FogBase =
        abstract name: string with get, set
        abstract color: Color with get, set
        abstract clone: unit -> FogBase
        abstract toJSON: unit -> obj option

    /// This class contains the parameters that define linear fog, i.e., that grows linearly denser with the distance.
    type [<AllowNullLiteral>] Fog =
        inherit FogBase
        /// <default>''</default>
        abstract name: string with get, set
        /// Fog color.
        abstract color: Color with get, set
        /// <summary>The minimum distance to start applying fog. Objects that are less than 'near' units from the active camera won't be affected by fog.</summary>
        /// <default>1</default>
        abstract near: float with get, set
        /// <summary>The maximum distance at which fog stops being calculated and applied. Objects that are more than 'far' units away from the active camera won't be affected by fog.</summary>
        /// <default>1000</default>
        abstract far: float with get, set
        abstract isFog: bool
        abstract clone: unit -> Fog
        abstract toJSON: unit -> obj option

    /// This class contains the parameters that define linear fog, i.e., that grows linearly denser with the distance.
    type [<AllowNullLiteral>] FogStatic =
        [<EmitConstructor>] abstract Create: color: ColorRepresentation * ?near: float * ?far: float -> Fog

module __scenes_FogExp2 =
    type Color = __math_Color.Color
    type FogBase = __scenes_Fog.FogBase

    type [<AllowNullLiteral>] IExports =
        /// This class contains the parameters that define linear fog, i.e., that grows exponentially denser with the distance.
        abstract FogExp2: FogExp2Static

    /// This class contains the parameters that define linear fog, i.e., that grows exponentially denser with the distance.
    type [<AllowNullLiteral>] FogExp2 =
        inherit FogBase
        /// <default>''</default>
        abstract name: string with get, set
        abstract color: Color with get, set
        /// <summary>Defines how fast the fog will grow dense.</summary>
        /// <default>0.00025</default>
        abstract density: float with get, set
        abstract isFogExp2: bool
        abstract clone: unit -> FogExp2
        abstract toJSON: unit -> obj option

    /// This class contains the parameters that define linear fog, i.e., that grows exponentially denser with the distance.
    type [<AllowNullLiteral>] FogExp2Static =
        [<EmitConstructor>] abstract Create: hex: U2<float, string> * ?density: float -> FogExp2

module __scenes_Scene =
    type FogBase = __scenes_Fog.FogBase
    type Material = __materials_Material.Material
    type Object3D = __core_Object3D.Object3D
    type Color = __math_Color.Color
    type Texture = __textures_Texture.Texture
    type WebGLRenderer = __renderers_WebGLRenderer.WebGLRenderer
    type Camera = __cameras_Camera.Camera

    type [<AllowNullLiteral>] IExports =
        /// Scenes allow you to set up what and where is to be rendered by three.js. This is where you place objects, lights and cameras.
        abstract Scene: SceneStatic

    /// Scenes allow you to set up what and where is to be rendered by three.js. This is where you place objects, lights and cameras.
    type [<AllowNullLiteral>] Scene =
        inherit Object3D
        abstract ``type``: string with get, set
        /// <summary>A fog instance defining the type of fog that affects everything rendered in the scene. Default is null.</summary>
        /// <default>null</default>
        abstract fog: FogBase option with get, set
        /// <summary>If not null, it will force everything in the scene to be rendered with that material. Default is null.</summary>
        /// <default>null</default>
        abstract overrideMaterial: Material option with get, set
        /// <default>true</default>
        abstract autoUpdate: bool with get, set
        /// <default>null</default>
        abstract background: U2<Color, Texture> option with get, set
        /// <default>null</default>
        abstract environment: Texture option with get, set
        abstract isScene: bool
        /// Calls before rendering scene
        abstract onBeforeRender: (WebGLRenderer -> Scene -> Camera -> obj option -> unit) with get, set
        /// Calls after rendering scene
        abstract onAfterRender: (WebGLRenderer -> Scene -> Camera -> unit) with get, set
        abstract toJSON: ?meta: obj -> obj option

    /// Scenes allow you to set up what and where is to be rendered by three.js. This is where you place objects, lights and cameras.
    type [<AllowNullLiteral>] SceneStatic =
        [<EmitConstructor>] abstract Create: unit -> Scene



module __objects_Bone =
    type Object3D = __core_Object3D.Object3D

    type [<AllowNullLiteral>] IExports =
        abstract Bone: BoneStatic

    type [<AllowNullLiteral>] Bone =
        inherit Object3D
        abstract isBone: bool
        abstract ``type``: string with get, set

    type [<AllowNullLiteral>] BoneStatic =
        [<EmitConstructor>] abstract Create: unit -> Bone

module __objects_Group =
    type Object3D = __core_Object3D.Object3D

    type [<AllowNullLiteral>] IExports =
        abstract Group: GroupStatic

    type [<AllowNullLiteral>] Group =
        inherit Object3D
        abstract ``type``: string with get, set
        abstract isGroup: bool

    type [<AllowNullLiteral>] GroupStatic =
        [<EmitConstructor>] abstract Create: unit -> Group

module __objects_InstancedMesh =
    open __objects_Mesh
    type BufferGeometry = __core_BufferGeometry.BufferGeometry
    type Material = __materials_Material.Material
    type BufferAttribute = __core_BufferAttribute.BufferAttribute
    type Matrix4 = __math_Matrix4.Matrix4
    type Color = __math_Color.Color

    type [<AllowNullLiteral>] IExports =
        abstract InstancedMesh: InstancedMeshStatic
// { Attributes = []  Comments = []  Name = InstancedMesh  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/objects/InstancedMesh".InstancedMesh  Type = Generic ({ Type = Mapped ({ Name = InstancedMesh  FullName = InstancedMesh  Declarations = [object Object] })  TypeParameters = [Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] }); Union ({ Option = false  Types = [Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }); Array (Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }))] })] })  TypeParameters = [] }
// { Attributes = []  Comments = []  Name = InstancedMesh  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/objects/InstancedMesh".InstancedMesh  Type = Generic ({ Type = Mapped ({ Name = InstancedMesh  FullName = InstancedMesh  Declarations = [object Object] })  TypeParameters = [GenericTypeParameter ({ Name = 'TGeometry  Constraint = Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] })  Default = undefined }); Union ({ Option = false  Types = [Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }); Array (Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }))] })] })  TypeParameters = [GenericTypeParameter ({ Name = 'TGeometry  Constraint = Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] })  Default = undefined })] }

    type [<AllowNullLiteral>] InstancedMesh<'TGeometry, 'TMaterial when 'TGeometry :> BufferGeometry> =
        inherit Mesh<'TGeometry, 'TMaterial>
        abstract count: float with get, set
        abstract instanceColor: BufferAttribute option with get, set
        abstract instanceMatrix: BufferAttribute with get, set
        abstract isInstancedMesh: bool
        abstract getColorAt: index: float * color: Color -> unit
        abstract getMatrixAt: index: float * matrix: Matrix4 -> unit
        abstract setColorAt: index: float * color: Color -> unit
        abstract setMatrixAt: index: float * matrix: Matrix4 -> unit
        abstract dispose: unit -> unit

    type [<AllowNullLiteral>] InstancedMeshStatic =
        [<EmitConstructor>] abstract Create: geometry: 'TGeometry option * material: 'TMaterial option * count: float -> InstancedMesh<'TGeometry, 'TMaterial>

module __objects_LOD =
    type Object3D = __core_Object3D.Object3D
    type Raycaster = __core_Raycaster.Raycaster
    type Camera = __cameras_Camera.Camera
    type Intersection = __core_Raycaster.Intersection

    type [<AllowNullLiteral>] IExports =
        abstract LOD: LODStatic

    type [<AllowNullLiteral>] LOD =
        inherit Object3D
        abstract ``type``: string with get, set
        abstract levels: Array<{| distance: float; object: Object3D |}> with get, set
        abstract autoUpdate: bool with get, set
        abstract isLOD: bool
        abstract addLevel: object: Object3D * ?distance: float -> LOD
        abstract getCurrentLevel: unit -> float
        abstract getObjectForDistance: distance: float -> Object3D option
        abstract raycast: raycaster: Raycaster * intersects: ResizeArray<Intersection> -> unit
        abstract update: camera: Camera -> unit
        abstract toJSON: meta: obj option -> obj option
        [<Obsolete("Use {@link LOD#levels .levels} instead.")>]
        abstract objects: ResizeArray<obj option> with get, set

    type [<AllowNullLiteral>] LODStatic =
        [<EmitConstructor>] abstract Create: unit -> LOD

module __objects_Line =
    type Material = __materials_Material.Material
    type Raycaster = __core_Raycaster.Raycaster
    type Object3D = __core_Object3D.Object3D
    type BufferGeometry = __core_BufferGeometry.BufferGeometry
    type Intersection = __core_Raycaster.Intersection

    type [<AllowNullLiteral>] IExports =
        abstract Line: LineStatic
// { Attributes = []  Comments = []  Name = Line  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/objects/Line".Line  Type = Generic ({ Type = Mapped ({ Name = Line  FullName = Line  Declarations = [object Object] })  TypeParameters = [Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] }); Union ({ Option = false  Types = [Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }); Array (Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }))] })] })  TypeParameters = [] }
// { Attributes = []  Comments = []  Name = Line  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/objects/Line".Line  Type = Generic ({ Type = Mapped ({ Name = Line  FullName = Line  Declarations = [object Object] })  TypeParameters = [GenericTypeParameter ({ Name = 'TGeometry  Constraint = Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] })  Default = undefined }); Union ({ Option = false  Types = [Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }); Array (Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }))] })] })  TypeParameters = [GenericTypeParameter ({ Name = 'TGeometry  Constraint = Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] })  Default = undefined })] }

    type [<AllowNullLiteral>] Line<'TGeometry, 'TMaterial when 'TGeometry :> BufferGeometry> =
        inherit Object3D
        abstract geometry: 'TGeometry with get, set
        abstract material: 'TMaterial with get, set
        abstract ``type``: U2<string, string> with get, set
        abstract isLine: bool
        abstract morphTargetInfluences: ResizeArray<float> option with get, set
        abstract morphTargetDictionary: LineMorphTargetDictionary option with get, set
        abstract computeLineDistances: unit -> Line<'TGeometry, 'TMaterial>
        abstract raycast: raycaster: Raycaster * intersects: ResizeArray<Intersection> -> unit
        abstract updateMorphTargets: unit -> unit

    type [<AllowNullLiteral>] LineStatic =
        [<EmitConstructor>] abstract Create: ?geometry: 'TGeometry * ?material: 'TMaterial -> Line<'TGeometry, 'TMaterial>

    type [<AllowNullLiteral>] LineMorphTargetDictionary =
        [<EmitIndexer>] abstract Item: key: string -> float with get, set

module __objects_LineLoop =
    open __objects_Line
    type Material = __materials_Material.Material
    type BufferGeometry = __core_BufferGeometry.BufferGeometry

    type [<AllowNullLiteral>] IExports =
        abstract LineLoop: LineLoopStatic
// { Attributes = []  Comments = []  Name = LineLoop  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/objects/LineLoop".LineLoop  Type = Generic ({ Type = Mapped ({ Name = LineLoop  FullName = LineLoop  Declarations = [object Object] })  TypeParameters = [Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] }); Union ({ Option = false  Types = [Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }); Array (Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }))] })] })  TypeParameters = [] }
// { Attributes = []  Comments = []  Name = LineLoop  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/objects/LineLoop".LineLoop  Type = Generic ({ Type = Mapped ({ Name = LineLoop  FullName = LineLoop  Declarations = [object Object] })  TypeParameters = [GenericTypeParameter ({ Name = 'TGeometry  Constraint = Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] })  Default = undefined }); Union ({ Option = false  Types = [Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }); Array (Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }))] })] })  TypeParameters = [GenericTypeParameter ({ Name = 'TGeometry  Constraint = Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] })  Default = undefined })] }

    type [<AllowNullLiteral>] LineLoop<'TGeometry, 'TMaterial when 'TGeometry :> BufferGeometry> =
        inherit Line<'TGeometry, 'TMaterial>
        abstract ``type``: string with get, set
        abstract isLineLoop: bool

    type [<AllowNullLiteral>] LineLoopStatic =
        [<EmitConstructor>] abstract Create: ?geometry: 'TGeometry * ?material: 'TMaterial -> LineLoop<'TGeometry, 'TMaterial>

module __objects_LineSegments =
    open __objects_Line
    type Material = __materials_Material.Material
    type BufferGeometry = __core_BufferGeometry.BufferGeometry

    type [<AllowNullLiteral>] IExports =
        [<Obsolete("")>]
        abstract LineStrip: float
        [<Obsolete("")>]
        abstract LinePieces: float
        abstract LineSegments: LineSegmentsStatic
// { Attributes = []  Comments = []  Name = LineSegments  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/objects/LineSegments".LineSegments  Type = Generic ({ Type = Mapped ({ Name = LineSegments  FullName = LineSegments  Declarations = [object Object] })  TypeParameters = [Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] }); Union ({ Option = false  Types = [Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }); Array (Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }))] })] })  TypeParameters = [] }
// { Attributes = []  Comments = []  Name = LineSegments  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/objects/LineSegments".LineSegments  Type = Generic ({ Type = Mapped ({ Name = LineSegments  FullName = LineSegments  Declarations = [object Object] })  TypeParameters = [GenericTypeParameter ({ Name = 'TGeometry  Constraint = Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] })  Default = undefined }); Union ({ Option = false  Types = [Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }); Array (Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }))] })] })  TypeParameters = [GenericTypeParameter ({ Name = 'TGeometry  Constraint = Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] })  Default = undefined })] }

    type [<AllowNullLiteral>] LineSegments<'TGeometry, 'TMaterial when 'TGeometry :> BufferGeometry> =
        inherit Line<'TGeometry, 'TMaterial>
        /// <default>'LineSegments'</default>
        abstract ``type``: U2<string, string> with get, set
        abstract isLineSegments: bool

    type [<AllowNullLiteral>] LineSegmentsStatic =
        [<EmitConstructor>] abstract Create: ?geometry: 'TGeometry * ?material: 'TMaterial -> LineSegments<'TGeometry, 'TMaterial>

module __objects_Mesh =
    type Material = __materials_Material.Material
    type Raycaster = __core_Raycaster.Raycaster
    type Object3D = __core_Object3D.Object3D
    type BufferGeometry = __core_BufferGeometry.BufferGeometry
    type Intersection = __core_Raycaster.Intersection

    type [<AllowNullLiteral>] IExports =
        abstract Mesh: MeshStatic
// { Attributes = []  Comments = []  Name = Mesh  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/objects/Mesh".Mesh  Type = Generic ({ Type = Mapped ({ Name = Mesh  FullName = Mesh  Declarations = [object Object] })  TypeParameters = [Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] }); Union ({ Option = false  Types = [Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }); Array (Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }))] })] })  TypeParameters = [] }
// { Attributes = []  Comments = []  Name = Mesh  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/objects/Mesh".Mesh  Type = Generic ({ Type = Mapped ({ Name = Mesh  FullName = Mesh  Declarations = [object Object] })  TypeParameters = [GenericTypeParameter ({ Name = 'TGeometry  Constraint = Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] })  Default = undefined }); Union ({ Option = false  Types = [Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }); Array (Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }))] })] })  TypeParameters = [GenericTypeParameter ({ Name = 'TGeometry  Constraint = Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] })  Default = undefined })] }

    type [<AllowNullLiteral>] Mesh<'TGeometry, 'TMaterial when 'TGeometry :> BufferGeometry> =
        inherit Object3D
        abstract geometry: 'TGeometry with get, set
        abstract material: 'TMaterial with get, set
        abstract morphTargetInfluences: ResizeArray<float> option with get, set
        abstract morphTargetDictionary: MeshMorphTargetDictionary option with get, set
        abstract isMesh: bool
        abstract ``type``: string with get, set
        abstract updateMorphTargets: unit -> unit
        abstract raycast: raycaster: Raycaster * intersects: ResizeArray<Intersection> -> unit

    type [<AllowNullLiteral>] MeshStatic =
        [<EmitConstructor>] abstract Create: ?geometry: 'TGeometry * ?material: 'TMaterial -> Mesh<'TGeometry, 'TMaterial>

    type [<AllowNullLiteral>] MeshMorphTargetDictionary =
        [<EmitIndexer>] abstract Item: key: string -> float with get, set

module __objects_Points =
    type Material = __materials_Material.Material
    type Raycaster = __core_Raycaster.Raycaster
    type Object3D = __core_Object3D.Object3D
    type BufferGeometry = __core_BufferGeometry.BufferGeometry
    type Intersection = __core_Raycaster.Intersection

    type [<AllowNullLiteral>] IExports =
        /// A class for displaying points. The points are rendered by the WebGLRenderer using gl.POINTS.
        abstract Points: PointsStatic
// { Attributes = []  Comments = [Summary ([A class for displaying points. The points are rendered by the WebGLRenderer using gl.POINTS.])]  Name = Points  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/objects/Points".Points  Type = Generic ({ Type = Mapped ({ Name = Points  FullName = Points  Declarations = [object Object] })  TypeParameters = [Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] }); Union ({ Option = false  Types = [Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }); Array (Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }))] })] })  TypeParameters = [] }
// { Attributes = []  Comments = [Summary ([A class for displaying points. The points are rendered by the WebGLRenderer using gl.POINTS.])]  Name = Points  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/objects/Points".Points  Type = Generic ({ Type = Mapped ({ Name = Points  FullName = Points  Declarations = [object Object] })  TypeParameters = [GenericTypeParameter ({ Name = 'TGeometry  Constraint = Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] })  Default = undefined }); Union ({ Option = false  Types = [Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }); Array (Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }))] })] })  TypeParameters = [GenericTypeParameter ({ Name = 'TGeometry  Constraint = Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] })  Default = undefined })] }

    /// A class for displaying points. The points are rendered by the WebGLRenderer using gl.POINTS.
    type [<AllowNullLiteral>] Points<'TGeometry, 'TMaterial when 'TGeometry :> BufferGeometry> =
        inherit Object3D
        abstract ``type``: string with get, set
        abstract morphTargetInfluences: ResizeArray<float> option with get, set
        abstract morphTargetDictionary: PointsMorphTargetDictionary option with get, set
        abstract isPoints: bool
        /// An instance of BufferGeometry, where each vertex designates the position of a particle in the system.
        abstract geometry: 'TGeometry with get, set
        /// An instance of Material, defining the object's appearance. Default is a PointsMaterial with randomised colour.
        abstract material: 'TMaterial with get, set
        abstract raycast: raycaster: Raycaster * intersects: ResizeArray<Intersection> -> unit
        abstract updateMorphTargets: unit -> unit

    /// A class for displaying points. The points are rendered by the WebGLRenderer using gl.POINTS.
    type [<AllowNullLiteral>] PointsStatic =
        /// <param name="geometry">An instance of BufferGeometry.</param>
        /// <param name="material">An instance of Material (optional).</param>
        [<EmitConstructor>] abstract Create: ?geometry: 'TGeometry * ?material: 'TMaterial -> Points<'TGeometry, 'TMaterial>

    type [<AllowNullLiteral>] PointsMorphTargetDictionary =
        [<EmitIndexer>] abstract Item: key: string -> float with get, set

module __objects_Skeleton =
    type Bone = __objects_Bone.Bone
    type Matrix4 = __math_Matrix4.Matrix4
    type DataTexture = __textures_DataTexture.DataTexture

    type [<AllowNullLiteral>] IExports =
        abstract Skeleton: SkeletonStatic

    type [<AllowNullLiteral>] Skeleton =
        abstract uuid: string with get, set
        abstract bones: ResizeArray<Bone> with get, set
        abstract boneInverses: ResizeArray<Matrix4> with get, set
        abstract boneMatrices: Float32Array with get, set
        abstract boneTexture: DataTexture option with get, set
        abstract boneTextureSize: float with get, set
        abstract frame: float with get, set
        abstract init: unit -> unit
        abstract calculateInverses: unit -> unit
        abstract computeBoneTexture: unit -> Skeleton
        abstract pose: unit -> unit
        abstract update: unit -> unit
        abstract clone: unit -> Skeleton
        abstract getBoneByName: name: string -> Bone option
        abstract dispose: unit -> unit
        [<Obsolete("This property has been removed completely.")>]
        abstract useVertexTexture: bool with get, set

    type [<AllowNullLiteral>] SkeletonStatic =
        [<EmitConstructor>] abstract Create: bones: ResizeArray<Bone> * ?boneInverses: ResizeArray<Matrix4> -> Skeleton

module __objects_SkinnedMesh =
    open __objects_Mesh
    type Material = __materials_Material.Material
    type Matrix4 = __math_Matrix4.Matrix4
    type Vector3 = __math_Vector3.Vector3
    type Skeleton = __objects_Skeleton.Skeleton
    type BufferGeometry = __core_BufferGeometry.BufferGeometry

    type [<AllowNullLiteral>] IExports =
        abstract SkinnedMesh: SkinnedMeshStatic
// { Attributes = []  Comments = []  Name = SkinnedMesh  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/objects/SkinnedMesh".SkinnedMesh  Type = Generic ({ Type = Mapped ({ Name = SkinnedMesh  FullName = SkinnedMesh  Declarations = [object Object] })  TypeParameters = [Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] }); Union ({ Option = false  Types = [Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }); Array (Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }))] })] })  TypeParameters = [] }
// { Attributes = []  Comments = []  Name = SkinnedMesh  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/objects/SkinnedMesh".SkinnedMesh  Type = Generic ({ Type = Mapped ({ Name = SkinnedMesh  FullName = SkinnedMesh  Declarations = [object Object] })  TypeParameters = [GenericTypeParameter ({ Name = 'TGeometry  Constraint = Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] })  Default = undefined }); Union ({ Option = false  Types = [Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }); Array (Mapped ({ Name = Material  FullName = Material  Declarations = [object Object] }))] })] })  TypeParameters = [GenericTypeParameter ({ Name = 'TGeometry  Constraint = Mapped ({ Name = BufferGeometry  FullName = BufferGeometry  Declarations = [object Object] })  Default = undefined })] }

    type [<AllowNullLiteral>] SkinnedMesh<'TGeometry, 'TMaterial when 'TGeometry :> BufferGeometry> =
        inherit Mesh<'TGeometry, 'TMaterial>
        abstract bindMode: string with get, set
        abstract bindMatrix: Matrix4 with get, set
        abstract bindMatrixInverse: Matrix4 with get, set
        abstract skeleton: Skeleton with get, set
        abstract isSkinnedMesh: bool
        abstract bind: skeleton: Skeleton * ?bindMatrix: Matrix4 -> unit
        abstract pose: unit -> unit
        abstract normalizeSkinWeights: unit -> unit
        abstract updateMatrixWorld: ?force: bool -> unit
        abstract boneTransform: index: float * target: Vector3 -> Vector3

    type [<AllowNullLiteral>] SkinnedMeshStatic =
        [<EmitConstructor>] abstract Create: ?geometry: 'TGeometry * ?material: 'TMaterial * ?useVertexTexture: bool -> SkinnedMesh<'TGeometry, 'TMaterial>

module __objects_Sprite =
    type Vector2 = __math_Vector2.Vector2
    type Raycaster = __core_Raycaster.Raycaster
    type Object3D = __core_Object3D.Object3D
    type Intersection = __core_Raycaster.Intersection
    type SpriteMaterial = __materials_SpriteMaterial.SpriteMaterial
    // type [<AllowNullLiteral>] SpriteMaterial = interface end
    type BufferGeometry = __core_BufferGeometry.BufferGeometry

    type [<AllowNullLiteral>] IExports =
        abstract Sprite: SpriteStatic

    type [<AllowNullLiteral>] Sprite =
        inherit Object3D
        abstract ``type``: string with get, set
        abstract isSprite: bool
        abstract geometry: BufferGeometry with get, set
        abstract material: SpriteMaterial with get, set
        abstract center: Vector2 with get, set
        abstract raycast: raycaster: Raycaster * intersects: ResizeArray<Intersection> -> unit
        abstract copy: source: Sprite -> Sprite

    type [<AllowNullLiteral>] SpriteStatic =
        [<EmitConstructor>] abstract Create: ?material: SpriteMaterial -> Sprite



module __materials_LineBasicMaterial =
    type ColorRepresentation = Utils.ColorRepresentation
    type Color = __math_Color.Color
    type MaterialParameters = __materials_Material.MaterialParameters
    type Material = __materials_Material.Material

    type [<AllowNullLiteral>] IExports =
        abstract LineBasicMaterial: LineBasicMaterialStatic

    type [<AllowNullLiteral>] LineBasicMaterialParameters =
        inherit MaterialParameters
        abstract color: ColorRepresentation option with get, set
        abstract linewidth: float option with get, set
        abstract linecap: string option with get, set
        abstract linejoin: string option with get, set

    type [<AllowNullLiteral>] LineBasicMaterial =
        inherit Material
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'LineBasicMaterial'</default>
        abstract ``type``: string with get, set
        /// <default>0xffffff</default>
        abstract color: Color with get, set
        /// <default>1</default>
        abstract linewidth: float with get, set
        /// <default>'round'</default>
        abstract linecap: string with get, set
        /// <default>'round'</default>
        abstract linejoin: string with get, set
        /// Sets the properties based on the values.
        abstract setValues: parameters: LineBasicMaterialParameters -> unit

    type [<AllowNullLiteral>] LineBasicMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: LineBasicMaterialParameters -> LineBasicMaterial

module __materials_LineDashedMaterial =
    type LineBasicMaterial = __materials_LineBasicMaterial.LineBasicMaterial
    type LineBasicMaterialParameters = __materials_LineBasicMaterial.LineBasicMaterialParameters

    type [<AllowNullLiteral>] IExports =
        abstract LineDashedMaterial: LineDashedMaterialStatic

    type [<AllowNullLiteral>] LineDashedMaterialParameters =
        inherit LineBasicMaterialParameters
        abstract scale: float option with get, set
        abstract dashSize: float option with get, set
        abstract gapSize: float option with get, set

    type [<AllowNullLiteral>] LineDashedMaterial =
        inherit LineBasicMaterial
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'LineDashedMaterial'</default>
        abstract ``type``: string with get, set
        /// <default>1</default>
        abstract scale: float with get, set
        /// <default>1</default>
        abstract dashSize: float with get, set
        /// <default>1</default>
        abstract gapSize: float with get, set
        abstract isLineDashedMaterial: bool
        /// Sets the properties based on the values.
        abstract setValues: parameters: LineDashedMaterialParameters -> unit

    type [<AllowNullLiteral>] LineDashedMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: LineDashedMaterialParameters -> LineDashedMaterial

module __materials_Material =
    type Plane = __math_Plane.Plane
    type EventDispatcher = __core_EventDispatcher.EventDispatcher
    type WebGLRenderer = __renderers_WebGLRenderer.WebGLRenderer
    type Shader = __renderers_shaders_ShaderLib.Shader
    type BlendingDstFactor = Constants.BlendingDstFactor
    type BlendingEquation = Constants.BlendingEquation
    type Blending = Constants.Blending
    type BlendingSrcFactor = Constants.BlendingSrcFactor
    type DepthModes = Constants.DepthModes
    type Side = Constants.Side
    type StencilFunc = Constants.StencilFunc
    type StencilOp = Constants.StencilOp
    type ColorRepresentation = Utils.ColorRepresentation
    type Color = __math_Color.Color
    type Texture = __textures_Texture.Texture

    type [<AllowNullLiteral>] IExports =
        /// Materials describe the appearance of objects. They are defined in a (mostly) renderer-independent way, so you don't have to rewrite materials if you decide to use a different renderer.
        abstract Material: MaterialStatic

    type [<AllowNullLiteral>] MaterialParameters =
        abstract alphaTest: float option with get, set
        abstract alphaToCoverage: bool option with get, set
        abstract blendDst: BlendingDstFactor option with get, set
        abstract blendDstAlpha: float option with get, set
        abstract blendEquation: BlendingEquation option with get, set
        abstract blendEquationAlpha: float option with get, set
        abstract blending: Blending option with get, set
        abstract blendSrc: U2<BlendingSrcFactor, BlendingDstFactor> option with get, set
        abstract blendSrcAlpha: float option with get, set
        abstract clipIntersection: bool option with get, set
        abstract clippingPlanes: ResizeArray<Plane> option with get, set
        abstract clipShadows: bool option with get, set
        abstract colorWrite: bool option with get, set
        abstract defines: obj option with get, set
        abstract depthFunc: DepthModes option with get, set
        abstract depthTest: bool option with get, set
        abstract depthWrite: bool option with get, set
        abstract fog: bool option with get, set
        abstract name: string option with get, set
        abstract opacity: float option with get, set
        abstract polygonOffset: bool option with get, set
        abstract polygonOffsetFactor: float option with get, set
        abstract polygonOffsetUnits: float option with get, set
        abstract precision: MaterialParametersPrecision option with get, set
        abstract premultipliedAlpha: bool option with get, set
        abstract dithering: bool option with get, set
        abstract side: Side option with get, set
        abstract shadowSide: Side option with get, set
        abstract toneMapped: bool option with get, set
        abstract transparent: bool option with get, set
        abstract vertexColors: bool option with get, set
        abstract visible: bool option with get, set
        abstract stencilWrite: bool option with get, set
        abstract stencilFunc: StencilFunc option with get, set
        abstract stencilRef: float option with get, set
        abstract stencilWriteMask: float option with get, set
        abstract stencilFuncMask: float option with get, set
        abstract stencilFail: StencilOp option with get, set
        abstract stencilZFail: StencilOp option with get, set
        abstract stencilZPass: StencilOp option with get, set
        abstract userData: obj option with get, set

    /// Materials describe the appearance of objects. They are defined in a (mostly) renderer-independent way, so you don't have to rewrite materials if you decide to use a different renderer.
    type [<AllowNullLiteral>] Material =
        inherit EventDispatcher
        /// <summary>Sets the alpha value to be used when running an alpha test. Default is 0.</summary>
        /// <default>0</default>
        abstract alphaTest: float with get, set
        /// <summary>Enables alpha to coverage. Can only be used with MSAA-enabled rendering contexts.</summary>
        /// <default>false</default>
        abstract alphaToCoverage: bool with get, set
        /// <summary>Blending destination. It's one of the blending mode constants defined in Three.js. Default is <see cref="OneMinusSrcAlphaFactor" />.</summary>
        /// <default>THREE.OneMinusSrcAlphaFactor</default>
        abstract blendDst: BlendingDstFactor with get, set
        /// <summary>The tranparency of the .blendDst. Default is null.</summary>
        /// <default>null</default>
        abstract blendDstAlpha: float option with get, set
        /// <summary>Blending equation to use when applying blending. It's one of the constants defined in Three.js. Default is <see cref="AddEquation" />.</summary>
        /// <default>THREE.AddEquation</default>
        abstract blendEquation: BlendingEquation with get, set
        /// <summary>The tranparency of the .blendEquation. Default is null.</summary>
        /// <default>null</default>
        abstract blendEquationAlpha: float option with get, set
        /// <summary>Which blending to use when displaying objects with this material. Default is <see cref="NormalBlending" />.</summary>
        /// <default>THREE.NormalBlending</default>
        abstract blending: Blending with get, set
        /// <summary>Blending source. It's one of the blending mode constants defined in Three.js. Default is <see cref="SrcAlphaFactor" />.</summary>
        /// <default>THREE.SrcAlphaFactor</default>
        abstract blendSrc: U2<BlendingSrcFactor, BlendingDstFactor> with get, set
        /// <summary>The tranparency of the .blendSrc. Default is null.</summary>
        /// <default>null</default>
        abstract blendSrcAlpha: float option with get, set
        /// <summary>Changes the behavior of clipping planes so that only their intersection is clipped, rather than their union. Default is false.</summary>
        /// <default>false</default>
        abstract clipIntersection: bool with get, set
        /// <summary>
        /// User-defined clipping planes specified as THREE.Plane objects in world space.
        /// These planes apply to the objects this material is attached to.
        /// Points in space whose signed distance to the plane is negative are clipped (not rendered).
        /// See the WebGL / clipping /intersection example. Default is null.
        /// </summary>
        /// <default>null</default>
        abstract clippingPlanes: obj option with get, set
        /// <summary>Defines whether to clip shadows according to the clipping planes specified on this material. Default is false.</summary>
        /// <default>false</default>
        abstract clipShadows: bool with get, set
        /// <summary>Whether to render the material's color. This can be used in conjunction with a mesh's .renderOrder property to create invisible objects that occlude other objects. Default is true.</summary>
        /// <default>true</default>
        abstract colorWrite: bool with get, set
        /// <summary>
        /// Custom defines to be injected into the shader. These are passed in form of an object literal, with key/value pairs. { MY_CUSTOM_DEFINE: '' , PI2: Math.PI * 2 }.
        /// The pairs are defined in both vertex and fragment shaders. Default is undefined.
        /// </summary>
        /// <default>undefined</default>
        abstract defines: MaterialDefines option with get, set
        /// <summary>Which depth function to use. Default is <see cref="LessEqualDepth" />. See the depth mode constants for all possible values.</summary>
        /// <default>THREE.LessEqualDepth</default>
        abstract depthFunc: DepthModes with get, set
        /// <summary>Whether to have depth test enabled when rendering this material. Default is true.</summary>
        /// <default>true</default>
        abstract depthTest: bool with get, set
        /// <summary>
        /// Whether rendering this material has any effect on the depth buffer. Default is true.
        /// When drawing 2D overlays it can be useful to disable the depth writing in order to layer several things together without creating z-index artifacts.
        /// </summary>
        /// <default>true</default>
        abstract depthWrite: bool with get, set
        /// <summary>Whether the material is affected by fog. Default is true.</summary>
        /// <default>fog</default>
        abstract fog: bool with get, set
        /// Unique number of this material instance.
        abstract id: float with get, set
        /// <summary>Whether rendering this material has any effect on the stencil buffer. Default is *false*.</summary>
        /// <default>false</default>
        abstract stencilWrite: bool with get, set
        /// <summary>The stencil comparison function to use. Default is <see cref="AlwaysStencilFunc" />. See stencil operation constants for all possible values.</summary>
        /// <default>THREE.AlwaysStencilFunc</default>
        abstract stencilFunc: StencilFunc with get, set
        /// <summary>The value to use when performing stencil comparisons or stencil operations. Default is *0*.</summary>
        /// <default>0</default>
        abstract stencilRef: float with get, set
        /// <summary>The bit mask to use when writing to the stencil buffer. Default is *0xFF*.</summary>
        /// <default>0xff</default>
        abstract stencilWriteMask: float with get, set
        /// <summary>The bit mask to use when comparing against the stencil buffer. Default is *0xFF*.</summary>
        /// <default>0xff</default>
        abstract stencilFuncMask: float with get, set
        /// <summary>Which stencil operation to perform when the comparison function returns false. Default is <see cref="KeepStencilOp" />. See the stencil operation constants for all possible values.</summary>
        /// <default>THREE.KeepStencilOp</default>
        abstract stencilFail: StencilOp with get, set
        /// <summary>
        /// Which stencil operation to perform when the comparison function returns true but the depth test fails.
        /// Default is <see cref="KeepStencilOp" />.
        /// See the stencil operation constants for all possible values.
        /// </summary>
        /// <default>THREE.KeepStencilOp</default>
        abstract stencilZFail: StencilOp with get, set
        /// <summary>
        /// Which stencil operation to perform when the comparison function returns true and the depth test passes.
        /// Default is <see cref="KeepStencilOp" />.
        /// See the stencil operation constants for all possible values.
        /// </summary>
        /// <default>THREE.KeepStencilOp</default>
        abstract stencilZPass: StencilOp with get, set
        /// Used to check whether this or derived classes are materials. Default is true.
        /// You should not change this, as it used internally for optimisation.
        abstract isMaterial: bool
        /// <summary>Material name. Default is an empty string.</summary>
        /// <default>''</default>
        abstract name: string with get, set
        /// <summary>
        /// Specifies that the material needs to be updated, WebGL wise. Set it to true if you made changes that need to be reflected in WebGL.
        /// This property is automatically set to true when instancing a new material.
        /// </summary>
        /// <default>false</default>
        abstract needsUpdate: bool with get, set
        /// <summary>Opacity. Default is 1.</summary>
        /// <default>1</default>
        abstract opacity: float with get, set
        /// <summary>Whether to use polygon offset. Default is false. This corresponds to the POLYGON_OFFSET_FILL WebGL feature.</summary>
        /// <default>false</default>
        abstract polygonOffset: bool with get, set
        /// <summary>Sets the polygon offset factor. Default is 0.</summary>
        /// <default>0</default>
        abstract polygonOffsetFactor: float with get, set
        /// <summary>Sets the polygon offset units. Default is 0.</summary>
        /// <default>0</default>
        abstract polygonOffsetUnits: float with get, set
        /// <summary>Override the renderer's default precision for this material. Can be "highp", "mediump" or "lowp". Defaults is null.</summary>
        /// <default>null</default>
        abstract precision: MaterialParametersPrecision with get, set
        /// <summary>Whether to premultiply the alpha (transparency) value. See WebGL / Materials / Transparency for an example of the difference. Default is false.</summary>
        /// <default>false</default>
        abstract premultipliedAlpha: bool with get, set
        /// <summary>Whether to apply dithering to the color to remove the appearance of banding. Default is false.</summary>
        /// <default>false</default>
        abstract dithering: bool with get, set
        /// <summary>
        /// Defines which of the face sides will be rendered - front, back or both.
        /// Default is THREE.FrontSide. Other options are THREE.BackSide and THREE.DoubleSide.
        /// </summary>
        /// <default>THREE.FrontSide</default>
        abstract side: Side with get, set
        /// <summary>
        /// Defines which of the face sides will cast shadows. Default is *null*.
        /// If *null*, the value is opposite that of side, above.
        /// </summary>
        /// <default>null</default>
        abstract shadowSide: Side with get, set
        /// <summary>
        /// Defines whether this material is tone mapped according to the renderer's toneMapping setting.
        /// Default is true.
        /// </summary>
        /// <default>true</default>
        abstract toneMapped: bool with get, set
        /// <summary>
        /// Defines whether this material is transparent. This has an effect on rendering as transparent objects need special treatment and are rendered after non-transparent objects.
        /// When set to true, the extent to which the material is transparent is controlled by setting it's .opacity property.
        /// Default is false.
        /// </summary>
        /// <default>false</default>
        abstract transparent: bool with get, set
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'Material'</default>
        abstract ``type``: string with get, set
        /// UUID of this material instance. This gets automatically assigned, so this shouldn't be edited.
        abstract uuid: string with get, set
        /// <summary>Defines whether vertex coloring is used. Default is false.</summary>
        /// <default>false</default>
        abstract vertexColors: bool with get, set
        /// <summary>Defines whether this material is visible. Default is true.</summary>
        /// <default>true</default>
        abstract visible: bool with get, set
        /// <summary>An object that can be used to store custom data about the Material. It should not hold references to functions as these will not be cloned.</summary>
        /// <default>{}</default>
        abstract userData: obj option with get, set
        /// <summary>This starts at 0 and counts how many times .needsUpdate is set to true.</summary>
        /// <default>0</default>
        abstract version: float with get, set
        /// Return a new material with the same parameters as this material.
        abstract clone: unit -> Material
        /// <summary>Copy the parameters from the passed material into this material.</summary>
        /// <param name="material" />
        abstract copy: material: Material -> Material
        /// <summary>This disposes the material. Textures of a material don't get disposed. These needs to be disposed by <see cref="Texture" />.</summary>
        abstract dispose: unit -> unit
        /// <summary>
        /// An optional callback that is executed immediately before the shader program is compiled.
        /// This function is called with the shader source code as a parameter.
        /// Useful for the modification of built-in materials.
        /// </summary>
        /// <param name="shader">Source code of the shader</param>
        /// <param name="renderer">WebGLRenderer Context that is initializing the material</param>
        abstract onBeforeCompile: shader: Shader * renderer: WebGLRenderer -> unit
        /// In case onBeforeCompile is used, this callback can be used to identify values of settings used in onBeforeCompile, so three.js can reuse a cached shader or recompile the shader as needed.
        abstract customProgramCacheKey: unit -> string
        /// <summary>Sets the properties based on the values.</summary>
        /// <param name="values">A container with parameters.</param>
        abstract setValues: values: MaterialParameters -> unit
        /// <summary>Convert the material to three.js JSON format.</summary>
        /// <param name="meta">Object containing metadata such as textures or images for the material.</param>
        abstract toJSON: ?meta: obj -> obj option

    /// Materials describe the appearance of objects. They are defined in a (mostly) renderer-independent way, so you don't have to rewrite materials if you decide to use a different renderer.
    type [<AllowNullLiteral>] MaterialStatic =
        [<EmitConstructor>] abstract Create: unit -> Material

    type [<StringEnum>] [<RequireQualifiedAccess>] MaterialParametersPrecision =
        | Highp
        | Mediump
        | Lowp

    type [<AllowNullLiteral>] MaterialDefines =
        [<EmitIndexer>] abstract Item: key: string -> obj option with get, set

module __materials_MeshBasicMaterial =
    type Color = __math_Color.Color
    type Texture = __textures_Texture.Texture
    type MaterialParameters = __materials_Material.MaterialParameters
    type Material = __materials_Material.Material
    type Combine = Constants.Combine
    type ColorRepresentation = Utils.ColorRepresentation

    type [<AllowNullLiteral>] IExports =
        abstract MeshBasicMaterial: MeshBasicMaterialStatic

    /// parameters is an object with one or more properties defining the material's appearance.
    type [<AllowNullLiteral>] MeshBasicMaterialParameters =
        inherit MaterialParameters
        abstract color: ColorRepresentation option with get, set
        abstract opacity: float option with get, set
        abstract map: Texture option with get, set
        abstract lightMap: Texture option with get, set
        abstract lightMapIntensity: float option with get, set
        abstract aoMap: Texture option with get, set
        abstract aoMapIntensity: float option with get, set
        abstract specularMap: Texture option with get, set
        abstract alphaMap: Texture option with get, set
        abstract envMap: Texture option with get, set
        abstract combine: Combine option with get, set
        abstract reflectivity: float option with get, set
        abstract refractionRatio: float option with get, set
        abstract wireframe: bool option with get, set
        abstract wireframeLinewidth: float option with get, set
        abstract wireframeLinecap: string option with get, set
        abstract wireframeLinejoin: string option with get, set

    type [<AllowNullLiteral>] MeshBasicMaterial =
        inherit Material
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'MeshBasicMaterial'</default>
        abstract ``type``: string with get, set
        /// <default>new THREE.Color( 0xffffff )</default>
        abstract color: Color with get, set
        /// <default>null</default>
        abstract map: Texture option with get, set
        /// <default>null</default>
        abstract lightMap: Texture option with get, set
        /// <default>1</default>
        abstract lightMapIntensity: float with get, set
        /// <default>null</default>
        abstract aoMap: Texture option with get, set
        /// <default>1</default>
        abstract aoMapIntensity: float with get, set
        /// <default>null</default>
        abstract specularMap: Texture option with get, set
        /// <default>null</default>
        abstract alphaMap: Texture option with get, set
        /// <default>null</default>
        abstract envMap: Texture option with get, set
        /// <default>THREE.MultiplyOperation</default>
        abstract combine: Combine with get, set
        /// <default>1</default>
        abstract reflectivity: float with get, set
        /// <default>0.98</default>
        abstract refractionRatio: float with get, set
        /// <default>false</default>
        abstract wireframe: bool with get, set
        /// <default>1</default>
        abstract wireframeLinewidth: float with get, set
        /// <default>'round'</default>
        abstract wireframeLinecap: string with get, set
        /// <default>'round'</default>
        abstract wireframeLinejoin: string with get, set
        /// Sets the properties based on the values.
        abstract setValues: parameters: MeshBasicMaterialParameters -> unit

    type [<AllowNullLiteral>] MeshBasicMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: MeshBasicMaterialParameters -> MeshBasicMaterial

module __materials_MeshDepthMaterial =
    type DepthPackingStrategies = Constants.DepthPackingStrategies
    type MaterialParameters = __materials_Material.MaterialParameters
    type Material = __materials_Material.Material
    type Texture = __textures_Texture.Texture

    type [<AllowNullLiteral>] IExports =
        abstract MeshDepthMaterial: MeshDepthMaterialStatic

    type [<AllowNullLiteral>] MeshDepthMaterialParameters =
        inherit MaterialParameters
        abstract map: Texture option with get, set
        abstract alphaMap: Texture option with get, set
        abstract depthPacking: DepthPackingStrategies option with get, set
        abstract displacementMap: Texture option with get, set
        abstract displacementScale: float option with get, set
        abstract displacementBias: float option with get, set
        abstract wireframe: bool option with get, set
        abstract wireframeLinewidth: float option with get, set

    type [<AllowNullLiteral>] MeshDepthMaterial =
        inherit Material
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'MeshDepthMaterial'</default>
        abstract ``type``: string with get, set
        /// <default>null</default>
        abstract map: Texture option with get, set
        /// <default>null</default>
        abstract alphaMap: Texture option with get, set
        /// <default>THREE.BasicDepthPacking</default>
        abstract depthPacking: DepthPackingStrategies with get, set
        /// <default>null</default>
        abstract displacementMap: Texture option with get, set
        /// <default>1</default>
        abstract displacementScale: float with get, set
        /// <default>0</default>
        abstract displacementBias: float with get, set
        /// <default>false</default>
        abstract wireframe: bool with get, set
        /// <default>1</default>
        abstract wireframeLinewidth: float with get, set
        /// <summary>Whether the material is affected by fog. Default is true.</summary>
        /// <default>false</default>
        abstract fog: bool with get, set
        /// Sets the properties based on the values.
        abstract setValues: parameters: MeshDepthMaterialParameters -> unit

    type [<AllowNullLiteral>] MeshDepthMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: MeshDepthMaterialParameters -> MeshDepthMaterial

module __materials_MeshDistanceMaterial =
    type MaterialParameters = __materials_Material.MaterialParameters
    type Material = __materials_Material.Material
    type Vector3 = __math_Vector3.Vector3
    type Texture = __textures_Texture.Texture

    type [<AllowNullLiteral>] IExports =
        abstract MeshDistanceMaterial: MeshDistanceMaterialStatic

    type [<AllowNullLiteral>] MeshDistanceMaterialParameters =
        inherit MaterialParameters
        abstract map: Texture option with get, set
        abstract alphaMap: Texture option with get, set
        abstract displacementMap: Texture option with get, set
        abstract displacementScale: float option with get, set
        abstract displacementBias: float option with get, set
        abstract farDistance: float option with get, set
        abstract nearDistance: float option with get, set
        abstract referencePosition: Vector3 option with get, set

    type [<AllowNullLiteral>] MeshDistanceMaterial =
        inherit Material
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'MeshDistanceMaterial'</default>
        abstract ``type``: string with get, set
        /// <default>null</default>
        abstract map: Texture option with get, set
        /// <default>null</default>
        abstract alphaMap: Texture option with get, set
        /// <default>null</default>
        abstract displacementMap: Texture option with get, set
        /// <default>1</default>
        abstract displacementScale: float with get, set
        /// <default>0</default>
        abstract displacementBias: float with get, set
        /// <default>1000</default>
        abstract farDistance: float with get, set
        /// <default>1</default>
        abstract nearDistance: float with get, set
        /// <default>new THREE.Vector3()</default>
        abstract referencePosition: Vector3 with get, set
        /// <summary>Whether the material is affected by fog. Default is true.</summary>
        /// <default>false</default>
        abstract fog: bool with get, set
        /// Sets the properties based on the values.
        abstract setValues: parameters: MeshDistanceMaterialParameters -> unit

    type [<AllowNullLiteral>] MeshDistanceMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: MeshDistanceMaterialParameters -> MeshDistanceMaterial

module __materials_MeshLambertMaterial =
    type Color = __math_Color.Color
    type Texture = __textures_Texture.Texture
    type MaterialParameters = __materials_Material.MaterialParameters
    type Material = __materials_Material.Material
    type Combine = Constants.Combine
    type ColorRepresentation = Utils.ColorRepresentation

    type [<AllowNullLiteral>] IExports =
        abstract MeshLambertMaterial: MeshLambertMaterialStatic

    type [<AllowNullLiteral>] MeshLambertMaterialParameters =
        inherit MaterialParameters
        abstract color: ColorRepresentation option with get, set
        abstract emissive: ColorRepresentation option with get, set
        abstract emissiveIntensity: float option with get, set
        abstract emissiveMap: Texture option with get, set
        abstract map: Texture option with get, set
        abstract lightMap: Texture option with get, set
        abstract lightMapIntensity: float option with get, set
        abstract aoMap: Texture option with get, set
        abstract aoMapIntensity: float option with get, set
        abstract specularMap: Texture option with get, set
        abstract alphaMap: Texture option with get, set
        abstract envMap: Texture option with get, set
        abstract combine: Combine option with get, set
        abstract reflectivity: float option with get, set
        abstract refractionRatio: float option with get, set
        abstract wireframe: bool option with get, set
        abstract wireframeLinewidth: float option with get, set
        abstract wireframeLinecap: string option with get, set
        abstract wireframeLinejoin: string option with get, set

    type [<AllowNullLiteral>] MeshLambertMaterial =
        inherit Material
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'MeshLambertMaterial'</default>
        abstract ``type``: string with get, set
        /// <default>new THREE.Color( 0xffffff )</default>
        abstract color: Color with get, set
        /// <default>new THREE.Color( 0x000000 )</default>
        abstract emissive: Color with get, set
        /// <default>1</default>
        abstract emissiveIntensity: float with get, set
        /// <default>null</default>
        abstract emissiveMap: Texture option with get, set
        /// <default>null</default>
        abstract map: Texture option with get, set
        /// <default>null</default>
        abstract lightMap: Texture option with get, set
        /// <default>1</default>
        abstract lightMapIntensity: float with get, set
        /// <default>null</default>
        abstract aoMap: Texture option with get, set
        /// <default>1</default>
        abstract aoMapIntensity: float with get, set
        /// <default>null</default>
        abstract specularMap: Texture option with get, set
        /// <default>null</default>
        abstract alphaMap: Texture option with get, set
        /// <default>null</default>
        abstract envMap: Texture option with get, set
        /// <default>THREE.MultiplyOperation</default>
        abstract combine: Combine with get, set
        /// <default>1</default>
        abstract reflectivity: float with get, set
        /// <default>0.98</default>
        abstract refractionRatio: float with get, set
        /// <default>false</default>
        abstract wireframe: bool with get, set
        /// <default>1</default>
        abstract wireframeLinewidth: float with get, set
        /// <default>'round'</default>
        abstract wireframeLinecap: string with get, set
        /// <default>'round'</default>
        abstract wireframeLinejoin: string with get, set
        /// Sets the properties based on the values.
        abstract setValues: parameters: MeshLambertMaterialParameters -> unit

    type [<AllowNullLiteral>] MeshLambertMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: MeshLambertMaterialParameters -> MeshLambertMaterial

module __materials_MeshMatcapMaterial =
    type Color = __math_Color.Color
    type Texture = __textures_Texture.Texture
    type Vector2 = __math_Vector2.Vector2
    type MaterialParameters = __materials_Material.MaterialParameters
    type Material = __materials_Material.Material
    type NormalMapTypes = Constants.NormalMapTypes
    type ColorRepresentation = Utils.ColorRepresentation

    type [<AllowNullLiteral>] IExports =
        abstract MeshMatcapMaterial: MeshMatcapMaterialStatic

    type [<AllowNullLiteral>] MeshMatcapMaterialParameters =
        inherit MaterialParameters
        abstract color: ColorRepresentation option with get, set
        abstract matcap: Texture option with get, set
        abstract map: Texture option with get, set
        abstract bumpMap: Texture option with get, set
        abstract bumpScale: float option with get, set
        abstract normalMap: Texture option with get, set
        abstract normalMapType: NormalMapTypes option with get, set
        abstract normalScale: Vector2 option with get, set
        abstract displacementMap: Texture option with get, set
        abstract displacementScale: float option with get, set
        abstract displacementBias: float option with get, set
        abstract alphaMap: Texture option with get, set
        abstract flatShading: bool option with get, set

    type [<AllowNullLiteral>] MeshMatcapMaterial =
        inherit Material
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'MeshMatcapMaterial'</default>
        abstract ``type``: string with get, set
        /// <summary>
        /// Custom defines to be injected into the shader. These are passed in form of an object literal, with key/value pairs. { MY_CUSTOM_DEFINE: '' , PI2: Math.PI * 2 }.
        /// The pairs are defined in both vertex and fragment shaders. Default is undefined.
        /// </summary>
        /// <default>{ 'MATCAP': '' }</default>
        abstract defines: MeshMatcapMaterialDefines with get, set
        /// <default>new THREE.Color( 0xffffff )</default>
        abstract color: Color with get, set
        /// <default>null</default>
        abstract matcap: Texture option with get, set
        /// <default>null</default>
        abstract map: Texture option with get, set
        /// <default>null</default>
        abstract bumpMap: Texture option with get, set
        /// <default>1</default>
        abstract bumpScale: float with get, set
        /// <default>null</default>
        abstract normalMap: Texture option with get, set
        /// <default>THREE.TangentSpaceNormalMap</default>
        abstract normalMapType: NormalMapTypes with get, set
        /// <default>new Vector2( 1, 1 )</default>
        abstract normalScale: Vector2 with get, set
        /// <default>null</default>
        abstract displacementMap: Texture option with get, set
        /// <default>1</default>
        abstract displacementScale: float with get, set
        /// <default>0</default>
        abstract displacementBias: float with get, set
        /// <default>null</default>
        abstract alphaMap: Texture option with get, set
        /// <summary>Define whether the material is rendered with flat shading. Default is false.</summary>
        /// <default>false</default>
        abstract flatShading: bool with get, set
        /// Sets the properties based on the values.
        abstract setValues: parameters: MeshMatcapMaterialParameters -> unit

    type [<AllowNullLiteral>] MeshMatcapMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: MeshMatcapMaterialParameters -> MeshMatcapMaterial

    type [<AllowNullLiteral>] MeshMatcapMaterialDefines =
        [<EmitIndexer>] abstract Item: key: string -> obj option with get, set

module __materials_MeshNormalMaterial =
    type MaterialParameters = __materials_Material.MaterialParameters
    type Material = __materials_Material.Material
    type Texture = __textures_Texture.Texture
    type Vector2 = __math_Vector2.Vector2
    type NormalMapTypes = Constants.NormalMapTypes

    type [<AllowNullLiteral>] IExports =
        abstract MeshNormalMaterial: MeshNormalMaterialStatic

    type [<AllowNullLiteral>] MeshNormalMaterialParameters =
        inherit MaterialParameters
        abstract bumpMap: Texture option with get, set
        abstract bumpScale: float option with get, set
        abstract normalMap: Texture option with get, set
        abstract normalMapType: NormalMapTypes option with get, set
        abstract normalScale: Vector2 option with get, set
        abstract displacementMap: Texture option with get, set
        abstract displacementScale: float option with get, set
        abstract displacementBias: float option with get, set
        abstract wireframe: bool option with get, set
        abstract wireframeLinewidth: float option with get, set
        abstract flatShading: bool option with get, set

    type [<AllowNullLiteral>] MeshNormalMaterial =
        inherit Material
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'MeshNormalMaterial'</default>
        abstract ``type``: string with get, set
        /// <default>null</default>
        abstract bumpMap: Texture option with get, set
        /// <default>1</default>
        abstract bumpScale: float with get, set
        /// <default>null</default>
        abstract normalMap: Texture option with get, set
        /// <default>THREE.TangentSpaceNormalMap</default>
        abstract normalMapType: NormalMapTypes with get, set
        /// <default>new THREE.Vector2( 1, 1 )</default>
        abstract normalScale: Vector2 with get, set
        /// <default>null</default>
        abstract displacementMap: Texture option with get, set
        /// <default>1</default>
        abstract displacementScale: float with get, set
        /// <default>0</default>
        abstract displacementBias: float with get, set
        /// <default>false</default>
        abstract wireframe: bool with get, set
        /// <default>1</default>
        abstract wireframeLinewidth: float with get, set
        /// <summary>Define whether the material is rendered with flat shading. Default is false.</summary>
        /// <default>false</default>
        abstract flatShading: bool with get, set
        /// Sets the properties based on the values.
        abstract setValues: parameters: MeshNormalMaterialParameters -> unit

    type [<AllowNullLiteral>] MeshNormalMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: MeshNormalMaterialParameters -> MeshNormalMaterial

module __materials_MeshPhongMaterial =
    type Color = __math_Color.Color
    type Texture = __textures_Texture.Texture
    type Vector2 = __math_Vector2.Vector2
    type MaterialParameters = __materials_Material.MaterialParameters
    type Material = __materials_Material.Material
    type Combine = Constants.Combine
    type NormalMapTypes = Constants.NormalMapTypes
    type ColorRepresentation = Utils.ColorRepresentation

    type [<AllowNullLiteral>] IExports =
        abstract MeshPhongMaterial: MeshPhongMaterialStatic

    type [<AllowNullLiteral>] MeshPhongMaterialParameters =
        inherit MaterialParameters
        /// geometry color in hexadecimal. Default is 0xffffff.
        abstract color: ColorRepresentation option with get, set
        abstract specular: ColorRepresentation option with get, set
        abstract shininess: float option with get, set
        abstract opacity: float option with get, set
        abstract map: Texture option with get, set
        abstract lightMap: Texture option with get, set
        abstract lightMapIntensity: float option with get, set
        abstract aoMap: Texture option with get, set
        abstract aoMapIntensity: float option with get, set
        abstract emissive: ColorRepresentation option with get, set
        abstract emissiveIntensity: float option with get, set
        abstract emissiveMap: Texture option with get, set
        abstract bumpMap: Texture option with get, set
        abstract bumpScale: float option with get, set
        abstract normalMap: Texture option with get, set
        abstract normalMapType: NormalMapTypes option with get, set
        abstract normalScale: Vector2 option with get, set
        abstract displacementMap: Texture option with get, set
        abstract displacementScale: float option with get, set
        abstract displacementBias: float option with get, set
        abstract specularMap: Texture option with get, set
        abstract alphaMap: Texture option with get, set
        abstract envMap: Texture option with get, set
        abstract combine: Combine option with get, set
        abstract reflectivity: float option with get, set
        abstract refractionRatio: float option with get, set
        abstract wireframe: bool option with get, set
        abstract wireframeLinewidth: float option with get, set
        abstract wireframeLinecap: string option with get, set
        abstract wireframeLinejoin: string option with get, set
        abstract flatShading: bool option with get, set

    type [<AllowNullLiteral>] MeshPhongMaterial =
        inherit Material
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'MeshNormalMaterial'</default>
        abstract ``type``: string with get, set
        /// <default>new THREE.Color( 0xffffff )</default>
        abstract color: Color with get, set
        /// <default>new THREE.Color( 0x111111 )</default>
        abstract specular: Color with get, set
        /// <default>30</default>
        abstract shininess: float with get, set
        /// <default>null</default>
        abstract map: Texture option with get, set
        /// <default>null</default>
        abstract lightMap: Texture option with get, set
        /// <default>null</default>
        abstract lightMapIntensity: float with get, set
        /// <default>null</default>
        abstract aoMap: Texture option with get, set
        /// <default>null</default>
        abstract aoMapIntensity: float with get, set
        /// <default>new THREE.Color( 0x000000 )</default>
        abstract emissive: Color with get, set
        /// <default>1</default>
        abstract emissiveIntensity: float with get, set
        /// <default>null</default>
        abstract emissiveMap: Texture option with get, set
        /// <default>null</default>
        abstract bumpMap: Texture option with get, set
        /// <default>1</default>
        abstract bumpScale: float with get, set
        /// <default>null</default>
        abstract normalMap: Texture option with get, set
        /// <default>THREE.TangentSpaceNormalMap</default>
        abstract normalMapType: NormalMapTypes with get, set
        /// <default>new Vector2( 1, 1 )</default>
        abstract normalScale: Vector2 with get, set
        /// <default>null</default>
        abstract displacementMap: Texture option with get, set
        /// <default>1</default>
        abstract displacementScale: float with get, set
        /// <default>0</default>
        abstract displacementBias: float with get, set
        /// <default>null</default>
        abstract specularMap: Texture option with get, set
        /// <default>null</default>
        abstract alphaMap: Texture option with get, set
        /// <default>null</default>
        abstract envMap: Texture option with get, set
        /// <default>THREE.MultiplyOperation</default>
        abstract combine: Combine with get, set
        /// <default>1</default>
        abstract reflectivity: float with get, set
        /// <default>0.98</default>
        abstract refractionRatio: float with get, set
        /// <default>false</default>
        abstract wireframe: bool with get, set
        /// <default>1</default>
        abstract wireframeLinewidth: float with get, set
        /// <default>'round'</default>
        abstract wireframeLinecap: string with get, set
        /// <default>'round'</default>
        abstract wireframeLinejoin: string with get, set
        /// <summary>Define whether the material is rendered with flat shading. Default is false.</summary>
        /// <default>false</default>
        abstract flatShading: bool with get, set
        [<Obsolete("Use {@link MeshStandardMaterial THREE.MeshStandardMaterial} instead.")>]
        abstract metal: bool with get, set
        /// Sets the properties based on the values.
        abstract setValues: parameters: MeshPhongMaterialParameters -> unit

    type [<AllowNullLiteral>] MeshPhongMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: MeshPhongMaterialParameters -> MeshPhongMaterial

module __materials_MeshPhysicalMaterial =
    type Texture = __textures_Texture.Texture
    type Vector2 = __math_Vector2.Vector2
    type MeshStandardMaterialParameters = __materials_MeshStandardMaterial.MeshStandardMaterialParameters
    type MeshStandardMaterial = __materials_MeshStandardMaterial.MeshStandardMaterial
    type Color = __math_Color.Color

    type [<AllowNullLiteral>] IExports =
        abstract MeshPhysicalMaterial: MeshPhysicalMaterialStatic

    type [<AllowNullLiteral>] MeshPhysicalMaterialParameters =
        inherit MeshStandardMaterialParameters
        abstract clearcoat: float option with get, set
        abstract clearcoatMap: Texture option with get, set
        abstract clearcoatRoughness: float option with get, set
        abstract clearcoatRoughnessMap: Texture option with get, set
        abstract clearcoatNormalScale: Vector2 option with get, set
        abstract clearcoatNormalMap: Texture option with get, set
        abstract reflectivity: float option with get, set
        abstract ior: float option with get, set
        abstract sheen: Color option with get, set
        abstract transmission: float option with get, set
        abstract transmissionMap: Texture option with get, set
        abstract attenuationDistance: float option with get, set
        abstract attenuationTint: Color option with get, set
        abstract specularIntensity: float option with get, set
        abstract specularTint: Color option with get, set
        abstract specularIntensityMap: Texture option with get, set
        abstract specularTintMap: Texture option with get, set

    type [<AllowNullLiteral>] MeshPhysicalMaterial =
        inherit MeshStandardMaterial
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'MeshPhysicalMaterial'</default>
        abstract ``type``: string with get, set
        /// <summary>
        /// Custom defines to be injected into the shader. These are passed in form of an object literal, with key/value pairs. { MY_CUSTOM_DEFINE: '' , PI2: Math.PI * 2 }.
        /// The pairs are defined in both vertex and fragment shaders. Default is undefined.
        /// </summary>
        /// <default>{ 'STANDARD': '', 'PHYSICAL': '' }</default>
        abstract defines: MeshPhysicalMaterialDefines with get, set
        /// <default>0</default>
        abstract clearcoat: float with get, set
        /// <default>null</default>
        abstract clearcoatMap: Texture option with get, set
        /// <default>0</default>
        abstract clearcoatRoughness: float with get, set
        /// <default>null</default>
        abstract clearcoatRoughnessMap: Texture option with get, set
        /// <default>new THREE.Vector2( 1, 1 )</default>
        abstract clearcoatNormalScale: Vector2 with get, set
        /// <default>null</default>
        abstract clearcoatNormalMap: Texture option with get, set
        /// <default>0.5</default>
        abstract reflectivity: float with get, set
        /// <default>1.5</default>
        abstract ior: float with get, set
        /// <default>null</default>
        abstract sheen: Color option with get, set
        /// <default>0</default>
        abstract transmission: float with get, set
        /// <default>null</default>
        abstract transmissionMap: Texture option with get, set
        /// <default>0.01</default>
        abstract thickness: float with get, set
        /// <default>null</default>
        abstract thicknessMap: Texture option with get, set
        /// <default>0.0</default>
        abstract attenuationDistance: float with get, set
        /// <default>Color( 1, 1, 1 )</default>
        abstract attenuationColor: Color with get, set
        /// <default>1.0</default>
        abstract specularIntensity: float with get, set
        /// <default>Color(1, 1, 1)</default>
        abstract specularTint: Color with get, set
        /// <default>null</default>
        abstract specularIntensityMap: Texture option with get, set
        /// <default>null</default>
        abstract specularTintMap: Texture option with get, set

    type [<AllowNullLiteral>] MeshPhysicalMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: MeshPhysicalMaterialParameters -> MeshPhysicalMaterial

    type [<AllowNullLiteral>] MeshPhysicalMaterialDefines =
        [<EmitIndexer>] abstract Item: key: string -> obj option with get, set

module __materials_MeshStandardMaterial =
    type Color = __math_Color.Color
    type Texture = __textures_Texture.Texture
    type Vector2 = __math_Vector2.Vector2
    type MaterialParameters = __materials_Material.MaterialParameters
    type Material = __materials_Material.Material
    type NormalMapTypes = Constants.NormalMapTypes
    type ColorRepresentation = Utils.ColorRepresentation

    type [<AllowNullLiteral>] IExports =
        abstract MeshStandardMaterial: MeshStandardMaterialStatic

    type [<AllowNullLiteral>] MeshStandardMaterialParameters =
        inherit MaterialParameters
        abstract color: ColorRepresentation option with get, set
        abstract roughness: float option with get, set
        abstract metalness: float option with get, set
        abstract map: Texture option with get, set
        abstract lightMap: Texture option with get, set
        abstract lightMapIntensity: float option with get, set
        abstract aoMap: Texture option with get, set
        abstract aoMapIntensity: float option with get, set
        abstract emissive: ColorRepresentation option with get, set
        abstract emissiveIntensity: float option with get, set
        abstract emissiveMap: Texture option with get, set
        abstract bumpMap: Texture option with get, set
        abstract bumpScale: float option with get, set
        abstract normalMap: Texture option with get, set
        abstract normalMapType: NormalMapTypes option with get, set
        abstract normalScale: Vector2 option with get, set
        abstract displacementMap: Texture option with get, set
        abstract displacementScale: float option with get, set
        abstract displacementBias: float option with get, set
        abstract roughnessMap: Texture option with get, set
        abstract metalnessMap: Texture option with get, set
        abstract alphaMap: Texture option with get, set
        abstract envMap: Texture option with get, set
        abstract envMapIntensity: float option with get, set
        abstract refractionRatio: float option with get, set
        abstract wireframe: bool option with get, set
        abstract wireframeLinewidth: float option with get, set
        abstract flatShading: bool option with get, set

    type [<AllowNullLiteral>] MeshStandardMaterial =
        inherit Material
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'MeshStandardMaterial'</default>
        abstract ``type``: string with get, set
        /// <summary>
        /// Custom defines to be injected into the shader. These are passed in form of an object literal, with key/value pairs. { MY_CUSTOM_DEFINE: '' , PI2: Math.PI * 2 }.
        /// The pairs are defined in both vertex and fragment shaders. Default is undefined.
        /// </summary>
        /// <default>{ 'STANDARD': '' }</default>
        abstract defines: MeshStandardMaterialDefines with get, set
        /// <default>new THREE.Color( 0xffffff )</default>
        abstract color: Color with get, set
        /// <default>1</default>
        abstract roughness: float with get, set
        /// <default>0</default>
        abstract metalness: float with get, set
        /// <default>null</default>
        abstract map: Texture option with get, set
        /// <default>null</default>
        abstract lightMap: Texture option with get, set
        /// <default>1</default>
        abstract lightMapIntensity: float with get, set
        /// <default>null</default>
        abstract aoMap: Texture option with get, set
        /// <default>1</default>
        abstract aoMapIntensity: float with get, set
        /// <default>new THREE.Color( 0x000000 )</default>
        abstract emissive: Color with get, set
        /// <default>1</default>
        abstract emissiveIntensity: float with get, set
        /// <default>null</default>
        abstract emissiveMap: Texture option with get, set
        /// <default>null</default>
        abstract bumpMap: Texture option with get, set
        /// <default>1</default>
        abstract bumpScale: float with get, set
        /// <default>null</default>
        abstract normalMap: Texture option with get, set
        /// <default>THREE.TangentSpaceNormalMap</default>
        abstract normalMapType: NormalMapTypes with get, set
        /// <default>new THREE.Vector2( 1, 1 )</default>
        abstract normalScale: Vector2 with get, set
        /// <default>null</default>
        abstract displacementMap: Texture option with get, set
        /// <default>1</default>
        abstract displacementScale: float with get, set
        /// <default>0</default>
        abstract displacementBias: float with get, set
        /// <default>null</default>
        abstract roughnessMap: Texture option with get, set
        /// <default>null</default>
        abstract metalnessMap: Texture option with get, set
        /// <default>null</default>
        abstract alphaMap: Texture option with get, set
        /// <default>null</default>
        abstract envMap: Texture option with get, set
        /// <default>1</default>
        abstract envMapIntensity: float with get, set
        /// <default>0.98</default>
        abstract refractionRatio: float with get, set
        /// <default>false</default>
        abstract wireframe: bool with get, set
        /// <default>1</default>
        abstract wireframeLinewidth: float with get, set
        /// <default>'round'</default>
        abstract wireframeLinecap: string with get, set
        /// <default>'round'</default>
        abstract wireframeLinejoin: string with get, set
        /// <summary>Define whether the material is rendered with flat shading. Default is false.</summary>
        /// <default>false</default>
        abstract flatShading: bool with get, set
        abstract isMeshStandardMaterial: bool with get, set
        /// Sets the properties based on the values.
        abstract setValues: parameters: MeshStandardMaterialParameters -> unit

    type [<AllowNullLiteral>] MeshStandardMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: MeshStandardMaterialParameters -> MeshStandardMaterial

    type [<AllowNullLiteral>] MeshStandardMaterialDefines =
        [<EmitIndexer>] abstract Item: key: string -> obj option with get, set

module __materials_MeshToonMaterial =
    type Color = __math_Color.Color
    type Texture = __textures_Texture.Texture
    type Vector2 = __math_Vector2.Vector2
    type MaterialParameters = __materials_Material.MaterialParameters
    type Material = __materials_Material.Material
    type NormalMapTypes = Constants.NormalMapTypes
    type ColorRepresentation = Utils.ColorRepresentation

    type [<AllowNullLiteral>] IExports =
        abstract MeshToonMaterial: MeshToonMaterialStatic

    type [<AllowNullLiteral>] MeshToonMaterialParameters =
        inherit MaterialParameters
        /// geometry color in hexadecimal. Default is 0xffffff.
        abstract color: ColorRepresentation option with get, set
        abstract opacity: float option with get, set
        abstract gradientMap: Texture option with get, set
        abstract map: Texture option with get, set
        abstract lightMap: Texture option with get, set
        abstract lightMapIntensity: float option with get, set
        abstract aoMap: Texture option with get, set
        abstract aoMapIntensity: float option with get, set
        abstract emissive: ColorRepresentation option with get, set
        abstract emissiveIntensity: float option with get, set
        abstract emissiveMap: Texture option with get, set
        abstract bumpMap: Texture option with get, set
        abstract bumpScale: float option with get, set
        abstract normalMap: Texture option with get, set
        abstract normalMapType: NormalMapTypes option with get, set
        abstract normalScale: Vector2 option with get, set
        abstract displacementMap: Texture option with get, set
        abstract displacementScale: float option with get, set
        abstract displacementBias: float option with get, set
        abstract alphaMap: Texture option with get, set
        abstract wireframe: bool option with get, set
        abstract wireframeLinewidth: float option with get, set
        abstract wireframeLinecap: string option with get, set
        abstract wireframeLinejoin: string option with get, set

    type [<AllowNullLiteral>] MeshToonMaterial =
        inherit Material
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'MeshToonMaterial'</default>
        abstract ``type``: string with get, set
        /// <summary>
        /// Custom defines to be injected into the shader. These are passed in form of an object literal, with key/value pairs. { MY_CUSTOM_DEFINE: '' , PI2: Math.PI * 2 }.
        /// The pairs are defined in both vertex and fragment shaders. Default is undefined.
        /// </summary>
        /// <default>{ 'TOON': '' }</default>
        abstract defines: MeshToonMaterialDefines with get, set
        /// <default>new THREE.Color( 0xffffff )</default>
        abstract color: Color with get, set
        /// <default>null</default>
        abstract gradientMap: Texture option with get, set
        /// <default>null</default>
        abstract map: Texture option with get, set
        /// <default>null</default>
        abstract lightMap: Texture option with get, set
        /// <default>1</default>
        abstract lightMapIntensity: float with get, set
        /// <default>null</default>
        abstract aoMap: Texture option with get, set
        /// <default>1</default>
        abstract aoMapIntensity: float with get, set
        /// <default>new THREE.Color( 0x000000 )</default>
        abstract emissive: Color with get, set
        /// <default>1</default>
        abstract emissiveIntensity: float with get, set
        /// <default>null</default>
        abstract emissiveMap: Texture option with get, set
        /// <default>null</default>
        abstract bumpMap: Texture option with get, set
        /// <default>1</default>
        abstract bumpScale: float with get, set
        /// <default>null</default>
        abstract normalMap: Texture option with get, set
        /// <default>THREE.TangentSpaceNormalMap</default>
        abstract normalMapType: NormalMapTypes with get, set
        /// <default>new THREE.Vector2( 1, 1 )</default>
        abstract normalScale: Vector2 with get, set
        /// <default>null</default>
        abstract displacementMap: Texture option with get, set
        /// <default>1</default>
        abstract displacementScale: float with get, set
        /// <default>0</default>
        abstract displacementBias: float with get, set
        /// <default>null</default>
        abstract alphaMap: Texture option with get, set
        /// <default>false</default>
        abstract wireframe: bool with get, set
        /// <default>1</default>
        abstract wireframeLinewidth: float with get, set
        /// <default>'round'</default>
        abstract wireframeLinecap: string with get, set
        /// <default>'round'</default>
        abstract wireframeLinejoin: string with get, set
        /// Sets the properties based on the values.
        abstract setValues: parameters: MeshToonMaterialParameters -> unit

    type [<AllowNullLiteral>] MeshToonMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: MeshToonMaterialParameters -> MeshToonMaterial

    type [<AllowNullLiteral>] MeshToonMaterialDefines =
        [<EmitIndexer>] abstract Item: key: string -> obj option with get, set

module __materials_PointsMaterial =
    type Material = __materials_Material.Material
    type MaterialParameters = __materials_Material.MaterialParameters
    type Color = __math_Color.Color
    type Texture = __textures_Texture.Texture
    type ColorRepresentation = Utils.ColorRepresentation

    type [<AllowNullLiteral>] IExports =
        abstract PointsMaterial: PointsMaterialStatic

    type [<AllowNullLiteral>] PointsMaterialParameters =
        inherit MaterialParameters
        abstract color: ColorRepresentation option with get, set
        abstract map: Texture option with get, set
        abstract alphaMap: Texture option with get, set
        abstract size: float option with get, set
        abstract sizeAttenuation: bool option with get, set

    type [<AllowNullLiteral>] PointsMaterial =
        inherit Material
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'PointsMaterial'</default>
        abstract ``type``: string with get, set
        /// <default>new THREE.Color( 0xffffff )</default>
        abstract color: Color with get, set
        /// <default>null</default>
        abstract map: Texture option with get, set
        /// <default>null</default>
        abstract alphaMap: Texture option with get, set
        /// <default>1</default>
        abstract size: float with get, set
        /// <default>true</default>
        abstract sizeAttenuation: bool with get, set
        /// Sets the properties based on the values.
        abstract setValues: parameters: PointsMaterialParameters -> unit

    type [<AllowNullLiteral>] PointsMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: PointsMaterialParameters -> PointsMaterial

module __materials_RawShaderMaterial =
    type ShaderMaterialParameters = __materials_ShaderMaterial.ShaderMaterialParameters
    type ShaderMaterial = __materials_ShaderMaterial.ShaderMaterial

    type [<AllowNullLiteral>] IExports =
        abstract RawShaderMaterial: RawShaderMaterialStatic

    type [<AllowNullLiteral>] RawShaderMaterial =
        inherit ShaderMaterial

    type [<AllowNullLiteral>] RawShaderMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: ShaderMaterialParameters -> RawShaderMaterial

module __materials_ShaderMaterial =
    type MaterialParameters = __materials_Material.MaterialParameters
    type Material = __materials_Material.Material
    type GLSLVersion = Constants.GLSLVersion

    type [<AllowNullLiteral>] IExports =
        abstract ShaderMaterial: ShaderMaterialStatic

    type [<AllowNullLiteral>] ShaderMaterialParameters =
        inherit MaterialParameters
        abstract uniforms: ShaderMaterialParametersUniforms option with get, set
        abstract vertexShader: string option with get, set
        abstract fragmentShader: string option with get, set
        abstract linewidth: float option with get, set
        abstract wireframe: bool option with get, set
        abstract wireframeLinewidth: float option with get, set
        abstract lights: bool option with get, set
        abstract clipping: bool option with get, set
        abstract extensions: {| derivatives: bool option; fragDepth: bool option; drawBuffers: bool option; shaderTextureLOD: bool option |} option with get, set
        abstract glslVersion: GLSLVersion option with get, set

    type [<AllowNullLiteral>] ShaderMaterial =
        inherit Material
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'ShaderMaterial'</default>
        abstract ``type``: string with get, set
        /// <summary>
        /// Custom defines to be injected into the shader. These are passed in form of an object literal, with key/value pairs. { MY_CUSTOM_DEFINE: '' , PI2: Math.PI * 2 }.
        /// The pairs are defined in both vertex and fragment shaders. Default is undefined.
        /// </summary>
        /// <default>{}</default>
        abstract defines: ShaderMaterialDefines with get, set
        /// <default>{}</default>
        abstract uniforms: ShaderMaterialParametersUniforms with get, set
        abstract vertexShader: string with get, set
        abstract fragmentShader: string with get, set
        /// <default>1</default>
        abstract linewidth: float with get, set
        /// <default>false</default>
        abstract wireframe: bool with get, set
        /// <default>1</default>
        abstract wireframeLinewidth: float with get, set
        /// <summary>Whether the material is affected by fog. Default is true.</summary>
        /// <default>false</default>
        abstract fog: bool with get, set
        /// <default>false</default>
        abstract lights: bool with get, set
        /// <default>false</default>
        abstract clipping: bool with get, set
        [<Obsolete("Use {@link ShaderMaterial#extensions.derivatives extensions.derivatives} instead.")>]
        abstract derivatives: obj option with get, set
        /// <default>{ derivatives: false, fragDepth: false, drawBuffers: false, shaderTextureLOD: false }</default>
        abstract extensions: {| derivatives: bool; fragDepth: bool; drawBuffers: bool; shaderTextureLOD: bool |} with get, set
        /// <default>{ 'color': [ 1, 1, 1 ], 'uv': [ 0, 0 ], 'uv2': [ 0, 0 ] }</default>
        abstract defaultAttributeValues: obj option with get, set
        /// <default>undefined</default>
        abstract index0AttributeName: string option with get, set
        /// <default>false</default>
        abstract uniformsNeedUpdate: bool with get, set
        /// <default>null</default>
        abstract glslVersion: GLSLVersion option with get, set
        abstract isShaderMaterial: bool with get, set
        /// Sets the properties based on the values.
        abstract setValues: parameters: ShaderMaterialParameters -> unit
        /// Convert the material to three.js JSON format.
        abstract toJSON: meta: obj option -> obj option

    type [<AllowNullLiteral>] ShaderMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: ShaderMaterialParameters -> ShaderMaterial

    type [<AllowNullLiteral>] ShaderMaterialParametersUniforms =
        [<EmitIndexer>] abstract Item: uniform: string -> IUniform with get, set

    type [<AllowNullLiteral>] ShaderMaterialDefines =
        [<EmitIndexer>] abstract Item: key: string -> obj option with get, set

module __materials_ShadowMaterial =
    type ColorRepresentation = Utils.ColorRepresentation
    type Color = __math_Color.Color
    type MaterialParameters = __materials_Material.MaterialParameters
    type Material = __materials_Material.Material

    type [<AllowNullLiteral>] IExports =
        abstract ShadowMaterial: ShadowMaterialStatic

    type [<AllowNullLiteral>] ShadowMaterialParameters =
        inherit MaterialParameters
        abstract color: ColorRepresentation option with get, set

    type [<AllowNullLiteral>] ShadowMaterial =
        inherit Material
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'ShadowMaterial'</default>
        abstract ``type``: string with get, set
        /// <default>new THREE.Color( 0x000000 )</default>
        abstract color: Color with get, set
        /// <summary>
        /// Defines whether this material is transparent. This has an effect on rendering as transparent objects need special treatment and are rendered after non-transparent objects.
        /// When set to true, the extent to which the material is transparent is controlled by setting it's .opacity property.
        /// Default is false.
        /// </summary>
        /// <default>true</default>
        abstract transparent: bool with get, set

    type [<AllowNullLiteral>] ShadowMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: ShadowMaterialParameters -> ShadowMaterial

module __materials_SpriteMaterial =
    type ColorRepresentation = Utils.ColorRepresentation
    type Color = __math_Color.Color
    type Texture = __textures_Texture.Texture
    type MaterialParameters = __materials_Material.MaterialParameters
    type Material = __materials_Material.Material

    type [<AllowNullLiteral>] IExports =
        abstract SpriteMaterial: SpriteMaterialStatic

    type [<AllowNullLiteral>] SpriteMaterialParameters =
        inherit MaterialParameters
        abstract color: ColorRepresentation option with get, set
        abstract map: Texture option with get, set
        abstract alphaMap: Texture option with get, set
        abstract rotation: float option with get, set
        abstract sizeAttenuation: bool option with get, set

    type [<AllowNullLiteral>] SpriteMaterial =
        inherit Material
        /// <summary>Value is the string 'Material'. This shouldn't be changed, and can be used to find all objects of this type in a scene.</summary>
        /// <default>'SpriteMaterial'</default>
        abstract ``type``: string with get, set
        /// <default>new THREE.Color( 0xffffff )</default>
        abstract color: Color with get, set
        /// <default>null</default>
        abstract map: Texture option with get, set
        /// <default>null</default>
        abstract alphaMap: Texture option with get, set
        /// <default>0</default>
        abstract rotation: float with get, set
        /// <default>true</default>
        abstract sizeAttenuation: bool with get, set
        /// <summary>
        /// Defines whether this material is transparent. This has an effect on rendering as transparent objects need special treatment and are rendered after non-transparent objects.
        /// When set to true, the extent to which the material is transparent is controlled by setting it's .opacity property.
        /// Default is false.
        /// </summary>
        /// <default>true</default>
        abstract transparent: bool with get, set
        abstract isSpriteMaterial: bool
        /// Sets the properties based on the values.
        abstract setValues: parameters: SpriteMaterialParameters -> unit
        /// Copy the parameters from the passed material into this material.
        abstract copy: source: SpriteMaterial -> SpriteMaterial

    type [<AllowNullLiteral>] SpriteMaterialStatic =
        [<EmitConstructor>] abstract Create: ?parameters: SpriteMaterialParameters -> SpriteMaterial



module __textures_CanvasTexture =
    type Texture = __textures_Texture.Texture
    type Mapping = Constants.Mapping
    type Wrapping = Constants.Wrapping
    type TextureFilter = Constants.TextureFilter
    type PixelFormat = Constants.PixelFormat
    type TextureDataType = Constants.TextureDataType

    type [<AllowNullLiteral>] IExports =
        abstract CanvasTexture: CanvasTextureStatic

    type [<AllowNullLiteral>] CanvasTexture =
        inherit Texture
        abstract isCanvasTexture: bool

    type [<AllowNullLiteral>] CanvasTextureStatic =
        /// <param name="canvas" />
        /// <param name="format" />
        /// <param name="type" />
        /// <param name="mapping" />
        /// <param name="wrapS" />
        /// <param name="wrapT" />
        /// <param name="magFilter" />
        /// <param name="minFilter" />
        /// <param name="anisotropy" />
        /// <param name="encoding" />
        [<EmitConstructor>] abstract Create: canvas: U4<HTMLImageElement, HTMLCanvasElement, HTMLVideoElement, ImageBitmap> * ?mapping: Mapping * ?wrapS: Wrapping * ?wrapT: Wrapping * ?magFilter: TextureFilter * ?minFilter: TextureFilter * ?format: PixelFormat * ?``type``: TextureDataType * ?anisotropy: float -> CanvasTexture

module __textures_CompressedTexture =
    type Texture = __textures_Texture.Texture
    type Mapping = Constants.Mapping
    type Wrapping = Constants.Wrapping
    type TextureFilter = Constants.TextureFilter
    type CompressedPixelFormat = Constants.CompressedPixelFormat
    type TextureDataType = Constants.TextureDataType
    type TextureEncoding = Constants.TextureEncoding

    type [<AllowNullLiteral>] IExports =
        abstract CompressedTexture: CompressedTextureStatic

    type [<AllowNullLiteral>] CompressedTexture =
        inherit Texture
        abstract image: {| width: float; height: float |} with get, set
        abstract mipmaps: ResizeArray<ImageData> with get, set
        /// <default>false</default>
        abstract flipY: bool with get, set
        /// <default>false</default>
        abstract generateMipmaps: bool with get, set
        abstract isCompressedTexture: bool

    type [<AllowNullLiteral>] CompressedTextureStatic =
        /// <param name="mipmaps" />
        /// <param name="width" />
        /// <param name="height" />
        /// <param name="format" />
        /// <param name="type" />
        /// <param name="mapping" />
        /// <param name="wrapS" />
        /// <param name="wrapT" />
        /// <param name="magFilter" />
        /// <param name="minFilter" />
        /// <param name="anisotropy" />
        /// <param name="encoding" />
        [<EmitConstructor>] abstract Create: mipmaps: ResizeArray<ImageData> * width: float * height: float * ?format: CompressedPixelFormat * ?``type``: TextureDataType * ?mapping: Mapping * ?wrapS: Wrapping * ?wrapT: Wrapping * ?magFilter: TextureFilter * ?minFilter: TextureFilter * ?anisotropy: float * ?encoding: TextureEncoding -> CompressedTexture

module __textures_CubeTexture =
    type Texture = __textures_Texture.Texture
    type Mapping = Constants.Mapping
    type Wrapping = Constants.Wrapping
    type TextureFilter = Constants.TextureFilter
    type PixelFormat = Constants.PixelFormat
    type TextureDataType = Constants.TextureDataType
    type TextureEncoding = Constants.TextureEncoding

    type [<AllowNullLiteral>] IExports =
        abstract CubeTexture: CubeTextureStatic

    type [<AllowNullLiteral>] CubeTexture =
        inherit Texture
        abstract images: obj option with get, set
        /// <default>false</default>
        abstract flipY: bool with get, set
        abstract isCubeTexture: bool

    type [<AllowNullLiteral>] CubeTextureStatic =
        /// <param name="images" />
        /// <param name="mapping" />
        /// <param name="wrapS" />
        /// <param name="wrapT" />
        /// <param name="magFilter" />
        /// <param name="minFilter" />
        /// <param name="format" />
        /// <param name="type" />
        /// <param name="anisotropy" />
        /// <param name="encoding" />
        [<EmitConstructor>] abstract Create: ?images: ResizeArray<obj option> * ?mapping: Mapping * ?wrapS: Wrapping * ?wrapT: Wrapping * ?magFilter: TextureFilter * ?minFilter: TextureFilter * ?format: PixelFormat * ?``type``: TextureDataType * ?anisotropy: float * ?encoding: TextureEncoding -> CubeTexture

module __textures_DataTexture =
    type Texture = __textures_Texture.Texture
    type Mapping = Constants.Mapping
    type Wrapping = Constants.Wrapping
    type TextureFilter = Constants.TextureFilter
    type PixelFormat = Constants.PixelFormat
    type TextureDataType = Constants.TextureDataType
    type TextureEncoding = Constants.TextureEncoding

    type [<AllowNullLiteral>] IExports =
        abstract DataTexture: DataTextureStatic

    type [<AllowNullLiteral>] DataTexture =
        inherit Texture
        abstract image: ImageData with get, set
        /// <default>false</default>
        abstract flipY: bool with get, set
        /// <default>false</default>
        abstract generateMipmaps: bool with get, set
        /// <default>1</default>
        abstract unpackAlignment: float with get, set
        /// <default>THREE.DepthFormat</default>
        abstract format: PixelFormat with get, set
        abstract isDataTexture: bool

    type [<AllowNullLiteral>] DataTextureStatic =
        /// <param name="data" />
        /// <param name="width" />
        /// <param name="height" />
        /// <param name="format" />
        /// <param name="type" />
        /// <param name="mapping" />
        /// <param name="wrapS" />
        /// <param name="wrapT" />
        /// <param name="magFilter" />
        /// <param name="minFilter" />
        /// <param name="anisotropy" />
        /// <param name="encoding" />
        [<EmitConstructor>] abstract Create: data: BufferSource * width: float * height: float * ?format: PixelFormat * ?``type``: TextureDataType * ?mapping: Mapping * ?wrapS: Wrapping * ?wrapT: Wrapping * ?magFilter: TextureFilter * ?minFilter: TextureFilter * ?anisotropy: float * ?encoding: TextureEncoding -> DataTexture

module __textures_DataTexture2DArray =
    type Texture = __textures_Texture.Texture
    type TextureFilter = Constants.TextureFilter

    type [<AllowNullLiteral>] IExports =
        abstract DataTexture2DArray: DataTexture2DArrayStatic

    type [<AllowNullLiteral>] DataTexture2DArray =
        inherit Texture
        /// <default>THREE.NearestFilter</default>
        abstract magFilter: TextureFilter with get, set
        /// <default>THREE.NearestFilter</default>
        abstract minFilter: TextureFilter with get, set
        /// <default>THREE.ClampToEdgeWrapping</default>
        abstract wrapR: bool with get, set
        /// <default>false</default>
        abstract flipY: bool with get, set
        /// <default>false</default>
        abstract generateMipmaps: bool with get, set
        abstract isDataTexture2DArray: bool

    type [<AllowNullLiteral>] DataTexture2DArrayStatic =
        [<EmitConstructor>] abstract Create: ?data: BufferSource * ?width: float * ?height: float * ?depth: float -> DataTexture2DArray

module __textures_DataTexture3D =
    type Texture = __textures_Texture.Texture
    type TextureFilter = Constants.TextureFilter

    type [<AllowNullLiteral>] IExports =
        abstract DataTexture3D: DataTexture3DStatic

    type [<AllowNullLiteral>] DataTexture3D =
        inherit Texture
        /// <default>THREE.NearestFilter</default>
        abstract magFilter: TextureFilter with get, set
        /// <default>THREE.NearestFilter</default>
        abstract minFilter: TextureFilter with get, set
        /// <default>THREE.ClampToEdgeWrapping</default>
        abstract wrapR: bool with get, set
        /// <default>false</default>
        abstract flipY: bool with get, set
        /// <default>false</default>
        abstract generateMipmaps: bool with get, set
        abstract isDataTexture3D: bool

    type [<AllowNullLiteral>] DataTexture3DStatic =
        [<EmitConstructor>] abstract Create: data: BufferSource * width: float * height: float * depth: float -> DataTexture3D

module __textures_DepthTexture =
    type Texture = __textures_Texture.Texture
    type Mapping = Constants.Mapping
    type Wrapping = Constants.Wrapping
    type TextureFilter = Constants.TextureFilter
    type TextureDataType = Constants.TextureDataType

    type [<AllowNullLiteral>] IExports =
        abstract DepthTexture: DepthTextureStatic

    type [<AllowNullLiteral>] DepthTexture =
        inherit Texture
        abstract image: {| width: float; height: float |} with get, set
        /// <default>false</default>
        abstract flipY: bool with get, set
        /// <default>false</default>
        abstract generateMipmaps: bool with get, set
        abstract isDepthTexture: bool

    type [<AllowNullLiteral>] DepthTextureStatic =
        /// <param name="width" />
        /// <param name="height" />
        /// <param name="type" />
        /// <param name="mapping" />
        /// <param name="wrapS" />
        /// <param name="wrapT" />
        /// <param name="magFilter" />
        /// <param name="minFilter" />
        /// <param name="anisotropy" />
        [<EmitConstructor>] abstract Create: width: float * height: float * ?``type``: TextureDataType * ?mapping: Mapping * ?wrapS: Wrapping * ?wrapT: Wrapping * ?magFilter: TextureFilter * ?minFilter: TextureFilter * ?anisotropy: float -> DepthTexture

module __textures_Texture =
    type Vector2 = __math_Vector2.Vector2
    type Matrix3 = __math_Matrix3.Matrix3
    type EventDispatcher = __core_EventDispatcher.EventDispatcher
    type Mapping = Constants.Mapping
    type Wrapping = Constants.Wrapping
    type TextureFilter = Constants.TextureFilter
    type PixelFormat = Constants.PixelFormat
    type PixelFormatGPU = Constants.PixelFormatGPU
    type TextureDataType = Constants.TextureDataType
    type TextureEncoding = Constants.TextureEncoding

    type [<AllowNullLiteral>] IExports =
        abstract Texture: TextureStatic

    type [<AllowNullLiteral>] Texture =
        inherit EventDispatcher
        abstract id: float with get, set
        abstract uuid: string with get, set
        /// <default>''</default>
        abstract name: string with get, set
        abstract sourceFile: string with get, set
        /// <default>THREE.Texture.DEFAULT_IMAGE</default>
        abstract image: obj option with get, set
        /// <default>[]</default>
        abstract mipmaps: ResizeArray<obj option> with get, set
        /// <default>THREE.Texture.DEFAULT_MAPPING</default>
        abstract mapping: Mapping with get, set
        /// <default>THREE.ClampToEdgeWrapping</default>
        abstract wrapS: Wrapping with get, set
        /// <default>THREE.ClampToEdgeWrapping</default>
        abstract wrapT: Wrapping with get, set
        /// <default>THREE.LinearFilter</default>
        abstract magFilter: TextureFilter with get, set
        /// <default>THREE.LinearMipmapLinearFilter</default>
        abstract minFilter: TextureFilter with get, set
        /// <default>1</default>
        abstract anisotropy: float with get, set
        /// <default>THREE.RGBAFormat</default>
        abstract format: PixelFormat with get, set
        abstract internalFormat: PixelFormatGPU option with get, set
        /// <default>THREE.UnsignedByteType</default>
        abstract ``type``: TextureDataType with get, set
        /// <default>new THREE.Matrix3()</default>
        abstract matrix: Matrix3 with get, set
        /// <default>true</default>
        abstract matrixAutoUpdate: bool with get, set
        /// <default>new THREE.Vector2( 0, 0 )</default>
        abstract offset: Vector2 with get, set
        /// <default>new THREE.Vector2( 1, 1 )</default>
        abstract repeat: Vector2 with get, set
        /// <default>new THREE.Vector2( 0, 0 )</default>
        abstract center: Vector2 with get, set
        /// <default>0</default>
        abstract rotation: float with get, set
        /// <default>true</default>
        abstract generateMipmaps: bool with get, set
        /// <default>false</default>
        abstract premultiplyAlpha: bool with get, set
        /// <default>true</default>
        abstract flipY: bool with get, set
        /// <default>4</default>
        abstract unpackAlignment: float with get, set
        /// <default>THREE.LinearEncoding</default>
        abstract encoding: TextureEncoding with get, set
        /// <default>false</default>
        abstract isRenderTargetTexture: bool with get, set
        /// <default>0</default>
        abstract version: float with get, set
        abstract needsUpdate: bool with get, set
        abstract isTexture: bool
        abstract onUpdate: (unit -> unit) with get, set
        abstract clone: unit -> Texture
        abstract copy: source: Texture -> Texture
        abstract toJSON: meta: obj option -> obj option
        abstract dispose: unit -> unit
        abstract transformUv: uv: Vector2 -> Vector2
        abstract updateMatrix: unit -> unit

    type [<AllowNullLiteral>] TextureStatic =
        /// <param name="image" />
        /// <param name="mapping" />
        /// <param name="wrapS" />
        /// <param name="wrapT" />
        /// <param name="magFilter" />
        /// <param name="minFilter" />
        /// <param name="format" />
        /// <param name="type" />
        /// <param name="anisotropy" />
        /// <param name="encoding" />
        [<EmitConstructor>] abstract Create: ?image: U3<HTMLImageElement, HTMLCanvasElement, HTMLVideoElement> * ?mapping: Mapping * ?wrapS: Wrapping * ?wrapT: Wrapping * ?magFilter: TextureFilter * ?minFilter: TextureFilter * ?format: PixelFormat * ?``type``: TextureDataType * ?anisotropy: float * ?encoding: TextureEncoding -> Texture
        abstract DEFAULT_IMAGE: obj option with get, set
        abstract DEFAULT_MAPPING: obj option with get, set

module __textures_VideoTexture =
    type Texture = __textures_Texture.Texture
    type Mapping = Constants.Mapping
    type Wrapping = Constants.Wrapping
    type TextureFilter = Constants.TextureFilter
    type PixelFormat = Constants.PixelFormat
    type TextureDataType = Constants.TextureDataType

    type [<AllowNullLiteral>] IExports =
        abstract VideoTexture: VideoTextureStatic

    type [<AllowNullLiteral>] VideoTexture =
        inherit Texture
        abstract isVideoTexture: bool
        /// <default>false</default>
        abstract generateMipmaps: bool with get, set

    type [<AllowNullLiteral>] VideoTextureStatic =
        /// <param name="video" />
        /// <param name="mapping" />
        /// <param name="wrapS" />
        /// <param name="wrapT" />
        /// <param name="magFilter" />
        /// <param name="minFilter" />
        /// <param name="format" />
        /// <param name="type" />
        /// <param name="anisotropy" />
        [<EmitConstructor>] abstract Create: video: HTMLVideoElement * ?mapping: Mapping * ?wrapS: Wrapping * ?wrapT: Wrapping * ?magFilter: TextureFilter * ?minFilter: TextureFilter * ?format: PixelFormat * ?``type``: TextureDataType * ?anisotropy: float -> VideoTexture



module __renderers_WebGL1Renderer =
    type WebGLRenderer = __renderers_WebGLRenderer.WebGLRenderer
    type WebGLRendererParameters = __renderers_WebGLRenderer.WebGLRendererParameters

    type [<AllowNullLiteral>] IExports =
        abstract WebGL1Renderer: WebGL1RendererStatic

    type [<AllowNullLiteral>] WebGL1Renderer =
        inherit WebGLRenderer
        abstract isWebGL1Renderer: bool

    type [<AllowNullLiteral>] WebGL1RendererStatic =
        [<EmitConstructor>] abstract Create: ?parameters: WebGLRendererParameters -> WebGL1Renderer

module __renderers_WebGLCubeRenderTarget =
    type WebGLRenderTargetOptions = __renderers_WebGLRenderTarget.WebGLRenderTargetOptions
    type WebGLRenderTarget = __renderers_WebGLRenderTarget.WebGLRenderTarget
    type WebGLRenderer = __renderers_WebGLRenderer.WebGLRenderer
    type Texture = __textures_Texture.Texture
    type CubeTexture = __textures_CubeTexture.CubeTexture

    type [<AllowNullLiteral>] IExports =
        abstract WebGLCubeRenderTarget: WebGLCubeRenderTargetStatic

    type [<AllowNullLiteral>] WebGLCubeRenderTarget =
        inherit WebGLRenderTarget
        abstract texture: CubeTexture with get, set
        abstract fromEquirectangularTexture: renderer: WebGLRenderer * texture: Texture -> WebGLCubeRenderTarget
        abstract clear: renderer: WebGLRenderer * color: bool * depth: bool * stencil: bool -> unit

    type [<AllowNullLiteral>] WebGLCubeRenderTargetStatic =
        [<EmitConstructor>] abstract Create: size: float * ?options: WebGLRenderTargetOptions -> WebGLCubeRenderTarget

module __renderers_WebGLMultipleRenderTargets =
    type EventDispatcher = __core_EventDispatcher.EventDispatcher
    type Texture = __textures_Texture.Texture

    type [<AllowNullLiteral>] IExports =
        /// This class originall extended WebGLMultipleRenderTarget
        /// However, there are some issues with this method as documented below
        abstract WebGLMultipleRenderTargets: WebGLMultipleRenderTargetsStatic

    /// This class originall extended WebGLMultipleRenderTarget
    /// However, there are some issues with this method as documented below
    type [<AllowNullLiteral>] WebGLMultipleRenderTargets =
        inherit EventDispatcher
        abstract texture: ResizeArray<Texture> with get, set
        abstract isWebGLMultipleRenderTargets: obj
        abstract setSize: width: float * height: float * ?depth: float -> WebGLMultipleRenderTargets
        abstract copy: source: WebGLMultipleRenderTargets -> WebGLMultipleRenderTargets
        abstract clone: unit -> WebGLMultipleRenderTargets
        abstract dispose: unit -> unit
        abstract setTexture: texture: Texture -> unit

    /// This class originall extended WebGLMultipleRenderTarget
    /// However, there are some issues with this method as documented below
    type [<AllowNullLiteral>] WebGLMultipleRenderTargetsStatic =
        [<EmitConstructor>] abstract Create: width: float * height: float * count: float -> WebGLMultipleRenderTargets

module __renderers_WebGLMultisampleRenderTarget =
    type WebGLRenderTarget = __renderers_WebGLRenderTarget.WebGLRenderTarget
    type WebGLRenderTargetOptions = __renderers_WebGLRenderTarget.WebGLRenderTargetOptions

    type [<AllowNullLiteral>] IExports =
        abstract WebGLMultisampleRenderTarget: WebGLMultisampleRenderTargetStatic

    type [<AllowNullLiteral>] WebGLMultisampleRenderTarget =
        inherit WebGLRenderTarget
        abstract isWebGLMultisampleRenderTarget: bool
        /// <summary>Specifies the number of samples to be used for the renderbuffer storage.However, the maximum supported size for multisampling is platform dependent and defined via gl.MAX_SAMPLES.</summary>
        /// <default>4</default>
        abstract samples: float with get, set

    type [<AllowNullLiteral>] WebGLMultisampleRenderTargetStatic =
        [<EmitConstructor>] abstract Create: width: float * height: float * ?options: WebGLRenderTargetOptions -> WebGLMultisampleRenderTarget

module __renderers_WebGLRenderTarget =
    type Vector4 = __math_Vector4.Vector4
    type Texture = __textures_Texture.Texture
    type DepthTexture = __textures_DepthTexture.DepthTexture
    type EventDispatcher = __core_EventDispatcher.EventDispatcher
    type Wrapping = Constants.Wrapping
    type TextureFilter = Constants.TextureFilter
    type TextureDataType = Constants.TextureDataType
    type TextureEncoding = Constants.TextureEncoding

    type [<AllowNullLiteral>] IExports =
        abstract WebGLRenderTarget: WebGLRenderTargetStatic

    type [<AllowNullLiteral>] WebGLRenderTargetOptions =
        abstract wrapS: Wrapping option with get, set
        abstract wrapT: Wrapping option with get, set
        abstract magFilter: TextureFilter option with get, set
        abstract minFilter: TextureFilter option with get, set
        abstract format: float option with get, set
        abstract ``type``: TextureDataType option with get, set
        abstract anisotropy: float option with get, set
        abstract depthBuffer: bool option with get, set
        abstract stencilBuffer: bool option with get, set
        abstract generateMipmaps: bool option with get, set
        abstract depthTexture: DepthTexture option with get, set
        abstract encoding: TextureEncoding option with get, set

    type [<AllowNullLiteral>] WebGLRenderTarget =
        inherit EventDispatcher
        abstract uuid: string with get, set
        abstract width: float with get, set
        abstract height: float with get, set
        abstract depth: float with get, set
        abstract scissor: Vector4 with get, set
        /// <default>false</default>
        abstract scissorTest: bool with get, set
        abstract viewport: Vector4 with get, set
        abstract texture: Texture with get, set
        /// <default>true</default>
        abstract depthBuffer: bool with get, set
        /// <default>true</default>
        abstract stencilBuffer: bool with get, set
        /// <default>null</default>
        abstract depthTexture: DepthTexture with get, set
        abstract isWebGLRenderTarget: bool
        [<Obsolete("Use {@link Texture#wrapS texture.wrapS} instead.")>]
        abstract wrapS: obj option with get, set
        [<Obsolete("Use {@link Texture#wrapT texture.wrapT} instead.")>]
        abstract wrapT: obj option with get, set
        [<Obsolete("Use {@link Texture#magFilter texture.magFilter} instead.")>]
        abstract magFilter: obj option with get, set
        [<Obsolete("Use {@link Texture#minFilter texture.minFilter} instead.")>]
        abstract minFilter: obj option with get, set
        [<Obsolete("Use {@link Texture#anisotropy texture.anisotropy} instead.")>]
        abstract anisotropy: obj option with get, set
        [<Obsolete("Use {@link Texture#offset texture.offset} instead.")>]
        abstract offset: obj option with get, set
        [<Obsolete("Use {@link Texture#repeat texture.repeat} instead.")>]
        abstract repeat: obj option with get, set
        [<Obsolete("Use {@link Texture#format texture.format} instead.")>]
        abstract format: obj option with get, set
        [<Obsolete("Use {@link Texture#type texture.type} instead.")>]
        abstract ``type``: obj option with get, set
        [<Obsolete("Use {@link Texture#generateMipmaps texture.generateMipmaps} instead.")>]
        abstract generateMipmaps: obj option with get, set
        abstract setTexture: texture: Texture -> unit
        abstract setSize: width: float * height: float * ?depth: float -> unit
        abstract clone: unit -> WebGLRenderTarget
        abstract copy: source: WebGLRenderTarget -> WebGLRenderTarget
        abstract dispose: unit -> unit

    type [<AllowNullLiteral>] WebGLRenderTargetStatic =
        [<EmitConstructor>] abstract Create: width: float * height: float * ?options: WebGLRenderTargetOptions -> WebGLRenderTarget

module __renderers_WebGLRenderer =
    type Scene = __scenes_Scene.Scene
    type Camera = __cameras_Camera.Camera
    type WebGLExtensions = __renderers_webgl_WebGLExtensions.WebGLExtensions
    type WebGLInfo = __renderers_webgl_WebGLInfo.WebGLInfo
    type WebGLShadowMap = __renderers_webgl_WebGLShadowMap.WebGLShadowMap
    type WebGLCapabilities = __renderers_webgl_WebGLCapabilities.WebGLCapabilities
    type WebGLProperties = __renderers_webgl_WebGLProperties.WebGLProperties
    type WebGLProgram = __renderers_webgl_WebGLProgram.WebGLProgram
    type RenderTarget = __renderers_webgl_WebGLRenderLists.RenderTarget
    type WebGLRenderLists = __renderers_webgl_WebGLRenderLists.WebGLRenderLists
    type WebGLState = __renderers_webgl_WebGLState.WebGLState
    type Vector2 = __math_Vector2.Vector2
    type Vector4 = __math_Vector4.Vector4
    type Color = __math_Color.Color
    type WebGLRenderTarget = __renderers_WebGLRenderTarget.WebGLRenderTarget
    type Object3D = __core_Object3D.Object3D
    type Material = __materials_Material.Material
    type ToneMapping = Constants.ToneMapping
    type ShadowMapType = Constants.ShadowMapType
    type CullFace = Constants.CullFace
    type TextureEncoding = Constants.TextureEncoding
    type WebXRManager = __renderers_webxr_WebXRManager.WebXRManager
    type BufferGeometry = __core_BufferGeometry.BufferGeometry
    type Texture = __textures_Texture.Texture
    type DataTexture3D = __textures_DataTexture3D.DataTexture3D
    type XRAnimationLoopCallback = __renderers_webxr_WebXR.XRAnimationLoopCallback
    type Vector3 = __math_Vector3.Vector3
    type Box3 = __math_Box3.Box3
    type DataTexture2DArray = __textures_DataTexture2DArray.DataTexture2DArray
    type ColorRepresentation = Utils.ColorRepresentation

    type [<AllowNullLiteral>] IExports =
        /// <summary>
        /// The WebGL renderer displays your beautifully crafted scenes using WebGL, if your device supports it.
        /// This renderer has way better performance than CanvasRenderer.
        /// 
        /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/renderers/WebGLRenderer.js">src/renderers/WebGLRenderer.js</see>
        /// </summary>
        abstract WebGLRenderer: WebGLRendererStatic

    type [<AllowNullLiteral>] Renderer =
        abstract domElement: HTMLCanvasElement with get, set
        abstract render: scene: Object3D * camera: Camera -> unit
        abstract setSize: width: float * height: float * ?updateStyle: bool -> unit

    /// This is only available in worker JS contexts, not the DOM.
    type [<AllowNullLiteral>] OffscreenCanvas =
        inherit EventTarget

    type [<AllowNullLiteral>] WebGLRendererParameters =
        /// A Canvas where the renderer draws its output.
        abstract canvas: U2<HTMLCanvasElement, OffscreenCanvas> option with get, set
        /// <summary>
        /// A WebGL Rendering Context.
        /// (<see href="https://developer.mozilla.org/en-US/docs/Web/API/WebGLRenderingContext)" />
        /// Default is null
        /// </summary>
        abstract context: WebGLRenderingContext option with get, set
        /// shader precision. Can be "highp", "mediump" or "lowp".
        abstract precision: string option with get, set
        /// default is false.
        abstract alpha: bool option with get, set
        /// default is true.
        abstract premultipliedAlpha: bool option with get, set
        /// default is false.
        abstract antialias: bool option with get, set
        /// default is true.
        abstract stencil: bool option with get, set
        /// default is false.
        abstract preserveDrawingBuffer: bool option with get, set
        /// Can be "high-performance", "low-power" or "default"
        abstract powerPreference: string option with get, set
        /// default is true.
        abstract depth: bool option with get, set
        /// default is false.
        abstract logarithmicDepthBuffer: bool option with get, set
        /// default is false.
        abstract failIfMajorPerformanceCaveat: bool option with get, set

    type [<AllowNullLiteral>] WebGLDebug =
        /// Enables error checking and reporting when shader programs are being compiled.
        abstract checkShaderErrors: bool with get, set

    /// <summary>
    /// The WebGL renderer displays your beautifully crafted scenes using WebGL, if your device supports it.
    /// This renderer has way better performance than CanvasRenderer.
    /// 
    /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/renderers/WebGLRenderer.js">src/renderers/WebGLRenderer.js</see>
    /// </summary>
    type [<AllowNullLiteral>] WebGLRenderer =
        inherit Renderer
        /// <summary>
        /// A Canvas where the renderer draws its output.
        /// This is automatically created by the renderer in the constructor (if not provided already); you just need to add it to your page.
        /// </summary>
        /// <default>document.createElementNS( '<see href="http://www.w3.org/1999/xhtml'," /> 'canvas' )</default>
        abstract domElement: HTMLCanvasElement with get, set
        /// The HTML5 Canvas's 'webgl' context obtained from the canvas where the renderer will draw.
        abstract context: WebGLRenderingContext with get, set
        /// <summary>Defines whether the renderer should automatically clear its output before rendering.</summary>
        /// <default>true</default>
        abstract autoClear: bool with get, set
        /// <summary>If autoClear is true, defines whether the renderer should clear the color buffer. Default is true.</summary>
        /// <default>true</default>
        abstract autoClearColor: bool with get, set
        /// <summary>If autoClear is true, defines whether the renderer should clear the depth buffer. Default is true.</summary>
        /// <default>true</default>
        abstract autoClearDepth: bool with get, set
        /// <summary>If autoClear is true, defines whether the renderer should clear the stencil buffer. Default is true.</summary>
        /// <default>true</default>
        abstract autoClearStencil: bool with get, set
        /// <summary>Debug configurations.</summary>
        /// <default>{ checkShaderErrors: true }</default>
        abstract debug: WebGLDebug with get, set
        /// <summary>Defines whether the renderer should sort objects. Default is true.</summary>
        /// <default>true</default>
        abstract sortObjects: bool with get, set
        /// <default>[]</default>
        abstract clippingPlanes: ResizeArray<obj option> with get, set
        /// <default>false</default>
        abstract localClippingEnabled: bool with get, set
        abstract extensions: WebGLExtensions with get, set
        /// <summary>Default is LinearEncoding.</summary>
        /// <default>THREE.LinearEncoding</default>
        abstract outputEncoding: TextureEncoding with get, set
        /// <default>false</default>
        abstract physicallyCorrectLights: bool with get, set
        /// <default>THREE.NoToneMapping</default>
        abstract toneMapping: ToneMapping with get, set
        /// <default>1</default>
        abstract toneMappingExposure: float with get, set
        abstract info: WebGLInfo with get, set
        abstract shadowMap: WebGLShadowMap with get, set
        abstract pixelRatio: float with get, set
        abstract capabilities: WebGLCapabilities with get, set
        abstract properties: WebGLProperties with get, set
        abstract renderLists: WebGLRenderLists with get, set
        abstract state: WebGLState with get, set
        abstract xr: WebXRManager with get, set
        /// Return the WebGL context.
        abstract getContext: unit -> WebGLRenderingContext
        abstract getContextAttributes: unit -> obj option
        abstract forceContextLoss: unit -> unit
        [<Obsolete("Use {@link WebGLCapabilities#getMaxAnisotropy .capabilities.getMaxAnisotropy()} instead.")>]
        abstract getMaxAnisotropy: unit -> float
        [<Obsolete("Use {@link WebGLCapabilities#precision .capabilities.precision} instead.")>]
        abstract getPrecision: unit -> string
        abstract getPixelRatio: unit -> float
        abstract setPixelRatio: value: float -> unit
        abstract getDrawingBufferSize: target: Vector2 -> Vector2
        abstract setDrawingBufferSize: width: float * height: float * pixelRatio: float -> unit
        abstract getSize: target: Vector2 -> Vector2
        /// Resizes the output canvas to (width, height), and also sets the viewport to fit that size, starting in (0, 0).
        abstract setSize: width: float * height: float * ?updateStyle: bool -> unit
        abstract getCurrentViewport: target: Vector4 -> Vector4
        /// Copies the viewport into target.
        abstract getViewport: target: Vector4 -> Vector4
        /// Sets the viewport to render from (x, y) to (x + width, y + height).
        /// (x, y) is the lower-left corner of the region.
        abstract setViewport: x: U2<Vector4, float> * ?y: float * ?width: float * ?height: float -> unit
        /// Copies the scissor area into target.
        abstract getScissor: target: Vector4 -> Vector4
        /// Sets the scissor area from (x, y) to (x + width, y + height).
        abstract setScissor: x: U2<Vector4, float> * ?y: float * ?width: float * ?height: float -> unit
        /// Returns true if scissor test is enabled; returns false otherwise.
        abstract getScissorTest: unit -> bool
        /// Enable the scissor test. When this is enabled, only the pixels within the defined scissor area will be affected by further renderer actions.
        abstract setScissorTest: enable: bool -> unit
        /// Sets the custom opaque sort function for the WebGLRenderLists. Pass null to use the default painterSortStable function.
        abstract setOpaqueSort: method: (obj option -> obj option -> float) -> unit
        /// Sets the custom transparent sort function for the WebGLRenderLists. Pass null to use the default reversePainterSortStable function.
        abstract setTransparentSort: method: (obj option -> obj option -> float) -> unit
        /// Returns a THREE.Color instance with the current clear color.
        abstract getClearColor: target: Color -> Color
        /// Sets the clear color, using color for the color and alpha for the opacity.
        abstract setClearColor: color: ColorRepresentation * ?alpha: float -> unit
        /// Returns a float with the current clear alpha. Ranges from 0 to 1.
        abstract getClearAlpha: unit -> float
        abstract setClearAlpha: alpha: float -> unit
        /// Tells the renderer to clear its color, depth or stencil drawing buffer(s).
        /// Arguments default to true
        abstract clear: ?color: bool * ?depth: bool * ?stencil: bool -> unit
        abstract clearColor: unit -> unit
        abstract clearDepth: unit -> unit
        abstract clearStencil: unit -> unit
        abstract clearTarget: renderTarget: WebGLRenderTarget * color: bool * depth: bool * stencil: bool -> unit
        [<Obsolete("Use {@link WebGLState#reset .state.reset()} instead.")>]
        abstract resetGLState: unit -> unit
        abstract dispose: unit -> unit
        abstract renderBufferImmediate: object: Object3D * program: WebGLProgram -> unit
        abstract renderBufferDirect: camera: Camera * scene: Scene * geometry: BufferGeometry * material: Material * object: Object3D * geometryGroup: obj option -> unit
        /// <summary>A build in function that can be used instead of requestAnimationFrame. For WebXR projects this function must be used.</summary>
        /// <param name="callback">The function will be called every available frame. If <c>null</c> is passed it will stop any already ongoing animation.</param>
        abstract setAnimationLoop: callback: XRAnimationLoopCallback option -> unit
        [<Obsolete("Use {@link WebGLRenderer#setAnimationLoop .setAnimationLoop()} instead.")>]
        abstract animate: callback: (unit -> unit) -> unit
        /// Compiles all materials in the scene with the camera. This is useful to precompile shaders before the first rendering.
        abstract compile: scene: Object3D * camera: Camera -> unit
        /// <summary>
        /// Render a scene or an object using a camera.
        /// The render is done to a previously specified <see cref="WebGLRenderTarget.renderTarget">.renderTarget</see> set by calling
        /// <see cref="WebGLRenderer.setRenderTarget">.setRenderTarget</see> or to the canvas as usual.
        /// 
        /// By default render buffers are cleared before rendering but you can prevent this by setting the property
        /// <see cref="WebGLRenderer.autoClear">autoClear</see> to false. If you want to prevent only certain buffers being cleared
        /// you can set either the <see cref="WebGLRenderer.autoClearColor">autoClearColor</see>,
        /// <see cref="WebGLRenderer.autoClearStencil">autoClearStencil</see> or <see cref="WebGLRenderer.autoClearDepth">autoClearDepth</see>
        /// properties to false. To forcibly clear one ore more buffers call <see cref="WebGLRenderer.clear">.clear</see>.
        /// </summary>
        abstract render: scene: Object3D * camera: Camera -> unit
        /// Returns the current active cube face.
        abstract getActiveCubeFace: unit -> float
        /// Returns the current active mipmap level.
        abstract getActiveMipmapLevel: unit -> float
        /// Returns the current render target. If no render target is set, null is returned.
        abstract getRenderTarget: unit -> RenderTarget option
        [<Obsolete("Use {@link WebGLRenderer#getRenderTarget .getRenderTarget()} instead.")>]
        abstract getCurrentRenderTarget: unit -> RenderTarget option
        /// <summary>Sets the active render target.</summary>
        /// <param name="renderTarget">The <see cref="WebGLRenderTarget">renderTarget</see> that needs to be activated. When <c>null</c> is given, the canvas is set as the active render target instead.</param>
        /// <param name="activeCubeFace">Specifies the active cube side (PX 0, NX 1, PY 2, NY 3, PZ 4, NZ 5) of <see cref="WebGLCubeRenderTarget" />.</param>
        /// <param name="activeMipmapLevel">Specifies the active mipmap level.</param>
        abstract setRenderTarget: renderTarget: RenderTarget option * ?activeCubeFace: float * ?activeMipmapLevel: float -> unit
        abstract readRenderTargetPixels: renderTarget: RenderTarget * x: float * y: float * width: float * height: float * buffer: obj option * ?activeCubeFaceIndex: float -> unit
        /// <summary>
        /// Copies a region of the currently bound framebuffer into the selected mipmap level of the selected texture.
        /// This region is defined by the size of the destination texture's mip level, offset by the input position.
        /// </summary>
        /// <param name="position">Specifies the pixel offset from which to copy out of the framebuffer.</param>
        /// <param name="texture">Specifies the destination texture.</param>
        /// <param name="level">Specifies the destination mipmap level of the texture.</param>
        abstract copyFramebufferToTexture: position: Vector2 * texture: Texture * ?level: float -> unit
        /// <summary>Copies srcTexture to the specified level of dstTexture, offset by the input position.</summary>
        /// <param name="position">Specifies the pixel offset into the dstTexture where the copy will occur.</param>
        /// <param name="srcTexture">Specifies the source texture.</param>
        /// <param name="dstTexture">Specifies the destination texture.</param>
        /// <param name="level">Specifies the destination mipmap level of the texture.</param>
        abstract copyTextureToTexture: position: Vector2 * srcTexture: Texture * dstTexture: Texture * ?level: float -> unit
        /// <summary>Copies the pixels of a texture in the bounds sourceBox in the desination texture starting from the given position.</summary>
        /// <param name="sourceBox">Specifies the bounds</param>
        /// <param name="position">Specifies the pixel offset into the dstTexture where the copy will occur.</param>
        /// <param name="srcTexture">Specifies the source texture.</param>
        /// <param name="dstTexture">Specifies the destination texture.</param>
        /// <param name="level">Specifies the destination mipmap level of the texture.</param>
        abstract copyTextureToTexture3D: sourceBox: Box3 * position: Vector3 * srcTexture: Texture * dstTexture: U2<DataTexture3D, DataTexture2DArray> * ?level: float -> unit
        /// <summary>Initializes the given texture. Can be used to preload a texture rather than waiting until first render (which can cause noticeable lags due to decode and GPU upload overhead).</summary>
        /// <param name="texture">The texture to Initialize.</param>
        abstract initTexture: texture: Texture -> unit
        /// Can be used to reset the internal WebGL state.
        abstract resetState: unit -> unit
        [<Obsolete("")>]
        abstract gammaFactor: float with get, set
        [<Obsolete("Use {@link WebGLRenderer#xr .xr} instead.")>]
        abstract vr: bool with get, set
        [<Obsolete("Use {@link WebGLShadowMap#enabled .shadowMap.enabled} instead.")>]
        abstract shadowMapEnabled: bool with get, set
        [<Obsolete("Use {@link WebGLShadowMap#type .shadowMap.type} instead.")>]
        abstract shadowMapType: ShadowMapType with get, set
        [<Obsolete("Use {@link WebGLShadowMap#cullFace .shadowMap.cullFace} instead.")>]
        abstract shadowMapCullFace: CullFace with get, set
        [<Obsolete("Use {@link WebGLExtensions#get .extensions.get( 'OES_texture_float' )} instead.")>]
        abstract supportsFloatTextures: unit -> obj option
        [<Obsolete("Use {@link WebGLExtensions#get .extensions.get( 'OES_texture_half_float' )} instead.")>]
        abstract supportsHalfFloatTextures: unit -> obj option
        [<Obsolete("Use {@link WebGLExtensions#get .extensions.get( 'OES_standard_derivatives' )} instead.")>]
        abstract supportsStandardDerivatives: unit -> obj option
        [<Obsolete("Use {@link WebGLExtensions#get .extensions.get( 'WEBGL_compressed_texture_s3tc' )} instead.")>]
        abstract supportsCompressedTextureS3TC: unit -> obj option
        [<Obsolete("Use {@link WebGLExtensions#get .extensions.get( 'WEBGL_compressed_texture_pvrtc' )} instead.")>]
        abstract supportsCompressedTexturePVRTC: unit -> obj option
        [<Obsolete("Use {@link WebGLExtensions#get .extensions.get( 'EXT_blend_minmax' )} instead.")>]
        abstract supportsBlendMinMax: unit -> obj option
        [<Obsolete("Use {@link WebGLCapabilities#vertexTextures .capabilities.vertexTextures} instead.")>]
        abstract supportsVertexTextures: unit -> obj option
        [<Obsolete("Use {@link WebGLExtensions#get .extensions.get( 'ANGLE_instanced_arrays' )} instead.")>]
        abstract supportsInstancedArrays: unit -> obj option
        [<Obsolete("Use {@link WebGLRenderer#setScissorTest .setScissorTest()} instead.")>]
        abstract enableScissorTest: boolean: obj option -> obj option

    /// <summary>
    /// The WebGL renderer displays your beautifully crafted scenes using WebGL, if your device supports it.
    /// This renderer has way better performance than CanvasRenderer.
    /// 
    /// see <see href="https://github.com/mrdoob/three.js/blob/master/src/renderers/WebGLRenderer.js">src/renderers/WebGLRenderer.js</see>
    /// </summary>
    type [<AllowNullLiteral>] WebGLRendererStatic =
        /// parameters is an optional object with properties defining the renderer's behaviour.
        /// The constructor also accepts no parameters at all.
        /// In all cases, it will assume sane defaults when parameters are missing.
        [<EmitConstructor>] abstract Create: ?parameters: WebGLRendererParameters -> WebGLRenderer

module __renderers_shaders_ShaderChunk =

    type [<AllowNullLiteral>] IExports =
        abstract ShaderChunk: IExportsShaderChunk with get, set

    type [<AllowNullLiteral>] IExportsShaderChunk =
        [<EmitIndexer>] abstract Item: name: string -> string with get, set
        abstract alphamap_fragment: string with get, set
        abstract alphamap_pars_fragment: string with get, set
        abstract alphatest_fragment: string with get, set
        abstract aomap_fragment: string with get, set
        abstract aomap_pars_fragment: string with get, set
        abstract begin_vertex: string with get, set
        abstract beginnormal_vertex: string with get, set
        abstract bsdfs: string with get, set
        abstract bumpmap_pars_fragment: string with get, set
        abstract clipping_planes_fragment: string with get, set
        abstract clipping_planes_pars_fragment: string with get, set
        abstract clipping_planes_pars_vertex: string with get, set
        abstract clipping_planes_vertex: string with get, set
        abstract color_fragment: string with get, set
        abstract color_pars_fragment: string with get, set
        abstract color_pars_vertex: string with get, set
        abstract color_vertex: string with get, set
        abstract common: string with get, set
        abstract cube_frag: string with get, set
        abstract cube_vert: string with get, set
        abstract cube_uv_reflection_fragment: string with get, set
        abstract defaultnormal_vertex: string with get, set
        abstract depth_frag: string with get, set
        abstract depth_vert: string with get, set
        abstract distanceRGBA_frag: string with get, set
        abstract distanceRGBA_vert: string with get, set
        abstract displacementmap_vertex: string with get, set
        abstract displacementmap_pars_vertex: string with get, set
        abstract emissivemap_fragment: string with get, set
        abstract emissivemap_pars_fragment: string with get, set
        abstract encodings_pars_fragment: string with get, set
        abstract encodings_fragment: string with get, set
        abstract envmap_fragment: string with get, set
        abstract envmap_common_pars_fragment: string with get, set
        abstract envmap_pars_fragment: string with get, set
        abstract envmap_pars_vertex: string with get, set
        abstract envmap_vertex: string with get, set
        abstract equirect_frag: string with get, set
        abstract equirect_vert: string with get, set
        abstract fog_fragment: string with get, set
        abstract fog_pars_fragment: string with get, set
        abstract linedashed_frag: string with get, set
        abstract linedashed_vert: string with get, set
        abstract lightmap_fragment: string with get, set
        abstract lightmap_pars_fragment: string with get, set
        abstract lights_lambert_vertex: string with get, set
        abstract lights_pars_begin: string with get, set
        abstract envmap_physical_pars_fragment: string with get, set
        abstract lights_pars_map: string with get, set
        abstract lights_phong_fragment: string with get, set
        abstract lights_phong_pars_fragment: string with get, set
        abstract lights_physical_fragment: string with get, set
        abstract lights_physical_pars_fragment: string with get, set
        abstract lights_fragment_begin: string with get, set
        abstract lights_fragment_maps: string with get, set
        abstract lights_fragment_end: string with get, set
        abstract logdepthbuf_fragment: string with get, set
        abstract logdepthbuf_pars_fragment: string with get, set
        abstract logdepthbuf_pars_vertex: string with get, set
        abstract logdepthbuf_vertex: string with get, set
        abstract map_fragment: string with get, set
        abstract map_pars_fragment: string with get, set
        abstract map_particle_fragment: string with get, set
        abstract map_particle_pars_fragment: string with get, set
        abstract meshbasic_frag: string with get, set
        abstract meshbasic_vert: string with get, set
        abstract meshlambert_frag: string with get, set
        abstract meshlambert_vert: string with get, set
        abstract meshphong_frag: string with get, set
        abstract meshphong_vert: string with get, set
        abstract meshphysical_frag: string with get, set
        abstract meshphysical_vert: string with get, set
        abstract metalnessmap_fragment: string with get, set
        abstract metalnessmap_pars_fragment: string with get, set
        abstract morphnormal_vertex: string with get, set
        abstract morphtarget_pars_vertex: string with get, set
        abstract morphtarget_vertex: string with get, set
        abstract normal_flip: string with get, set
        abstract normal_frag: string with get, set
        abstract normal_fragment_begin: string with get, set
        abstract normal_fragment_maps: string with get, set
        abstract normal_vert: string with get, set
        abstract normalmap_pars_fragment: string with get, set
        abstract clearcoat_normal_fragment_begin: string with get, set
        abstract clearcoat_normal_fragment_maps: string with get, set
        abstract clearcoat_pars_fragment: string with get, set
        abstract packing: string with get, set
        abstract points_frag: string with get, set
        abstract points_vert: string with get, set
        abstract shadow_frag: string with get, set
        abstract shadow_vert: string with get, set
        abstract premultiplied_alpha_fragment: string with get, set
        abstract project_vertex: string with get, set
        abstract roughnessmap_fragment: string with get, set
        abstract roughnessmap_pars_fragment: string with get, set
        abstract shadowmap_pars_fragment: string with get, set
        abstract shadowmap_pars_vertex: string with get, set
        abstract shadowmap_vertex: string with get, set
        abstract shadowmask_pars_fragment: string with get, set
        abstract skinbase_vertex: string with get, set
        abstract skinning_pars_vertex: string with get, set
        abstract skinning_vertex: string with get, set
        abstract skinnormal_vertex: string with get, set
        abstract specularmap_fragment: string with get, set
        abstract specularmap_pars_fragment: string with get, set
        abstract tonemapping_fragment: string with get, set
        abstract tonemapping_pars_fragment: string with get, set
        abstract uv2_pars_fragment: string with get, set
        abstract uv2_pars_vertex: string with get, set
        abstract uv2_vertex: string with get, set
        abstract uv_pars_fragment: string with get, set
        abstract uv_pars_vertex: string with get, set
        abstract uv_vertex: string with get, set
        abstract worldpos_vertex: string with get, set

module __renderers_shaders_ShaderLib =
    open __renderers_shaders_UniformsLib

    type [<AllowNullLiteral>] IExports =
        abstract ShaderLib: IExportsShaderLib with get, set

    type [<AllowNullLiteral>] Shader =
        abstract uniforms: ShaderUniforms with get, set
        abstract vertexShader: string with get, set
        abstract fragmentShader: string with get, set

    type [<AllowNullLiteral>] IExportsShaderLib =
        [<EmitIndexer>] abstract Item: name: string -> Shader with get, set
        abstract basic: Shader with get, set
        abstract lambert: Shader with get, set
        abstract phong: Shader with get, set
        abstract standard: Shader with get, set
        abstract matcap: Shader with get, set
        abstract points: Shader with get, set
        abstract dashed: Shader with get, set
        abstract depth: Shader with get, set
        abstract normal: Shader with get, set
        abstract sprite: Shader with get, set
        abstract background: Shader with get, set
        abstract cube: Shader with get, set
        abstract equirect: Shader with get, set
        abstract distanceRGBA: Shader with get, set
        abstract shadow: Shader with get, set
        abstract physical: Shader with get, set

    type [<AllowNullLiteral>] ShaderUniforms =
        [<EmitIndexer>] abstract Item: uniform: string -> IUniform with get, set

module __renderers_shaders_UniformsLib =

    type [<AllowNullLiteral>] IExports =
        abstract UniformsLib: IExportsUniformsLib with get, set
// { Attributes = []  Comments = []  Name = IUniform  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/renderers/shaders/UniformsLib".IUniform  Type = Generic ({ Type = Mapped ({ Name = IUniform  FullName = IUniform  Declarations = [object Object] })  TypeParameters = [Union ({ Option = true  Types = [Mapped ({ Name = obj  FullName = obj  Declarations = [object Object] })] })] })  TypeParameters = [] }

    type [<AllowNullLiteral>] IUniform<'TValue> =
        abstract value: 'TValue with get, set

    type [<AllowNullLiteral>] IExportsUniformsLibCommon =
        abstract diffuse: IUniform with get, set
        abstract opacity: IUniform with get, set
        abstract map: IUniform with get, set
        abstract uvTransform: IUniform with get, set
        abstract uv2Transform: IUniform with get, set
        abstract alphaMap: IUniform with get, set

    type [<AllowNullLiteral>] IExportsUniformsLibEnvmap =
        abstract envMap: IUniform with get, set
        abstract flipEnvMap: IUniform with get, set
        abstract reflectivity: IUniform with get, set
        abstract refractionRatio: IUniform with get, set
        abstract maxMipLevel: IUniform with get, set

    type [<AllowNullLiteral>] IExportsUniformsLibLightsDirectionalLightsPropertiesDirection =
        interface end

    type [<AllowNullLiteral>] IExportsUniformsLibLightsSpotLightsProperties =
        abstract color: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection with get, set
        abstract position: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection with get, set
        abstract direction: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection with get, set
        abstract distance: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection with get, set
        abstract coneCos: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection with get, set
        abstract penumbraCos: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection with get, set
        abstract decay: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection with get, set

    type [<AllowNullLiteral>] IExportsUniformsLibLights =
        abstract ambientLightColor: IUniform with get, set
        abstract directionalLights: {| value: ResizeArray<obj option>; properties: {| direction: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; color: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection |} |} with get, set
        abstract directionalLightShadows: {| value: ResizeArray<obj option>; properties: {| shadowBias: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; shadowNormalBias: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; shadowRadius: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; shadowMapSize: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection |} |} with get, set
        abstract directionalShadowMap: IUniform with get, set
        abstract directionalShadowMatrix: IUniform with get, set
        abstract spotLights: {| value: ResizeArray<obj option>; properties: IExportsUniformsLibLightsSpotLightsProperties |} with get, set
        abstract spotLightShadows: {| value: ResizeArray<obj option>; properties: {| shadowBias: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; shadowNormalBias: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; shadowRadius: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; shadowMapSize: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection |} |} with get, set
        abstract spotShadowMap: IUniform with get, set
        abstract spotShadowMatrix: IUniform with get, set
        abstract pointLights: {| value: ResizeArray<obj option>; properties: {| color: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; position: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; decay: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; distance: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection |} |} with get, set
        abstract pointLightShadows: {| value: ResizeArray<obj option>; properties: {| shadowBias: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; shadowNormalBias: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; shadowRadius: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; shadowMapSize: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection |} |} with get, set
        abstract pointShadowMap: IUniform with get, set
        abstract pointShadowMatrix: IUniform with get, set
        abstract hemisphereLights: {| value: ResizeArray<obj option>; properties: {| direction: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; skycolor: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; groundColor: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection |} |} with get, set
        abstract rectAreaLights: {| value: ResizeArray<obj option>; properties: {| color: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; position: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; width: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection; height: IExportsUniformsLibLightsDirectionalLightsPropertiesDirection |} |} with get, set

    type [<AllowNullLiteral>] IExportsUniformsLibPoints =
        abstract diffuse: IUniform with get, set
        abstract opacity: IUniform with get, set
        abstract size: IUniform with get, set
        abstract scale: IUniform with get, set
        abstract map: IUniform with get, set
        abstract uvTransform: IUniform with get, set

    type [<AllowNullLiteral>] IExportsUniformsLib =
        abstract common: IExportsUniformsLibCommon with get, set
        abstract specularmap: {| specularMap: IUniform |} with get, set
        abstract envmap: IExportsUniformsLibEnvmap with get, set
        abstract aomap: {| aoMap: IUniform; aoMapIntensity: IUniform |} with get, set
        abstract lightmap: {| lightMap: IUniform; lightMapIntensity: IUniform |} with get, set
        abstract emissivemap: {| emissiveMap: IUniform |} with get, set
        abstract bumpmap: {| bumpMap: IUniform; bumpScale: IUniform |} with get, set
        abstract normalmap: {| normalMap: IUniform; normalScale: IUniform |} with get, set
        abstract displacementmap: {| displacementMap: IUniform; displacementScale: IUniform; displacementBias: IUniform |} with get, set
        abstract roughnessmap: {| roughnessMap: IUniform |} with get, set
        abstract metalnessmap: {| metalnessMap: IUniform |} with get, set
        abstract gradientmap: {| gradientMap: IUniform |} with get, set
        abstract fog: {| fogDensity: IUniform; fogNear: IUniform; fogFar: IUniform; fogColor: IUniform |} with get, set
        abstract lights: IExportsUniformsLibLights with get, set
        abstract points: IExportsUniformsLibPoints with get, set

module __renderers_shaders_UniformsUtils =

    type [<AllowNullLiteral>] IExports =
        abstract cloneUniforms: uniforms_src: obj option -> obj option
        abstract mergeUniforms: uniforms: ResizeArray<obj option> -> obj option

module __renderers_webgl_WebGLAttributes =
    type WebGLCapabilities = __renderers_webgl_WebGLCapabilities.WebGLCapabilities
    type BufferAttribute = __core_BufferAttribute.BufferAttribute
    type InterleavedBufferAttribute = __core_InterleavedBufferAttribute.InterleavedBufferAttribute

    type [<AllowNullLiteral>] IExports =
        abstract WebGLAttributes: WebGLAttributesStatic

    type [<AllowNullLiteral>] WebGLAttributes =
        abstract get: attribute: U2<BufferAttribute, InterleavedBufferAttribute> -> {| buffer: WebGLBuffer; ``type``: float; bytesPerElement: float; version: float |}
        abstract remove: attribute: U2<BufferAttribute, InterleavedBufferAttribute> -> unit
        abstract update: attribute: U2<BufferAttribute, InterleavedBufferAttribute> * bufferType: float -> unit

    type [<AllowNullLiteral>] WebGLAttributesStatic =
        [<EmitConstructor>] abstract Create: gl: U2<WebGLRenderingContext, WebGL2RenderingContext> * capabilities: WebGLCapabilities -> WebGLAttributes

module __renderers_webgl_WebGLBindingStates =
    type WebGLExtensions = __renderers_webgl_WebGLExtensions.WebGLExtensions
    type WebGLAttributes = __renderers_webgl_WebGLAttributes.WebGLAttributes
    type WebGLProgram = __renderers_webgl_WebGLProgram.WebGLProgram
    type WebGLCapabilities = __renderers_webgl_WebGLCapabilities.WebGLCapabilities
    type Object3D = __core_Object3D.Object3D
    type BufferGeometry = __core_BufferGeometry.BufferGeometry
    type BufferAttribute = __core_BufferAttribute.BufferAttribute
    type Material = __materials_Material.Material

    type [<AllowNullLiteral>] IExports =
        abstract WebGLBindingStates: WebGLBindingStatesStatic

    type [<AllowNullLiteral>] WebGLBindingStates =
        abstract setup: object: Object3D * material: Material * program: WebGLProgram * geometry: BufferGeometry * index: BufferAttribute -> unit
        abstract reset: unit -> unit
        abstract resetDefaultState: unit -> unit
        abstract dispose: unit -> unit
        abstract releaseStatesOfGeometry: unit -> unit
        abstract releaseStatesOfProgram: unit -> unit
        abstract initAttributes: unit -> unit
        abstract enableAttribute: attribute: float -> unit
        abstract disableUnusedAttributes: unit -> unit

    type [<AllowNullLiteral>] WebGLBindingStatesStatic =
        [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * extensions: WebGLExtensions * attributes: WebGLAttributes * capabilities: WebGLCapabilities -> WebGLBindingStates

module __renderers_webgl_WebGLBufferRenderer =
    type WebGLExtensions = __renderers_webgl_WebGLExtensions.WebGLExtensions
    type WebGLInfo = __renderers_webgl_WebGLInfo.WebGLInfo
    type WebGLCapabilities = __renderers_webgl_WebGLCapabilities.WebGLCapabilities

    type [<AllowNullLiteral>] IExports =
        abstract WebGLBufferRenderer: WebGLBufferRendererStatic

    type [<AllowNullLiteral>] WebGLBufferRenderer =
        abstract setMode: value: obj option -> unit
        abstract render: start: obj option * count: float -> unit
        abstract renderInstances: start: obj option * count: float * primcount: float -> unit

    type [<AllowNullLiteral>] WebGLBufferRendererStatic =
        [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * extensions: WebGLExtensions * info: WebGLInfo * capabilities: WebGLCapabilities -> WebGLBufferRenderer

module __renderers_webgl_WebGLCapabilities =

    type [<AllowNullLiteral>] IExports =
        abstract WebGLCapabilities: WebGLCapabilitiesStatic

    type [<AllowNullLiteral>] WebGLCapabilitiesParameters =
        abstract precision: string option with get, set
        abstract logarithmicDepthBuffer: bool option with get, set

    type [<AllowNullLiteral>] WebGLCapabilities =
        abstract isWebGL2: bool
        abstract precision: string with get, set
        abstract logarithmicDepthBuffer: bool with get, set
        abstract maxTextures: float with get, set
        abstract maxVertexTextures: float with get, set
        abstract maxTextureSize: float with get, set
        abstract maxCubemapSize: float with get, set
        abstract maxAttributes: float with get, set
        abstract maxVertexUniforms: float with get, set
        abstract maxVaryings: float with get, set
        abstract maxFragmentUniforms: float with get, set
        abstract vertexTextures: bool with get, set
        abstract floatFragmentTextures: bool with get, set
        abstract floatVertexTextures: bool with get, set
        abstract getMaxAnisotropy: unit -> float
        abstract getMaxPrecision: precision: string -> string

    type [<AllowNullLiteral>] WebGLCapabilitiesStatic =
        [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * extensions: obj option * parameters: WebGLCapabilitiesParameters -> WebGLCapabilities

module __renderers_webgl_WebGLClipping =
    type Camera = __cameras_Camera.Camera
    type Material = __materials_Material.Material
    type WebGLProperties = __renderers_webgl_WebGLProperties.WebGLProperties

    type [<AllowNullLiteral>] IExports =
        abstract WebGLClipping: WebGLClippingStatic

    type [<AllowNullLiteral>] WebGLClipping =
        abstract uniform: {| value: obj option; needsUpdate: bool |} with get, set
        /// <default>0</default>
        abstract numPlanes: float with get, set
        /// <default>0</default>
        abstract numIntersection: float with get, set
        abstract init: planes: ResizeArray<obj option> * enableLocalClipping: bool * camera: Camera -> bool
        abstract beginShadows: unit -> unit
        abstract endShadows: unit -> unit
        abstract setState: material: Material * camera: Camera * useCache: bool -> unit

    type [<AllowNullLiteral>] WebGLClippingStatic =
        [<EmitConstructor>] abstract Create: properties: WebGLProperties -> WebGLClipping

module __renderers_webgl_WebGLCubeMaps =
    type WebGLRenderer = __renderers_WebGLRenderer.WebGLRenderer

    type [<AllowNullLiteral>] IExports =
        abstract WebGLCubeMaps: WebGLCubeMapsStatic

    type [<AllowNullLiteral>] WebGLCubeMaps =
        abstract get: texture: obj option -> obj option
        abstract dispose: unit -> unit

    type [<AllowNullLiteral>] WebGLCubeMapsStatic =
        [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> WebGLCubeMaps

module __renderers_webgl_WebGLCubeUVMaps =
    type WebGLRenderer = WebGLRenderer
    type Texture = Texture

    type [<AllowNullLiteral>] IExports =
        abstract WebGLCubeUVMaps: WebGLCubeUVMapsStatic

    type [<AllowNullLiteral>] WebGLCubeUVMaps =
        abstract get: texture: 'T -> obj
        abstract dispose: unit -> unit

    type [<AllowNullLiteral>] WebGLCubeUVMapsStatic =
        [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> WebGLCubeUVMaps

module __renderers_webgl_WebGLExtensions =
    type WebGLCapabilities = __renderers_webgl_WebGLCapabilities.WebGLCapabilities

    type [<AllowNullLiteral>] IExports =
        abstract WebGLExtensions: WebGLExtensionsStatic

    type [<AllowNullLiteral>] WebGLExtensions =
        abstract has: name: string -> bool
        abstract init: capabilities: WebGLCapabilities -> unit
        abstract get: name: string -> obj option

    type [<AllowNullLiteral>] WebGLExtensionsStatic =
        [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext -> WebGLExtensions

module __renderers_webgl_WebGLGeometries =
    type WebGLAttributes = __renderers_webgl_WebGLAttributes.WebGLAttributes
    type WebGLInfo = __renderers_webgl_WebGLInfo.WebGLInfo
    type BufferAttribute = __core_BufferAttribute.BufferAttribute
    type BufferGeometry = __core_BufferGeometry.BufferGeometry
    type Object3D = __core_Object3D.Object3D

    type [<AllowNullLiteral>] IExports =
        abstract WebGLGeometries: WebGLGeometriesStatic

    type [<AllowNullLiteral>] WebGLGeometries =
        abstract get: object: Object3D * geometry: BufferGeometry -> BufferGeometry
        abstract update: geometry: BufferGeometry -> unit
        abstract getWireframeAttribute: geometry: BufferGeometry -> BufferAttribute

    type [<AllowNullLiteral>] WebGLGeometriesStatic =
        [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * attributes: WebGLAttributes * info: WebGLInfo -> WebGLGeometries

module __renderers_webgl_WebGLIndexedBufferRenderer =

    type [<AllowNullLiteral>] IExports =
        abstract WebGLIndexedBufferRenderer: WebGLIndexedBufferRendererStatic

    type [<AllowNullLiteral>] WebGLIndexedBufferRenderer =
        abstract setMode: value: obj option -> unit
        abstract setIndex: index: obj option -> unit
        abstract render: start: obj option * count: float -> unit
        abstract renderInstances: start: obj option * count: float * primcount: float -> unit

    type [<AllowNullLiteral>] WebGLIndexedBufferRendererStatic =
        [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * extensions: obj option * info: obj option * capabilities: obj option -> WebGLIndexedBufferRenderer

module __renderers_webgl_WebGLInfo =
    type WebGLProgram = __renderers_webgl_WebGLProgram.WebGLProgram

    type [<AllowNullLiteral>] IExports =
        /// An object with a series of statistical information about the graphics board memory and the rendering process.
        abstract WebGLInfo: WebGLInfoStatic

    /// An object with a series of statistical information about the graphics board memory and the rendering process.
    type [<AllowNullLiteral>] WebGLInfo =
        /// <default>true</default>
        abstract autoReset: bool with get, set
        /// <default>{ geometries: 0, textures: 0 }</default>
        abstract memory: {| geometries: float; textures: float |} with get, set
        /// <default>null</default>
        abstract programs: ResizeArray<WebGLProgram> option with get, set
        /// <default>{ frame: 0, calls: 0, triangles: 0, points: 0, lines: 0 }</default>
        abstract render: WebGLInfoRender with get, set
        abstract update: count: float * mode: float * instanceCount: float -> unit
        abstract reset: unit -> unit

    /// An object with a series of statistical information about the graphics board memory and the rendering process.
    type [<AllowNullLiteral>] WebGLInfoStatic =
        [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext -> WebGLInfo

    type [<AllowNullLiteral>] WebGLInfoRender =
        abstract calls: float with get, set
        abstract frame: float with get, set
        abstract lines: float with get, set
        abstract points: float with get, set
        abstract triangles: float with get, set

module __renderers_webgl_WebGLLights =
    type WebGLExtensions = __renderers_webgl_WebGLExtensions.WebGLExtensions
    type WebGLCapabilities = __renderers_webgl_WebGLCapabilities.WebGLCapabilities

    type [<AllowNullLiteral>] IExports =
        abstract WebGLLights: WebGLLightsStatic

    type [<AllowNullLiteral>] WebGLLights =
        abstract state: WebGLLightsState with get, set
        abstract get: light: obj option -> obj option
        abstract setup: lights: obj option -> unit
        abstract setupView: lights: obj option * camera: obj option -> unit

    type [<AllowNullLiteral>] WebGLLightsStatic =
        [<EmitConstructor>] abstract Create: extensions: WebGLExtensions * capabilities: WebGLCapabilities -> WebGLLights

    type [<AllowNullLiteral>] WebGLLightsStateHash =
        abstract directionalLength: float with get, set
        abstract pointLength: float with get, set
        abstract spotLength: float with get, set
        abstract rectAreaLength: float with get, set
        abstract hemiLength: float with get, set
        abstract numDirectionalShadows: float with get, set
        abstract numPointShadows: float with get, set
        abstract numSpotShadows: float with get, set

    type [<AllowNullLiteral>] WebGLLightsState =
        abstract version: float with get, set
        abstract hash: WebGLLightsStateHash with get, set
        abstract ambient: ResizeArray<float> with get, set
        abstract probe: ResizeArray<obj option> with get, set
        abstract directional: ResizeArray<obj option> with get, set
        abstract directionalShadow: ResizeArray<obj option> with get, set
        abstract directionalShadowMap: ResizeArray<obj option> with get, set
        abstract directionalShadowMatrix: ResizeArray<obj option> with get, set
        abstract spot: ResizeArray<obj option> with get, set
        abstract spotShadow: ResizeArray<obj option> with get, set
        abstract spotShadowMap: ResizeArray<obj option> with get, set
        abstract spotShadowMatrix: ResizeArray<obj option> with get, set
        abstract rectArea: ResizeArray<obj option> with get, set
        abstract point: ResizeArray<obj option> with get, set
        abstract pointShadow: ResizeArray<obj option> with get, set
        abstract pointShadowMap: ResizeArray<obj option> with get, set
        abstract pointShadowMatrix: ResizeArray<obj option> with get, set
        abstract hemi: ResizeArray<obj option> with get, set

module __renderers_webgl_WebGLObjects =

    type [<AllowNullLiteral>] IExports =
        abstract WebGLObjects: WebGLObjectsStatic

    type [<AllowNullLiteral>] WebGLObjects =
        abstract update: object: obj option -> obj option
        abstract dispose: unit -> unit

    type [<AllowNullLiteral>] WebGLObjectsStatic =
        [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * geometries: obj option * attributes: obj option * info: obj option -> WebGLObjects

module __renderers_webgl_WebGLProgram =
    type WebGLRenderer = __renderers_WebGLRenderer.WebGLRenderer
    // type WebGLShader = __renderers_webgl_WebGLShader.WebGLShader
    type WebGLUniforms = __renderers_webgl_WebGLUniforms.WebGLUniforms

    type [<AllowNullLiteral>] IExports =
        abstract WebGLProgram: WebGLProgramStatic

    type [<AllowNullLiteral>] WebGLProgram =
        abstract name: string with get, set
        abstract id: float with get, set
        abstract cacheKey: string with get, set
        /// <default>1</default>
        abstract usedTimes: float with get, set
        abstract program: obj option with get, set
        abstract vertexShader: WebGLShader with get, set
        abstract fragmentShader: WebGLShader with get, set
        [<Obsolete("Use {@link WebGLProgram#getUniforms getUniforms()} instead.")>]
        abstract uniforms: obj option with get, set
        [<Obsolete("Use {@link WebGLProgram#getAttributes getAttributes()} instead.")>]
        abstract attributes: obj option with get, set
        abstract getUniforms: unit -> WebGLUniforms
        abstract getAttributes: unit -> obj option
        abstract destroy: unit -> unit

    type [<AllowNullLiteral>] WebGLProgramStatic =
        [<EmitConstructor>] abstract Create: renderer: WebGLRenderer * cacheKey: string * parameters: obj -> WebGLProgram

module __renderers_webgl_WebGLPrograms =
    type WebGLRenderer = __renderers_WebGLRenderer.WebGLRenderer
    type WebGLProgram = __renderers_webgl_WebGLProgram.WebGLProgram
    type WebGLCapabilities = __renderers_webgl_WebGLCapabilities.WebGLCapabilities
    type WebGLCubeMaps = __renderers_webgl_WebGLCubeMaps.WebGLCubeMaps
    type WebGLExtensions = __renderers_webgl_WebGLExtensions.WebGLExtensions
    type WebGLClipping = __renderers_webgl_WebGLClipping.WebGLClipping
    type WebGLBindingStates = __renderers_webgl_WebGLBindingStates.WebGLBindingStates
    type Material = __materials_Material.Material
    type Scene = __scenes_Scene.Scene

    type [<AllowNullLiteral>] IExports =
        abstract WebGLPrograms: WebGLProgramsStatic

    type [<AllowNullLiteral>] WebGLPrograms =
        abstract programs: ResizeArray<WebGLProgram> with get, set
        abstract getParameters: material: Material * lights: obj option * shadows: ResizeArray<obj> * scene: Scene * object: obj option -> obj option
        abstract getProgramCacheKey: parameters: obj option -> string
        abstract getUniforms: material: Material -> obj
        abstract acquireProgram: parameters: obj option * cacheKey: string -> WebGLProgram
        abstract releaseProgram: program: WebGLProgram -> unit

    type [<AllowNullLiteral>] WebGLProgramsStatic =
        [<EmitConstructor>] abstract Create: renderer: WebGLRenderer * cubemaps: WebGLCubeMaps * extensions: WebGLExtensions * capabilities: WebGLCapabilities * bindingStates: WebGLBindingStates * clipping: WebGLClipping -> WebGLPrograms

module __renderers_webgl_WebGLProperties =

    type [<AllowNullLiteral>] IExports =
        abstract WebGLProperties: WebGLPropertiesStatic

    type [<AllowNullLiteral>] WebGLProperties =
        abstract get: object: obj option -> obj option
        abstract remove: object: obj option -> unit
        abstract update: object: obj option * key: obj option * value: obj option -> obj option
        abstract dispose: unit -> unit

    type [<AllowNullLiteral>] WebGLPropertiesStatic =
        [<EmitConstructor>] abstract Create: unit -> WebGLProperties

module __renderers_webgl_WebGLRenderLists =
    type Object3D = __core_Object3D.Object3D
    type Material = __materials_Material.Material
    type WebGLProgram = __renderers_webgl_WebGLProgram.WebGLProgram
    type Group = __objects_Group.Group
    type Scene = __scenes_Scene.Scene
    type Camera = __cameras_Camera.Camera
    type BufferGeometry = __core_BufferGeometry.BufferGeometry
    type WebGLProperties = __renderers_webgl_WebGLProperties.WebGLProperties

    type [<AllowNullLiteral>] IExports =
        abstract WebGLRenderList: WebGLRenderListStatic
        abstract WebGLRenderLists: WebGLRenderListsStatic

    type [<AllowNullLiteral>] RenderTarget =
        interface end

    type [<AllowNullLiteral>] RenderItem =
        abstract id: float with get, set
        abstract object: Object3D with get, set
        abstract geometry: BufferGeometry option with get, set
        abstract material: Material with get, set
        abstract program: WebGLProgram with get, set
        abstract groupOrder: float with get, set
        abstract renderOrder: float with get, set
        abstract z: float with get, set
        abstract group: Group option with get, set

    type [<AllowNullLiteral>] WebGLRenderList =
        /// <default>[]</default>
        abstract opaque: ResizeArray<RenderItem> with get, set
        /// <default>[]</default>
        abstract transparent: ResizeArray<RenderItem> with get, set
        abstract init: unit -> unit
        abstract push: object: Object3D * geometry: BufferGeometry option * material: Material * groupOrder: float * z: float * group: Group option -> unit
        abstract unshift: object: Object3D * geometry: BufferGeometry option * material: Material * groupOrder: float * z: float * group: Group option -> unit
        abstract sort: opaqueSort: (obj option -> obj option -> float) * transparentSort: (obj option -> obj option -> float) -> unit
        abstract finish: unit -> unit

    type [<AllowNullLiteral>] WebGLRenderListStatic =
        [<EmitConstructor>] abstract Create: properties: WebGLProperties -> WebGLRenderList

    type [<AllowNullLiteral>] WebGLRenderLists =
        abstract dispose: unit -> unit
        abstract get: scene: Scene * camera: Camera -> WebGLRenderList

    type [<AllowNullLiteral>] WebGLRenderListsStatic =
        [<EmitConstructor>] abstract Create: properties: WebGLProperties -> WebGLRenderLists

module __renderers_webgl_WebGLShader =

    type [<AllowNullLiteral>] IExports =
        abstract WebGLShader: gl: WebGLRenderingContext * ``type``: string * string: string -> WebGLShader

module __renderers_webgl_WebGLShadowMap =
    type WebGLCapabilities = __renderers_webgl_WebGLCapabilities.WebGLCapabilities
    type Scene = __scenes_Scene.Scene
    type Camera = __cameras_Camera.Camera
    type WebGLRenderer = __renderers_WebGLRenderer.WebGLRenderer
    type ShadowMapType = Constants.ShadowMapType
    type WebGLObjects = __renderers_webgl_WebGLObjects.WebGLObjects
    type Light = __lights_Light.Light

    type [<AllowNullLiteral>] IExports =
        abstract WebGLShadowMap: WebGLShadowMapStatic

    type [<AllowNullLiteral>] WebGLShadowMap =
        /// <default>false</default>
        abstract enabled: bool with get, set
        /// <default>true</default>
        abstract autoUpdate: bool with get, set
        /// <default>false</default>
        abstract needsUpdate: bool with get, set
        /// <default>THREE.PCFShadowMap</default>
        abstract ``type``: ShadowMapType with get, set
        abstract render: shadowsArray: ResizeArray<Light> * scene: Scene * camera: Camera -> unit
        [<Obsolete("Use {@link Material#shadowSide} instead.")>]
        abstract cullFace: obj option with get, set

    type [<AllowNullLiteral>] WebGLShadowMapStatic =
        [<EmitConstructor>] abstract Create: _renderer: WebGLRenderer * _objects: WebGLObjects * _capabilities: WebGLCapabilities -> WebGLShadowMap

module __renderers_webgl_WebGLState =
    type CullFace = Constants.CullFace
    type Blending = Constants.Blending
    type BlendingEquation = Constants.BlendingEquation
    type BlendingSrcFactor = Constants.BlendingSrcFactor
    type BlendingDstFactor = Constants.BlendingDstFactor
    type DepthModes = Constants.DepthModes
    type WebGLCapabilities = __renderers_webgl_WebGLCapabilities.WebGLCapabilities
    type WebGLExtensions = __renderers_webgl_WebGLExtensions.WebGLExtensions
    type Material = __materials_Material.Material
    type Vector4 = __math_Vector4.Vector4

    type [<AllowNullLiteral>] IExports =
        abstract WebGLColorBuffer: WebGLColorBufferStatic
        abstract WebGLDepthBuffer: WebGLDepthBufferStatic
        abstract WebGLStencilBuffer: WebGLStencilBufferStatic
        abstract WebGLState: WebGLStateStatic

    type [<AllowNullLiteral>] WebGLColorBuffer =
        abstract setMask: colorMask: bool -> unit
        abstract setLocked: lock: bool -> unit
        abstract setClear: r: float * g: float * b: float * a: float * premultipliedAlpha: bool -> unit
        abstract reset: unit -> unit

    type [<AllowNullLiteral>] WebGLColorBufferStatic =
        [<EmitConstructor>] abstract Create: unit -> WebGLColorBuffer

    type [<AllowNullLiteral>] WebGLDepthBuffer =
        abstract setTest: depthTest: bool -> unit
        abstract setMask: depthMask: bool -> unit
        abstract setFunc: depthFunc: DepthModes -> unit
        abstract setLocked: lock: bool -> unit
        abstract setClear: depth: float -> unit
        abstract reset: unit -> unit

    type [<AllowNullLiteral>] WebGLDepthBufferStatic =
        [<EmitConstructor>] abstract Create: unit -> WebGLDepthBuffer

    type [<AllowNullLiteral>] WebGLStencilBuffer =
        abstract setTest: stencilTest: bool -> unit
        abstract setMask: stencilMask: float -> unit
        abstract setFunc: stencilFunc: float * stencilRef: float * stencilMask: float -> unit
        abstract setOp: stencilFail: float * stencilZFail: float * stencilZPass: float -> unit
        abstract setLocked: lock: bool -> unit
        abstract setClear: stencil: float -> unit
        abstract reset: unit -> unit

    type [<AllowNullLiteral>] WebGLStencilBufferStatic =
        [<EmitConstructor>] abstract Create: unit -> WebGLStencilBuffer

    type [<AllowNullLiteral>] WebGLState =
        abstract buffers: {| color: WebGLColorBuffer; depth: WebGLDepthBuffer; stencil: WebGLStencilBuffer |} with get, set
        abstract initAttributes: unit -> unit
        abstract enableAttribute: attribute: float -> unit
        abstract enableAttributeAndDivisor: attribute: float * meshPerAttribute: float -> unit
        abstract disableUnusedAttributes: unit -> unit
        abstract vertexAttribPointer: index: float * size: float * ``type``: float * normalized: bool * stride: float * offset: float -> unit
        abstract enable: id: float -> unit
        abstract disable: id: float -> unit
        abstract bindFramebuffer: target: float * framebuffer: WebGLFramebuffer option -> unit
        abstract bindXRFramebuffer: framebuffer: WebGLFramebuffer option -> unit
        abstract useProgram: program: obj option -> bool
        abstract setBlending: blending: Blending * ?blendEquation: BlendingEquation * ?blendSrc: BlendingSrcFactor * ?blendDst: BlendingDstFactor * ?blendEquationAlpha: BlendingEquation * ?blendSrcAlpha: BlendingSrcFactor * ?blendDstAlpha: BlendingDstFactor * ?premultiplyAlpha: bool -> unit
        abstract setMaterial: material: Material * frontFaceCW: bool -> unit
        abstract setFlipSided: flipSided: bool -> unit
        abstract setCullFace: cullFace: CullFace -> unit
        abstract setLineWidth: width: float -> unit
        abstract setPolygonOffset: polygonoffset: bool * ?factor: float * ?units: float -> unit
        abstract setScissorTest: scissorTest: bool -> unit
        abstract activeTexture: webglSlot: float -> unit
        abstract bindTexture: webglType: float * webglTexture: obj option -> unit
        abstract unbindTexture: unit -> unit
        abstract compressedTexImage2D: target: float * level: float * internalformat: float * width: float * height: float * border: float * data: ArrayBufferView -> unit
        abstract texImage2D: target: float * level: float * internalformat: float * width: float * height: float * border: float * format: float * ``type``: float * pixels: ArrayBufferView option -> unit
        abstract texImage2D: target: float * level: float * internalformat: float * format: float * ``type``: float * source: obj option -> unit
        abstract texImage3D: target: float * level: float * internalformat: float * width: float * height: float * depth: float * border: float * format: float * ``type``: float * pixels: obj option -> unit
        abstract scissor: scissor: Vector4 -> unit
        abstract viewport: viewport: Vector4 -> unit
        abstract reset: unit -> unit

    type [<AllowNullLiteral>] WebGLStateStatic =
        [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * extensions: WebGLExtensions * capabilities: WebGLCapabilities -> WebGLState

module __renderers_webgl_WebGLTextures =
    type WebGLExtensions = __renderers_webgl_WebGLExtensions.WebGLExtensions
    type WebGLState = __renderers_webgl_WebGLState.WebGLState
    type WebGLProperties = __renderers_webgl_WebGLProperties.WebGLProperties
    type WebGLCapabilities = __renderers_webgl_WebGLCapabilities.WebGLCapabilities
    type WebGLUtils = __renderers_webgl_WebGLUtils.WebGLUtils
    type WebGLInfo = __renderers_webgl_WebGLInfo.WebGLInfo

    type [<AllowNullLiteral>] IExports =
        abstract WebGLTextures: WebGLTexturesStatic

    type [<AllowNullLiteral>] WebGLTextures =
        abstract allocateTextureUnit: unit -> unit
        abstract resetTextureUnits: unit -> unit
        abstract setTexture2D: texture: obj option * slot: float -> unit
        abstract setTexture2DArray: texture: obj option * slot: float -> unit
        abstract setTexture3D: texture: obj option * slot: float -> unit
        abstract setTextureCube: texture: obj option * slot: float -> unit
        abstract setupRenderTarget: renderTarget: obj option -> unit
        abstract updateRenderTargetMipmap: renderTarget: obj option -> unit
        abstract updateMultisampleRenderTarget: renderTarget: obj option -> unit
        abstract safeSetTexture2D: texture: obj option * slot: float -> unit
        abstract safeSetTextureCube: texture: obj option * slot: float -> unit

    type [<AllowNullLiteral>] WebGLTexturesStatic =
        [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * extensions: WebGLExtensions * state: WebGLState * properties: WebGLProperties * capabilities: WebGLCapabilities * utils: WebGLUtils * info: WebGLInfo -> WebGLTextures

module __renderers_webgl_WebGLUniforms =
    type WebGLProgram = __renderers_webgl_WebGLProgram.WebGLProgram
    type WebGLTextures = __renderers_webgl_WebGLTextures.WebGLTextures

    type [<AllowNullLiteral>] IExports =
        abstract WebGLUniforms: WebGLUniformsStatic

    type [<AllowNullLiteral>] WebGLUniforms =
        abstract setValue: gl: WebGLRenderingContext * name: string * value: obj option * textures: WebGLTextures -> unit
        abstract setOptional: gl: WebGLRenderingContext * object: obj option * name: string -> unit

    type [<AllowNullLiteral>] WebGLUniformsStatic =
        [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * program: WebGLProgram -> WebGLUniforms
        abstract upload: gl: WebGLRenderingContext * seq: obj option * values: ResizeArray<obj option> * textures: WebGLTextures -> unit
        abstract seqWithValue: seq: obj option * values: ResizeArray<obj option> -> ResizeArray<obj option>

module __renderers_webgl_WebGLUtils =

    type [<AllowNullLiteral>] IExports =
        abstract WebGLUtils: WebGLUtilsStatic

    type [<AllowNullLiteral>] WebGLUtils =
        abstract convert: p: obj option -> unit

    type [<AllowNullLiteral>] WebGLUtilsStatic =
        [<EmitConstructor>] abstract Create: gl: U2<WebGLRenderingContext, WebGL2RenderingContext> * extensions: obj option * capabilities: obj option -> WebGLUtils

module __renderers_webxr_WebXR =

    type [<AllowNullLiteral>] IExports =
        abstract XRWebGLLayer: XRWebGLLayerStatic
        abstract XRRigidTransform: XRRigidTransformStatic
        abstract XRRay: XRRayStatic
        abstract Constructor: ConstructorStatic

    type [<StringEnum>] [<RequireQualifiedAccess>] XRSessionMode =
        | Inline
        | [<CompiledName("immersive-vr")>] ImmersiveVr
        | [<CompiledName("immersive-ar")>] ImmersiveAr

    type [<StringEnum>] [<RequireQualifiedAccess>] XRReferenceSpaceType =
        | Viewer
        | Local
        | [<CompiledName("local-floor")>] LocalFloor
        | [<CompiledName("bounded-floor")>] BoundedFloor
        | Unbounded

    type [<StringEnum>] [<RequireQualifiedAccess>] XREnvironmentBlendMode =
        | Opaque
        | Additive
        | [<CompiledName("alpha-blend")>] AlphaBlend

    type [<StringEnum>] [<RequireQualifiedAccess>] XRVisibilityState =
        | Visible
        | [<CompiledName("visible-blurred")>] VisibleBlurred
        | Hidden

    type [<StringEnum>] [<RequireQualifiedAccess>] XRHandedness =
        | None
        | Left
        | Right

    type [<StringEnum>] [<RequireQualifiedAccess>] XRTargetRayMode =
        | Gaze
        | [<CompiledName("tracked-pointer")>] TrackedPointer
        | Screen

    type [<StringEnum>] [<RequireQualifiedAccess>] XREye =
        | None
        | Left
        | Right

    type [<StringEnum>] [<RequireQualifiedAccess>] XREventType =
        | End
        | Select
        | Selectstart
        | Selectend
        | Squeeze
        | Squeezestart
        | Squeezeend
        | Inputsourceschange

    type [<AllowNullLiteral>] XRAnimationLoopCallback =
        [<Emit("$0($1...)")>] abstract Invoke: time: float * ?frame: XRFrame -> unit

    type [<AllowNullLiteral>] XRFrameRequestCallback =
        [<Emit("$0($1...)")>] abstract Invoke: time: float * frame: XRFrame -> unit

    type [<AllowNullLiteral>] XR =
        inherit EventTarget
        abstract requestSession: mode: XRSessionMode * ?options: XRSessionInit -> Promise<XRSession>
        abstract isSessionSupported: mode: XRSessionMode -> Promise<bool>

    type [<AllowNullLiteral>] Window =
        abstract XRSession: Constructor<XRSession> option with get, set
        abstract XR: Constructor<XR> option with get, set

    type [<AllowNullLiteral>] Navigator =
        abstract xr: XR option with get, set

    type [<AllowNullLiteral>] XRReferenceSpace =
        inherit EventTarget
        abstract getOffsetReferenceSpace: originOffset: XRRigidTransform -> XRReferenceSpace
        abstract onreset: obj option with get, set

    type [<AllowNullLiteral>] XRHitTestOptionsInit =
        abstract space: EventTarget with get, set
        abstract entityTypes: ResizeArray<XRHitTestTrackableType> option with get, set
        abstract offsetRay: XRRay option with get, set

    type [<AllowNullLiteral>] XRTransientInputHitTestOptionsInit =
        abstract profile: string with get, set
        abstract entityTypes: ResizeArray<XRHitTestTrackableType> option with get, set
        abstract offsetRay: XRRay option with get, set

    type [<AllowNullLiteral>] XRViewport =
        abstract x: float
        abstract y: float
        abstract width: float
        abstract height: float

    type [<AllowNullLiteral>] WebGLRenderingContext =
        abstract makeXRCompatible: unit -> Promise<unit>

    type [<AllowNullLiteral>] XRRenderState =
        abstract depthNear: float
        abstract depthFar: float
        abstract inlineVerticalFieldOfView: float option
        abstract baseLayer: XRWebGLLayer option

    type [<AllowNullLiteral>] XRRenderStateInit =
        abstract depthNear: float option with get, set
        abstract depthFar: float option with get, set
        abstract inlineVerticalFieldOfView: float option with get, set
        abstract baseLayer: XRWebGLLayer option with get, set

    type [<AllowNullLiteral>] XRGamepad =
        abstract id: string
        abstract index: float
        abstract connected: bool
        abstract timestamp: DOMHighResTimeStamp
        abstract mapping: GamepadMappingType
        abstract axes: Float32Array
        abstract buttons: ResizeArray<GamepadButton>

    type [<AllowNullLiteral>] XRInputSource =
        abstract handedness: XRHandedness
        abstract targetRayMode: XRTargetRayMode
        abstract targetRaySpace: EventTarget
        abstract gripSpace: EventTarget option
        abstract profiles: ResizeArray<string>
        abstract gamepad: XRGamepad
        abstract hand: XRHand option

    type [<AllowNullLiteral>] XRSessionInit =
        abstract optionalFeatures: ResizeArray<string> option with get, set
        abstract requiredFeatures: ResizeArray<string> option with get, set

    type [<AllowNullLiteral>] XRSession =
        inherit EventTarget
        abstract requestReferenceSpace: ``type``: XRReferenceSpaceType -> Promise<XRReferenceSpace>
        abstract updateRenderState: renderStateInit: XRRenderStateInit -> Promise<unit>
        abstract requestAnimationFrame: callback: XRFrameRequestCallback -> float
        abstract cancelAnimationFrame: id: float -> unit
        abstract ``end``: unit -> Promise<unit>
        abstract renderState: XRRenderState with get, set
        abstract inputSources: ResizeArray<XRInputSource> with get, set
        abstract environmentBlendMode: XREnvironmentBlendMode with get, set
        abstract visibilityState: XRVisibilityState with get, set
        abstract requestHitTestSource: options: XRHitTestOptionsInit -> Promise<XRHitTestSource>
        abstract requestHitTestSourceForTransientInput: options: XRTransientInputHitTestOptionsInit -> Promise<XRTransientInputHitTestSource>
        abstract requestHitTest: ray: XRRay * referenceSpace: XRReferenceSpace -> Promise<ResizeArray<XRHitResult>>
        abstract updateWorldTrackingState: options: {| planeDetectionState: {| enabled: bool |} option |} -> unit
// { Attributes = []  Comments = []  Name = XRPlaneSet  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/renderers/webxr/WebXR".XRPlaneSet  Type = Generic ({ Type = Mapped ({ Name = Set  FullName =   Declarations = [object Object] })  TypeParameters = [Mapped ({ Name = XRPlane  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/renderers/webxr/WebXR".XRPlane  Declarations = [object Object] })] })  TypeParameters = [] }
// { Attributes = []  Comments = []  Name = XRAnchorSet  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/renderers/webxr/WebXR".XRAnchorSet  Type = Generic ({ Type = Mapped ({ Name = Set  FullName =   Declarations = [object Object] })  TypeParameters = [Mapped ({ Name = XRAnchor  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/renderers/webxr/WebXR".XRAnchor  Declarations = [object Object] })] })  TypeParameters = [] }

    type [<AllowNullLiteral>] XRFrame =
        abstract session: XRSession
        abstract getViewerPose: referenceSpace: XRReferenceSpace -> XRViewerPose option
        abstract getPose: space: EventTarget * baseSpace: EventTarget -> XRPose option
        abstract getHitTestResults: hitTestSource: XRHitTestSource -> ResizeArray<XRHitTestResult>
        abstract getHitTestResultsForTransientInput: hitTestSource: XRTransientInputHitTestSource -> ResizeArray<XRTransientInputHitTestResult>
        abstract trackedAnchors: XRAnchorSet option with get, set
        abstract createAnchor: pose: XRRigidTransform * space: EventTarget -> Promise<XRAnchor>
        abstract worldInformation: {| detectedPlanes: XRPlaneSet option |} with get, set
        abstract getJointPose: joint: XRJointSpace * baseSpace: EventTarget -> XRJointPose

    type [<AllowNullLiteral>] XRViewerPose =
        abstract transform: XRRigidTransform
        abstract views: ResizeArray<XRView>

    type [<AllowNullLiteral>] XRPose =
        abstract emulatedPosition: bool
        abstract transform: XRRigidTransform

    type [<AllowNullLiteral>] XRWebGLLayerInit =
        abstract antialias: bool option with get, set
        abstract depth: bool option with get, set
        abstract stencil: bool option with get, set
        abstract alpha: bool option with get, set
        abstract ignoreDepthValues: bool option with get, set
        abstract framebufferScaleFactor: float option with get, set

    type [<AllowNullLiteral>] XRWebGLLayer =
        abstract framebuffer: WebGLFramebuffer with get, set
        abstract framebufferWidth: float with get, set
        abstract framebufferHeight: float with get, set
        abstract getViewport: view: XRView -> XRViewport

    type [<AllowNullLiteral>] XRWebGLLayerStatic =
        [<EmitConstructor>] abstract Create: session: XRSession * gl: WebGLRenderingContext option * ?options: XRWebGLLayerInit -> XRWebGLLayer

    type [<AllowNullLiteral>] DOMPointInit =
        abstract w: float option with get, set
        abstract x: float option with get, set
        abstract y: float option with get, set
        abstract z: float option with get, set

    type [<AllowNullLiteral>] XRRigidTransform =
        abstract position: DOMPointReadOnly with get, set
        abstract orientation: DOMPointReadOnly with get, set
        abstract matrix: Float32Array with get, set
        abstract inverse: XRRigidTransform with get, set

    type [<AllowNullLiteral>] XRRigidTransformStatic =
        [<EmitConstructor>] abstract Create: matrix: U2<Float32Array, DOMPointInit> * ?direction: DOMPointInit -> XRRigidTransform

    type [<AllowNullLiteral>] XRView =
        abstract eye: XREye
        abstract projectionMatrix: Float32Array
        abstract viewMatrix: Float32Array
        abstract transform: XRRigidTransform

    type [<AllowNullLiteral>] XRRayDirectionInit =
        abstract x: float option with get, set
        abstract y: float option with get, set
        abstract z: float option with get, set
        abstract w: float option with get, set

    type [<AllowNullLiteral>] XRRay =
        abstract origin: DOMPointReadOnly
        abstract direction: XRRayDirectionInit
        abstract matrix: Float32Array with get, set

    type [<AllowNullLiteral>] XRRayStatic =
        [<EmitConstructor>] abstract Create: transformOrOrigin: U2<XRRigidTransform, DOMPointInit> * ?direction: XRRayDirectionInit -> XRRay

    type [<RequireQualifiedAccess>] XRHitTestTrackableType =
        | Point = 0
        | Plane = 1
        | Mesh = 2

    type [<AllowNullLiteral>] XRHitResult =
        abstract hitMatrix: Float32Array with get, set

    type [<AllowNullLiteral>] XRTransientInputHitTestResult =
        abstract inputSource: XRInputSource
        abstract results: ResizeArray<XRHitTestResult>

    type [<AllowNullLiteral>] XRHitTestResult =
        abstract getPose: baseSpace: EventTarget -> XRPose option
        abstract createAnchor: pose: XRRigidTransform -> Promise<XRAnchor>

    type [<AllowNullLiteral>] XRHitTestSource =
        abstract cancel: unit -> unit

    type [<AllowNullLiteral>] XRTransientInputHitTestSource =
        abstract cancel: unit -> unit

    type [<AllowNullLiteral>] XRAnchor =
        abstract anchorSpace: EventTarget with get, set
        abstract delete: unit -> unit

    type [<AllowNullLiteral>] XRPlane =
        abstract orientation: XRPlaneOrientation with get, set
        abstract planeSpace: EventTarget with get, set
        abstract polygon: ResizeArray<DOMPointReadOnly> with get, set
        abstract lastChangedTime: float with get, set

    type [<RequireQualifiedAccess>] XRHandJoint =
        | Wrist = 0
        | ThumbMetacarpal = 1
        | ThumbPhalanxProximal = 2
        | ThumbPhalanxDistal = 3
        | ThumbTip = 4
        | IndexFingerMetacarpal = 5
        | IndexFingerPhalanxProximal = 6
        | IndexFingerPhalanxIntermediate = 7
        | IndexFingerPhalanxDistal = 8
        | IndexFingerTip = 9
        | MiddleFingerMetacarpal = 10
        | MiddleFingerPhalanxProximal = 11
        | MiddleFingerPhalanxIntermediate = 12
        | MiddleFingerPhalanxDistal = 13
        | MiddleFingerTip = 14
        | RingFingerMetacarpal = 15
        | RingFingerPhalanxProximal = 16
        | RingFingerPhalanxIntermediate = 17
        | RingFingerPhalanxDistal = 18
        | RingFingerTip = 19
        | PinkyFingerMetacarpal = 20
        | PinkyFingerPhalanxProximal = 21
        | PinkyFingerPhalanxIntermediate = 22
        | PinkyFingerPhalanxDistal = 23
        | PinkyFingerTip = 24

    type [<AllowNullLiteral>] XRJointSpace =
        inherit EventTarget
        abstract jointName: XRHandJoint

    type [<AllowNullLiteral>] XRJointPose =
        inherit XRPose
        abstract radius: float option

    type [<AllowNullLiteral>] XRHand =
        inherit Map<XRHandJoint, XRJointSpace>
        abstract size: float
// { Attributes = []  Comments = []  Name = Constructor  FullName = "/home/dave/back-up/code/FableGame/node_modules/@types/three/src/renderers/webxr/WebXR".Constructor  Type = Generic ({ Type = Mapped ({ Name = Constructor  FullName = Constructor  Declarations = [object Object] })  TypeParameters = [Mapped ({ Name = obj  FullName = obj  Declarations = [object Object] })] })  TypeParameters = [] }

    type [<AllowNullLiteral>] Constructor<'T> =
        abstract prototype: 'T with get, set

    type [<AllowNullLiteral>] ConstructorStatic =
        [<EmitConstructor>] abstract Create: [<ParamArray>] args: obj option[] -> Constructor<'T>

    type [<AllowNullLiteral>] XRInputSourceChangeEvent =
        abstract session: XRSession with get, set
        abstract removed: ResizeArray<XRInputSource> with get, set
        abstract added: ResizeArray<XRInputSource> with get, set

    type [<AllowNullLiteral>] XRInputSourceEvent =
        inherit Event
        abstract frame: XRFrame
        abstract inputSource: XRInputSource

    type [<StringEnum>] [<RequireQualifiedAccess>] XRPlaneOrientation =
        | [<CompiledName("Horizontal")>] Horizontal
        | [<CompiledName("Vertical")>] Vertical

module __renderers_webxr_WebXRController =
    type Group = __objects_Group.Group
    type XREventType = __renderers_webxr_WebXR.XREventType
    type XRFrame = __renderers_webxr_WebXR.XRFrame
    type XRInputSource = __renderers_webxr_WebXR.XRInputSource
    type XRReferenceSpace = __renderers_webxr_WebXR.XRReferenceSpace

    type [<AllowNullLiteral>] IExports =
        abstract WebXRController: WebXRControllerStatic

    type XRControllerEventType =
        U2<XREventType, string>

    type [<AllowNullLiteral>] WebXRController =
        abstract getTargetRaySpace: unit -> Group
        abstract getGripSpace: unit -> Group
        abstract dispatchEvent: ``event``: {| ``type``: XRControllerEventType; data: XRInputSource option |} -> WebXRController
        abstract disconnect: inputSource: XRInputSource -> WebXRController
        abstract update: inputSource: XRInputSource * frame: XRFrame * referenceSpace: XRReferenceSpace -> WebXRController

    type [<AllowNullLiteral>] WebXRControllerStatic =
        [<EmitConstructor>] abstract Create: unit -> WebXRController

module __renderers_webxr_WebXRManager =
    type Group = __objects_Group.Group
    type Camera = __cameras_Camera.Camera
    type EventDispatcher = __core_EventDispatcher.EventDispatcher
    type XRFrameRequestCallback = __renderers_webxr_WebXR.XRFrameRequestCallback
    type XRReferenceSpace = __renderers_webxr_WebXR.XRReferenceSpace
    type XRReferenceSpaceType = __renderers_webxr_WebXR.XRReferenceSpaceType
    type XRSession = __renderers_webxr_WebXR.XRSession

    type [<AllowNullLiteral>] IExports =
        abstract WebXRManager: WebXRManagerStatic

    type [<AllowNullLiteral>] WebXRManager =
        inherit EventDispatcher
        /// <default>false</default>
        abstract enabled: bool with get, set
        /// <default>false</default>
        abstract isPresenting: bool with get, set
        abstract getController: index: float -> Group
        abstract getControllerGrip: index: float -> Group
        abstract getHand: index: float -> Group
        abstract setFramebufferScaleFactor: value: float -> unit
        abstract setReferenceSpaceType: value: XRReferenceSpaceType -> unit
        abstract getReferenceSpace: unit -> XRReferenceSpace option
        abstract getSession: unit -> XRSession option
        abstract setSession: value: XRSession -> Promise<unit>
        abstract getCamera: camera: Camera -> Camera
        abstract setAnimationLoop: callback: XRFrameRequestCallback -> unit
        abstract getFoveation: unit -> float option
        abstract setFoveation: foveation: float -> unit
        abstract dispose: unit -> unit

    type [<AllowNullLiteral>] WebXRManagerStatic =
        [<EmitConstructor>] abstract Create: renderer: obj option * gl: WebGLRenderingContext -> WebXRManager



module __lights_AmbientLight =
    type ColorRepresentation = Utils.ColorRepresentation
    type Light = __lights_Light.Light

    type [<AllowNullLiteral>] IExports =
        /// <summary>This light's color gets applied to all the objects in the scene globally.</summary>
        abstract AmbientLight: AmbientLightStatic

    /// <summary>This light's color gets applied to all the objects in the scene globally.</summary>
    type [<AllowNullLiteral>] AmbientLight =
        inherit Light
        /// <default>'AmbientLight'</default>
        abstract ``type``: string with get, set
        abstract isAmbientLight: bool

    /// <summary>This light's color gets applied to all the objects in the scene globally.</summary>
    type [<AllowNullLiteral>] AmbientLightStatic =
        /// <summary>This creates a Ambientlight with a color.</summary>
        /// <param name="color">Numeric value of the RGB component of the color or a Color instance.</param>
        /// <param name="intensity" />
        [<EmitConstructor>] abstract Create: ?color: ColorRepresentation * ?intensity: float -> AmbientLight

module __lights_AmbientLightProbe =
    type ColorRepresentation = Utils.ColorRepresentation
    type LightProbe = __lights_LightProbe.LightProbe

    type [<AllowNullLiteral>] IExports =
        abstract AmbientLightProbe: AmbientLightProbeStatic

    type [<AllowNullLiteral>] AmbientLightProbe =
        inherit LightProbe
        abstract isAmbientLightProbe: bool

    type [<AllowNullLiteral>] AmbientLightProbeStatic =
        [<EmitConstructor>] abstract Create: ?color: ColorRepresentation * ?intensity: float -> AmbientLightProbe

module __lights_DirectionalLight =
    type Object3D = __core_Object3D.Object3D
    type DirectionalLightShadow = __lights_DirectionalLightShadow.DirectionalLightShadow
    type Light = __lights_Light.Light
    type Vector3 = __math_Vector3.Vector3
    type ColorRepresentation = Utils.ColorRepresentation

    type [<AllowNullLiteral>] IExports =
        /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/lights/DirectionalLight.js">src/lights/DirectionalLight.js</see></summary>
        /// <example>
        /// // White directional light at half intensity shining from the top.
        /// const directionalLight = new THREE.DirectionalLight( 0xffffff, 0.5 );
        /// directionalLight.position.set( 0, 1, 0 );
        /// scene.add( directionalLight );
        /// </example>
        abstract DirectionalLight: DirectionalLightStatic

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/lights/DirectionalLight.js">src/lights/DirectionalLight.js</see></summary>
    /// <example>
    /// // White directional light at half intensity shining from the top.
    /// const directionalLight = new THREE.DirectionalLight( 0xffffff, 0.5 );
    /// directionalLight.position.set( 0, 1, 0 );
    /// scene.add( directionalLight );
    /// </example>
    type [<AllowNullLiteral>] DirectionalLight =
        inherit Light
        /// <default>'DirectionalLight'</default>
        abstract ``type``: string with get, set
        /// <default>THREE.Object3D.DefaultUp</default>
        abstract position: Vector3 with get, set
        /// <summary>Target used for shadow camera orientation.</summary>
        /// <default>new THREE.Object3D()</default>
        abstract target: Object3D with get, set
        /// <summary>Light's intensity.</summary>
        /// <default>1</default>
        abstract intensity: float with get, set
        /// <default>new THREE.DirectionalLightShadow()</default>
        abstract shadow: DirectionalLightShadow with get, set
        abstract isDirectionalLight: bool

    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/lights/DirectionalLight.js">src/lights/DirectionalLight.js</see></summary>
    /// <example>
    /// // White directional light at half intensity shining from the top.
    /// const directionalLight = new THREE.DirectionalLight( 0xffffff, 0.5 );
    /// directionalLight.position.set( 0, 1, 0 );
    /// scene.add( directionalLight );
    /// </example>
    type [<AllowNullLiteral>] DirectionalLightStatic =
        [<EmitConstructor>] abstract Create: ?color: ColorRepresentation * ?intensity: float -> DirectionalLight

module __lights_DirectionalLightShadow =
    type OrthographicCamera = __cameras_OrthographicCamera.OrthographicCamera
    type LightShadow = __lights_LightShadow.LightShadow

    type [<AllowNullLiteral>] IExports =
        abstract DirectionalLightShadow: DirectionalLightShadowStatic

    type [<AllowNullLiteral>] DirectionalLightShadow =
        inherit LightShadow
        abstract camera: OrthographicCamera with get, set
        abstract isDirectionalLightShadow: bool

    type [<AllowNullLiteral>] DirectionalLightShadowStatic =
        [<EmitConstructor>] abstract Create: unit -> DirectionalLightShadow

module __lights_HemisphereLight =
    type Color = __math_Color.Color
    type Vector3 = __math_Vector3.Vector3
    type Light = __lights_Light.Light
    type ColorRepresentation = Utils.ColorRepresentation

    type [<AllowNullLiteral>] IExports =
        abstract HemisphereLight: HemisphereLightStatic

    type [<AllowNullLiteral>] HemisphereLight =
        inherit Light
        /// <default>'HemisphereLight'</default>
        abstract ``type``: string with get, set
        /// <default>THREE.Object3D.DefaultUp</default>
        abstract position: Vector3 with get, set
        abstract groundColor: Color with get, set
        abstract isHemisphereLight: bool

    type [<AllowNullLiteral>] HemisphereLightStatic =
        /// <param name="skyColor" />
        /// <param name="groundColor" />
        /// <param name="intensity" />
        [<EmitConstructor>] abstract Create: ?skyColor: ColorRepresentation * ?groundColor: ColorRepresentation * ?intensity: float -> HemisphereLight

module __lights_HemisphereLightProbe =
    type ColorRepresentation = Utils.ColorRepresentation
    type LightProbe = __lights_LightProbe.LightProbe

    type [<AllowNullLiteral>] IExports =
        abstract HemisphereLightProbe: HemisphereLightProbeStatic

    type [<AllowNullLiteral>] HemisphereLightProbe =
        inherit LightProbe
        abstract isHemisphereLightProbe: bool

    type [<AllowNullLiteral>] HemisphereLightProbeStatic =
        [<EmitConstructor>] abstract Create: ?skyColor: ColorRepresentation * ?groundColor: ColorRepresentation * ?intensity: float -> HemisphereLightProbe

module __lights_Light =
    type Color = __math_Color.Color
    type LightShadow = __lights_LightShadow.LightShadow
    type Object3D = __core_Object3D.Object3D

    type [<AllowNullLiteral>] IExports =
        /// Abstract base class for lights.
        abstract Light: LightStatic

    /// Abstract base class for lights.
    type [<AllowNullLiteral>] Light =
        inherit Object3D
        /// <default>'Light'</default>
        abstract ``type``: string with get, set
        abstract color: Color with get, set
        /// <default>1</default>
        abstract intensity: float with get, set
        abstract isLight: bool
        abstract shadow: LightShadow with get, set
        [<Obsolete("Use shadow.camera.fov instead.")>]
        abstract shadowCameraFov: obj option with get, set
        [<Obsolete("Use shadow.camera.left instead.")>]
        abstract shadowCameraLeft: obj option with get, set
        [<Obsolete("Use shadow.camera.right instead.")>]
        abstract shadowCameraRight: obj option with get, set
        [<Obsolete("Use shadow.camera.top instead.")>]
        abstract shadowCameraTop: obj option with get, set
        [<Obsolete("Use shadow.camera.bottom instead.")>]
        abstract shadowCameraBottom: obj option with get, set
        [<Obsolete("Use shadow.camera.near instead.")>]
        abstract shadowCameraNear: obj option with get, set
        [<Obsolete("Use shadow.camera.far instead.")>]
        abstract shadowCameraFar: obj option with get, set
        [<Obsolete("Use shadow.bias instead.")>]
        abstract shadowBias: obj option with get, set
        [<Obsolete("Use shadow.mapSize.width instead.")>]
        abstract shadowMapWidth: obj option with get, set
        [<Obsolete("Use shadow.mapSize.height instead.")>]
        abstract shadowMapHeight: obj option with get, set
        abstract dispose: unit -> unit

    /// Abstract base class for lights.
    type [<AllowNullLiteral>] LightStatic =
        [<EmitConstructor>] abstract Create: ?hex: U2<float, string> * ?intensity: float -> Light

module __lights_LightProbe =
    type SphericalHarmonics3 = __math_SphericalHarmonics3.SphericalHarmonics3
    type Light = __lights_Light.Light

    type [<AllowNullLiteral>] IExports =
        abstract LightProbe: LightProbeStatic

    type [<AllowNullLiteral>] LightProbe =
        inherit Light
        /// <default>'LightProbe'</default>
        abstract ``type``: string with get, set
        abstract isLightProbe: bool
        /// <default>new THREE.SphericalHarmonics3()</default>
        abstract sh: SphericalHarmonics3 with get, set
        abstract fromJSON: json: obj -> LightProbe

    type [<AllowNullLiteral>] LightProbeStatic =
        [<EmitConstructor>] abstract Create: ?sh: SphericalHarmonics3 * ?intensity: float -> LightProbe

module __lights_LightShadow =
    type Camera = __cameras_Camera.Camera
    type Light = __lights_Light.Light
    type Vector2 = __math_Vector2.Vector2
    type Vector4 = __math_Vector4.Vector4
    type Matrix4 = __math_Matrix4.Matrix4
    type RenderTarget = __renderers_webgl_WebGLRenderLists.RenderTarget

    type [<AllowNullLiteral>] IExports =
        abstract LightShadow: LightShadowStatic

    type [<AllowNullLiteral>] LightShadow =
        abstract camera: Camera with get, set
        /// <default>0</default>
        abstract bias: float with get, set
        /// <default>0</default>
        abstract normalBias: float with get, set
        /// <default>1</default>
        abstract radius: float with get, set
        /// <default>new THREE.Vector2( 512, 512 )</default>
        abstract mapSize: Vector2 with get, set
        /// <default>null</default>
        abstract map: RenderTarget with get, set
        /// <default>null</default>
        abstract mapPass: RenderTarget with get, set
        /// <default>new THREE.Matrix4()</default>
        abstract matrix: Matrix4 with get, set
        /// <default>true</default>
        abstract autoUpdate: bool with get, set
        /// <default>false</default>
        abstract needsUpdate: bool with get, set
        abstract copy: source: LightShadow -> LightShadow
        abstract clone: ?recursive: bool -> LightShadow
        abstract toJSON: unit -> obj option
        abstract getFrustum: unit -> float
        abstract updateMatrices: light: Light * ?viewportIndex: float -> unit
        abstract getViewport: viewportIndex: float -> Vector4
        abstract getFrameExtents: unit -> Vector2
        abstract dispose: unit -> unit

    type [<AllowNullLiteral>] LightShadowStatic =
        [<EmitConstructor>] abstract Create: camera: Camera -> LightShadow

module __lights_PointLight =
    type ColorRepresentation = Utils.ColorRepresentation
    type Light = __lights_Light.Light
    type PointLightShadow = __lights_PointLightShadow.PointLightShadow

    type [<AllowNullLiteral>] IExports =
        /// <example>
        /// const light = new THREE.PointLight( 0xff0000, 1, 100 );
        /// light.position.set( 50, 50, 50 );
        /// scene.add( light );
        /// </example>
        abstract PointLight: PointLightStatic

    /// <example>
    /// const light = new THREE.PointLight( 0xff0000, 1, 100 );
    /// light.position.set( 50, 50, 50 );
    /// scene.add( light );
    /// </example>
    type [<AllowNullLiteral>] PointLight =
        inherit Light
        /// <default>'PointLight'</default>
        abstract ``type``: string with get, set
        /// <summary>Light's intensity.</summary>
        /// <default>1</default>
        abstract intensity: float with get, set
        /// <summary>If non-zero, light will attenuate linearly from maximum intensity at light position down to zero at distance.</summary>
        /// <default>0</default>
        abstract distance: float with get, set
        /// <default>1</default>
        abstract decay: float with get, set
        /// <default>new THREE.PointLightShadow()</default>
        abstract shadow: PointLightShadow with get, set
        abstract power: float with get, set

    /// <example>
    /// const light = new THREE.PointLight( 0xff0000, 1, 100 );
    /// light.position.set( 50, 50, 50 );
    /// scene.add( light );
    /// </example>
    type [<AllowNullLiteral>] PointLightStatic =
        [<EmitConstructor>] abstract Create: ?color: ColorRepresentation * ?intensity: float * ?distance: float * ?decay: float -> PointLight

module __lights_PointLightShadow =
    type PerspectiveCamera = __cameras_PerspectiveCamera.PerspectiveCamera
    type LightShadow = __lights_LightShadow.LightShadow

    type [<AllowNullLiteral>] IExports =
        abstract PointLightShadow: PointLightShadowStatic

    type [<AllowNullLiteral>] PointLightShadow =
        inherit LightShadow
        abstract camera: PerspectiveCamera with get, set

    type [<AllowNullLiteral>] PointLightShadowStatic =
        [<EmitConstructor>] abstract Create: unit -> PointLightShadow

module __lights_RectAreaLight =
    type Light = __lights_Light.Light
    type ColorRepresentation = Utils.ColorRepresentation

    type [<AllowNullLiteral>] IExports =
        abstract RectAreaLight: RectAreaLightStatic

    type [<AllowNullLiteral>] RectAreaLight =
        inherit Light
        /// <default>'RectAreaLight'</default>
        abstract ``type``: string with get, set
        /// <default>10</default>
        abstract width: float with get, set
        /// <default>10</default>
        abstract height: float with get, set
        /// <default>1</default>
        abstract intensity: float with get, set
        abstract isRectAreaLight: bool

    type [<AllowNullLiteral>] RectAreaLightStatic =
        [<EmitConstructor>] abstract Create: ?color: ColorRepresentation * ?intensity: float * ?width: float * ?height: float -> RectAreaLight

module __lights_SpotLight =
    type Color = __math_Color.Color
    type Vector3 = __math_Vector3.Vector3
    type Object3D = __core_Object3D.Object3D
    type SpotLightShadow = __lights_SpotLightShadow.SpotLightShadow
    type Light = __lights_Light.Light
    type ColorRepresentation = Utils.ColorRepresentation

    type [<AllowNullLiteral>] IExports =
        /// A point light that can cast shadow in one direction.
        abstract SpotLight: SpotLightStatic

    /// A point light that can cast shadow in one direction.
    type [<AllowNullLiteral>] SpotLight =
        inherit Light
        /// <default>'SpotLight'</default>
        abstract ``type``: string with get, set
        /// <default>THREE.Object3D.DefaultUp</default>
        abstract position: Vector3 with get, set
        /// <summary>Spotlight focus points at target.position.</summary>
        /// <default>new THREE.Object3D()</default>
        abstract target: Object3D with get, set
        /// <summary>Light's intensity.</summary>
        /// <default>1</default>
        abstract intensity: float with get, set
        /// <summary>If non-zero, light will attenuate linearly from maximum intensity at light position down to zero at distance.</summary>
        /// <default>0</default>
        abstract distance: float with get, set
        /// <summary>Maximum extent of the spotlight, in radians, from its direction.</summary>
        /// <default>Math.PI / 3.</default>
        abstract angle: float with get, set
        /// <default>1</default>
        abstract decay: float with get, set
        /// <default>new THREE.SpotLightShadow()</default>
        abstract shadow: SpotLightShadow with get, set
        abstract power: float with get, set
        /// <default>0</default>
        abstract penumbra: float with get, set
        abstract isSpotLight: bool

    /// A point light that can cast shadow in one direction.
    type [<AllowNullLiteral>] SpotLightStatic =
        [<EmitConstructor>] abstract Create: ?color: ColorRepresentation * ?intensity: float * ?distance: float * ?angle: float * ?penumbra: float * ?decay: float -> SpotLight

module __lights_SpotLightShadow =
    type PerspectiveCamera = __cameras_PerspectiveCamera.PerspectiveCamera
    type LightShadow = __lights_LightShadow.LightShadow

    type [<AllowNullLiteral>] IExports =
        abstract SpotLightShadow: SpotLightShadowStatic

    type [<AllowNullLiteral>] SpotLightShadow =
        inherit LightShadow
        abstract camera: PerspectiveCamera with get, set
        abstract isSpotLightShadow: bool
        /// <default>1</default>
        abstract focus: float with get, set

    type [<AllowNullLiteral>] SpotLightShadowStatic =
        [<EmitConstructor>] abstract Create: unit -> SpotLightShadow

module __extras_core_Curve =
    type Vector = __math_Vector2.Vector
    type Vector3 = __math_Vector3.Vector3

    type [<AllowNullLiteral>] IExports =
        /// An extensible curve object which contains methods for interpolation
        /// class Curve<T extends Vector>
        abstract Curve: CurveStatic

    /// An extensible curve object which contains methods for interpolation
    /// class Curve<T extends Vector>
    type [<AllowNullLiteral>] Curve<'T when 'T :> Vector> =
        /// <default>'Curve'</default>
        abstract ``type``: string with get, set
        /// <summary>
        /// This value determines the amount of divisions when calculating the cumulative segment lengths of a curve via .getLengths.
        /// To ensure precision when using methods like .getSpacedPoints, it is recommended to increase .arcLengthDivisions if the curve is very large.
        /// </summary>
        /// <default>200</default>
        abstract arcLengthDivisions: float with get, set
        /// Returns a vector for point t of the curve where t is between 0 and 1
        /// getPoint(t: number, optionalTarget?: T): T;
        abstract getPoint: t: float * ?optionalTarget: 'T -> 'T
        /// Returns a vector for point at relative position in curve according to arc length
        /// getPointAt(u: number, optionalTarget?: T): T;
        abstract getPointAt: u: float * ?optionalTarget: 'T -> 'T
        /// Get sequence of points using getPoint( t )
        /// getPoints(divisions?: number): T[];
        abstract getPoints: ?divisions: float -> ResizeArray<'T>
        /// Get sequence of equi-spaced points using getPointAt( u )
        /// getSpacedPoints(divisions?: number): T[];
        abstract getSpacedPoints: ?divisions: float -> ResizeArray<'T>
        /// Get total curve arc length
        abstract getLength: unit -> float
        /// Get list of cumulative segment lengths
        abstract getLengths: ?divisions: float -> ResizeArray<float>
        /// Update the cumlative segment distance cache
        abstract updateArcLengths: unit -> unit
        /// Given u ( 0 .. 1 ), get a t to find p. This gives you points which are equi distance
        abstract getUtoTmapping: u: float * distance: float -> float
        /// Returns a unit vector tangent at t. If the subclassed curve do not implement its tangent derivation, 2 points a
        /// small delta apart will be used to find its gradient which seems to give a reasonable approximation
        /// getTangent(t: number, optionalTarget?: T): T;
        abstract getTangent: t: float * ?optionalTarget: 'T -> 'T
        /// Returns tangent at equidistance point u on the curve
        /// getTangentAt(u: number, optionalTarget?: T): T;
        abstract getTangentAt: u: float * ?optionalTarget: 'T -> 'T
        /// Generate Frenet frames of the curve
        abstract computeFrenetFrames: segments: float * ?closed: bool -> {| tangents: ResizeArray<Vector3>; normals: ResizeArray<Vector3>; binormals: ResizeArray<Vector3> |}
        abstract clone: unit -> Curve<'T>
        abstract copy: source: Curve<'T> -> Curve<'T>
        abstract toJSON: unit -> obj
        abstract fromJSON: json: obj -> Curve<'T>

    /// An extensible curve object which contains methods for interpolation
    /// class Curve<T extends Vector>
    type [<AllowNullLiteral>] CurveStatic =
        [<EmitConstructor>] abstract Create: unit -> Curve<'T> when 'T :> Vector
        [<Obsolete("since r84.")>]
        abstract create: constructorFunc: (unit -> unit) * getPointFunc: (unit -> unit) -> (unit -> unit)

module __extras_core_CurvePath =
    open __extras_core_Curve
    type Vector = __math_Vector2.Vector

    type [<AllowNullLiteral>] IExports =
        abstract CurvePath: CurvePathStatic

    type [<AllowNullLiteral>] CurvePath<'T when 'T :> Vector> =
        inherit Curve<'T>
        /// <default>'CurvePath'</default>
        abstract ``type``: string with get, set
        /// <default>[]</default>
        abstract curves: Array<Curve<'T>> with get, set
        /// <default>false</default>
        abstract autoClose: bool with get, set
        // abstract add: curve: Curve<'T> -> unit
        abstract closePath: unit -> unit
        /// Returns a vector for point t of the curve where t is between 0 and 1
        /// getPoint(t: number, optionalTarget?: T): T;
        abstract getPoint: t: float -> 'T
        abstract getCurveLengths: unit -> ResizeArray<float>

    type [<AllowNullLiteral>] CurvePathStatic =
        [<EmitConstructor>] abstract Create: unit -> CurvePath<'T>

module __extras_core_Font =
    type Shape = __extras_core_Shape.Shape

    type [<AllowNullLiteral>] IExports =
        abstract Font: FontStatic

    type [<AllowNullLiteral>] Font =
        /// <default>'Font'</default>
        abstract ``type``: string with get, set
        abstract data: string with get, set
        abstract generateShapes: text: string * size: float -> ResizeArray<Shape>

    type [<AllowNullLiteral>] FontStatic =
        [<EmitConstructor>] abstract Create: jsondata: obj option -> Font

module __extras_core_Path =
    open __extras_core_CurvePath
    type Vector2 = __math_Vector2.Vector2

    type [<AllowNullLiteral>] IExports =
        /// a 2d path representation, comprising of points, lines, and cubes, similar to the html5 2d canvas api. It extends CurvePath.
        abstract Path: PathStatic

    /// a 2d path representation, comprising of points, lines, and cubes, similar to the html5 2d canvas api. It extends CurvePath.
    type [<AllowNullLiteral>] Path =
        inherit CurvePath<Vector2>
        /// <default>'Path'</default>
        abstract ``type``: string with get, set
        /// <default>new THREE.Vector2()</default>
        abstract currentPoint: Vector2 with get, set
        [<Obsolete("Use {@link Path#setFromPoints .setFromPoints()} instead.")>]
        abstract fromPoints: vectors: ResizeArray<Vector2> -> Path
        abstract setFromPoints: vectors: ResizeArray<Vector2> -> Path
        abstract moveTo: x: float * y: float -> Path
        abstract lineTo: x: float * y: float -> Path
        abstract quadraticCurveTo: aCPx: float * aCPy: float * aX: float * aY: float -> Path
        abstract bezierCurveTo: aCP1x: float * aCP1y: float * aCP2x: float * aCP2y: float * aX: float * aY: float -> Path
        abstract splineThru: pts: ResizeArray<Vector2> -> Path
        abstract arc: aX: float * aY: float * aRadius: float * aStartAngle: float * aEndAngle: float * aClockwise: bool -> Path
        abstract absarc: aX: float * aY: float * aRadius: float * aStartAngle: float * aEndAngle: float * aClockwise: bool -> Path
        abstract ellipse: aX: float * aY: float * xRadius: float * yRadius: float * aStartAngle: float * aEndAngle: float * aClockwise: bool * aRotation: float -> Path
        abstract absellipse: aX: float * aY: float * xRadius: float * yRadius: float * aStartAngle: float * aEndAngle: float * aClockwise: bool * aRotation: float -> Path

    /// a 2d path representation, comprising of points, lines, and cubes, similar to the html5 2d canvas api. It extends CurvePath.
    type [<AllowNullLiteral>] PathStatic =
        [<EmitConstructor>] abstract Create: ?points: ResizeArray<Vector2> -> Path

module __extras_core_Shape =
    open __extras_core_CurvePath
    type Vector2 = __math_Vector2.Vector2
    type Path = __extras_core_Path.Path

    type [<AllowNullLiteral>] IExports =
        /// Defines a 2d shape plane using paths.
        abstract Shape: ShapeStatic

    /// Defines a 2d shape plane using paths.
    type [<AllowNullLiteral>] Shape =
        // inherit Path
        inherit Path
        /// <default>'Shape'</default>
        abstract ``type``: string with get, set
        abstract uuid: string with get, set
        /// <default>[]</default>
        abstract holes: ResizeArray<Path> with get, set
        abstract getPointsHoles: divisions: float -> ResizeArray<ResizeArray<Vector2>>
        abstract extractPoints: divisions: float -> {| shape: ResizeArray<Vector2>; holes: ResizeArray<ResizeArray<Vector2>> |}

    /// Defines a 2d shape plane using paths.
    type [<AllowNullLiteral>] ShapeStatic =
        [<EmitConstructor>] abstract Create: ?points: ResizeArray<Vector2> -> Shape

module __extras_core_ShapePath =
    type Vector2 = __math_Vector2.Vector2
    type Shape = __extras_core_Shape.Shape
    type Color = __math_Color.Color

    type [<AllowNullLiteral>] IExports =
        abstract ShapePath: ShapePathStatic

    type [<AllowNullLiteral>] ShapePath =
        /// <default>'ShapePath'</default>
        abstract ``type``: string with get, set
        /// <default>new THREE.Color()</default>
        abstract color: Color with get, set
        /// <default>[]</default>
        abstract subPaths: ResizeArray<obj option> with get, set
        /// <default>null</default>
        abstract currentPath: obj option with get, set
        abstract moveTo: x: float * y: float -> ShapePath
        abstract lineTo: x: float * y: float -> ShapePath
        abstract quadraticCurveTo: aCPx: float * aCPy: float * aX: float * aY: float -> ShapePath
        abstract bezierCurveTo: aCP1x: float * aCP1y: float * aCP2x: float * aCP2y: float * aX: float * aY: float -> ShapePath
        abstract splineThru: pts: ResizeArray<Vector2> -> ShapePath
        abstract toShapes: isCCW: bool * ?noHoles: bool -> ResizeArray<Shape>

    type [<AllowNullLiteral>] ShapePathStatic =
        [<EmitConstructor>] abstract Create: unit -> ShapePath


module __extras_core_Curve =
    type Vector = __math_Vector2.Vector
    type Vector3 = __math_Vector3.Vector3

    type [<AllowNullLiteral>] IExports =
        /// An extensible curve object which contains methods for interpolation
        /// class Curve<T extends Vector>
        abstract Curve: CurveStatic

    /// An extensible curve object which contains methods for interpolation
    /// class Curve<T extends Vector>
    type [<AllowNullLiteral>] Curve<'T when 'T :> Vector> =
        /// <default>'Curve'</default>
        abstract ``type``: string with get, set
        /// <summary>
        /// This value determines the amount of divisions when calculating the cumulative segment lengths of a curve via .getLengths.
        /// To ensure precision when using methods like .getSpacedPoints, it is recommended to increase .arcLengthDivisions if the curve is very large.
        /// </summary>
        /// <default>200</default>
        abstract arcLengthDivisions: float with get, set
        /// Returns a vector for point t of the curve where t is between 0 and 1
        /// getPoint(t: number, optionalTarget?: T): T;
        abstract getPoint: t: float * ?optionalTarget: 'T -> 'T
        /// Returns a vector for point at relative position in curve according to arc length
        /// getPointAt(u: number, optionalTarget?: T): T;
        abstract getPointAt: u: float * ?optionalTarget: 'T -> 'T
        /// Get sequence of points using getPoint( t )
        /// getPoints(divisions?: number): T[];
        abstract getPoints: ?divisions: float -> ResizeArray<'T>
        /// Get sequence of equi-spaced points using getPointAt( u )
        /// getSpacedPoints(divisions?: number): T[];
        abstract getSpacedPoints: ?divisions: float -> ResizeArray<'T>
        /// Get total curve arc length
        abstract getLength: unit -> float
        /// Get list of cumulative segment lengths
        abstract getLengths: ?divisions: float -> ResizeArray<float>
        /// Update the cumlative segment distance cache
        abstract updateArcLengths: unit -> unit
        /// Given u ( 0 .. 1 ), get a t to find p. This gives you points which are equi distance
        abstract getUtoTmapping: u: float * distance: float -> float
        /// Returns a unit vector tangent at t. If the subclassed curve do not implement its tangent derivation, 2 points a
        /// small delta apart will be used to find its gradient which seems to give a reasonable approximation
        /// getTangent(t: number, optionalTarget?: T): T;
        abstract getTangent: t: float * ?optionalTarget: 'T -> 'T
        /// Returns tangent at equidistance point u on the curve
        /// getTangentAt(u: number, optionalTarget?: T): T;
        abstract getTangentAt: u: float * ?optionalTarget: 'T -> 'T
        /// Generate Frenet frames of the curve
        abstract computeFrenetFrames: segments: float * ?closed: bool -> {| tangents: ResizeArray<Vector3>; normals: ResizeArray<Vector3>; binormals: ResizeArray<Vector3> |}
        abstract clone: unit -> Curve<'T>
        abstract copy: source: Curve<'T> -> Curve<'T>
        abstract toJSON: unit -> obj
        abstract fromJSON: json: obj -> Curve<'T>

    /// An extensible curve object which contains methods for interpolation
    /// class Curve<T extends Vector>
    type [<AllowNullLiteral>] CurveStatic =
        [<EmitConstructor>] abstract Create: unit -> Curve<'T> when 'T :> Vector
        [<Obsolete("since r84.")>]
        abstract create: constructorFunc: (unit -> unit) * getPointFunc: (unit -> unit) -> (unit -> unit)

module __extras_core_CurvePath =
    open __extras_core_Curve
    type Vector = __math_Vector2.Vector

    type [<AllowNullLiteral>] IExports =
        abstract CurvePath: CurvePathStatic

    type [<AllowNullLiteral>] CurvePath<'T when 'T :> Vector> =
        inherit Curve<'T>
        /// <default>'CurvePath'</default>
        abstract ``type``: string with get, set
        /// <default>[]</default>
        abstract curves: Array<Curve<'T>> with get, set
        /// <default>false</default>
        abstract autoClose: bool with get, set
        abstract add: curve: Curve<'T> -> unit
        abstract closePath: unit -> unit
        /// Returns a vector for point t of the curve where t is between 0 and 1
        /// getPoint(t: number, optionalTarget?: T): T;
        abstract getPoint: t: float -> 'T
        abstract getCurveLengths: unit -> ResizeArray<float>

    type [<AllowNullLiteral>] CurvePathStatic =
        [<EmitConstructor>] abstract Create: unit -> CurvePath<'T>

module __extras_core_Font =
    type Shape = __extras_core_Shape.Shape

    type [<AllowNullLiteral>] IExports =
        abstract Font: FontStatic

    type [<AllowNullLiteral>] Font =
        /// <default>'Font'</default>
        abstract ``type``: string with get, set
        abstract data: string with get, set
        abstract generateShapes: text: string * size: float -> ResizeArray<Shape>

    type [<AllowNullLiteral>] FontStatic =
        [<EmitConstructor>] abstract Create: jsondata: obj option -> Font

module __extras_core_Path =
    open __extras_core_CurvePath
    type Vector2 = __math_Vector2.Vector2

    type [<AllowNullLiteral>] IExports =
        /// a 2d path representation, comprising of points, lines, and cubes, similar to the html5 2d canvas api. It extends CurvePath.
        abstract Path: PathStatic

    /// a 2d path representation, comprising of points, lines, and cubes, similar to the html5 2d canvas api. It extends CurvePath.
    type [<AllowNullLiteral>] Path =
        inherit CurvePath<Vector2>
        /// <default>'Path'</default>
        abstract ``type``: string with get, set
        /// <default>new THREE.Vector2()</default>
        abstract currentPoint: Vector2 with get, set
        [<Obsolete("Use {@link Path#setFromPoints .setFromPoints()} instead.")>]
        abstract fromPoints: vectors: ResizeArray<Vector2> -> Path
        abstract setFromPoints: vectors: ResizeArray<Vector2> -> Path
        abstract moveTo: x: float * y: float -> Path
        abstract lineTo: x: float * y: float -> Path
        abstract quadraticCurveTo: aCPx: float * aCPy: float * aX: float * aY: float -> Path
        abstract bezierCurveTo: aCP1x: float * aCP1y: float * aCP2x: float * aCP2y: float * aX: float * aY: float -> Path
        abstract splineThru: pts: ResizeArray<Vector2> -> Path
        abstract arc: aX: float * aY: float * aRadius: float * aStartAngle: float * aEndAngle: float * aClockwise: bool -> Path
        abstract absarc: aX: float * aY: float * aRadius: float * aStartAngle: float * aEndAngle: float * aClockwise: bool -> Path
        abstract ellipse: aX: float * aY: float * xRadius: float * yRadius: float * aStartAngle: float * aEndAngle: float * aClockwise: bool * aRotation: float -> Path
        abstract absellipse: aX: float * aY: float * xRadius: float * yRadius: float * aStartAngle: float * aEndAngle: float * aClockwise: bool * aRotation: float -> Path

    /// a 2d path representation, comprising of points, lines, and cubes, similar to the html5 2d canvas api. It extends CurvePath.
    type [<AllowNullLiteral>] PathStatic =
        [<EmitConstructor>] abstract Create: ?points: ResizeArray<Vector2> -> Path

module __extras_core_Shape =
    open __extras_core_CurvePath
    type Vector2 = __math_Vector2.Vector2
    type Path = __extras_core_Path.Path

    type [<AllowNullLiteral>] IExports =
        /// Defines a 2d shape plane using paths.
        abstract Shape: ShapeStatic

    /// Defines a 2d shape plane using paths.
    type [<AllowNullLiteral>] Shape =
        // inherit Path
        inherit Path
        /// <default>'Shape'</default>
        abstract ``type``: string with get, set
        abstract uuid: string with get, set
        /// <default>[]</default>
        abstract holes: ResizeArray<Path> with get, set
        abstract getPointsHoles: divisions: float -> ResizeArray<ResizeArray<Vector2>>
        abstract extractPoints: divisions: float -> {| shape: ResizeArray<Vector2>; holes: ResizeArray<ResizeArray<Vector2>> |}

    /// Defines a 2d shape plane using paths.
    type [<AllowNullLiteral>] ShapeStatic =
        [<EmitConstructor>] abstract Create: ?points: ResizeArray<Vector2> -> Shape

module __extras_core_ShapePath =
    type Vector2 = __math_Vector2.Vector2
    type Shape = __extras_core_Shape.Shape
    type Color = __math_Color.Color

    type [<AllowNullLiteral>] IExports =
        abstract ShapePath: ShapePathStatic

    type [<AllowNullLiteral>] ShapePath =
        /// <default>'ShapePath'</default>
        abstract ``type``: string with get, set
        /// <default>new THREE.Color()</default>
        abstract color: Color with get, set
        /// <default>[]</default>
        abstract subPaths: ResizeArray<obj option> with get, set
        /// <default>null</default>
        abstract currentPath: obj option with get, set
        abstract moveTo: x: float * y: float -> ShapePath
        abstract lineTo: x: float * y: float -> ShapePath
        abstract quadraticCurveTo: aCPx: float * aCPy: float * aX: float * aY: float -> ShapePath
        abstract bezierCurveTo: aCP1x: float * aCP1y: float * aCP2x: float * aCP2y: float * aX: float * aY: float -> ShapePath
        abstract splineThru: pts: ResizeArray<Vector2> -> ShapePath
        abstract toShapes: isCCW: bool * ?noHoles: bool -> ResizeArray<Shape>

    type [<AllowNullLiteral>] ShapePathStatic =
        [<EmitConstructor>] abstract Create: unit -> ShapePath

module __audio_AudioContext =
    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/audio/AudioContext.js">src/audio/AudioContext.js</see></summary>
    let [<Import("AudioContext","three/audio/AudioContext")>] audioContext: AudioContext.IExports = jsNative

    type AudioContext = interface end
    /// <summary>see <see href="https://github.com/mrdoob/three.js/blob/master/src/audio/AudioContext.js">src/audio/AudioContext.js</see></summary>
    module AudioContext =

        type [<AllowNullLiteral>] IExports =
            abstract getContext: unit -> AudioContext
            abstract setContext: unit -> unit



module __geometries_BoxGeometry =
    type BufferGeometry = __core_BufferGeometry.BufferGeometry

    type [<AllowNullLiteral>] IExports =
        abstract BoxGeometry: BoxGeometryStatic

    type [<AllowNullLiteral>] BoxGeometry =
        inherit BufferGeometry
        /// <default>'BoxGeometry'</default>
        abstract ``type``: string with get, set
        abstract parameters: BoxGeometryParameters with get, set

    type [<AllowNullLiteral>] BoxGeometryStatic =
        /// <param name="width">— Width of the sides on the X axis.</param>
        /// <param name="height">— Height of the sides on the Y axis.</param>
        /// <param name="depth">— Depth of the sides on the Z axis.</param>
        /// <param name="widthSegments">— Number of segmented faces along the width of the sides.</param>
        /// <param name="heightSegments">— Number of segmented faces along the height of the sides.</param>
        /// <param name="depthSegments">— Number of segmented faces along the depth of the sides.</param>
        [<EmitConstructor>] abstract Create: ?width: float * ?height: float * ?depth: float * ?widthSegments: float * ?heightSegments: float * ?depthSegments: float -> BoxGeometry
        abstract fromJSON: data: obj option -> BoxGeometry

    type [<AllowNullLiteral>] BoxGeometryParameters =
        abstract width: float with get, set
        abstract height: float with get, set
        abstract depth: float with get, set
        abstract widthSegments: float with get, set
        abstract heightSegments: float with get, set
        abstract depthSegments: float with get, set
// Manually copied code    
module __loaders_TextureLoader =
    type Loader = __loaders_Loader.Loader
    type LoadingManager = __loaders_LoadingManager.LoadingManager
    type Texture = __textures_Texture.Texture

    type [<AllowNullLiteral>] IExports =
        /// Class for loading a texture.
        /// Unlike other loaders, this one emits events instead of using predefined callbacks. So if you're interested in getting notified when things happen, you need to add listeners to the object.
        abstract TextureLoader: TextureLoaderStatic

    /// Class for loading a texture.
    /// Unlike other loaders, this one emits events instead of using predefined callbacks. So if you're interested in getting notified when things happen, you need to add listeners to the object.
    type [<AllowNullLiteral>] TextureLoader =
        inherit Loader
        abstract load: url: string * ?onLoad: (Texture -> unit) * ?onProgress: (ProgressEvent -> unit) * ?onError: (ErrorEvent -> unit) -> Texture
        abstract loadAsync: url: string * ?onProgress: (ProgressEvent -> unit) -> Promise<Texture>

    /// Class for loading a texture.
    /// Unlike other loaders, this one emits events instead of using predefined callbacks. So if you're interested in getting notified when things happen, you need to add listeners to the object.
    type [<AllowNullLiteral>] TextureLoaderStatic =
        [<EmitConstructor>] abstract Create: ?manager: LoadingManager -> TextureLoader
module __loaders_Loader =
    type LoadingManager = __loaders_LoadingManager.LoadingManager

    type [<AllowNullLiteral>] IExports =
        /// Base class for implementing loaders.
        abstract Loader: LoaderStatic

    /// Base class for implementing loaders.
    type [<AllowNullLiteral>] Loader =
        /// <default>'anonymous'</default>
        abstract crossOrigin: string with get, set
        /// <default>: false</default>
        abstract withCredentials: bool with get, set
        /// <default>''</default>
        abstract path: string with get, set
        /// <default>''</default>
        abstract resourcePath: string with get, set
        abstract manager: LoadingManager with get, set
        /// <default>{}</default>
        abstract requestHeader: LoaderRequestHeader with get, set
        abstract loadAsync: url: string * ?onProgress: (ProgressEvent -> unit) -> Promise<obj option>
        abstract setCrossOrigin: crossOrigin: string -> Loader
        abstract setWithCredentials: value: bool -> Loader
        abstract setPath: path: string -> Loader
        abstract setResourcePath: resourcePath: string -> Loader
        abstract setRequestHeader: requestHeader: LoaderSetRequestHeaderRequestHeader -> Loader

    /// <summary>
    /// Typescript interface contains an <see href="https://www.typescriptlang.org/docs/handbook/2/objects.html#index-signatures">index signature</see> (like <c>{ [key:string]: string }</c>).
    /// Unlike an indexer in F#, index signatures index over a type's members.
    /// 
    /// As such an index signature cannot be implemented via regular F# Indexer (<c>Item</c> property),
    /// but instead by just specifying fields.
    /// 
    /// Easiest way to declare such a type is with an Anonymous Record and force it into the function.
    /// For example:
    /// <code lang="fsharp">
    /// type I =
    ///     [&lt;EmitIndexer&gt;]
    ///     abstract Item: string -&gt; string
    /// let f (i: I) = jsNative
    /// 
    /// let t = {| Value1 = "foo"; Value2 = "bar" |}
    /// f (!! t)
    /// </code>
    /// </summary>
    type [<AllowNullLiteral>] LoaderSetRequestHeaderRequestHeader =
        [<EmitIndexer>] abstract Item: header: string -> string with get, set

    /// Base class for implementing loaders.
    type [<AllowNullLiteral>] LoaderStatic =
        [<EmitConstructor>] abstract Create: ?manager: LoadingManager -> Loader

    type [<AllowNullLiteral>] LoaderRequestHeader =
        [<EmitIndexer>] abstract Item: header: string -> string with get, set
module __loaders_LoadingManager =
    type Loader = __loaders_Loader.Loader

    type [<AllowNullLiteral>] IExports =
        abstract DefaultLoadingManager: LoadingManager
        /// Handles and keeps track of loaded and pending data.
        abstract LoadingManager: LoadingManagerStatic

    /// Handles and keeps track of loaded and pending data.
    type [<AllowNullLiteral>] LoadingManager =
        /// <summary>Will be called when loading of an item starts.</summary>
        /// <param name="url">The url of the item that started loading.</param>
        /// <param name="loaded">The number of items already loaded so far.</param>
        /// <param name="total">The total amount of items to be loaded.</param>
        abstract onStart: (string -> float -> float -> unit) option with get, set
        /// Will be called when all items finish loading.
        /// The default is a function with empty body.
        abstract onLoad: (unit -> unit) with get, set
        /// <summary>
        /// Will be called for each loaded item.
        /// The default is a function with empty body.
        /// </summary>
        /// <param name="url">The url of the item just loaded.</param>
        /// <param name="loaded">The number of items already loaded so far.</param>
        /// <param name="total">The total amount of items to be loaded.</param>
        abstract onProgress: (string -> float -> float -> unit) with get, set
        /// <summary>
        /// Will be called when item loading fails.
        /// The default is a function with empty body.
        /// </summary>
        /// <param name="url">The url of the item that errored.</param>
        abstract onError: (string -> unit) with get, set
        /// <summary>
        /// If provided, the callback will be passed each resource URL before a request is sent.
        /// The callback may return the original URL, or a new URL to override loading behavior.
        /// This behavior can be used to load assets from .ZIP files, drag-and-drop APIs, and Data URIs.
        /// </summary>
        /// <param name="callback">URL modifier callback. Called with url argument, and must return resolvedURL.</param>
        abstract setURLModifier: ?callback: (string -> string) -> LoadingManager
        /// <summary>
        /// Given a URL, uses the URL modifier callback (if any) and returns a resolved URL.
        /// If no URL modifier is set, returns the original URL.
        /// </summary>
        /// <param name="url">the url to load</param>
        abstract resolveURL: url: string -> string
        abstract itemStart: url: string -> unit
        abstract itemEnd: url: string -> unit
        abstract itemError: url: string -> unit
        abstract addHandler: regex: RegExp * loader: Loader -> LoadingManager
        abstract removeHandler: regex: RegExp -> LoadingManager
        abstract getHandler: file: string -> Loader option

    /// Handles and keeps track of loaded and pending data.
    type [<AllowNullLiteral>] LoadingManagerStatic =
        [<EmitConstructor>] abstract Create: ?onLoad: (unit -> unit) * ?onProgress: (string -> float -> float -> unit) * ?onError: (string -> unit) -> LoadingManager
        
type IExports =
    (**
        * Animation
        *)
    // abstract member VectorKeyframeTrack: animation_tracks_VectorKeyframeTrack.VectorKeyframeTrackStatic
    // abstract member StringKeyframeTrack: __animation_tracks_StringKeyframeTrack.StringKeyframeTrackStatic
    // abstract member QuaternionKeyframeTrack: __animation_tracks_QuaternionKeyframeTrack.QuaternionKeyframeTrackStatic
    // abstract member NumberKeyframeTrack: __animation_tracks_NumberKeyframeTrack.NumberKeyframeTrackStatic
    // abstract member ColorKeyframeTrack: __animation_tracks_ColorKeyframeTrack.ColorKeyframeTrackStatic
    // abstract member BooleanKeyframeTrack: __animation_tracks_BooleanKeyframeTrack.BooleanKeyframeTrackStatic
    // abstract member PropertyMixer: __animation_PropertyMixer.PropertyMixerStatic
    // abstract member PropertyBinding: __animation_PropertyBinding.PropertyBindingStatic
    abstract member KeyframeTrack: __animation_KeyframeTrack.KeyframeTrackStatic
    // abstract member AnimationUtils: __animation_AnimationUtils.AnimationUtilsStatic
    abstract member AnimationObjectGroup: __animation_AnimationObjectGroup.AnimationObjectGroupStatic
    abstract member AnimationMixer: __animation_AnimationMixer.AnimationMixerStatic
    abstract member AnimationClip: __animation_AnimationClip.AnimationClipStatic
    abstract member AnimationAction: __animation_AnimationAction.AnimationActionStatic
(**
 * Audio
 *)
    // abstract member AudioListener: __audio_AudioListener.AudioListenerStatic
    // abstract member PositionalAudio: __audio_PositionalAudio.PositionalAudioStatic
    // abstract member AudioContext: __audio_AudioContext.AudioContextStatic
    // abstract member AudioAnalyser: __audio_AudioAnalyser.AudioAnalyserStatic
    // abstract member Audio: __audio_Audio.AudioStatic
(**
 * Cameras
 *)
    abstract member StereoCamera: __cameras_StereoCamera.StereoCameraStatic
    abstract member PerspectiveCamera: __cameras_PerspectiveCamera.PerspectiveCameraStatic
    abstract member OrthographicCamera: __cameras_OrthographicCamera.OrthographicCameraStatic
    abstract member CubeCamera: __cameras_CubeCamera.CubeCameraStatic
    abstract member ArrayCamera: __cameras_ArrayCamera.ArrayCameraStatic
    abstract member Camera: __cameras_Camera.CameraStatic
(**
 * Core
 *)
    // abstract member Uniform: __core_Uniform.UniformStatic
    abstract member InstancedBufferGeometry: __core_InstancedBufferGeometry.InstancedBufferGeometryStatic
    abstract member BufferGeometry: __core_BufferGeometry.BufferGeometryStatic
    abstract member InterleavedBufferAttribute: __core_InterleavedBufferAttribute.InterleavedBufferAttributeStatic
    abstract member InstancedInterleavedBuffer: __core_InstancedInterleavedBuffer.InstancedInterleavedBufferStatic
    abstract member InterleavedBuffer: __core_InterleavedBuffer.InterleavedBufferStatic
    abstract member InstancedBufferAttribute: __core_InstancedBufferAttribute.InstancedBufferAttributeStatic
    abstract member GLBufferAttribute: __core_GLBufferAttribute.GLBufferAttributeStatic
    abstract member BufferAttribute: __core_BufferAttribute.BufferAttributeStatic
    abstract member Object3D: __core_Object3D.Object3DStatic
    abstract member Raycaster: __core_Raycaster.RaycasterStatic
    abstract member Layers: __core_Layers.LayersStatic
    abstract member EventDispatcher: __core_EventDispatcher.EventDispatcherStatic
    abstract member Clock: __core_Clock.ClockStatic
(**
 * Extras
 *)
    // abstract member ImmediateRenderObject: __extras_objects_ImmediateRenderObject.ImmediateRenderObjectStatic
    // abstract member Curves: __extras_curves_Curves.CurvesStatic
    // abstract member Shape: __extras_core_Shape.ShapeStatic
    // abstract member Path: __extras_core_Path.PathStatic
    // abstract member ShapePath: __extras_core_ShapePath.ShapePathStatic
    // abstract member Font: __extras_core_Font.FontStatic
    // abstract member CurvePath: __extras_core_CurvePath.CurvePathStatic
    // abstract member Curve: __extras_core_Curve.CurveStatic
    // abstract member DataUtils: __extras_DataUtils.DataUtilsStatic
    // abstract member ImageUtils: __extras_ImageUtils.ImageUtilsStatic
    // abstract member ShapeUtils: __extras_ShapeUtils.ShapeUtilsStatic
    // abstract member PMREMGenerator: __extras_PMREMGenerator.PMREMGeneratorStatic
(**
 * Geometries
 *)
    // abstract member Geometries: __geometries_Geometries.GeometriesStatic
(**
 * Helpers
 *)
    // abstract member SpotLightHelper: __helpers_SpotLightHelper.SpotLightHelperStatic
    // abstract member SkeletonHelper: __helpers_SkeletonHelper.SkeletonHelperStatic
    // abstract member PointLightHelper: __helpers_PointLightHelper.PointLightHelperStatic
    // abstract member HemisphereLightHelper: __helpers_HemisphereLightHelper.HemisphereLightHelperStatic
    // abstract member GridHelper: __helpers_GridHelper.GridHelperStatic
    // abstract member PolarGridHelper: __helpers_PolarGridHelper.PolarGridHelperStatic
    // abstract member DirectionalLightHelper: __helpers_DirectionalLightHelper.DirectionalLightHelperStatic
    // abstract member CameraHelper: __helpers_CameraHelper.CameraHelperStatic
    // abstract member BoxHelper: __helpers_BoxHelper.BoxHelperStatic
    // abstract member Box3Helper: __helpers_Box3Helper.Box3HelperStatic
    // abstract member PlaneHelper: __helpers_PlaneHelper.PlaneHelperStatic
    // abstract member ArrowHelper: __helpers_ArrowHelper.ArrowHelperStatic
    // abstract member AxesHelper: __helpers_AxesHelper.AxesHelperStatic
(**
 * Lights
 *)
    abstract member SpotLightShadow: __lights_SpotLightShadow.SpotLightShadowStatic
    abstract member SpotLight: __lights_SpotLight.SpotLightStatic
    abstract member PointLight: __lights_PointLight.PointLightStatic
    abstract member PointLightShadow: __lights_PointLightShadow.PointLightShadowStatic
    abstract member RectAreaLight: __lights_RectAreaLight.RectAreaLightStatic
    abstract member HemisphereLight: __lights_HemisphereLight.HemisphereLightStatic
    abstract member DirectionalLightShadow: __lights_DirectionalLightShadow.DirectionalLightShadowStatic
    abstract member DirectionalLight: __lights_DirectionalLight.DirectionalLightStatic
    abstract member AmbientLight: __lights_AmbientLight.AmbientLightStatic
    abstract member LightShadow: __lights_LightShadow.LightShadowStatic
    abstract member Light: __lights_Light.LightStatic
    abstract member AmbientLightProbe: __lights_AmbientLightProbe.AmbientLightProbeStatic
    abstract member HemisphereLightProbe: __lights_HemisphereLightProbe.HemisphereLightProbeStatic
    abstract member LightProbe: __lights_LightProbe.LightProbeStatic
(**
 * Loaders
 *)
    // abstract member AnimationLoader: __loaders_AnimationLoader.AnimationLoaderStatic
    // abstract member CompressedTextureLoader: __loaders_CompressedTextureLoader.CompressedTextureLoaderStatic
    // abstract member DataTextureLoader: __loaders_DataTextureLoader.DataTextureLoaderStatic
    // abstract member CubeTextureLoader: __loaders_CubeTextureLoader.CubeTextureLoaderStatic
    abstract member TextureLoader: __loaders_TextureLoader.TextureLoaderStatic
    // abstract member ObjectLoader: __loaders_ObjectLoader.ObjectLoaderStatic
    // abstract member MaterialLoader: __loaders_MaterialLoader.MaterialLoaderStatic
    // abstract member BufferGeometryLoader: __loaders_BufferGeometryLoader.BufferGeometryLoaderStatic
    // abstract member LoadingManager: __loaders_LoadingManager.LoadingManagerStatic
    // abstract member ImageLoader: __loaders_ImageLoader.ImageLoaderStatic
    // abstract member ImageBitmapLoader: __loaders_ImageBitmapLoader.ImageBitmapLoaderStatic
    // abstract member FontLoader: __loaders_FontLoader.FontLoaderStatic
    // abstract member FileLoader: __loaders_FileLoader.FileLoaderStatic
    abstract member Loader: __loaders_Loader.LoaderStatic
    // abstract member LoaderUtils: __loaders_LoaderUtils.LoaderUtilsStatic
    // abstract member Cache: __loaders_Cache.CacheStatic
    // abstract member AudioLoader: __loaders_AudioLoader.AudioLoaderStatic
(**
 * Materials
 *)
    // abstract member Materials: __materials_Materials.MaterialsStatic
    abstract member SpriteMaterial: __materials_SpriteMaterial.SpriteMaterialStatic
(**
 * Math
 *)
    abstract member QuaternionLinearInterpolant: __math_interpolants_QuaternionLinearInterpolant.QuaternionLinearInterpolantStatic
    abstract member LinearInterpolant: __math_interpolants_LinearInterpolant.LinearInterpolantStatic
    abstract member DiscreteInterpolant: __math_interpolants_DiscreteInterpolant.DiscreteInterpolantStatic
    abstract member CubicInterpolant: __math_interpolants_CubicInterpolant.CubicInterpolantStatic
    abstract member Interpolant: __math_Interpolant.InterpolantStatic
    abstract member Triangle: __math_Triangle.TriangleStatic
    abstract member Spherical: __math_Spherical.SphericalStatic
    abstract member Cylindrical: __math_Cylindrical.CylindricalStatic
    abstract member Plane: __math_Plane.PlaneStatic
    abstract member Frustum: __math_Frustum.FrustumStatic
    abstract member Sphere: __math_Sphere.SphereStatic
    abstract member Ray: __math_Ray.RayStatic
    abstract member Matrix4: __math_Matrix4.Matrix4Static
    abstract member Matrix3: __math_Matrix3.Matrix3Static
    abstract member Box3: __math_Box3.Box3Static
    abstract member Box2: __math_Box2.Box2Static
    abstract member Line3: __math_Line3.Line3Static
    abstract member Euler: __math_Euler.EulerStatic
    abstract member Vector4: __math_Vector4.Vector4Static
    abstract member Vector3: __math_Vector3.Vector3Static
    abstract member Vector2: __math_Vector2.Vector2Static
    abstract member Quaternion: __math_Quaternion.QuaternionStatic
    abstract member Color: __math_Color.ColorStatic
    abstract member SphericalHarmonics3: __math_SphericalHarmonics3.SphericalHarmonics3Static
(**
 * Objects
 *)
    abstract member Sprite: __objects_Sprite.SpriteStatic
    abstract member LOD: __objects_LOD.LODStatic
    abstract member InstancedMesh: __objects_InstancedMesh.InstancedMeshStatic
    abstract member SkinnedMesh: __objects_SkinnedMesh.SkinnedMeshStatic
    abstract member Skeleton: __objects_Skeleton.SkeletonStatic
    abstract member Bone: __objects_Bone.BoneStatic
    abstract member Mesh: __objects_Mesh.MeshStatic
    abstract member LineSegments: __objects_LineSegments.LineSegmentsStatic
    abstract member LineLoop: __objects_LineLoop.LineLoopStatic
    abstract member Line: __objects_Line.LineStatic
    abstract member Points: __objects_Points.PointsStatic
    abstract member Group: __objects_Group.GroupStatic
    abstract member BoxGeometry: __geometries_BoxGeometry.BoxGeometryStatic
    abstract member MeshBasicMaterial: __materials_MeshBasicMaterial.MeshBasicMaterialStatic
(**
 * Renderers
 *)
    abstract member WebGLMultisampleRenderTarget: __renderers_WebGLMultisampleRenderTarget.WebGLMultisampleRenderTargetStatic
    abstract member WebGLCubeRenderTarget: __renderers_WebGLCubeRenderTarget.WebGLCubeRenderTargetStatic
    abstract member WebGLMultipleRenderTargets: __renderers_WebGLMultipleRenderTargets.WebGLMultipleRenderTargetsStatic
    abstract member WebGLRenderTarget: __renderers_WebGLRenderTarget.WebGLRenderTargetStatic
    abstract member WebGLRenderer: __renderers_WebGLRenderer.WebGLRendererStatic
    abstract member WebGL1Renderer: __renderers_WebGL1Renderer.WebGL1RendererStatic
    
    // abstract member ShaderLib: __renderers_shaders_ShaderLib.ShaderLibStatic
    abstract member ShaderLib: __renderers_shaders_ShaderLib.IExportsShaderLib
    
    // abstract member UniformsLib: __renderers_shaders_UniformsLib.UniformsLibStatic
    abstract member UniformsLib: __renderers_shaders_UniformsLib.IExportsUniformsLib
    
    // abstract member UniformsUtils: __renderers_shaders_UniformsUtils.UniformsUtilsStatic
    // abstract member ShaderChunk: __renderers_shaders_ShaderChunk.ShaderChunkStatic
    abstract member WebGLBufferRenderer: __renderers_webgl_WebGLBufferRenderer.WebGLBufferRendererStatic
    abstract member WebGLCapabilities: __renderers_webgl_WebGLCapabilities.WebGLCapabilitiesStatic
    abstract member WebGLClipping: __renderers_webgl_WebGLClipping.WebGLClippingStatic
    abstract member WebGLCubeUVMaps: __renderers_webgl_WebGLCubeUVMaps.WebGLCubeUVMapsStatic
    abstract member WebGLExtensions: __renderers_webgl_WebGLExtensions.WebGLExtensionsStatic
    abstract member WebGLGeometries: __renderers_webgl_WebGLGeometries.WebGLGeometriesStatic
    abstract member WebGLIndexedBufferRenderer: __renderers_webgl_WebGLIndexedBufferRenderer.WebGLIndexedBufferRendererStatic
    abstract member WebGLInfo: __renderers_webgl_WebGLInfo.WebGLInfoStatic
    abstract member WebGLLights: __renderers_webgl_WebGLLights.WebGLLightsStatic
    abstract member WebGLObjects: __renderers_webgl_WebGLObjects.WebGLObjectsStatic
    abstract member WebGLProgram: __renderers_webgl_WebGLProgram.WebGLProgramStatic
    abstract member WebGLPrograms: __renderers_webgl_WebGLPrograms.WebGLProgramsStatic
    abstract member WebGLProperties: __renderers_webgl_WebGLProperties.WebGLPropertiesStatic
    abstract member WebGLRenderLists: __renderers_webgl_WebGLRenderLists.WebGLRenderListsStatic
    // abstract member WebGLShader: __renderers_webgl_WebGLShader.WebGLShaderStatic
    abstract member WebGLShadowMap: __renderers_webgl_WebGLShadowMap.WebGLShadowMapStatic
    abstract member WebGLState: __renderers_webgl_WebGLState.WebGLStateStatic
    abstract member WebGLTextures: __renderers_webgl_WebGLTextures.WebGLTexturesStatic
    abstract member WebGLUniforms: __renderers_webgl_WebGLUniforms.WebGLUniformsStatic
    // abstract member WebXR: __renderers_webxr_WebXR.WebXRStatic
    abstract member WebXRController: __renderers_webxr_WebXRController.WebXRControllerStatic
    abstract member WebXRManager: __renderers_webxr_WebXRManager.WebXRManagerStatic
(**
 * Scenes
 *)
    abstract member FogExp2: __scenes_FogExp2.FogExp2Static
    abstract member Fog: __scenes_Fog.FogStatic
    abstract member Scene: __scenes_Scene.SceneStatic
(**
 * Textures
 *)
    abstract member VideoTexture: __textures_VideoTexture.VideoTextureStatic
    abstract member DataTexture: __textures_DataTexture.DataTextureStatic
    abstract member DataTexture2DArray: __textures_DataTexture2DArray.DataTexture2DArrayStatic
    abstract member DataTexture3D: __textures_DataTexture3D.DataTexture3DStatic
    abstract member CompressedTexture: __textures_CompressedTexture.CompressedTextureStatic
    abstract member CubeTexture: __textures_CubeTexture.CubeTextureStatic
    abstract member CanvasTexture: __textures_CanvasTexture.CanvasTextureStatic
    abstract member DepthTexture: __textures_DepthTexture.DepthTextureStatic
    abstract member Texture: __textures_Texture.TextureStatic
    
    // TODO: Manually added
    abstract member MeshPhongMaterial: __materials_MeshPhongMaterial.MeshPhongMaterialStatic
    abstract member LineBasicMaterial: __materials_LineBasicMaterial.LineBasicMaterialStatic
