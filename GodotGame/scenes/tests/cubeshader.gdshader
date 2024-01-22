shader_type spatial;

float fmod(float t, float div) {
    float remainder = t;
    while (remainder > div) {
        remainder -= div;
    }
    return remainder;
}

float triangle(float phi) {
    float t = fmod(phi, 2.0);
    if (t <= 0.5) { return t * 2.0; }
    else if (t >= 0.5 && t <= 1.5) { return 2.0 - (2.0 * t); }
    else { return -2.0 * (2.0 - t); }
}

void vertex() {
	
}

void fragment() {
	// ALBEDO = vec3(0.5 * (1.0 + triangle(TIME)), 0.4, 0.7); //, 1);
	ALBEDO = vec3(0, 0.5 * (1.0 + triangle(TIME * 0.25)), 1); //, 1);
	// ALBEDO = vec3(0, 0, triangle(TIME));
}