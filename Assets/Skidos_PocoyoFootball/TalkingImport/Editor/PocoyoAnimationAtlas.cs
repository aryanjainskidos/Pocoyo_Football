#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEditor;

public class PocoyoAnimationAtlas
{
	private MemoryStream _data;
	private Texture2D _rgb;
	private Texture2D _rgb2;
	private Texture2D _argb;
	private PocoyoAnimationAtlasRegion _base;
	private PocoyoAnimationAtlasRegion _ghost;
	private PocoyoAnimationAtlasRegion[] _logos;
	private PocoyoAnimationAtlasRegion _maskTshirt;
	private PocoyoAnimationAtlasRegion _maskTshirtHorizontal;
	private PocoyoAnimationAtlasRegion _maskTshirtVertical;
	private PocoyoAnimationAtlasRegion _maskTshirtLeft;
	private PocoyoAnimationAtlasRegion _maskTshirtRight;
	private PocoyoAnimationAtlasRegion _maskNeck;
	private PocoyoAnimationAtlasRegion _maskArmLeft;
	private PocoyoAnimationAtlasRegion _maskArmRight;
	private PocoyoAnimationAtlasRegion _maskLine1Left;
	private PocoyoAnimationAtlasRegion _maskLine1Right;
	private PocoyoAnimationAtlasRegion _maskLine2Left;
	private PocoyoAnimationAtlasRegion _maskLine2Right;
	private PocoyoAnimationAtlasRegion _maskLine3Left;
	private PocoyoAnimationAtlasRegion _maskLine3Right;
	private PocoyoAnimationAtlasRegion _maskZip;
	private PocoyoAnimationAtlasRegion _shoes02Left;
	private PocoyoAnimationAtlasRegion _shoes02Right;
	private PocoyoAnimationAtlasRegion _shoes03Left;
	private PocoyoAnimationAtlasRegion _shoes03Right;

	public void load(BinaryReader fs)
    {
        // Datos del fichero.
        if (_data == null)
        {
            _data = new MemoryStream();
            uint size = fs.ReadUInt32();
            byte[] temp = new byte[size];
            fs.Read(temp, 0, temp.Length);
            _data.Write(temp,0, temp.Length);
            _data.Position = 0;
        }
        // Texturas.
        if (_rgb == null)
        {
            // Datos descomprimidos.
            DeflateStream dataUncompressed = new DeflateStream(_data, CompressionMode.Decompress);
            MemoryStream o = new MemoryStream();
            dataUncompressed.CopyTo(o);

            BinaryReader output = new BinaryReader(o);

            _rgb = new Texture2D(output.ReadInt16(), output.ReadInt16(), TextureFormat.RGB24, false);
            
            // Texturas RGB.
            byte[] pixels = output.ReadBytes(output.ReadInt32());
            _rgb.LoadRawTextureData(pixels);

            if (output.ReadBoolean())
            {
                _rgb2 = new Texture2D(output.ReadInt16(), output.ReadInt16(), TextureFormat.RGB24, false);
                byte[] pixels2 = output.ReadBytes(output.ReadInt32());
                _rgb2.LoadRawTextureData(pixels2);
            }
            
            // Textura ARGB.
            _argb = new Texture2D(output.ReadInt16(), output.ReadInt16(), TextureFormat.ARGB32, false);
            pixels = output.ReadBytes(output.ReadInt32());
            _argb.LoadRawTextureData(pixels);

            // Regiones.
            _base = new PocoyoAnimationAtlasRegion(_rgb, output);
            _ghost = new PocoyoAnimationAtlasRegion(_argb, output);
            _logos = new PocoyoAnimationAtlasRegion[33];
            for (int i = 0; i < _logos.Length; i++)
			{
                _logos[i] = new PocoyoAnimationAtlasRegion( (_rgb2 != null && (i == 4 || i == 16)) ? _rgb2 : _rgb, output);
            }

            _maskTshirt = new PocoyoAnimationAtlasRegion(_argb, output);
            _maskTshirtHorizontal = new PocoyoAnimationAtlasRegion(_argb, output);
            _maskTshirtVertical = new PocoyoAnimationAtlasRegion(_argb, output);
            _maskTshirtLeft = new PocoyoAnimationAtlasRegion(_argb, output);
            _maskTshirtRight = new PocoyoAnimationAtlasRegion(_argb, output);
            _maskNeck = new PocoyoAnimationAtlasRegion(_argb, output);
            _maskArmLeft = new PocoyoAnimationAtlasRegion(_argb, output);
            _maskArmRight = new PocoyoAnimationAtlasRegion(_argb, output);
            _maskLine1Left = new PocoyoAnimationAtlasRegion(_argb, output);
            _maskLine1Right = new PocoyoAnimationAtlasRegion(_argb, output);
            _maskLine2Left = new PocoyoAnimationAtlasRegion(_argb, output);
            _maskLine2Right = new PocoyoAnimationAtlasRegion(_argb, output);
            _maskLine3Left = new PocoyoAnimationAtlasRegion(_argb, output);
            _maskLine3Right = new PocoyoAnimationAtlasRegion(_argb, output);
            _maskZip = new PocoyoAnimationAtlasRegion(_argb, output);
            _shoes02Left = new PocoyoAnimationAtlasRegion(_argb, output);
            _shoes02Right = new PocoyoAnimationAtlasRegion(_argb, output);
            _shoes03Left = new PocoyoAnimationAtlasRegion(_argb, output);
            _shoes03Right = new PocoyoAnimationAtlasRegion(_argb, output);
        }
    }

    public void Save(string fileName)
    {
        var dirPath = Application.dataPath + "/Imported/";

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        byte[] bytes = _rgb.EncodeToPNG();
        string filename = dirPath + "Base_" + fileName + ".png";
        File.WriteAllBytes(filename, bytes);


        string filename2 = dirPath + "Base_" + fileName + ".png";
        if (_rgb2 != null)
        {
            bytes = _rgb2.EncodeToPNG();
            File.WriteAllBytes(filename, bytes);
        }

        bytes = _argb.EncodeToPNG();
        string filename3 = dirPath + "Ghost_" + fileName + ".png";
        File.WriteAllBytes(filename3, bytes);

        AssetDatabase.Refresh();

        _base.SaveAsAsset(filename);
        _ghost.SaveAsAsset(filename2);

        for (int i = 0; i < _logos.Length; i++)
        {
            if(_rgb2 != null && (i == 4 || i == 16))
            {
                _logos[i].SaveAsAsset(filename2);
            }
            else
            {
                _logos[i].SaveAsAsset(filename3);
            }
        }

        _maskTshirt.SaveAsAsset(filename3);
        _maskTshirtHorizontal.SaveAsAsset(filename3);
        _maskTshirtVertical.SaveAsAsset(filename3);
        _maskTshirtLeft.SaveAsAsset(filename3);
        _maskTshirtRight.SaveAsAsset(filename3);
        _maskNeck.SaveAsAsset(filename3);
        _maskArmLeft.SaveAsAsset(filename3);
        _maskArmRight.SaveAsAsset(filename3);
        _maskLine1Left.SaveAsAsset(filename3);
        _maskLine1Right.SaveAsAsset(filename3);
        _maskLine2Left.SaveAsAsset(filename3);
        _maskLine2Right.SaveAsAsset(filename3);
        _maskLine3Left.SaveAsAsset(filename3);
        _maskLine3Right.SaveAsAsset(filename3);
        _maskZip.SaveAsAsset(filename3);
        _shoes02Left.SaveAsAsset(filename3);
        _shoes02Right.SaveAsAsset(filename3);
        _shoes03Left.SaveAsAsset(filename3);
        _shoes03Right.SaveAsAsset(filename3);
    }
}



#endif