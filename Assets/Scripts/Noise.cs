using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset){
        float[,] noiseMap = new float[mapWidth, mapHeight];
        System.Random rand = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        
        float maxHeight = 0;
        float amplitude = 1;
        float freq = 1;
        
        for(int i=0;i<octaves;i++){
            float offSetX = rand.Next(-100000, 100000) + offset.x;
            float offSetZ = rand.Next(-100000, 100000) - offset.y;
            octaveOffsets[i] = new Vector2(offSetX, offSetZ);

            maxHeight += amplitude;
            amplitude *= persistance;
        }
        if (scale<=0){
            scale=0.0001f;
        }
        float maxLocalNoiseHeight = float.MinValue;
        float minLocalNoiseHeight = float.MaxValue; 
        float halfWidth = mapWidth/2f;
        float halfHeight = mapHeight/2f;
        for(int z=0; z<mapHeight; z++){
            for (int x=0; x<mapWidth; x++){
                amplitude = 1;
                freq = 1;
                float noiseHeight = 0;
                for (int o=0; o<octaves; o++){
                    float sampleX = (float)(x-halfWidth + octaveOffsets[o].x)/scale * freq;
                    float sampleZ = (float)(z-halfHeight + octaveOffsets[o].y)/scale * freq;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ)*2-1;
                    noiseHeight += perlinValue*amplitude;
                    
                    amplitude *= persistance;
                    freq *= lacunarity;
                }
                if (noiseHeight>maxLocalNoiseHeight){
                    maxLocalNoiseHeight = noiseHeight;
                }
                else if(noiseHeight< minLocalNoiseHeight){
                    minLocalNoiseHeight = noiseHeight;
                }
                noiseMap[x, z] = noiseHeight;
            }
        }
        for(int z=0; z<mapHeight; z++){
            for (int x=0; x<mapWidth; x++){
                float normalizedHeight = (noiseMap[x, z]+1)/(2f * maxHeight/1.75f);
                noiseMap[x, z] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
            }
        }
        return noiseMap;
    }
}
