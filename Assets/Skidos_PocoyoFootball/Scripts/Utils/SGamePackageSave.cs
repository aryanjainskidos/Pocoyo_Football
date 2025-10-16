using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SGamePackageSave {

	private static SGamePackageSave current = null;

    public bool m_IsGameBought;

	public bool m_IsMicrophoneShow;

	public int CurrentUniform;
	public int[] CustomIdx;

	public uint teamLockStatusFlags;


	public static SGamePackageSave GetInstance()
	{
		if (current == null)
		{
			current = new SGamePackageSave();
            SSaveLoad.pkgInstance = current;
		}

		return current;
	}

	public void setSave(SGamePackageSave save)
	{
		current = save;
	}

	public SGamePackageSave()
	{
		m_IsGameBought = false;
		CurrentUniform = 0;
		CustomIdx = new int[21] { 0, 0, 0, 0, 0, 0, 0,
									0, 0, 0, 0, 0, 0, 0,
									0, 0, 0, 0, 0, 0, 0 };
		teamLockStatusFlags = 0;
	}
}
