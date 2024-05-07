shader_type canvas_item;


void fragment() {
    vec4 original_color = texture(TEXTURE, UV);
    vec4 final_color = vec4(1.0, 1.0, 1.0, original_color.a);
    COLOR = final_color;
}