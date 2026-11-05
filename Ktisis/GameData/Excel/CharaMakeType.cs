// ReSharper disable all
#pragma warning disable CS8618

using System.Collections.Generic;

using Ktisis.Common.Extensions;

using Lumina.Data;
using Lumina.Excel;
using Lumina.Excel.Sheets;

namespace Ktisis.GameData.Excel;

// Temporarily forking CharaMakeType while FacialFeatureOption is bugged -
// It gets parsed as an int[8] rather than an int[8,7]. I've reached out to perchbird about this issue.

[Sheet( "CharaMakeType", columnHash: 0x50cdbeef)]
public partial struct CharaMakeType(uint row) : IExcelRow<CharaMakeType> {
        public ExcelPage ExcelPage { get; private set; }
        public uint RowOffset { get; private set; }
	public uint RowId { get; } = row;

	public struct CharaMakeStructStruct {
    	public RowRef<Lobby> Menu { get; internal set; }
    	public uint SubMenuMask { get; internal set; }
    	public uint Customize { get; internal set; }
    	public uint[] SubMenuParam { get; internal set; }
    	public byte InitVal { get; internal set; }
    	public byte SubMenuType { get; internal set; }
    	public byte SubMenuNum { get; internal set; }
    	public byte LookAt { get; internal set; }
    	public byte[] SubMenuGraphic { get; internal set; }
    }
    public struct EquipmentStruct {
    	public ulong Helmet { get; internal set; }
    	public ulong Top { get; internal set; }
    	public ulong Gloves { get; internal set; }
    	public ulong Legs { get; internal set; }
    	public ulong Shoes { get; internal set; }
    	public ulong Weapon { get; internal set; }
    	public ulong SubWeapon { get; internal set; }
    }
    
    public CharaMakeStructStruct[] CharaMakeStruct { get; private set; }
    public byte[] VoiceStruct { get; private set; }
    public int[,] FacialFeatureOption { get; private set; }
    public EquipmentStruct[] Equipment { get; private set; }
    public RowRef<Race> Race { get; private set; }
    public RowRef<Tribe> Tribe { get; private set; }
    public sbyte Gender { get; private set; }

	// Keep the plugin's array model while decoding with the API14 generated schema.
	static CharaMakeType IExcelRow<CharaMakeType>.Create(ExcelPage page, uint offset, uint row) {
        var native = new Lumina.Excel.Sheets.CharaMakeType(page, offset, row);
        var menus = new CharaMakeStructStruct[native.CharaMakeStruct.Count];
        for (var i = 0; i < menus.Length; i++) {
            var entry = native.CharaMakeStruct[i];
            menus[i] = new CharaMakeStructStruct {
                Menu = entry.Menu, SubMenuMask = entry.SubMenuMask, Customize = entry.Customize,
                SubMenuParam = System.Linq.Enumerable.ToArray(entry.SubMenuParam), InitVal = entry.InitVal,
                SubMenuType = entry.SubMenuType, SubMenuNum = entry.SubMenuNum, LookAt = entry.LookAt,
                SubMenuGraphic = System.Linq.Enumerable.ToArray(entry.SubMenuGraphic)
            };
        }
        var features = new int[8, 7];
        for (var i = 0; i < 8; i++) {
            var entry = native.FacialFeatureOption[i];
            int[] values = [entry.Option1, entry.Option2, entry.Option3, entry.Option4, entry.Option5, entry.Option6, entry.Option7];
            for (var j = 0; j < 7; j++) features[i, j] = values[j];
        }
        var equipment = new EquipmentStruct[native.Equipment.Count];
        for (var i = 0; i < equipment.Length; i++) {
            var entry = native.Equipment[i];
            equipment[i] = new EquipmentStruct { Helmet = entry.Helmet, Top = entry.Top, Gloves = entry.Gloves,
                Legs = entry.Legs, Shoes = entry.Shoes, Weapon = entry.Weapon, SubWeapon = entry.SubWeapon };
        }
        return new CharaMakeType(row) { ExcelPage = page, RowOffset = offset, CharaMakeStruct = menus,
            VoiceStruct = System.Linq.Enumerable.ToArray(native.VoiceStruct), FacialFeatureOption = features,
            Equipment = equipment, Race = native.Race, Tribe = native.Tribe, Gender = native.Gender };
    }
}
