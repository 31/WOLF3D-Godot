﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOLF3D.Graphics
{
    /// <summary>
    /// Copied from GFXV_WL1.H
    /// </summary>
    class Shareware
    {
        enum GraphicNums
        {
            H_BJPIC = 3,
            H_CASTLEPIC = 4,
            H_KEYBOARDPIC = 5,
            H_JOYPIC = 6,
            H_HEALPIC = 7,
            H_TREASUREPIC = 8,
            H_GUNPIC = 9,
            H_KEYPIC = 10,
            H_BLAZEPIC = 11,
            H_WEAPON1234PIC = 12,
            H_WOLFLOGOPIC = 13,
            H_VISAPIC = 14,
            H_MCPIC = 15,
            H_IDLOGOPIC = 16,
            H_TOPWINDOWPIC = 17,
            H_LEFTWINDOWPIC = 18,
            H_RIGHTWINDOWPIC = 19,
            H_BOTTOMINFOPIC = 20,
            C_OPTIONSPIC = 21,
            C_CURSOR1PIC = 22,
            C_CURSOR2PIC = 23,
            C_NOTSELECTEDPIC = 24,
            C_SELECTEDPIC = 25,
            C_FXTITLEPIC = 26,
            C_DIGITITLEPIC = 27,
            C_MUSICTITLEPIC = 28,
            C_MOUSELBACKPIC = 29,
            C_BABYMODEPIC = 30,
            C_EASYPIC = 31,
            C_NORMALPIC = 32,
            C_HARDPIC = 33,
            C_LOADSAVEDISKPIC = 34,
            C_DISKLOADING1PIC = 35,
            C_DISKLOADING2PIC = 36,
            C_CONTROLPIC = 37,
            C_CUSTOMIZEPIC = 38,
            C_LOADGAMEPIC = 39,
            C_SAVEGAMEPIC = 40,
            C_EPISODE1PIC = 41,
            C_EPISODE2PIC = 42,
            C_EPISODE3PIC = 43,
            C_EPISODE4PIC = 44,
            C_EPISODE5PIC = 45,
            C_EPISODE6PIC = 46,
            C_CODEPIC = 47,
            L_GUYPIC = 48,
            L_COLONPIC = 49,
            L_NUM0PIC = 50,
            L_NUM1PIC = 51,
            L_NUM2PIC = 52,
            L_NUM3PIC = 53,
            L_NUM4PIC = 54,
            L_NUM5PIC = 55,
            L_NUM6PIC = 56,
            L_NUM7PIC = 57,
            L_NUM8PIC = 58,
            L_NUM9PIC = 59,
            L_PERCENTPIC = 60,
            L_APIC = 61,
            L_BPIC = 62,
            L_CPIC = 63,
            L_DPIC = 64,
            L_EPIC = 65,
            L_FPIC = 66,
            L_GPIC = 67,
            L_HPIC = 68,
            L_IPIC = 69,
            L_JPIC = 70,
            L_KPIC = 71,
            L_LPIC = 72,
            L_MPIC = 73,
            L_NPIC = 74,
            L_OPIC = 75,
            L_PPIC = 76,
            L_QPIC = 77,
            L_RPIC = 78,
            L_SPIC = 79,
            L_TPIC = 80,
            L_UPIC = 81,
            L_VPIC = 82,
            L_WPIC = 83,
            L_XPIC = 84,
            L_YPIC = 85,
            L_ZPIC = 86,
            L_EXPOINTPIC = 87,
            L_GUY2PIC = 88,
            L_BJWINSPIC = 89,
            STATUSBARPIC = 90,
            TITLEPIC = 91,
            PG13PIC = 92,
            CREDITSPIC = 93,
            HIGHSCORESPIC = 94,
            KNIFEPIC = 95,
            GUNPIC = 96,
            MACHINEGUNPIC = 97,
            GATLINGGUNPIC = 98,
            NOKEYPIC = 99,
            GOLDKEYPIC = 100,
            SILVERKEYPIC = 101,
            N_BLANKPIC = 102,
            N_0PIC = 103,
            N_1PIC = 104,
            N_2PIC = 105,
            N_3PIC = 106,
            N_4PIC = 107,
            N_5PIC = 108,
            N_6PIC = 109,
            N_7PIC = 110,
            N_8PIC = 111,
            N_9PIC = 112,
            FACE1APIC = 113,
            FACE1BPIC = 114,
            FACE1CPIC = 115,
            FACE2APIC = 116,
            FACE2BPIC = 117,
            FACE2CPIC = 118,
            FACE3APIC = 119,
            FACE3BPIC = 120,
            FACE3CPIC = 121,
            FACE4APIC = 122,
            FACE4BPIC = 123,
            FACE4CPIC = 124,
            FACE5APIC = 125,
            FACE5BPIC = 126,
            FACE5CPIC = 127,
            FACE6APIC = 128,
            FACE6BPIC = 129,
            FACE6CPIC = 130,
            FACE7APIC = 131,
            FACE7BPIC = 132,
            FACE7CPIC = 133,
            FACE8APIC = 134,
            GOTGATLINGPIC = 135,
            MUTANTBJPIC = 136,
            PAUSEDPIC = 137,
            GETPSYCHEDPIC = 138,
            ORDERSCREEN = 554,
            ERRORSCREEN = 555,
            ENUMEND
        };

        public static readonly int README_LUMP_START = 3;
        public static readonly int README_LUMP_END = 20;
        public static readonly int CONTROLS_LUMP_START = 21;
        public static readonly int CONTROLS_LUMP_END = 47;
        public static readonly int LEVELEND_LUMP_START = 48;
        public static readonly int LEVELEND_LUMP_END = 89;
        public static readonly int LATCHPICS_LUMP_START = 95;
        public static readonly int LATCHPICS_LUMP_END = 138;

        //
        // Amount of each data item
        //
        public static readonly int NUMCHUNKS = 556;
        public static readonly int NUMFONT = 2;
        public static readonly int NUMFONTM = 0;
        public static readonly int NUMPICS = 136;
        public static readonly int NUMPICM = 0;
        public static readonly int NUMSPRITES = 0;
        public static readonly int NUMTILE8 = 72;
        public static readonly int NUMTILE8M = 0;
        public static readonly int NUMTILE16 = 144;
        public static readonly int NUMTILE16M = 270;
        public static readonly int NUMTILE32 = 0;
        public static readonly int NUMTILE32M = 0;
        public static readonly int NUMEXTERNS = 2;

        //
        // File offsets for data items
        //
        public static readonly int STRUCTPIC = 0;
        public static readonly int STARTFONT = 1;
        public static readonly int STARTFONTM = 3;
        public static readonly int STARTPICS = 3;
        public static readonly int STARTPICM = 139;
        public static readonly int STARTSPRITES = 139;
        public static readonly int STARTTILE8 = 139;
        public static readonly int STARTTILE8M = 140;
        public static readonly int STARTTILE16 = 140;
        public static readonly int STARTTILE16M = 284;
        public static readonly int STARTTILE32 = 554;
        public static readonly int STARTTILE32M = 554;
        public static readonly int STARTEXTERNS = 554;
    }
}
