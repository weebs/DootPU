// alias motor = mat2x4f;
// fn gp_rr(a: motor, b: motor) -> motor {
//     return motor( a[0].x*b[0] + vec4( -dot(a[0].yzw, b[0].yzw), b[0].x*a[0].yzw + cross(b[0].yzw,a[0].yzw) ), vec4(0.) ); 
// }
// fn gp_mm(a: motor, b: motor) -> motor {
//     return motor(
//          a[0].x*b[0].x   - dot(a[0].yzw, b[0].yzw), 
//          a[0].x*b[0].yzw + b[0].x*a[0].yzw + cross(b[0].yzw, a[0].yzw),
//          a[0].x*b[1].xyz + b[0].x*a[1].xyz + cross(b[0].yzw, a[1].xyz) + cross(b[1].xyz, a[0].yzw) - b[1].w*a[0].yzw - a[1].w*b[0].yzw, 
//          a[0].x*b[1].w + b[0].x*a[1].w + dot(a[0].yzw, b[1].xyz) + dot(a[1].xyz, b[0].yzw));
// }
//struct Screen {
//    gridSize: i32,
//    posX: f32,
//    posY: f32,
//    width: f32,
//    height: f32
//};
//struct output {
//    @builtin(position) position: vec4f,
//    @location(0) xy: vec2f,
//};

fn rotX(p: vec3f, a: f32) -> vec3f { let s = sin(a); let c = cos(a); let r = p.yz * mat2x2f(c, s, -s, c); return vec3f(p.x, r.x, r.y); }
fn rotY(p: vec3f, a: f32) -> vec3f { let s = sin(a); let c = cos(a); let r = p.zx * mat2x2f(c, s, -s, c); return vec3f(r.y, p.y, r.x); }
fn rotM(p: vec3f, m: vec2f) -> vec3f { return rotY(rotX(p, 3.14159265 * m.y), 2. * 3.14159265 * m.x); }

//@group(0) @binding(0) var<uniform> screen: Screen;
//@group(0) @binding(1) var<storage, read_write> circles: array<f32>;

fn sdfSphere(origin: vec3f, radius: f32, pt: vec3f) -> f32 {
    return length(origin - pt) - radius;    
}
fn normal(origin: vec3f, radius: f32, p: vec3f) -> vec3f {
    let e = vec2f(0., 0.0001);
    
    return normalize(vec3f(
        sdfSphere(origin, radius, p + e.yxx) - sdfSphere(origin, radius, p - e.yxx),
        sdfSphere(origin, radius, p + e.xyx) - sdfSphere(origin, radius, p - e.xyx),
        sdfSphere(origin, radius, p + e.xxy) - sdfSphere(origin, radius, p - e.xxy)
    ));
}

// fn funA(value: f32) {
//   if (value == 0.0) {}
//   else { funA(value - 1.0); }
// }

// fn jumpTable(address: u32, arg1: u32, arg2: u32, arg3: u32, arg4: u32) -> i32 {
//     while (address != 0) {
//         switch (address) {
//             case 0: { return 0; }
//         }
//     }
//     return 0;
// }

var<private> counter: f32 = 0.0;


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
    
    let n = i32(in_vertex_index);
    var vsOutput: output;
    vsOutput.position = vec4f(pos[n], 0.0, 1.0);
    vsOutput.xy = vsOutput.position.xy;
    return vsOutput;
}

fn calculateDistance(shape: vec3f, point: vec3f) -> f32 {
    // return length((1.0 * shape) - point) - 1.0;
    var minDistance = 1000.0;
    var minSquareDistance = 1000.0 * 1000.0;
    // let j = 1;
    // var results = array<f32, 16>(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    let scale = 16;
    // var results = array<f32, 64>();
    for (var i = 0; i < scale; i++) {
    for (var j = 0; j < scale; j++) {
        let dx = (shape.x * 10.0 + (f32(i) * 8.0)) - point.x;
        let dy = (shape.y * 10.0 + (f32(j) * 8.0)) - point.y;
        let dz = shape.z - point.z;
        let squared = (dx * dx) + (dy * dy) + (dz * dz);
        // results[(i * 8) + j] = squared;
        minSquareDistance = min(squared, minSquareDistance);
        // minSquareDistance = min(, minSquareDistance);
        // let distance = length(vec3f(shape.x * 10.0 + (f32(i) * 8.0), shape.y * 10.0 + (f32(j) * 8.0), shape.z) - point) - 1.0;
        // minDistance = min(distance, minDistance);
        // return minDistance;
        // return length((1.0 * shape) - point) - 1.0;
        // if (distance < minDistance) { 
            // minDistance = distance; 
        // }
        // if (minDistance <= 0.001) { return minDistance; }
    }
    }
    // for (var i = 0; i < scale * scale; i++) {
    //     minSquareDistance = min(results[i], minSquareDistance);
    // }
    return sqrt(minSquareDistance) - 1.0;
    // return sqrt(minSquareDistance) - 1.0;
    // return minDistance;
}

// fn fs_main(@builtin(position) p: vec4<f32>) -> @location(0) vec4<f32> {
// const shapes = array(vec3f(0, 0, 0));
fn newDistance(point: vec3f) -> f32 {
    // let len = arrayLength(circles);
    let len = 2048 / 4 / 4;
    let gridSize = screen.gridSize;
    var minSquaredDistance = 1000.0 * 1000.0;
    for (var i = 0; i < gridSize; i++) {
    for (var j = 0; j < gridSize; j++) {
        let index = 4 * ((i * gridSize) + j);
        let px = circles[index];
        let py = circles[index + 1];
        // let px = circles[0];
        // let py = circles[1];
        // let px = 0.0;
        // let py = 0.0;
        let dx = px - point.x;
        let dy = py - point.y;
        let dz = 0.0 - point.z;
        let squared = (dx * dx) + (dy * dy) + (dz * dz);
        minSquaredDistance = min(squared, minSquaredDistance);
    }
    }
    return sqrt(minSquaredDistance) - 0.1;
}
var<private> stack: array<f32, 102400>;
@fragment
fn fs_main(fsInput: output) -> @location(0) vec4<f32> {
    let r = 1.0;
    let pixelsPerMeter = 500.0;
    let ratio = screen.height / screen.width;
    let width = screen.width;
    let height = screen.height;
//    let x = fsInput.xy.x * (screen.width / 2.0 / pixelsPerMeter);
//    let y = fsInput.xy.y * (screen.height / 2.0 / pixelsPerMeter);
    let x = fsInput.xy.x * (screen.width / pixelsPerMeter);
    let y = fsInput.xy.y * (screen.height / pixelsPerMeter); // (screen.height / 2.0 / pixelsPerMeter);
    let shapeOrigin = vec3f(screen.posX, screen.posY, 20.0);
//    let shapeOrigin = vec3f(0.0, 0.0, 5.0);
    let cameraOrigin = vec3f(0.0, 0.0, -2.0);
    let f = 2.0;
    var p = vec3f(x, y, f) + cameraOrigin;
    let rotatedShapeOrigin = rotM(shapeOrigin, vec2f(0.0, 0.0));

    var offset = rotatedShapeOrigin - p;
    // var distance = length(offset) - r;
    // var distance = calculateDistance(shapeOrigin, p);
    var distance = newDistance(p);

    let dir = normalize(p - cameraOrigin);
    var iterations = 0;
    var maxDistance = 0.0;
    for (var i = 0; i <= 100; i++) {
        counter += 1.0;
        stack[i] = counter;
        p = p + (dir * distance);
        // distance = length(rotatedShapeOrigin - p) - r;
        // distance = calculateDistance(shapeOrigin, p);
        distance = newDistance(p);
        // iterations = i;
        // iterations++;
        if (distance > maxDistance) {
            maxDistance = distance;
            iterations = i;
        }
        if (distance >= 20.0) { break; }
        if (distance <= 0.001) { break; }
        // maxDistance = max(maxDistance, distance);
    }
    let n = normalize(normal(shapeOrigin, r, p));
    let color = (n + vec3f(1.0, 1.0, 1.0)) / 2.0;
    if (distance <= 0.001) {
//        return vec4(n.x, 1.0, 0.0, 1.0);
        return vec4(color.x, color.y, color.z, 1.0);
        // return vec4(f32(iterations) / 1000.0, 0.0, 0.0, 0.0);
        // return vec4(f32(iterations) / 1000.0, 0.0, 0.0, 0.0);
    } else {
//        return vec4(0.0, (fsInput.xy.xy + vec2(1.0, 1.0)) / 2.0, 1.0);
        // return vec4(f32(iterations) / 1000.0, maxDistance / 1000.0, 0.0, 0.0);
        return vec4(f32(iterations) / 1000.0, 0.0, counter / 10.0, 0.0);
    }
//    if (sqrt((x * x) + (y * y)) > r) {
//        return vec4(0.0, (fsInput.xy.xy + vec2(1.0, 1.0)) / 2.0, 1.0);
//    } else {
//        return vec4(0.0, 1.0, 0.0, 1.0);
//    }
}