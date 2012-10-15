using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class AssetProcessorAlpha : AssetPostprocessor {
	
	void OnPostprocessTexture(Texture2D texture)
	{
		// premultiply		
		Color32[] colors = texture.GetPixels32(0);
		for(int i = 0; i < colors.Length; ++i) {
			byte a = colors[i].a;
			if ( a != 255 ) {
				if ( a == 0 ) {
					colors[i].r = 0;
					colors[i].g = 0;
					colors[i].b = 0;
				}
				else {
					colors[i].r = (byte)(((colors[i].r * a) + 255) >> 8);
					colors[i].g = (byte)(((colors[i].g * a) + 255) >> 8);
					colors[i].b = (byte)(((colors[i].b * a) + 255) >> 8);
				}
			}
		}
		texture.SetPixels32(colors,0);
	}
}

public class AssetProcessorAudio : AssetPostprocessor {
	
	private uint version = 3;
    public override uint GetVersion() {return version;}

    void OnPreprocessAudio()
    {
		// default audio settings
        AudioImporter audioImporter = (AudioImporter)assetImporter;
        audioImporter.format = AudioImporterFormat.Compressed;
        audioImporter.threeD = false;
        audioImporter.forceToMono = true;
        audioImporter.compressionBitrate = 64000; // 128000
    }

    void OnPostprocessAudio(AudioClip clip)
    {
        var audioImporter = (AudioImporter)assetImporter;
        if (clip.length >= 1.5f) {
            audioImporter.loadType = AudioImporterLoadType.StreamFromDisc;
        }
        else {
            audioImporter.loadType = AudioImporterLoadType.CompressedInMemory;
        }
    }
}
