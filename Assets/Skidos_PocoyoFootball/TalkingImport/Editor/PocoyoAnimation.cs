#if UNITY_EDITOR
using System.IO;
using System.Numerics;


public class PocoyoAnimation
{
    public const float FRAME_TIME = (1.0f / 12.0f);

	// Datos.
	private string _id;
	private string _file;
	//private var _audio:Audio;
	private bool _streaming;
	private int[] _frames;
	private PocoyoAnimationAtlas[] _atlases;
	// Variables de control.
	private BinaryReader _fs;
	private int _fsPosition;
	private int _frame;
	private float _time;
	private int _loops;
	private bool _playing;
	private bool _pause;
	private bool _loadNextFrame;
	// Debug.
	internal int loadMin = int.MaxValue;
	internal int loadMax;
	internal int loadMed;
	private int loadMedFrames;
	private MemoryStream _data;

	public PocoyoAnimation(string id, string fileName, bool streaming)
	{
		_id = id;
		_file = fileName;
		//_audio = audio;
		_streaming = streaming;
		_frame = -1;
		// Carga la animaci�n.
		if (!_streaming)
		{
			_data = new MemoryStream();
			FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			byte[] bytes = new byte[file.Length];
			file.Read(bytes, 0, (int)file.Length);
			_data.Write(bytes, 0, (int)file.Length);
			_data.Position = 0;
			file.Close();

			_fs = new BinaryReader(_data);
			readHeader();
			foreach(var atlas in _atlases)
			{
				atlas.load(_fs);
			}
			_fs = null;
		}
	}

	private void readHeader()
	{
		_frames= new int[_fs.ReadByte()];
		for (int i = 0; i<_frames.Length; i++)
		{
			_frames[i]=(int)_fs.ReadByte();
		}
		_atlases = new PocoyoAnimationAtlas[_fs.ReadByte()];
		for (int i = 0; i < _atlases.Length; i++)
		{
			_atlases[i] = new PocoyoAnimationAtlas();
		}
	}


	public void SaveAll()
	{
		for (int i = 0; i < _atlases.Length; i++)
		{
			PocoyoAnimationAtlas atlas = _atlases[i];

			atlas.Save(_file + i.ToString());
		}
	}	
}

#endif
