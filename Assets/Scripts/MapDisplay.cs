using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
   public MeshFilter meshFilter;
   public MeshRenderer meshRenderer;

   public void DrawMesh(MeshData meshData, Texture2D texture){
       MapGenerator mapGenerator = new MapGenerator();
       meshFilter.sharedMesh = meshData.CreateMesh(mapGenerator);
       meshRenderer.sharedMaterial.mainTexture = texture;
   }
}
