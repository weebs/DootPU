shader_type canvas_item;

void vertex() {
	
}

void fragment() {
	COLOR = vec4(FRAGCOORD.x * SCREEN_PIXEL_SIZE.x * (0.5 * (cos(TIME) + 1.0)), 0, 0, 1);
}