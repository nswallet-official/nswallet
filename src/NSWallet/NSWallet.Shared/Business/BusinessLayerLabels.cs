using System.Collections.Generic;
using System.Linq;

namespace NSWallet.Shared
{
	public static partial class BL
	{
		public static void AddSystemLabels(bool reset = false)
		{
			var dal = DataAccessLayer.GetInstance();

			if (reset) {
				DeleteSystemLabels();
			} else {
				if (dal.Labels.Any()) return;
			}

			dal.CreateLabel(GConsts.FLDTYPE_MAIL, TR.TrEn(GConsts.FLDTYPE_MAIL), GConsts.FLDTYPE_MAIL_ICON, GConsts.VALUETYPE_MAIL, true);
			dal.CreateLabel(GConsts.FLDTYPE_EXPD, TR.TrEn(GConsts.FLDTYPE_EXPD), GConsts.FLDTYPE_EXPD_ICON, GConsts.VALUETYPE_DATE, true);
			dal.CreateLabel(GConsts.FLDTYPE_PASS, TR.TrEn(GConsts.FLDTYPE_PASS), GConsts.FLDTYPE_PASS_ICON, GConsts.VALUETYPE_PASS, true);
			dal.CreateLabel(GConsts.FLDTYPE_NOTE, TR.TrEn(GConsts.FLDTYPE_NOTE), GConsts.FLDTYPE_NOTE_ICON, GConsts.VALUETYPE_TEXT, true);
			dal.CreateLabel(GConsts.FLDTYPE_LINK, TR.TrEn(GConsts.FLDTYPE_LINK), GConsts.FLDTYPE_LINK_ICON, GConsts.VALUETYPE_LINK, true);
			dal.CreateLabel(GConsts.FLDTYPE_ACNT, TR.TrEn(GConsts.FLDTYPE_ACNT), GConsts.FLDTYPE_ACNT_ICON, GConsts.VALUETYPE_TEXT, true);
			dal.CreateLabel(GConsts.FLDTYPE_CARD, TR.TrEn(GConsts.FLDTYPE_CARD), GConsts.FLDTYPE_CARD_ICON, GConsts.VALUETYPE_TEXT, true);
			dal.CreateLabel(GConsts.FLDTYPE_NAME, TR.TrEn(GConsts.FLDTYPE_NAME), GConsts.FLDTYPE_NAME_ICON, GConsts.VALUETYPE_TEXT, true);
			dal.CreateLabel(GConsts.FLDTYPE_PHON, TR.TrEn(GConsts.FLDTYPE_PHON), GConsts.FLDTYPE_PHON_ICON, GConsts.VALUETYPE_PHON, true);
			dal.CreateLabel(GConsts.FLDTYPE_PINC, TR.TrEn(GConsts.FLDTYPE_PINC), GConsts.FLDTYPE_PINC_ICON, GConsts.VALUETYPE_TEXT, true);
			dal.CreateLabel(GConsts.FLDTYPE_USER, TR.TrEn(GConsts.FLDTYPE_USER), GConsts.FLDTYPE_USER_ICON, GConsts.VALUETYPE_TEXT, true);
			dal.CreateLabel(GConsts.FLDTYPE_OLDP, TR.TrEn(GConsts.FLDTYPE_OLDP), GConsts.FLDTYPE_OLDP_ICON, GConsts.VALUETYPE_TEXT, true);
			dal.CreateLabel(GConsts.FLDTYPE_DATE, TR.TrEn(GConsts.FLDTYPE_DATE), GConsts.FLDTYPE_DATE_ICON, GConsts.VALUETYPE_DATE, true);
			dal.CreateLabel(GConsts.FLDTYPE_TIME, TR.TrEn(GConsts.FLDTYPE_TIME), GConsts.FLDTYPE_TIME_ICON, GConsts.VALUETYPE_TIME, true);
			dal.CreateLabel(GConsts.FLDTYPE_SNUM, TR.TrEn(GConsts.FLDTYPE_SNUM), GConsts.FLDTYPE_SNUM_ICON, GConsts.VALUETYPE_TEXT, true);
			dal.CreateLabel(GConsts.FLDTYPE_ADDR, TR.TrEn(GConsts.FLDTYPE_ADDR), GConsts.FLDTYPE_ADDR_ICON, GConsts.VALUETYPE_TEXT, true);
			dal.CreateLabel(GConsts.FLDTYPE_SQUE, TR.TrEn(GConsts.FLDTYPE_SQUE), GConsts.FLDTYPE_SQUE_ICON, GConsts.VALUETYPE_TEXT, true);
			dal.CreateLabel(GConsts.FLDTYPE_SANS, TR.TrEn(GConsts.FLDTYPE_SANS), GConsts.FLDTYPE_SANS_ICON, GConsts.VALUETYPE_TEXT, true);

			// Appeared after upgrade to 2FA (<see cref="PrepareUpgradeTo2FA"/>).
			dal.CreateLabel(GConsts.FLDTYPE_2FAC, TR.TrEn(GConsts.FLDTYPE_2FAC), GConsts.FLDTYPE_ACNT_ICON, GConsts.VALUETYPE_TEXT, true);
		}

		public static void DeleteSystemLabels()
		{
			var dal = DataAccessLayer.GetInstance();
			var labels = dal.Labels;
			var systemLabels = labels.Where(x => x.Value.System == true);
			foreach (var systemLabel in systemLabels) {
				dal.RemoveLabelForReal(systemLabel.Value.FieldType);
			}
		}

		public static string AddLabel(string name, string icon, string valueType, string labelID = null, bool system = false)
		{
			var dal = DataAccessLayer.GetInstance();

			if (labelID == null) {
				labelID = Common.GenerateUniqueString(GConsts.LABELID_LENGTH);
			}

			var result = dal.CreateLabel(labelID, name, icon, valueType, system);

			if (!result) {
				labelID = "";
			}

			return labelID;
		}

		public static int RemoveLabel(string fieldType)
		{
			var dal = DataAccessLayer.GetInstance();
			var result = dal.RemoveLabel(fieldType);
			return result;
		}

		public static void UpdateLabelIcon(string fieldType, string icon)
		{
			var dal = DataAccessLayer.GetInstance();
			dal.UpdateLabelIcon(fieldType, icon);
		}

		public static void UpdateLabelTitle(string fieldType, string name)
		{
			var dal = DataAccessLayer.GetInstance();
			dal.UpdateLabelTitle(fieldType, name);
		}


		public static List<NSWLabel> GetLabels()
		{
			var labelsList = new List<NSWLabel>();
			var dal = DataAccessLayer.GetInstance();

			foreach (var label in dal.Labels) {
				labelsList.Add(label.Value);
			}

			var sortedList = labelsList.OrderByDescending(x => x.System).ThenBy(y => y.Name).ToList();

			return sortedList;
		}

		public static List<NSWLabel> GetLabelsByUsage()
		{
			var labelsList = new List<NSWLabel>();
			var dal = DataAccessLayer.GetInstance();

			foreach (var label in dal.Labels) {
				labelsList.Add(label.Value);
			}

			var sortedList = labelsList.OrderByDescending(y => y.Usage).ToList();

			var cleverlySortedList = new List<NSWLabel>();
			for (int i = 0; i < GConsts.LISTLENGTH_OF_CLEVER_LABELS && i < sortedList.Capacity; i++) {
				cleverlySortedList.Add(sortedList[i]);
			}

			sortedList = labelsList.OrderBy(y => y.Name).ToList();

			foreach(var label in sortedList) {
				if (cleverlySortedList.Exists(f => f.FieldType == label.FieldType)) continue;
				cleverlySortedList.Add(label);
			}

			return cleverlySortedList;
		}
	}
}
