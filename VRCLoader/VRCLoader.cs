using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using VRCLoader.Modules;
using VRCLoader.Utils;

namespace VRCLoader;

public class VRCLoader
{
	internal class ServerResponce
	{
		public string Username { get; set; }

		public string IsStaff { get; set; }

		public string AquiredVIA { get; set; }

		public string UUID { get; set; }

		public bool AllowMal { get; set; }

		public string ServerUpdates { get; set; }

		public string message { get; set; }
	}

	internal class Post
	{
		public string Key { get; set; }

		public string Hwid { get; set; }

		public string BETA { get; set; }

		public string LoaderVersion { get; set; }

		public string ClientTime { get; set; }
	}

	internal static VRCLoader _self { get; set; }

	private ModuleManager _moduleManager { get; set; }

	[DllImport("kernel32.dll")]
	private static extern int AllocConsole();

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool SetForegroundWindow(IntPtr hWnd);

	[DllImport("kernel32.dll")]
	private static extern IntPtr GetConsoleWindow();

	private static void ShowConsole()
	{
		SetForegroundWindow(GetConsoleWindow());
	}

	public static void Load()
	{
		AllocConsole();
		Console.SetOut(new StreamWriter(Console.OpenStandardOutput())
		{
			AutoFlush = true
		});
		Console.SetIn(new StreamReader(Console.OpenStandardInput()));
		Console.Clear();
		Console.Title = "YokochoClient Loader";
		ShowConsole();
		Console.CursorVisible = false;
		Console.OutputEncoding = Encoding.UTF8;
		_self = new VRCLoader();
		_self.Awake();
	}

	public void Awake()
	{
		_moduleManager = new ModuleManager();
		int count = _moduleManager.FindModules().Count;
		Logs.Log("{0} module{1} loaded.", count, (count == 1) ? "" : "s");
	}

	public void Start()
	{
		Console.WriteLine("Done Loading Mods");
		foreach (VRModule module in _moduleManager.Modules)
		{
			try
			{
				module.type.GetMethod("Main").Invoke(null, null);
			}
			catch
			{
			}
		}
	}
}
