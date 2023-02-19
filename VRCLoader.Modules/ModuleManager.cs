using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using VRCLoader.Attributes;
using VRCLoader.Utils;

namespace VRCLoader.Modules;

public sealed class ModuleManager
{
	private List<VRModule> _modules { get; } = new List<VRModule>();


	public ReadOnlyCollection<VRModule> Modules => _modules.AsReadOnly();

	internal ReadOnlyCollection<VRModule> FindModules(string path = "Modules\\")
	{
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
			Logs.Log("Modules folder does not exist.");
			return Modules;
		}
		string[] files = Directory.GetFiles(path);
		foreach (string path2 in files)
		{
			if (!(Path.GetExtension(path2) == ".dll"))
			{
				continue;
			}
			Assembly assembly;
			try
			{
				assembly = Assembly.Load(File.ReadAllBytes(path2));
			}
			catch (Exception)
			{
				Logs.Log("Error loading \"{0}\". Are you sure this is a valid assembly?", Path.GetFileName(path2));
				continue;
			}
			foreach (Type item in from t in assembly.GetTypes()
				where t.IsSubclassOf(typeof(VRModule))
				select t)
			{
				if (item.GetCustomAttributes(typeof(ModuleInfoAttribute), inherit: true).FirstOrDefault() is ModuleInfoAttribute moduleInfo)
				{
					VRModule vRModule = new VRModule(item);
					_modules.Add(vRModule);
					vRModule.Initialize(moduleInfo, this);
					Logs.Log("{0} loaded.", vRModule);
				}
			}
		}
		return Modules;
	}

	public void UnloadModule(VRModule module)
	{
		if (_modules.Contains(module))
		{
			_modules.Remove(module);
			Logs.Log("{0} unloaded.", module);
		}
	}
}
