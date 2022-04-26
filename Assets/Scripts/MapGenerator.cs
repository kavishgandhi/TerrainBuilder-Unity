using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class MapGenerator : MonoBehaviour
{
    public GameObject shrubPrefab;
    public TerrainType[] regions = new TerrainType[8];
    public List<Vector3> gos_pos = new List<Vector3>();
    public const int mapChunkSize = 239; //actual mesh is 240x240
    int levelDetail = 0;
    int seed = 42;
    float scale = 24;
    int octaves = 5;
    float persistance = 0.5f;
    float lacunarity = 2;
    public Vector2 offset;
    float heightMulti = 24;
    public AnimationCurve meshHeightCurve;
    public bool autoUpdate = true;
    Queue<mapThreadInfo<MapData>> mapDataThreadInfoQ = new Queue<mapThreadInfo<MapData>>();
    Queue<mapThreadInfo<MeshData>> meshDataThreadInfoQ = new Queue<mapThreadInfo<MeshData>>();
    MeshGenerator meshGenerator = new MeshGenerator();
    public void DrawMap(){
        MapData mapData = GenerateMapData(Vector2.zero); 
        MapDisplay display = FindObjectOfType<MapDisplay>();   
        display.DrawMesh(meshGenerator.generateTerrainMesh(mapData.heightMap, heightMulti, meshHeightCurve, levelDetail), TextureGenerator.TextureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize));
    }

    public void requestMapData(Vector2 center, Action<MapData> callback){
        ThreadStart threadStart = delegate{
            mapDataThread(center, callback);
        };

        new Thread(threadStart).Start();
    }
    void mapDataThread(Vector2 center, Action<MapData> callback){
        MapData mapData = GenerateMapData(center);
        lock (mapDataThreadInfoQ){
            mapDataThreadInfoQ.Enqueue(new mapThreadInfo<MapData>(callback, mapData));
        }
    }

    public void requestMeshData(MapData mapData, int lod, Action<MeshData> callback){
        ThreadStart threadStart = delegate {
			meshdataThread (mapData, lod, callback);
		};

		new Thread (threadStart).Start ();
    }
    void meshdataThread(MapData mapData, int lod, Action<MeshData> callback){
        MeshData meshData = meshGenerator.generateTerrainMesh (mapData.heightMap, heightMulti, meshHeightCurve, lod);
		lock (meshDataThreadInfoQ) {
			meshDataThreadInfoQ.Enqueue (new mapThreadInfo<MeshData> (callback, meshData));
		}
    }
    void Update(){
        if (mapDataThreadInfoQ.Count>0){
            for(int i=0;i<mapDataThreadInfoQ.Count; i++){
                mapThreadInfo<MapData> threadInfo = mapDataThreadInfoQ.Dequeue();
                threadInfo.callback(threadInfo.param);
            }
        }
        if (meshDataThreadInfoQ.Count>0){
            for(int i=0;i<meshDataThreadInfoQ.Count; i++){
                mapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQ.Dequeue();
                threadInfo.callback(threadInfo.param);
            }
        }
    }
    MapData GenerateMapData(Vector2 center){
        float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize+2, mapChunkSize+2, seed, scale, octaves, persistance, lacunarity, center+offset);
        
        regions[0].name="Water deep";
        regions[0].height=0f;
        regions[0].color=new Color(35/255f,137/255f,218/255f);
        regions[1].name="Water shallow";
        regions[1].height=0.27f;
        regions[1].color=new Color(28/255f,163/255f,236/255f);
        regions[2].name="Sand";
        regions[2].height=0.3f;
        regions[2].color=new Color(255/255f,229/255f,156/255f);
        regions[3].name="Grass";
        regions[3].height=0.36f;
        regions[3].color=new Color(57/255f,131/255f,0/255f);
        regions[4].name="Grass2";
        regions[4].height=0.42f;
        regions[4].color=new Color(16/255f,77/255f,6/255f);
        regions[5].name="Rock";
        regions[5].height=0.51f;
        regions[5].color=new Color(66/255f,58/255f,53/255f);
        regions[6].name="Rock2";
        regions[6].height=0.75f;
        regions[6].color=new Color(73/255f,64/255f,60/255f);
        regions[7].name="Snow";
        regions[7].height=0.81f;
        regions[7].color=new Color(255/255f,255/255f,255/255f);
        

        Color[] colorMap = new Color[mapChunkSize*mapChunkSize];
        for(int z=0; z<mapChunkSize; z++){
            for (int x=0; x<mapChunkSize; x++){
                float currHeight = noiseMap[x, z];
                for (int i=0; i<regions.Length; i++){
                    if(i==7){
                        if(gos_pos.Count<100){
                            gos_pos.Add(new Vector3(x-240/2,0,z-240/2));
                        }
                    }
                    if (currHeight>=regions[i].height){
                        colorMap[z*mapChunkSize+x] = regions[i].color;
                    }
                    else{
                        break;
                    }
                }
            }
        }
        return new MapData(noiseMap, colorMap);
    }
    struct mapThreadInfo<T>{
        public readonly Action<T> callback;
        public readonly T param;
        public mapThreadInfo(Action<T> callback, T param){
            this.callback = callback;
            this.param = param;
        }
    }
}

// [System.Serializable]
public struct TerrainType{
    public string name;
    public float height;
    public Color color;
}
public struct MapData{
    public readonly float[,] heightMap;
    public readonly Color[] colorMap;
    public MapData(float[,] heightMap, Color[] colorMap){
        this.heightMap = heightMap;
        this.colorMap = colorMap;
    }
}