// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33015,y:32125,varname:node_3138,prsc:2|emission-5016-RGB,alpha-3261-OUT;n:type:ShaderForge.SFN_TexCoord,id:9077,x:32039,y:32392,varname:node_9077,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:4076,x:32415,y:32392,varname:node_4076,prsc:2|A-8869-OUT,B-8131-OUT;n:type:ShaderForge.SFN_Sin,id:3261,x:32599,y:32392,varname:node_3261,prsc:2|IN-4076-OUT;n:type:ShaderForge.SFN_Slider,id:8312,x:31880,y:32252,ptovrint:False,ptlb:velocity,ptin:_velocity,varname:_node_8312,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.625641,max:20;n:type:ShaderForge.SFN_Pi,id:60,x:32072,y:32636,varname:node_60,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8131,x:32223,y:32576,varname:node_8131,prsc:2|A-3708-OUT,B-60-OUT;n:type:ShaderForge.SFN_Add,id:8869,x:32223,y:32392,varname:node_8869,prsc:2|A-8856-OUT,B-9077-U;n:type:ShaderForge.SFN_Time,id:2344,x:32037,y:32092,varname:node_2344,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8856,x:32223,y:32229,varname:node_8856,prsc:2|A-2344-T,B-8312-OUT;n:type:ShaderForge.SFN_Vector1,id:3708,x:32039,y:32576,varname:node_3708,prsc:2,v1:5;n:type:ShaderForge.SFN_Color,id:5016,x:32543,y:32078,ptovrint:False,ptlb:color,ptin:_color,varname:node_5016,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;proporder:8312-5016;pass:END;sub:END;*/

Shader "k014/SnapLineShader" {
    Properties {
        _velocity ("velocity", Range(0, 20)) = 1.625641
        _color ("color", Color) = (1,0,0,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _velocity;
            uniform float4 _color;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float3 emissive = _color.rgb;
                float3 finalColor = emissive;
                float4 node_2344 = _Time;
                return fixed4(finalColor,sin((((node_2344.g*_velocity)+i.uv0.r)*(5.0*3.141592654))));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
