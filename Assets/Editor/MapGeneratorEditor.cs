using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGenerator), true)]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI(){
        MapGenerator mapGen = (MapGenerator)target;
        if (DrawDefaultInspector()){
            if (mapGen.autoUpdate){
                mapGen.DrawMap();
            }
        }

        if(GUILayout.Button ("Generate")){
            mapGen.DrawMap();
        }
        // if(GUILayout.Button ("Detroy Shrubs")){
        //     mapGen.destroyPrefabs();
        // }
    }
}
