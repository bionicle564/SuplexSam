Shader "FullScreen/waterBlend"
{
    HLSLINCLUDE

    #pragma vertex Vert

    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"

    // The PositionInputs struct allow you to retrieve a lot of useful information for your fullScreenShader:
    // struct PositionInputs
    // {
    //     float3 positionWS;  // World space position (could be camera-relative)
    //     float2 positionNDC; // Normalized screen coordinates within the viewport    : [0, 1) (with the half-pixel offset)
    //     uint2  positionSS;  // Screen space pixel coordinates                       : [0, NumPixels)
    //     uint2  tileCoord;   // Screen tile coordinates                              : [0, NumTiles)
    //     float  deviceDepth; // Depth from the depth buffer                          : [0, 1] (typically reversed)
    //     float  linearDepth; // View space Z coordinate                              : [Near, Far]
    // };

    // To sample custom buffers, you have access to these functions:
    // But be careful, on most platforms you can't sample to the bound color buffer. It means that you
    // can't use the SampleCustomColor when the pass color buffer is set to custom (and same for camera the buffer).
    // float4 CustomPassSampleCustomColor(float2 uv);
    // float4 CustomPassLoadCustomColor(uint2 pixelCoords);
    // float LoadCustomDepth(uint2 pixelCoords);
    // float SampleCustomDepth(float2 uv);

    // There are also a lot of utility function you can use inside Common.hlsl and Color.hlsl,
    // you can check them out in the source code of the core SRP package.

    StructuredBuffer<float3> _WaterPos;

    // Sample depth helper
    float SampleDepth(float2 uv)
    {
        return SampleCustomColor(uv);
    }
    
    // Simple 5-tap blur (cross pattern)
    float BlurDepth(float2 uv)
    {
        float2 pixel = float2(_ScreenSize.z, _ScreenSize.w);

        float d0 = SampleDepth(uv);
        float d1 = SampleDepth(uv + float2( pixel.x, 0));
        float d2 = SampleDepth(uv + float2(-pixel.x, 0));
        float d3 = SampleDepth(uv + float2(0,  pixel.y));
        float d4 = SampleDepth(uv + float2(0, -pixel.y));

        return (d0 + d1 + d2 + d3 + d4) / 5.0;
    }
    
    // Reconstruct a fake normal from depth differences
    float3 ReconstructNormal(float2 uv)
    {
        float2 pixel = float2(_ScreenSize.z, _ScreenSize.w);

        float dC = BlurDepth(uv);
        float dR = BlurDepth(uv + float2(pixel.x, 0));
        float dU = BlurDepth(uv + float2(0, pixel.y));

        float3 dx = float3(pixel.x, 0, dR - dC);
        float3 dy = float3(0, pixel.y, dU - dC);

        float3 n = normalize(cross(dx, dy));
        return n;
    }


    float4 FullScreenPass(Varyings varyings) : SV_Target
    {

        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(varyings);
        float depth = LoadCameraDepth(varyings.positionCS.xy);
        PositionInputs posInput = GetPositionInput(varyings.positionCS.xy, _ScreenSize.zw, depth, UNITY_MATRIX_I_VP, UNITY_MATRIX_V);
        float3 viewDirection = GetWorldSpaceNormalizeViewDir(posInput.positionWS);
        float4 color = float4(0.0, 0.0, 0.0, 0.0);

        // Load the camera color buffer at the mip 0 if we're not at the before rendering injection point
        if (_CustomPassInjectionPoint != CUSTOMPASSINJECTIONPOINT_BEFORE_RENDERING)
            color = float4(CustomPassLoadCameraColor(varyings.positionCS.xy, 0), 1);

        // Add your custom pass code here

        float4 custom = CustomPassLoadCustomColor(varyings.positionCS.xy);
        //return float4(custom.rgb, 1);

        float2 uv = varyings.positionCS.xy / _ScreenSize.xy;

        float originalDepth = SampleDepth(uv);
        float blurredDepth  = BlurDepth(uv);

        // Thickness approximation
        float thickness = saturate((blurredDepth - originalDepth) * 50.0);

        // Normal
        float3 normal = ReconstructNormal(uv);

        // Simple lighting
        float3 lightDir = normalize(float3(0.3, 0.7, 0.2));
        float ndotl = saturate(dot(normal, lightDir));

        // Fake water color
        float3 waterColor = float3(0.0, 0.3, 0.6);

        // Fresnel
        float fresnel = pow(1.0 - saturate(normal.z), 5.0);

        float3 col = waterColor * ndotl;
        col = lerp(col, float3(1,1,1), fresnel * 0.5);

        if (custom.r > 0.01)
        {
            return float4(col, .5f);
            //return custom;
        }

        return color;
        //return float4(col, thickness);

        // Fade value allow you to increase the strength of the effect while the camera gets closer to the custom pass volume
        // float f = 1 - abs(_FadeValue * 2 - 1);
        // return float4(color.rgb + f, color.a);
  
    }

    ENDHLSL

    SubShader
    {
        Tags{ "RenderPipeline" = "HDRenderPipeline" }
        Pass
        {
            Name "FullScreenPass"

            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            HLSLPROGRAM
                #pragma fragment FullScreenPass
            ENDHLSL
        }
    }
    Fallback Off
}
