using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class busyo_mst_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/Resources/Data/busyo_mst.xls";
    private static readonly string[] sheetNames = { "busyo_mst", };
    
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
                    var data = (Entity_busyo_mst)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_busyo_mst));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_busyo_mst>();
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
                        
                        var p = new Entity_busyo_mst.Param();
			
					cell = row.GetCell(0); p.id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.rank = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.heisyu = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.basehp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.baseatk = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.basedfc = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.hp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.atk = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.dfc = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.spd = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.minHp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.minAtk = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.minDfc = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.minSpd = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.ship = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.senpou_id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(17); p.saku_id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(18); p.GacyaFree = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(19); p.GacyaTama = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(20); p.daimyoId = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(21); p.daimyoHst = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(22); p.daimyoId1 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(23); p.daimyoHst1 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(24); p.daimyoId2 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(25); p.daimyoHst2 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(26); p.daimyoId3 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(27); p.daimyoHst3 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(28); p.nameEng = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(29); p.nameSChn = (cell == null ? "" : cell.StringCellValue);

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
