using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class lvch_mst_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/Resources/Data/lvch_mst.xls";
    private static readonly string[] sheetNames = { "lvch_mst", };
    
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            if (!filePath.Equals(asset))
                continue;

            using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read))
            {
                var book = new HSSFWorkbook(stream);

                foreach (string sheetName in sheetNames)
                {
                    var exportPath = "Assets/Resources/Data/" + sheetName + ".asset";
                    
                    // check scriptable object
                    var data = (Entity_lvch_mst)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_lvch_mst));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_lvch_mst>();
                        AssetDatabase.CreateAsset((ScriptableObject)data, exportPath);
                        data.hideFlags = HideFlags.NotEditable;
                    }
                    data.param.Clear();

					// check sheet
                    var sheet = book.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        Debug.LogError("[QuestData] sheet not found:" + sheetName);
                        continue;
                    }

                	// add infomation
                    for (int i=1; i<= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        ICell cell = null;
                        
                        var p = new Entity_lvch_mst.Param();
			
					cell = row.GetCell(0); p.id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.typ = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.min = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.max = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.lv1 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.lv2 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.lv3 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.lv4 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.lv5 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.lv6 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.lv7 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.lv8 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.lv9 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.lv10 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.lv11 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.lv12 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.lv13 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(17); p.lv14 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(18); p.lv15 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(19); p.lv16 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(20); p.lv17 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(21); p.lv18 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(22); p.lv19 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(23); p.lv20 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(24); p.lv21 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(25); p.lv22 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(26); p.lv23 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(27); p.lv24 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(28); p.lv25 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(29); p.lv26 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(30); p.lv27 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(31); p.lv28 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(32); p.lv29 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(33); p.lv30 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(34); p.lv31 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(35); p.lv32 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(36); p.lv33 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(37); p.lv34 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(38); p.lv35 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(39); p.lv36 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(40); p.lv37 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(41); p.lv38 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(42); p.lv39 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(43); p.lv40 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(44); p.lv41 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(45); p.lv42 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(46); p.lv43 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(47); p.lv44 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(48); p.lv45 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(49); p.lv46 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(50); p.lv47 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(51); p.lv48 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(52); p.lv49 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(53); p.lv50 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(54); p.lv51 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(55); p.lv52 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(56); p.lv53 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(57); p.lv54 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(58); p.lv55 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(59); p.lv56 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(60); p.lv57 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(61); p.lv58 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(62); p.lv59 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(63); p.lv60 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(64); p.lv61 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(65); p.lv62 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(66); p.lv63 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(67); p.lv64 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(68); p.lv65 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(69); p.lv66 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(70); p.lv67 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(71); p.lv68 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(72); p.lv69 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(73); p.lv70 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(74); p.lv71 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(75); p.lv72 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(76); p.lv73 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(77); p.lv74 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(78); p.lv75 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(79); p.lv76 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(80); p.lv77 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(81); p.lv78 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(82); p.lv79 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(83); p.lv80 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(84); p.lv81 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(85); p.lv82 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(86); p.lv83 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(87); p.lv84 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(88); p.lv85 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(89); p.lv86 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(90); p.lv87 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(91); p.lv88 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(92); p.lv89 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(93); p.lv90 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(94); p.lv91 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(95); p.lv92 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(96); p.lv93 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(97); p.lv94 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(98); p.lv95 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(99); p.lv96 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(100); p.lv97 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(101); p.lv98 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(102); p.lv99 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(103); p.lv100 = (int)(cell == null ? 0 : cell.NumericCellValue);

                        data.param.Add(p);
                    }
                    
                    // save scriptable object
                    ScriptableObject obj = AssetDatabase.LoadAssetAtPath(exportPath, typeof(ScriptableObject)) as ScriptableObject;
                    EditorUtility.SetDirty(obj);
                }
            }

        }
    }
}
