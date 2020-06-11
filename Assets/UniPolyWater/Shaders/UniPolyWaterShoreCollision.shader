Shader "AutonoMoe/UniPolyWater Transparent with Shore and Collision" 
{
	Properties
	{
        _Color("Color", Color) = (1, 1, 1, 1)
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.5
        _ShoreColor("Shore Color", Color) = (1, 1, 1, 1)
		_ShoreThresholdMax("Shore Threshold Max", Float) = 1
		
		_GIntensity("Per vertex displacement", Float) = 1.0
		_GAmplitude("Wave Amplitude", Vector) = (0.3 ,0.35, 0.25, 0.25)
		_GFrequency("Wave Frequency", Vector) = (1.3, 1.35, 1.25, 1.25)
		_GSteepness("Wave Steepness", Vector) = (1.0, 1.0, 1.0, 1.0)
		_GSpeed("Wave Speed", Vector) = (1.2, 1.375, 1.1, 1.5)
		_GDirectionAB("Wave Direction", Vector) = (0.3 ,0.85, 0.85, 0.25)
		_GDirectionCD("Wave Direction", Vector) = (0.1 ,0.9, 0.5, 0.5)

		_RandomHeight("Random height", Float) = 0.5
		_RandomSpeed("Random Speed", Float) = 0.5

        _CollisionWaveLength("Collision Wave Lenght", Float) = 1
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent-1" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

		CGPROGRAM
			#pragma surface surf Standard vertex:vert alpha:premul
			#pragma target 4.0
			#include "UnityCG.cginc"
			            
				uniform float _GIntensity;
				uniform float4 _GAmplitude;
				uniform float4 _GFrequency;
				uniform float4 _GSteepness;
				uniform float4 _GSpeed;
				uniform float4 _GDirectionAB;
				uniform float4 _GDirectionCD;
				uniform float _RandomHeight;
				uniform float _RandomSpeed;

                float _CollisionWaveLength;
                float4 _CollisionVectors[10];
                float _CollisionWaveOffsets[10];
				
				uniform float _ShoreThresholdMax;

                half rand(float3 co)
                {
                    return frac(sin(dot(co.xyz ,half3(12.9898,78.233,45.5432))) * 43758.5453);
                }
                
                float3 GerstnerOffset(float2 xzVtx, float4 steepness, float4 amp, float4 freq, float4 speed, float4 dirAB, float4 dirCD, float rHeight, float rSpeed, float4 Time)
                {
                    float3 offsets;

                    float4 AB = steepness.xxyy * amp.xxyy * dirAB.xyzw;
                    float4 CD = steepness.zzww * amp.zzww * dirCD.xyzw;

                    float4 dotABCD = freq.xyzw * half4(dot(dirAB.xy, xzVtx), dot(dirAB.zw, xzVtx), dot(dirCD.xy, xzVtx), dot(dirCD.zw, xzVtx));
                    float timeOffset = rHeight * rand(xzVtx.xyy);
                    Time += half4(timeOffset, timeOffset, timeOffset, timeOffset);
                    float4 TIME = Time.yyyy * speed;

                    float4 COS = cos(dotABCD + TIME);
                    float4 SIN = sin(dotABCD + TIME);

                    offsets.x = dot(COS, half4(AB.xz, CD.xz));
                    offsets.z = dot(COS, half4(AB.yw, CD.yw));
                    offsets.y = dot(SIN, amp);
               
                    offsets.y += rHeight * 0.2 * cos(Time.y * rSpeed * sin(rand(xzVtx.xyy)) + sin(cos(rand(xzVtx.xyy))));
               
                    return offsets;
                }
            
                float CollisionOffset(float2 xzVtx)
                {
                    float collPhase = 0;

                    for (int i = 0; i < 10; i++)
                    {
                        float distanceToCenter = length(xzVtx - _CollisionVectors[i].xy);
                        float waveHeight = _CollisionVectors[i].z;
                        float waveState = _CollisionVectors[i].w;

                        if (distanceToCenter < _CollisionWaveLength * 7 + _CollisionWaveOffsets[i] && distanceToCenter > _CollisionWaveOffsets[i])
                        {
                            collPhase -= waveHeight * sin(3.14 * waveState) * sin((distanceToCenter * 3.14) / (_CollisionWaveLength * 7 + _CollisionWaveOffsets[i])) * sin((distanceToCenter * 9.42) / (_CollisionWaveLength * 7 + _CollisionWaveOffsets[i]) - (waveState * 6.28)) * min(1.0, (min(1.0, (waveState * 2.0)) + (1.0 - (distanceToCenter / (_CollisionWaveLength * 7 + _CollisionWaveOffsets[i])))));
                        }
                    }
                    return collPhase;
                }

				//Structure of Surface shader input
				struct Input {
					float3 normal;
					float4 screenPos;
					float4 vertToSurf;
				};

				void vert(inout appdata_full v, out Input o) {
					//Initialize the output
					UNITY_INITIALIZE_OUTPUT(Input, o);


                    float4 v0 = v.vertex;
                    //Calculate position of triangles two other verts (from UV)
                    float v1x_off = 0.0;
                    float v1z_off = 0.0;
                    float v2x_off = 0.0;
                    float v2z_off = 0.0;
                    if(v.texcoord2.x < 0.1)
                    {
                        v1x_off = 0.0;
                        v1z_off = 1.0;
                        v2x_off = 1.0;
                        v2z_off = 0.0;
                    }
                    else if(v.texcoord2.x < 1.1)
                    {
                        v1x_off = -1.0;
                        v1z_off = 1.0;
                        v2x_off = 0.0;
                        v2z_off = 1.0;
                    }
                    else if(v.texcoord2.x < 2.1)
                    {
                        v1x_off = 1.0;
                        v1z_off = 0.0;
                        v2x_off = 1.0;
                        v2z_off = -1.0;
                    }
                    else if(v.texcoord2.x < 3.1)
                    {
                        v1x_off = -1.0;
                        v1z_off = 0.0;
                        v2x_off = -1.0;
                        v2z_off = 1.0;
                    }
                    else if(v.texcoord2.x < 4.1)
                    {
                        v1x_off = 0.0;
                        v1z_off = 1.0;
                        v2x_off = 1.0;
                        v2z_off = 0.0;
                    }
                    else
                    {
                        v1x_off = 1.0;
                        v1z_off = -1.0;
                        v2x_off = 0.0;
                        v2z_off = -1.0;
                    }

                    float4 v1 = v0 + float4(-v1x_off, 0.0, -v1z_off, 0.0);
                    float4 v2 = v0 + float4(-v2x_off, 0.0, -v2z_off, 0.0);

                    //Vertex world space coordinates
                    float3 worldv0 = mul(unity_ObjectToWorld, v0).xyz;
                    float3 worldv1 = mul(unity_ObjectToWorld, v1).xyz;
                    float3 worldv2 = mul(unity_ObjectToWorld, v2).xyz;

                    //Generate phase shift for the three verts based on the verts world position
                    float3 offset0 = GerstnerOffset(worldv0.xz, _GSteepness, _GAmplitude, _GFrequency, _GSpeed, _GDirectionAB, _GDirectionCD, _RandomHeight, _RandomSpeed, _Time);
                    float3 offset1 = GerstnerOffset(worldv1.xz, _GSteepness, _GAmplitude, _GFrequency, _GSpeed, _GDirectionAB, _GDirectionCD, _RandomHeight, _RandomSpeed, _Time);
                    float3 offset2 = GerstnerOffset(worldv2.xz, _GSteepness, _GAmplitude, _GFrequency, _GSpeed, _GDirectionAB, _GDirectionCD, _RandomHeight, _RandomSpeed, _Time);
                    
                    float collOffset0 = CollisionOffset(worldv0.xz);
                    float collOffset1 = CollisionOffset(worldv1.xz);
                    float collOffset2 = CollisionOffset(worldv2.xz);

                    v0.xyz += _GIntensity * mul((float3x3)unity_WorldToObject, offset0);
                    v1.xyz += _GIntensity * mul((float3x3)unity_WorldToObject, offset1);
                    v2.xyz += _GIntensity * mul((float3x3)unity_WorldToObject, offset2);
                    
                    v0.y += collOffset0;
                    v1.y += collOffset1;
                    v2.y += collOffset2;

                    //Calculate the new vertex normal as the cross product of the three new positions
                    float3 vn = normalize(cross(v1 - v0, v2 - v0));
                    
					float sinVal = sin(worldv0.x / rand(0.5 + abs(worldv0.z))) + sin(worldv0.z / rand(0.5 + abs(worldv0.x)));
					o.vertToSurf.x = _ShoreThresholdMax + _ShoreThresholdMax * 0.1 * sin(_Time.y + sinVal);
					//o.vertToSurf.y = _ShoreThresholdMax * 0.95 + _ShoreThresholdMax * 0.55 * sin(_Time.y * 2.1 + sinVal);
					//o.vertToSurf.z = _ShoreThresholdMax * 0.3 + _ShoreThresholdMax * 0.4 * sin(_Time.y * 2.3 + sinVal);

					v.normal = vn;
					v.vertex.xyz = v0.xyz;

					o.normal = v.normal;
					o.vertToSurf.w = -UnityObjectToViewPos(v.vertex).z;

				}

				fixed4 _Color;
                fixed4 _ShoreColor;
                half _Smoothness;
                half _Metallic;

				uniform sampler2D _CameraDepthTexture;

				void surf(Input IN, inout SurfaceOutputStandard o) {

                    //Calculate the actual depth of the screen space position of the center of the vertex-triangle
					float sceneZ = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)).r);

					//Actual distance to the triangles center position
					float partZ = IN.vertToSurf.w;

					//If the two are similar, then there is an object intersecting with our object
					float absDist = abs(sceneZ - partZ);
					float diff = absDist / IN.vertToSurf.x;
					//float diff2 = absDist / IN.vertToSurf.y;
					//float diff3 = absDist / IN.vertToSurf.z;
					
					if(diff <= 1)  // && (diff2 > 1 || diff3 <= 1))
                    {
                        o.Albedo = _ShoreColor.rgb; //lerp(_ShoreColor, _Color, diff);
                        o.Alpha = 1;
					}
                    else
                    {
                        o.Albedo = _Color.rgb;
                        o.Alpha = _Color.a;
                    }
                    o.Smoothness = _Smoothness;
                    o.Metallic = _Metallic;
				}
			ENDCG
	}
	Fallback "Diffuse"
}