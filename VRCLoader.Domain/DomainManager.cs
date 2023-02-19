using System;
using System.Reflection;
using VRCLoader.Utils;

namespace VRCLoader.Domain;

internal sealed class DomainManager : AppDomainManager, INetDomain
{
	public override void InitializeNewDomain(AppDomainSetup appDomainInfo)
	{
		base.InitializationFlags = AppDomainManagerInitializationOptions.RegisterWithHost;
	}

	[Obfuscation(Exclude = true, ApplyToMembers = false)]
	public void Initialize()
	{
		VRCLoader.Load();
	}

	[Obfuscation(Exclude = true, ApplyToMembers = false)]
	public void OnApplicationStart()
	{
		StartLoadModules();
	}

	[Obfuscation(Exclude = true, ApplyToMembers = false)]
	public static void StartLoadModules()
	{
		VRCLoader._self.Start();
	}

	public void MinHook_CreateInstance(IntPtr mVRC_CreateHook, IntPtr mVRC_RemoveHook, IntPtr mVRC_EnableHook, IntPtr mVRC_DisableHook)
	{
		MinHook.CreateInstance(mVRC_CreateHook, mVRC_RemoveHook, mVRC_EnableHook, mVRC_DisableHook);
	}
}
