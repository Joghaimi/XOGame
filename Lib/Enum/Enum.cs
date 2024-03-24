using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public enum SoundType
    {
        Ticking = 1,
        Button,
        Done,
        Bonus,
        Finish,
        Start,
        Start2,
        LaserScan,
        LevelWin,
        MarioJump,
        MissionCompleted,
        Descend2,
        Descend,
        GameOver,
        GameScoreTally,
        WinSquare,
        Winner2,
        Winner1,
        Background,
        ScanId,
        instruction
    }
    public enum Room {
        Fort,
        Dark,
        Diving,
        FloorIsLava,
        Gathering,
        Shooting,
    }
    public enum Displays
    {
        first = 1,
        second,
        third,
        fourth,
        fifth,
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
        White,
        Turquoise,
        Cyan,
        purple,
        Yellow,
        Magenta,
        Off
    }
    public enum MotorStatus
    {
        Stop = 0,
        Run = 2,
        Reverse = 4,
    }
    public enum MotorSpeed
    {
        Stop = 0,
        Slow = 1000,
        Medium = 1500,
        High = 2000,
    }
    public enum ModbusAddress
    {
        Speed = 4,
        startStop = 5
    }
    public enum ModbusSlave
    {
        Slave1 = 1,
        Slave2 = 2,
        Slave3 = 3,
        Slave4 = 4,
    }
    public enum Target
    {
        Target1 = 1,
        Target2 = 2,
        Target3 = 3,
        Target4 = 4,
        Target5 = 5,
    }

}
