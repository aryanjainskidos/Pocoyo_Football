#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;


public class PocoyoAnimationAtlasRegion
{
	private Sprite _texture;
	private Rect _region;
	private Vector2 _offset;


	public PocoyoAnimationAtlasRegion(Texture2D texture, BinaryReader data)
	{
		_region = new Rect(data.ReadInt16(), data.ReadInt16(), data.ReadInt16(), data.ReadInt16());
		_offset = new Vector2( data.ReadInt16(), data.ReadInt16() );

		if (_region.width > 0 && _region.height > 0)
		{
			_texture = Sprite.Create(texture, _region, _offset);
        }
	}

    public void  SaveAsAsset(string proj_path)
    {
        var ti = AssetImporter.GetAtPath(proj_path) as TextureImporter;
        ti.spritePixelsPerUnit = _texture.pixelsPerUnit;
        ti.mipmapEnabled = false;
        ti.textureType = TextureImporterType.Sprite;

        EditorUtility.SetDirty(ti);
        ti.SaveAndReimport();
    }
}

#endif
