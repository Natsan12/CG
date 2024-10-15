Shader "MyShaders/MyUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TinteColor("Tint Color",Color)=(1,1,1,1)
        _Transparency("Transparency", Range (0.0,0.5))= 0.25
        _Cutout("Cutout", Range (0.0,1.0)) = 0.2
        _Distance("Distance", float) =  1
        _Amplitude("Amplitude", float) = 1
        _Speed ("Speed", float) = 1
        _MiniBaseAmount ( " Min Base Amount",Range (0.0,1.0)) = 0.1
        _MaxBaseAmount("Max Min Base Amount", Range (0.0,1.0))=1.0
        _Interval ("Interval", float) = 5 
        _SpikeDuration ("Spike Duration", float) = 0.5 
        _MinSpikeChance ("Min Spike Chance", float) = 0.05 
        _MaxSpikeChance ("Max Spike Chance", float) = 0.3 
        _TextureScrollSpeed ("Texture Scroll Speed", float)=1
        _GlowColor ("Glo Color",Color)= (1,1,1,1)
        _GlowIntensity ("Glow Intensity", Range (0.0,10.0)) =5
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _TinteColor;
            float _Transparency;
            float _Cutout;
            float _Distance;
            float _Amplitude;
            float _Speed;
            float _MinBaseAmount;
            float _MaxBaseAmount;
            float _Interval; // New interval variable
            float _SpikeDuration;
            float _MinSpikeChance;
            float _MaxSpikeChance;
            float _TextureScrollSpeed;
            float _GlowColor;
            float _GlowIntensity;

            float rand(float seed) {
                return frac(sin(seed)* 43758.5453);
            }


            float GetRandomSpikeChance(float seed) {
                return lerp(_MinSpikeChance, _MaxSpikeChance, rand(seed));
            }


            float GetRandomBaseAmount(float seed){
                return lerp(_MinBaseAmount, _MaxBaseAmount, rand(seed));
                }

            float GetRandomAmount(float time) {
            float randomTime = floor(time / _Interval) * _Interval; // calculate random time interval
            float spikeChance = GetRandomSpikeChance(randomTime);   // Pass the randomTime to get spike chance
            float spikeTrigger = rand(randomTime + 1.0);      

        if (spikeTrigger < spikeChance) {
        // If within spike duration, return random Base Amount
             float timeInInterval = frac(time / _Interval) * _Interval;
            if (timeInInterval < _SpikeDuration) {
            return GetRandomBaseAmount(randomTime + 2.0); // Different seed for base amount
            }   
        }       
    
    // Otherwise, return zero
        return 0.0;
    }
            v2f vert (appdata v)
            {
                v2f o;
                float randomAmount = GetRandomAmount(_Time.y);
                v.vertex. x += sin(_Time.y * _Speed + v. vertex.y * _Amplitude)* _Distance * randomAmount;
                
                o.uv = v.uv;
                o.uv.y += _Time.y * _TextureScrollSpeed;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) +_TinteColor;
                col.a = _Transparency;  
                clip(col.rgb-_Cutout);

                fixed4 glow = _GlowColor * _GlowIntensity;
                col.rgb += glow.rgb * col.a;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
    SubShader
    {
    Tags { "RenderType"="Opaque" }
    Pass
    {
        Name "EMISSION"
        Tags { "LightMode" = "Always" }

        CGPROGRAM
        #pragma vertex vertEmission
        #pragma fragment fragEmission
        #include "UnityCG.cginc"

        struct appdata
        {
            float4 vertex : POSITION;
        };

        struct v2f
        {
            float4 vertex : SV_POSITION;
        };
            
        float4 _GlowColor;
        float _GlowIntensity;
        sampler2D _MainTex;

        v2f vertEmission(appdata v)
        
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            return o;
        }

        fixed4 fragEmission(v2f i) : SV_Target
        {
            // Emit a strong glow color with proper alpha modulation
            float alpha = tex2D(_MainTex, i.vertex.xy).a;
            return (_GlowColor * _GlowIntensity) * alpha;
            
          }
            ENDCG
        }
    }
}