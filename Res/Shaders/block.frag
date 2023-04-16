#version 330 core
in vec2 fUv;

uniform sampler2D uTexture0;
uniform vec4 uHighlight;

out vec4 FragColor;

void main()
{
    vec4 tex = texture2D(uTexture0, fUv);
    FragColor = uHighlight + vec4(tex.rgb/tex.a, tex.a) * tex.a;
}