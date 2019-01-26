namespace HomeTakeover.Editor
{
    
    using UnityEngine;
    using UnityEditor;

    public class ImportSprites : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            if (assetPath.Contains("png"))
            {   
                TextureImporter textureImporter  = (TextureImporter)assetImporter;
                textureImporter.spritePixelsPerUnit = 32;

                textureImporter.maxTextureSize = 512;
                textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
            }
        }
    }
}
