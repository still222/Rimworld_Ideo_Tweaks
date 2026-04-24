using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace StkIdeoNerf;

[HarmonyPatch(typeof(IdeoFoundation), nameof(IdeoFoundation.CanAdd))]
public static class Patch_IdeoFoundation_CanAdd
{
	[HarmonyTranspiler]
	public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
	{
		var codes = new List<CodeInstruction>(instructions);

		for (int i = 0; i < codes.Count; i++)
		{
			// Anchor: ldstr "MaxMultiRolesCount"
			if (codes[i].opcode == OpCodes.Ldstr &&
				codes[i].operand is string str &&
				str == "MaxMultiRolesCount")
			{
				// Replace message value (ldc.i4.2 after the string)
				if (i + 1 < codes.Count && codes[i + 1].opcode == OpCodes.Ldc_I4_2)
				{
					codes[i + 1].opcode = OpCodes.Ldc_I4_3;
				}

				// Replace limit check (ldc.i4.2 before the branch)
				// From your dump it's at i - 2
				if (i - 2 >= 0 && codes[i - 2].opcode == OpCodes.Ldc_I4_2)
				{
					codes[i - 2].opcode = OpCodes.Ldc_I4_3;
				}
				break;
			}
		}

		return codes;
	}
}