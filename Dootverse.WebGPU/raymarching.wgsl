struct Screen {
    width: f32,
    height: f32
};
struct output {
    @builtin(position) position: vec4f,
    @location(0) xy: vec2f,
};

fn rotX(p: vec3f, a: f32) -> vec3f { let s = sin(a); let c = cos(a); let r = p.yz * mat2x2f(c, s, -s, c); return vec3f(p.x, r.x, r.y); }
fn rotY(p: vec3f, a: f32) -> vec3f { let s = sin(a); let c = cos(a); let r = p.zx * mat2x2f(c, s, -s, c); return vec3f(r.y, p.y, r.x); }
fn rotM(p: vec3f, m: vec2f) -> vec3f { return rotY(rotX(p, 3.14159265 * m.y), 2. * 3.14159265 * m.x); }

@group(0) @binding(0) var<uniform> screen: Screen;

@vertex
// fn vs_main(@builtin(vertex_index) in_vertex_index: u32) -> @builtin(position) vec4<f32> {
fn vs_main(@builtin(vertex_index) in_vertex_index: u32) -> output {
    var pos = array<vec2f, 6>(
        vec2f(-1.0, 1.0),
        vec2f(-1.0, -1.0),
        vec2f(1.0, -1.0),
        
        vec2f(1.0, 1.0),
        vec2f(-1.0, 1.0),
        vec2f(1.0, -1.0)
    );
    var color = array<vec4f, 6>(
        vec4f(1.0, 0.0, 0.0, 1.0), // red
        vec4f(0.0, 1.0, 0.0, 1.0), // green
        vec4f(0.0, 0.0, 1.0, 1.0), // blue
        vec4f(0.0, 1.0, 0.0, 1.0), // green
        vec4f(1.0, 0.0, 0.0, 1.0), // red
        vec4f(0.0, 0.0, 1.0, 1.0), // blue
    );
    let x = f32(i32(in_vertex_index) - 1);
    let y = f32(i32(in_vertex_index & 1u) * 2 - 1);
    
    var vsOutput: output;
    vsOutput.position = vec4f(pos[in_vertex_index], 0.0, 1.0);
    vsOutput.xy = vsOutput.position.xy;
    return vsOutput;
}
// fn fs_main(@builtin(position) p: vec4<f32>) -> @location(0) vec4<f32> {
@fragment
fn fs_main(fsInput: output) -> @location(0) vec4<f32> {
    // [ -1 .. 1 ]
    let r = 1.0;
    let ratio = screen.width / screen.height;
    let width = screen.width;
    let height = screen.height;
    let x = fsInput.xy.x * ratio;
    // [ -1 .. 1 ]
    let y = fsInput.xy.y;
    let shapeOrigin = vec3f(0.0, 0.0, 8.0);
    let cameraOrigin = vec3f(0.0, 0.0, -2.0);
    let rotatedShapeOrigin = rotM(shapeOrigin, vec2f(0.1, 0.1));
//    let rotatedShapeOrigin = shapeOrigin;
//    var px = x;
//    var py = y;
    let f = 0.5;
//    var pz = cameraOrigin.z + f;
//    let dx = px - rotatedShapeOrigin.x;
//    let dy = py - rotatedShapeOrigin.y;
//    let dz = pz - rotatedShapeOrigin.z;
    var p = vec3f(x, y, f) + cameraOrigin;
    var offset = rotatedShapeOrigin - p;
    var distance = length(offset) - r;
    let dir = normalize(p - cameraOrigin);
//    var distance = sqrt((dx * dx) + (dy * dy) + (dz * dz)) - r;
//    let dir = vec3f(px, py, pz) - cameraOrigin;
    for (var i = 0; distance > 0.001 && distance < 3000.0 && i <= 1000; i++) {
        p = p + (dir * distance);
        distance = length(rotatedShapeOrigin - p) - r;
    }
    if (distance <= 0.001) {
        return vec4(0.0, 1.0, 0.0, 1.0);
    } else {
        return vec4(0.0, (fsInput.xy.xy + vec2(1.0, 1.0)) / 2.0, 1.0);
    }
//    if (sqrt((x * x) + (y * y)) > r) {
//        return vec4(0.0, (fsInput.xy.xy + vec2(1.0, 1.0)) / 2.0, 1.0);
//    } else {
//        return vec4(0.0, 1.0, 0.0, 1.0);
//    }
}
