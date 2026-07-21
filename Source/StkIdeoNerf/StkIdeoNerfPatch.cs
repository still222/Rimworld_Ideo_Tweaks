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
		var code = new List<CodeInstruction>(instructions);

		for (int i = 0; i < code.Count; i++)
		{
			// Anchor: ldstr "MaxMultiRolesCount"
			if (code[i].opcode == OpCodes.Ldstr &&
				code[i].operand is string str &&
				str == "MaxMultiRolesCount")
			{
				// Replace message value (ldc.i4.2 after the string)
				code[i + 1].opcode = OpCodes.Ldc_I4_3;
				// Replace limit check (ldc.i4.2 before the branch)
				code[i - 2].opcode = OpCodes.Ldc_I4_3;

				break;
			}

		}

		return code;
	}

}
