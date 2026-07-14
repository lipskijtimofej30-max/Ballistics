using System.IO;
using UnityEngine;

namespace Game.Scripts.Core.Simulation
{
    public class GraphExporter
    {
        public void ExportToPNG(Camera graphCamera, int width, int height, string filePath)
        {
            RenderTexture renderTexture = new RenderTexture(width, height, 24);
        
            graphCamera.targetTexture = renderTexture;
            Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        
            graphCamera.Render();
            RenderTexture.active = renderTexture;
        
            screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenshot.Apply();
        
            graphCamera.targetTexture = null;
            RenderTexture.active = null;
            Object.Destroy(renderTexture);
        
            byte[] bytes = screenshot.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
        }
    }
}