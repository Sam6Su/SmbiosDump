using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SmbiosDump
{
    public partial class Form1 : Form
    {
        byte[] SmbiosRawData;
        List<string> SmbiosType;
        List<string> SmbiosTypeData;
        int AlignmentValue = 35;
        uint SmbiosRev;

        #region BIT_define
        const UInt64 BIT0 = 1;
        const UInt64 BIT1 = 2;
        const UInt64 BIT2 = 4;
        const UInt64 BIT3 = 8;
        const UInt64 BIT4 = 0x10;
        const UInt64 BIT5 = 0x20;
        const UInt64 BIT6 = 0x40;
        const UInt64 BIT7 = 0x80;

        const UInt64 BIT8 = 0x100;
        const UInt64 BIT9 = 0x200;
        const UInt64 BIT10 = 0x400;
        const UInt64 BIT11 = 0x800;

        const UInt64 BIT12 = 0x1000;
        const UInt64 BIT13 = 0x2000;
        const UInt64 BIT14 = 0x4000;
        const UInt64 BIT15 = 0x8000;

        const UInt64 BIT16 = 0x10000;
        const UInt64 BIT17 = 0x20000;
        const UInt64 BIT18 = 0x40000;
        const UInt64 BIT19 = 0x80000;

        const UInt64 BIT20 = 0x100000;
        const UInt64 BIT21 = 0x200000;
        const UInt64 BIT22 = 0x400000;
        const UInt64 BIT23 = 0x800000;

        const UInt64 BIT24 = 0x1000000;
        const UInt64 BIT25 = 0x2000000;
        const UInt64 BIT26 = 0x4000000;
        const UInt64 BIT27 = 0x8000000;

        const UInt64 BIT28 = 0x10000000;
        const UInt64 BIT29 = 0x20000000;
        const UInt64 BIT30 = 0x40000000;
        const UInt64 BIT31 = 0x80000000;

        const UInt64 BIT32 = 0x100000000;
        const UInt64 BIT33 = 0x200000000;
        const UInt64 BIT34 = 0x400000000;
        const UInt64 BIT35 = 0x800000000;
        const UInt64 BIT36 = 0x1000000000;
        const UInt64 BIT37 = 0x2000000000;
        const UInt64 BIT38 = 0x4000000000;
        const UInt64 BIT39 = 0x8000000000;

        const UInt64 BIT40 = 0x10000000000;
        const UInt64 BIT41 = 0x20000000000;
        const UInt64 BIT42 = 0x40000000000;
        const UInt64 BIT43 = 0x80000000000;

        const UInt64 BIT44 = 0x100000000000;
        const UInt64 BIT45 = 0x200000000000;
        const UInt64 BIT46 = 0x400000000000;
        const UInt64 BIT47 = 0x800000000000;

        const UInt64 BIT48 = 0x1000000000000;
        const UInt64 BIT49 = 0x2000000000000;
        const UInt64 BIT50 = 0x4000000000000;
        const UInt64 BIT51 = 0x8000000000000;

        const UInt64 BIT52 = 0x10000000000000;
        const UInt64 BIT53 = 0x20000000000000;
        const UInt64 BIT54 = 0x40000000000000;
        const UInt64 BIT55 = 0x80000000000000;

        const UInt64 BIT56 = 0x100000000000000;
        const UInt64 BIT57 = 0x200000000000000;
        const UInt64 BIT58 = 0x400000000000000;
        const UInt64 BIT59 = 0x800000000000000;

        const UInt64 BIT60 = 0x1000000000000000;
        const UInt64 BIT61 = 0x2000000000000000;
        const UInt64 BIT62 = 0x4000000000000000;
        const UInt64 BIT63 = 0x8000000000000000;
        #endregion

        public Form1()
        {
            InitializeComponent();
            Init();
            GetSmbiosRawData();
            SmbiosDecode();
            DisplayDeacodedDate();
        }

        private void Init()
        { 
            SmbiosRawData = null;
            SmbiosType = new List<string> { };
            SmbiosTypeData = new List<string> { };
            Lb_Types.SelectedIndex = 0;
            Rtb_Data.ReadOnly = true;
            Rtb_Data.Font = new Font("Lucida Console", 8);
            SmbiosRev = 0;
        }

        private void GetSmbiosRawData()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\Mssmbios\\Data"))
                {
                    if (key != null)
                    {
                        Object Obj = key.GetValue("SMBiosData");
                        if (Obj != null)
                        {
                            SmbiosRawData = (byte[])Obj;
                            int Index = 0;
                            Console.WriteLine("SMBIOS Raw Data:");
                            for (int i = 0; i < 0x10; i++)
                            {
                                Console.Write(i.ToString("X").PadLeft(2, '0'));
                                if (i == 7)
                                {
                                    Console.Write("-");
                                }
                                else if (i == 0xf)
                                {
                                    Console.WriteLine();
                                }
                                else
                                {
                                    Console.Write(" ");
                                }
                            }
                            for (int i = 0; i < 0x10; i++)
                            {
                                Console.Write("---");
                            }
                            Console.WriteLine();
                            foreach (byte bData in SmbiosRawData)
                            {
                                Console.Write(bData.ToString("X").PadLeft(2, '0'));
                                if (Index % 0x10 == 7)
                                {
                                    Console.Write("-");
                                }
                                else if (Index % 0x10 == 0xf)
                                {
                                    Console.WriteLine();
                                }
                                else
                                {
                                    Console.Write(" ");
                                }
                                Index++;
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void SmbiosDecode()
        {
            if (SmbiosRawData == null)
            {
                return;
            }
            int RawDataLength = SmbiosRawData.Length;
            int NextIndex = EntryDecode();
            for (int i = NextIndex; i < RawDataLength; i++)
            {
                switch (SmbiosRawData[i])
                {
                    case 0:
                        NextIndex = Type0Decode(i);
                        break;
                    case 1:
                        NextIndex = Type1Decode(i);
                        break;
                    case 2:
                        NextIndex = Type2Decode(i);
                        break;
                    case 3:
                        NextIndex = Type3Decode(i);
                        break;
                    case 4:
                        NextIndex = Type4Decode(i);
                        break;
                    case 7:
                        NextIndex = Type7Decode(i);
                        break;
                    case 8:
                        NextIndex = Type8Decode(i);
                        break;
                    case 11:
                        NextIndex = Type11Decode(i);
                        break;
                    case 12:
                        NextIndex = Type12Decode(i);
                        break;
                    case 14:
                        NextIndex = Type14Decode(i);
                        break;
                    case 16:
                        NextIndex = Type16Decode(i);
                        break;
                    case 17:
                        NextIndex = Type17Decode(i);
                        break;
                    case 19:
                        NextIndex = Type19Decode(i);
                        break;
                    case 20:
                        NextIndex = Type20Decode(i);
                        break;
                    case 24:
                        NextIndex = Type24Decode(i);
                        break;
                    case 25:
                        NextIndex = Type25Decode(i);
                        break;
                    case 27:
                        NextIndex = Type27Decode(i);
                        break;
                    case 28:
                        NextIndex = Type28Decode(i);
                        break;
                    case 41:
                        NextIndex = Type41Decode(i);
                        break;
                    case 43:
                        NextIndex = Type43Decode(i);
                        break;
                    case 127:
                        NextIndex = Type127Decode(i);
                        break;
                    default:
                        NextIndex = TypeUnknownDecode(i);
                        break;
                }
                i += NextIndex;

                if (i >= RawDataLength)
                {
                    break;
                }
            }
        }

        #region SmbiosTypeDecode
        private int EntryDecode()
        {
            string OutputStr = "[SMBIOS Entry]";
            UInt64 TempUint = 0;
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;
            OutputStr += "Major Version".PadRight(AlignmentValue);
            OutputStr += "0x" + SmbiosRawData[1].ToString("X").PadLeft(2,'0');
            OutputStr += Environment.NewLine;

            OutputStr += "Minor Version".PadRight(AlignmentValue);
            OutputStr += "0x" + SmbiosRawData[2].ToString("X").PadLeft(2, '0');
            OutputStr += Environment.NewLine;

            OutputStr += "DMI Revision".PadRight(AlignmentValue);
            OutputStr += "0x" + SmbiosRawData[3].ToString("X").PadLeft(2, '0');
            OutputStr += Environment.NewLine;

            OutputStr += "Length".PadRight(AlignmentValue);
            OutputStr += Bytes2String(4, 4, out TempUint);
            OutputStr += Environment.NewLine;

            SmbiosRev = (uint)SmbiosRawData[1] * 10 + (uint)SmbiosRawData[2];

            SmbiosTypeData.Add(OutputStr);
            return 8;
        }

        private string TypeStructDecode(int Offset, out int TypeTotalLength, out List<string> TypeStr)
        {
            TypeStr = new List<string> { };
            string OutputStr = string.Empty;
            UInt64 TempUint = 0;
            int Index = 0;
            int CheckStart = (int)SmbiosRawData[Offset + 1];
            string RawData = string.Empty;
            string Text = string.Empty;
            string TempStr = string.Empty;
            bool BreakNext = false;
            while (true)
            {
                byte TempByte = SmbiosRawData[Offset + Index];
                RawData += TempByte.ToString("X").PadLeft(2, '0');
                if ((TempByte < 0x20) || (TempByte > 0x7E))
                {
                    Text += ".";
                }
                else
                {
                    Text += (char)SmbiosRawData[Offset + Index];
                }
                if (Index % 0x10 == 0xF)
                {
                    OutputStr += RawData.PadRight(48);
                    OutputStr += Text;
                    OutputStr += Environment.NewLine;
                    RawData = string.Empty;
                    Text = string.Empty;
                }
                else
                {
                    RawData += " ";
                }

                if (Index >= CheckStart)
                {                    
                    if (SmbiosRawData[Offset + Index] == 0)
                    {
                        TypeStr.Add(TempStr);
                        TempStr = string.Empty;

                        if(BreakNext)
                        { 
                            OutputStr += RawData.PadRight(48);
                            OutputStr += Text;
                            OutputStr += Environment.NewLine;
                            break;
                        }
                        if (SmbiosRawData[Offset + Index + 1] == 0)
                        {
                            BreakNext = true;
                        }
                    }
                    else
                    {
                        TempStr += (char)SmbiosRawData[Offset + Index];
                    }
                }

                Index++;
            }
            TypeTotalLength = Index;

            OutputStr += "Type".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0, 1, out TempUint);
            OutputStr += Environment.NewLine;
            Console.WriteLine("Type " + TempUint.ToString());

            OutputStr += "Length".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 1, 1, out TempUint);
            OutputStr += Environment.NewLine;

            OutputStr += "Handle".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 2, 2, out TempUint);
            OutputStr += Environment.NewLine;

            Console.WriteLine("Total Length = 0x" + TypeTotalLength.ToString("X") + " (" + TypeTotalLength.ToString() + ")");
            return OutputStr;
        }

        private int Type0Decode(int Offset)
        {
            List<string> CharactStr = new List<string> {
                "Reserved - ",
                "Reserved - ",
                "Unknown - ",
                "BIOS Characteristics Not Supported - ",
                "ISA is supported - ",
                "MCA is supported - ",
                "EISA is supported - ",
                "PCI is supported - ",
                "PC Card (PCMCIA) is supported - ",
                "Plug and Play is supported - ",
                "APM is supported - ",
                "BIOS is Upgradeable (Flash) - ",
                "BIOS shadowing is allowed - ",
                "VL-VESA is supported - ",
                "ESCD support is available - ",
                "Boot from CD is supported - ",
                "Selectable Boot is supported - ",
                "BIOS ROM is socketed - ",
                "Boot From PC Card (PCMCIA) is supported - ",
                "EDD (Enhanced Disk Drive) Specification is supported - ",
                "Int 13h - Japanese Floppy for NEC 9800 1.2mb (3.5\", 1k Bytes/Sector, 360 RPM) is supported - ",
                "Int 13h - Japanese Floppy for Toshiba 1.2mb (3.5\", 360 RPM) is supported - ",
                "Int 13h - 5.25\" / 360 KB Floppy Services are supported - ",
                "Int 13h - 5.25\" / 1.2MB Floppy Services are supported - ",
                "Int 13h - 3.5\" / 720 KB Floppy Services are supported - ",
                "Int 13h - 3.5\" / 2.88 MB Floppy Services are supported - ",
                "Int 5h, Print Screen Service is supported - ",
                "Int 9h, 8042 Keyboard services are supported - ",
                "Int 14h, Serial Services are supported - ",
                "Int 17h, Printer Services are supported - ",
                "Int 10h, CGA/Mono Video Services are supported - ",
                "NEC PC-98 - "
            };
            List<string> CharactExt1Str = new List<string> {
                "ACPI supported - ",
                "USB Legacy is supported - ",
                "AGP is supported - ",
                "I2O boot is supported - ",
                "LS-120 boot is supported - ",
                "ATAPI ZIP Drive boot is supported - ",
                "1394 boot is supported - ",
                "Smart Battery supported - "
            };
            List<string> CharactExt2Str = new List<string> {
                "BIOS Boot Specification supported - ",
                "Function key-initiated Network Service boot supported - ",
                "Enable Targeted Content Distribution - ",
                "UEFI Specification is supported - ",
                "SMBIOS table describes a virtual machine - "
            };

            UInt64 TempUint = 0;
            string OutputStr = "[BIOS Information] (Type 0)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += ItemTypeString("BIOS Vendor", Offset, 4, TypeStr);
            
            OutputStr += ItemTypeString("BIOS Version", Offset, 5, TypeStr);

            OutputStr += ItemTypeByte("Start Address Segment", Offset, 6, 2);

            OutputStr += ItemTypeString("Release Date", Offset, 8, TypeStr);

            OutputStr += "Rom Size".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 9, 1, out TempUint, false);
            OutputStr += "(" + ((int)SmbiosRawData[Offset + 9] + 1) * 64 + "KB)";
            OutputStr += Environment.NewLine;

            OutputStr += "BIOS Characteristics".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0xA, 8, out TempUint, false);
            OutputStr += Environment.NewLine;
            for (int i = 0; i < 32; i++)
            {
                OutputStr += "  Bit" + i.ToString().PadRight(AlignmentValue - 5);
                OutputStr += CharactStr[i];
                OutputStr += ((TempUint & (BIT0 << i)) == (BIT0 << i)) ? "1 (Yes)" : "0 (No)";
                OutputStr += Environment.NewLine;
            }

            if (SmbiosRev > 23)
            {
                OutputStr += "Characteristics Ext1".PadRight(AlignmentValue);
                OutputStr += Bytes2String(Offset + 0x12, 1, out TempUint, false);
                OutputStr += Environment.NewLine;
                for (int i = 0; i < 8; i++)
                {
                    OutputStr += "  Bit" + i.ToString().PadRight(AlignmentValue - 5);
                    OutputStr += CharactExt1Str[i];
                    OutputStr += ((TempUint & (BIT0 << i)) == (BIT0 << i)) ? "1 (Yes)" : "0 (No)";
                    OutputStr += Environment.NewLine;
                }

                OutputStr += "Characteristics Ext2".PadRight(AlignmentValue);
                OutputStr += Bytes2String(Offset + 0x13, 1, out TempUint, false);
                OutputStr += Environment.NewLine;
                for (int i = 0; i < 5; i++)
                {
                    OutputStr += "  Bit" + i.ToString().PadRight(AlignmentValue - 5);
                    OutputStr += CharactExt2Str[i];
                    OutputStr += ((TempUint & (BIT0 << i)) == (BIT0 << i)) ? "1 (Yes)" : "0 (No)";
                    OutputStr += Environment.NewLine;
                }

                OutputStr += ItemTypeByte("System BIOS Major Release ", Offset, 0x14, 1);

                OutputStr += ItemTypeByte("System BIOS Minor Release ", Offset, 0x15, 1);
                
                OutputStr += ItemTypeByte("EC Firmware Major Release ", Offset, 0x16, 1);

                OutputStr += ItemTypeByte("EC Firmware Minor Release ", Offset, 0x17, 1);

                if (SmbiosRev > 30)
                {
                    OutputStr += "Extended BIOS ROM Size ".PadRight(AlignmentValue);
                    OutputStr += Bytes2String(Offset + 0x18, 2, out TempUint, false);
                    OutputStr += " - ";
                    if ((TempUint & 0xC000) < 0x2000)
                    {
                        OutputStr += (TempUint & 0x3FFF).ToString();
                        if ((TempUint & BIT14) == BIT14)
                        {
                            OutputStr += "GB";
                        }
                        else
                        {
                            OutputStr += "MB";
                        }
                    }
                    else
                    {
                        OutputStr += "Reserved";
                    }
                    OutputStr += Environment.NewLine;
                }
            }

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type1Decode(int Offset)
        {
            UInt64 TempUint = 0;
            string OutputStr = "[System Information] (Type 1)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);
            try
            {
                OutputStr += ItemTypeString("Manufacturer", Offset, 4, TypeStr);

                OutputStr += ItemTypeString("Product Name", Offset, 5, TypeStr);
                
                OutputStr += ItemTypeString("Version", Offset, 6, TypeStr);
                
                OutputStr += ItemTypeString("Serial Number", Offset, 7, TypeStr);

                OutputStr += "UUID".PadRight(AlignmentValue);
                for (int i = 0; i < 0x10; i++)
                {
                    OutputStr += "0x" + SmbiosRawData[Offset + 8 + i].ToString("X");
                    if (i < 0xF)
                    {
                        OutputStr += " ";
                    }
                }
                OutputStr += Environment.NewLine;

                OutputStr += "Wakeup Type".PadRight(AlignmentValue);
                OutputStr += Bytes2String(Offset + 0x18, 1, out TempUint, false);
                OutputStr += " - ";
                switch (TempUint)
                {
                    case 0:
                        OutputStr += "Reserved";
                        break;
                    case 1:
                        OutputStr += "Other";
                        break;
                    case 2:
                        OutputStr += "Unknown";
                        break;
                    case 3:
                        OutputStr += "APM Timer";
                        break;
                    case 4:
                        OutputStr += "Modem Ring";
                        break;
                    case 5:
                        OutputStr += "LAN Remote";
                        break;
                    case 6:
                        OutputStr += "Power Switch";
                        break;
                    case 7:
                        OutputStr += "PCI PME#";
                        break;
                    case 8:
                        OutputStr += "AC Power Restored";
                        break;
                    default:
                        OutputStr += "Undefined";
                        break;
                }
                OutputStr += Environment.NewLine;

                if (SmbiosRev > 23)
                {
                    OutputStr += ItemTypeString("SKU Number", Offset, 0x19, TypeStr);

                    OutputStr += ItemTypeString("Family", Offset, 0x1A, TypeStr);
                }
            }
            catch
            {
                OutputStr += Environment.NewLine;
                OutputStr += "Type 1 decode error";
                OutputStr += Environment.NewLine;
            }
            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type2Decode(int Offset)
        {
            List<string> FeatrueFlagStr = new List<string> {
                "Hosting board (motherboard) - ",
                "Requires at least one daughter board or auxiliary - ",
                "Removable - ",
                "Replaceable - ",
                "Hot swappable - "
            };
            List<string> BoardTypeStr = new List<string> {
                "",
                "Unknown",
                "Other",
                "Server Blade",
                "Connectivity Switch",
                "System Management Module",
                "Processor Module",
                "I/O Module",
                "Memory Module",
                "Daughter board",
                "Motherboard (includes processor, memory, and I/O)",
                "Processor/Memory Module",
                "Processor/IO Module",
                "Interconnect board"
            };

            UInt64 TempUint = 0;
            string OutputStr = "[Base Board Information] (Type 2)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);
            try
            {
                OutputStr += ItemTypeString("Manufacturer", Offset, 4, TypeStr);

                OutputStr += ItemTypeString("Product", Offset, 5, TypeStr);

                OutputStr += ItemTypeString("Version", Offset, 6, TypeStr);

                OutputStr += ItemTypeString("Serial Number", Offset, 7, TypeStr);

                OutputStr += ItemTypeString("Asset Tag", Offset, 8, TypeStr);

                OutputStr += "Featrue Flags".PadRight(AlignmentValue);
                OutputStr += Bytes2String(Offset + 9, 1, out TempUint, false);
                OutputStr += Environment.NewLine;
                for (int i = 0; i < 5; i++)
                {
                    OutputStr += "  Bit" + i.ToString().PadRight(AlignmentValue - 5);
                    OutputStr += FeatrueFlagStr[i];
                    OutputStr += ((TempUint & (BIT0 << i)) == (BIT0 << i)) ? "1 (Yes)" : "0 (No)";
                    OutputStr += Environment.NewLine;
                }

                OutputStr += ItemTypeString("Location in Chassis", Offset, 0xA, TypeStr);

                OutputStr += ItemTypeByte("Chassis Handle", Offset, 0xB, 2, false);

                OutputStr += "Board Type".PadRight(AlignmentValue);
                OutputStr += Bytes2String(Offset + 0xD, 1, out TempUint, false);
                OutputStr += " - ";
                if ((TempUint > 0) && (TempUint <= (UInt64)BoardTypeStr.Count))
                {
                    OutputStr += BoardTypeStr[(int)TempUint];
                }
                else
                {
                    OutputStr += "Undefined";
                }
                OutputStr += Environment.NewLine;
            }
            catch
            {
                OutputStr += Environment.NewLine;
                OutputStr += "Type 2 decode error";
                OutputStr += Environment.NewLine;
            }

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type3Decode(int Offset)
        {
            UInt64 TempUint = 0;
            List<string> TypeEnumStr = new List<string> {
                "",
                "Other",
                "Unknown",
                "Desktop",
                "Low Profile Desktop",
                "Pizza Box",
                "Mini Tower",
                "Tower",
                "Portable",
                "Laptop",
                "Notebook",
                "Hand Held",
                "Docking Station",
                "All in One",
                "Sub Notebook",
                "Space-saving",
                "Lunch Box",
                "Main Server Chassis",
                "Expansion Chassis",
                "SubChassis",
                "Bus Expansion Chassis",
                "Peripheral Chassis",
                "RAID Chassis",
                "Rack Mount Chassis",
                "Sealed-case PC",
                "Multi-system chassis",
                "Compact PCI",
                "Advanced TCA",
                "Blade",
                "Blade Enclosure",
                "Tablet",
                "Convertible",
                "Detachable",
                "IoT Gateway",
                "Embedded PC",
                "Mini PC",
                "Stick PC"
            };
            List<string> ChassisStateStr = new List<string> {
                "",
                "Other",
                "Unknown",
                "Safe",
                "Warning",
                "Critical",
                "Non-recoverable"
            };
            List<string> ChassisSecStateStr = new List<string> {
                "",
                "Other",
                "Unknown",
                "None",
                "External interface locked out",
                "External interface enabled"
            };
            string OutputStr = "[System Enclosure or Chassis] (Type 3)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += ItemTypeString("Manufacturer", Offset, 4, TypeStr);

            OutputStr += "Type".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 5, 1, out TempUint, false);
            OutputStr += " - ";
            UInt64 TempTypeUint = TempUint & (~BIT7);
            if ((TempTypeUint > 0) && (TempTypeUint <= (UInt64)TypeEnumStr.Count))
            {
                OutputStr += TypeEnumStr[(int)TempTypeUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;
            OutputStr += ("  Bit7 = " + (((TempUint & BIT7) == BIT7) ? "1" : "0")).PadRight(AlignmentValue);
            if ((TempUint & BIT7) == BIT7)
            {
                OutputStr += "Chassis lock is present";
            }
            else
            {
                OutputStr += "Chassis lock is not present or unknown if enclosure has a lock";
            }
            OutputStr += Environment.NewLine;

            OutputStr += ItemTypeString("Version", Offset, 6, TypeStr);
            
            OutputStr += ItemTypeString("Serial Number", Offset, 7, TypeStr);
            
            OutputStr += ItemTypeString("Asset Tag", Offset, 8, TypeStr);

            OutputStr += "Boot-up State".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 9, 1, out TempUint, false);
            OutputStr += " - ";
            if ((TempUint > 0) && (TempUint <= (UInt64)ChassisStateStr.Count))
            {
                OutputStr += ChassisStateStr[(int)TempUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Power Supply State".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0xA, 1, out TempUint, false);
            OutputStr += " - ";
            if ((TempUint > 0) && (TempUint <= (UInt64)ChassisStateStr.Count))
            {
                OutputStr += ChassisStateStr[(int)TempUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Thermal State".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0xB, 1, out TempUint, false);
            OutputStr += " - ";
            if ((TempUint > 0) && (TempUint <= (UInt64)ChassisStateStr.Count))
            {
                OutputStr += ChassisStateStr[(int)TempUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Security State".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0xC, 1, out TempUint, false);
            OutputStr += " - ";
            if ((TempUint > 0) && (TempUint <= (UInt64)ChassisSecStateStr.Count))
            {
                OutputStr += ChassisSecStateStr[(int)TempUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            OutputStr += ItemTypeByte("OEM-defined", Offset, 0xD, 4);

            OutputStr += ItemTypeByte("Height", Offset, 0x11, 1);
            
            OutputStr += ItemTypeByte("Number of Power Cords", Offset, 0x12, 1);
            
            OutputStr += ItemTypeByte("Contained Element Count", Offset, 0x13, 1);
            
            OutputStr += ItemTypeByte("Element Record Length", Offset, 0x14, 1);

            if (SmbiosRev > 22)
            {
                OutputStr += ItemTypeString("SKU Number", Offset, 0x15, TypeStr);
            }

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type4Decode(int Offset)
        {
            List<string> CpuTypeStr = new List<string> {
                "",
                "Other",
                "Unknown",
                "Central Processor",
                "Math Processor",
                "DSP Processor",
                "Video Processor"
            };
            List<string> CpuFamilyStr = new List<string> {
                "",
                "Other",
                "Unknown",
                "8086",
                "80286",
                "Intel386™ processor",
                "Intel486™ processor",
                "8087",
                "80287",
                "80387",
                "80487",
                "Intel® Pentium® processor",
                "Pentium® Pro processor",
                "Pentium® II processor",
                "Pentium® processor with MMX™ technology",
                "Intel® Celeron® processor",
                "Pentium® II Xeon™ processor",
                "Pentium® III processor",
                "M1 Family",
                "M2 Family",
                "Intel® Celeron® M processor",
                "Intel® Pentium® 4 HT processor",
                "",
                "",
                "AMD Duron™ Processor Family",
                "K5 Family",
                "K6 Family",
                "K6-2",
                "K6-3",
                "AMD Athlon™ Processor Family",
                "AMD29000 Family",
                "K6-2+",
                "Power PC Family",
                "Power PC 601",
                "Power PC 603",
                "Power PC 603+",
                "Power PC 604",
                "Power PC 620",
                "Power PC x704",
                "Power PC 750",
                "Intel® Core™ Duo processor",
                "Intel® Core™ Duo mobile processor",
                "Intel® Core™ Solo mobile processor",
                "Intel® Atom™ processor",
                "Intel® Core™ M processor",
                "Intel(R) Core(TM) m3 processor",
                "Intel(R) Core(TM) m5 processor",
                "Intel(R) Core(TM) m7 processor",
                "Alpha Family",
                "Alpha 21064",
                "Alpha 21066",
                "Alpha 21164",
                "Alpha 21164PC",
                "Alpha 21164a",
                "Alpha 21264",
                "Alpha 21364",
                "AMD Turion™ II Ultra Dual-Core Mobile M Processor Family",
                "AMD Turion™ II Dual-Core Mobile M Processor Family",
                "AMD Athlon™ II Dual-Core M Processor Family",
                "AMD Opteron™ 6100 Series Processor",
                "AMD Opteron™ 4100 Series Processor",
                "AMD Opteron™ 6200 Series Processor",
                "AMD Opteron™ 4200 Series Processor",
                "AMD FX™ Series Processor",
                "MIPS Family",
                "MIPS R4000",
                "MIPS R4200",
                "MIPS R4400",
                "MIPS R4600",
                "MIPS R10000",
                "AMD C-Series Processor",
                "AMD E-Series Processor",
                "AMD A-Series Processor",
                "AMD G-Series Processor",
                "AMD Z-Series Processor",
                "AMD R-Series Processor",
                "AMD Opteron™ 4300 Series Processor",
                "AMD Opteron™ 6300 Series Processor",
                "AMD Opteron™ 3300 Series Processor",
                "AMD FirePro™ Series Processor",
                "SPARC Family",
                "SuperSPARC",
                "microSPARC II",
                "microSPARC IIep",
                "UltraSPARC",
                "UltraSPARC II",
                "UltraSPARC Iii",
                "UltraSPARC III",
                "UltraSPARC IIIi",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "68040 Family",
                "68xxx",
                "68000",
                "68010",
                "68020",
                "68030",
                "AMD Athlon(TM) X4 Quad-Core Processor Family",
                "AMD Opteron(TM) X1000 Series Processor",
                "AMD Opteron(TM) X2000 Series APU",
                "AMD Opteron(TM) A-Series Processor",
                "AMD Opteron(TM) X3000 Series APU",
                "AMD Zen Processor Family",
                "",
                "",
                "",
                "",
                "Hobbit Family",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "Crusoe™ TM5000 Family",
                "Crusoe™ TM3000 Family",
                "Efficeon™ TM8000 Family",
                "",
                "",
                "",
                "",
                "",
                "Weitek",
                "Available for assignment",
                "Itanium™ processor",
                "AMD Athlon™ 64 Processor Family",
                "AMD Opteron™ Processor Family",
                "AMD Sempron™ Processor Family",
                "AMD Turion™ 64 Mobile Technology",
                "Dual-Core AMD Opteron™ Processor Family",
                "AMD Athlon™ 64 X2 Dual-Core Processor Family",
                "AMD Turion™ 64 X2 Mobile Technology",
                "Quad-Core AMD Opteron™ Processor Family",
                "Third-Generation AMD Opteron™ Processor Family",
                "AMD Phenom™ FX Quad-Core Processor Family",
                "AMD Phenom™ X4 Quad-Core Processor Family",
                "AMD Phenom™ X2 Dual-Core Processor Family",
                "AMD Athlon™ X2 Dual-Core Processor Family",
                "PA-RISC Family",
                "PA-RISC 8500",
                "PA-RISC 8000",
                "PA-RISC 7300LC",
                "PA-RISC 7200",
                "PA-RISC 7100LC",
                "PA-RISC 7100",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "V30 Family",
                "Quad-Core Intel® Xeon® processor 3200 Series",
                "Dual-Core Intel® Xeon® processor 3000 Series",
                "Quad-Core Intel® Xeon® processor 5300 Series",
                "Dual-Core Intel® Xeon® processor 5100 Series",
                "Dual-Core Intel® Xeon® processor 5000 Series",
                "Dual-Core Intel® Xeon® processor LV",
                "Dual-Core Intel® Xeon® processor ULV",
                "Dual-Core Intel® Xeon® processor 7100 Series",
                "Quad-Core Intel® Xeon® processor 5400 Series",
                "Quad-Core Intel® Xeon® processor",
                "Dual-Core Intel® Xeon® processor 5200 Series",
                "Dual-Core Intel® Xeon® processor 7200 Series",
                "Quad-Core Intel® Xeon® processor 7300 Series",
                "Quad-Core Intel® Xeon® processor 7400 Series",
                "Multi-Core Intel® Xeon® processor 7400 Series",
                "Pentium® III Xeon™ processor",
                "Pentium® III Processor with Intel® SpeedStep™ Technology",
                "Pentium® 4 Processor",
                "Intel® Xeon® processor",
                "AS400 Family",
                "Intel® Xeon™ processor MP",
                "AMD Athlon™ XP Processor Family",
                "AMD Athlon™ MP Processor Family",
                "Intel® Itanium® 2 processor",
                "Intel® Pentium® M processor",
                "Intel® Celeron® D processor",
                "Intel® Pentium® D processor",
                "Intel® Pentium® Processor Extreme Edition",
                "Intel® Core™ Solo Processor",
                "Reserved",
                "Intel® Core™ 2 Duo Processor",
                "Intel® Core™ 2 Solo processor",
                "Intel® Core™ 2 Extreme processor",
                "Intel® Core™ 2 Quad processor",
                "Intel® Core™ 2 Extreme mobile processor",
                "Intel® Core™ 2 Duo mobile processor",
                "Intel® Core™ 2 Solo mobile processor",
                "Intel® Core™ i7 processor",
                "Dual-Core Intel® Celeron® processor",
                "IBM390 Family",
                "G4",
                "G5",
                "ESA/390 G6",
                "z/Architecture base",
                "Intel® Core™ i5 processor",
                "Intel® Core™ i3 processor",
                "Intel® Core™ i9 processor",
                "",
                "",
                "VIA C7™-M Processor Family",
                "VIA C7™-D Processor Family",
                "VIA C7™ Processor Family",
                "VIA Eden™ Processor Family",
                "Multi-Core Intel® Xeon® processor",
                "Dual-Core Intel® Xeon® processor 3xxx Series",
                "Quad-Core Intel® Xeon® processor 3xxx Series",
                "VIA Nano™ Processor Family",
                "Dual-Core Intel® Xeon® processor 5xxx Series",
                "Quad-Core Intel® Xeon® processor 5xxx Series",
                "Available for assignment",
                "Dual-Core Intel® Xeon® processor 7xxx Series",
                "Quad-Core Intel® Xeon® processor 7xxx Series",
                "Multi-Core Intel® Xeon® processor 7xxx Series",
                "Multi-Core Intel® Xeon® processor 3400 Series",
                "",
                "",
                "",
                "AMD Opteron™ 3000 Series Processor",
                "AMD Sempron™ II Processor",
                "Embedded AMD Opteron™ Quad-Core Processor Family",
                "AMD Phenom™ Triple-Core Processor Family",
                "AMD Turion™ Ultra Dual-Core Mobile Processor Family",
                "AMD Turion™ Dual-Core Mobile Processor Family",
                "AMD Athlon™ Dual-Core Processor Family",
                "AMD Sempron™ SI Processor Family",
                "AMD Phenom™ II Processor Family",
                "AMD Athlon™ II Processor Family",
                "Six-Core AMD Opteron™ Processor Family",
                "AMD Sempron™ M Processor Family",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "i860",
                "i960",
                "",
                "",
                "Indicator to obtain the processor family from the Processor Family 2 field",
                "Reserved",
                "ARMv7",
                "ARMv8"
            };
            List<string> CpuSocketStr = new List<string> {
                "",
                "Other",
                "Unknown",
                "Daughter Board",
                "ZIF Socket",
                "Replaceable Piggy Back",
                "None",
                "LIF Socket",
                "Slot 1",
                "Slot 2",
                "370-pin socket",
                "Slot A",
                "Slot M",
                "Socket 423",
                "Socket A (Socket 462)",
                "Socket 478",
                "Socket 754",
                "Socket 940",
                "Socket 939",
                "Socket mPGA604",
                "Socket LGA771",
                "Socket LGA775",
                "Socket S1",
                "Socket AM2",
                "Socket F (1207)",
                "Socket LGA1366",
                "Socket G34",
                "Socket AM3",
                "Socket C32",
                "Socket LGA1156",
                "Socket LGA1567",
                "Socket PGA988A",
                "Socket BGA1288",
                "Socket rPGA988B",
                "Socket BGA1023",
                "Socket BGA1224",
                "Socket LGA1155",
                "Socket LGA1356",
                "Socket LGA2011",
                "Socket FS1",
                "Socket FS2",
                "Socket FM1",
                "Socket FM2",
                "Socket LGA2011-3",
                "Socket LGA1356-3",
                "Socket LGA1150",
                "Socket BGA1168",
                "Socket BGA1234",
                "Socket BGA1364",
                "Socket AM4",
                "Socket LGA1151",
                "Socket BGA1356",
                "Socket BGA1440",
                "Socket BGA1515",
                "Socket LGA3647-1",
                "Socket SP3",
                "Socket SP3r2",
                "Socket LGA2066",
                "Socket BGA1392",
                "Socket BGA1510",
                "Socket BGA1528"
            };
            List<string> CpuCharactStr = new List<string> {
                "Reserved - ",
                "Unknown - ",
                "64-bit Capable - ",
                "Multi-Core - ",
                "Hardware Thread - ",
                "Execute Protection - ",
                "Enhanced Virtualization - ",
                "Power/Performance Control - ",
                "Reserved - "
            };
            UInt64 TempUint = 0;
            string OutputStr = "[Processor Information] (Type 4)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += ItemTypeString("Socket Designation", Offset, 4, TypeStr);

            OutputStr += "Processor Type".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 5, 1, out TempUint, false);
            OutputStr += " - ";
            if ((TempUint > 0) && (TempUint <= (UInt64)CpuTypeStr.Count))
            {
                OutputStr += CpuTypeStr[(int)TempUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Processor Family".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 6, 1, out TempUint, false);
            OutputStr += " - ";
            if ((TempUint > 0) && (TempUint <= (UInt64)CpuFamilyStr.Count))
            {
                OutputStr += CpuFamilyStr[(int)TempUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            OutputStr += ItemTypeString("Processor Manufacturer", Offset, 7, TypeStr);

            OutputStr += ItemTypeByte("Processor ID", Offset, 8, 8);

            OutputStr += ItemTypeString("Processor Version", Offset, 0x10, TypeStr);

            OutputStr += "Processor Voltage".PadRight(AlignmentValue);
            Bytes2String(Offset + 0x11, 1, out TempUint, false);
            if ((TempUint & BIT7) == BIT7)
            {
                float CpuVolt = ((uint)TempUint - 0x80) / 10;
                OutputStr += CpuVolt.ToString("#0.0") + "V";
            }
            else
            {
                if ((TempUint & BIT0) == BIT0)
                {
                    OutputStr += "5V";
                }
                else if ((TempUint & BIT1) == BIT1)
                {
                    OutputStr += "3.3V";
                }
                else if ((TempUint & BIT2) == BIT2)
                {
                    OutputStr += "2.9V";
                }
            }
            OutputStr += Environment.NewLine;

            OutputStr += "External Clock".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0x12, 2, out TempUint);
            OutputStr += "MHz";
            OutputStr += Environment.NewLine;

            OutputStr += "Max Speed".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0x14, 2, out TempUint);
            OutputStr += "MHz";
            OutputStr += Environment.NewLine;

            OutputStr += "Current Speed".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0x16, 2, out TempUint);
            OutputStr += "MHz";
            OutputStr += Environment.NewLine;

            OutputStr += "Processor Status".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0x18, 1, out TempUint);
            OutputStr += Environment.NewLine;
            if ((TempUint & BIT6) == BIT6)
            {
                OutputStr += "  Bit6 = 1".PadRight(AlignmentValue) + "CPU Socket Populated";
            }
            else
            { 
                OutputStr += "  Bit6 = 0".PadRight(AlignmentValue) + "CPU Socket Unpopulated";
            }
            OutputStr += Environment.NewLine;
            OutputStr += "  Bits 2:0".PadRight(AlignmentValue);
            uint CpuStatus = (uint)(TempUint & (BIT0 | BIT1 | BIT2));
            OutputStr += CpuStatus.ToString() + "h - ";
            switch (CpuStatus)
            {
                case 0:
                    OutputStr += "Unknown";
                    break;
                case 1:
                    OutputStr += "CPU Enabled";
                    break;
                case 2:
                    OutputStr += "CPU Disabled by User through BIOS Setup";
                    break;
                case 3:
                    OutputStr += "CPU Disabled By BIOS (POST Error)";
                    break;
                case 4:
                    OutputStr += "CPU is Idle, waiting to be enabled.";
                    break;
                case 5:
                case 6:
                    OutputStr += "Reserved";
                    break;
                case 7:
                    OutputStr += "Other";
                    break;
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Processor Upgrade".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0x19, 1, out TempUint, false);
            OutputStr += " - ";
            if ((TempUint > 0) && (TempUint <= (UInt64)CpuSocketStr.Count))
            {
                OutputStr += CpuSocketStr[(int)TempUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            OutputStr += ItemTypeByte("L1 Cache Handle", Offset, 0x1A, 2);

            OutputStr += ItemTypeByte("L2 Cache Handle", Offset, 0x1C, 2);

            OutputStr += ItemTypeByte("L3 Cache Handle", Offset, 0x1E, 2);

            if (SmbiosRev > 22)
            {

                OutputStr += ItemTypeString("Serial Number", Offset, 0x20, TypeStr);

                OutputStr += ItemTypeString("Asset Tag", Offset, 0x21, TypeStr);

                OutputStr += ItemTypeString("Part Number", Offset, 0x22, TypeStr);

                if (SmbiosRev > 24)
                {
                    OutputStr += ItemTypeByte("Core Count", Offset, 0x23, 1);

                    OutputStr += ItemTypeByte("Core Enabled", Offset, 0x24, 1);

                    OutputStr += ItemTypeByte("Thread Count", Offset, 0x25, 1);

                    OutputStr += "Processor Characteristics".PadRight(AlignmentValue);
                    OutputStr += Bytes2String(Offset + 9, 1, out TempUint, false);
                    OutputStr += Environment.NewLine;
                    for (int i = 1; i < 8; i++)
                    {
                        OutputStr += "  Bit" + i.ToString().PadRight(AlignmentValue - 5);
                        OutputStr += CpuCharactStr[i];
                        OutputStr += ((TempUint & (BIT0 << i)) == (BIT0 << i)) ? "1 (Yes)" : "0 (No)";
                        OutputStr += Environment.NewLine;
                    }

                    if (SmbiosRev > 25)
                    {
                        OutputStr += "Processor Family 2".PadRight(AlignmentValue);
                        OutputStr += Bytes2String(Offset + 0x28, 2, out TempUint, false);
                        OutputStr += " - ";
                        if ((TempUint > 0) && (TempUint <= (UInt64)CpuFamilyStr.Count))
                        {
                            OutputStr += CpuFamilyStr[(int)TempUint];
                        }
                        else if (TempUint == 0x104)     // 0x104 = 260
                        {
                            OutputStr += "SH-3";
                        }
                        else if (TempUint == 0x105)     // 0x105 = 261
                        {
                            OutputStr += "SH-4";
                        }
                        else if (TempUint == 0x118)     // 0x118 = 280
                        {
                            OutputStr += "ARM";
                        }
                        else if (TempUint == 0x119)     // 0x119 = 281
                        {
                            OutputStr += "StrongARM";
                        }
                        else if (TempUint == 0x12C)     // 0x12C = 300
                        {
                            OutputStr += "6x86";
                        }
                        else if (TempUint == 0x12D)     // 0x12D = 301
                        {
                            OutputStr += "MediaGX";
                        }
                        else if (TempUint == 0x12E)     // 0x12E = 302
                        {
                            OutputStr += "MII";
                        }
                        else if (TempUint == 0x140)     // 0x140 = 320
                        {
                            OutputStr += "WinChip";
                        }
                        else if (TempUint == 0x15E)     // 0x15E = 350
                        {
                            OutputStr += "DSP";
                        }
                        else if (TempUint == 0x1F4)     // 0x1F4 = 500
                        {
                            OutputStr += "Video Processor";
                        }
                        else if (TempUint == 0xFFFE)     // 0xFFFE = 65534
                        {
                            OutputStr += "Reserved";
                        }
                        else if (TempUint == 0xFFFF)     // 0xFFFF = 65535
                        {
                            OutputStr += "Reserved";
                        }
                        else
                        {
                            OutputStr += "";
                        }
                        OutputStr += Environment.NewLine;

                        if (SmbiosRev > 29)
                        {
                            OutputStr += "Core Count 2".PadRight(AlignmentValue);
                            OutputStr += Bytes2String(Offset + 0x2A, 2, out TempUint, false);
                            OutputStr += " - ";
                            if (TempUint == 0)
                            {
                                OutputStr += "Unknown";
                            }
                            else if (TempUint == 0xFFFF)
                            {
                                OutputStr += "reserved";
                            }
                            else
                            {
                                OutputStr += TempUint.ToString() + "Core(s)";
                            }
                            OutputStr += Environment.NewLine;

                            OutputStr += "Core Enabled 2".PadRight(AlignmentValue);
                            OutputStr += Bytes2String(Offset + 0x2C, 2, out TempUint, false);
                            OutputStr += " - ";
                            if (TempUint == 0)
                            {
                                OutputStr += "Unknown";
                            }
                            else if (TempUint == 0xFFFF)
                            {
                                OutputStr += "reserved";
                            }
                            else
                            {
                                OutputStr += TempUint.ToString() + "Core(s)";
                            }
                            OutputStr += Environment.NewLine;

                            OutputStr += "Thread Count 2".PadRight(AlignmentValue);
                            OutputStr += Bytes2String(Offset + 0x2E, 2, out TempUint, false);
                            OutputStr += " - ";
                            if (TempUint == 0)
                            {
                                OutputStr += "Unknown";
                            }
                            else if (TempUint == 0xFFFF)
                            {
                                OutputStr += "reserved";
                            }
                            else
                            {
                                OutputStr += TempUint.ToString() + "Thread(s)";
                            }
                            OutputStr += Environment.NewLine;
                        }
                    }
                }
            }

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type7Decode(int Offset)
        {
            List<string> SupSramTypeStr = new List<string> {
                "Other - ",
                "Unknown - ",
                "Non-Burst - ",
                "Burst - ",
                "Pipeline Burst - ",
                "Synchronous - ",
                "Asynchronous - "
            };
            List<string> ErrorCorrectTypeStr = new List<string> {
                "",
                "Other",
                "Unknown",
                "None",
                "Parity",
                "Single-bit ECC",
                "Multi-bit ECC"
            };
            List<string> SystemCacheTypeStr = new List<string> {
                "",
                "Other",
                "Unknown",
                "Instruction",
                "Data",
                "Unified"
            };
            List<string> AssociativityStr = new List<string> {
                "",
                "Other",
                "Unknown",
                "Direct Mapped",
                "2-way Set-Associative",
                "4-way Set-Associative",
                "Fully Associative",
                "8-way Set-Associative",
                "16-way Set-Associative",
                "12-way Set-Associative",
                "24-way Set-Associative",
                "32-way Set-Associative",
                "48-way Set-Associative",
                "64-way Set-Associative",
                "20-way Set-Associative"
            };

            UInt64 TempUint = 0;
            string OutputStr = "[Cache Information] (Type 7)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += ItemTypeString("Socket Designation", Offset, 4, TypeStr);

            OutputStr += "Cache Configuration".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 5, 2, out TempUint, false);
            OutputStr += Environment.NewLine;
            UInt64 TempWord = (TempUint & (BIT9 | BIT8)) >> 8;
            OutputStr += ("  Bit9:8 = " + TempWord.ToString()).PadRight(AlignmentValue);
            OutputStr += "Operational Mode - ";
            if (TempWord == 0) OutputStr += "Write Through";
            else if (TempWord == 1) OutputStr += "Write Back";
            else if (TempWord == 2) OutputStr += "Varies with Memory Address";
            else if (TempWord == 3) OutputStr += "Unknown";
            OutputStr += Environment.NewLine;

            TempWord = (TempUint & BIT7) >> 7;
            OutputStr += ("  Bit7 = " + TempWord.ToString()).PadRight(AlignmentValue);
            OutputStr += "Enabled/Disabled (at boot time) - ";
            if (TempWord == 0) OutputStr += "Disabled";
            else if (TempWord == 1) OutputStr += "Enabled";
            OutputStr += Environment.NewLine;

            TempWord = (TempUint & (BIT6 | BIT5)) >> 5;
            OutputStr += ("  Bit6:5 = " + TempWord.ToString()).PadRight(AlignmentValue);
            OutputStr += "Location, relative to the CPU module - ";
            if (TempWord == 0) OutputStr += "Internal";
            else if (TempWord == 1) OutputStr += "External";
            else if (TempWord == 2) OutputStr += "Reserved";
            else if (TempWord == 3) OutputStr += "Unknown";
            OutputStr += Environment.NewLine;

            TempWord = (TempUint & BIT3) >> 3;
            OutputStr += ("  Bit3 = " + TempWord.ToString()).PadRight(AlignmentValue);
            OutputStr += "Cache Socketed - ";
            if (TempWord == 0) OutputStr += "Not Socketed";
            else if (TempWord == 1) OutputStr += "Socketed";
            OutputStr += Environment.NewLine;

            TempWord = TempUint & (BIT2 | BIT1 | BIT0);
            OutputStr += ("  Bit2:0 = " + TempWord.ToString()).PadRight(AlignmentValue);
            OutputStr += "Cache Level - 0x" + TempWord.ToString().PadLeft(2, '0');
            OutputStr += Environment.NewLine;

            OutputStr += "Maximum Cache Size".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 7, 2, out TempUint, false);
            OutputStr += " - ";
            if ((TempUint & BIT15) == BIT15)
            {
                OutputStr += ((TempUint & 0x7FFF) * 64).ToString() + "K";
            }
            else
            {
                OutputStr += (TempUint & 0x7FFF).ToString() + "K";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Installed Size ".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 9, 2, out TempUint, false);
            OutputStr += " - ";
            if (TempUint == 0)
            {
                OutputStr += "no cache is installed";
            }
            else if ((TempUint & BIT15) == BIT15)
            {
                OutputStr += ((TempUint & 0x7FFF) * 64).ToString() + "K";
            }
            else
            {
                OutputStr += (TempUint & 0x7FFF).ToString() + "K";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Supported SRAM Type".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0xB, 2, out TempUint, false);
            OutputStr += Environment.NewLine;
            for (int i = 0; i < 7; i++)
            {
                OutputStr += "  Bit" + i.ToString().PadRight(AlignmentValue - 5);
                OutputStr += SupSramTypeStr[i];
                OutputStr += ((TempUint & (BIT0 << i)) == (BIT0 << i)) ? "1 (Yes)" : "0 (No)";
                OutputStr += Environment.NewLine;
            }

            OutputStr += "Current SRAM Type".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0xD, 2, out TempUint, false);
            OutputStr += Environment.NewLine;
            for (int i = 0; i < 7; i++)
            {
                OutputStr += "  Bit" + i.ToString().PadRight(AlignmentValue - 5);
                OutputStr += SupSramTypeStr[i];
                OutputStr += ((TempUint & (BIT0 << i)) == (BIT0 << i)) ? "1 (Yes)" : "0 (No)";
                OutputStr += Environment.NewLine;
            }

            OutputStr += "Cache Speed".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0xF, 2, out TempUint, false);
            OutputStr += " - ";
            if (TempUint == 0)
            {
                OutputStr += "Unknown";
            }
            else
            {
                OutputStr += TempUint.ToString() + "ns";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Error Correction Type".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0x10, 1, out TempUint, false);
            OutputStr += " - ";
            if ((TempUint > 0) && (TempUint <= (UInt64)ErrorCorrectTypeStr.Count))
            {
                OutputStr += ErrorCorrectTypeStr[(int)TempUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "System Cache Type".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0x11, 1, out TempUint, false);
            OutputStr += " - ";
            if ((TempUint > 0) && (TempUint <= (UInt64)SystemCacheTypeStr.Count))
            {
                OutputStr += SystemCacheTypeStr[(int)TempUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Associativity ".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0x12, 1, out TempUint, false);
            OutputStr += " - ";
            if ((TempUint > 0) && (TempUint <= (UInt64)AssociativityStr.Count))
            {
                OutputStr += AssociativityStr[(int)TempUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type8Decode(int Offset)
        {
            #region DefinePortType
            Dictionary<UInt64, string> PortTypeStr = new Dictionary<UInt64, string>();
            PortTypeStr.Add(0x00, "None");
            PortTypeStr.Add(0x01, "Centronics");
            PortTypeStr.Add(0x02, "Mini Centronics");
            PortTypeStr.Add(0x03, "Proprietary");
            PortTypeStr.Add(0x04, "DB-25 pin male");
            PortTypeStr.Add(0x05, "DB-25 pin female");
            PortTypeStr.Add(0x06, "DB-15 pin male");
            PortTypeStr.Add(0x07, "DB-15 pin female");
            PortTypeStr.Add(0x08, "DB-9 pin male");
            PortTypeStr.Add(0x09, "DB-9 pin female");
            PortTypeStr.Add(0x0A, "RJ-11");
            PortTypeStr.Add(0x0B, "RJ-45");
            PortTypeStr.Add(0x0C, "50-pin MiniSCSI");
            PortTypeStr.Add(0x0D, "Mini-DIN");
            PortTypeStr.Add(0x0E, "Micro-DIN");
            PortTypeStr.Add(0x0F, "PS/2");
            PortTypeStr.Add(0x10, "Infrared");
            PortTypeStr.Add(0x11, "HP-HIL");
            PortTypeStr.Add(0x12, "Access Bus (USB)");
            PortTypeStr.Add(0x13, "SSA SCSI");
            PortTypeStr.Add(0x14, "Circular DIN-8 male");
            PortTypeStr.Add(0x15, "Circular DIN-8 female");
            PortTypeStr.Add(0x16, "On Board IDE");
            PortTypeStr.Add(0x17, "On Board Floppy");
            PortTypeStr.Add(0x18, "9-pin Dual Inline (pin 10 cut)");
            PortTypeStr.Add(0x19, "25-pin Dual Inline (pin 26 cut)");
            PortTypeStr.Add(0x1A, "50-pin Dual Inline");
            PortTypeStr.Add(0x1B, "68-pin Dual Inline");
            PortTypeStr.Add(0x1C, "On Board Sound Input from CD-ROM");
            PortTypeStr.Add(0x1D, "Mini-Centronics Type-14");
            PortTypeStr.Add(0x1E, "Mini-Centronics Type-26");
            PortTypeStr.Add(0x1F, "Mini-jack (headphones)");
            PortTypeStr.Add(0x20, "BNC");
            PortTypeStr.Add(0x21, "1394");
            PortTypeStr.Add(0x22, "SAS/SATA Plug Receptacle");
            PortTypeStr.Add(0x23, "USB Type-C Receptacle");
            PortTypeStr.Add(0xA0, "PC-98");
            PortTypeStr.Add(0xA1, "PC-98Hireso");
            PortTypeStr.Add(0xA2, "PC-H98");
            PortTypeStr.Add(0xA3, "PC-98Note");
            PortTypeStr.Add(0xA4, "PC-98Full");
            PortTypeStr.Add(0xFF, "Other");
            Dictionary<UInt64, string> PortTypeFieldStr = new Dictionary<UInt64, string>();
            PortTypeFieldStr.Add(0x00, "None");
            PortTypeFieldStr.Add(0x01, "Parallel Port XT/AT Compatible");
            PortTypeFieldStr.Add(0x02, "Parallel Port PS/2");
            PortTypeFieldStr.Add(0x03, "Parallel Port ECP");
            PortTypeFieldStr.Add(0x04, "Parallel Port EPP");
            PortTypeFieldStr.Add(0x05, "Parallel Port ECP/EPP");
            PortTypeFieldStr.Add(0x06, "Serial Port XT/AT Compatible");
            PortTypeFieldStr.Add(0x07, "Serial Port 16450 Compatible");
            PortTypeFieldStr.Add(0x08, "Serial Port 16550 Compatible");
            PortTypeFieldStr.Add(0x09, "Serial Port 16550A Compatible");
            PortTypeFieldStr.Add(0x0A, "SCSI Port");
            PortTypeFieldStr.Add(0x0B, "MIDI Port");
            PortTypeFieldStr.Add(0x0C, "Joy Stick Port");
            PortTypeFieldStr.Add(0x0D, "Keyboard Port");
            PortTypeFieldStr.Add(0x0E, "Mouse Port");
            PortTypeFieldStr.Add(0x0F, "SSA SCSI");
            PortTypeFieldStr.Add(0x10, "USB");
            PortTypeFieldStr.Add(0x11, "FireWire (IEEE P1394)");
            PortTypeFieldStr.Add(0x12, "PCMCIA Type I");
            PortTypeFieldStr.Add(0x13, "PCMCIA Type II");
            PortTypeFieldStr.Add(0x14, "PCMCIA Type III");
            PortTypeFieldStr.Add(0x15, "Cardbus");
            PortTypeFieldStr.Add(0x16, "Access Bus Port");
            PortTypeFieldStr.Add(0x17, "SCSI II");
            PortTypeFieldStr.Add(0x18, "SCSI Wide");
            PortTypeFieldStr.Add(0x19, "PC-98");
            PortTypeFieldStr.Add(0x1A, "PC-98-Hireso");
            PortTypeFieldStr.Add(0x1B, "PC-H98");
            PortTypeFieldStr.Add(0x1C, "Video Port");
            PortTypeFieldStr.Add(0x1D, "Audio Port");
            PortTypeFieldStr.Add(0x1E, "Modem Port");
            PortTypeFieldStr.Add(0x1F, "Network Port");
            PortTypeFieldStr.Add(0x20, "SATA");
            PortTypeFieldStr.Add(0x21, "SAS");
            PortTypeFieldStr.Add(0x22, "MFDP (Multi-Function Display Port)");
            PortTypeFieldStr.Add(0x23, "Thunderbolt");
            PortTypeFieldStr.Add(0xA0, "8251 Compatible");
            PortTypeFieldStr.Add(0xA1, "8251 FIFO Compatible");
            PortTypeFieldStr.Add(0xFF, "Other");
            #endregion

            UInt64 TempUint = 0;
            string OutputStr = "[Port Connector Information] (Type 8)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += ItemTypeString("Internal Reference Designator", Offset, 4, TypeStr);

            OutputStr += "Internal Connector Type".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 5, 1, out TempUint, false);
            OutputStr += " - ";
            if (PortTypeStr.ContainsKey(TempUint))
            {
                OutputStr += PortTypeStr[TempUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            OutputStr += ItemTypeString("External Reference Designator", Offset, 6, TypeStr);

            OutputStr += "External Connector Type".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 7, 1, out TempUint, false);
            OutputStr += " - " + PortTypeStr[TempUint];
            OutputStr += Environment.NewLine;

            OutputStr += "Port Type ".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 8, 1, out TempUint, false);
            OutputStr += " - ";
            if (PortTypeFieldStr.ContainsKey(TempUint))
            {
                OutputStr += PortTypeFieldStr[TempUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type11Decode(int Offset)
        {
            //UInt64 TempUint = 0;
            string OutputStr = "[OEM Strings] (Type 11)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += ItemTypeByte("Count", Offset, 4, 1);

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type12Decode(int Offset)
        {
            //UInt64 TempUint = 0;
            string OutputStr = "[System Strings] (Type 12)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += ItemTypeByte("Count", Offset, 4, 1);

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type14Decode(int Offset)
        {
            // UInt64 TempUint = 0;
            string OutputStr = "[Group Associations] (Type 14)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += ItemTypeString("Group Name", Offset, 4, TypeStr);

            OutputStr += ItemTypeByte("Item Type", Offset, 5, 1);

            OutputStr += ItemTypeByte("Item Handle", Offset, 6, 2);

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type16Decode(int Offset)
        {
            List<string> UseFieldStr = new List<string> {
                "",
                "Other",
                "Unknown",
                "System memory",
                "Video memory",
                "Flash memory",
                "Non-volatile RAM",
                "Cache memory"
            };
            List<string> ErrorCorrectTypeStr = new List<string> {
                "",
                "Other",
                "Unknown",
                "None",
                "Parity",
                "Single-bit ECC",
                "Multi-bit ECC",
                "CRC"
            };

            UInt64 TempUint = 0;
            string OutputStr = "[Physical Memory Array] (Type 16)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += "Location".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 4, 1, out TempUint, false);
            OutputStr += " - ";
            switch (TempUint)
            {
                case 0x01:
                    OutputStr += "Other";
                    break;
                case 0x02:
                    OutputStr += "Unknown";
                    break;
                case 0x03:
                    OutputStr += "System board or motherboard";
                    break;
                case 0x04:
                    OutputStr += "ISA add-on card";
                    break;
                case 0x05:
                    OutputStr += "EISA add-on card";
                    break;
                case 0x06:
                    OutputStr += "PCI add-on card";
                    break;
                case 0x07:
                    OutputStr += "MCA add-on card";
                    break;
                case 0x08:
                    OutputStr += "PCMCIA add-on card";
                    break;
                case 0x09:
                    OutputStr += "Proprietary add-on card";
                    break;
                case 0x0A:
                    OutputStr += "NuBus";
                    break;
                case 0xA0:
                    OutputStr += "PC-98/C20 add-on card";
                    break;
                case 0xA1:
                    OutputStr += "PC-98/C24 add-on card";
                    break;
                case 0xA2:
                    OutputStr += "PC-98/E add-on card";
                    break;
                case 0xA3:
                    OutputStr += "PC-98/Local bus add-on card";
                    break;
                default:
                    OutputStr += "Undefined";
                    break;
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Use".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 5, 1, out TempUint, false);
            OutputStr += " - ";
            if ((TempUint > 0) && (TempUint <= (UInt64)UseFieldStr.Count))
            {
                OutputStr += UseFieldStr[(int)TempUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Memory Error Correction".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 6, 1, out TempUint, false);
            OutputStr += " - ";
            if ((TempUint > 0) && (TempUint <= (UInt64)ErrorCorrectTypeStr.Count))
            {
                OutputStr += ErrorCorrectTypeStr[(int)TempUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Maximum Capacity ".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 7, 4, out TempUint, false);
            OutputStr += " - ";
            OutputStr += TempUint.ToString() + "KB, ";
            OutputStr += (TempUint >> 10).ToString() + "MB";
            if ((TempUint / 1048576) > 1)
            {
                OutputStr += ", ";
                OutputStr += ((float)TempUint/1048576).ToString("#0.00") + "GB";
            }
            OutputStr += Environment.NewLine;

            OutputStr += ItemTypeByte("Memory Error Handle", Offset, 0xB, 2, false);

            OutputStr += ItemTypeByte("Number of Memory Devices", Offset, 0xD, 2);

            OutputStr += ItemTypeByte("Extended Maximum Capacity", Offset, 0xF, 8);

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type17Decode(int Offset)
        {
            List<string> FormFactorStr = new List<string> {
                "",
                "Other",
                "Unknown",
                "SIMM",
                "SIP",
                "Chip",
                "DIP",
                "ZIP",
                "Proprietary Card",
                "DIMM",
                "TSOP",
                "Row of chips",
                "RIMM",
                "SODIMM",
                "SRIMM",
                "FB-DIMM"
            };
            List<string> MemTypeStr = new List<string> {
                "",
                "Other",
                "Unknown",
                "DRAM",
                "EDRAM",
                "VRAM",
                "SRAM",
                "RAM",
                "ROM",
                "FLASH",
                "EEPROM",
                "FEPROM",
                "EPROM",
                "CDRAM",
                "3DRAM",
                "SDRAM",
                "SGRAM",
                "RDRAM",
                "DDR",
                "DDR2",
                "DDR2 FB-DIMM",
                "Reserve",
                "Reserve",
                "Reserved",
                "DDR3",
                "FBD2",
                "DDR4",
                "LPDDR",
                "LPDDR2",
                "LPDDR3",
                "LPDDR4",
                "Logical non-volatile device",
            };
            List<string> SupSramTypeStr = new List<string> {
                "Reserved - ",
                "Other - ",
                "Unknown - ",
                "Fast-paged - ",
                "Static column - ",
                "Pseudo-static - ",
                "RAMBUS - ",
                "Synchronous - ",
                "CMOS - ",
                "EDO - ",
                "Window DRAM - ",
                "Cache DRAM - ",
                "Non-volatile - ",
                "Registered (Buffered) - ",
                "Unbuffered (Unregistered) - ",
                "LRDIMM - "
            };

            UInt64 TempUint = 0;
            string OutputStr = "[Memory Device] (Type 17)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += ItemTypeByte("Memory Array Handle", Offset, 4, 2, false);

            OutputStr += "Memory Error Handle".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 6, 2, out TempUint, false);
            if (TempUint == 0xFFFE)
            {
                OutputStr += " - No error information structure";
            }
            else if (TempUint == 0xFFFF)
            {
                OutputStr += "No error was detected";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Total Width".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 8, 2, out TempUint);
            OutputStr += "bits";
            OutputStr += Environment.NewLine;

            OutputStr += "Data Width".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0xA, 2, out TempUint);
            OutputStr += "bits";
            OutputStr += Environment.NewLine;

            OutputStr += "Size".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0xC, 2, out TempUint, false);
            OutputStr += " - ";
            if (TempUint == 0)
            {
                OutputStr += "No memory device is installed";
            }
            else if (TempUint == 0xFFFF)
            {
                OutputStr += "Unknow size";
            }
            else if (TempUint == 0x7FFF)
            {
                OutputStr += "Actual size is stored in the Extended Size";
            }
            else if ((TempUint & BIT15) == BIT15)
            {
                OutputStr += TempUint.ToString() + "KB, ";
                OutputStr += (TempUint >> 10).ToString() + "MB";
            }
            else
            {
                OutputStr += TempUint.ToString() + "MB, ";
                OutputStr += (TempUint >> 10).ToString() + "GB";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Form Factor".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0xE, 1, out TempUint, false);
            OutputStr += " - ";
            if ((TempUint > 0) && (TempUint <= (UInt64)FormFactorStr.Count))
            {
                OutputStr += FormFactorStr[(int)TempUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Device Set".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0xF, 1, out TempUint, false);
            if (TempUint == 0)
            {
                OutputStr += " - Not part of a set";
            }
            else if (TempUint == 0xFF)
            {
                OutputStr += " - Unknow attribute";
            }
            OutputStr += Environment.NewLine;

            OutputStr += ItemTypeString("Device Locator", Offset, 0x10, TypeStr);

            OutputStr += ItemTypeString("Bank Locator", Offset, 0x11, TypeStr);

            OutputStr += "Memory Type".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0xE, 1, out TempUint, false);
            OutputStr += " - ";
            if ((TempUint > 0) && (TempUint <= (UInt64)MemTypeStr.Count))
            {
                OutputStr += MemTypeStr[(int)TempUint];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Type Detail".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0x13, 2, out TempUint, false);
            OutputStr += Environment.NewLine;
            for (int i = 0; i < 7; i++)
            {
                OutputStr += "  Bit" + i.ToString().PadRight(AlignmentValue - 5);
                OutputStr += SupSramTypeStr[i];
                OutputStr += ((TempUint & (BIT0 << i)) == (BIT0 << i)) ? "1 (Yes)" : "0 (No)";
                OutputStr += Environment.NewLine;
            }

            OutputStr += "Speed".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0x15, 2, out TempUint, false);
            if (TempUint == 0)
            {
                OutputStr += " - Unknown";
            }
            else if (TempUint == 0xFFFF)
            {
                OutputStr += " - Reserved";
            }
            else
            {
                OutputStr += " (" + TempUint.ToString() + ") MT/s";
            }
            OutputStr += Environment.NewLine;

            OutputStr += ItemTypeString("Manufacturer", Offset, 0x17, TypeStr);

            OutputStr += ItemTypeString("Serial Number", Offset, 0x18, TypeStr);

            OutputStr += ItemTypeString("Asset Tag", Offset, 0x19, TypeStr);

            OutputStr += ItemTypeString("Part Number", Offset, 0x1A, TypeStr);

            OutputStr += "Attributes (rank)".PadRight(AlignmentValue);
            Bytes2String(Offset + 0x1B, 1, out TempUint, false);
            TempUint = TempUint & (BIT3 | BIT2 | BIT1 | BIT0);
            OutputStr += "0x" + TempUint.ToString("X").PadLeft(2, '0');
            if (TempUint == 0)
            {
                OutputStr += " - Unknown rank";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Extended Size".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0x1C, 4, out TempUint, false);
            if (TempUint == 0)
            {
                OutputStr += " - Refer to Size";
            }
            else
            {
                OutputStr += " - ";
                OutputStr += TempUint.ToString() + "MB, ";
                OutputStr += ((float)TempUint/1024).ToString() + "GB";
            }
            OutputStr += Environment.NewLine;

            OutputStr += "Configured Memory Speed".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0x20, 2, out TempUint, false);
            OutputStr += " - " + TempUint.ToString() + "MHz";
            OutputStr += Environment.NewLine;

            if (SmbiosRev > 27)
            {
                OutputStr += "Minimum voltage ".PadRight(AlignmentValue);
                OutputStr += Bytes2String(Offset + 0x22, 2, out TempUint);
                if (TempUint == 0)
                {
                    OutputStr += " - Unknown";
                }
                else
                {
                    OutputStr += "mV";
                }
                OutputStr += Environment.NewLine;

                OutputStr += "Maximum voltage ".PadRight(AlignmentValue);
                OutputStr += Bytes2String(Offset + 0x24, 2, out TempUint);
                if (TempUint == 0)
                {
                    OutputStr += " - Unknown";
                }
                else
                {
                    OutputStr += "mV";
                }
                OutputStr += Environment.NewLine;

                OutputStr += "Configured voltage ".PadRight(AlignmentValue);
                OutputStr += Bytes2String(Offset + 0x26, 2, out TempUint);
                if (TempUint == 0)
                {
                    OutputStr += " - Unknown";
                }
                else
                {
                    OutputStr += "mV";
                }
                OutputStr += Environment.NewLine;
            }

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type19Decode(int Offset)
        {
            UInt64 TempUint = 0;
            string OutputStr = "[Memory Array Mapped Address] (Type 19)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += ItemTypeByte("Starting Address", Offset, 4, 4, false);

            OutputStr += "Ending Address".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 8, 4, out TempUint, false);
            OutputStr += " - " + TempUint.ToString() + "KB, ";
            UInt64 TempSize = (TempUint + 1) >> 10;
            OutputStr += TempSize.ToString() + "MB, ";
            OutputStr += ((float)TempSize/1024).ToString("#0.00") + "GB";
            OutputStr += Environment.NewLine;

            OutputStr += ItemTypeByte("Memory Array Handle", Offset, 0xC, 2);

            OutputStr += ItemTypeByte("Partition Width", Offset, 0xE);

            OutputStr += ItemTypeByte("Extended Starting Address", Offset, 0xF, 8);

            OutputStr += ItemTypeByte("Extended Ending Address", Offset, 0x17, 8);

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type20Decode(int Offset)
        {
            UInt64 TempUint = 0;
            UInt64 TempSize = 0;
            string OutputStr = "[Memory Device Mapped Address] (Type 20)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += "Starting Address".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 4, 4, out TempUint, false);
            if (TempUint != 0)
            {
                OutputStr += " - " + TempUint.ToString() + "KB, ";
                TempSize = (TempUint + 1) >> 10;
                OutputStr += TempSize.ToString() + "MB, ";
                OutputStr += ((float)TempSize / 1024).ToString("#0.00") + "GB";
                OutputStr += Environment.NewLine;
            }

            OutputStr += "Ending Address".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 8, 4, out TempUint, false);
            OutputStr += " - " + TempUint.ToString() + "KB, ";
            TempSize = (TempUint + 1) >> 10;
            OutputStr += TempSize.ToString() + "MB, ";
            OutputStr += ((float)TempSize / 1024).ToString("#0.00") + "GB";
            OutputStr += Environment.NewLine;

            OutputStr += ItemTypeByte("Memory Device Handle", Offset, 0xC, 2);

            OutputStr += ItemTypeByte("Array Mapped Addrress Handle", Offset, 0xE, 2);
            
            OutputStr += ItemTypeByte("Partition Row Position", Offset, 0x10, 1);
            
            OutputStr += ItemTypeByte("InterLeave Position", Offset, 0x11, 1);
            
            OutputStr += ItemTypeByte("Interleaved Data Depth", Offset, 0x12, 1);

            if (SmbiosRev > 26)
            {
                OutputStr += ItemTypeByte("Extended Starting Address", Offset, 0x13, 8);

                OutputStr += ItemTypeByte("Extended Ending Address", Offset, 0x1B, 8);
            }

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type24Decode(int Offset)
        {
            List<string> StatusValue = new List<string> 
            {
                "Disabled",
                "Enabled",
                "Not Implemented",
                "Unknown"
            };
            List<string> StatusName = new List<string>
            {
                "Power-on Password Status value      ",
                "Keyboard Password Status value      ",
                "Administrator Password Status value ",
                "Front Panel Reset Status value      "
            };

            UInt64 TempUint = 0;
            string OutputStr = "[Hardware Security] (Type 24)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += "Hardware Security Settings".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 4, 1, out TempUint);
            OutputStr += Environment.NewLine;
            for (int i = 0; i < 4; i++)
            {
                int Index = 6 - 2 * i;
                OutputStr += string.Format("  Bits {0}:{1}", Index + 1, Index).PadRight(AlignmentValue);
                OutputStr += StatusName[i];
                int BitsValue = (int)((TempUint & (uint)(3 << Index)) >> Index);
                OutputStr += " - " + BitsValue.ToString() + " ";
                OutputStr += StatusValue[BitsValue];
                OutputStr += Environment.NewLine;
            }

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type25Decode(int Offset)
        {
            //UInt64 TempUint = 0;
            string OutputStr = "[System Power Controls] (Type 25)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += ItemTypeByte("Next Scheduled Power-on Month", Offset, 4, 1, false);
            OutputStr += ItemTypeByte("Next Scheduled Power-on Day", Offset, 5, 1, false);
            OutputStr += ItemTypeByte("Next Scheduled Power-on Hour", Offset, 6, 1, false);
            OutputStr += ItemTypeByte("Next Scheduled Power-on Minute", Offset, 7, 1, false);
            OutputStr += ItemTypeByte("Next Scheduled Power-on Second", Offset, 8, 1, false);

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type27Decode(int Offset)
        {
            //UInt64 TempUint = 0;
            string OutputStr = "[Cooling Device] (Type 27)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += ItemTypeByte("Temperature Probe Handle", Offset, 4, 2, false);
            OutputStr += ItemTypeByte("Device Type and Status", Offset, 6, 1, false);
            OutputStr += ItemTypeByte("Cooling Unit Group", Offset, 7, 1, false);
            OutputStr += ItemTypeByte("OEM-Defined", Offset, 8, 4, false);
            OutputStr += ItemTypeByte("Nominal Speed", Offset, 0xC, 2, false);
            
            if (SmbiosRev > 26)
            {
                OutputStr += ItemTypeString("Description", Offset, 0xE, TypeStr);
            }

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type28Decode(int Offset)
        {
            List<string> ProbeStatus = new List<string> {
                "",
                "Other",
                "Unknown",
                "OK",
                "Non-critical",
                "Critical",
                "Non-recoverable"
            };
            List<string> ProbeLocation = new List<string> {
                "",
                "Other",
                "Unknown",
                "Processor",
                "Disk",
                "Peripheral Bay",
                "System Management Module",
                "Motherboard",
                "Memory Module",
                "Processor Module",
                "Power Unit",
                "Add-in Card",
                "Front Panel Board",
                "Back Panel Board",
                "Power System Board",
                "Drive Back Plane"
            };

            UInt64 TempUint = 0;
            string OutputStr = "[Temperature Probe] (Type 28)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += ItemTypeString("Description", Offset, 4, TypeStr);

            OutputStr += "Location and Status".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 5, 1, out TempUint, false);
            OutputStr += Environment.NewLine;
            OutputStr += "  Bit7:5 Status - ".PadRight(AlignmentValue);
            UInt64 TempCode = (TempUint & 0xE0) >> 5;
            if ((TempCode > 0) && (TempCode <= (UInt64)ProbeStatus.Count))
            {
                OutputStr += ProbeStatus[(int)TempCode];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;
            OutputStr += "  Bit4:0 Location - ".PadRight(AlignmentValue);
            TempCode = TempUint & 0x1F;
            if ((TempCode > 0) && (TempCode <= (UInt64)ProbeLocation.Count))
            {
                OutputStr += ProbeLocation[(int)TempCode];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;



            OutputStr += ItemTypeByte("Maximum Value", Offset, 6, 2, false);
            OutputStr += ItemTypeByte("Minimum Value", Offset, 8, 2, false);
            OutputStr += ItemTypeByte("Resolution", Offset, 0xA, 2, false);
            OutputStr += ItemTypeByte("Tolerance", Offset, 0xC, 2, false);
            OutputStr += ItemTypeByte("Accuracy", Offset, 0xE, 2, false);
            OutputStr += ItemTypeByte("OEM-Defined", Offset, 0x10, 4, false);
            OutputStr += ItemTypeByte("Nominal Value", Offset, 0x14, 2, false);

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type41Decode(int Offset)
        {
            List<string> DevType = new List<string>
            {
                "",
                "Other",
                "Unknown",
                "Video",
                "SCSI Controller",
                "Ethernet",
                "Token Ring",
                "Sound",
                "PATA Controller",
                "SATA Controller",
                "SAS Controller"
            };
            UInt64 TempUint = 0;
            string OutputStr = "[Onboard Devices Extended Information] (Type 41)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += ItemTypeString("Reference Designation", Offset, 4, TypeStr);

            OutputStr += "Device Type".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 5, 1, out TempUint);
            OutputStr += " - ";
            if ((TempUint & BIT7) == BIT7)
            {
                OutputStr += "Device Enabled";
            }
            else
            {
                OutputStr += "Device Disabled";
            }
            OutputStr += ", ";
            int TempIndex = (int)(TempUint & 0x7F);
            if ((TempUint > 0) && (TempIndex <= DevType.Count))
            {
                OutputStr += DevType[TempIndex];
            }
            else
            {
                OutputStr += "Undefined";
            }
            OutputStr += Environment.NewLine;

            OutputStr += ItemTypeByte("Device Type Instance", Offset, 6, 1, false);

            OutputStr += ItemTypeByte("Segment Group Number", Offset, 7, 2, false);

            OutputStr += ItemTypeByte("Bus Number", Offset, 9, 1, false);

            OutputStr += "Device/Function Number".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0xA, 1, out TempUint, false);
            OutputStr += " - Device ";
            OutputStr += ((TempUint & 0xF8) >> 3).ToString();
            OutputStr += ", Function ";
            OutputStr += (TempUint & 0x7).ToString();
            OutputStr += Environment.NewLine;

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type43Decode(int Offset)
        {
            List<string> CpuCharactStr = new List<string> {
                "Reserved - ",
                "Reserved - ",
                "TPM Device Characteristics are not supported - ",
                "Family configurable via firmware update - ",
                "Family configurable via platform software support - ",
                "Family configurable via OEM proprietary mechanism - "
            };

            UInt64 TempUint = 0;
            string OutputStr = "[TPM Device] (Type 43)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            OutputStr += "Vender ID".PadRight(AlignmentValue);
            for (int i = 0; i < 4; i++)
            {
                byte bTemp = SmbiosRawData[Offset + 4 + i];
                if ((bTemp < 0x20)||(bTemp > 0x7E))
                {
                    continue;
                }
                OutputStr += (char)bTemp;
            }
            OutputStr += Environment.NewLine;

            OutputStr += ItemTypeByte("Major Spec version", Offset, 8);
            OutputStr += ItemTypeByte("Minor Spec version", Offset, 9);
            OutputStr += ItemTypeByte("Firmware version 1", Offset, 0xA, 4);
            OutputStr += ItemTypeByte("Firmware version 2", Offset, 0xE, 4);
            OutputStr += ItemTypeString("Description", Offset, 0x12, TypeStr);

            OutputStr += "Characteristics".PadRight(AlignmentValue);
            OutputStr += Bytes2String(Offset + 0x13, 8, out TempUint, false);
            OutputStr += Environment.NewLine;
            for (int i = 0; i < 6; i++)
            {
                OutputStr += "  Bit" + i.ToString().PadRight(AlignmentValue - 5);
                OutputStr += CpuCharactStr[i];
                OutputStr += ((TempUint & (BIT0 << i)) == (BIT0 << i)) ? "1 (Yes)" : "0 (No)";
                OutputStr += Environment.NewLine;
            }

            OutputStr += ItemTypeByte("OEM-defined", Offset, 0x1B, 4);

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private int Type127Decode(int Offset)
        {
            //UInt64 TempUint = 0;
            string OutputStr = "[End of Table] (Type 127)";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private string GetTypeString(int gOffset, int tOffset, List<string> TypeStr)
        {
            string OutputStr = string.Empty;
            UInt64 TempUint = 0;
            OutputStr += "String" + Bytes2String(gOffset + tOffset, 1, out TempUint, false, false, false);
            if (TempUint < 1)
            {
                OutputStr = "NULL";
            }
            else
            {
                OutputStr += " - \"" + TypeStr[(int)SmbiosRawData[gOffset + tOffset] - 1] + "\"";
            }

            return OutputStr;
        }

        private int TypeUnknownDecode(int Offset)
        {
            string OutputStr = "[Unknown]";
            SmbiosType.Add(OutputStr);
            OutputStr += Environment.NewLine;

            OutputStr += TypeStructDecode(Offset, out int TypeTotalLength, out List<string> TypeStr);

            SmbiosTypeData.Add(OutputStr);
            return TypeTotalLength;
        }

        private string ItemTypeString(string ItemName, int gOffset, int tOffset, List<string> TypeStr)
        {
            string OutputStr = ItemName.PadRight(AlignmentValue);
            OutputStr += GetTypeString(gOffset, tOffset, TypeStr);
            OutputStr += Environment.NewLine;

            return OutputStr;
        }

        private string ItemTypeByte(string ItemName, int gOffset, int tOffset, int DataLength = 1, bool NeedDec = true)
        {
            string OutputStr = ItemName.PadRight(AlignmentValue);
            OutputStr += Bytes2String(gOffset + tOffset, DataLength, out UInt64 TempUint, NeedDec);
            OutputStr += Environment.NewLine;

            return OutputStr;
        }

        #endregion

        private string Bytes2String(int Offset, int DataLength, out UInt64 TempUint, bool NeedDec = true, bool HexSign = true, bool bPadLeft = true)
        {
            TempUint = 0;
            string Result = string.Empty;
            for (int i = 0; i < DataLength; i++)
            {
                TempUint = TempUint << 8;
                TempUint += SmbiosRawData[Offset + DataLength - 1 - i];
            }
            if (HexSign)
            {
                Result += "0x";
            }
            if (bPadLeft)
            {
                Result += TempUint.ToString("X").PadLeft(DataLength * 2, '0');
            }
            else
            {
                Result += TempUint.ToString("X");
            }
            if (NeedDec)
            {
                Result += " (" + TempUint.ToString() + ")";
            }
            return Result;
        }

        private void DisplayDeacodedDate()
        {
            if ((SmbiosType.Count > 0) && (SmbiosTypeData.Count > 0))
            {
                Lb_Types.Items.Clear();
                for (int i = 0; i < SmbiosType.Count; i++)
                {
                    Lb_Types.Items.Add(SmbiosType[i]);
                }
                Lb_Types.SelectedIndex = 0;
            }

        }

        private void Lb_Types_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Lb_Types.SelectedIndex == 0)
            {
                Rtb_Data.Text = string.Empty;
                foreach (string TempStr in SmbiosTypeData)
                {
                    Rtb_Data.Text += TempStr;
                    Rtb_Data.Text += Environment.NewLine;
                }
            }
            else
            {
                Rtb_Data.Text = SmbiosTypeData[Lb_Types.SelectedIndex];
            }
        }
    }
}
