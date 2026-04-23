using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace StkIdeoNerf;

[StaticConstructorOnStartup]
public static class Startup
{
	static Startup()
	{
		var harmony = new Harmony("stk.ideotweaks");
		//Check if anything else patches it
		var method = AccessTools.Method(typeof(IdeoFoundation), nameof(IdeoFoundation.CanAdd));
		var patchInfo = Harmony.GetPatchInfo(method);

		if (patchInfo != null)
		{
			var otherOwners = patchInfo.Prefixes
				.Concat(patchInfo.Postfixes)
				.Concat(patchInfo.Transpilers)
				.Select(p => p.owner)
				.Where(owner => owner != harmony.Id)
				.Distinct()
				.ToList();

			if (otherOwners.Count > 0)
			{
				Log.Warning("[StkIdeoTweaks] Other mods patch ideo role limits:");

				foreach (var owner in otherOwners)
				{
					Log.Warning($" - {owner}");
				}
			}
		}

		harmony.PatchAll();
	}
}