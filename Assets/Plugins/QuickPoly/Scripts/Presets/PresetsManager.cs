using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System;

namespace QuickPoly
{
	[XmlRoot("PresetsCollection")]
	public class PresetsContainer
	{
		public const string fileName = "PresetsLibrary";
		public static string presetSavePath = string.Format("{0}/QuickPoly/Resources/{1}.xml", Application.dataPath, fileName);

		[XmlArray("PresetsGO")]
		[XmlArrayItem("PresetsRecord")]
		public List<PresetsRecord> PresetsGOList = new List<PresetsRecord>();

		public void Save()
		{
			Save(presetSavePath);
		}
		public void Save(string path)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(PresetsContainer));
			if (!File.Exists(path))
			{
				File.CreateText(path);
			}
			using (FileStream stream = new FileStream(path, FileMode.Create))
			{
				serializer.Serialize(stream, this);
			}

		}

		public static PresetsContainer Load()
		{
		#if UNITY_EDITOR
			return Load(presetSavePath);
		#else
			TextAsset xx = Resources.Load<TextAsset>(fileName);
			return LoadFromText(xx.text);
		#endif
		}

		public static PresetsContainer Load(string path)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(PresetsContainer));

			if (!File.Exists(path))
			{
				File.CreateText(path);
				return new PresetsContainer();
			}
			else
			{
				using (FileStream stream = new FileStream(path, FileMode.Open))
				{
					try
					{
						return serializer.Deserialize(stream) as PresetsContainer;
					}
					catch
					{
						return new PresetsContainer();
					}
				}
			}

		}
		public static PresetsContainer LoadFromText(string text)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(PresetsContainer));
			return serializer.Deserialize(new StringReader(text)) as PresetsContainer;
		}

		public PresetsRecord GetPresetByID(int id)
		{
			return PresetsGOList.Count > id ? PresetsGOList[id] : null;
		}

		public PresetsRecord GetMostUsedPreset()
		{
			return GetMostUsed(PresetsGOList);
		}

		private static PresetsRecord GetMostUsed(List<PresetsRecord> list)
		{
			if (list.Count == 0)
			{
				return null;
			}else if (list.Count == 1)
			{
				return list[0];
			}
			int maxID = 0;
			for (int i = 1; i < list.Count; i++)
			{
				maxID = list[maxID].timesUsed>= list[i].timesUsed ? maxID : i;
			}
			return list[maxID];
		}

		public bool PresetExist(string presetName)
		{
			return PresetExist(PresetsGOList, presetName);
		}
		public bool PresetExist(PresetsRecord preset)
		{
			return PresetExist(PresetsGOList, preset);
		}

		private static bool PresetExist(List<PresetsRecord> list, string presetName)
		{
			if (list.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].name == presetName)
				{
					return true;
				}
			}
			return false;
		}
		private static bool PresetExist(List<PresetsRecord> list, PresetsRecord presetRecord)
		{
			if (list.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsTheSame(presetRecord))
				{
					return true;
				}
			}
			return false;
		}
		
		public PresetsRecord FindPresetByName(string presetName)
		{
			return FindPresetByName(PresetsGOList, presetName);
		}

		private static PresetsRecord FindPresetByName(List<PresetsRecord> list, string name)
		{
			if (list.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].name == name)
				{
					return list[i];
				}
			}
			return null;
		}

		public void RemovePreset(string presetName)
		{
			PresetsRecord presetToDelete = FindPresetByName(presetName);
			if (presetToDelete == null)
			{
				throw new Exception(MSG.Errors.PRESET_DELETE_NOT_EXIST);
			}
			PresetsGOList.Remove(presetToDelete);
			Save();
		}
		
		public void AddNewRecord(PresetsRecord presetRecord)
		{
			PresetsGOList.Add(presetRecord);
			this.Save();
		}

		internal int GetObjectID(PresetsRecord loadedPreset)
		{
			if (loadedPreset == null || !PresetExist(loadedPreset))
			{
				return -1;
			}
			return PresetsGOList.IndexOf(loadedPreset);
		}

		public int GetIDByName(string presetName)
		{
			PresetsRecord x = FindPresetByName(presetName);
			if (x != null)
			{
				return GetObjectID(x);
			}
			else
			{
				return -1;
			}
		}
	}
	public class PresetsRecord
	{
		[XmlAttribute("name")]
		public string name;

		public int meshType;
		public int timesUsed = 0;
		public string polySerialised;

		public PresetsRecord()
		{
			meshType = 0;
			name = "NoName";
			timesUsed = 0;
			polySerialised = "";
		}
		public PresetsRecord(string presetName,QuickPolygon shape)
		{
			this.name = presetName;
			this.meshType = (int)shape.ShapeMeshIndex;
			this.timesUsed = 0;
			this.polySerialised = SerializationClass.Serialize(shape);
		}

		public void Update(QuickPolygon shape)
		{
            this.polySerialised = SerializationClass.Serialize(shape);
			this.meshType = (int) shape.ShapeMeshIndex;
		}

		public bool IsTheSame(PresetsRecord p)
		{
			if (this.name != p.name)
			{
				return false;
			}
			if (this.meshType != p.meshType)
			{
				return false;
			}
			if (this.polySerialised != p.polySerialised)
			{
				return false;
			}
			return true;
		}
	}
	public class PresetsManager
	{
		public static PresetsContainer presetsContainer = new PresetsContainer();

		private static int lastGOpreset = 0;

		public static void RefreshPresetContainer()
		{
			presetsContainer = PresetsContainer.Load();
		}

		public static PresetsRecord GetMostUsedPresetRecord()
		{
			if (presetsContainer == null || presetsContainer.PresetsGOList.Count < 1)
			{
				RefreshPresetContainer();
			}
			return presetsContainer.GetMostUsedPreset();
		}
		
		public static string GetMostUsedPreset()
		{
			if (presetsContainer == null || presetsContainer.PresetsGOList.Count < 1)
			{
				RefreshPresetContainer();
			}
			PresetsRecord mostUsedRecord = presetsContainer.GetMostUsedPreset();
			if (mostUsedRecord != null)
			{
				return mostUsedRecord.polySerialised;
			}
			else
			{
				return "";
			}
		}

		public static PresetsRecord GetLastUsedPreset()
		{
			return presetsContainer.GetPresetByID(lastGOpreset);
		}
		
		public static bool IsPresetExists(PresetsRecord presetsRecord)
		{
			return presetsContainer.PresetExist(presetsRecord);
		}
		public static bool IsPresetExists(string presetName)
		{
			return presetsContainer.PresetExist(presetName);
		}
		
		public static void DeletePreset(string presetName)
		{
			presetsContainer.RemovePreset(presetName);
		}
		
		public static void SavePreset(string presetName, QuickPolygon shape)
		{
			PresetsRecord presetRecord = presetsContainer.FindPresetByName(presetName);
			if (presetRecord == null)
			{
				presetRecord = new PresetsRecord(presetName, shape);
				presetsContainer.AddNewRecord(presetRecord);
			}
			else
			{
				presetRecord.Update(shape);
				presetsContainer.Save();
			}
			shape.CurrentPresetName = presetName;
			RefreshPresetContainer();
		}

		public static void LoadPresetFor(string presetName, QuickPolygon shape, bool loadMesh = false)
		{
			PresetsRecord loadedPreset = presetsContainer.FindPresetByName(presetName);
			LoadPresetFor(loadedPreset, shape, loadMesh);
		}
		public static void LoadPresetFor(PresetsRecord presetRecord, QuickPolygon shape, bool loadMesh = false)
		{
			if (presetRecord == null)
			{
				throw new Exception(MSG.Errors.PRESET_LOAD_NOT_EXIST);
			}
			else
			{
				lastGOpreset = presetsContainer.GetObjectID(presetRecord);
				presetRecord.timesUsed++;
                SerializationClass.Deserialize(shape, presetRecord.polySerialised, shape.AllowLoadPresetGeometry);
				shape.CurrentPresetName = presetRecord.name;
				if (loadMesh)
				{
					shape.ShapeMeshIndex = (MeshType)presetRecord.meshType;
				}
			}
		}
	}
}