using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Windows;

public class ExportHeightmap : MonoBehaviour
{
    #if UNITY_EDITOR
    public Terrain terrain;

    [Button]
    public void Export()
    {
        if (terrain == null) terrain = GetComponent<Terrain>();
        if (terrain == null) return;

        TerrainData terrainData = terrain.terrainData;

        int terrainWidth = terrainData.heightmapResolution;
        int terrainHeight = terrainData.heightmapResolution;

        float[,] heights = terrainData.GetHeights(0, 0, terrainWidth, terrainHeight);
        Texture2D texture = new Texture2D(terrainWidth, terrainHeight, TextureFormat.RGBA32, false);

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                float heightValue = heights[y, x];
                texture.SetPixel(x, y, new Color(heightValue, heightValue, heightValue));
            }
        }

        texture.Apply();

        // Save the texture to a PNG file
        byte[] pngData = texture.EncodeToPNG();
        if (pngData != null)
        {
            string path = Application.dataPath + "/Scenes/BlackHarborIsle/ExportedHeightmap.png";
            File.WriteAllBytes(path, pngData);
            Debug.Log("Heightmap exported to: " + path);
        }
    }
    #endif
}