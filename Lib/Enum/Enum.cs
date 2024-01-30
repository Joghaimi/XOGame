using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public enum SoundType
    {
        Beeb=0,
        Warning,
        Done,
        Bonus,
        Finish,
        Start,
        Start2,
        LaserScan,
        MarioJump,
        Complete,
        Descend,
        Descend2,
        GameOver,
        WinSquare,
        Winner1,
        Winner2
    }
    public enum Displays
    {
        first = 1,
        second,
        third,
        fourth
    }
    public enum DisplayCommand
    {
        clear = 0,
        rightArrow
    }
    public enum MCP23017
    {
        MCP2301720,
        MCP2301721,
        MCP2301722,
        MCP2301723,
        MCP2301724,
        MCP2301725,
        MCP2301726,
        MCP2301727,
    }
    public enum RGBColor
    {
        Red,
        Green,
        Blue,
        Off
    }
}
